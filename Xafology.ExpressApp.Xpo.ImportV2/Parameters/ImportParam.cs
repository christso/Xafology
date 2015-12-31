using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xafology.ExpressApp.Xpo.Import.Logic.New;

namespace Xafology.ExpressApp.Xpo.Import.Parameters
{
    public interface ImportParam
    {
        XPBaseCollection FieldMaps { get; }
        bool CacheLookupObjects { get; set; }
        bool CreateMembers { get; set; }
        ITypeInfo ObjectTypeInfo { get; set; }
    }
}
