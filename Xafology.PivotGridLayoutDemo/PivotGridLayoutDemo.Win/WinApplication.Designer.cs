namespace PivotGridLayoutDemo.Win
{
    partial class PivotGridLayoutDemoWindowsFormsApplication
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
            this.systemModule1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.systemWinFormsModule1 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.pivotGridLayoutDemoModule = new PivotGridLayoutDemo.Module.PivotGridLayoutDemoModule();
            this.pivotGridModule1 = new DevExpress.ExpressApp.PivotGrid.PivotGridModule();
            this.pivotGridLayoutModule = new Xafology.ExpressApp.PivotGridLayout.PivotGridLayoutModule();
            this.pivotGridWindowsFormsModule1 = new DevExpress.ExpressApp.PivotGrid.Win.PivotGridWindowsFormsModule();
            this.pivotGridLayoutWindowsFormsModule1 = new Xafology.ExpressApp.PivotGridLayout.Win.PivotGridLayoutWindowsFormsModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // securityModule1
            // 
            this.securityModule1.UserType = typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemUser);
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.RoleType = typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole);
            this.securityStrategyComplex1.UserType = typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemUser);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // PivotGridLayoutDemoWindowsFormsApplication
            // 
            this.ApplicationName = "PivotGridLayoutDemo";
            this.Modules.Add(this.systemModule1);
            this.Modules.Add(this.systemWinFormsModule1);
            this.Modules.Add(this.pivotGridModule1);
            this.Modules.Add(this.securityModule1);
            this.Modules.Add(this.pivotGridLayoutModule);
            this.Modules.Add(this.pivotGridLayoutDemoModule);
            this.Modules.Add(this.pivotGridWindowsFormsModule1);
            this.Modules.Add(this.pivotGridLayoutWindowsFormsModule1);
            this.Security = this.securityStrategyComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.PivotGridLayoutDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.PivotGridLayoutDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule systemModule1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule systemWinFormsModule1;
        private PivotGridLayoutDemo.Module.PivotGridLayoutDemoModule pivotGridLayoutDemoModule;
        private DevExpress.ExpressApp.PivotGrid.PivotGridModule pivotGridModule1;
        private Xafology.ExpressApp.PivotGridLayout.PivotGridLayoutModule pivotGridLayoutModule;
        private DevExpress.ExpressApp.PivotGrid.Win.PivotGridWindowsFormsModule pivotGridWindowsFormsModule1;
        private Xafology.ExpressApp.PivotGridLayout.Win.PivotGridLayoutWindowsFormsModule pivotGridLayoutWindowsFormsModule1;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityStrategyComplex1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
    }
}
