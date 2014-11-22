using Xafology.ExpressApp.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.ExpressApp.PivotGrid.Editors
{
    public interface IModelPivotGridViewItem : IModelViewItem
    {

    }
    [ViewItem(typeof(IModelCustomUserControlViewItem))]
    public abstract class PivotGridViewItem : CustomUserControlViewItem
    {
        public PivotGridViewItem(IModelViewItem model, Type objectType)
            : base(model, objectType)
        {

        }
    }
}
