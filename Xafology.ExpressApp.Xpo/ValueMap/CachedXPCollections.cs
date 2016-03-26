using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Xafology.Utils;

namespace Xafology.ExpressApp.Xpo.ValueMap
{
    public class CachedXPCollections : UniqueIndexedType<Type, XPCollection>
    {
        protected override Type GetKey(XPCollection value)
        {
            return value.ObjectType;
        }
    }
}
