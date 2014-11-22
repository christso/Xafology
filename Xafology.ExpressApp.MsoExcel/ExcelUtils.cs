using OfficeOpenXml;

namespace Xafology.ExpressApp.MsoExcel
{
    public class ExcelUtils
    {
        public static void ClearCells(ExcelWorksheet ws)
        {
            if (ws.Dimension != null)
                ws.Cells[ws.Dimension.Start.Row, ws.Dimension.Start.Column,
                    ws.Dimension.End.Row, ws.Dimension.End.Column].Clear();
        }
    }
}
