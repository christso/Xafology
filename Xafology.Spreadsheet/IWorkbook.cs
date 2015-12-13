using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Spreadsheet
{
    public interface IWorkbook
    {
        IWorksheet GetWorksheet(string sheetName);
        Stream Stream { get; }
        void Save();
    }
}
