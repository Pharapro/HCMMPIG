using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Attendance
{
    public interface IClsLeaveType : IClsApplication
    {
        List<HRLeaveType> ListHeader { get; set; }
        HRLeaveType Header { get; set; }
        IQueryable ListLeaveProRate { get; set; }
        IQueryable ListLHourPolicy { get; set; }
        IQueryable ListLeavePolicy { get; set; }

        string Create();
        string Delete(string id);
        void OnCreatingLoading();
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorPolicy(params object[] keys);
        void OnDetailLoading(params object[] keys);
        string OnGridModifyHour(HRLeaveHourPolicy MModel, string Action);
        string OnGridModifyLeaveCondition(HRLeaveDedPolicy MModel, string Action);
        string OnGridModifyPolicy(HRLeaveProRate MModel, string Action);
        void OnIndexCondition();
        void OnIndexHourPolicy();
        void OnIndexLoading();
        void OnIndexLoadingPolicy();
        void OnIndexPolicy();
        string Update(string id);
    }
}
