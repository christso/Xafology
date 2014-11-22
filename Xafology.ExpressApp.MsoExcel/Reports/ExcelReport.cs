using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Xafology.ExpressApp.MsoExcel.Reports
{
    [FileAttachment("TemplateFile")]
    public class ExcelReport : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).
        public ExcelReport(Session session)
            : base(session)
        {

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (http://documentation.devexpress.com/#Xaf/CustomDocument2834).
        }
        // Fields...
        private FileData _TemplateFile;
        private string _ReportName;

        public string ReportName
        {
            get
            {
                return _ReportName;
            }
            set
            {
                SetPropertyValue("ReportName", ref _ReportName, value);
            }
        }

        public FileData TemplateFile
        {
            get
            {
                return _TemplateFile;
            }
            set
            {
                SetPropertyValue("TemplateFile", ref _TemplateFile, value);
            }
        }
    }
}
