using Xafology.ExpressApp.MsoExcel.Reports;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using System;
using System.Collections.Generic;
using System.IO;
using Xafology.Spreadsheet;

namespace Xafology.ExpressApp.MsoExcel.Controllers
{
    public class ExcelReportViewController : ViewController
    {
        private readonly Dictionary<string, Type> _ReportCreatorTypes;

        public ExcelReportViewController()
        {
            TargetObjectType = typeof(ExcelReport);
            var createXlReportAction = new SimpleAction(this, "CreateXlReportAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            createXlReportAction.Caption = "Create Report";
            createXlReportAction.Execute += createXlReportAction_Execute;

            var initAction = new SimpleAction(this, "InitReportsAction", DevExpress.Persistent.Base.PredefinedCategory.Edit);
            initAction.Caption = "Init";
            initAction.Execute += initAction_Execute;

            // Report Creator Names
            _ReportCreatorTypes = new Dictionary<string, Type>();
        }

        void initAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Setup.CreateReportObjects(((XPObjectSpace)ObjectSpace).Session);
            ObjectSpace.Refresh();
        }

        public Dictionary<string, Type> ReportCreatorTypes
        {
            get
            {
                return _ReportCreatorTypes;
            }
        }

        private void createXlReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CreateXlReport();
        }

        public void CreateXlReport()
        {
            var reportObj = View.CurrentObject as ExcelReport;

            // Update Excel Report for selected Report object
            ExcelReportCreator reportCreator = Setup.ReportCreatorInstance(reportObj.ReportName);
            if (reportCreator == null)
                throw new UserFriendlyException(string.Format(
                    "No Report Creator Type defined for Report Object '{0}'",
                    reportObj.ReportName));

            Stream stream = null;

            try
            {
                if (reportObj.TemplateFile == null || reportObj.TemplateFile.IsEmpty)
                {
                    // create report
                    reportObj.TemplateFile = new FileData(reportObj.Session);
                    stream = reportCreator.CreateReportTemplateStream();
                    if (stream == null)
                        throw new UserFriendlyException("Report Template does not exist.");
                    reportObj.TemplateFile.LoadFromStream(reportCreator.FileName, stream);
                }
                else
                {
                    // get report
                    stream = new MemoryStream();
                    reportObj.TemplateFile.SaveToStream(stream);
                }
                stream.Position = 0;
                
                using (Xafology.SpreadsheetImpl.WorkbookProxy package = new Xafology.SpreadsheetImpl.WorkbookProxy(stream))
                {
                    reportCreator.Setup(Application, package);
                    reportCreator.Execute();

                    package.Save();

                    package.Stream.Position = 0;

                    reportObj.TemplateFile.LoadFromStream(reportObj.TemplateFile.FileName, package.Stream);
                    reportObj.Save();
                    reportObj.Session.CommitTransaction();
                }
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
        }
    }
}
