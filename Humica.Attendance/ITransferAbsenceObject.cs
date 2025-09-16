using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System;
using System.Collections.Generic;

namespace Humica.Attendance
{
    public interface ITransferAbsenceObject : IClsApplication
    {
        string ScreenId { get; set; }
        string EmpID { get; set; }
        int Progress { get; set; }
        FTFilterAttendance Attendance { get; set; }
        List<VIEW_ATEmpSchedule> ListEmpSchdule { get; set; }
		List<HRDepartment> ListDepartment { get; set; }
		HRStaffProfile StaffProfile { get; set; }
        void OnIndexLoading();
        string GenrateAttendance(string ID, DateTime FromDate, DateTime ToDate);
    }
}
