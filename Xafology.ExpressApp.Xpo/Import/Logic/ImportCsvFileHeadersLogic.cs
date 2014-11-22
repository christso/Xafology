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

namespace Xafology.ExpressApp.Xpo.Import
{

    public class ImportCsvFileHeadersLogic : ImportCsvFileLogic
    {
        private readonly ImportCsvFileHeadersParam _HeadersParam;
        private const bool hasHeaders = true;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="application"></param>
        /// <param name="param"></param>
        /// <param name="stream">Stream containing the CSV data</param>
        public ImportCsvFileHeadersLogic(XafApplication application, ImportCsvFileHeadersParam param, Stream stream)
            : base(application, param)
        {
            _csvReader = new CsvReader(new StreamReader(stream), hasHeaders);
            _HeadersParam = param;
        }

        public override void Import()
        {
            var request = new RequestManager(Application);
            var logic = this;
            logic.CancellationTokenSource = request.CancellationTokenSource;
            logic.Options.CreateMembers = _HeadersParam.CreateMembers;
            logic.Options.CacheObjects = _HeadersParam.CacheLookupObjects;

            // import
            if (_HeadersParam.FieldHeadImportMaps.Count == 0)
                logic.CreateFieldImportMaps();

            Action job = new Action(() =>
            {
                try
                {
                    // setup
                    OnBeforeImport();

                    if (_HeadersParam.ImportActionType == ImportActionType.Insert)
                        logic.Insert();
                    else if (_HeadersParam.ImportActionType == ImportActionType.Update)
                        logic.Update();

                    // log for creating new members
                    if (logic.XpObjectsNotFound.Count != 0)
                    {
                        string messageFormat;
                        if (_HeadersParam.CreateMembers)
                            messageFormat = "Members created: {0} = {1}";
                        else
                            messageFormat = "Members not found: {0} = {1}";
                        foreach (var pair in logic.XpObjectsNotFound)
                        {
                            request.WriteLogLine(string.Format(messageFormat, pair.Key, Xafology.Utils.StringUtils.QuoteJoinString(",", "'", pair.Value)), false);
                            request.WriteLogLine("", false);
                        }
                    }
                }
                catch (ConvertException ex)
                {
                    request.CustomRequestExitStatus = RequestStatus.Error;

                    if (logic.ErrorInfo != null)
                    {
                        request.WriteLogLine(string.Format("Line: {0}. Field: {1}. Error converting '{2}' to type: '{3}'. {4}\r\n{5}",
                            logic.ErrorInfo.LineNumber,
                            logic.ErrorInfo.ColumnName,
                            logic.ErrorInfo.OrigValue,
                            logic.ErrorInfo.ColumnType,
                            logic.ErrorInfo.ExceptionInfo.Message,
                            ex.StackTrace), false);
                        request.WriteLogLine("", false);
                    }
                }
                catch (UserFriendlyException)
                {
                    if (logic.ErrorInfo == null) throw;
                    request.CustomRequestExitStatus = RequestStatus.Error;
                    request.WriteLogLine(string.Format("Line: {0}. {1}",
                        logic.ErrorInfo.LineNumber,
                        logic.ErrorInfo.ExceptionInfo.Message), false);
                    request.WriteLogLine("", false);
                }
                finally
                {
                    OnAfterImport();
                }
            });
            request.SubmitRequest("Import CSV File", job);
        }

        public override void Insert()
        {
            _ImportEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            var csv = _csvReader;

            string[] headers = csv.GetFieldHeaders();
            ValidateSourceNames(headers, _HeadersParam.FieldHeadImportMaps);

            List<IMemberInfo> targetMembers = GetTargetMembersForInsert(_objTypeInfo); // Insert sepecific
            if (targetMembers.Count == 0) return;

            CacheTargetMembers();

            while (csv.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                IXPObject targetObject = (IXPObject)Activator.CreateInstance(_objTypeInfo.Type, _Param.Session); // Insert sepecific

                SetMemberValuesFromCsv(targetObject, targetMembers, csv, _HeadersParam.FieldHeadImportMaps, hasHeaders);
            }
            _Param.Session.CommitTransaction();
        }

        /// <summary>
        /// Updates the persistent object's member values to equal the CSV data based on matching the 1st member.
        /// </summary>
        /// <param name="stream">Stream containing the CSV data</param>
        /// <param name="objTypeInfo">Type Info for the object to import data to</param>
        /// <param name="hasHeaders">whether the first line in the CSV contains headers</param>
        public override void Update()
        {
            _ImportEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            var csv = _csvReader;
            string[] headers = csv.GetFieldHeaders();

            List<IMemberInfo> targetMembers = GetTargetMembersForUpdate(_objTypeInfo); // update specific
            if (targetMembers.Count == 0) return;

            if (_Param.CacheLookupObjects)
            {
                _ImportEngine.CacheXpObjectTypes(_objTypeInfo, targetMembers, _Param.Session);
            }

            while (csv.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                var map = _HeadersParam.FieldHeadImportMaps.FirstOrDefault(x => x.SourceName == headers[0]);
                string keyField;
                if (map != null)
                    keyField = map.TargetName;
                else
                    throw new UserFriendlyException(string.Format("First column of CSV ({0}) does not match Field Map ({1})",
                        headers[0], map.SourceName));

                IXPObject targetObject = (IXPObject)_Param.Session.FindObject(_objTypeInfo.Type, CriteriaOperator.Parse(keyField + " = ?", csv[0]));
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
                SetMemberValuesFromCsv(targetObject, targetMembers, csv, _HeadersParam.FieldHeadImportMaps, hasHeaders);
            }
            _Param.Session.CommitTransaction();
        }

        public List<IMemberInfo> GetTargetMembersForInsert(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = _HeadersParam.FieldHeadImportMaps.Count(x => x.TargetName == (member.Name));
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
            foreach (CsvFieldImportMap map in _HeadersParam.FieldImportMaps)
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
                var targetCount = _HeadersParam.FieldHeadImportMaps.Count(x => x.TargetName == (member.Name));
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
                    _ImportEngine.SetMemberValue(targetObject, targetMember,
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
            string[] headers = _csvReader.GetFieldHeaders();
            foreach (var header in headers)
            {
                _HeadersParam.FieldHeadImportMaps.Add(new CsvFieldHeadersImportMap(_Param.Session)
                {
                    SourceName = header,
                    TargetName = header
                });
            }
            _Param.Session.CommitTransaction();
        }

    }
}
