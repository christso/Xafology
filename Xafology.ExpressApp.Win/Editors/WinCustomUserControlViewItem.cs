using System;
using System.ComponentModel;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using Xafology.ExpressApp.Editors;

namespace Xafology.ExpressApp.Win.Editors
{
    /// <summary>
    /// A custom Application Model element extension for the View Item node to be able to specify custom WinForms controls via the Model Editor.
    /// </summary>
    public interface IModelWinCustomUserControlViewItem
    {
        [Category("Data")]
        string CustomControlTypeName { get; set; }
    }
    /// <summary>
    /// An custom View Item that hosts a custom WinForms user control (http://documentation.devexpress.com/#Xaf/CustomDocument2612) to show it in the XAF View.
    /// </summary>
    [ViewItem(typeof(IModelCustomUserControlViewItem))]
    public class WinCustomUserControlViewItem : CustomUserControlViewItem
    {
        protected IModelWinCustomUserControlViewItem model;
        public WinCustomUserControlViewItem(IModelViewItem model, Type objectType)
            : base(model, objectType)
        {
            this.model = model as IModelWinCustomUserControlViewItem;
            if (this.model == null)
                throw new ArgumentNullException("IModelWinCustomUserControlViewItem must extend IModelCustomUserControlViewItem in the ExtendModelInterfaces method of your Win ModuleBase descendant.");
        }
        protected override object CreateControlCore()
        {
            //You can access the View and other properties here to additionally initialize your control.
            return DevExpress.Persistent.Base.ReflectionHelper.CreateObject(model.CustomControlTypeName);
        }
    }
}
