using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Attendance
{
    public interface IClsATPolicyObject : IClsApplication
    {
        ATPolicy Header { get; set; }
        string ScreenId { get; set; }

        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        void OnIndexLoading();
        string Update();
        //FTFilterAttendance Attendance { get; set; }
        //List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
        //HRStaffProfile StaffProfile { get; set; }
        //void OnIndexLoading();
        //void OnFilterStaff(string EmpCode);
        //string GenrateAttendance(string TranNo);

        //Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        //string Set_DefaultShift(DateTime FromDate, DateTime ToDate, List<HRBranch> ListBranch);
    }
}
