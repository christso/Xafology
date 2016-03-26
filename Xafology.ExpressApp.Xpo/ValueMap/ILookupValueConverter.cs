using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.ExpressApp.Xpo.ValueMap
{
    public delegate void LogUnmatchedLookupsDelegate(Type type, string value);

    public interface ILookupValueConverter
    {
        IXPObject ConvertToXpObject(string value, IMemberInfo memberInfo, Session session,
            bool createMember = false);
        LogUnmatchedLookupsDelegate UnmatchedLookupLogger { get; set; }
    }
}
