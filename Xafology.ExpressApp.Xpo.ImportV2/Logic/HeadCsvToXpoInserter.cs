
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
    public class HeadCsvToXpoInserter : ICsvToXpoLoader, IDisposable
    {
        private readonly CsvReader csvReader;
        private readonly IXpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportHeadersParam param;
        private readonly IImportLogger logger;
        private readonly HeadCsvToXpoRecordMapper recordMapper;

        public HeadCsvToXpoInserter(ImportHeadersParam param, Stream stream,
            IXpoFieldMapper xpoFieldMapper, IImportLogger logger)
        {
            csvReader = new CsvReader(new StreamReader(stream), true);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            this.logger = logger;
            recordMapper = new HeadCsvToXpoRecordMapper(xpoFieldMapper, param.HeaderToFieldMaps, csvReader);
            FieldMapsUtil.ValidateParameters(param);
        }

        public void Execute()
        {
            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject();
                recordMapper.SetMemberValues(targetObject);
            }
            param.Session.CommitTransaction();
        }


        // TODO: dependency injection?
        private IXPObject GetTargetObject()
        {
            return (IXPObject)Activator.CreateInstance(objTypeInfo.Type, param.Session);
        }

        public void Dispose()
        {
            if (csvReader != null)
                csvReader.Dispose();
        }
    }
}
