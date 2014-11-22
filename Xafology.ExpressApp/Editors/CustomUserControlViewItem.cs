using System;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Data.Filtering;
using System.Collections;

namespace Xafology.ExpressApp.Editors
{
    /// <summary>
    /// A base custom Application Model element extension for the View Item node.
    /// </summary>
    public interface IModelCustomUserControlViewItem : IModelViewItem
    {
    }

    /// <summary>
    /// A base custom View Item that hosts a custom user control (http://documentation.devexpress.com/#Xaf/CustomDocument2612) to show it in the XAF View.
    /// </summary>
    [ViewItem(typeof(IModelCustomUserControlViewItem))]
    public abstract class CustomUserControlViewItem : ViewItem, IComplexViewItem
    {
        public CustomUserControlViewItem(IModelViewItem model, Type objectType)
            : base(objectType, model != null ? model.Id : string.Empty)
        {
        }
        private IObjectSpace theObjectSpace;
        private XafApplication theApplication;
        public IObjectSpace ObjectSpace
        {
            get { return theObjectSpace; }
        }
        public XafApplication Application
        {
            get { return theApplication; }
        }
        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            theObjectSpace = objectSpace;
            theApplication = application;
        }
        protected override void OnControlCreated()
        {
            base.OnControlCreated();
            // Initializing a control when it is created by XAF (as part of a ViewItem).
            // If you do not need to query data via 
            XpoSessionAwareControlInitializer.Initialize(Control as IXpoSessionAwareControl, theObjectSpace);
        }
    }
    /// <summary>
    /// This interface is designed to provide acces to controls within the user control
    /// </summary>
    public interface IMasterUserControl
    {
        List<object> UserControls { get; }
    }
    /// <summary>
    /// This interface is designed to provide persistent data from the XAF application to custom user controls or forms (the interface should be implemented by them).
    /// </summary>
    public interface IXpoSessionAwareControl
    {
        void UpdateDataSource(Session session);
    }
    public static class XpoSessionAwareControlInitializer
    {
        public static void Initialize(IXpoSessionAwareControl control, IObjectSpace objectSpace)
        {
            // The IXpoSessionAwareControl interface is needed to pass a Session into a custom control that is supposed to implement this interface.
            Guard.ArgumentNotNull(control, "control");
            Guard.ArgumentNotNull(objectSpace, "objectSpace");

            // If a custom control is XAF-aware, then use the IObjectSpace to query data and bind it to your custom control (http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppBaseObjectSpacetopic). 
            // See some examples below:
            //Type persistentDataType = typeof(DevExpress.Persistent.BaseImpl.Task);
            //IList persistentData = objectSpace.GetObjects(persistentDataType, CriteriaOperator.Parse("Status = 'InProgress'"));

            // Session is required to query data when a custom control is XPO-aware only. 
            // You can pass an XafApplication into your custom control in a similar manner, if necessary.
            DevExpress.ExpressApp.Xpo.XPObjectSpace xpObjectSpace = ((DevExpress.ExpressApp.Xpo.XPObjectSpace)objectSpace);
            control.UpdateDataSource(xpObjectSpace.Session);

            // It is required to update the session when ObjectSpace is reloaded.
            objectSpace.Reloaded += delegate(object sender, EventArgs args)
            {
                control.UpdateDataSource(xpObjectSpace.Session);
            };
        }
        public static void Initialize(IXpoSessionAwareControl sessionAwareControl, XafApplication theApplication)
        {
            Initialize(sessionAwareControl, theApplication != null ? theApplication.CreateObjectSpace() : null);
        }
    }
}
