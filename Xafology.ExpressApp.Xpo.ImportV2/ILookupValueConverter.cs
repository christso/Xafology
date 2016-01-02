using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.ExpressApp.Xpo.Import
{
    public interface ILookupValueConverter
    {
        IXPObject ConvertToXpObject(string value, IMemberInfo memberInfo, Session session,
            bool createMember = false);
        Dictionary<Type, List<string>> LookupsNotFound { get; }
    }
}
