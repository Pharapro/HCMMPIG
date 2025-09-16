using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;

namespace Humica.Logic.EOB
{
    public interface IClsEOBConfirmAlert : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        string DocType { get; set; }
        EOBConfirmAlert Header { get; set; }
        RCMApplicant Applicant { get; set; }
        List<EOBConfirmAlert> ListHeader { get; set; }
        List<RCMApplicant> ListApplicant { get; set; }
        ClsEmail EmailObject { get; set; }
        TPEmailTemplate EmailTemplate { get; set; }
        string Action { get; set; }
        SYSplitItem SelectListItem { get; set; }
        List<object> ListObjectDictionary { get; set; }
        string MessageError { get; set; }
        string ConfAlert(string ID);
        string updConfirm(string ID);
        string ApproveTheDoc(string ID, string Upload);
        string requestApprove(string ID);
        string RejectTheDoc(string ID);
        string Consider(string ID);
    }

}