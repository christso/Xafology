namespace Xafology.SequentialBaseObjectDemo.Module {
	partial class SequentialBaseObjectDemoModule {
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
			// SequentialBaseObjectDemoModule
			// 
			this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.SystemModule.XafologySystemModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.Xpo.SequentialBase.XafologySequentialBaseModule));
            //this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.OidGenerator));
            //this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ServerPrefix));
		}

		#endregion
	}
}
