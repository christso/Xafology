using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.SystemModule;

namespace Xafology.ExpressApp.Xpo.SequentialBase
{
    public sealed class XafologySequentialBaseModule : XafologyModuleBase
    {
        public XafologySequentialBaseModule()
        {
            // Create Tables in Database
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.OidGenerator));
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ServerPrefix));
        }

    }
}
