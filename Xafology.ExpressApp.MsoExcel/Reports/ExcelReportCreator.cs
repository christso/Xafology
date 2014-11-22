using DevExpress.ExpressApp;
using OfficeOpenXml;
using System.IO;

namespace Xafology.ExpressApp.MsoExcel.Reports
{
    /// <summary>
    /// Please increase the database version if you add a new ExcelReportCreator or change its ReportName.
    /// </summary>
    public abstract class ExcelReportCreator
    {
        public string FileName;

        public ExcelReportCreator()
        {
            FileName = "Template.xlsx";
        }

        public ExcelReportCreator(XafApplication app, ExcelPackage package)
            : this()
        {
            Setup(app, package);
        }

        public void Setup(XafApplication app, ExcelPackage package)
        {
            this.Application = app;
            this.Package = package;
        }

        public virtual Stream CreateReportTemplateStream()
        {
            return null;
        }

        public abstract void Execute();

        protected string TargetName;
        protected XafApplication Application;
        protected ExcelPackage Package;
    }
}
