using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humica.Logic
{
    public class ClsMonthOfYear
    {
        public int Month { get; set; }
        public String MonthDesc { get; set; }
        public static List<ClsMonthOfYear> LoadMonth()
        {
            List<ClsMonthOfYear> _lst = new List<ClsMonthOfYear>();
            _lst.Add(new ClsMonthOfYear { Month = 1, MonthDesc = "Jan" });
            _lst.Add(new ClsMonthOfYear { Month = 2, MonthDesc = "Feb" });
            _lst.Add(new ClsMonthOfYear { Month = 3, MonthDesc = "Mar" });
            _lst.Add(new ClsMonthOfYear { Month = 4, MonthDesc = "Apr" });
            _lst.Add(new ClsMonthOfYear { Month = 5, MonthDesc = "May" });
            _lst.Add(new ClsMonthOfYear { Month = 6, MonthDesc = "Jun" });
            _lst.Add(new ClsMonthOfYear { Month = 7, MonthDesc = "Jul" });
            _lst.Add(new ClsMonthOfYear { Month = 8, MonthDesc = "Aug" });
            _lst.Add(new ClsMonthOfYear { Month = 9, MonthDesc = "Sep" });
            _lst.Add(new ClsMonthOfYear { Month = 10, MonthDesc = "Oct" });
            _lst.Add(new ClsMonthOfYear { Month = 11, MonthDesc = "Nov" });
            _lst.Add(new ClsMonthOfYear { Month = 12, MonthDesc = "Dec" });
            return _lst;
        }
    }
}
