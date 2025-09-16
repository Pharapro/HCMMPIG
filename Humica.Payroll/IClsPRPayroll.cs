using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Payroll
{
    public interface IClsPRPayroll : IClsApplication
    {
        string EmpID { get; set; }
        string MessageCode { get; set; }
        List<ClsEmpGenerateSalary> ListEmployeeGen { get; set; }
        FTFilerPayroll Filter { get; set; }
        List<SYEventLog> ListLog { get; set; }
        List<MDUploadTemplate> ListTemplate { get; set; }
        List<ListUploadPayHis> ListImport { get; set; }
        HISGenSalary HeaderSalary { get; set; }
        List<HISGenSalaryD> ListBasicSalary { get; set; }
        List<HISGenOTHour> ListOverTime { get; set; }
        List<HisEmpReward> ListAllowance { get; set; }
        List<HisEmpReward> ListBonus { get; set; }
        List<Payroll> ListEmpPayment { get; set; }
        List<HisEmpReward> ListDeduction { get; set; }
        List<LeaveDeduction> ListLeaveDed { get; set; }
        HR_STAFF_VIEW STAFF_VIEW { get; set; }
        List<HisEmpSocialSecurity> ListEmpSoSe { get; set; }
        List<ClsGLCharge> ListGLCharge { get; set; }

        string Delete_GenerateAll(int Period);
        string Delete_PayRecord(string EmpCode, int Period);
        string GenerateSalary();
        void GetDataDetail(string EmpCode, int Period);
        void OnIndexLoading();
        void OnIndexLoadingDetail();
        string OnIndexLoadingFilter();
        Dictionary<string, dynamic> OnDataSelector(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
    }
}
