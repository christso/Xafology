using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;


namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    [ImageName("BO_List")]
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

        public override FieldMaps FieldMaps
        {
            get { return new FieldMaps(OrdToFieldMaps); }
        }

    }
}
