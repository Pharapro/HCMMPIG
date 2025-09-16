using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;

namespace Humica.EForm
{
    public interface IClsDisciplinary: IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        string ScreenId { get; set; }
        FTINYear FInYear { get; set; }
        string DocType { get; set; }
        string MessageError { get; set; }
        HREmpDisciplinary Header { get; set; }
        HRStaffProfile Staff { get; set; }
        string CompanyCode { get; set; }
        string Plant { get; set; }
        string MessageCode { get; set; }
        List<ClsEmpDisciplinary> ListRequestPending { get; set; }
        List<HRStaffProfile> ListStaff { get; set; }
        List<HREmpDisciplinary> ListHeader { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        List<ExCfWFApprover> ListEXCFApproval { get; set; }
        void LoadData(string userName);
        string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action);
        string CreateEmpDiscp(string URL, bool IsDoc = false);
        string EditEmpDiscp(int id);
        string DeleteEmpDiscp(int id);
        void SetAutoApproval(string screenId, string docType, string branch, string deptCode);
        string RequestForApprove(string Doc, string fileName, string URL);
        string Cancel(string Doc);
        string Reject(string Document);
        string approveTheDoc(string Inc, string fileName, string URL);
        List<HREmpDisciplinary> LoadData();
    }
}