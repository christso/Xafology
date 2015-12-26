using DevExpress.Xpo;
using DevExpress.Xpo.Metadata.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo
{
    public class ExtUnitOfWork : UnitOfWork
    {
        public ExtUnitOfWork() { }
        public ExtUnitOfWork(DevExpress.Xpo.Metadata.XPDictionary dictionary) : base(dictionary) { } 
        public ExtUnitOfWork(IDataLayer layer, params IDisposable[] disposeOnDisconnect) : base(layer, disposeOnDisconnect) { }
        public ExtUnitOfWork(IObjectLayer layer, params IDisposable[] disposeOnDisconnect) : base(layer, disposeOnDisconnect) { }

        protected override MemberInfoCollection GetPropertiesListForUpdateInsert(object theObject, bool isUpdate, bool addDelayedReference)
        {
            var propertiesForUpdateInsert = base.GetPropertiesListForUpdateInsert(theObject, isUpdate, addDelayedReference);
            for (int i = propertiesForUpdateInsert.Count - 1; i >= 0; i--)
                if (propertiesForUpdateInsert[i].FindAttributeInfo("ExcludeFromUpdate") != null)
                    propertiesForUpdateInsert.RemoveAt(i);

            return propertiesForUpdateInsert;
        }
    }
}
