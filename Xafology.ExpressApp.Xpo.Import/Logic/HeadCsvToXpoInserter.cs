
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
using Xafology.ExpressApp.Xpo;
using Xafology.ExpressApp.Xpo.Import.Parameters;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public class HeadCsvToXpoInserter : ICsvToXpoLoader, IDisposable
    {
        private readonly CsvReader csvReader;
        private readonly Xafology.ExpressApp.Xpo.ValueMap.IXpoFieldMapper xpoFieldMapper;
        private readonly ITypeInfo objTypeInfo;
        private readonly ImportHeadersParam param;
        private readonly Xafology.ExpressApp.Xpo.ValueMap.IImportLogger logger;
        private readonly HeadCsvToXpoRecordMapper recordMapper;

        public HeadCsvToXpoInserter(ImportHeadersParam param, Stream stream,
            Xafology.ExpressApp.Xpo.ValueMap.IXpoFieldMapper xpoFieldMapper, Xafology.ExpressApp.Xpo.ValueMap.IImportLogger logger)
        {

            if (logger == null)
                this.logger = new Xafology.ExpressApp.Xpo.ValueMap.NullImportLogger();
            else
                this.logger = logger;

            if (param == null)
                throw new UserFriendlyException("Param cannot be null");
            if (stream == null)
                throw new UserFriendlyException("Stream cannot be null");
            if (xpoFieldMapper == null)
                throw new UserFriendlyException("XpoFieldMapper cannot be null");

            csvReader = new CsvReader(new StreamReader(stream), true);
            objTypeInfo = param.ObjectTypeInfo;
            this.param = param;
            this.xpoFieldMapper = xpoFieldMapper;
            
            recordMapper = new HeadCsvToXpoRecordMapper(xpoFieldMapper, param.HeaderToFieldMaps, csvReader);
            FieldMapsUtil.ValidateParameters(param);
        }

        public HeadCsvToXpoInserter(ImportHeadersParam param, Stream stream,
            Xafology.ExpressApp.Xpo.ValueMap.IXpoFieldMapper xpoFieldMapper)
            : this(param, stream, xpoFieldMapper, null)
        {
            
        }

        public void Execute()
        {
            int counter = 0;
            while (csvReader.ReadNextRecord())
            {
                var targetObject = GetTargetObject();
                recordMapper.SetMemberValues(targetObject);
                counter++;
            }
            param.Session.CommitTransaction();

            logger.Log("{0} records inserted.", counter);
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
