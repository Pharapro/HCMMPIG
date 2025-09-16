using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Attendance
{
    public interface IClsAttendanceCheck : IClsApplication
    {
        FTFilterAttendance Filter { get; set; }
        List<VIEW_ATInOut> ListInOut { get; set; }
        HRStaffProfile StaffProfile { get; set; }

        void OnIndexLoading();
        void OnIndexLoading(FTFilterAttendance _Filter);

        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        string OnGridModify(VIEW_ATInOut MModel, string Action);
        void OnFilterStaff(string EmpCode);
    }
}
