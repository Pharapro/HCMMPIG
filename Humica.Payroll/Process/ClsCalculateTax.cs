using Humica.Core;
using Humica.Core.DB;
using Humica.Core.Helper;
using Humica.EF;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Humica.Payroll
{
    public class ClsCalculateTax
    {
        public decimal AmountBeforeTax { get; set; }
        public decimal AmountBeforeTaxBase { get; set; }
        public decimal TaxRate { get; set; }
        public decimal FringeBenefit { get; set; }
        public bool IsFBEmployee { get; set; }
        public decimal Exclude_Family { get; set; }
        public ClsCalculateTax()
        {
        }
        public void Calculate_Benefit(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            var ListEmpPayment = _filter.ListHisEmpPayment.ToList();
            var ObjSalary = _filter.HeaderSalary;

            //ObjSalary.OTAM = ListEmpPayment.Where(w => w.PayType == "Overtime").Sum(w => w.Amount);
            ObjSalary.TaxALWAM = ListEmpPayment.Where(w => w.PayType == "Allowance" && w.TaxType== "TAX").Sum(w => w.Amount);
            ObjSalary.TAXBONAM = ListEmpPayment.Where(w => w.PayType == "Bonus" && w.TaxType== "TAX").Sum(w => w.Amount);
            ObjSalary.TAXDEDAM = -ListEmpPayment.Where(w => w.PayType == "Deduction" && w.TaxType== "TAX").Sum(w => w.Amount);

            ObjSalary.UTAXALWAM = ListEmpPayment.Where(w => w.PayType == "Allowance" && w.TaxType != "TAX").Sum(w => w.Amount);
            ObjSalary.UTAXBONAM = ListEmpPayment.Where(w => w.PayType == "Bonus" && w.TaxType != "TAX").Sum(w => w.Amount);
            ObjSalary.UTAXDEDAM = -ListEmpPayment.Where(w => w.PayType == "Deduction" && w.TaxType != "TAX").Sum(w => w.Amount);

            ObjSalary.LeaveDeduct = -ListEmpPayment.Where(w => w.PayType == "Leave Deduction" && w.TaxType== "TAX").Sum(w => w.Amount);

            var GrossPay = ListEmpPayment.Sum(w => w.Amount.Value) - ListEmpPayment.Where(w => w.PayType == "Deduction" && w.TaxType != "TAX").Sum(x => x.Amount.Value);
            ObjSalary.GrossNoTIP = GrossPay;
            ObjSalary.GrossPay = GrossPay;

            Exclude_Family = ExceptionTax(_filter, _FPayrllMaster);

            // _filter.ListHisEmpPayment.Add(clsEmpReward.EmpPayment(_filter, "Gross Pay", "GP", "Gross Pay", GrossPay));
        }
        public void CalculateSocialSecurity(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            var ObjSalary = _filter.HeaderSalary;
            decimal? SalaryNSSF = ObjSalary.Salary;
            CalculateSocialSecurity(_filter, _FPayrllMaster, SalaryNSSF);
        }
        public decimal Get_Amount_Before_Tax(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster, decimal ExchRate)
        {
            var ObjSalary = _filter.HeaderSalary;
            decimal TotalTaxableIncome = _filter.ListHisEmpPayment.Where(w => w.TaxType == "TAX").Sum(w => w.Amount.Value);
            AmountBeforeTax = _filter.ListHisEmpPayment.Where(w => w.TaxType == "TAX").Sum(w => w.Amount.Value);
            var AmountFringeBenefit = _filter.ListHisEmpPayment.Where(w => w.TaxType == "FTAX").Sum(w => w.Amount.Value);
            decimal SeniorityTaxable = 0;
            if (ObjSalary.SeniorityTaxable.HasValue && ObjSalary.SeniorityTaxable > 0)
            {
                SeniorityTaxable = ObjSalary.SeniorityTaxable.Value;
                AmountBeforeTax += SeniorityTaxable;
                TotalTaxableIncome += SeniorityTaxable;
            }
            decimal RealAmountTobeTax = (AmountBeforeTax * _filter.SalaryBase * ExchRate);
            if (_filter.Staff.CompanyCode == "LEGB")
            {
                RealAmountTobeTax += (AmountFringeBenefit * ExchRate);
            }
            return RealAmountTobeTax;
        }
        public void Calculate_Amount_Before_Tax(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            var ListEmpPayment = _filter.ListHisEmpPayment.ToList();
            var ObjSalary = _filter.HeaderSalary;

            FringeBenefit = ListEmpPayment.Where(w => w.TaxType == "FTAX").Sum(w => w.Amount.Value);

            //decimal Exclude_Family = ExceptionTax(_filter, _FPayrllMaster);

            decimal TotalTaxableIncome = _filter.ListHisEmpPayment.Where(w => w.TaxType == "TAX").Sum(w => w.Amount.Value);
            AmountBeforeTax = _filter.ListHisEmpPayment.Where(w => w.TaxType == "TAX").Sum(w => w.Amount.Value);
            decimal SeniorityTaxable = 0;
            if (ObjSalary.SeniorityTaxable.HasValue && ObjSalary.SeniorityTaxable > 0)
            {
                SeniorityTaxable = ObjSalary.SeniorityTaxable.Value;
                AmountBeforeTax += SeniorityTaxable;
                TotalTaxableIncome += SeniorityTaxable;
            }
            decimal RealAmountTobeTax = (AmountBeforeTax * _filter.SalaryBase * ObjSalary.ExchRate ?? 0) - Exclude_Family;
            //RealAmountTobeTax = RealAmountTobeTax;
            var Cal_tax = TaxCalculator(_FPayrllMaster, RealAmountTobeTax, _FPayrllMaster.ListTaxSetting, _filter.Staff.IsResident);

            TotalTaxableIncome = TotalTaxableIncome * _filter.SalaryBase;
            ObjSalary.TotalTaxableIncome = TotalTaxableIncome;
            Cal_tax = Cal_tax / _filter.SalaryBase;
            var TaxUSD = Cal_tax / ObjSalary.ExchRate.Value;
            TaxUSD = clsEmpReward.RoundReward(_FPayrllMaster, TaxUSD, "TAX");
            ObjSalary.TAXAM = TaxUSD;
            ObjSalary.AmtoBeTaxKH = RealAmountTobeTax;
            AmountBeforeTax = RealAmountTobeTax / ObjSalary.ExchRate.Value;
            ObjSalary.AMTOBETAX = AmountBeforeTax;
            ObjSalary.TAXRATE = TaxRate;
            _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter,_FPayrllMaster ,"TTI", "Tax", "Total Taxable Income", TotalTaxableIncome,false, ""));
            //_filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter,_FPayrllMaster , "TAX", "Tax", "Amount Before Tax", AmountBeforeTax));
            bool IsSalary = false;
            if (ObjSalary.TXPayType == "S") IsSalary = true;
            _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "TAX", "Tax", "Income Tax", -TaxUSD, IsSalary, ""));
        }
        public decimal Cal_NetPay(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            var ObjSalary = _filter.HeaderSalary;
            decimal? NetPay = 0;
            decimal AmFringRate = 0.2M;
            ObjSalary.FRINGRATE = 20;
            decimal? AdvpayAm = ObjSalary.ADVPay;
            if (_FPayrllMaster.ListFringeBenefitSetting.Where(w => w.EmployeeType == _filter.Staff.EmpType).Any())
            {
                var Fringe = _FPayrllMaster.ListFringeBenefitSetting.FirstOrDefault(w => w.EmployeeType == _filter.Staff.EmpType);
                if (Fringe != null)
                {
                    AmFringRate = Fringe.Rate / 100;
                    ObjSalary.FRINGRATE = AmFringRate;
                    IsFBEmployee = Fringe.IsEmployee;
                    if (Fringe.IsEmployee && Fringe.IsSalaryPaid && _filter.Staff.TXPayType == "S")
                    {
                        IsFBEmployee = true;
                    }
                    else if (Fringe.IsEmployee && !Fringe.IsSalaryPaid)
                    {
                        IsFBEmployee = true;
                    }
                    else
                    {
                        IsFBEmployee = false;
                    }
                }
            }
            ObjSalary.AMFRINGTAX = _filter.ListHisEmpPayment.Where(w => w.TaxType == "FTAX").Sum(w => w.Amount.Value);
            ObjSalary.FRINGAM = ObjSalary.AMFRINGTAX * AmFringRate;
            NetPay = _filter.ListHisEmpPayment.Where(w => w.IsSalary == true).Sum(x => x.Amount.Value);
            NetPay = NetPay - (ObjSalary.LOAN.Value + ObjSalary.ADVPay.Value);
            //NetPay = (ObjSalary.GrossPay - AdvpayAm - ObjSalary.LOAN - ObjSalary.TAXDEDAM - ObjSalary.UTAXDEDAM);
            return NetPay.Value;
        }

        public decimal ExceptionTax(ClsSalary _filter, ClsFilterPayroll _FPayrllMaster)
        {
            ClsEmpReward clsEmpReward = new ClsEmpReward();
            decimal ExchangeRate = _filter.ExchangeRate.Value;
            decimal tax = 0;
            var ListFamily = _FPayrllMaster.ListEmpFamily.Where(w => w.EmpCode == _filter.EmpCode).ToList();
            var ListExclude = new Dictionary<string, int>();
            foreach (var Family in ListFamily)
            {
                string key = Family.Spouse ? "Spouse" :
                     Family.Child ? "Child" : null;
                if (key != null)
                {
                    if (ListExclude.TryGetValue(key, out int count))
                        ListExclude[key] = count + 1;
                    else
                        ListExclude[key] = 1;
                }
                else if (_FPayrllMaster.ListExceptType.Where(w => w.Code == Family.OtherExceptType).ToList().Any())
                {
                    var objFam = _FPayrllMaster.ListExceptType.FirstOrDefault(w => w.Code == Family.OtherExceptType);
                    if (ListExclude.TryGetValue(objFam.ExceptType, out int count))
                        ListExclude[objFam.ExceptType] = count + 1;
                    else
                        ListExclude[objFam.ExceptType] = 1;
                }
            }
            foreach (var _Exclude in ListExclude)
            {
                string Code = _Exclude.Key;
                decimal deductionPerUnit = 0;
                decimal AmountDed = 0;
                switch (_Exclude.Key)
                {
                    case "Spouse":
                        deductionPerUnit = _FPayrllMaster.PayrollSetting.Spouse ?? 0;
                        break;
                    case "Child":
                        deductionPerUnit = _FPayrllMaster.PayrollSetting.Child ?? 0;
                        break;
                }
                if (deductionPerUnit == 0)
                {
                    var objFam = _FPayrllMaster.ListExceptType.FirstOrDefault(w => w.ExceptType == _Exclude.Key);
                    if (objFam != null)
                    {
                        Code = objFam.Code;
                        decimal Amount = objFam.Amount;
                        if (objFam.ValueBased == "AMOUNT")
                        {
                            AmountDed = (_Exclude.Value * objFam.Amount) / ExchangeRate;
                        }
                        else if (objFam.ValueBased == "RATE")
                        {
                            decimal Salary = _filter.HeaderSalary.Salary.Value * ExchangeRate;
                            Salary = Salary * _filter.SalaryBase;
                            decimal Rate = objFam.Amount;
                            if (objFam.Amount > 1) Rate = Rate / 100.00M;
                            Amount = Salary * Rate;
                            AmountDed = ((_Exclude.Value * Amount) / ExchangeRate) ;
                        }
                        tax += _Exclude.Value * Amount;
                    }
                }
                else
                {
                    AmountDed = (_Exclude.Value * deductionPerUnit) / ExchangeRate;
                    tax += _Exclude.Value * deductionPerUnit;
                }

                _filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(_filter, _FPayrllMaster, "EXCEP",
                    Code, _Exclude.Key, -AmountDed, false, ""));
            }
            return tax;
        }
        public void CalculatePensionFund(ClsSalary filter, ClsFilterPayroll _FPayrllMaster, decimal? Salary)
        {
            filter.HeaderSalary.StaffPensionFundRate = 0;
            filter.HeaderSalary.StaffPensionFundAmount = 0;
            filter.HeaderSalary.StaffPensionFundAmountKH = 0;
            filter.HeaderSalary.CompanyPensionFundRate = 0;
            filter.HeaderSalary.CompanyPensionFundAmount = 0;
            filter.HeaderSalary.CompanyPensionFundAmountKH = 0;
            filter.HeaderSalary.SOSEC = 0;
            filter.HeaderSalary.CompHealth = 0;
            filter.HeaderSalary.StaffRiskKH = 0;
            filter.HeaderSalary.StaffRisk = 0;
            filter.HeaderSalary.TotalRisk = 0;
            filter.HeaderSalary.StaffHealthCareUSD = 0;
            filter.HeaderSalary.TotalHealthCare = 0;
            //Get employee service length
            if (Salary.Value > 0)
            {
                EmploymentInfo empInfo = new EmploymentInfo();
                DateTime FromDate = filter.Staff.StartDate.Value;
                DateTime ToDate = new DateTime(filter.ToDate.Year, filter.ToDate.Month, 1);
                double serviceLength = empInfo.GetEmploymentServiceLength(FromDate, filter.ToDate, ServiceLengthType.Month);
                double EmpLength = empInfo.GetEmploymentServiceLength(filter.Staff.DOB.Value, ToDate, ServiceLengthType.Month);

                decimal? maxContribution = 1200000;
                decimal? minContribution = 400000;
                decimal? basicSalary = 0;
                decimal? ExchangeRate = filter.HeaderSalary.NSSFRate;
                //Get Pension Fund Setting
                List<PRPensionFundSetting> lists = _FPayrllMaster.ListPensionFund.ToList();
                var list = lists.Where(x => x.SeviceLenghtFrom <= serviceLength & x.SeviceLenghtTo >= serviceLength).FirstOrDefault();

                var _Setting = _FPayrllMaster.HRSetting;
                if (_Setting != null)
                {
                    maxContribution = _Setting.MaxContribute;
                    minContribution = _Setting.MinContribue;
                }
                if (filter.Staff.ISNSSF == true)
                {
                    decimal? Rate = 0;
                    decimal? _RateUSD = 0;
                    //if (filter.HeaderSalary != null && (filter.Staff != null && filter.Staff.NSSFContributionType == "RHP"))
                    if (filter.HeaderSalary != null && (filter.Staff != null))
                    {
                        basicSalary = Salary;

                        //Convert local currency to foreign currency
                        if ((EmpLength / 12.00) < 60)
                        {

                            basicSalary = basicSalary * ExchangeRate;// filter.HeaderSalary.ExchRate;
                            if (basicSalary > maxContribution) basicSalary = maxContribution;
                            else if (basicSalary < minContribution) basicSalary = minContribution;

                            if (list != null && list.StaffPercentage > 0)
                            {
                                Rate = ((basicSalary * list.StaffPercentage) / 100);
                                filter.HeaderSalary.StaffPensionFundRate = list.StaffPercentage;
                                filter.HeaderSalary.StaffPensionFundAmount = (Rate / ExchangeRate).Value();
                                filter.HeaderSalary.StaffPensionFundAmountKH = Rate;
                            }
                            if (list != null && list.ComPercentage > 0)
                            {
                                Rate = (basicSalary * list.ComPercentage) / 100;
                                filter.HeaderSalary.CompanyPensionFundRate = list.ComPercentage;
                                filter.HeaderSalary.CompanyPensionFundAmount = (Rate / ExchangeRate).Value();
                                filter.HeaderSalary.CompanyPensionFundAmountKH = Rate;
                            }
                            filter.HeaderSalary.AVGGrSOSC = basicSalary;
                            filter.HeaderSalary.CompanyPensionFundAmountKH = Math.Round((decimal)filter.HeaderSalary.CompanyPensionFundAmountKH, 0);
                            filter.HeaderSalary.StaffPensionFundAmountKH = Math.Round((decimal)filter.HeaderSalary.StaffPensionFundAmountKH, 0);
                        }
                    }

                    filter.HeaderSalary.AVGGrSOSC = basicSalary;
                    if (_Setting.StaffRisk.HasValue)
                    {
                        Rate = (decimal)(basicSalary * _Setting.StaffRisk);
                        _RateUSD = (Rate.Value / ExchangeRate).Value();
                        filter.HeaderSalary.StaffRiskKH = Math.Round(Rate.Value, 0);
                        filter.HeaderSalary.StaffRisk = ClsRounding.Rounding(_RateUSD.Value, SYConstant.DECIMAL_PLACE, "N");
                        filter.HeaderSalary.TotalRisk = filter.HeaderSalary.StaffRisk;
                    }
                    if (_Setting.StaffHealthCare.HasValue)
                    {
                        Rate = (decimal)(basicSalary * _Setting.StaffHealthCare);
                        _RateUSD = (Rate.Value / ExchangeRate).Value();
                        filter.HeaderSalary.StaffHealth = Math.Round(Rate.Value, 0);
                        filter.HeaderSalary.StaffHealthCareUSD = ClsRounding.Rounding(_RateUSD.Value, SYConstant.DECIMAL_PLACE, "N");
                        filter.HeaderSalary.TotalHealthCare = filter.HeaderSalary.StaffHealthCareUSD;
                    }
                }
            }
        }
        public decimal Calculate_TaxDeduction(decimal income, List<PRTaxSetting> listTax, bool isForeigner = false)
        {
            TaxRate = 0;
            if (isForeigner) return income * 0.2m;
            decimal TaxDeduct = 0.0m;
            foreach (var Rate in listTax)
            {
                if (income > Rate.TaxFrom && income <= Rate.TaxTo)
                {
                    TaxRate = Rate.TaxPercent ?? 0;
                    decimal AmountDeduct = Rate.Amdeduct ?? 0;
                    TaxDeduct = (income) * (Rate.TaxPercent.Value / 100) - AmountDeduct;
                    break;
                }
            }
            return TaxDeduct;
        }
        public void CalculateSocialSecurity(ClsSalary filter, ClsFilterPayroll _FPayrllMaster, decimal? Salary)
        {
            //Get employee service length
            if (Salary.Value > 0 && filter.Staff.ISNSSF == true)
            {
                EmploymentInfo empInfo = new EmploymentInfo();
                DateTime FromDate = filter.Staff.StartDate.Value;
                DateTime ToDate = new DateTime(filter.ToDate.Year, filter.ToDate.Month, 1);
                double serviceLength = empInfo.GetEmploymentServiceLength(FromDate, filter.ToDate, ServiceLengthType.Month);
                double EmpLength = DateTimeHelper.GetServiceLength(filter.Staff.DOB.Value, ToDate, ServiceLengthType.Month);
                double EmpLength1 = DateTimeHelper.GetServiceLength(filter.Staff.DOB.Value, ToDate, ServiceLengthType.Year);
                var AgeOfStaff = EmpLength / 12.00;
                decimal? ExchangeRate = filter.HeaderSalary.NSSFRate;
                decimal? basicSalary = Salary * ExchangeRate;
                decimal AmountSoSe = 0;
                //string IsTax = "TAX";
                string IsTax = "";
                foreach (var Item in _FPayrllMaster.ListSocialSecurity)
                {
                    if (Item.SalaryType == SalaryType.NP.ToString())
                    {
                        basicSalary = Get_Amount_Before_Tax(filter, _FPayrllMaster, ExchangeRate.Value);
                        IsTax = "TAX";
                    }
                    else if (Item.SalaryType == SalaryType.BS.ToString())
                    {
                        IsTax = "TAX";
                    }
                    else if (Item.SalaryType != SalaryType.BS.ToString())
                    {
                        if (filter.Staff.CompanyCode == "LEGB")
                        {
                            basicSalary = (filter.HeaderSalary.GrossPay + filter.ListHisEmpPayment.Where(w => w.TaxType == "FTAX").Sum(w => w.Amount.Value)) * ExchangeRate;
                        }
                        else
                            basicSalary = filter.HeaderSalary.GrossPay * ExchangeRate;
                    }
                     
                    decimal? SalarySoSe = basicSalary;
                    int Age = Item.Age ?? 0;
                    decimal Rate = Item.Amount;

                    if (basicSalary > Item.Maximum) SalarySoSe = Item.Maximum;
                    else if (basicSalary < Item.Minimum) SalarySoSe = Item.Minimum;

                    if (Age == 0 || AgeOfStaff < Age)
                    {
                        if (Item.IsValueBased == "AMOUNT")
                        {
                            AmountSoSe = Rate;
                        }
                        else if (Item.IsValueBased == "RATE")
                        {
                            AmountSoSe = (SalarySoSe.Value * Rate) / 100;
                        }
                    }
                    decimal AmountBase = ClsRounding.Rounding(AmountSoSe / ExchangeRate.Value, 2, "N");
                    if (AmountSoSe > 0 && Item.IsEmpoyee == true)
                    {
                        ClsEmpReward clsEmpReward = new ClsEmpReward();
                        filter.ListHisEmpPayment.Add(clsEmpReward.Emp_Payment(filter, _FPayrllMaster, Item.IncomeType, Item.Code, Item.Description, -AmountBase, true, IsTax));
                        filter.SalarySoSe += (AmountSoSe * filter.SalaryBase);
                    }
                    filter.ListEmpSoSe.Add(new HisEmpSocialSecurity()
                    {
                        CompanyCode = filter.CompanyCode,
                        PeriodID = filter.Period,
                        EmpCode = filter.EmpCode,
                        Code = Item.Code,
                        Description = Item.Description,
                        IsEmpoyee = Item.IsEmpoyee,
                        ExchangeRate = filter.NSSFRate,
                        Amount = AmountBase,
                        AmountBasic = AmountSoSe,
                    });
                }
            }
        }
        public decimal TaxCalculator(ClsFilterPayroll _FPayrllMaster,decimal income, List<PRTaxSetting> lstTaxSetting, bool IsResident = false)
        {
            decimal totalTax = 0;
            decimal remainingIncome = income;
            if (!IsResident)
            {
                TaxRate = 20;
                return income * 0.2m;
            }
            foreach (var bracket in lstTaxSetting.OrderBy(w => w.TaxFrom))
            {
                decimal Rate = bracket.TaxPercent ?? 0;
                if (Rate > 1)
                {
                    Rate = Rate / 100.00M;
                }
                if (income > bracket.TaxFrom)
                {
                    decimal taxableAmount = Math.Min(income, bracket.TaxTo.Value) - bracket.TaxFrom.Value + 1;
                    decimal tax = taxableAmount * Rate;
                    totalTax += tax;
                    TaxRate = bracket.TaxPercent ?? 0;
                    if (income <= bracket.TaxTo.Value)
                        break;
                }
            }

            return totalTax;
        }
    }
}