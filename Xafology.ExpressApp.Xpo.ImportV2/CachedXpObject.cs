using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Xpo.Import
{
    public class CachedXpObject
    {
        public CachedXpObject(Type type, object value)
        {
            this.Type = type;
            this.Value = value;
        }

        public Type Type { get; }
        public object Value { get; }

    }
}
