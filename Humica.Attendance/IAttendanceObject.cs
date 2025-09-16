using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Attendance
{
    public interface IAttendanceObject : IClsApplication
    {
        string ScreenId { get; set; }
        string EmpID { get; set; }
        int Progress { get; set; }
        FTFilterAttendance Attendance { get; set; }
        List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
        List<HRRequestOT> ListOT { get; set; }
        HRStaffProfile StaffProfile { get; set; }
        void OnIndexLoading();
        void OnFilterStaff(string EmpCode);
        string GenrateAttendance(string TranNo);
        string TransferOT(string ID, List<HRRequestOT> List);
        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        string Set_DefaultShift(DateTime FromDate, DateTime ToDate, List<HRBranch> ListBranch);
    }
}
