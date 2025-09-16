using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMPShortList : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string MessageError { get; set; }
        RCMApplicant Header { get; set; }
        List<RCMApplicant> ListHeader { get; set; }
        List<RCMAWorkHistory> ListWorkHistory { get; set; }
        List<RCMAEdu> ListEdu { get; set; }
        List<RCMALanguage> ListLang { get; set; }
        List<RCMAReference> ListRef { get; set; }
        FilterShortLsit Filtering { get; set; }
        string Passed(string ApplicantIDs);
        string Reject(string ApplicantIDs);
        string Fail(string ApplicantIDs);
        string Kept(string ApplicantIDs);
    }
}
