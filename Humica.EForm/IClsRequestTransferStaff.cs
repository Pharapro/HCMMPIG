using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.EForm
{
    public interface IClsRequestTransferStaff : IClsApplication
    {
        SYUser User { get; set; }
        SYUserBusiness BS { get; set; }
        FTINYear FInYear { get; set; }
        string ScreenId { get; set; }
        string MessageError { get; set; }
        HR_STAFF_VIEW HeaderStaff { get; set; }
        string DocType { get; set; }
        List<HR_STAFF_VIEW> ListStaffView { get; set; }
        HREFRequestTransferStaff Header { get; set; }
        List<HREFRequestTransferStaff> ListHeader { get; set; }
        List<ClsEmpTransfer> ListPending { get; set; }
        List<ExDocApproval> ListApproval { get; set; }
        List<RCMAEdu> ListRCMAEdu { get; set; }
        List<RCMAWorkHistory> ListRCMAWorkHistory { get; set; }
        List<HR_HisCareer> ListHisCareer { get; set; }
        ClsEmail EmailObject { get; set; }
        string NewSalary { get; set; }
        string OldSalary { get; set; }
        string Increase { get; set; }
        void ProcessTransfers(string userName);
        string CreateTransferStaff();
        string DeleteEmp(string id);
        string EditEmp(string id);
        void SetAutoApproval(string SCREEN_ID, string DocType,string branch, string DeptCode);
        string Approve(string DocNo, string fileName, string URL);
        string Reject(string Document);
        string RequestForApprove(string DocNo, string fileName, string URL);
        string Cancel(string DocNo);
        IEnumerable<HRDivision> GetDivision();
        IEnumerable<HRDepartment> GetDepartment();
        IEnumerable<HRPosition> GetPosition();
        IEnumerable<HRSection> GetSection();
        IEnumerable<HRLevel> GetLevel();
        IEnumerable<HRJobGrade> GetJobGrade();
        string GetDataCompanyGroup(string WorkGroup);
        bool IsHideSalary(string Level);

    }
}