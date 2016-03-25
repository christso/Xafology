using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.ExpressApp.Paste.Win
{
    public class Clipboard : ICopiedText
    {
        public string Data
        {
            get
            {
                System.Windows.Forms.IDataObject iData = System.Windows.Forms.Clipboard.GetDataObject();
                if (iData == null) return "";

                if (iData.GetDataPresent(System.Windows.Forms.DataFormats.Text))
                    return (string)iData.GetData(System.Windows.Forms.DataFormats.Text);
                return "";
            }
        }
    }
}
