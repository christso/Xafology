using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Xafology.Spreadsheet
{
    public interface IWorksheet
    {
        void Clear();
        void CopyObjectsToWorksheet(Session session, System.Collections.ICollection objs);
    }
}
