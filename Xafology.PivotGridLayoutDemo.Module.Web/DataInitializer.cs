using System;
using System.Linq;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using Xafology.PivotGridLayoutDemo.Module.Web.BusinessObjects;

namespace Xafology.PivotGridLayoutDemo.Module.Web
{
    public class DataInitializer
    {
        private IObjectSpace objectSpace;
        public IObjectSpace ObjectSpace
        {
            get
            {
                return objectSpace;
            }
            set
            {
                objectSpace = value;
            }
        }

        public DataInitializer(IObjectSpace objectSpace)
        {
            this.objectSpace = objectSpace;
        }

        public void Run()
        {
            string layoutName = "Bob";
            var theObject = ObjectSpace.FindObject<PivotGridLayout>(CriteriaOperator.Parse("LayoutName=?", layoutName));
            if (theObject == null)
            {
                theObject = ObjectSpace.CreateObject<PivotGridLayout>();
                theObject.LayoutName = layoutName;
                theObject.TypeName = "Type for " + layoutName;
            }
            layoutName = "Alice";
            theObject = ObjectSpace.FindObject<PivotGridLayout>(CriteriaOperator.Parse("LayoutName=?", layoutName));
            if (theObject == null)
            {
                theObject = ObjectSpace.CreateObject<PivotGridLayout>();
                theObject.LayoutName = layoutName;
                theObject.TypeName = "Type for " + layoutName;
            }
        }
    }
}
