using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class WorkFlowLeaveController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "WFL0000003";
        private const string URL_SCREEN = "/Config/Setup/WorkFlowLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public WorkFlowLeaveController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            CFWorkFlowLeave BSM = new CFWorkFlowLeave();
            BSM.ListHeader = new List<HRWorkFlowLeave>();
            var PosFamily = DBV.HR_LineMgr_View.ToList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var BSM1 = (CFWorkFlowLeave)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeader = DB.HRWorkFlowLeaves.ToList();
            foreach (var item in ListHeader)
            {
                if (item.AssignBy == "Position")
                {
                    var result = PosFamily.FirstOrDefault(w => w.Code == item.ApproveBy);
                    if (result != null) item.ApproveBy = result.Description;
                }
                BSM.ListHeader.Add(item);
            }
            // BSM.ListHeader = DB.HRWorkFlowLeaves.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            //DataList();
            UserSession();
            UserConfListAndForm(this.KeyName);
            CFWorkFlowLeave BSM = new CFWorkFlowLeave();
            BSM.ListHeader = new List<HRWorkFlowLeave>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CFWorkFlowLeave)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region "Create"

        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            CFWorkFlowLeave BSM = new CFWorkFlowLeave();
            BSM.Header = new HRWorkFlowLeave();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CFWorkFlowLeave)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = new List<HRWorkFlowLeave>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }


        [HttpPost]
        public ActionResult Create(CFWorkFlowLeave collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            CFWorkFlowLeave BSM = new CFWorkFlowLeave();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CFWorkFlowLeave)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            try
            {
                string sms = BSM.CreateApprovalWorkflow();

                if (sms == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new CFWorkFlowLeave();
                    BSM.Header = new HRWorkFlowLeave();
                    return View(BSM);
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);

                }
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserID.ToString();
                log.DocurmentAction = BSM.Header.Code;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
            }
            return null;
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(int ID)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (ID != null)
            {
                CFWorkFlowLeave BSM = new CFWorkFlowLeave();
                BSM.Header = DB.HRWorkFlowLeaves.Find(ID);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WORKFLOW_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(int ID, CFWorkFlowLeave collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new CFWorkFlowLeave();
            if (ID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (CFWorkFlowLeave)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.UpdateApprovalWorkflow(ID);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Details"
        public ActionResult Details(int ID)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (ID != null)
            {
                CFWorkFlowLeave BSM = new CFWorkFlowLeave();
                BSM.Header = DB.HRWorkFlowLeaves.Find(ID);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WORKFLOW_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Delete"
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id.ToString() != "null")
            {
                CFWorkFlowLeave Del = new CFWorkFlowLeave();
                string msg = Del.DeleteWFL(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("ASSIGNBY");
            ViewData["ASSIGNBY_SELECT"] = objList.ListData;

            ViewData["POSITION_SELECT"] = DBV.HR_LineMgr_View.ToList();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["WorkFlow_SELECT"] = DB.HRWorkFlowLeaves.ToList();
            objList = new SYDataList("WORKFLOW_STATE");
            ViewData["WORKFLOW_STATE_SELECT"] = objList.ListData;
        }
    }
}