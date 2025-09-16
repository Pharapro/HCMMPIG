using Humica.Core.DB;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Condition
{
    public class ClsPerformance
    {
        public void NewAppraisalItem(HREmpAppraisalItem D, List<HRApprRegion> _ListRegion, HRApprFactor Factor, string AppraisalNo)
        {
            D.ApprID = AppraisalNo;
            D.Region = Factor.Region;
            D.Remark = Factor.Remark;
            D.Description = Factor.Description;
            D.SecDescription = Factor.SecDescription;
            D.Code = Factor.Code;
            D.RatingID = 0;
            foreach (var read in _ListRegion.Where(w => w.Code == Factor.Region).ToList())
            {
                D.RegionDescription = read.Description;
            }
        }
        public void NewAppraisalSummary(HREmpAppraisalSummary D, HRApprRegion S, string AppraisalNo, string AppraisalType)
        {
            D.AppraisalNo = AppraisalNo;
            D.TaskID = S.Code;
            D.AppraisalType = AppraisalType;
            D.EvaluationCriteria = S.Description;
            if (!D.Weight.HasValue) D.Weight = 0;
            if (S.IsKPI != true)
            {
                D.Weight = S.Rating;
                D.Score = 0;
            }
            if (D.Weight > 1) D.Weight = D.Weight / 100.00M;
            D.InOrder = S.InOrder;
            D.IsKPI = S.IsKPI;
        }

    }
}
