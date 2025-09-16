using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsKPITracking : IClsApplication
    {
        FTINYear FInYear { get; set; }
        SYUser User { get; set; }
        string ScreenId { get; set; }
        DateTime DocumentDate { get; set; }
        HRKPITracking HeaderKPITracking { get; set; }
        List<ClsTracking> listKPITracking { get; set; }
        List<ListAssign> ListKPIEmpPending { get; set; }
        List<HRKPITimeSheet> ListTimeSheet { get; set; }
        string Options { get; set; }
        string TotalActual { get; set; }
        DateTime FromTime { get; set; }
        DateTime ToTime { get; set; }

        void OnIndexLoading(int INYear,DateTime FromDate,DateTime ToDate,string Status, bool ESS = false);
        List<ListAssign> OnIndexLoadingAssign(bool ESS = false);
        void OnCreatingLoading(string ID, string EmpCode);
        void OnDetailLoading(params object[] keys);
        string Create();
        string Update(int id);
        string Delete(params object[] keys);
        string Approved(string id, bool ESS = false);
        string Reject(string id,string comment, bool ESS = false);
        string OnGridModify(HRKPITimeSheet MModel, string Action);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataStatusLoading(params object[] keys);
        HRKPIAssignItem GetDataTaskItem(string AssignCode, string KPI);
        (decimal Hours, DateTime From_Time) CalculateHour(DateTime DocmentDate, DateTime FromTime, DateTime ToTime);
        void GetTimer(string EmpCode, DateTime InDate);
        string DeleteAll(string id);
    }
}
