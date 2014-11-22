using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xafology.Utils
{
    public class DateUtils
    {
        public static DateTime DateObject(int Year, int Month, int Day)
        {
            //adjust Month that is greater than 12, e.g. if month = 13,
            //then reduce month by 12 and add 1 to Year
            var yearsInMonth = (int)(Month / 12);
            if (Month > 12)
            {
                Year += yearsInMonth;
                Month -= yearsInMonth * 12;
            }
            //get end of previous month month
            //by getting 1st day of current month less 1 day
            //TODO: if day or month <= 0 then adjust
            if (Day == 0)
                return new DateTime(Year, Month, 1).AddDays(-1);
            return new DateTime(Year, Month, Day);
        }
    }
}
