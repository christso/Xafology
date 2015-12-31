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
using Xafology.ExpressApp.Xpo.Import.Parameters;
namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class OrdCsvToXpoLoader : Xafology.ExpressApp.Xpo.Import.Logic.CsvToXpoLoader
    {
        private readonly ImportOrdinalsParam ordParam;
        private readonly FieldMapListCreator fieldMapListCreator;
        private readonly AsyncRequestManager request;

        public OrdCsvToXpoLoader(ImportOrdinalsParam param, Stream stream, AsyncRequestManager request)
            : base(param)
        {
            csvReader = new CsvReader(new StreamReader(stream), param.HasHeaders);
            ordParam = param;
            fieldMapListCreator = new FieldMapListCreator(csvReader);
            this.request = request;
        }

        public override void Execute()
        {
            var logic = this;
            logic.CancellationTokenSource = request.CancellationTokenSource;

            // import
            if (ordParam.OrdToFieldMaps.Count == 0)
                CreateFieldImportMaps();

            Action job = new Action(() =>
            {
                try
                {
                    // setup
                    OnBeforeImport();

                    if (ordParam.ImportActionType == ImportActionType.Insert)
                        logic.Insert();
                    else if (ordParam.ImportActionType == ImportActionType.Update)
                        logic.Update();

                    // log for creating new members
                    if (logic.XpObjectsNotFound.Count != 0)
                    {
                        string messageFormat;
                        if (ordParam.CreateMembers)
                            messageFormat = "Members created: {0} = {1}";
                        else
                            messageFormat = "Members not found: {0} = {1}";
                        foreach (var pair in logic.XpObjectsNotFound)
                        {
                            request.Log(string.Format(messageFormat, pair.Key, Xafology.Utils.StringUtils.QuoteJoinString(",", "'", pair.Value)));
                            request.Log("");
                        }
                    }
                }
                catch (ConvertException)
                {
                    request.CustomRequestExitStatus = RequestStatus.Error;

                    if (logic.ErrorInfo != null)
                    {
                        request.Log(string.Format("Line: {0}. Field: {1}. Error converting '{2}' to type: '{3}'. {4}",
                            logic.ErrorInfo.LineNumber,
                            logic.ErrorInfo.ColumnName,
                            logic.ErrorInfo.OrigValue,
                            logic.ErrorInfo.ColumnType,
                            logic.ErrorInfo.ExceptionInfo.Message));
                        request.Log("");
                    }
                }
                catch (UserFriendlyException)
                {
                    if (logic.ErrorInfo == null) throw;
                    request.CustomRequestExitStatus = RequestStatus.Error;
                    request.Log(string.Format("Line: {0}. {1}",
                        logic.ErrorInfo.LineNumber,
                        logic.ErrorInfo.ExceptionInfo.Message));
                    request.Log("");
                }
                finally
                {
                    OnAfterImport();
                }
            });
            request.ProcessRequest("Import CSV File", job);
        }

        public override void CreateFieldImportMaps()
        {
            fieldMapListCreator.AddFieldMaps(ordParam.OrdToFieldMaps, param.Session);
        }

        public override void Insert()
        {
            xpoMapper.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            List<IMemberInfo> targetMembers = GetTargetMembers(_objTypeInfo); // Insert sepecific
            if (targetMembers.Count == 0) return;

            if (param.CacheLookupObjects)
            {
                xpoMapper.CacheXpObjectTypes(_objTypeInfo, targetMembers, param.Session);
            }

            while (csvReader.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                // skip header row
                if (csvReader.HasHeaders && csvReader.CurrentRecordIndex == 0) continue;

                IXPObject targetObject = (IXPObject)Activator.CreateInstance(_objTypeInfo.Type, param.Session); // Insert sepecific

                SetMemberValuesFromCsv(targetObject, targetMembers, csvReader,
                    ordParam.OrdToFieldMaps, csvReader.HasHeaders);
            }
            param.Session.CommitTransaction();
        }

        public override void Update()
        {
            int keyOrd = 0;
            xpoMapper.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            List<IMemberInfo> targetMembers = GetTargetMembersForUpdate(_objTypeInfo); // update specific
            if (targetMembers.Count == 0) return;

            if (param.CacheLookupObjects)
            {
                xpoMapper.CacheXpObjectTypes(_objTypeInfo, targetMembers, param.Session);
            }

            while (csvReader.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                // skip header row
                if (csvReader.HasHeaders && csvReader.CurrentRecordIndex == 0) continue;

                var map = ordParam.OrdToFieldMaps.First(x => x.SourceOrdinal == keyOrd);
                string keyField;
                if (map != null)
                    keyField = map.TargetName;
                else
                    throw new UserFriendlyException("There is no Field Map with zero Source Ordinal.");

                IXPObject targetObject = (IXPObject)param.Session.FindObject(_objTypeInfo.Type, CriteriaOperator.Parse(keyField + " = ?", csvReader[0]));
                if (targetObject == null)
                {
                    var ex = new UserFriendlyException(string.Format("No object matches criteria {0} = {1}. Try removing the object from the import file.",
                        keyField, csvReader[0]));
                    ErrorInfo = new ImportErrorInfo()
                    {
                        LineNumber = csvReader.CurrentRecordIndex + 1 + Convert.ToInt32(csvReader.HasHeaders),
                        ExceptionInfo = ex
                    };
                    throw ex;
                }
                SetMemberValuesFromCsv(targetObject, targetMembers, csvReader,
                    ordParam.OrdToFieldMaps, csvReader.HasHeaders);
            }
            param.Session.CommitTransaction();
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
            IEnumerable<OrdinalToFieldMap> FieldMaps, bool hasHeaders)
        {
            foreach (var targetMember in targetMembers)
            {
                var map = FieldMaps
                    .FirstOrDefault(x => x.TargetName == targetMember.Name);
                try
                {
                    xpoMapper.SetMemberValue(targetObject, targetMember,
                        csv[map.SourceOrdinal], map.CreateMember);
                }
                catch (Exception ex)
                {
                    ErrorInfo = new ImportErrorInfo()
                    {
                        ColumnName = targetMember.Name,
                        ColumnType = targetMember.MemberType,
                        LineNumber = csv.CurrentRecordIndex + 1 + Convert.ToInt32(hasHeaders),
                        ExceptionInfo = ex,
                        OrigValue = csv[map.SourceOrdinal]
                    };
                    throw new ConvertException(ErrorInfo.OrigValue, ErrorInfo.ColumnType);
                }
            }
        }

        private void ValidateSourceOrdinals(string[] headers, IEnumerable<OrdinalToFieldMap> maps)
        {
            foreach (var map in maps)
            {
                if (map.SourceOrdinal >= csvReader.FieldCount)
                    throw new UserFriendlyException("Source Ordinal in Field Map cannot exceed " + map.SourceOrdinal);
            }
        }

        public List<IMemberInfo> GetTargetMembers(ITypeInfo objTypeInfo)
        {
            if (param.ImportActionType == ImportActionType.Insert)
                return this.GetTargetMembersForInsert(objTypeInfo);
            else if (param.ImportActionType == ImportActionType.Update)
                return this.GetTargetMembers(objTypeInfo);
            else
                throw new ArgumentException("Unrecognised ImportActionType");
        }


        public List<IMemberInfo> GetTargetMembersForInsert(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = ordParam.OrdToFieldMaps.Count(x => ((FieldMap)x).TargetName == (member.Name));
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

        public List<IMemberInfo> GetTargetMembersForUpdate(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = ordParam.OrdToFieldMaps.Count(x => x.TargetName == (member.Name));
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
            foreach (FieldMap map in ordParam.FieldMaps)
            {
                if (targetMembers.FirstOrDefault(x => x.Name == map.TargetName) == null)
                    throw new UserFriendlyException(string.Format("Member '{0}' is not a valid member name", map.TargetName));
            }
        }

    }
}
