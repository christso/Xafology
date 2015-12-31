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
using Xafology.ExpressApp.Xpo.Import.Logic.New;
using System.Diagnostics.Contracts;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class HeadCsvToXpoUpdater : ICsvToXpoLoaderV2, IDisposable
    {
      private readonly CsvReader csvReader;
        private readonly XpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportHeadersParam param;
        private readonly IImportLogger logger;
        private readonly HeadCsvToXpoRecordMapper recordMapper;

        public HeadCsvToXpoUpdater(ImportHeadersParam param, Stream stream, 
            XpoFieldMapper xpoFieldMapper, IImportLogger logger)
        {
            csvReader = GetCsvReaderFromStream(stream, true);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            this.logger = logger;
            recordMapper = new HeadCsvToXpoRecordMapper(xpoFieldMapper, param.HeaderToFieldMaps, csvReader);
        }

        public void Execute()
        {
            List<IMemberInfo> targetMembers = GetTargetMembers();
            if (targetMembers.Count == 0) return;

            var keyFieldMap = GetKeyFieldMap();
            
            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject(keyFieldMap.TargetName, csvReader[0]);
                recordMapper.SetMemberValues(targetObject, objTypeInfo);
            }
            
            param.Session.CommitTransaction();
        }

        private CsvReader GetCsvReaderFromStream(Stream stream, bool hasHeaders)
        {
            return new CsvReader(new StreamReader(stream), hasHeaders);
        }

        private List<IMemberInfo> GetTargetMembers()
        {
            var fieldMapper = new XpoTargetMembers(param.HeaderToFieldMaps);
            return fieldMapper.GetList(objTypeInfo);
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
