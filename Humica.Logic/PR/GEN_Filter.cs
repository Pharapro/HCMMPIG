using Humica.Core.DB;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Logic.PR
{
    public class GEN_Filter1
    {
        public decimal? ExchangeRate { get; set; }
        public int PeriodID { get; set; }

        public string EmpCode { get; set; }

        public int InYear { get; set; }

        public int InMonth { get; set; }

        public string Status { get; set; }

        public string PayType { get; set; }

        public string Default_Curremcy { get; set; }

        public string Round_UP { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public SYHRCompany CompanyCode { get; set; }

        public DateTime PayFrom { get; set; }

        public DateTime PayTo { get; set; }

        public HRStaffProfile Staff { get; set; }

        public PRParameter Parameter { get; set; }

        public HisPendingAppSalaryFP HisAppSalary { get; set; }

        public HISGenSalary HisGensalarys { get; set; }

        public HISGenFirstPay HisGenFirstPay { get; set; }

        public List<PREmpAllw> LstEmpAllow { get; set; }

        public List<PREmpBonu> LstEmpBon { get; set; }

        public List<HREmpCareer> LstEmpCareer { get; set; }

        public List<PR_RewardsType> LstRewardsType { get; set; }

        public List<PRBankFee> LstBankFee { get; set; }

        public List<ClsPayHis> LstPayHis { get; set; }

        public List<HISGenAllowance> ListHisEmpAllw { get; set; }

        public List<HISGenBonu> ListHisEmpBonu { get; set; }

        public List<HISGenDeduction> ListHisEmpDed { get; set; }

        public List<HISGenOTHour> ListHisEmpOT { get; set; }

        public List<HISGenLeaveDeduct> ListHisEmpLeave { get; set; }

        public List<PREmpHold> LstEmpHold { get; set; }

        public List<PRTaxSetting> ListTaxSetting { get; set; }

        public List<HisGenOTFirstPay> ListHisEmpOTFirstPay { get; set; }

        public List<HisGenLeaveDFirstPay> ListHisEmpLeaveFirstPay { get; set; }

        public List<HisGenMonthlySalaryRetro> ListHisGenMonthlySalaryRetro { get; set; }
        public BiMonthlySalarySetting BiMonthlySalarySetting { get; set; } = new BiMonthlySalarySetting();
    }
}
