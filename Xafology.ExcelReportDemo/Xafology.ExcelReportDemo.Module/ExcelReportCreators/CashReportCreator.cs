using Xafology.ExcelReportDemo.Module.BusinessObjects;
using Xafology.ExpressApp.MsoExcel.Attributes;
using Xafology.ExpressApp.MsoExcel.Reports;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using Xafology.Spreadsheet;
using System.IO;

namespace Xafology.ExcelReportDemo.Module.ExcelReportCreators
{
    [ReportName("Cash Report")]
    public class CashReportCreator : ExcelReportCreator
    {
        public CashReportCreator()
        {
            FileName = "Cash Report.xlsx";
        }
        public CashReportCreator(XafApplication app, IWorkbook package)
            : base(app, package)
        {

        }

        public override void Execute()
        {
            var objSpace = (XPObjectSpace)Application.CreateObjectSpace();
            var session = objSpace.Session;

            var sortProps = new SortingCollection(null);
            var cashFlows = session.GetObjects(session.GetClassInfo(typeof(CashFlow)),
                null, sortProps, 0, false, true);

            IWorksheet ws = Package.GetWorksheet("Data");
            if (ws == null)
                throw new UserFriendlyException("Worksheet 'Data' not found in workbook.");
            
            ws.CopyObjectsToWorksheet(session, cashFlows);
            session.CommitTransaction();
        }

        public override Stream CreateReportTemplateStream()
        {
            return GetType().Assembly.GetManifestResourceStream(
                   "Xafology.ExcelReportDemo.Module.EmbeddedExcelTemplates.Cash Report.xlsx");
        }

    }
}
