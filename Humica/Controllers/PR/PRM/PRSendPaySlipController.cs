using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{

    public class PRSendPaySlipController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000024";
        private const string URL_SCREEN = "/PR/PRM/PRSendPaySlip/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();

        public PRSendPaySlipController()
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
            BSM.ListEmpPaySlip = new List<HR_PR_EmpSalary>();
            BSM.Filter = new Core.FT.FTFilerPayroll();
            BSM.Filter.InMonth = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PRGenerate_Salary BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.ListEmpPaySlip = BSM.LoadEmpPaySlip(BSM.Filter, SYConstant.getBranchDataAccess());

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        #endregion
        public ActionResult SendToTelegram()
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
            string TranNo = BSM.EmpID;
            if (TranNo != null)
            {
                string[] Emp = TranNo.Split(';');
                RptPaySlipByEmp sa = new RptPaySlipByEmp();
                var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == "ESS0000009"
                   && w.IsActive == true).ToList();
                if (objRpt.Count > 0)
                {
                    sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                }
                var EmailTemplate = SMS.TPEmailTemplates.Find("Send_PAYSLIP");
                var Telegram = DB.TelegramBots.FirstOrDefault();
                //var ListSTaff = DB.HRStaffProfiles.ToList();
                List<object> ListObjectDictionary = new List<object>();
                //ListObjectDictionary.Add(Staff);
                foreach (var Code in Emp)
                {
                    var _Tele = BSM.ListEmpPaySlip.Where(w => w.EmpCode == Code).ToList();
                    sa.Parameters["InMonth"].Value = BSM.Filter.InMonth;
                    sa.Parameters["InMonth"].Visible = false;
                    sa.Parameters["EmpCode"].Value = Code;
                    sa.Parameters["EmpCode"].Visible = false;
                    string FileName = Server.MapPath("~/Content/UPLOAD/Report/PaySlip_" + Code + ".png");

                    sa.ExportToImage(FileName);
                    var Chart_ID = _Tele.FirstOrDefault().TelegramID;

                    if (Telegram != null)
                    {
                        PRSendTelegram Tele = new PRSendTelegram();
                        Tele.Send_PaySlip_Telegram(Chart_ID, FileName, Telegram.TokenID, _Tele.FirstOrDefault());
                    }
                    else
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);

                }
                //var msg = BSM.GenerateSalarys(TranNo);

                //if (msg == SYConstant.OK)
                //{
                //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                //}
                //else
                //{
                //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                //}
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpPaySlip);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            PRGenerate_Salary BSM = new PRGenerate_Salary();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];

                BSM.EmpID = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        private void DataSelector()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DB.HRBranches.ToList();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["CareerHistories_SELECT"] = DB.HRCareerHistories.ToList();
        }
    }

}
