namespace PivotGridLayoutDemo.Module.Web {
    partial class PivotGridLayoutDemoAspNetModule {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            // 
            // PivotGridLayoutDemoAspNetModule
            // 


            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.Web.PivotGridAspNetModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.PivotGridLayout.Web.PivotGridLayoutAspNetModule));
            //this.AdditionalExportedTypes.Add(typeof(PivotGridLayoutDemo.Module.BusinessObjects.DomainObject1));
            this.RequiredModuleTypes.Add(typeof(PivotGridLayoutDemo.Module.PivotGridLayoutDemoModule));
        }

        #endregion
    }
}