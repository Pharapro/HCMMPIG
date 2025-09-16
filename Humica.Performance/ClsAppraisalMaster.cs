using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Performance
{
    public class ClsAppraisalMaster : IClsAppraisalMaster
    {
        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<HRAppGrade> ListApprResult { get; set; }
        public List<HRApprRating> ListApprRating { get; set; }
        public List<HRAppLevelMidPoint> ListLeveMidPoint { get; set; }
        public List<HRAppPerformanceRate> ListPerformanceRate { get; set; }
        public List<HRAppSalaryBudget> ListSalaryBudget { get; set; }
        public List<HRAppCompareRatio> ListCompareRatio { get; set; }
        public ClsAppraisalMaster()
        {
            OnLoad();
        }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
        public void OnIndexLoading()
        {
            ListApprResult = unitOfWork.Set<HRAppGrade>().OrderBy(w => w.Grade).ToList();
            ListApprRating = unitOfWork.Set<HRApprRating>().ToList();
            ListCompareRatio = unitOfWork.Set<HRAppCompareRatio>().ToList();
            ListLeveMidPoint = unitOfWork.Set<HRAppLevelMidPoint>().ToList();
            ListPerformanceRate = unitOfWork.Set<HRAppPerformanceRate>().ToList();
            ListSalaryBudget = unitOfWork.Set<HRAppSalaryBudget>().ToList();
        }
       
        public Dictionary<string, dynamic> OnDataSelectorLoading()
        {
            Dictionary<string, dynamic> keyValues = new Dictionary<string, dynamic>();
            keyValues.Add("REGION_SELECT", unitOfWork.Set<HRApprRegion>().Where(w => w.IsKPI != true).ToList().OrderBy(w => w.Description));
            keyValues.Add("AppraiselType_SELECT", unitOfWork.Set<HRApprType>().ToList());
            keyValues.Add("Level_SELECT", unitOfWork.Set<HRLevel>().ToList());
            return keyValues;
        }
    }
}
