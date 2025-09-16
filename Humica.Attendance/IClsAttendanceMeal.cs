using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Humica.Attendance
{
    public interface IClsAttendanceMeal : IClsApplication
    {
        FTFilterAttendance Attendance { get; set; }
        string EmpID { get; set; }
        List<ATGenMeal> ListMeal { get; set; }
        string ScreenId { get; set; }

        string GenerateMeal(int PayPeriodId);
        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        void OnIndexLoading();
        void OnLoadingFilter(int PayPeriodId);
        string TransferMeal(string ID, int payperiod);
    }
}
