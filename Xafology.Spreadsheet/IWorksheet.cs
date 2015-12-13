using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Spreadsheet
{
    public interface IWorksheet
    {
        void Clear();
        void CopyObjectsToWorksheet(Session session, System.Collections.ICollection objs);
    }
}
