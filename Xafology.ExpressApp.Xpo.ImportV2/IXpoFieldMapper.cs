using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import
{
    public interface IXpoFieldMapper
    {
        void SetMemberValue(IXPObject targetObj, IMemberInfo memberInfo, string value, bool createMember = false, bool cacheObject = false);
        Dictionary<Type, List<string>> LookupsNotFound { get; }
        Dictionary<Type, XPCollection> LookupCacheDictionary { get; }
    }
}
