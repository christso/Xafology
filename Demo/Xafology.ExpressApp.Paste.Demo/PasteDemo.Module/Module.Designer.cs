namespace PasteDemo.Module {
	partial class PasteDemoModule {
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
			// PasteDemoModule
			// 
			this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.Paste.PasteModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.Xpo.XpoModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.Xpo.Import.ImportModule));
            this.RequiredModuleTypes.Add(typeof(Xafology.ExpressApp.BatchDelete.BatchDeleteModule));
		}

		#endregion
	}
}
