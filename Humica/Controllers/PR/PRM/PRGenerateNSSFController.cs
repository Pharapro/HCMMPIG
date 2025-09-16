using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{

    public class PRGenerateNSSFController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000008";
        private const string URL_SCREEN = "/PR/PRM/PRGenerateNSSF/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public PRGenerateNSSFController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }


        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            PRGenerate_Salary BSM = new PRGenerate_Salary();
            BSM.Filter = new Humica.Core.FT.FTFilerPayroll();
            BSM.ListEmployeeGen = new List<HR_View_EmpGenSalary>();
            DateTime DateNow = DateTime.Now;
            BSM.Filter.Period = 0;
            BSM.Filter.ValueDate = DateNow;
            BSM.Filter.ExchangeRate = 0;
            var Exchange = DB.PRExchRates.FirstOrDefault(w => w.PeriodID == BSM.Filter.Period);
            if (Exchange != null) BSM.Filter.ExchangeRate = Exchange.NSSFRate;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        public ActionResult Generate(int PeriodID, string Branch)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRGenerate_Salary();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
            }
            if (Branch == "null") Branch = null;
            BSM.Filter.Branch = Branch;
            //BSM.Filter.InMonth = PeriodID;
            var msg = BSM.Generate_NSSF(PeriodID, SYConstant.getBranchDataAccess());

            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Transfer(int PeriodID, string Branch)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRGenerate_Salary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
            }
            if (Branch == "null") Branch = null;
            BSM.Filter.Branch = Branch;
            //BSM.Filter.InMonth = InMonth;
            var msg = BSM.Transfer_NSSF(PeriodID, SYConstant.getBranchDataAccess());

            if (msg == SYConstant.OK)
            {
                //Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TRANSFER_COMPLATED", user.Lang);
                string fileName = Server.MapPath("~/Content/TEMPLATE/E-Form_v10.accdb");
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=E-Form_v10.accdb");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                if (!Directory.Exists(fileName))
                {

                }
                Response.WriteFile(fileName);
                Response.End();
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult DownloadLeter(int PeriodID, string Branch, DateTime ValueDate)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            string fileName = Server.MapPath("~/Content/TEMPLATE/NSSFLatter.pdf");
            string fileName_new = Server.MapPath("~/Content/TEMPLATE/NSSFLatter_new.pdf");
            string[] _Branch = Branch.Split(',');
            List<string> LstBranch = new List<string>();
            foreach (var read in _Branch)
            {
                if (read.Trim() != "")
                {
                    LstBranch.Add(read.Trim());
                }
            }
            //var _Listbranch = SYConstant.getBranchDataAccess();
            //var _branch = _Listbranch.FirstOrDefault(w => w.Code == Branch);
            var Period = DBV.PRPayPeriodItems.FirstOrDefault(w => w.PeriodID == PeriodID);
            var ListSalary = DBV.NSSF_Latter.Where(w => w.INYear == Period.EndDate.Year && w.INMonth == Period.EndDate.Month).ToList();
            if (ListSalary.Count() > 0)
            {
                ListSalary = ListSalary.Where(w => LstBranch.Contains(w.Branch)).ToList();
                NSSF_Latter objNSSF = new NSSF_Latter();
                objNSSF.Health = 0;
                objNSSF.SOSEC = 0;
                objNSSF.PensionFund = 0;
                objNSSF.Count_Sex = 0;
                objNSSF.Count_Female = 0;
                string NSSFNo = "";
                foreach (var read in ListSalary)
                {
                    if (NSSFNo == read.NSSFNo)
                    {
                    }
                    objNSSF.CompKHM = read.CompKHM;
                    objNSSF.CompAct = read.CompAct;
                    objNSSF.DirName = read.DirName;
                    objNSSF.Phone = read.Phone;
                    objNSSF.HDStreet = read.HDStreet;
                    objNSSF.HDCommune = read.HDCommune;
                    objNSSF.HDDistrict = read.HDDistrict;
                    objNSSF.HDProvince = read.HDProvince;
                    objNSSF.Email = read.Email;
                    objNSSF.FAX = read.FAX;
                    objNSSF.Health += read.Health;
                    objNSSF.SOSEC += read.SOSEC;
                    objNSSF.PensionFund += read.PensionFund;
                    objNSSF.Count_Sex += read.Count_Sex;
                    objNSSF.Count_Female += read.Count_Female;
                    objNSSF.NSSFNo = read.NSSFNo;
                    objNSSF.INYear = read.INYear;
                    objNSSF.INMonth = read.INMonth;
                    objNSSF.Branch = read.Branch;
                    objNSSF.FromDate = read.FromDate;
                    objNSSF.ToDate = read.ToDate;
                    objNSSF.HDHouse = read.HDHouse;
                    NSSFNo = read.NSSFNo;

                }
                var msg = SYExecuteFindAndReplace.ExtractRegexNSSF(fileName, fileName_new, objNSSF, ValueDate);

                if (msg == SYConstant.OK)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=NSSFLatter.pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.WriteFile(fileName_new);
                    Response.End();
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_RANGE_SELECTED", user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult ShowData(int PeriodID)
        {
            decimal Exch = 0;
            var ExchValue = DB.PRExchRates.FirstOrDefault(w => w.PeriodID == PeriodID);
            if (ExchValue != null) Exch = ExchValue.NSSFRate;
            var result = new
            {
                MS = SYConstant.OK,
                Exchange = Exch
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ClsFilterJob clsFilter = new ClsFilterJob();
            ViewData["PERIOD_SELECT"]= clsFilter.LoadPeriod().Where(w => w.IsActive == true);
            // ViewData["DEPARTMENT_SELECT"] = DH.HRDepartments.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            //ViewData["SECTION_SELECT"] = DH.HRSections.ToList();
            //ViewData["POSITION_SELECT"] = DH.HRPositions.ToList();
            //ViewData["DIVISION_SELECT"] = DH.HRDivisions.ToList();
        }

    }
}
