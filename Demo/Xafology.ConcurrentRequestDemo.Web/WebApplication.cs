using System;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.Web;
using System.Collections.Generic;
//using DevExpress.ExpressApp.Security;

namespace Xafology.ConcurrentRequestDemo.Web
{
    // For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
    public partial class ConcurrentRequestDemoAspNetApplication : WebApplication
    {
        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule module2;
        private Xafology.ConcurrentRequestDemo.Module.ConcurrentRequestDemoModule module3;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule businessClassLibraryCustomizationModule1;
        private GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule generateUserFriendlyIdModule1;
        private ExpressApp.SystemModule.XafologySystemModule XafologySystemModule1;
        private ExpressApp.Concurrency.ConcurrencyModule concurrencyModule1;
        private Xafology.ConcurrentRequestDemo.Module.Web.ConcurrentRequestDemoAspNetModule module4;

        public ConcurrentRequestDemoAspNetApplication()
        {
            InitializeComponent();
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new XPObjectSpaceProvider(args.ConnectionString, args.Connection, true);
        }

        private void ConcurrentRequestDemoAspNetApplication_DatabaseVersionMismatch(object sender, DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (System.Diagnostics.Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application.\r\n" +
                    "This error occurred  because the automatic database update was disabled when the application was started without debugging.\r\n" +
                    "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " +
                    "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " +
                    "or manually create a database using the 'DBUpdater' tool.\r\n" +
                    "Anyway, refer to the following help topics for more detailed information:\r\n" +
                    "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm\r\n" +
                    "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm\r\n" +
                    "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
                {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new Xafology.ConcurrentRequestDemo.Module.ConcurrentRequestDemoModule();
            this.module4 = new Xafology.ConcurrentRequestDemo.Module.Web.ConcurrentRequestDemoAspNetModule();
            this.businessClassLibraryCustomizationModule1 = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.generateUserFriendlyIdModule1 = new GenerateUserFriendlyId.Module.GenerateUserFriendlyIdModule();
            this.XafologySystemModule1 = new Xafology.ExpressApp.SystemModule.XafologySystemModule();
            this.concurrencyModule1 = new Xafology.ExpressApp.Concurrency.ConcurrencyModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ConcurrentRequestDemoAspNetApplication
            // 
            this.ApplicationName = "Xafology.ConcurrentRequestDemo";
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.businessClassLibraryCustomizationModule1);
            this.Modules.Add(this.generateUserFriendlyIdModule1);
            this.Modules.Add(this.XafologySystemModule1);
            this.Modules.Add(this.concurrencyModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.module4);
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.ConcurrentRequestDemoAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }
}
