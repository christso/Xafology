using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xafology.ExpressApp.PivotGridLayout.Helpers
{
    public class StaticHelpers
    {
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

    }
}
