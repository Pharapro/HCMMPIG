using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMInterviewProc : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        RCMApplicant Filter { get; set; }
        string ApplyPosition { get; set; }
        string DocType { get; set; }
        string Vacancy { get; set; }
        string ChkData { get; set; }
        int IntvStep { get; set; }
        RCMPInterview Header { get; set; }
        RCMApplicant App { get; set; }
        string ScreenId { get; set; }
        List<MDUploadTemplate> ListTemplate { get; set; }
        List<RCMPInterview> ListInterview { get; set; }
        List<RCMPInterview> ListWaiting { get; set; }
        List<RCMAEdu> ListEdu { get; set; }
        List<RCMAWorkHistory> ListWorkHistory { get; set; }
        List<RCMALanguage> ListLanguage { get; set; }
        List<RCMInterveiwFactor> ListFactor { get; set; }
        List<RCMEmpEvaluateScore> ListScore { get; set; }
        List<RCMInterveiwRating> ListInterviewRating { get; set; }
        List<RCMInterveiwRegion> ListRegion { get; set; }
        string ErrorMessage { get; set; }

        string createIntV(string TranNo);
        string Cancel(string TranNo);
        string Passed(int TranNo);
        string Keepped(int TranNo);
        string NextStep(int TranNo);
    }
}