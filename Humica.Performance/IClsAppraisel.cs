using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Performance
{
    public interface IClsAppraisel : IClsApplication
    {
        SYUser User { get; set; }
        string ScreenId { get; set; }
        string Rating { get; set; }
        int RateMin { get; set; }
        int RateMax { get; set; }
        string DocType { get; set; }
        HREmpAppraisal Header { get; set; }
        FTFilterEmployee Filter { get; set; }
        HREmpAppProcess HeaderProcess { get; set; }
        List<HREmpAppraisal> ListHeader { get; set; }
        List<HREmpAppraisalItem> ListScore { get; set; }
        List<HRAppGrade> ListApprResult { get; set; }
        List<HRApprRating> ListApprRating { get; set; }
        List<HRApprRegion> ListRegion { get; set; }
        List<HRApprFactor> ListFactor { get; set; }
        List<_ListStaff> ListEmpStaff { get; set; }
        List<HREmpAppraisal> ListAppraisaPending { get; set; }
        List<HREmpAppProcess> ListAppProcess { get; set; }
        List<HREmpAppProcessItem> ListAppProcessItem { get; set; }
        List<HREmpAppraisal> OnIndexLoading(bool IsESS = false);
        List<HRKPIEvalIndicator> ListHRKPIndicator { get; set; }
        string ErrorMessage { get; set; }

        void OnReveiwIndexLoading();
        void OnReveiwIndexLoadingPending();
        void OnIndexLoadingPending();
        void OnIndexLoadingTeam();
        void LoadData(FTFilterEmployee Filter1, List<HRBranch> _lstBranch);
        void OnCreatingLoading(params object[] keys);
        string CreateMulti(string EmpCode, HREmpAppraisal Appraisal);
        string OnEditESSLoading(params object[] keys);
        void OnDetailLoading(params object[] keys);
        string Create();
        string Update(string ID, bool IS_ESS = false);
        string Delete(string ID);
        string RequestToApprove(string id);
        string CancelDoc(string id);
        string CloseDoc(string id);
        string ApproveDoc(string id);
        HREmpAppProcess GetDateAppraisal(string AppID);
        string CreateAppraisalReview(string id);
        void OnDetailLoadingReview(params object[] keys);
        string ReleaseDocReview(params object[] keys);
        string ClosedTheDocReview(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorTeam(params object[] keys);
    }
}
