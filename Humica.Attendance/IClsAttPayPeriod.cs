using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Attendance
{
    public interface IClsAttPayPeriod : IClsApplication
    {
        List<ATPayperiod> ListPeriod { get; set; }
        ATPayperiod Header { get; set; }
        string OnGridModifyPeriod(ATPayperiod MModel, string Action);
        void OnIndexLoading();
        void OnIndexLoadingPeriod();
    }
}


