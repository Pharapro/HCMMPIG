using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsFilterPayroll
    {
        protected IUnitOfWork unitOfWork;
        public SYHRSetting HRSetting { get; set; }
        public PRPayrollSetting PayrollSetting { get; set; }
        public List<HRTerminType> ListResginType { get; set; }
        public List<HRCareerHistory> ListCareerType { get; set; }
        public List<PROTRate> ListOTType { get; set; }
        public List<HRLeaveType> ListLeaveType { get; set; }
        public List<PRParameter> ListParameter { get; set; }
        public List<PR_RewardsType> LstRewardsType { get; set; }
        public List<PRPensionFundSetting> ListPensionFund { get; set; }
        public List<PRTaxSetting> ListTaxSetting { get; set; }
        public List<HRStaffProfile> ListEmployee { get; set; }
        public List<HREmpCareer> ListtEmpCareer { get; set; }
        public List<HREmpFamily> ListEmpFamily { get; set; }
        public List<HREmpIdentity> ListEmpIdentity { get; set; }
        public List<PRADVPay> ListEmpAdvance { get; set; }
        public List<HREmpLoan> ListEmpLoan { get; set; }
        public List<PREmpOverTime> ListEmpOT { get; set; }
        public List<PREmpAllw> LstEmpAllow { get; set; }
        public List<PREmpBonu> LstEmpBon { get; set; }
        public List<PREmpDeduc> ListEmpDedection { get; set; }
        public List<HREmpLeaveD> ListEmpLeaveDed { get; set; }
        public List<BiMonthlySalarySetting> ListBiMonthlySalary { get; set; }
        public List<PRBankFee> LstBankFee { get; set; }
        public List<HISGenFirstPay> ListSalaryFistPay { get; set; }
        public List<PRExceptType> ListExceptType { get; set; }
        public List<PRIncomeType> ListIncomeType { get; set; }
        public List<PRSocialSecurity> ListSocialSecurity { get; set; }
        public List<PRFringeBenefitSetting> ListFringeBenefitSetting { get; set; }
        public List<SYRoundingRule> ListRounding { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsFilterPayroll()
        {
            OnLoad();
        }
        public void OnLoadSetting()
        {
            HRSetting = unitOfWork.Repository<SYHRSetting>().Queryable().FirstOrDefault();
            PayrollSetting = unitOfWork.Repository<PRPayrollSetting>().Queryable().FirstOrDefault();
            ListResginType = unitOfWork.Repository<HRTerminType>().Queryable().ToList();
            ListCareerType = unitOfWork.Repository<HRCareerHistory>().Queryable().ToList();
            ListPensionFund = unitOfWork.Repository<PRPensionFundSetting>().Queryable().ToList();
            ListOTType = unitOfWork.Repository<PROTRate>().Queryable().ToList();
            ListLeaveType = unitOfWork.Repository<HRLeaveType>().Queryable().ToList();
            ListParameter = unitOfWork.Repository<PRParameter>().Queryable().ToList();
            LstRewardsType = unitOfWork.Repository<PR_RewardsType>().Queryable().ToList();
            ListTaxSetting = unitOfWork.Repository<PRTaxSetting>().Queryable().ToList();
            LstBankFee = unitOfWork.Repository<PRBankFee>().Queryable().ToList();
            ListExceptType = unitOfWork.Repository<PRExceptType>().Where(w => w.IsActive == true).ToList();
            ListIncomeType = unitOfWork.Repository<PRIncomeType>().Queryable().ToList();
            ListSocialSecurity = unitOfWork.Repository<PRSocialSecurity>().Queryable().ToList();
            ListFringeBenefitSetting = unitOfWork.Repository<PRFringeBenefitSetting>().Queryable().ToList();
        }

        public void OnLoadDataEmp(List<string> ListEmpCode)
        {
            ListEmployee = unitOfWork.Repository<HRStaffProfile>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode)).ToList();
            ListtEmpCareer = unitOfWork.Repository<HREmpCareer>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode)).ToList();
            ListEmpFamily = unitOfWork.Repository<HREmpFamily>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.TaxDeduc == true).ToList();
            ListEmpIdentity = unitOfWork.Repository<HREmpIdentity>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) && w.IdentityTye == "IDCard").ToList();
            ListEmpAdvance = unitOfWork.Repository<PRADVPay>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode)).ToList();
            ListEmpLoan = unitOfWork.Repository<HREmpLoan>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode)).ToList();

        }

        public void OnLoadDataBenefit(List<string> ListEmpCode, PRPayPeriodItem Period)
        {
            DateTime FromDate = Period.StartDate; DateTime ToDate = Period.EndDate;
            ListRounding = unitOfWork.Set<SYRoundingRule>().AsQueryable().Where(w => w.EffectiveDate <= ToDate && w.EndDate >= ToDate).ToList();

            ListEmpOT = unitOfWork.Repository<PREmpOverTime>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) &&
               w.PayMonth >= Period.ATStartDate && w.PayMonth <= Period.ATEndDate && w.OTHour > 0).ToList();
            LstEmpAllow = unitOfWork.Repository<PREmpAllw>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) &&
              (w.FromDate.HasValue && w.FromDate <= ToDate) && (w.ToDate.HasValue && w.ToDate >= FromDate)).ToList();

            LstEmpBon = unitOfWork.Repository<PREmpBonu>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) &&
              (w.FromDate.HasValue && w.FromDate <= ToDate) && (w.ToDate.HasValue && w.ToDate >= FromDate)).ToList();
            ListEmpDedection = unitOfWork.Repository<PREmpDeduc>().Queryable().Where(w => ListEmpCode.Contains(w.EmpCode) &&
              (w.FromDate.HasValue && w.FromDate <= ToDate) && (w.ToDate.HasValue && w.ToDate >= FromDate)).ToList();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            ListEmpLeaveDed = (from Leave in unitOfWork.Repository<HREmpLeave>().Queryable()
                               join LeaveD in unitOfWork.Repository<HREmpLeaveD>().Queryable()
                                   on new { Increment = (int)Leave.Increment, EmpCode = Leave.EmpCode }
                                    equals new { Increment = (int)LeaveD.LeaveTranNo, EmpCode = LeaveD.EmpCode }
                               join leaveType in unitOfWork.Repository<HRLeaveType>().Queryable()
                                   on LeaveD.LeaveCode equals leaveType.Code
                               where ListEmpCode.Contains(LeaveD.EmpCode)
                               && Leave.Status == Approve
                               && LeaveD.Status == "Leave"
                               && leaveType.CUT == true
                               && (
                                   (LeaveD.LeaveCode != "ML" && LeaveD.LeaveDate >= Period.ATStartDate && LeaveD.LeaveDate <= Period.ATEndDate)
                                   ||
                                   (LeaveD.LeaveCode == "ML" && LeaveD.LeaveDate >= FromDate && LeaveD.LeaveDate <= ToDate)
                               )
                               select LeaveD
                ).ToList();
        }

        public void OnLoadSettingFP()
        {
            ListBiMonthlySalary = unitOfWork.Repository<BiMonthlySalarySetting>().Queryable().ToList();
        }
        public void OnLadaDataSalryFrist(List<string> ListEmpCode, PRPayPeriodItem Period)
        {
            ListSalaryFistPay = unitOfWork.Repository<HISGenFirstPay>().Queryable().Where(w =>
            ListEmpCode.Contains(w.EmpCode) && w.PeriodID == Period.PeriodID).ToList();
        }

        public List<PRGLmapping> ListGLMap { get; set; }
        public List<PRCostCenter> ListCostCenter { get; set; }
        public List<SYSHRBuiltinAcc> ListSyAccount { get; set; }
        public void OnLoadDataGL()
        {
            ListGLMap = unitOfWork.Repository<PRGLmapping>().Queryable().ToList();
            ListCostCenter = unitOfWork.Repository<PRCostCenter>().Queryable().ToList();
            ListSyAccount = unitOfWork.Repository<SYSHRBuiltinAcc>().Queryable().ToList();
        }
    }
}
