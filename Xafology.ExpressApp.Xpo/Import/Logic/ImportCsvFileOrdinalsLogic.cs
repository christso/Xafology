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

namespace Xafology.ExpressApp.Xpo.Import
{
    public class ImportCsvFileOrdinalsLogic : ImportCsvFileLogic
    {
        private readonly ImportCsvFileOrdinalsParam _OrdinalsParam;

        public ImportCsvFileOrdinalsLogic(XafApplication application, ImportCsvFileOrdinalsParam param,
            Stream stream)
            : base(application, param)
        {
            _csvReader = new CsvReader(new StreamReader(stream), param.HasHeaders);
            _OrdinalsParam = param;
        }


        public override void Import()
        {
            var request = new RequestManager(Application);
            var logic = this;
            logic.CancellationTokenSource = request.CancellationTokenSource;
            logic.Options.CreateMembers = _OrdinalsParam.CreateMembers;
            logic.Options.CacheObjects = _OrdinalsParam.CacheLookupObjects;

            // import
            if (_OrdinalsParam.FieldOrdImportMaps.Count == 0)
                logic.CreateFieldImportMaps();

            Action job = new Action(() =>
            {
                try
                {
                    // setup
                    OnBeforeImport();

                    if (_OrdinalsParam.ImportActionType == ImportActionType.Insert)
                        logic.Insert();
                    else if (_OrdinalsParam.ImportActionType == ImportActionType.Update)
                        logic.Update();

                    // log for creating new members
                    if (logic.XpObjectsNotFound.Count != 0)
                    {
                        string messageFormat;
                        if (_OrdinalsParam.CreateMembers)
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
                catch (ConvertException)
                {
                    request.CustomRequestExitStatus = RequestStatus.Error;

                    if (logic.ErrorInfo != null)
                    {
                        request.WriteLogLine(string.Format("Line: {0}. Field: {1}. Error converting '{2}' to type: '{3}'. {4}",
                            logic.ErrorInfo.LineNumber,
                            logic.ErrorInfo.ColumnName,
                            logic.ErrorInfo.OrigValue,
                            logic.ErrorInfo.ColumnType,
                            logic.ErrorInfo.ExceptionInfo.Message), false);
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

        public override void CreateFieldImportMaps()
        {
            for (int i = 0; i < _csvReader.FieldCount; i++)
            {
                _OrdinalsParam.FieldImportMaps.BaseAdd(new CsvFieldOrdinalsImportMap(_Param.Session)
                {
                    SourceOrdinal = i,
                    TargetName = string.Format("Field_{0}", i)
                });
            }
            _Param.Session.CommitTransaction();
        }

        public override void Insert()
        {
            _ImportEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            List<IMemberInfo> targetMembers = GetTargetMembersForInsert(_objTypeInfo); // Insert sepecific
            if (targetMembers.Count == 0) return;

            if (_Param.CacheLookupObjects)
            {
                _ImportEngine.CacheXpObjectTypes(_objTypeInfo, targetMembers, _Param.Session);
            }

            while (_csvReader.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                // skip header row
                if (_csvReader.HasHeaders && _csvReader.CurrentRecordIndex == 0) continue;

                IXPObject targetObject = (IXPObject)Activator.CreateInstance(_objTypeInfo.Type, _Param.Session); // Insert sepecific

                SetMemberValuesFromCsv(targetObject, targetMembers, _csvReader,
                    _OrdinalsParam.FieldOrdImportMaps, _csvReader.HasHeaders);
            }
            _Param.Session.CommitTransaction();
        }

        public override void Update()
        {
            int keyOrd = 0;
            _ImportEngine.XpObjectsNotFound.Clear();
            ErrorInfo = null;

            List<IMemberInfo> targetMembers = GetTargetMembersForUpdate(_objTypeInfo); // update specific
            if (targetMembers.Count == 0) return;

            if (_Param.CacheLookupObjects)
            {
                _ImportEngine.CacheXpObjectTypes(_objTypeInfo, targetMembers, _Param.Session);
            }

            while (_csvReader.ReadNextRecord())
            {
                if (CancellationTokenSource != null && CancellationTokenSource.IsCancellationRequested)
                    CancellationTokenSource.Token.ThrowIfCancellationRequested();

                // skip header row
                if (_csvReader.HasHeaders && _csvReader.CurrentRecordIndex == 0) continue;

                var map = _OrdinalsParam.FieldOrdImportMaps.First(x => x.SourceOrdinal == keyOrd);
                string keyField;
                if (map != null)
                    keyField = map.TargetName;
                else
                    throw new UserFriendlyException("There is no Field Map with zero Source Ordinal.");

                IXPObject targetObject = (IXPObject)_Param.Session.FindObject(_objTypeInfo.Type, CriteriaOperator.Parse(keyField + " = ?", _csvReader[0]));
                if (targetObject == null)
                {
                    var ex = new UserFriendlyException(string.Format("No object matches criteria {0} = {1}. Try removing the object from the import file.",
                        keyField, _csvReader[0]));
                    ErrorInfo = new ImportErrorInfo()
                    {
                        LineNumber = _csvReader.CurrentRecordIndex + 1 + Convert.ToInt32(_csvReader.HasHeaders),
                        ExceptionInfo = ex
                    };
                    throw ex;
                }
                SetMemberValuesFromCsv(targetObject, targetMembers, _csvReader,
                    _OrdinalsParam.FieldOrdImportMaps, _csvReader.HasHeaders);
            }
            _Param.Session.CommitTransaction();
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
            IEnumerable<CsvFieldOrdinalsImportMap> fieldImportMaps, bool hasHeaders)
        {
            foreach (var targetMember in targetMembers)
            {
                var map = fieldImportMaps
                    .FirstOrDefault(x => x.TargetName == targetMember.Name);
                try
                {
                    _ImportEngine.SetMemberValue(targetObject, targetMember,
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

        private void ValidateSourceOrdinals(string[] headers, IEnumerable<CsvFieldOrdinalsImportMap> maps)
        {
            foreach (var map in maps)
            {
                if (map.SourceOrdinal >= _csvReader.FieldCount)
                    throw new UserFriendlyException("Source Ordinal in Field Map cannot exceed " + map.SourceOrdinal);
            }
        }

        public List<IMemberInfo> GetTargetMembersForInsert(ITypeInfo objTypeInfo)
        {
            var targetMembers = new List<IMemberInfo>();
            foreach (var member in objTypeInfo.Members)
            {
                var targetCount = _OrdinalsParam.FieldOrdImportMaps.Count(x => ((CsvFieldImportMap)x).TargetName == (member.Name));
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
                var targetCount = _OrdinalsParam.FieldOrdImportMaps.Count(x => x.TargetName == (member.Name));
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
            foreach (CsvFieldImportMap map in _OrdinalsParam.FieldImportMaps)
            {
                if (targetMembers.FirstOrDefault(x => x.Name == map.TargetName) == null)
                    throw new UserFriendlyException(string.Format("Member '{0}' is not a valid member name", map.TargetName));
            }
        }

    }
}
