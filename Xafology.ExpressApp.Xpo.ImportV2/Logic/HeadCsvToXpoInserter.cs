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
    public class HeadCsvToXpoInserter : ICsvToXpoLoader, IDisposable
    {
        private readonly CsvReader csvReader;
        private readonly IXpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportHeadersParam param;
        private readonly Xafology.ExpressApp.Xpo.Import.Logic.IImportLogger logger;
        private readonly Xafology.ExpressApp.Xpo.Import.Logic.HeadCsvToXpoRecordMapper recordMapper;

        public HeadCsvToXpoInserter(ImportHeadersParam param, Stream stream, 
            IXpoFieldMapper xpoFieldMapper, Xafology.ExpressApp.Xpo.Import.Logic.IImportLogger logger)
        {
            csvReader = new CsvReader(new StreamReader(stream), true);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            this.logger = logger;
            recordMapper = new Xafology.ExpressApp.Xpo.Import.Logic.HeadCsvToXpoRecordMapper(xpoFieldMapper, param.HeaderToFieldMaps, csvReader);
        }

        public void Execute()
        {
            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject();
                recordMapper.SetMemberValues(targetObject, objTypeInfo);
            }
            param.Session.CommitTransaction();
        }

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
