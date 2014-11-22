namespace Xafology.ExcelReportDemo.Win
{
    partial class ExcelReportDemoWindowsFormsApplication
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new Xafology.ExcelReportDemo.Module.ExcelReportDemoModule();
            this.module4 = new Xafology.ExcelReportDemo.Module.Win.ExcelReportDemoWindowsFormsModule();
            this.XafologySystemModule1 = new Xafology.ExpressApp.SystemModule.XafologySystemModule();
            this.msoExcelModule1 = new Xafology.ExpressApp.MsoExcel.MsoExcelModule();
            this.fileAttachmentsWindowsFormsModule1 = new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ExcelReportDemoWindowsFormsApplication
            // 
            this.ApplicationName = "Xafology.ExcelReportDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.XafologySystemModule1);
            this.Modules.Add(this.msoExcelModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule1);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ExcelReportDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.ExcelReportDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private Xafology.ExcelReportDemo.Module.ExcelReportDemoModule module3;
        private Xafology.ExcelReportDemo.Module.Win.ExcelReportDemoWindowsFormsModule module4;
        private ExpressApp.SystemModule.XafologySystemModule XafologySystemModule1;
        private ExpressApp.MsoExcel.MsoExcelModule msoExcelModule1;
        private DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule1;
    }
}
