using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMRefCheck : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        RCMApplicant Filter { get; set; }
        RCMRefCheckPerson Header { get; set; }
        RCMAReference RefPerson { get; set; }
        List<RCMRefCheckPerson> ListRefPerson { get; set; }
        List<RCMApplicant> ListApplicant { get; set; }
        List<RCMRefQuestionnaire> ListRefQuestion { get; set; }
        string ErrorMessage { get; set; }
        string RefCheck();
        string UpdateRefCheck(string ApplicantID);
    }
}