using Humica.Core;
using Humica.Core.DB;
using System;
using System.Collections.Generic;

namespace Humica.Payroll
{
    public class ClsFilerFPSalary
    {
        public int SalaryBase { get; set; }
        public decimal SalarySoSe { get; set; }
        public string UserName { get; set; }
        public string CompanyCode { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? NSSFRate { get; set; }
        public DateTime PayFrom { get; set; }
        public DateTime PayTo { get; set; }
        public string Round_UP { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime ATFromDate { get; set; }
        public DateTime ATToDate { get; set; }
        public int InYear { get; set; }
        //public string NSSFSalaryType { get; set; }
        public string Default_Curremcy { get; set; }
        public int InMonth { get; set; }
        public int Period { get; set; }
        public string EmpCode { get; set; }
        public HRStaffProfile Staff { get; set; }
        public PRParameter Parameter { get; set; }
        public PRPayPeriodItem PeriodItem { get; set; }
        public BiMonthlySalarySetting BiMonthlySalary { get; set; }
        public List<HREmpCareer> LstEmpCareer { get; set; }
        public List<PREmpHold> LstEmpHold { get; set; }
        //public List<ClsPayHis> LstPayHis { get; set; }
        public List<PR_RewardsType> LstRewardsType { get; set; }
    }

    public class ClsFPSalary : ClsFilerFPSalary
    {
        public HisPendingAppSalaryFP HisAppSalary { get; set; }
        public List<ClsPayHis> LstPayHis { get; set; }
        public List<HisPendingAppSalaryFP> AppSalaryFP { get; set; }
        public HISGenFirstPay HisGenFirstPay { get; set; }
        public List<HISGenFirstPay> ListFirstPay { get; set; }
        public List<HISGenFirstPaySalaryD> ListFirstPayItem { get; set; }
        public List<HisGenOTFirstPay> ListEmpOTFP { get; set; }
        public List<HisEmpRewardFP> ListEmpRewardFP { get; set; }
        public List<HisGenLeaveDFirstPay> ListDedLeaveFP { get; set; }
        public List<HISPaySlipFirstPay> ListPaySlip { get; set; }
    }

    public class ClsSalary : ClsFilerFPSalary
    {
        public HisPendingAppSalary HisAppSalary { get; set; }
        public List<HisPendingAppSalary> ApprovalSalary { get; set; }
        public HISGenSalary HeaderSalary { get; set; }
        public List<HISGenSalary> ListSalary { get; set; }
        public List<HISGenSalaryD> ListSalaryItem { get; set; }
        public List<HISGenOTHour> ListEmpOT { get; set; }
        public List<HisEmpReward> ListEmpReward { get; set; }
        //public List<HISGenAllowance> ListHisAllowance { get; set; }
        //public List<HISGenBonu> ListHisBonus { get; set; }
        //public List<HISGenDeduction> ListDeduction { get; set; }
        public List<HISGenLeaveDeduct> ListDedLeave { get; set; }
        public List<HISPaySlip> ListPaySlip { get; set; }
        public List<HisEmpPayment> ListHisEmpPayment { get; set; }
        public List<HisSalaryPay> ListHisSalaryPay { get; set; }
        public List<HisEmpSocialSecurity> ListEmpSoSe { get; set; }
        public List<HISGLBenCharge> ListHisGLCharge { get; set; }
    }
}
