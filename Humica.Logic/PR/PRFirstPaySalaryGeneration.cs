using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Logic.CF;
using Humica.Logic.LM;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.IO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Humica.Logic.PR
{
    public class PRFirstPaySalaryGeneration
    {
        public HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public SYUser User = new SYUser();

        private SMSystemEntity DMS = new SMSystemEntity();
        public string MessageError { get; set; }
        public string MessageCode { get; set; }
        public List<HR_STAFF_VIEW> ListStaff { get; set; }
        public string EmpID { get; set; }
        public int Progress { get; set; }
        public string ScreenId { get; set; }
        public ClsEmail EmailObject { get; set; }
        public SYUserBusiness BS { get; set; }
        public FTFilterEmployee Filter { get; set; }
        public HISGenFirstPay HeaderSalary { get; set; }
        public List<HISGenFirstPaySalaryD> ListBasicSalary { get; set; }
        public List<HisGenOTFirstPay> ListHisGenOTFirstPay { get; set; }
        public List<HisGenLeaveDFirstPay> ListHisGenLeaveDFirstPay { get; set; }
        public static List<ListProgress> ListProgress { get; set; }
        public List<HR_View_EmpGenSalary> ListEmployeeGen { get; set; }
        public List<HISPaySlipFirstPay> ListPaySlip { get; set; }
        public List<ExCfWFSalaryApprover> SalaryApprover { get; set; }
        public List<SalaryApprovalFP> ListSalaryApproveFp { get; set; }
        public List<LeaveDeduction> ListLeaveDed { get; set; }
        public List<HISApproveSalaryFD> ListApproveSalary { get; set; }
        public List<HisPendingAppSalaryFP> ListAppSalaryPending { get; set; }
        public List<ExDocApproval> ListApproval { get; set; }
        public HISApproveSalaryFD HeaderAppSalary { get; set; }
        public PRFirstPaySalaryGeneration()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        //public string GenerateSalaryFirstPay(string EmpCode)
        //{
        //    DB = new HumicaDBContext();
        //    string Result = "";
        //    try
        //    {
        //        var LstExchangeRate = DB.PRBiExchangeRates.ToList();
        //        var ExchangeRate = LstExchangeRate.FirstOrDefault(w => w.InYear == Filter.InMonth.Year & w.InMonth == Filter.InMonth.Month);
        //        if (ExchangeRate == null)
        //            return "EXCHANGERATE";
        //        var LstAppSalary = DB.HisPendingAppSalaryFPs.Where(w => w.InMonth == Filter.InMonth.Month && w.InYear == Filter.InMonth.Year).ToList();
        //        if (LstAppSalary.Where(w => w.IsLock == true).Count() > 0)
        //        {
        //            return "APPROVE_SALARY";
        //        }
        //        var emp = DB.HRStaffProfiles.Where(w => w.EmpCode == EmpCode & w.PayParam != "");

        //        if (emp == null) return "";

        //        var prFiscalYear = DB.PRFiscalYears.Where(x => x.InMonth.Value == Filter.InMonth).FirstOrDefault();
        //        //var _listPayHis = DB.HISPaySlipFirstPays.Where(w => w.INYear == Filter.InMonth.Year & w.INMonth == Filter.InMonth.Month).ToList();
        //        var _lstPara = DB.PRParameters.ToList();
        //        var _lstRewardType = DB.PR_RewardsType.ToList();
        //        var _lstBankFee = DB.PRBankFees.ToList();
        //        var ListBranch = DMS.HRBranches.ToList();

        //        string CLEARED = SYDocumentStatus.CLEARED.ToString();
        //        var _lstCateCode = DB.HRCareerHistories.Where(w => w.NotCalSalary == true).ToList();
        //        bool IsTax = false;
        //        Filter.ExchangeRate = ExchangeRate.Rate;
        //        if (Filter.ExchangeRate > 0)
        //        {
        //            string[] Emp = EmpCode.Split(';');
        //            Progress = Emp.Count();
        //            //_lstStaff = _lstStaff.ToList();
        //            _lstPara = _lstPara.ToList();
        //            //_lstEmpCareer = _lstEmpCareer.ToList();
        //            _lstRewardType = _lstRewardType.ToList();
        //            _lstBankFee = _lstBankFee.ToList();
        //            var ListTaxSetting = DB.PRTaxSettings.ToList();
        //            int i = 0;
        //            var SYData = DMS.SYDatas.Where(w => w.DropDownType == "PR_YEAR_SELECT").ToList();
        //            if (SYData.Where(w => w.SelectValue == Filter.InMonth.Year.ToString()).Count() == 0)
        //            {
        //                var _sydata = new SYData()
        //                {
        //                    SelectText = Filter.InMonth.Year.ToString(),
        //                    SelectValue = Filter.InMonth.Year.ToString(),
        //                    DropDownType = "PR_YEAR_SELECT",
        //                    IsActive = true
        //                };
        //                DMS.SYDatas.Add(_sydata);
        //                DMS.SaveChanges();
        //            }
        //            decimal _p = 0;

        //            ListProgress.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Progress = Progress);
        //            bool IsGenerate = false;
        //            foreach (var Code in Emp)
        //            {
        //                if (Code.Trim() != "")
        //                {
        //                    Result = Code;
        //                    GEN_Filter1 _Filter = new GEN_Filter1();
        //                    _Filter.Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Code);
        //                    _Filter.Parameter = _lstPara.FirstOrDefault(w => w.Code == _Filter.Staff.PayParam);
        //                    if (_Filter.Parameter == null)
        //                        continue;

        //                    _Filter.BiMonthlySalarySetting = DB.BiMonthlySalarySettings.Where(x => x.PayrollParameterID == _Filter.Parameter.Code).FirstOrDefault();
        //                    DateTime FromDate = Filter.InMonth.Date;
        //                    DateTime ToDate = DateTimeHelper.DateInMonth(Filter.InMonth);
        //                    var biMonthlySalary = DB.BiMonthlySalarySettings.Where(x => x.PayrollParameterID == _Filter.Parameter.Code).FirstOrDefault();
        //                    if (prFiscalYear != null)
        //                    {
        //                        FromDate = prFiscalYear.StartDate.Value;
        //                        ToDate = prFiscalYear.EndDate.Value;
        //                    }
        //                    if (_Filter.BiMonthlySalarySetting.IsAccrualTax == 1)
        //                    {
        //                        if (ExchangeRate == null)
        //                            return "EXCHANGERATE";
        //                    }
        //                    else
        //                {
        //                    _Filter.ExchangeRate = 1;
        //                }
        //                    int num = 15;
        //                    _Filter.PayFrom = FromDate;
        //                    if (biMonthlySalary != null && biMonthlySalary.FirstEndDay > 0)
        //                        num = biMonthlySalary.FirstEndDay.Value;
        //                    _Filter.PayTo = _Filter.PayFrom.AddDays(num).AddDays(-1);


        //                    _Filter.LstEmpCareer = new List<HREmpCareer>();
        //                    _Filter.LstRewardsType = new List<PR_RewardsType>();
        //                    _Filter.LstEmpHold = new List<PREmpHold>();
        //                    _Filter.EmpCode = Code;
        //                    _Filter.InYear = Filter.InMonth.Year;
        //                    _Filter.InMonth = Filter.InMonth.Month;
        //                    _Filter.FromDate = FromDate;
        //                    _Filter.ToDate = ToDate;
        //                    _Filter.LstPayHis = new List<ClsPayHis>();
        //                    _Filter.HisGenFirstPay = new HISGenFirstPay();
        //                    _Filter.ListHisEmpOTFirstPay = new List<HisGenOTFirstPay>();
        //                    _Filter.ListHisEmpLeaveFirstPay = new List<HisGenLeaveDFirstPay>();
        //                    _Filter.ListTaxSetting = new List<PRTaxSetting>();
        //                    _Filter.LstEmpCareer = DB.HREmpCareers.Where(w => w.EmpCode == _Filter.Staff.EmpCode).ToList();
        //                    _Filter.LstRewardsType = _lstRewardType.ToList();
        //                    _Filter.LstBankFee = _lstBankFee.ToList();
        //                    _Filter.ListTaxSetting = ListTaxSetting.ToList();
        //                    _Filter.LstEmpAllow = DB.PREmpAllws.Where(w => w.EmpCode == Code).ToList();
        //                    _Filter.LstEmpBon = DB.PREmpBonus.Where(w => w.EmpCode == Code).ToList();
        //                    var Branch = ListBranch.Where(w => w.Code == _Filter.Staff.Branch).FirstOrDefault();
        //                    _Filter.HisAppSalary = LstAppSalary.FirstOrDefault(w => w.IsLock == false);
        //                    Delete_Generate(_Filter);
        //                    Generate_Salary(_Filter, Branch, _lstCateCode, ref IsGenerate);
        //                    if (_Filter.HisGenFirstPay.EmpCode != null)
        //                    {
        //                        Calculate_Overtime(_Filter, Branch);
        //                        Calculate_Allowance(_Filter);
        //                        Calculate_Bonus(_Filter);
        //                        Calculate_Deduction(_Filter);
        //                        Calculate_LeaveDeduction(_Filter, Branch);
        //                        CalculateTax(_Filter, true, Branch, IsTax);
        //                        Commit_PaySlip(_Filter, Branch);
        //                    }
        //                }
        //                i += 1;
        //                _p = i;
        //                ListProgress.Where(w => w.UserName == User.UserName).ToList().ForEach(x => x.Percen = _p / x.Progress * 100);
        //            }
        //        }
        //        else
        //        {
        //            return "EXCHANGERATE";
        //        }
        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Result;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Result;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Result;
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}

        public int GetLineTranNo(string EmpCode, int InYear, int InMonth, int Status)
        {
            int Result = 0;
            if (Status == 1)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.EarnDesc == "EARNING").ToList();

                if (PaySlips.Count > 0) Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            else if (Status == 2)
            {
                var PaySlips = ListPaySlip.Where(w => w.EmpCode == EmpCode && w.INYear == InYear && w.INMonth == InMonth && w.DeductDesc == "DEDUCTIONS").ToList();

                if (PaySlips.Count > 0) Result = Convert.ToInt32(PaySlips.Min(w => w.TranLine));
            }
            return Result;
        }

        public void Commit_PaySlip(GEN_Filter1 _filter, HRBranch _Branch)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                string SalaryDesc = "";

                ListPaySlip = new List<HISPaySlipFirstPay>();
                var Gen_salry = _filter.HisGenFirstPay;
                var _listLeave = _filter.ListHisEmpLeaveFirstPay;
                var _listMaternity = _listLeave.Where(x => x.LeaveCode == "ML").ToList();
                var OverTime_G = _filter.ListHisEmpOTFirstPay;
                var LeaveDed_G = _listLeave.Where(x => x.LeaveCode != "ML").ToList();
                var OTType = DB.PROTRates.ToList();
                DateTime payFrom = _filter.PayFrom;
                DateTime payTo = _filter.PayTo;
                int TranLine = 1;
                for (int i = 1; i <= 13; i++)
                {
                    var Gen = new HISPaySlipFirstPay()
                    {
                        TranLine = i,
                        EmpCode = _filter.EmpCode,
                        INYear = _filter.InYear,
                        INMonth = _filter.InMonth,
                        EarnDesc = "EARNING",
                        EAmount = 0,
                        DeductDesc = "DEDUCTIONS",
                        DeductAmount = 0
                    };
                    ListPaySlip.Add(Gen);
                }
                decimal Maternity = 0;
                if (_listMaternity.Count > 0) Maternity = _listMaternity.Sum(x => x.Amount).Value;
                if (Maternity > 0) SalaryDesc = "Basic Pay - Maternity Leave";
                else SalaryDesc = "Basic First Pay";
                var Salary = Gen_salry.Salary - Math.Round(Maternity, SYConstant.DECIMAL_PLACE);
                TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Salary);
                ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = SalaryDesc);
                
                //---------OverTime-------
                var G_Over = from F in OverTime_G
                             group F by new { F.OTCode, F.OTDesc, F.OTRate } into myGroup
                             select new
                             {
                                 myGroup.Key.OTCode,
                                 OTHour = myGroup.Sum(w => w.OTHour),
                                 myGroup.Key.OTRate,
                                 Amount = myGroup.Sum(w => w.Amount)
                             };
                foreach (var item in G_Over)
                {
                    string Desc = "DAYS";
                    string StrDes = "";
                    var Type = OTType.FirstOrDefault(w => w.OTCode == item.OTCode);
                    if (Type != null)
                    {
                        if (Type.Measure == "H") Desc = "HOURS";
                        StrDes = Type.OTType + "(" + item.OTHour + " " + Desc + ") *" + Math.Round(item.OTRate.Value, 2);
                    }
                    else
                    {
                        Desc = "HOURS";
                        StrDes = "OT Claim";
                    }
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = StrDes);
                }
                //---------PayBack------------
                var Payback = Gen_salry.PayBack;
                if (Payback > 0)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 1);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EAmount = Math.Round(Convert.ToDecimal(Payback), SYConstant.DECIMAL_PLACE));
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.EarnDesc = "Pay Back");
                }
                //---------Leave Deduction------------
                var G_Leave = from F in LeaveDed_G
                              group F by new { F.LeaveDesc } into myGroup
                              select new
                              {
                                  myGroup.Key.LeaveDesc,
                                  Amount = myGroup.Sum(w => w.Amount),
                              };
                foreach (var item in G_Leave)
                {
                    var Amount = Math.Round(Convert.ToDecimal(item.Amount), SYConstant.DECIMAL_PLACE);
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Amount);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = item.LeaveDesc);
                }
                //---------Loan------------
                var Loan_value = Gen_salry.LOAN;
                if (Loan_value > 0)
                {
                    Loan_value = Math.Round(Convert.ToDecimal(Loan_value), SYConstant.DECIMAL_PLACE);
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = Loan_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Loan");
                }
                //---------Advance------------
                var ADVPay_value = Gen_salry.ADVPay;
                if (ADVPay_value > 0)
                {
                    TranLine = GetLineTranNo(_filter.EmpCode, _filter.InYear, _filter.InMonth, 2);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductAmount = ADVPay_value);
                    ListPaySlip.Where(w => w.EmpCode == _filter.EmpCode && w.TranLine == TranLine).ToList().ForEach(w => w.DeductDesc = "Advance Pay");
                }
                foreach (var item in ListPaySlip)
                {
                    DB.HISPaySlipFirstPays.Add(item);
                }
                var row1 = DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void CalculateTax(GEN_Filter1 _filter, bool TaxType, HRBranch _Branch, bool IsTax)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                string CareerCode = _filter.Staff.CareerDesc;
                decimal NetAmount = 0;
                decimal GrossPay = 0;
                decimal AmountToBeTax = 0;
                decimal AdvPayAmout = 0;
                decimal TaxKH = 0;
                decimal TaxUSD = 0;
                decimal TaxRate = 0;
                decimal TaxExemptionAmount = 0;

                AdvPayAmout = Convert.ToDecimal(DB.PRADVPays.Where(w => w.EmpCode == _filter.EmpCode && (w.TranDate >= _filter.FromDate && w.TranDate <= _filter.ToDate)).ToList().Sum(x => x.Amount));
                AdvPayAmout = AdvPayAmout / 2;

                var hisGenSalary = _filter.HisGenFirstPay;
                decimal ExchangeRate = Convert.ToDecimal(hisGenSalary.ExchRate);
                decimal RealAmountTobeTax = 0;
                if (hisGenSalary.Salary == null) hisGenSalary.Salary = 0;
                if (hisGenSalary.OTAM == null) hisGenSalary.OTAM = 0;
                if (hisGenSalary.PayBack == null) hisGenSalary.PayBack = 0;
                if (hisGenSalary.LeaveDeduct == null) hisGenSalary.LeaveDeduct = 0;

                GrossPay = hisGenSalary.Salary.Value() +
                           hisGenSalary.OTAM.Value() +
                           hisGenSalary.PayBack.Value() +
                           hisGenSalary.TaxALWAM.Value() +
                           hisGenSalary.UTaxALWAM.Value() +
                           hisGenSalary.TaxBONAM.Value() +
                           hisGenSalary.UTaxBONAM.Value() -
                           hisGenSalary.LeaveDeduct.Value();
                GrossPay = Math.Round(GrossPay, SYConstant.DECIMAL_PLACE);
                TaxExemptionAmount = (hisGenSalary.UTAXSP.Value() / hisGenSalary.ExchRate.Value + hisGenSalary.UTAXCH.Value() / hisGenSalary.ExchRate.Value);
                NetAmount = GrossPay -
                    (AdvPayAmout +
                    hisGenSalary.LOAN.Value() +
                    hisGenSalary.TaxDEDAM.Value() +
                    hisGenSalary.UTaxDEDAM.Value()
                    );
                if (hisGenSalary.UTAXSP == null) hisGenSalary.UTAXSP = 0;
                if (hisGenSalary.UTAXCH == null) hisGenSalary.UTAXCH = 0;
                if (_filter.BiMonthlySalarySetting != null && _filter.BiMonthlySalarySetting.IsAccrualTax == 1)
                {
                    var TxPayType = hisGenSalary.TXPayType;
                    AmountToBeTax = hisGenSalary.Salary.Value() +
                                   hisGenSalary.OTAM.Value() +
                                   hisGenSalary.PayBack.Value() +
                                   hisGenSalary.TaxALWAM.Value() +
                                   hisGenSalary.TaxBONAM.Value() -
                                   (hisGenSalary.LeaveDeduct.Value() + hisGenSalary.TaxDEDAM.Value()) -
                                   TaxExemptionAmount;
                    AmountToBeTax = GrossPay - (hisGenSalary.UTAXSP.Value() / hisGenSalary.ExchRate.Value() + hisGenSalary.UTAXCH.Value() / hisGenSalary.ExchRate.Value());
                    AmountToBeTax = Math.Round(AmountToBeTax, SYConstant.DECIMAL_PLACE);
                    RealAmountTobeTax = AmountToBeTax * ExchangeRate;
                    if (RealAmountTobeTax < 0)
                    {
                        RealAmountTobeTax = 0;
                        AmountToBeTax = 0;
                    }
                    if (TaxType)
                    {
                        var ListTax = _filter.ListTaxSetting;
                        if (_filter.Staff.IsResident == false)
                        {
                            TaxRate = 20;
                            TaxUSD = ((RealAmountTobeTax * TaxRate) / 100) / ExchangeRate;
                        }
                        else
                        {
                            var TaxSetting = ListTax.Where(w => w.TaxFrom <= RealAmountTobeTax && w.TaxTo >= RealAmountTobeTax);
                            foreach (var tax in TaxSetting)
                            {
                                TaxKH = (RealAmountTobeTax * tax.TaxPercent.Value() / 100) - tax.Amdeduct.Value();
                                TaxUSD = Math.Round(TaxKH / ExchangeRate, SYConstant.DECIMAL_PLACE);
                                TaxRate = tax.TaxPercent.Value();
                            }
                        }

                        if (SYConstant.DECIMAL_PLACE == 0) TaxKH = Math.Round(TaxKH, SYConstant.DECIMAL_PLACE);
                    }
                    if (TxPayType != "C")
                    {
                        NetAmount -= TaxUSD;
                    }

                }
                if (_filter.LstBankFee.Where(w => w.BrankCode == hisGenSalary.BankName).Count() > 0)
                {
                    var _bankFee = _filter.LstBankFee.Where(w => w.BrankCode == hisGenSalary.BankName).ToList();
                    var ListBabkFee = _bankFee.Where(w => w.FeeFrom <= hisGenSalary.NetWage && w.FeeTo >= hisGenSalary.NetWage).ToList();
                    foreach (var item in ListBabkFee)
                    {
                        if (item.Type == "Amount")
                        {
                            hisGenSalary.BankFee = item.Rate;
                        }
                    }
                }

                NetAmount = Math.Round(NetAmount, SYConstant.DECIMAL_PLACE);
                if (_filter.Default_Curremcy == "KH" && _filter.Round_UP == "YES") NetAmount = Rounding(NetAmount);

                hisGenSalary.TAXTYPE = 0;
                hisGenSalary.AMTOBETAX = AmountToBeTax;
                hisGenSalary.AmountKH = RealAmountTobeTax;
                hisGenSalary.TaxAmountUSD = TaxUSD;
                hisGenSalary.TaxAmountKH = TaxKH;
                hisGenSalary.GrossPay = GrossPay;
                hisGenSalary.NetWage = NetAmount;
                hisGenSalary.CareerCode = CareerCode;

                DB.HISGenFirstPays.Add(hisGenSalary);
                DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public int Rounding(decimal? Salary)
        {
            int _result = 0;
            if (Salary == 0) return 0;
            int _netPay = Convert.ToInt32(Salary);
            int Values = Convert.ToInt32(_netPay.ToString().Substring(_netPay.ToString().Length - 2, 2));
            int _rounding = 100;
            if (Values >= 50)
            {
                int result = _rounding - Values;
                _result = _netPay + result;
            }
            else
            {
                _result = _netPay - Values;
            }
            return _result;
        }
        public void Calculate_Overtime(GEN_Filter1 _filter, HRBranch _Branch)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var OTRate = DB.PROTRates.ToList();
                decimal OTAmount = 0;
                decimal WorkHourPerDay = Convert.ToDecimal(_filter.Parameter.WHOUR);
                DateTime ToDate = _filter.FromDate.AddMonths(1).AddDays(-1);
                if (_filter.BiMonthlySalarySetting != null && _filter.BiMonthlySalarySetting.IsCalOvertime == 1)
                {
                    var EmpOT = DB.PREmpOverTimes.Where(w => w.EmpCode == _filter.EmpCode && w.PayMonth.Value.Year == _filter.InYear &&
                    w.PayMonth.Value.Month == _filter.InMonth && w.OTHour > 0).ToList();

                    foreach (var OT in EmpOT.OrderBy(w => w.OTDate))
                    {
                        decimal TempRate = 0;
                        decimal Rate = 0;
                        decimal WorkDayPerMonth = PRPayParameterObject.Get_WorkingDay_Salary(_filter.Parameter, _filter.FromDate, ToDate);
                        if (_filter.LstEmpCareer.Where(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate.Value()).Any())
                        {
                            decimal BaseSalary = Convert.ToDecimal(_filter.LstEmpCareer.FirstOrDefault(w => OT.OTDate >= w.FromDate && OT.OTDate <= w.ToDate.Value()).NewSalary);
                            var Result = OTRate.FirstOrDefault(w => w.OTCode == OT.OTType);
                            Rate = BaseSalary / WorkDayPerMonth;
                            if (Result.Soperator == "+")
                                Rate = Convert.ToDecimal(Rate + Result.Toperand);
                            else if (Result.Soperator == "-")
                                Rate = Convert.ToDecimal(Rate - Result.Toperand);
                            else if (Result.Soperator == "/")
                                Rate = Convert.ToDecimal(Rate / Result.Toperand);
                            else if (Result.Soperator == "*")
                                Rate = Convert.ToDecimal(Rate * Result.Toperand);

                            if (Result.Measure == "H") TempRate = Convert.ToDecimal(Rate / WorkHourPerDay);

                            else if (Result.Measure == "D") TempRate = Convert.ToDecimal(Rate);

                            OTAmount = OTAmount + (Convert.ToDecimal(OT.OTHour) * TempRate);
                            OTAmount = Math.Round(OTAmount, SYConstant.DECIMAL_PLACE);
                            var Gen_OT = new HisGenOTFirstPay()
                            {
                                EmpCode = _filter.EmpCode,
                                OTDate = OT.OTDate,
                                BaseSalary = BaseSalary,
                                WorkDay = WorkDayPerMonth,
                                WorkHour = WorkHourPerDay,
                                InMonth = _filter.InMonth,
                                InYear = _filter.InYear,
                                OTHour = OT.OTHour,
                                OTDesc = Result.OTType,
                                OTHDesc = Result.OTHDESC,
                                OTCode = OT.OTType,
                                OTRate = TempRate,
                                OTFormula = "(" + Result.Foperand + ")" + Result.Soperator + Result.Toperand,
                                Measure = Result.Measure,
                                Amount = Math.Round(TempRate * Convert.ToDecimal(OT.OTHour), SYConstant.DECIMAL_PLACE),
                                CreateBy = User.UserName,
                                CreateOn = DateTime.Now,
                                PeriodID = _filter.PeriodID
                            };
                            _filter.ListHisEmpOTFirstPay.Add(Gen_OT);
                            DB.HisGenOTFirstPays.Add(Gen_OT);
                        }
                    }
                }
                _filter.HisGenFirstPay.OTAM = OTAmount;
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Calculate_Deduction(GEN_Filter1 _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = true;

                if (_filter.BiMonthlySalarySetting != null && _filter.BiMonthlySalarySetting.IsCalDeduction == 1)
                {

                    DateTime startDate = _filter.FromDate;
                    DateTime endDate = _filter.ToDate;
                    DateTime payFrom = _filter.PayFrom;
                    DateTime payTo = _filter.PayTo;
                    decimal workedDay = PRPayParameterObject.Get_WorkingDay_Ded(_filter.Parameter, startDate, endDate);
                    PRSRewardType rewardType = new PRSRewardType();
                    var Reward_Type = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.DED.ToString(), TermCalculation.FIRST.ToString());
                    //_filter.LstRewardsType.Where(w => w.ReCode == "DED").ToList();

                    var _lstDedection = DB.PREmpDeducs.Where(w => w.EmpCode == _filter.EmpCode).ToList();
                    var deductions = _lstDedection.Where(w => (w.FromDate.HasValue && w.FromDate <= endDate) && (w.ToDate.HasValue && w.ToDate.Value().Date >= startDate)).ToList();
                    deductions = deductions.Where(w => Reward_Type.Where(x => x.Code == w.DedCode).Any()).ToList();
                    foreach (var deduction in deductions)
                    {
                        decimal actualWorkedDay = 0;
                        decimal tempAmount = 0;
                        decimal rate = 0;
                        actualWorkedDay = workedDay / 2;
                        DateTime fromDate = (deduction.FromDate.HasValue && deduction.FromDate.Value > startDate) ? deduction.FromDate.Value : startDate;
                        DateTime toDate = (deduction.ToDate.HasValue && deduction.ToDate.Value < endDate) ? deduction.ToDate.Value : endDate;
                        bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                        rate = (decimal)(deduction.Amount / workedDay);
                        var deductType = Reward_Type.FirstOrDefault(w => w.Code == deduction.DedCode);
                        int percetage = 100;
                        if (deductType != null)
                        {
                            rate = deduction.Amount.Value / workedDay;
                            if (deduction.Status == 1 || isFullMonth)
                            {
                                tempAmount = deduction.Amount.ToDecimal();
                            }
                            else if (fromDate.Date == payFrom.Date && toDate.Date == endDate.Date)
                            {
                                actualWorkedDay = workedDay;
                                tempAmount = deduction.Amount.Value;
                            }
                            else
                            {
                                actualWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                                tempAmount = actualWorkedDay * rate;
                            }
                            tempAmount = (tempAmount * deductType.BIPercentageAm) / percetage;
                            DB.SaveChanges();
                        }
                    }
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Calculate_Bonus(GEN_Filter1 _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;

                if (_filter.BiMonthlySalarySetting != null && _filter.BiMonthlySalarySetting.IsCalBounus == 1)
                {
                    DateTime startDate = _filter.FromDate;
                    DateTime endDate = _filter.ToDate;
                    DateTime payFrom = _filter.PayFrom;
                    DateTime payTo = _filter.PayTo;
                    decimal workedDay = PRPayParameterObject.Get_WorkingDay_Allw(_filter.Parameter, startDate, endDate);
                    PRSRewardType rewardType = new PRSRewardType();
                    #region bonus fix amount
                    var bonus = _filter.LstEmpBon.Where(w => w.EmpCode == _filter.EmpCode &&
                        (w.FromDate.HasValue && w.FromDate <= endDate) && (w.ToDate.HasValue && w.ToDate.Value().Date >= startDate)).ToList();
                    var bonusTypes = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.BON.ToString(), TermCalculation.FIRST.ToString());
                    //_filter.LstRewardsType.Where(w => w.ReCode == "BON").ToList();
                    bonus = bonus.Where(w => bonusTypes.Where(x => x.Code == w.BonCode).Any()).ToList();
                    foreach (var bon in bonus)
                    {
                        decimal tempAmount = 0;
                        decimal rate = 0;
                        decimal actualWorkedDay = workedDay / 2;

                        DateTime fromDate = (bon.FromDate.HasValue && bon.FromDate.Value > startDate) ? bon.FromDate.Value : startDate;
                        DateTime toDate = (bon.ToDate.HasValue && bon.ToDate.Value < endDate) ? bon.ToDate.Value : endDate;
                        bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                        rate = (decimal)(bon.Amount / workedDay);
                        var bonusType = bonusTypes.FirstOrDefault(w => w.Code == bon.BonCode);
                        int percetage = 100;
                        if (bonusType != null)
                        {
                            rate = bon.Amount / workedDay;
                            if (isFullMonth)//(bon.Status == 1 || isFullMonth)
                            {
                                tempAmount = bon.Amount;
                            }
                            else if (fromDate.Date == payFrom.Date && toDate.Date == payTo.Date)
                            {
                                actualWorkedDay = workedDay;
                                tempAmount = bon.Amount;
                            }
                            else
                            {
                                actualWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                                tempAmount = actualWorkedDay * rate;
                            }
                            tempAmount = (tempAmount * bonusType.BIPercentageAm) / percetage;
                            _filter.LstPayHis.Add(new ClsPayHis()
                            {
                                EmpCode = _filter.EmpCode,
                                SGROUP = "B",
                                PayType = "ALW",
                                Code = bonusType.Code,
                                Description = bon.BonDescription.ToUpper(),
                                Amount = Convert.ToDecimal(bon.Amount)
                            });
                        }
                    }
                    #endregion
                    DB.SaveChanges();
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Calculate_Allowance(GEN_Filter1 _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;

                if (_filter.BiMonthlySalarySetting != null && _filter.BiMonthlySalarySetting.IsCalAllowan == 1)
                {
                    DateTime startDate = _filter.FromDate;
                    DateTime endDate = _filter.ToDate;
                    DateTime payFrom = _filter.PayFrom;
                    DateTime payTo = _filter.PayTo;
                    decimal workedDay = PRPayParameterObject.Get_WorkingDay_Allw(_filter.Parameter, startDate, endDate);
                    PRSRewardType rewardType = new PRSRewardType();
                    var allowances = _filter.LstEmpAllow.Where(w => w.EmpCode == _filter.EmpCode &&
                        (w.FromDate.HasValue && w.FromDate <= endDate) && (w.ToDate.HasValue && w.ToDate.Value().Date >= startDate)).ToList();
                    var allowanceTypes = rewardType.GetRewardsType(_filter.LstRewardsType, RewardTypeCode.ALLW.ToString(), TermCalculation.FIRST.ToString());
                    //_filter.LstRewardsType.Where(w => w.ReCode == "ALLW").ToList();
                    int percetage = 100;
                    allowances = allowances.Where(x => allowanceTypes.Where(w => w.Code.Contains(x.AllwCode)).Any()).ToList();
                    foreach (var allowance in allowances)
                    {
                        decimal tempAmount = 0;
                        decimal rate = 0;
                        decimal actualWorkedDay = workedDay / 2;

                        DateTime fromDate = (allowance.FromDate.HasValue && allowance.FromDate.Value > startDate) ? allowance.FromDate.Value : startDate;
                        DateTime toDate = (allowance.ToDate.HasValue && allowance.ToDate.Value < endDate) ? allowance.ToDate.Value : endDate;
                        bool isFullMonth = fromDate.Date == startDate.Date && toDate.Date == endDate.Date;
                        rate = (decimal)(allowance.Amount / workedDay);
                        var allwType = allowanceTypes.FirstOrDefault(w => w.Code == allowance.AllwCode);
                        if (allwType != null)
                        {
                            rate = allowance.Amount.Value / workedDay;
                            if (isFullMonth)
                            {
                                tempAmount = allowance.Amount.ToDecimal();
                            }
                            else if (fromDate.Date == payFrom.Date && toDate.Date == payTo.Date)
                            {
                                actualWorkedDay = workedDay;
                                tempAmount = allowance.Amount.Value;
                            }
                            else
                            {
                                actualWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, fromDate, toDate);
                                tempAmount = actualWorkedDay * rate;
                            }
                            tempAmount = (tempAmount * allwType.BIPercentageAm) / percetage;
                            _filter.LstPayHis.Add(new ClsPayHis()
                            {
                                EmpCode = _filter.EmpCode,
                                SGROUP = "B",
                                PayType = "ALW",
                                Code = allwType.Code,
                                Description = allowance.AllwDescription.ToUpper(),
                                Amount = Convert.ToDecimal(allowance.Amount)
                            });

                        }
                    }
                    DB.SaveChanges();
                }

            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public void Calculate_LeaveDeduction(GEN_Filter1 _filter, HRBranch _Branch)
        {
            try
            {
                var biMonthlySalary = DB.BiMonthlySalarySettings.Where(x => x.PayrollParameterID == _filter.Parameter.Code).FirstOrDefault();
                if (biMonthlySalary != null && biMonthlySalary.IsCalLeaveDed == 1)
                {
                    DB.Configuration.AutoDetectChangesEnabled = false;
                    var LeaveType = DB.HRLeaveTypes.ToList();

                    decimal DayRate = 0;
                    string Approve = SYDocumentStatus.APPROVED.ToString();
                    decimal workHour = _filter.HisGenFirstPay.WorkHour.Value;
                    decimal NoDayInMonth = _filter.HisGenFirstPay.WorkDay.Value;
                    DateTime startDate = _filter.HisGenFirstPay.PayFrom.Value;
                    DateTime endDate = _filter.HisGenFirstPay.PayTo.Value;
                    decimal totalDeductAmount = 0;
                    decimal actWorkedDay = _filter.HisGenFirstPay.ActWorkDay.Value;

                    var LeaveH = DB.HREmpLeaves.Where(w => w.EmpCode == _filter.EmpCode && w.Status == Approve).ToList();

                    LeaveH = LeaveH.Where(C => ((C.FromDate >= startDate && C.FromDate <= endDate) || (C.ToDate >= startDate && C.ToDate <= endDate) ||
                     (startDate >= C.FromDate && startDate <= C.ToDate) || (endDate >= C.FromDate && endDate <= C.ToDate))).ToList();
                    var EmpLeave = DB.HREmpLeaveDs.Where(w => w.EmpCode == _filter.EmpCode && (w.CutMonth >= startDate && w.CutMonth <= endDate) && w.Status == "Leave").ToList();

                    EmpLeave = EmpLeave.Where(w => LeaveH.Where(x => x.EmpCode == w.EmpCode && x.Increment == w.LeaveTranNo).Any()).ToList();
                    EmpLeave = EmpLeave.Where(w => LeaveType.Where(x => x.Code == w.LeaveCode && x.CUT == true).Any()).ToList();
                    // in case MTL full month
                    decimal workDay = 0;
                    decimal variant = 0;
                    int i = 1;
                    Dictionary<string, string> ltype = new Dictionary<string, string>();
                    var mtl = EmpLeave.FindAll(x => LeaveH.Any(y => y.Increment == x.LeaveTranNo && y.LeaveCode == "ML"));
                    if (mtl.Count > 0)
                    {
                        DateTime fromDate = mtl.FirstOrDefault().LeaveDate;
                        DateTime toDate = mtl.LastOrDefault().LeaveDate;
                        decimal totalDay = fromDate.Subtract(toDate).Days;
                        if (mtl.Count() == totalDay)
                        {

                            workDay = mtl.Count();
                            variant = NoDayInMonth - workDay;
                            if (variant < 0)
                                variant = 0;
                        }
                    }
                    foreach (var Leave in EmpLeave)
                    {
                        DayRate = 0;
                        var Emp_C = _filter.LstEmpCareer.FirstOrDefault(C => Leave.LeaveDate >= C.FromDate && Leave.LeaveDate <= C.ToDate.Value());
                        var _Type = LeaveType.FirstOrDefault(w => w.Code == Leave.LeaveCode);
                        DayRate = LeaveTypeObject.GetUnitLeaveDeductionAmoun(_Type, Emp_C.NewSalary, NoDayInMonth, workHour);
                        decimal LHour = Convert.ToDecimal(Leave.LHour);
                        string Measure = "H";
                        if (_Type.CUTTYPE == 1) Measure = "D";
                        if (Measure == "D") LHour = Convert.ToDecimal(Leave.LHour / _filter.Parameter.WHOUR);
                        decimal? Amount = LHour * DayRate;
                        if (!ltype.ContainsKey(_Type.Code))
                        {
                            ltype.Add(_Type.Code, _Type.Code);
                            i = 1;
                        }
                        if (actWorkedDay < i)
                        {
                            DayRate = 0;
                            Amount = 0;
                        }
                        var _Gen = new HisGenLeaveDFirstPay()
                        {
                            EmpCode = _filter.EmpCode,
                            InYear = _filter.InYear,
                            InMonth = _filter.InMonth,
                            LeaveCode = Leave.LeaveCode,
                            LeaveDesc = _Type.Description,
                            LeaveOthDesc = _Type.OthDesc,
                            LeaveDate = Leave.LeaveDate,
                            ForMular = "(" + _Type.Foperand + ")" + _Type.Operator + _Type.Soperand,
                            BaseSalary = Emp_C.NewSalary,
                            WorkDay = NoDayInMonth,
                            WorkHour = _filter.Parameter.WHOUR,
                            Measure = Measure,
                            Qty = LHour,
                            Rate = Math.Round(DayRate, 2),
                            Amount = Amount,
                            CreateBy = User.UserName,
                            CreateOn = DateTime.Now
                        };
                        ++i;
                        totalDeductAmount += Convert.ToDecimal(Amount);
                        _filter.ListHisEmpLeaveFirstPay.Add(_Gen);
                        DB.HisGenLeaveDFirstPays.Add(_Gen);
                    }
                    _filter.HisGenFirstPay.LeaveDeduct = Math.Round(totalDeductAmount, SYConstant.DECIMAL_PLACE, MidpointRounding.AwayFromZero);
                }
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        private bool IsSalaryChanged(List<HREmpCareer> careers)
        {
            return careers.FindAll(x => !(String.IsNullOrEmpty(x.Increase.ToString()) || x.Increase == 0)).Count > 0;
        }
        //public void Generate_Salary(GEN_Filter1 _filter, HRBranch _Branch, List<HRCareerHistory> LstCarcode, ref bool IsGenerate)
        //{
        //    try
        //    {
        //        DB.Configuration.AutoDetectChangesEnabled = false;
        //        var _listGenD = new List<HISGenFirstPaySalaryD>();
        //        DateTime _PStartDate = _filter.FromDate;
        //        DateTime _PToDate = _filter.ToDate;
        //        DateTime payFrom = _filter.PayFrom;
        //        DateTime payTo = _filter.PayTo;
        //        decimal TempSalary = 0;
        //        decimal Rate = 0;
        //        decimal Amount = 0;
        //        decimal WorkHourPerDay = Convert.ToDecimal(_filter.Parameter.WHOUR);
        //        DateTime ToDate = _filter.FromDate.AddMonths(1).AddDays(-1);
        //        decimal DayInMonth = PRPayParameterObject.Get_WorkingDay_Salary(_filter.Parameter, _PStartDate, _PToDate);
        //        decimal halfWorkedDay = DayInMonth / 2;
        //        decimal Loan = 0;
        //        decimal? AdvPay = 0;
        //        decimal? Payback = 0;
        //        decimal? BaseSalary = 0;
        //        decimal? _Increased = 0;

        //        //var Emp_C = _filter.LstEmpCareer.Where(w => ((w.FromDate >= payFrom && w.FromDate <= payTo) ||
        //        //            ((w.ToDate.HasValue ? w.ToDate : DateTimeHelper.MaxValue) >= payFrom && (w.ToDate.HasValue ? w.ToDate : DateTimeHelper.MaxValue) <= payTo) ||
        //        //            (payFrom >= w.FromDate && payFrom <= (w.ToDate.HasValue ? w.ToDate : DateTimeHelper.MaxValue)) ||
        //        //            (payTo >= w.FromDate && payTo <= (w.ToDate.HasValue ? w.ToDate : DateTimeHelper.MaxValue)))
        //        //            && w.EmpCode == _filter.EmpCode).ToList();
        //        var Emp_C = _filter.LstEmpCareer.Where(w => w.FromDate.Value() <= payTo && w.ToDate.Value() >= payFrom
        //                  && w.EmpCode == _filter.EmpCode).OrderBy(x => x.FromDate).ToList();

        //        if (LstCarcode.Where(w => w.NotCalSalary == true && Emp_C.Where(x => x.CareerCode == w.Code).Any()).Any())
        //        {
        //            var _lstInAct = Emp_C.Where(w => LstCarcode.Where(x => w.CareerCode == x.Code).Any()).ToList();
        //            Emp_C = Emp_C.Where(w => !LstCarcode.Where(x => w.CareerCode == x.Code).Any()).ToList();
        //            var ResType = DB.HRTerminTypes.Where(w => w.IsCalSalary == true).ToList();
        //            if (!_lstInAct.Where(w => ResType.Where(x => x.Code == w.resigntype).Any()).Any())
        //            {
        //                return;
        //            }
        //        }

        //        foreach (var emp in Emp_C)
        //        {
        //            Rate = TempSalary = Amount = 0;

        //            DateTime PFromDate = (emp.FromDate == null || emp.FromDate.Value < payFrom) ? payFrom : emp.FromDate.Value;
        //            DateTime PToDate = (emp.ToDate == null || emp.ToDate.Value > payTo) ? payTo : emp.ToDate.Value;
        //            decimal prorateWorkedDay = 0;
        //            prorateWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, payFrom, payTo, PFromDate, PToDate, Emp_C.Count);
        //            #region not use
        //            //decimal prorateWorkedDay = 0;
        //            //if (PFromDate > payFrom)
        //            //{
        //            //    if (PToDate < payTo)
        //            //        prorateWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, PFromDate, PToDate);
        //            //    else
        //            //    {
        //            //        decimal nonWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, payFrom, PFromDate.AddDays(-1));
        //            //        prorateWorkedDay = halfWorkedDay - nonWorkedDay;
        //            //    }
        //            //}
        //            //else if (PToDate < payTo)
        //            //{
        //            //    if (Emp_C.Count() > 1)
        //            //        prorateWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, PFromDate, PToDate);
        //            //    else
        //            //    {
        //            //        prorateWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, payFrom, PToDate);
        //            //        //decimal nonWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, PToDate, payTo);

        //            //        //prorateWorkedDay = halfWorkedDay - nonWorkedDay;
        //            //    }
        //            //}
        //            //else if (PFromDate.Date == payFrom.Date && PToDate.Date == payTo.Date)
        //            //{
        //            //    prorateWorkedDay = halfWorkedDay;
        //            //}

        //            ////prorateWorkedDay = PRPayParameterObject.Get_WorkingDay(_filter.Parameter, PFromDate, PToDate);
        //            #endregion

        //            decimal TMPD = 0;
        //            TMPD = Convert.ToDecimal(_listGenD.Sum(x => x.ActWorkDay));
        //            if ((TMPD + prorateWorkedDay) > halfWorkedDay) prorateWorkedDay = Convert.ToDecimal(halfWorkedDay) - TMPD;

        //            if (!IsSalaryChanged(Emp_C) || Emp_C.Count == 1)
        //            {
        //                Rate = emp.NewSalary / DayInMonth;
        //                if (payFrom.Date == PFromDate.Date && PToDate.Date == payTo.Date)
        //                    TempSalary = emp.NewSalary / 2;
        //                else
        //                    TempSalary = prorateWorkedDay * Rate;
        //            }
        //            else
        //            {
        //                Rate = emp.NewSalary / DayInMonth;
        //                TempSalary = prorateWorkedDay * Rate;
        //            }
        //            TempSalary = Math.Round(TempSalary, SYConstant.DECIMAL_PLACE);
        //            var _His_GenD = new HISGenFirstPaySalaryD()
        //            {
        //                INYear = _filter.InYear,
        //                INMonth = _filter.InMonth,
        //                FromDate = _filter.FromDate,
        //                ToDate = _filter.ToDate,
        //                CareerCode = emp.CareerCode,
        //                WorkDay = DayInMonth,
        //                WorkHour = WorkHourPerDay,
        //                EmpCode = emp.EmpCode,
        //                EmpType = emp.EmpType,
        //                Branch = emp.Branch,
        //                Location = emp.LOCT,
        //                Division = emp.Division,
        //                DEPT = emp.DEPT,
        //                LINE = emp.LINE,
        //                CATE = emp.CATE,
        //                Sect = emp.SECT,
        //                POST = emp.JobCode,
        //                LevelCode = emp.LevelCode,
        //                PayFrom = PFromDate,
        //                PayTo = PToDate,
        //                ActWorkDay = prorateWorkedDay,
        //                BasicSalary = emp.NewSalary,
        //                Rate = Rate,
        //                Amount = TempSalary,
        //                CreateBy = User.UserName,
        //                CreateOn = DateTime.Now,
        //                PeriodID = _filter.PeriodID,
        //                SalaryPartial = TempSalary
        //            };
        //            _listGenD.Add(_His_GenD);
        //            DB.HISGenFirstPaySalaryDs.Add(_His_GenD);
        //        }
        //        var ExchangeRate = DB.PRBiExchangeRates.FirstOrDefault(w => w.InYear == _filter.InYear && w.InMonth == _filter.InMonth);
        //        var EmpFamaily = DB.HREmpFamilies.Where(w => w.EmpCode == _filter.EmpCode).ToList();
        //        var Except = DB.SYHRSettings.FirstOrDefault();
        //        var EMP_GENERATESALARY_C = (from Emp_salary in _listGenD
        //                                    group Emp_salary by new { Emp_salary.EmpCode }
        //                                   into myGroup
        //                                    where myGroup.Count() > 0
        //                                    select new
        //                                    {
        //                                        myGroup.Key.EmpCode,
        //                                        ActWorkDay = myGroup.Sum(w => w.ActWorkDay),
        //                                        Amount = myGroup.Sum(w => w.Amount),
        //                                        ActworkHour = myGroup.Sum(w => w.ActWorkHours)
        //                                    }).ToList();
        //        var staff = _filter.Staff;
        //        var ADV = DB.PRADVPays.Where(w => w.EmpCode == _filter.EmpCode && (w.TranDate >= _filter.FromDate && w.TranDate <= _filter.ToDate)).ToList();
        //        foreach (var read in ADV.Where(w => w.IsBIMonthly == true))
        //        {
        //            AdvPay += read.Amount * (read.BIPercentageAm / 100);
        //        }
        //        Loan = DB.HREmpLoans.Where(w => w.EmpCode == _filter.EmpCode && ((w.PayMonth >= payFrom && w.PayMonth <= payTo && w.LoanDate >= payFrom))).ToList().Sum(x => x.LoanAm);
        //        Loan /= 2;
        //        Payback = _filter.LstEmpHold.Sum(w => w.Salary / 2);
        //        var empCardId = DB.HREmpIdentities.FirstOrDefault(w => w.EmpCode == _filter.EmpCode && w.IdentityTye == "IDCard");
        //        string IDCard = "";
        //        if (empCardId != null) IDCard = empCardId.IDCardNo;

        //        if (_listGenD.Count > 0)
        //            BaseSalary = _listGenD.OrderByDescending(x => x.PayTo).FirstOrDefault().BasicSalary;

        //        foreach (var Emp in EMP_GENERATESALARY_C)
        //        {
        //            int Child = EmpFamaily.Where(w => w.Child == true && w.TaxDeduc == true).ToList().Count;
        //            int Spouse = EmpFamaily.Where(w => w.Spouse == true && w.TaxDeduc == true).ToList().Count;
        //            decimal? ChildAmount = Convert.ToDecimal(Child * Except.Child.Value);
        //            decimal? SpouseAmount = Convert.ToDecimal(Spouse * Except.Spouse.Value);

        //            if (Emp.Amount - BaseSalary > 0)
        //                _Increased = Emp.Amount - BaseSalary;
        //            else if (LstCarcode.Where(x => x.Code == staff.CareerDesc).Any() && (Emp.Amount - BaseSalary) > 0)
        //                _Increased = Emp.Amount - BaseSalary;

        //            var _GenSala = new HISGenFirstPay()
        //            {
        //                Status = SYDocumentStatus.OPEN.ToString(),
        //                TermStatus = staff.TerminateStatus,
        //                TermRemark = staff.TerminateRemark,
        //                TermDate = staff.DateTerminate,
        //                INYear = _filter.InYear,
        //                INMonth = _filter.InMonth,
        //                FromDate = _filter.FromDate,
        //                ToDate = _filter.ToDate,
        //                PayFrom = _filter.PayFrom,
        //                PayTo = _filter.PayTo,
        //                WorkDay = DayInMonth,
        //                WorkHour = WorkHourPerDay,
        //                ExchRate = ExchangeRate.Rate,
        //                EmpCode = _filter.EmpCode,
        //                EmpName = staff.AllName,
        //                EmpType = staff.EmpType,
        //                Branch = staff.Branch,
        //                Location = staff.LOCT,
        //                Division = staff.Division,
        //                DEPT = staff.DEPT,
        //                LINE = staff.Line,
        //                CATE = staff.CATE,
        //                Sect = staff.SECT,
        //                POST = staff.JobCode,
        //                JobGrade = staff.JobGrade,
        //                LevelCode = staff.LevelCode,
        //                Sex = staff.Sex,
        //                ICNO = IDCard,
        //                BankName = staff.BankName,
        //                BankBranch = staff.BankBranch,
        //                BankAcc = staff.BankAcc,
        //                DateJoin = staff.StartDate,
        //                ActWorkDay = Emp.ActWorkDay,
        //                Salary = Emp.Amount,
        //                ADVPay = AdvPay,
        //                USERGEN = User.UserName,
        //                DATEGEN = DateTime.Now,
        //                PayBack = Payback,
        //                LOAN = Loan,
        //                Increased = _Increased,
        //                Child = Child,
        //                Spouse = Spouse,
        //                UTAXCH = ChildAmount,
        //                UTAXSP = SpouseAmount,
        //                BasicSalary = BaseSalary
        //            };
        //            Amount = Emp.Amount.Value;
        //            _filter.HisGenFirstPay = _GenSala;
        //        }
        //        if (_listGenD.Count == 0)
        //        {
        //            if (_filter.Staff.ReSalary.Year == _filter.InYear && _filter.Staff.ReSalary.Month == _filter.InMonth && _filter.Staff.SalaryFlag == true)
        //            {
        //                var _EmpCareer = _filter.LstEmpCareer.FirstOrDefault(w => LstCarcode.Where(x => x.Code == w.CareerCode).Any() && w.EmpCode == _filter.EmpCode);
        //                var _His_GenD = new HISGenFirstPaySalaryD()
        //                {
        //                    INYear = _filter.InYear,
        //                    INMonth = _filter.InMonth,
        //                    FromDate = _filter.FromDate,
        //                    ToDate = _filter.ToDate,
        //                    CareerCode = _EmpCareer.CareerCode,
        //                    WorkDay = DayInMonth,
        //                    WorkHour = WorkHourPerDay,
        //                    EmpCode = _EmpCareer.EmpCode,
        //                    EmpType = _EmpCareer.EmpType,
        //                    Branch = _EmpCareer.Branch,
        //                    Location = _EmpCareer.LOCT,
        //                    Division = _EmpCareer.Division,
        //                    DEPT = _EmpCareer.DEPT,
        //                    LINE = _EmpCareer.LINE,
        //                    CATE = _EmpCareer.CATE,
        //                    Sect = _EmpCareer.SECT,
        //                    POST = _EmpCareer.JobCode,
        //                    LevelCode = _EmpCareer.LevelCode,
        //                    PayFrom = _filter.FromDate,
        //                    PayTo = _filter.ToDate,
        //                    ActWorkDay = 0,
        //                    BasicSalary = _EmpCareer.NewSalary,
        //                    Rate = Rate,
        //                    Amount = TempSalary,
        //                    CreateBy = User.UserName,
        //                    CreateOn = DateTime.Now,
        //                    PeriodID = _filter.PeriodID
        //                };
        //                var _GenSala = new HISGenFirstPay()
        //                {
        //                    Status = SYDocumentStatus.OPEN.ToString(),
        //                    TermStatus = staff.TerminateStatus,
        //                    TermRemark = staff.TerminateRemark,
        //                    TermDate = staff.DateTerminate,
        //                    INYear = _filter.InYear,
        //                    INMonth = _filter.InMonth,
        //                    FromDate = _filter.FromDate,
        //                    ToDate = _filter.ToDate,
        //                    WorkDay = DayInMonth,
        //                    WorkHour = WorkHourPerDay,
        //                    ExchRate = ExchangeRate.Rate,
        //                    EmpCode = _filter.EmpCode,
        //                    EmpName = staff.AllName,
        //                    EmpType = staff.EmpType,
        //                    Branch = staff.Branch,
        //                    Location = staff.LOCT,
        //                    Division = staff.Division,
        //                    DEPT = staff.DEPT,
        //                    LINE = staff.Line,
        //                    CATE = staff.CATE,
        //                    Sect = staff.SECT,
        //                    JobGrade = staff.JobCode,
        //                    LevelCode = staff.LevelCode,
        //                    Sex = staff.Sex,
        //                    ICNO = IDCard,
        //                    BankName = staff.BankName,
        //                    BankBranch = staff.BankBranch,
        //                    BankAcc = staff.BankAcc,
        //                    DateJoin = staff.StartDate,
        //                    ActWorkDay = 0,
        //                    Salary = 0,
        //                    ADVPay = AdvPay,
        //                    USERGEN = User.UserName,
        //                    DATEGEN = DateTime.Now,
        //                    PayBack = Payback,
        //                    LOAN = Loan
        //                };
        //                Amount = 0;
        //                DB.HISGenFirstPaySalaryDs.Add(_His_GenD);
        //                _filter.HisGenFirstPay = _GenSala;
        //            }
        //        }
        //        _filter.LstPayHis.Add(new ClsPayHis()
        //        {
        //            EmpCode = _filter.EmpCode,
        //            SGROUP = "A",
        //            PayType = "BS",
        //            Code = "BS",
        //            Description = "BASIC SALARY",
        //            Amount = Amount
        //        });
        //        if (_filter.HisAppSalary == null && IsGenerate == false)
        //        {
        //            IsGenerate = true;
        //            var _AppSalary = new HisPendingAppSalaryFP();
        //            _AppSalary.InYear = _filter.InYear;
        //            _AppSalary.InMonth = _filter.InMonth;
        //            _AppSalary.FromDate = _filter.FromDate;
        //            _AppSalary.ToDate = _filter.ToDate;
        //            _AppSalary.IsLock = false;
        //            DB.HisPendingAppSalaryFPs.Add(_AppSalary);
        //        }
        //        int row = DB.SaveChanges();
        //    }
        //    finally
        //    {
        //        DB.Configuration.AutoDetectChangesEnabled = true;
        //    }
        //}

        public void Delete_Generate(GEN_Filter1 _filter)
        {
            try
            {
                DB.Configuration.AutoDetectChangesEnabled = false;
                var _PayType = DB.PRFiscalYears.FirstOrDefault(w => w.InMonth.Value.Year == _filter.InYear & w.InMonth.Value.Month == _filter.InMonth);

                var hisGenSalary = DB.HISGenFirstPays.Where(w => w.EmpCode == _filter.EmpCode && w.INYear == _filter.InYear && w.INMonth == _filter.InMonth).ToList();
                foreach (var item in hisGenSalary)
                {
                    DB.HISGenFirstPays.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                // delete overtime
                var hisGenOT = DB.HisGenOTFirstPays.Where(w => w.EmpCode == _filter.EmpCode && w.InYear == _filter.InYear && w.InMonth == _filter.InMonth).ToList();
                foreach (var item in hisGenOT)
                {
                    DB.HisGenOTFirstPays.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                // delete leave deduction
                var hisGenLeaveDeduction = DB.HisGenLeaveDFirstPays.Where(w => w.EmpCode == _filter.EmpCode
                    && w.InYear == _filter.InYear && w.InMonth == _filter.InMonth).ToList();
                foreach (var item in hisGenLeaveDeduction)
                {
                    DB.HisGenLeaveDFirstPays.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                // delete payslip
                var hisPaySlip = DB.HISPaySlipFirstPays.Where(w => w.EmpCode == _filter.EmpCode && w.INYear == _filter.InYear && w.INMonth == _filter.InMonth).ToList();
                foreach (var item in hisPaySlip)
                {
                    DB.HISPaySlipFirstPays.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                // delete salary d
                var hisGenSalaryD = DB.HISGenFirstPaySalaryDs.Where(w => w.EmpCode == _filter.EmpCode && w.INYear == _filter.InYear && w.INMonth == _filter.InMonth).ToList();
                foreach (var item in hisGenSalaryD)
                {
                    DB.HISGenFirstPaySalaryDs.Attach(item);
                    DB.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }
                int row = DB.SaveChanges();
            }
            finally
            {
                DB.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        public List<HR_View_EmpGenSalary> LoadDataEmpGen(FTFilterEmployee Filter1, List<HRBranch> _ListBranch)
        {
            if (Filter1.InMonth == null)
            {
                MessageCode = "Please Salary Month";
                return null;
            }

            DateTime salaryMonth = DateTimeHelper.DateInMonth(Filter1.InMonth);

            var prFiscalYear = DB.PRFiscalYears.FirstOrDefault(w => w.InMonth.Value.Month == Filter1.InMonth.Month & w.InMonth.Value.Year == Filter1.InMonth.Year);
            Filter.FromDate = Filter1.InMonth;
            Filter1.EndDate = DateTimeHelper.DateInMonth(Filter1.InMonth);
            if (prFiscalYear != null)
            {
                Filter.FromDate = prFiscalYear.StartDate.Value;
                Filter.EndDate = prFiscalYear.EndDate.Value;
            }
            var _List = new List<HR_View_EmpGenSalary>();

            var _listStaff = DBV.HR_View_EmpGenSalary.Where(w => w.IsPayPartial == true).ToList();
            DateTime date = new DateTime(1900, 1, 1);
            DateTime InMonth = new DateTime(Filter1.InMonth.Year, Filter1.InMonth.Month, 16);
            DateTime FromDate = InMonth.StartDateOfMonth();
            DateTime ToDate = InMonth;
            _listStaff = _listStaff.Where(w => (w.DateTerminate.AddDays(-1) >= InMonth) || w.DateTerminate == date).ToList();

            _listStaff = _listStaff.Where(w => _ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            if (Filter.Branch != null)
                _listStaff = _listStaff.Where(w => w.Branch == Filter1.Branch).ToList();
            if (Filter1.Division != null)
                _listStaff = _listStaff.Where(w => w.Division == Filter1.Division).ToList();
            if (Filter1.Department != null)
                _listStaff = _listStaff.Where(w => w.DEPT == Filter1.Department).ToList();
            if (Filter1.Section != null)
                _listStaff = _listStaff.Where(w => w.SECT == Filter1.Section).ToList();
            if (Filter1.Position != null)
                _listStaff = _listStaff.Where(w => w.JobCode == Filter1.Position).ToList();
            if (Filter1.Level != null)
                _listStaff = _listStaff.Where(w => w.LevelCode == Filter1.Level).ToList();
            //if (Filter1.StaffType != null)
            //    _listStaff = _listStaff.Where(w => w.StaffType == Filter1.StaffType).ToList();
            _listStaff = _listStaff.Where(w => w.IsHold != true).ToList();
            //new
            //var payparameters = DB.PRParameters.ToList();
            //var monthlySalarySetting = DB.BiMonthlySalarySettings.ToList();
            //foreach (var item in payparameters)
            //{
            //    DateTime startDate = Filter1.FromDate;
            //    var Bi = monthlySalarySetting.FirstOrDefault(w => w.PayrollParameterID == item.Code);
            //    if (Bi != null)
            //    {
            //        DateTime E_InMonth = new DateTime(Filter1.InMonth.Year, Filter1.InMonth.Month, Bi.FirstEndDay.Value);
            //        var staff1 = _listStaff.Where(x => x.PayParam == item.Code).ToList();
            //        staff1 = staff1.Where(w => w.StartDate.Value.Date <= E_InMonth && ((w.DateTerminate.Date == date.Date
            //   || w.DateTerminate.AddDays(-1) >= startDate))).ToList();

            //        staff1.Where(x => _List.All(y => y.EmpCode != x.EmpCode)).ToList().ForEach(w => _List.Add(w));
            //    }

            //}
            _List = _listStaff;
            return _List.OrderBy(w => w.EmpCode).ToList();
        }
        //public string Delete_GenerateAll(DateTime PayrollMonth, List<HRBranch> _lstBranch)
        //{
        //    var DBD = new HumicaDBContext();
        //    try
        //    {
        //        DBD.Configuration.AutoDetectChangesEnabled = false;
        //        DB.Configuration.AutoDetectChangesEnabled = false;

        //        var AppSalry = DB.HisPendingAppSalaryFPs.Where(w => w.InYear == PayrollMonth.Year & w.InMonth == PayrollMonth.Month & w.IsLock == true).ToList();
        //        if (AppSalry.Count() > 0) return "APPROVE_SALARY";

        //        var ListEmpFirstPay = DB.HISGenFirstPays.Where(w => w.INYear == PayrollMonth.Year & w.INMonth == PayrollMonth.Month).ToList();
        //        ListEmpFirstPay = ListEmpFirstPay.Where(x => _lstBranch.Where(w => w.Code == x.Branch).Any()).ToList();
        //        try
        //        {
        //            var hisGenFirstPaySalaryDs = DB.HISGenFirstPaySalaryDs.Where(w => w.INMonth == PayrollMonth.Month & w.INYear == PayrollMonth.Year).ToList();
        //            hisGenFirstPaySalaryDs = hisGenFirstPaySalaryDs.Where(w => ListEmpFirstPay.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
        //            foreach (var item in hisGenFirstPaySalaryDs)
        //            {
        //                DBD.HISGenFirstPaySalaryDs.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }
        //            foreach (var item in ListEmpFirstPay)
        //            {
        //                DBD.HISGenFirstPays.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }
        //            var His_GenOT = DB.HisGenOTFirstPays.Where(w => w.InMonth == PayrollMonth.Month & w.InYear == PayrollMonth.Year).ToList();
        //            His_GenOT = His_GenOT.Where(w => ListEmpFirstPay.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
        //            foreach (var item in His_GenOT)
        //            {
        //                DBD.HisGenOTFirstPays.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }
        //            var His_GenLeave = DB.HisGenLeaveDFirstPays.Where(w => w.InMonth == PayrollMonth.Month & w.InYear == PayrollMonth.Year).ToList();
        //            His_GenLeave = His_GenLeave.Where(w => ListEmpFirstPay.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
        //            foreach (var item in His_GenLeave)
        //            {
        //                DBD.HisGenLeaveDFirstPays.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }
        //            var His_GenPay = DB.HISPaySlipFirstPays.Where(w => w.INYear == PayrollMonth.Year && w.INMonth == PayrollMonth.Month).ToList();
        //            His_GenPay = His_GenPay.Where(w => ListEmpFirstPay.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
        //            foreach (var item in His_GenPay)
        //            {
        //                DBD.HISPaySlipFirstPays.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }

        //            var His_TransferToMgrs = DB.HisTransferToMgrs.Where(w => w.InYear == PayrollMonth.Year && w.InMonth == PayrollMonth.Month).ToList();
        //            foreach (var item in His_TransferToMgrs)
        //            {
        //                DBD.HisTransferToMgrs.Attach(item);
        //                DBD.Entry(item).State = System.Data.Entity.EntityState.Deleted;
        //            }
        //            var EmpLoan = DB.HREmpLoans.Where(w => w.PayMonth.Year == PayrollMonth.Year && w.PayMonth.Month == PayrollMonth.Month).ToList();
        //            foreach (var read in EmpLoan)
        //            {
        //                read.Status = SYDocumentStatus.OPEN.ToString();
        //                DBD.HREmpLoans.Attach(read);
        //                DBD.Entry(read).Property(w => w.Status).IsModified = true;
        //            }
        //            int row = DBD.SaveChanges();
        //            return SYConstant.OK;
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            /*------------------SaveLog----------------------------------*/
        //            SYEventLog log = new SYEventLog();
        //            log.ScreenId = ScreenId;
        //            log.UserId = User.UserName;
        //            log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //            SYEventLogObject.saveEventLog(log, e, true);
        //            /*----------------------------------------------------------*/
        //            return "EE001";
        //        }
        //        catch (DbUpdateException e)
        //        {
        //            /*------------------SaveLog----------------------------------*/
        //            SYEventLog log = new SYEventLog();
        //            log.ScreenId = ScreenId;
        //            log.UserId = User.UserName;
        //            log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

        //            SYEventLogObject.saveEventLog(log, e, true);
        //            /*----------------------------------------------------------*/
        //            return "EE001";
        //        }
        //        catch (Exception e)
        //        {
        //            /*------------------SaveLog----------------------------------*/
        //            SYEventLog log = new SYEventLog();
        //            log.ScreenId = ScreenId;
        //            log.UserId = User.UserName;
        //            log.Action = Humica.EF.SYActionBehavior.DELETE.ToString();

        //            SYEventLogObject.saveEventLog(log, e, true);
        //            /*----------------------------------------------------------*/
        //            return "EE001";
        //        }
        //    }
        //    finally
        //    {
        //        DBD.Configuration.AutoDetectChangesEnabled = true;
        //        DB.Configuration.AutoDetectChangesEnabled = true;
        //    }
        //}
        //public string Delete_PayRecord(string EmpCode, DateTime InMonth)
        //{
        //    try
        //    {
        //        var AppSalary = DB.HisPendingAppSalaryFPs.Where(w => w.InYear == InMonth.Year && w.InMonth == InMonth.Month && w.IsLock == true).ToList();
        //        if (AppSalary.Count() > 0)
        //        {
        //            return "APPROVE_SALARY";
        //        }
        //        GEN_Filter1 _filter = new GEN_Filter1();
        //        _filter.Staff = new HRStaffProfile();
        //        _filter.Staff.EmpCode = EmpCode;
        //        _filter.EmpCode = EmpCode;
        //        _filter.InYear = InMonth.Year;
        //        _filter.InMonth = InMonth.Month;
        //        Delete_Generate(_filter);
        //        return SYConstant.OK;
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Filter.INYear.ToString();
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Filter.INYear.ToString();
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = ScreenId;
        //        log.UserId = User.UserName;
        //        log.DocurmentAction = Filter.INYear.ToString();
        //        log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        return "EE001";
        //    }
        //}

        #region Approval Salary
        public void SetAutoApproval(string SCREEN_ID, string DocType)
        {
            ListApproval = new List<ExDocApproval>();
            var DBX = new HumicaDBContext();
            var objDoc = DBX.ExCfWorkFlowItems.Find(SCREEN_ID, DocType);
            if (objDoc != null)
            {
                if (objDoc.IsRequiredApproval == true)
                {
                    var listDefaultApproval = DBX.ExCfWFSalaryApprovers.Where(w => w.WFObject == objDoc.ApprovalFlow && w.IsSelected == true).ToList();
                    foreach (var read in listDefaultApproval)
                    {
                        var objApp = new ExDocApproval();
                        objApp.Approver = read.Employee;
                        objApp.ApproverName = read.EmployeeName;
                        objApp.DocumentType = DocType;
                        objApp.ApproveLevel = read.ApproveLevel;
                        objApp.WFObject = objDoc.ApprovalFlow;
                        ListApproval.Add(objApp);
                    }
                }
            }
        }
        public string isValidApproval(ExDocApproval Approver, EnumActionGridLine Action)
        {
            if (Action == EnumActionGridLine.Add)
            {
                if (ListApproval.Where(w => w.Approver == Approver.Approver).ToList().Count > 0)
                {
                    return "DUPLICATED_ITEM";
                }
            }

            return SYConstant.OK;
        }
        public HRStaffProfile getNextApprover(string id, string pro)
        {
            var objStaff = new HRStaffProfile();
            var DBX = new HumicaDBContext();
            var objHeader = DBX.HISApproveSalaryFDs.Find(id);
            if (objHeader == null)
            {
                return new HRStaffProfile();
            }
            string open = SYDocumentStatus.OPEN.ToString();
            var listCanApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == id && w.Status == open && w.DocumentType == objHeader.DocumentType).ToList();

            if (listCanApproval.Count == 0)
            {
                return new HRStaffProfile();
            }

            var min = listCanApproval.Min(w => w.ApproveLevel);
            var NextApp = listCanApproval.Where(w => w.ApproveLevel == min).First();
            objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);//, objHeader.Property);
            return objStaff;
        }
        public string CreateAppSalary()
        {
            try
            {
                var DBR = new HumicaDBContext();
                var objCF = DB.ExCfWorkFlowItems.Find("PRM0000022", HeaderAppSalary.DocumentType);
                if (objCF == null)
                {
                    return "REQUEST_TYPE_NE";
                }
                if (!ListApproval.Any()) return "INVALIDED_APPROVER";
                var objNumber = new CFNumberRank(HeaderAppSalary.DocumentType, "PRM0000022");
                if (objNumber == null)
                {
                    return "NUMBER_RANK_NE";
                }
                HeaderAppSalary.ASNumber = objNumber.NextNumberRank.Trim();
                HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString();
                //Add approver
                foreach (var read in ListApproval)
                {
                    read.ID = 0;
                    read.LastChangedDate = DateTime.Now;
                    read.DocumentNo = HeaderAppSalary.ASNumber;
                    read.DocumentType = HeaderAppSalary.DocumentType;
                    read.Status = SYDocumentStatus.OPEN.ToString();
                    read.WFObject = objCF.ApprovalFlow;
                    read.ApprovedBy = "";
                    read.ApprovedName = "";
                    read.ApproverName = "";
                    var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                    if (objStaff != null)
                    {
                        read.ApproverName = objStaff.AllName;
                    }
                    DB.ExDocApprovals.Add(read);
                }
                var objUpdate = DBR.HisPendingAppSalaryFPs.FirstOrDefault(w => w.PeriodID == HeaderAppSalary.PeriodID);
                if (objUpdate == null) return "INVALIDED";
                //if (objUpdate != null)
                //{
                //    objUpdate.IsLock = true;
                //    DB.HisPendingAppSalaryFPs.Attach(objUpdate);
                //    DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                //}
                HeaderAppSalary.PeriodID = objUpdate.PeriodID;
                HeaderAppSalary.CompanyCode = objUpdate.CompanyCode;
                HeaderAppSalary.CreatedOn = DateTime.Now;
                HeaderAppSalary.CreatedBy = User.UserName;

                DB.HISApproveSalaryFDs.Add(HeaderAppSalary);
                int row = DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string requestToApprove(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HISApproveSalaryFDs.Find(id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderAppSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.OPEN.ToString())
                {
                    return "INV_DOC";
                }

                objMatch.Status = SYDocumentStatus.PENDING.ToString();
                DB.HISApproveSalaryFDs.Attach(objMatch);
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();

                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderAppSalary.ASNumber;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string approveTheDoc(string id)
        {
            try
            {
                HumicaDBContext DBX = new HumicaDBContext();
                var objMatch = DB.HISApproveSalaryFDs.FirstOrDefault(w => w.ASNumber == id);
                if (objMatch == null)
                {
                    return "REQUEST_NE";
                }
                HeaderAppSalary = objMatch;

                if (objMatch.Status != SYDocumentStatus.PENDING.ToString())
                {
                    return "INV_DOC";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objMatch.DocumentType
                                    && w.DocumentNo == objMatch.ASNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                var approverLevel = 0;
                foreach (var read in listApproval)
                {
                    approverLevel = read.ApproveLevel;
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }

                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            //StaffProfile = objStaff;
                            read.ApprovedBy = objStaff.EmpCode;
                            read.ApprovedName = objStaff.AllName;
                            read.LastChangedDate = DateTime.Now.Date;
                            read.ApprovedDate = DateTime.Now;
                            read.Status = SYDocumentStatus.APPROVED.ToString();
                            DBX.ExDocApprovals.Attach(read);
                            DBX.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            b = true;
                            break;
                        }
                    }

                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }
                var status = SYDocumentStatus.APPROVED.ToString();
                var Appro_ = objMatch.Requestor;
                // objMatch.IsApproved = true;
                if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                {
                    status = SYDocumentStatus.PENDING.ToString();
                    Appro_ = listApproval.Where(w => w.ApproveLevel == approverLevel).Select(w => w.Approver)?.FirstOrDefault();
                }
                if (status == SYDocumentStatus.APPROVED.ToString())
                {
                    var objUpdate = DB.HisPendingAppSalaryFPs.FirstOrDefault(w => w.PeriodID == objMatch.PeriodID);
                    if (objUpdate != null)
                    {
                        objUpdate.IsLock = true;
                        DB.HisPendingAppSalaryFPs.Attach(objUpdate);
                        DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                    }
                }
                objMatch.Status = status;
                DB.Entry(objMatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                #region Email
                if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                {
                    ExDocApproval currenctApproval_ = listApproval.Where(w => w.Approver == Appro_).FirstOrDefault();
                    ExDocApproval exDocApproval_ = GetNextItemFromList(listApproval, currenctApproval_).ToList().FirstOrDefault();
                    var AlertTO_ = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == exDocApproval_.Approver);
                    var EmailConf_ = DMS.CFEmailAccounts.FirstOrDefault();

                    if (EmailConf_ != null && AlertTO_ != null)
                    {
                        CFEmailAccount emailAccount = EmailConf_;
                        string Subject = "Second Payroll Approval";
                        string Body = string.Format("Please kindly find the payroll report both summary and details for the month of   {0:dd-MMM-yyyy}. Please kindly check and approve on payroll.Should you have any questions, kindly let me know. Best regards,", DateTime.Today);

                        string filePath = "";
                        string fileName = Path.GetFileName(filePath);
                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMail(emailAccount, Subject, AlertTO_.Email,
                            Subject, Body, filePath, fileName);
                    }
                }
                else
                {
                    var AlertTO = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objMatch.Requestor);
                    var EmailConf = DMS.CFEmailAccounts.FirstOrDefault();
                    if (EmailConf != null && AlertTO != null)
                    {
                        CFEmailAccount emailAccount = EmailConf;
                        string Subject = "Second Payroll Approval";
                        string Body = string.Format("Your Salary Approve Requset has been Approved  {0:dd-MMM-yyyy}", DateTime.Today);

                        string filePath = "";
                        string fileName = Path.GetFileName(filePath);
                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMail(emailAccount, Subject, AlertTO.Email,
                            Subject, Body, filePath, fileName);
                    }
                }
                #endregion
                int row = DBX.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.ScreenId = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = HeaderAppSalary.ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        private static List<T> GetNextItemFromList<T>(List<T> list, T currentObj)
        {
            return list.SkipWhile(obj => obj.Equals(currentObj)).Take(1).ToList();
        }
        public string CancelAppSalary(string ASNumber)
        {
            try
            {
                string approved = SYDocumentStatus.CANCELLED.ToString();
                var objmatch = DB.HISApproveSalaryFDs.FirstOrDefault(w => w.ASNumber == ASNumber);
                var _obj = DB.ExDocApprovals.Where(x => x.DocumentNo == ASNumber);
                if (objmatch == null)
                {
                    return "INV_EN";
                }
                if (objmatch.IsPostGL == true)
                {
                    return "DOC_POST_GL";
                }
                var objUpdate = DB.HisPendingAppSalaryFPs.FirstOrDefault(w => w.PeriodID == objmatch.PeriodID);
                if (objUpdate != null)
                {
                    objUpdate.IsLock = false;
                    DB.HisPendingAppSalaryFPs.Attach(objUpdate);
                    DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                    foreach (var read in _obj)
                    {
                        read.Status = approved;
                        read.LastChangedDate = DateTime.Now;
                        DB.Entry(read).Property(w => w.Status).IsModified = true;
                        DB.Entry(read).Property(w => w.LastChangedDate).IsModified = true;
                        DB.SaveChanges();
                    }
                }
                objmatch.Status = approved;
                objmatch.ChangedOn = DateTime.Now;
                objmatch.ChangedBy = User.UserName;
                DB.HISApproveSalaryFDs.Attach(objmatch);
                DB.Entry(objmatch).Property(w => w.ChangedOn).IsModified = true;
                DB.Entry(objmatch).Property(w => w.ChangedBy).IsModified = true;
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public string RejectSalary(string ASNumber)
        {
            try
            {
               // DBBusinessProcess DBI = new DBBusinessProcess();

                string Reject = SYDocumentStatus.REJECTED.ToString();
                var objmatch = DB.HISApproveSalaryFDs.FirstOrDefault(w => w.ASNumber == ASNumber);
                if (objmatch == null)
                {
                    return "INV_EN";
                }
                if (objmatch.Status == Reject)
                {
                    return "DOC_RJ_AR";
                }
                string open = SYDocumentStatus.OPEN.ToString();
                var listApproval = DB.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocumentType
                                    && w.DocumentNo == objmatch.ASNumber && w.Status == open).OrderBy(w => w.ApproveLevel).ToList();
                var listUser = DB.HRStaffProfiles.Where(w => w.EmpCode == User.UserName).ToList();
                var b = false;
                foreach (var read in listApproval)
                {
                    var checklist = listUser.Where(w => w.EmpCode == read.Approver).ToList();
                    if (checklist.Count > 0)
                    {
                        if (read.Status == SYDocumentStatus.APPROVED.ToString())
                        {
                            return "USER_ALREADY_APP";
                        }

                        if (read.ApproveLevel > listApproval.Min(w => w.ApproveLevel))
                        {
                            return "REQUIRED_PRE_LEVEL";
                        }
                        var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == read.Approver);
                        if (objStaff != null)
                        {
                            read.LastChangedDate = DateTime.Now.Date;
                            read.Status = SYDocumentStatus.REJECTED.ToString();
                            DB.ExDocApprovals.Attach(read);
                            DB.Entry(read).State = System.Data.Entity.EntityState.Modified;
                            b = true;
                            break;
                        }
                    }

                }
                if (listApproval.Count > 0)
                {
                    if (b == false)
                    {
                        return "USER_NOT_APPROVOR";
                    }
                }
                var status = SYDocumentStatus.REJECTED.ToString();
                //var open = SYDocumentStatus.OPEN.ToString();
                // objMatch.IsApproved = true;
                if (listApproval.Where(w => w.Status == open).ToList().Count > 0)
                {
                    status = SYDocumentStatus.REJECTED.ToString();
                    // objMatch.IsApproved = false;
                }

                objmatch.Status = status;
                var objUpdate = DB.HisPendingAppSalaryFPs.FirstOrDefault(w => w.PeriodID == objmatch.PeriodID);
                if (objUpdate != null)
                {
                    objUpdate.IsLock = false;
                    DB.HisPendingAppSalaryFPs.Attach(objUpdate);
                    DB.Entry(objUpdate).Property(w => w.IsLock).IsModified = true;
                }
                DB.Entry(objmatch).Property(w => w.Status).IsModified = true;
                DB.SaveChanges();
                return SYConstant.OK;
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreenId;
                log.UserId = User.UserName;
                log.DocurmentAction = ASNumber;
                log.Action = Humica.EF.SYActionBehavior.RELEASE.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                return "EE001";
            }
        }
        public class SalaryApprovalFP
        {
            public int PeriodID { get; set; }

            [Column(TypeName = "date")]
            public DateTime FromDate { get; set; }

            [Column(TypeName = "date")]
            public DateTime ToDate { get; set; }

            public bool IsLock { get; set; }

        }
        #endregion
    }
}