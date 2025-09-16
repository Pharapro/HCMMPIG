using Humica.Core;
using Humica.Core.DB;
using Humica.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsEmpReward
    {
        public bool IncludeIsDaily { get; set; }
        public decimal RoundReward(ClsFilterPayroll _filter, decimal Amount, string Type)
        {
            decimal Re_Amount = 0;
            var Round = _filter.ListRounding.FirstOrDefault(w => w.Type == Type);
            IncludeIsDaily = false;
            if (Round != null && Round.IncludeIsDaily)
            {
                IncludeIsDaily = true;
                Re_Amount = ClsRounding.Rounding(Amount, Round.RoundPlaces, Round.RoundMethod);
            }
            else if (Round != null)
            {
                Re_Amount = ClsRounding.Rounding(Amount, Round.RoundPlaces, Round.RoundMethod);
            }
            else
                Re_Amount = Amount;

            return Re_Amount;
        }
        public HISGenOTHour AddEmpOverTime(ClsSalary _filter,PROTRate OTType,PREmpOverTime OT,decimal DailyRate)
        {
            var EmpOverTime = new HISGenOTHour()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.CompanyCode,
                EmpCode = _filter.EmpCode,
                OTDate = OT.OTDate,
                //BaseSalary = BaseSalary,
                //WorkDay = WorkDayPerMonth,
                //WorkHour = WorkHourPerDay,
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                OTHour = OT.OTHour,
                OTDesc = OTType.OTType,
                OTHDesc = OTType.OTHDESC,
                OTCode = OT.OTType,
                OTRate = DailyRate,
                OTFormula = "(" + OTType.Foperand + ")" + OTType.Soperator + OTType.Toperand,
                Measure = OTType.Measure,
                Amount = DailyRate * OT.OTHour,
                CreateBy = _filter.UserName,
                CreateOn = DateTime.Now
            };
            return EmpOverTime;
        }
        public HISGenAllowance AddEmpAllowance(ClsSalary _filter, PR_RewardsType RewType, decimal Amount, decimal ActualWorkDay)
        {
            var EmpAllowance = new HISGenAllowance()
            {
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                EmpCode = _filter.EmpCode,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                WorkDay = 0,
                RatePerDay = 0,
                AllwCode = RewType.Code,
                AllwDesc = RewType.Description,
                OthDesc = RewType.OthDesc,
                TaxAble = RewType.Tax,
                FringTax = RewType.FTax,
                AllwAm = Amount,
                AllwAmPM = Amount / ActualWorkDay,
                CreateBy = _filter.UserName,
                CreateOn = DateTime.Now
            };
            return EmpAllowance;
        }
        public HISGenBonu AddEmpBonus(ClsSalary _filter, PR_RewardsType RewType, decimal Amount)
        {
            var EmpBonus = new HISGenBonu()
            {
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                EmpCode = _filter.EmpCode,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                BonusCode = RewType.Code,
                BonusDesc = RewType.Description,
                OthDesc = RewType.OthDesc,
                TaxAble = RewType.Tax,
                FringTax = RewType.FTax,
                BonusAM = Amount,
                CreateBy = _filter.UserName,
                CreateOn = DateTime.Now
            };
            return EmpBonus;
        }
        public HISGenDeduction AddEmpDeduction(ClsSalary _filter, PR_RewardsType RewType, decimal Amount, decimal ActualWorkDay)
        {
            var EmpDeduction = new HISGenDeduction()
            {
                INYear = _filter.InYear,
                INMonth = _filter.InMonth,
                EmpCode = _filter.EmpCode,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                WorkDay = 0,
                RatePerDay = 0,
                DedCode = RewType.Code,
                DedDesc = RewType.Description,
                OthDesc = RewType.OthDesc,
                TaxAble = RewType.Tax,
                DedAm = Amount,
                DedAMPM = Amount / ActualWorkDay,
                CreateBy = _filter.UserName,
                CreateOn = DateTime.Now
            };
            return EmpDeduction;
        }
        public HisEmpReward AddEmpReward(ClsSalary _filter, PR_RewardsType RewType, decimal Amount, decimal ActualWorkDay)
        {
            string Description = null;
            var objTaxType = ClsFilterGeneral.OnLoadTaxType().ToList().FirstOrDefault(w => w.Code == RewType.TaxType);
            if (objTaxType != null) { Description = objTaxType.Description; }
            var EmpAllowance = new HisEmpReward()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.CompanyCode,
                EmpCode = _filter.EmpCode,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                Amount = Amount,
                RewardType = RewType.RewardType,
                Code = RewType.Code,
                Description = RewType.Description,
                TaxType = RewType.TaxType,
                TaxDescription = Description,
                //CreateBy = _filter.UserName,
                //CreateOn = DateTime.Now
            };
            return EmpAllowance;
        }
        public HisEmpRewardFP AddEmpRewardFP(ClsFPSalary _filter, PR_RewardsType RewType, decimal Amount, decimal ActualWorkDay)
        {
            string Description = null;
            var objTaxType = ClsFilterGeneral.OnLoadTaxType().ToList().FirstOrDefault(w => w.Code == RewType.TaxType);
            if (objTaxType != null) { Description = objTaxType.Description; }
            var EmpAllowance = new HisEmpRewardFP()
            {
                PeriodID = _filter.Period,
                CompanyCode = _filter.CompanyCode,
                EmpCode = _filter.EmpCode,
                FromDate = _filter.FromDate,
                ToDate = _filter.ToDate,
                Amount = Amount,
                RewardType = RewType.RewardType,
                Code = RewType.Code,
                Description = RewType.Description,
                TaxType = RewType.TaxType,
                TaxDescription = Description,
            };
            return EmpAllowance;
        }

        public HisEmpPayment Emp_Payment(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster,
            string IncomeType, string Code, string Description, decimal Amount,bool IsSalary, string TaxType)
        {
            if (TaxType == "TX-001") TaxType = "TAX";
            else if (TaxType == "TX-002") TaxType = "FTAX";
            clsOrderPayment clspayment = new clsOrderPayment();
            var objIncome = _FPayrllMaster.ListIncomeType.FirstOrDefault(w => w.Code == IncomeType);
            HisEmpPayment payment = new HisEmpPayment();
            if (objIncome != null)
            {
                payment = new HisEmpPayment()
                {
                    PeriodID = _filter.Period,
                    CompanyCode = _filter.CompanyCode,
                    FromDate = _filter.FromDate,
                    ToDate = _filter.ToDate,
                    EmpCode = _filter.EmpCode,
                    IncomeType = IncomeType,
                    PayType = objIncome.IncomeType,
                    Code = Code,
                    Description = Description,
                    Amount = Amount,
                    TaxType = TaxType,
                    IsSalary = IsSalary
                };
            }
            return payment;
        }
    }
    public class clsOrderPayment
    {
        public string Groups { get; set; }
        public string IncomeType { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Tax { get; set; }
        public bool FTax { get; set; }

        public List<clsOrderPayment> LoadData()
        {
            return new List<clsOrderPayment>
            {
                new clsOrderPayment { Groups = "A",IncomeType="EARNING", Code = "BS", Description = "Basic Salary",Tax=true },
                new clsOrderPayment { Groups = "B",IncomeType="EARNING", Code = "OT", Description = "Overtime",Tax=true },
                new clsOrderPayment { Groups = "C",IncomeType="EARNING", Code = "Allowance", Description = "Allowance" , Tax = true},
                new clsOrderPayment { Groups = "D",IncomeType="EARNING", Code = "Bonus", Description = "Bonus",Tax=true },
                new clsOrderPayment { Groups = "E",IncomeType="DEDUCTIONS", Code = "LD", Description = "Leave Deduction",Tax=true },
                new clsOrderPayment { Groups = "F",IncomeType="DEDUCTIONS", Code = "Deduction", Description = "Deduction",Tax=true },
                new clsOrderPayment { Groups = "G",IncomeType="TAX", Code = "Exception", Description = "Exception" },
                new clsOrderPayment { Groups = "H",IncomeType="TAX", Code = "TAX", Description = "AmountBeforeTax" },
                new clsOrderPayment { Groups = "H",IncomeType="TAX", Code = "TAX", Description = "Tax" },
                new clsOrderPayment { Groups = "I",IncomeType="OTHB", Code = "Allowance", Description = "Other Benefit" },
                new clsOrderPayment { Groups = "J",IncomeType="EARNING", Code = "Bonus", Description = "Bonus"},
                new clsOrderPayment { Groups = "K",IncomeType="DEDUCTIONS", Code = "Deduction", Description = "Deduction" },
                new clsOrderPayment { Groups = "K",IncomeType="INCOME", Code = "GP", Description = "Gross Pay" },
                new clsOrderPayment { Groups = "L",IncomeType="DEDUCTIONS", Code = "Deduction", Description = "SSB" },
                new clsOrderPayment { Groups = "M1",IncomeType="DEDUCTIONS", Code = "FB", Description = "FringeBenefit" },
                new clsOrderPayment { Groups = "M2",IncomeType="DEDUCTIONS", Code = "First", Description = "First" },
                new clsOrderPayment { Groups = "M3",IncomeType="INCOME", Code = "NETPAY", Description = "Net Pay" },
            };
        }
    }
}