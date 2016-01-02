using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Collections;
using DevExpress.ExpressApp.DC;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class CachedLookupObjects : Dictionary<Type, XPCollection>
    {
        public void AddToCache(IMemberInfo memberInfo, Session session)
        {
            // add objects to cache dictionary
            XPCollection objs = null;
            if (!this.TryGetValue(memberInfo.MemberType, out objs))
            {
                objs = new XPCollection(session, memberInfo.MemberType);
                this.Add(memberInfo.MemberType, objs);
            }
        }
    }
}
