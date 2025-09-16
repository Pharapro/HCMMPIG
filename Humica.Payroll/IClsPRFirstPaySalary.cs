using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Payroll
{
    public interface IClsPRFirstPaySalary : IClsApplication
    {
        List<ClsEmpGenerateSalary> ListEmployeeGen { get; set; }
        FTFilerPayroll Filter { get; set; }
        string EmpID { get; set; }
        string ScreenId { get; set; }
        string MessageCode { get; set; }
        List<SYEventLog> ListLog { get; set; }
        HISGenFirstPay HeaderSalaryFP { get; set; }
        List<HISPaySlipFirstPay> ListPaySlip { get; set; }
        List<LeaveDeduction> ListLeaveDed { get; set; }
        List<HisEmpRewardFP> ListDeductionFP { get; set; }
        List<HisGenOTFirstPay> ListEmpOTFP { get; set; }
        List<HisEmpRewardFP> ListEmpBonusFP { get; set; }
        List<HisEmpRewardFP> ListEmpAllowanceFP { get; set; }
        List<HISGenFirstPaySalaryD> ListBasicSalary { get; set; }
        HR_STAFF_VIEW STAFF_VIEW { get; set; }

        void OnIndexLoading();
        string OnIndexLoadingFilter();
        string Delete_GenerateAll(int Period);
        string GenerateSalaryFirstPay();
        Dictionary<string, dynamic> OnDataSelectorLoading(params object[] keys);
        Dictionary<string, dynamic> OnDataSelectorDetailFP(params object[] keys);
        void OnIndexLoadingDetail();
        void GetDataDetailFP(string EmpCode, int Period);
        string DeleteGenerate(string EmpCode, int Period);
    }
}
