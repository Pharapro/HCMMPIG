using Humica.Core.DB;
using Humica.EF;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsAppraisalMaster : IClsApplication
    {
        List<HRAppGrade> ListApprResult { get; set; }
        List<HRApprRating> ListApprRating { get; set; }
        List<HRAppLevelMidPoint> ListLeveMidPoint { get; set; }
        List<HRAppPerformanceRate> ListPerformanceRate { get; set; }
        List<HRAppSalaryBudget> ListSalaryBudget { get; set; }
        List<HRAppCompareRatio> ListCompareRatio { get; set; }

        Dictionary<string, dynamic> OnDataSelectorLoading();
        void OnIndexLoading();
    }
}
