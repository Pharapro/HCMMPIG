using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{
    public class HRClaimLeaveController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRL0000007";
        private const string URL_SCREEN = "/HRM/Leave/HRClaimLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRClaimLeaveController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            ClaimLeaveObject BSM = new ClaimLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HRClaimLeaves.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.ListHeader = new List<HRClaimLeave>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            ClaimLeaveObject BSD = new ClaimLeaveObject();

            BSD.Header = new HRClaimLeave();
            BSD.ListHeader = new List<HRClaimLeave>();
            BSD.ListApproval = new List<ExDocApproval>();
            BSD.Header.WorkingDate = DateTime.Now;
            BSD.Header.WorkingHour = 8;

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(ClaimLeaveObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            BSM.Header = collection.Header;

            string msg = BSM.ClaimLeave();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.TranNo.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

                BSM = NewClaimLeave();

                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            }
            else
            {
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (id != "null")
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "REQ_CL").ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Edit'
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClaimLeaveObject BSM = new ClaimLeaveObject();

            if (id != "null")
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClaimLeaveObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            ClaimLeaveObject BSM = new ClaimLeaveObject();

            BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];

            collection.ScreenId = SCREEN_ID;

            if (ModelState.IsValid)
            {
                string msg = collection.UpdateClaimLeave(id);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber; ;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Delete'  
        public ActionResult Delete(string id)
        {
            UserSession();
            ClaimLeaveObject BSM = new ClaimLeaveObject();

            if (id != "null")
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    string msg = BSM.DeleteClaimLeav(id);

                    if (msg == SYConstant.OK)
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }
        #region 'Private Code'
        public ActionResult ShowData(string EmpCode, string Action)
        {
            ActionName = Action;

            ClaimLeaveObject BSM = new ClaimLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }

            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);

            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private ClaimLeaveObject NewClaimLeave()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            ClaimLeaveObject BSD = new ClaimLeaveObject();

            BSD.Header = new HRClaimLeave();
            BSD.ListHeader = new List<HRClaimLeave>();
            BSD.Header.WorkingDate = DateTime.Now;
            BSD.Header.WorkingHour = 8;

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return BSD;
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("CLAIM_LEAVE");
            ViewData["WORKINGTYPE_SELECT"] = objList.ListData;
            ViewData["EMPCODE_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["LEAVETYPE_SELECT"] = DB.HRLeaveTypes.Where(x => x.IsParent != true).ToList().OrderBy(w => w.Description);
        }
        #endregion 
    }

}
