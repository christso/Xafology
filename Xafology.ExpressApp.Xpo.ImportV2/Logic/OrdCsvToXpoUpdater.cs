
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

    public class OrdCsvToXpoUpdater : ICsvToXpoLoader, IDisposable
    {
        private readonly CsvReader csvReader;
        private readonly IXpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportOrdinalsParam param;
        private readonly IImportLogger logger;
        private readonly OrdCsvToXpoRecordMapper recordMapper;

        public OrdCsvToXpoUpdater(ImportOrdinalsParam param, Stream stream,
            IXpoFieldMapper xpoFieldMapper, IImportLogger logger)
        {
            csvReader = new CsvReader(new StreamReader(stream), false);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            this.logger = logger;
            recordMapper = new OrdCsvToXpoRecordMapper(xpoFieldMapper, param.OrdToFieldMaps, csvReader);
            FieldMapsUtil.ValidateParameters(param);
        }

        public void Execute()
        {
            List<IMemberInfo> targetMembers = FieldMapsUtil.GetTargetMembers(param.OrdToFieldMaps, objTypeInfo);

            if (targetMembers.Count == 0) return;

            var keyFieldMap = GetKeyFieldMap();

            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject(keyFieldMap.TargetName, csvReader[0]);
                recordMapper.SetMemberValues(targetObject);
            }

            param.Session.CommitTransaction();
        }

        private OrdinalToFieldMap GetKeyFieldMap()
        {
            var keyFields = param.OrdToFieldMaps.Where(x => x.IsKeyField == true);

            if (keyFields.Count() != 1)
                throw new UserFriendlyException("You can only define one Key Field");

            var keyField = keyFields.FirstOrDefault();

            return keyField;
        }

        private IXPObject GetTargetObject(string keyName, object keyValue)
        {
            return (IXPObject)param.Session.FindObject(objTypeInfo.Type,
                CriteriaOperator.Parse(keyName + " = ?", keyValue));
        }

        public void Dispose()
        {
            if (csvReader != null)
                csvReader.Dispose();
        }
    }
}
