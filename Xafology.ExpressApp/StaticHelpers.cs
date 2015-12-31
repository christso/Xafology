using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.ExpressApp
{
    public class StaticHelpers
    {
        /// <summary>
        /// Get singleton instance. This will instantiate the object if no object exists.
        /// </summary>
        public static T GetInstance<T>(IObjectSpace objectSpace)
        {
            T result = objectSpace.FindObject<T>(null);
            if (result == null)
            {
                result = (T)Activator.CreateInstance(typeof(T), (((XPObjectSpace)objectSpace).Session));
                ((IXPObject)result).Session.Save(result);
            }
            return result;
        }

        public static SecuritySystemUser GetCurrentUser(Session session)
        {
            if (SecuritySystem.CurrentUser != null)
            {
                SecuritySystemUser currentUser = session.GetObjectByKey<SecuritySystemUser>(
                    session.GetKeyValue(SecuritySystem.CurrentUser));
                return currentUser;
            }
            return null;
        }

        public static SecuritySystemUser GetCurrentUser(IObjectSpace objSpace)
        {
            return GetCurrentUser(((XPObjectSpace)objSpace).Session);
        }


        public static void SetupDetailViewController(XafApplication application, XPObjectSpace objSpace, ViewController controller, IXPObject currentObject)
        {
            controller.Application = application;
            var view = application.CreateDetailView(objSpace, currentObject);
            controller.SetView(view);
        }
    }
}
