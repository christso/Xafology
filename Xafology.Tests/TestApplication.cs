using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.TestUtils
{
    public class TestApplication : XafApplication
    {
        private Window mainWindow;

        public TestApplication()
        {

        }

        //protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        //{
        //    args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection);
        //}

        //public override Window MainWindow
        //{
        //    get
        //    {
        //        return mainWindow;
        //    }
        //}

        //public void CreateMainWindow()
        //{
        //    mainWindow = base.CreateWindow(TemplateContext.ApplicationWindow, null, true, true);
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
}
