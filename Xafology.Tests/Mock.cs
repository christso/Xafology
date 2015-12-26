using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.TestUtils
{
    public class TestApplication : XafApplication
    {
        public TestApplication()
        {

        }

        //protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        //{
        //    args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection);
        //}

        protected override void OnDatabaseVersionMismatch(DatabaseVersionMismatchEventArgs e)
        {
            base.OnDatabaseVersionMismatch(e);
            e.Updater.Update();
            e.Handled = true;
        }
        protected override LayoutManager CreateLayoutManagerCore(bool simple)
        {
            return null;
        }

    }

    public class TestListEditor : ListEditor
    {
        public TestListEditor(IModelListView model)
            : base(model) { }
        protected override void AssignDataSourceToControl(object dataSource)
        {
        }
        public override DevExpress.ExpressApp.Templates.IContextMenuTemplate ContextMenuTemplate
        {
            get { throw new NotImplementedException(); }
        }
        protected override object CreateControlsCore()
        {
            return null;
        }
        public override System.Collections.IList GetSelectedObjects()
        {
            return new List<object>();
        }
        public override void Refresh()
        {
        }
        public override SelectionType SelectionType
        {
            get { return SelectionType.MultipleSelection; ; }
        }
    }
}
