using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;
using static Humica.Logic.HR.ClsBookingRoom;

namespace Humica.Logic.HR
{
    public interface IClsBookingRoom : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        List<HRBookingSchedule> ListBookingSchedule { get; set; }
        List<HRBookingRoom> ListHeader { get; set; }
        List<ClsListHeader> ListHeaderItem { get; set; }
        HRBookingRoom Header { get; set; }

        string createBooking();
        string EditBooking(string id);
        string RejctedBooking(string id, string Comment);
        void OnCreated();
        void OnDetail(string id);
        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        void OnLoadingIndex();
        string IsValidBookingTime(HRBookingSchedule objCheck, List<HRBookingSchedule> ListCurrent);
        string IsValidBookingTimes(HRBookingSchedule objCheck, List<HRBookingSchedule> ListCurrent);
    }
}
