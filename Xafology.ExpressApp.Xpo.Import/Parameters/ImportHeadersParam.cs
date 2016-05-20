
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.IO;


namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    [FileAttachment("File")]
    [DefaultListViewOptions(allowEdit: true, newItemRowPosition: NewItemRowPosition.None)]
    [ImageName("BO_List")]
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

        public override FieldMaps FieldMaps
        {
            get { return new FieldMaps(HeaderToFieldMaps); }
        }

    }
}