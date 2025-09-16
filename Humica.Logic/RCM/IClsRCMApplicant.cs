using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMApplicant : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        RCMApplicant Header { get; set; }
        RCMApplicant PersonalDate { get; set; }
        RCMPInterview PIntV { get; set; }
        List<RCMApplicant> ListHeader { get; set; }
        List<RCMADependent> ListDependent { get; set; }
        List<RCMAEdu> ListEdu { get; set; }
        List<RCMALanguage> ListLang { get; set; }
        List<RCMATraining> ListTraining { get; set; }
        List<RCMAWorkHistory> ListWorkHistory { get; set; }
        List<RCMAReference> ListRef { get; set; }
        List<RCMAIdentity> ListIdentity { get; set; }
        FTINYear Filter { get; set; }
        Filters Filters { get; set; }
        List<MDUploadTemplate> ListTemplate { get; set; }
        ClsEmail EmailObject { get; set; }
        Filtering Filtering { get; set; }
        string Code { get; set; }
        string MessageCode { get; set; }

        string createApplicant(string savedFilePath, string URL);
        string updateApplicant(string ApplicantID);
        string deleteApplicant(string ApplicantID);
        string upload();
        string SENTCV(string ApplicantID, string URL);
        void ApplicantEmail(string RequestedBy, string Applic_, string URL, List<string> filePaths, List<string> positions);
    }
}
