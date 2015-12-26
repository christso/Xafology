using Xafology.ExpressApp.Concurrency;
using Xafology.Utils.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Xafology.ExpressApp.Xpo.Import.Parameters;
using Xafology.Utils;
namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class ImportCsvFileHeadersLogic : ImportCsvFileLogic
    {
        private readonly ImportCsvFileHeadersParam headersParam;
        private const bool hasHeaders = true;
        private readonly ILogger logger;
        private readonly RequestManager requestMgr;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application"></param>
        /// <param name="param"></param>
        /// <param name="stream">Stream containing the CSV data</param>
        public ImportCsvFileHeadersLogic(XafApplication application, ImportCsvFileHeadersParam param, Stream stream)
            : base(application, param)
        {
            csvReader = new CsvReader(new StreamReader(stream), hasHeaders);
            headersParam = param;
            this.requestMgr = new RequestManager(Application);
            this.logger = new ActionRequestLogger(requestMgr);
        }

        public ImportCsvFileHeadersParam Param
        {
            get
            {
                return headersParam;
            }
        }

        protected void Setup()
        {
            this.Options.CreateMembers = headersParam.CreateMembers;
            this.Options.CacheObjects = headersParam.CacheLookupObjects;
        }

        public override void Import()
        {
            Setup();

            CancellationTokenSource = requestMgr.CancellationTokenSource;

            // import
            if (Param.FieldHeadImportMaps.Count == 0)
                CreateFieldImportMaps();

            Action job = new Action(() =>
            {
                try
                {
                    // setup
                    OnBeforeImport();

                    if (Param.ImportActionType == ImportActionType.Insert)
                        this.Insert();
                    else if (Param.ImportActionType == ImportActionType.Update)
                        this.Update();

                    // log for creating new members
                    if (this.XpObjectsNotFound.Count != 0)
                    {
                        string messageFormat;
                        if (Param.CreateMembers)
                            messageFormat = "Members created: {0} = {1}";
                        else
                            messageFormat = "Members not found: {0} = {1}";
                        foreach (var pair in this.XpObjectsNotFound)
                        {
                            logger.Log(messageFormat, pair.Key, 
                                StringUtils.QuoteJoinString(",", "'", pair.Value));
                            logger.Log("");
                        }
                    }
                }
                catch (ConvertException ex)
                {
                    requestMgr.CustomRequestExitStatus = RequestStatus.Error;

                    if (this.ErrorInfo != null)
                    {
                        logger.Log(
                            "Line: {0}. Field: {1}. Error converting '{2}' to type: '{3}'. {4}\r\n{5}",
                            this.ErrorInfo.LineNumber,
                            this.ErrorInfo.ColumnName,
                            this.ErrorInfo.OrigValue,
                            this.ErrorInfo.ColumnType,
                            this.ErrorInfo.ExceptionInfo.Message,
                            ex.StackTrace);
                        logger.Log("");
                    }
                }
                catch (UserFriendlyException)
                {
                    if (this.ErrorInfo == null) throw;
                    requestMgr.CustomRequestExitStatus = RequestStatus.Error;
                    logger.Log("Line: {0}. {1}",
                        this.ErrorInfo.LineNumber,
                        this.ErrorInfo.ExceptionInfo.Message);
                    logger.Log("");
                }
                finally
                {
                    OnAfterImport();
                }
            });
            requestMgr.ProcessRequest("Import CSV File", job);
        }

        public override void Insert()
        {
            importEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            var csv = csvReader;

            string[] headers = csv.GetFieldHeaders();
            ValidateSourceNames(headers, headersParam.FieldHeadImportMaps);

            List<IMemberInfo> targetMembers = GetTargetMembersForInsert(_objTypeInfo); // Insert sepecific
            if (targetMembers.Count == 0) return;

            CacheTargetMembers();

            while (csv.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                IXPObject targetObject = (IXPObject)Activator.CreateInstance(_objTypeInfo.Type, paramBase.Session); // Insert sepecific

                SetMemberValuesFromCsv(targetObject, targetMembers, csv, headersParam.FieldHeadImportMaps, hasHeaders);
            }
            paramBase.Session.CommitTransaction();
        }

        /// <summary>
        /// Updates the persistent object's member values to equal the CSV data based on matching the 1st member.
        /// </summary>
        /// <param name="stream">Stream containing the CSV data</param>
        /// <param name="objTypeInfo">Type Info for the object to import data to</param>
        /// <param name="hasHeaders">whether the first line in the CSV contains headers</param>
        public override void Update()
        {
            importEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            var csv = csvReader;
            string[] headers = csv.GetFieldHeaders();

            List<IMemberInfo> targetMembers = GetTargetMembersForUpdate(_objTypeInfo); // update specific
            if (targetMembers.Count == 0) return;

            if (paramBase.CacheLookupObjects)
            {
                importEngine.CacheXpObjectTypes(_objTypeInfo, targetMembers, paramBase.Session);
            }

            while (csv.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                var map = headersParam.FieldHeadImportMaps.FirstOrDefault(x => x.SourceName == headers[0]);
                string keyField;
                if (map != null)
                    keyField = map.TargetName;
                else
                    throw new UserFriendlyException(string.Format("First column of CSV ({0}) does not match Field Map ({1})",
                        headers[0], map.SourceName));

                IXPObject targetObject = (IXPObject)paramBase.Session.FindObject(_objTypeInfo.Type, CriteriaOperator.Parse(keyField + " = ?", csv[0]));
                if (targetObject == null)
                {
                    var ex = new UserFriendlyException(string.Format("No object matches criteria {0} = {1}. Try removing the object from the import file.",
                        keyField, csv[0]));
                    ErrorInfo = new ImportErrorInfo()
                    {
                        LineNumber = csv.CurrentRecordIndex + 1 + Convert.ToInt32(hasHeaders),
                        ExceptionInfo = ex
                    };
                    throw ex;
                }
                SetMemberValuesFromCsv(targetObject, targetMembers, csv, headersParam.FieldHeadImportMaps, hasHeaders);
            }
            paramBase.Session.CommitTransaction();
        }

        public List<IMemberInfo> GetTargetMembersForInsert(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = headersParam.FieldHeadImportMaps.Count(x => x.TargetName == (member.Name));
                if (targetCount > 1)
                    throw new UserFriendlyException("Duplicate maps were found for member '" + member.Name + "'");
                else if (targetCount == 0)
                    continue;
                else if (member.IsKey)
                    continue;
                // the below executes given condition: targetCount == 1 && !member.IsKey
                targetMembers.Add(member);
            }
            ValidateTargetMembers(targetMembers);
            return targetMembers;
        }

        /// <summary>
        /// validate that all target member names in the specified map do exist in XPO
        /// </summary>
        private void ValidateTargetMembers(List<IMemberInfo> targetMembers)
        {
            foreach (CsvFieldImportMap map in headersParam.FieldImportMaps)
            {
                if (targetMembers.FirstOrDefault(x => x.Name == map.TargetName) == null)
                    throw new UserFriendlyException(string.Format("Member '{0}' is not a valid member name", map.TargetName));
            }
        }

        public List<IMemberInfo> GetTargetMembersForUpdate(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = headersParam.FieldHeadImportMaps.Count(x => x.TargetName == (member.Name));
                if (targetCount > 1)
                    throw new UserFriendlyException("Duplicate maps were found for member '" + member.Name + "'");
                else if (targetCount == 0)
                    continue;
                else if (member.IsKey)
                    continue;
                // the below executes given condition: targetCount == 1 && !member.IsKey
                targetMembers.Add(member);
            }
            ValidateTargetMembers(targetMembers);
            return targetMembers;
        }

        private void ValidateSourceNames(string[] headers, IEnumerable<CsvFieldHeadersImportMap> maps)
        {
            foreach (string header in headers)
            {
                if (maps.FirstOrDefault(x => x.SourceName == header) == null)
                    throw new UserFriendlyException(
                        "Column name '" + header + "' in the CSV file does not exist as a source column in the import map.");
            }
        }

        /// <summary>
        /// Set Member Values of the XPO object to the imported values of the CSV Reader.
        /// </summary>
        /// <param name="targetObject">The XPO object to set values for</param>
        /// <param name="targetMembers">The members in the XPO object to set values for</param>
        /// <param name="csv">CSV Reader containing the data to be imported</param>
        /// <param name="hasHeaders">whether the first line contain headings. 
        /// This will affect the reported line number in error messages.</param>
        private void SetMemberValuesFromCsv(IXPObject targetObject, List<IMemberInfo> targetMembers, CsvReader csv,
            IEnumerable<CsvFieldHeadersImportMap> fieldImportMaps, bool hasHeaders)
        {
            foreach (var targetMember in targetMembers)
            {
                var map = fieldImportMaps
                    .FirstOrDefault(x => x.TargetName == targetMember.Name);
                try
                {
                    importEngine.SetMemberValue(targetObject, targetMember,
                        csv[map.SourceName], map.CreateMember);
                }
                catch (Exception ex)
                {
                    ErrorInfo = new ImportErrorInfo()
                    {
                        ColumnName = targetMember.Name,
                        ColumnType = targetMember.MemberType,
                        LineNumber = csv.CurrentRecordIndex + 1 + Convert.ToInt32(hasHeaders),
                        ExceptionInfo = ex,
                        OrigValue = csv[map.SourceName]
                    };
                    throw new ConvertException(ErrorInfo.OrigValue, ErrorInfo.ColumnType);
                }
            }
        }

        public override void CreateFieldImportMaps()
        {
            string[] headers = csvReader.GetFieldHeaders();
            foreach (var header in headers)
            {
                headersParam.FieldHeadImportMaps.Add(new CsvFieldHeadersImportMap(paramBase.Session)
                {
                    SourceName = header,
                    TargetName = header
                });
            }
            paramBase.Session.CommitTransaction();
        }

    }
}
