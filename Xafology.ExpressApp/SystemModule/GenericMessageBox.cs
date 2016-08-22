//EXAMPLE:
//new GenericMessageBox("hello", 
//    delegate(object s1, ShowViewParameters e1) 
//    { 
//        return; 
//    },
//    delegate(object s1, EventArgs e1) 
//    { 
//        return; 
//    });

using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;

namespace Xafology.ExpressApp.SystemModule
{
    public class GenericMessageBox
    {
        public delegate void MessageBoxEventHandler(object sender, ShowViewParameters e);

        private event MessageBoxEventHandler localAccept;
        private event EventHandler localCancel;

        #region XAF Module
        public GenericMessageBox(string Message, string caption, MessageBoxEventHandler Accept)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message, caption);
            AttachDialogController(svp, app, Accept);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public GenericMessageBox(string Message, string caption)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message, caption);
            AttachDialogController(svp, app);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public GenericMessageBox(string Message, MessageBoxEventHandler Accept, EventHandler Cancel)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app, Accept, Cancel);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        public GenericMessageBox(string Message, string caption, MessageBoxEventHandler Accept, EventHandler Cancel)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message, caption);
            AttachDialogController(svp, app, Accept, Cancel);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }

        public GenericMessageBox(string Message, MessageBoxEventHandler Accept)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app, Accept);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public GenericMessageBox(XafApplication app, string Message)
        {
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public GenericMessageBox(XafApplication app, string message, string caption)
        {
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, message, caption);
            AttachDialogController(svp, app);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        public GenericMessageBox(string Message)
        {
            var app = GetAppInstance();
            var svp = new ShowViewParameters();
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app);
            app.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
        private XafApplication GetAppInstance()
        {
            var app = ApplicationHelper.Instance.Application;
            if (app == null)
                throw new UserFriendlyException("Application object cannot be null. "
                    + "Ensure that the MessageBoxModule is added to your main application module, "
                    + "or pass the Application object to the GenericMessageBox constructor");
            return app;
        }
        #endregion

        public GenericMessageBox(ShowViewParameters svp, XafApplication app, string Message, MessageBoxEventHandler Accept, EventHandler Cancel)
        {
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app, Accept, Cancel);
        }

        public GenericMessageBox(ShowViewParameters svp, XafApplication app, string Message, MessageBoxEventHandler Accept)
        {
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app, Accept);
        }

        public GenericMessageBox(ShowViewParameters svp, XafApplication app, string Message)
        {
            CreateDetailView(svp, app, Message);
            AttachDialogController(svp, app);
        }

        public GenericMessageBox(ShowViewParameters svp, XafApplication app, string message, string caption)
        {
            CreateDetailView(svp, app, message, caption);
            AttachDialogController(svp, app);
        }

        private void AttachDialogController(ShowViewParameters svp, XafApplication app, MessageBoxEventHandler Accept, EventHandler Cancel)
        {
            localAccept = Accept;
            localCancel = Cancel;
            var dc = app.CreateController<DialogController>();
            dc.AcceptAction.Execute += AcceptAction_Execute;
            dc.Cancelling += dc_Cancelling;
            svp.Controllers.Add(dc);
        }

        private void AttachDialogController(ShowViewParameters svp, XafApplication app, MessageBoxEventHandler Accept)
        {
            localAccept = Accept;
            var dc = app.CreateController<DialogController>();
            dc.AcceptAction.Execute += AcceptAction_Execute;
            dc.CancelAction.Enabled.SetItemValue("Cancel Disabled", false);
            dc.CancelAction.Active.SetItemValue("Cancel Disabled", false);
            svp.Controllers.Add(dc);
        }

        private void AttachDialogController(ShowViewParameters svp, XafApplication app)
        {
            var dc = app.CreateController<DialogController>();
            dc.AcceptAction.Execute += AcceptAction_Execute;
            dc.CancelAction.Enabled.SetItemValue("Cancel Disabled", false);
            dc.CancelAction.Active.SetItemValue("Cancel Disabled", false);
            svp.Controllers.Add(dc);
        }

        private static void CreateDetailView(ShowViewParameters svp, XafApplication app, string Message)
        {
            svp.CreatedView = app.CreateDetailView(ObjectSpaceInMemory.CreateNew(), new MessageBoxTextMessage(Message)); ;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.PopupWindow;
            svp.CreateAllControllers = true;
        }
        private static void CreateDetailView(ShowViewParameters svp, XafApplication app, string Message, string caption)
        {
            svp.CreatedView = app.CreateDetailView(ObjectSpaceInMemory.CreateNew(), new MessageBoxTextMessage(Message));
            svp.CreatedView.Caption = caption;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.PopupWindow;
            svp.CreateAllControllers = true;
        }
        void AcceptAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (localAccept != null)
                localAccept(this, e.ShowViewParameters);
        }

        void dc_Cancelling(object sender, EventArgs e)
        {
            if (localCancel != null)
                localCancel(this, e);
        }
    }
}
