using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMVacancy : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string JobResponsibility { get; set; }
        string JobRequirement { get; set; }
        string PostOfJD { get; set; }
        string MessageError { get; set; }
        RCMVacancy Header { get; set; }
        RCMRecruitRequest RecruitRequest { get; set; }
        RCMApplicant Applicants { get; set; }
        List<RCMVacancy> ListHeader { get; set; }
        List<RCMRecruitRequest> ListPending { get; set; }
        RCMVInterviewer VInt { get; set; }
        List<RCMVInterviewer> ListInt { get; set; }
        List<RCMAdvertising> ListAdvertise { get; set; }
        string Code { get; set; }
        string MessageCode { get; set; }

        string createVAC();
        string updateVAC(string Code);
        string deleteVAC(string Code);
        string Processing(string Code);
        string Closed(string Code);
        string Completed(string Code);
    }
}
