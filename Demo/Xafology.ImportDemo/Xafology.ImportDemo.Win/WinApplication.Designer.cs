namespace Xafology.ImportDemo.Win
{
    partial class ImportDemoWindowsFormsApplication
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
            this.module3 = new Xafology.ImportDemo.Module.ImportDemoModule();
            this.module4 = new Xafology.ImportDemo.Module.Win.ImportDemoWindowsFormsModule();
            this.businessClassLibraryCustomizationModule1 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.xpoModule1 = new Xafology.ExpressApp.Xpo.XpoModule();
            this.XafologySystemModule1 = new Xafology.ExpressApp.SystemModule.XafologySystemModule();
            this.fileAttachmentsWindowsFormsModule1 = new DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule();
            this.xpoModule2 = new Xafology.ExpressApp.Xpo.XpoModule();
            this.conditionalAppearanceModule1 = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.importModule1 = new Xafology.ExpressApp.Xpo.Import.ImportModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ImportDemoWindowsFormsApplication
            // 
            this.ApplicationName = "Xafology.ImportDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.businessClassLibraryCustomizationModule1);
            this.Modules.Add(this.XafologySystemModule1);
            this.Modules.Add(this.xpoModule1);
            this.Modules.Add(this.conditionalAppearanceModule1);
            this.Modules.Add(this.importModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.fileAttachmentsWindowsFormsModule1);
            this.Modules.Add(this.module4);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ImportDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.ImportDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private Xafology.ImportDemo.Module.ImportDemoModule module3;
        private Xafology.ImportDemo.Module.Win.ImportDemoWindowsFormsModule module4;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule businessClassLibraryCustomizationModule1;
        private ExpressApp.Xpo.XpoModule xpoModule1;
        private ExpressApp.SystemModule.XafologySystemModule XafologySystemModule1;
        private DevExpress.ExpressApp.FileAttachments.Win.FileAttachmentsWindowsFormsModule fileAttachmentsWindowsFormsModule1;
        private ExpressApp.Xpo.XpoModule xpoModule2;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule1;
        private ExpressApp.Xpo.Import.ImportModule importModule1;
    }
}
