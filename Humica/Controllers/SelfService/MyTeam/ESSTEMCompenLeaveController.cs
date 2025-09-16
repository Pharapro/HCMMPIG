using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class ESSTEMCompenLeaveController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000019";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSTEMCompenLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public ESSTEMCompenLeaveController()
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
            UserConfList(this.KeyName);
            DataSelector();

            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeaveReq = new List<HRClaimLeave>();
            var staff = DB.HRStaffProfiles.ToList();
            BSM.FInYear.Status = SYDocumentStatus.PENDING.ToString();
            if (staff.Where(w => w.EmpCode == user.UserName).ToList().Count > 0)
            {
                string approved = SYDocumentStatus.APPROVED.ToString();
                string Reject = SYDocumentStatus.REJECTED.ToString();
                string Cancel = SYDocumentStatus.CANCELLED.ToString();
                string Open = SYDocumentStatus.OPEN.ToString();
                var ListApp = DB.ExDocApprovals.Where(w => w.Status == Open && w.Approver == user.UserName).ToList();
                var ListEmpLEave = DB.HRClaimLeaves.ToList();
                ListEmpLEave = ListEmpLEave.Where(w => ListApp.Where(x => x.DocumentNo == w.TranNo.ToString()).Any()).ToList();
                var ListEmpLeaveReq = DBV.HRClaimLeaves.Where(w => w.WorkingDate.Year == BSM.FInYear.INYear).ToList();
                ListEmpLeaveReq = ListEmpLeaveReq.Where(w => ListEmpLEave.Where(x => x.EmpCode == w.EmpCode && w.TranNo == x.TranNo).Any()).ToList();


                ListEmpLeaveReq = ListEmpLeaveReq.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();

                BSM.ListEmpLeaveReq = ListEmpLeaveReq.ToList();
            }

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClaimLeaveObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeaveReq = new List<HRClaimLeave>();
            var listWordFlow = DB.HRWorkFlowLeaves.ToList();
            BSM.FInYear = collection.FInYear;
            var staff = DB.HRStaffProfiles.ToList();
            if (staff.Where(w => w.EmpCode == user.UserName).ToList().Count > 0)
            {
                string Open = SYDocumentStatus.OPEN.ToString();
                var ListApp = DB.ExDocApprovals.Where(w => w.Status == Open && w.Approver == user.UserName).ToList();
                var ListEmpLEave = DB.HRClaimLeaves.ToList();
                ListEmpLEave = ListEmpLEave.Where(w => ListApp.Where(x => x.DocumentNo == w.TranNo.ToString()).Any()).ToList();
                var ListEmpLeaveReq = DBV.HRClaimLeaves.Where(w => w.WorkingDate.Year == BSM.FInYear.INYear).ToList();
                // var Division = staff.Where(w => w.EmpCode == user.UserName).ToList();
                // var _listEmp = staff.Where(w => Division.Where(x => x.Division == w.Division && w.DEPT == x.DEPT).Any()).ToList();
                ListEmpLeaveReq = ListEmpLeaveReq.Where(w => ListEmpLEave.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                if (BSM.FInYear.Status != null)
                {
                    if (BSM.FInYear.Status.ToString() == SYDocumentStatus.PENDING.ToString())
                    {
                        string approved = SYDocumentStatus.APPROVED.ToString();
                        string Reject = SYDocumentStatus.REJECTED.ToString();
                        string Cancel = SYDocumentStatus.CANCELLED.ToString();
                        ListEmpLeaveReq = ListEmpLeaveReq.Where(x => x.Status != approved && x.Status != Reject && x.Status != Cancel).ToList();
                    }
                    else
                    {
                        ListEmpLeaveReq = ListEmpLeaveReq.Where(w => w.Status == BSM.FInYear.Status).ToList();
                    }
                }
                BSM.ListEmpLeaveReq = ListEmpLeaveReq.ToList();
            }

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpLeaveReq);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header = new HRClaimLeave();
            BSM.Header.WorkingDate = DateTime.Now;
            BSM.Header.WorkingHour = 8;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ClaimLeaveObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new ClaimLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
                BSM.Header.EmpCode = collection.HeaderStaff.EmpCode;
            }
            if (ModelState.IsValid)
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSTEMCompenLeave/Details/";
                string msg = BSM.ClaimLeave(fileName, true);// (ID, URL, fileName);

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.Header.TranNo.ToString();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (!string.IsNullOrEmpty(id))
            {
                int TranNo = Convert.ToInt32(id);
                BSM.Header = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);

                string Status = BSM.Header.Status;

                if (BSM.Header != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.Header.EmpCode);
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "REQ_CL").ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Approve"
        public ActionResult Approve(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (id.ToString() != "null")
            {

                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/ESSTEMCompenLeave/Details/";
                string msg = BSM.approveTheDoc(id, URL);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Reject"
        public ActionResult Reject(string id)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                ClaimLeaveObject BSM = new ClaimLeaveObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectLeave(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);

        }
        #endregion

        //#region "Cancel"
        //public ActionResult Cancel(string id)
        //{
        //    //ActionName = "Details";
        //    this.UserSession();
        //    DataSelector();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID] = id;
        //    ClaimLeaveObject BSD = new ClaimLeaveObject();
        //    if (id.ToString() != "null")
        //    {
        //        string sms = BSD.CancelLeave(id);
        //        if (sms == "OK")
        //        {
        //            SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
        //            messageObject.DocumentNumber = id;
        //            messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
        //            SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
        //            Session[SYConstant.MESSAGE_SUBMIT] = mess;
        //        }
        //        else
        //        {
        //            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
        //        }
        //        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        //    }

        //    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        //}
        //#endregion
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
        }

        #region "Ajax Approval"
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

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                    }

                    //var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    //if (msg == SYConstant.OK)
                    //{

                    //    if (BSM.ListApproval.Count == 0)
                    //    {
                    //        ModelObject.ID = 1;
                    //    }
                    //    else
                    //    {
                    //        ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                    //    }
                    //    //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                    //    ModelObject.DocumentNo = "N/A";
                    //    BSM.ListApproval.Add(ModelObject);
                    //}
                    //else
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage(msg);
                    //}
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListApproval.Where(w => w.Approver == Approver).ToList();
                    if (BSM.ListApproval.Count > 0)
                    {
                        var objDel = BSM.ListApproval.Where(w => w.Approver == Approver).First();
                        BSM.ListApproval.Remove(objDel);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("APPROVER_NE");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }

        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                Session["EmpCode"] = EmpCode;
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
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
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("CLAIM_LEAVE");
            ViewData["WORKINGTYPE_SELECT"] = objList.ListData;
            ViewData["LEAVETYPE_SELECT"] = DB.HRLeaveTypes.Where(x => x.IsParent != true).ToList().OrderBy(w => w.Description);

            var ListBranch = SYConstant.getBranchDataAccess();

            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (staff != null)
            {
                var _listEmp = DB.HRStaffProfiles.Where(w => w.OTFirstLine == staff.EmpCode || w.OTSecondLine == staff.EmpCode || w.HODCode == staff.EmpCode).ToList();
                ListStaff = _listEmp.ToList();
            }
            ViewData["STAFF_SELECT"] = ListStaff.ToList();
        }
    }
}
