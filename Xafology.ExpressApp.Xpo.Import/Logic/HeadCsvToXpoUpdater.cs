
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

using Xafology.Utils;
using System.Diagnostics.Contracts;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class HeadCsvToXpoUpdater : ICsvToXpoLoader, IDisposable
    {
      private readonly CsvReader csvReader;
        private readonly Xafology.ExpressApp.Xpo.ValueMap.XpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportHeadersParam param;
        private readonly Xafology.ExpressApp.Xpo.ValueMap.IImportLogger logger;
        private readonly HeadCsvToXpoRecordMapper recordMapper;

        public HeadCsvToXpoUpdater(ImportHeadersParam param, Stream stream,
            Xafology.ExpressApp.Xpo.ValueMap.XpoFieldMapper xpoFieldMapper, Xafology.ExpressApp.Xpo.ValueMap.IImportLogger logger)
        {
            csvReader = GetCsvReaderFromStream(stream, true);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            this.logger = logger;
            recordMapper = new HeadCsvToXpoRecordMapper(xpoFieldMapper, param.HeaderToFieldMaps, csvReader);
            FieldMapsUtil.ValidateParameters(param);
        }

        public void Execute()
        {
            List<IMemberInfo> targetMembers = FieldMapsUtil.GetTargetMembers(param.HeaderToFieldMaps, objTypeInfo);
            if (targetMembers.Count == 0) return;

            var keyFieldMap = GetKeyFieldMap();
            
            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject(keyFieldMap.TargetName, csvReader[0]);
                recordMapper.SetMemberValues(targetObject);
            }
            
            param.Session.CommitTransaction();
        }

        private CsvReader GetCsvReaderFromStream(Stream stream, bool hasHeaders)
        {
            return new CsvReader(new StreamReader(stream), hasHeaders);
        }

        private HeaderToFieldMap GetKeyFieldMap()
        {
            var keyFields = param.HeaderToFieldMaps.Where(x => x.IsKeyField == true);

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
