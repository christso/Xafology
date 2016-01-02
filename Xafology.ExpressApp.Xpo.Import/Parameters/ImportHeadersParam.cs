﻿using Xafology.ExpressApp.Concurrency;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Xpo.Import;
using Xafology.ExpressApp.Xpo.Import.Logic.New;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    public class ImportHeadersParam : ImportParamBase, ImportParam
    {
        public ImportHeadersParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileHeadersParam-CsvFieldHeadersImportMaps"), Aggregated]
        public XPCollection<HeaderToFieldMap> HeaderToFieldMaps
        {
            get
            {
                return GetCollection<HeaderToFieldMap>("HeaderToFieldMaps");
            }
        }

        public override XPBaseCollection FieldMaps
        {
            get { return HeaderToFieldMaps; }
        }

        public override CsvToXpoLoader CreateCsvToXpoLoader(XafApplication application,
            Stream stream)
        {
            var requestMgr = new AsyncRequestManager(application);
            return new HeadCsvToXpoLoader(this, stream, requestMgr);

        }
    }
}