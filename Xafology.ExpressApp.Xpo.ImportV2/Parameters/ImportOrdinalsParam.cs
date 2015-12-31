using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;
using Xafology.ExpressApp.Concurrency;
using Xafology.ExpressApp.Xpo.Import.Logic;
using Xafology.ExpressApp.Xpo.Import.Logic.New;
namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    public class ImportOrdinalsParam : ImportParamBase, ImportParam
    {
        private bool _HasHeaders;

        public ImportOrdinalsParam(Session session)
            : base(session)
        {
            _File = new FileData(Session);
        }

        [Association("ImportCsvFileOrdinalsParam-CsvFieldOrdinalsImportMaps"), Aggregated]
        public XPCollection<OrdinalToFieldMap> OrdToFieldMaps
        {
            get
            {
                return GetCollection<OrdinalToFieldMap>("OrdToFieldMaps");
            }
        }

        public bool HasHeaders
        {
            get
            {
                return _HasHeaders;
            }
            set
            {
                SetPropertyValue("HasHeaders", ref _HasHeaders, value);
            }
        }

        public override XPBaseCollection FieldMaps
        {
            get { return OrdToFieldMaps; }
        }

        public override CsvToXpoLoader CreateCsvToXpoLoader(XafApplication application, Stream stream)
        {
            var requestMgr = new AsyncRequestManager(application);
            return new OrdCsvToXpoLoader(this, stream, requestMgr);
        }
    }
}
