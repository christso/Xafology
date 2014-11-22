namespace Xafology.ConcurrentRequestDemo.Win
{
    partial class ConcurrentRequestDemoWindowsFormsApplication
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
            this.module3 = new Xafology.ConcurrentRequestDemo.Module.ConcurrentRequestDemoModule();
            this.module4 = new Xafology.ConcurrentRequestDemo.Module.Win.ConcurrentRequestDemoWindowsFormsModule();
            this.businessClassLibraryCustomizationModule1 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.generateUserFriendlyIdModule1 = new GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule();
            this.XafologySystemModule1 = new Xafology.ExpressApp.SystemModule.XafologySystemModule();
            this.concurrencyModule1 = new Xafology.ExpressApp.Concurrency.ConcurrencyModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ConcurrentRequestDemoWindowsFormsApplication
            // 
            this.ApplicationName = "Xafology.ConcurrentRequestDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.businessClassLibraryCustomizationModule1);
            this.Modules.Add(this.generateUserFriendlyIdModule1);
            this.Modules.Add(this.XafologySystemModule1);
            this.Modules.Add(this.concurrencyModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ConcurrentRequestDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.ConcurrentRequestDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private Xafology.ConcurrentRequestDemo.Module.ConcurrentRequestDemoModule module3;
        private Xafology.ConcurrentRequestDemo.Module.Win.ConcurrentRequestDemoWindowsFormsModule module4;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule businessClassLibraryCustomizationModule1;
        private GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule generateUserFriendlyIdModule1;
        private Xafology.ExpressApp.SystemModule.XafologySystemModule XafologySystemModule1;
        private Xafology.ExpressApp.Concurrency.ConcurrencyModule concurrencyModule1;
    }
}
