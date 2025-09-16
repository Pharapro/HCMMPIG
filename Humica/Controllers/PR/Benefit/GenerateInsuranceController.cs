using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Humica.Controllers.PR.Benefit
{

    public class GenerateInsuranceController : Humica.EF.Controllers.MasterSaleController
    {
        private static string Error = "";
        private const string SCREEN_ID = "PRB0000003";
        private const string URL_SCREEN = "/PR/PRM/GenerateInsurance";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SM = new SMSystemEntity();

        public GenerateInsuranceController()
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

            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.ListHeader = new List<HREmpInsurance>();
            BSM.Filter = new FTFilterEmployee();
            BSM.ListHeaderHisInsurance = new List<HisInsurance>();
            BSM.Filter.InMonth = DateTime.Now.Date;
            BSM.Filter.ToDate = DateTime.Now.Date;
            BSM.ListHeaderHisInsurance = DB.HisInsurances.ToList();
            BSM.GridInsurance = BSM.GridList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(HRInsuranceObject BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject obj = new HRInsuranceObject();
            //if (Session[Index_Sess_Obj + ActionName] != null)
            //{
            //    obj = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            //    obj.Filter = BSM.Filter;
            //}
            BSM.ListHeaderHisInsurance = DB.HisInsurances.ToList();
            BSM.GridInsurance = BSM.GridList();
            //BSM.ListEmployeeGen = BSM.LoadDataEmpGen(BSM.Filter,SYConstant.getBranchDataAccess());
            //BSM.Progress = BSM.ListEmployeeGen.Count();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            // Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView(Humica.Models.SY.SYListConfuration.ListDefaultViewSelect, BSM.GridInsurance);
        }
        #endregion
        public ActionResult Generate(string Action, string EmpCode)
        {
            ActionName = Action;
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            var BSM = new HRInsuranceObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListWaiting = SM.SYWaitingBackgrounds.Where(w => w.UserName == user.UserName && w.ScreenId == SCREEN_ID && w.IsFinish == false).ToList();
            if (ListWaiting.Count() == 0)
            {
                string id = EmpCode;
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => this.ReleaseDocWaiting(id, BSM.Filter, cancellationToken));
                int len = EmpCode.Split(';').Length;
                string actionDesc = SYMessages.getMessage("RELEASE_DOC") + " " + SYActionObject.getTitleScreen(SCREEN_ID);

                InsertWaitingProcess(len, actionDesc, SYMessages.getMessage("PROCESSING"));
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WAITING_PRO", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WAITING_FOR_PRO", user.Lang);
            }
            ////string TranNo = EmpCode;
            //if (EmpCode != null)
            //{
            //    var msg = BSM.GenerateInsurance(EmpCode);

            //    if (msg == SYConstant.OK)
            //    {
            //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
            //    }
            //    else
            //    {
            //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            //    }
            //}
            //else
            //{
            //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            //}
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Delete(int ID, string EmpCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            HRInsuranceObject GLA = new HRInsuranceObject();
            string msg = GLA.DeleteInsuranceGenerated(ID, EmpCode);
            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeaderHisInsurance);
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HRInsuranceObject BSM = new HRInsuranceObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];

                BSM.EmpID = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }

        #region Process
        public async void ReleaseDocWaiting(string EmpID, FTFilterEmployee _filter, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                UserSession();
                HRInsuranceObject param = new HRInsuranceObject();
                param.Filter = _filter;
                string userName = user.UserName;
                param.User = user;
                param.BS = bs;
                Error = "";
                HRInsuranceObject.Percen = 0;
                if (EmpID != null)
                {
                    string msg = param.GenerateInsurance(EmpID);
                    if (msg != SYConstant.OK)
                    {
                        Error = msg;
                        ProcessFail(SYMessages.getMessage(msg));

                    }
                    else
                    {
                        Error = "GENERATER_COMPLATED";
                        ProcessDone(SYMessages.getMessage(Error));
                    }
                }
                else
                {
                    ProcessFail(SYMessages.getMessage("DOC_ERQ"));
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(10000);
                ProcessFail();
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreendIDControl;
                log.UserId = user.UserID.ToString();
                log.DocurmentAction = "MASTER_QUOTA";
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);

                /*----------------------------------------------------------*/
            }
        }


        public ActionResult ShowProcess()
        {
            decimal P = 0;
            UserSession();
            P = HRInsuranceObject.Percen;
            var ListWaiting = SM.SYWaitingBackgrounds.Where(w => w.UserName == user.UserName && w.ScreenId == SCREEN_ID && w.IsFinish == false).ToList();

            if (ListWaiting.Count() == 0)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(Error, user.Lang);
                var result = new
                {
                    MS = SYConstant.FAIL,
                    Percen = P
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (Error.Length > 0)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(Error, user.Lang);
                var result = new
                {
                    MS = SYConstant.FAIL,
                    Percen = P
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (P >= 100)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Percen = P
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var result_ = new
            {
                MS = SYConstant.OK,
                Percen = P
            };
            return Json(result_, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private void DataSelector()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DB.HRBranches.ToList();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["CareerHistories_SELECT"] = DB.HRCareerHistories.ToList();
            ViewData["LIST_INSURANCE_TYPE"] = DB.HRInsuranceTypes.ToList();
        }

    }

}
