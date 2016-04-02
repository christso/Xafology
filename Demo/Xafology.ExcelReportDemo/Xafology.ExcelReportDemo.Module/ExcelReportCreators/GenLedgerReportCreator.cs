using Xafology.ExpressApp.MsoExcel.Attributes;
using Xafology.ExpressApp.MsoExcel.Reports;
using DevExpress.ExpressApp;
using Xafology.Spreadsheet;

namespace Xafology.ExcelReportDemo.Module.ExcelReportCreators
{
    [ReportName("Gen Ledger Report")]
    public class GenLedgerReportCreator : ExcelReportCreator
    {
        public GenLedgerReportCreator()
        {
            FileName = "Gen Ledger Report.xlsx";
        }
        public GenLedgerReportCreator(XafApplication app, IWorkbook package)
            : base(app, package)
        {

        }

        public override void Execute()
        {

        }

        public override System.IO.Stream CreateReportTemplateStream()
        {
            return GetType().Assembly.GetManifestResourceStream(
                   "Xafology.ExcelReportDemo.Module.EmbeddedExcelTemplates.Gen Ledger Report.xlsx");
        }
    }
}
