using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Xml;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Utils;
using Xafology.Spreadsheet.Attributes;

namespace Xafology.Spreadsheet
{
    internal class WorksheetProxy : IWorksheet, IDisposable
    {
        private readonly ExcelWorksheet _worksheet;

        public WorksheetProxy(ExcelWorksheet worksheet)
        {
            _worksheet = worksheet;
        }

        public void Clear()
        {
            if (_worksheet.Dimension != null)
                _worksheet.Cells[_worksheet.Dimension.Start.Row, _worksheet.Dimension.Start.Column,
                    _worksheet.Dimension.End.Row, _worksheet.Dimension.End.Column].Clear();
        }

        // TODO: remove dependency on Session
        public void CopyObjectsToWorksheet(Session session, System.Collections.ICollection objs)
        {
            // clear existing values
            Clear();

            // headings
            Type objType = null;
            foreach (var obj in objs)
            {
                objType = obj.GetType();
            }
            if (objType == null) return;

            var classInfo = session.GetClassInfo(objType);

            int h = 1;
            foreach (var memberInfo in classInfo.Members
                .Where(x => x.HasAttribute(typeof(ExcelReportFieldAttribute))))
            {
                _worksheet.Cells[1, h++].Value = string.IsNullOrEmpty(memberInfo.DisplayName) ?
                    CaptionHelper.ConvertCompoundName(memberInfo.Name)
                    : memberInfo.DisplayName;
            }

            // data
            int r = 2;
            foreach (IXPObject obj in objs)
            {
                int c = 1;
                foreach (var memberInfo in obj.ClassInfo.Members
                    .Where(x => x.HasAttribute(typeof(ExcelReportFieldAttribute))))
                {
                    _worksheet.Cells[r, c++].Value = memberInfo.GetValue(obj);
                }
                r++;
            }

        }

        public void Dispose()
        {
            if (_worksheet != null)
                _worksheet.Dispose();
        }
    }
}
