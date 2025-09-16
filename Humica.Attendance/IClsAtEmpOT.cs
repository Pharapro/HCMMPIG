using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using System.Collections.Generic;
namespace Humica.Attendance
{
    public interface IClsAtEmpOT : IClsApplication
    {
        FTFilterAttendance Attendance { get; set; }
        List<VIEW_ATEmpSchedule> ListEmpOTNS { get; set; }
        string ScreenId { get; set; }

        void OnLoadingFilter();
        string TransferOT(string ID, List<VIEW_ATEmpSchedule> List);
    }
}
