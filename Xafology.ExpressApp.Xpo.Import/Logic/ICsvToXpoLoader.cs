using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import.Logic
{
    public interface ICsvToXpoLoader
    {
        List<IMemberInfo> GetTargetMembers(ITypeInfo objTypeInfo);
    }
}
