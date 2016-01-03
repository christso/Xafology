namespace Xafology.ExpressApp.Xpo.Import.Controllers
{
    partial class ImportableViewController
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
            this.components = new System.ComponentModel.Container();
            this.importAction = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // popupWindowShowAction1
            // 
            this.importAction.AcceptButtonCaption = null;
            this.importAction.CancelButtonCaption = null;
            this.importAction.Caption = "Import";
            this.importAction.Category = "Edit";
            this.importAction.ConfirmationMessage = null;
            this.importAction.Id = "contextImport";
            this.importAction.ToolTip = null;
            this.importAction.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.importAction_CustomizePopupWindowParams);
            this.importAction.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.importAction_Execute);
            // 
            // ImportableViewController
            // 
            this.Actions.Add(this.importAction);
            this.TargetObjectType = typeof(Xafology.ExpressApp.Xpo.Import.IXpoImportable);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction importAction;
    }
}
