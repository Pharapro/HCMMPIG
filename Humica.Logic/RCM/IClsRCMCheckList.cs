using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMCheckList : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ApplyPosition { get; set; }
        string DocType { get; set; }
        string ApplicantName { get; set; }
        string ApplyBranch { get; set; }
        string Gender { get; set; }
        string ApplyDept { get; set; }
        string WorkType { get; set; }
        string VacNo { get; set; }
        int? IntVStep { get; set; }
        decimal ExpectSalary { get; set; }
        RCMApplicant App { get; set; }
        RCMPInterview Header { get; set; }
        RCMIntVQuestionnaire IntVQ { get; set; }
        List<RCMApplicant> ListCandidate { get; set; }
        List<RCMPInterview> ListInt { get; set; }
        List<RCMIntVQuestionnaire> ListQuestionnair { get; set; }
        List<RCMVInterviewer> ListInterViewer { get; set; }
        FilterCandidate Filtering { get; set; }
        string ScreenId { get; set; }
        string ErrorMessage { get; set; }
        string createJobIntV();
        string updateCheckList(string TranNo);
        string ReleaseDoc(int ID);
    }
}