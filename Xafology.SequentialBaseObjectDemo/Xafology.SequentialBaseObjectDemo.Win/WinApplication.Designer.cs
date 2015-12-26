namespace Xafology.SequentialBaseObjectDemo.Win {
    partial class SequentialBaseObjectDemoWindowsFormsApplication {
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
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new Xafology.SequentialBaseObjectDemo.Module.SequentialBaseObjectDemoModule();
            this.module4 = new Xafology.SequentialBaseObjectDemo.Module.Win.SequentialBaseObjectDemoWindowsFormsModule();
            this.xafologySystemModule1 = new Xafology.ExpressApp.SystemModule.XafologySystemModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // SequentialBaseObjectDemoWindowsFormsApplication
            // 
            this.ApplicationName = "Xafology.SequentialBaseObjectDemo";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.LinkNewObjectToParentImmediately = false;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.xafologySystemModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.UseOldTemplates = false;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.SequentialBaseObjectDemoWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.SequentialBaseObjectDemoWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private Xafology.SequentialBaseObjectDemo.Module.SequentialBaseObjectDemoModule module3;
        private Xafology.SequentialBaseObjectDemo.Module.Win.SequentialBaseObjectDemoWindowsFormsModule module4;
        private ExpressApp.SystemModule.XafologySystemModule xafologySystemModule1;
    }
}
