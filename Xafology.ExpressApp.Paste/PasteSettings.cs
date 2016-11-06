using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste
{
    public static class PasteSettings
    {
        private static int maximumOnlineRows = 100;
        public static int MaximumOnlineRows
        {
            get
            {
                return maximumOnlineRows;
            }
            set
            {
                maximumOnlineRows = value;
            }
        }
    }
}
