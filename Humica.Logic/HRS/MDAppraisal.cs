using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public class MDAppraisal
    {
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRAppGrade> ListApprResult { get; set; }
        public List<HRApprRating> ListApprRating { get; set; }
        public List<HRAppLevelMidPoint> ListLeveMidPoint { get; set; }

        public List<HRAppPerformanceRate> ListPerformanceRate { get; set; }
        public List<HRAppSalaryBudget> ListSalaryBudget { get; set; }
        public List<HRAppCompareRatio> ListCompareRatio { get; set; }
        public MDAppraisal()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}