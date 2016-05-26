using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Parameters
{
    public interface IPasteParam
    {
        bool CacheLookupObjects { get; set; }
        bool CreateMembers { get; set; }
        string ObjectTypeName { get; set; }
    }
}
