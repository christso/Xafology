using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class LookupCacheCollections
    { 

        private readonly Dictionary<Type, XPCollection> collectionsByName;

        public LookupCacheCollections()
        {
            collectionsByName = new Dictionary<Type, XPCollection>();
        }

    }
}
