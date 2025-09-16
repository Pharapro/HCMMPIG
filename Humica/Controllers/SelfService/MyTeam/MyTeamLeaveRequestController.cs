using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class MyTeamLeaveRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000005";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyTeamLeaveRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public MyTeamLeaveRequestController()
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

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.FInYear = new Core.FT.FTFilerIndex();
            BSM.FInYear.InYear = DateTime.Now.Year;
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeaveReq = new List<HR_VIEW_EmpLeave>();
            BSM.FInYear.Status = SYDocumentStatus.PENDING.ToString();
            var userName = user.UserName;
            BSM.LoadData(userName);
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(GenerateLeaveObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataSelector();
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeaveReq = new List<HR_VIEW_EmpLeave>();
            BSM.FInYear = collection.FInYear;
            var userName = user.UserName;
            if (BSM.FInYear.Status != null)
            {
                BSM.LoadData(userName);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
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
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ListEmpLeaveD = new List<HREmpLeaveD>();
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeave = new List<HREmpLeave>();
            BSM.EmpLeaveB = new HREmpLeaveB();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.HeaderEmpLeave = new HREmpLeave();
            BSM.HeaderEmpLeave.FromDate = DateTime.Now;
            BSM.HeaderEmpLeave.ToDate = DateTime.Now;
            Session[PATH_FILE] = null;
            BSM.HeaderEmpLeave.Units = "Day";
            BSM.Units = "Day";
            Session["LEAVE_TYEP"] = null;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, GenerateLeaveObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new GenerateLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderEmpLeave = collection.HeaderEmpLeave;
                BSM.HeaderStaff = collection.HeaderStaff;
                if (Session[PATH_FILE] != null)
                {
                    BSM.HeaderEmpLeave.Attachment = Session[PATH_FILE].ToString();
                }
            }
            if (ModelState.IsValid)
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamLeaveRequest/Details/";
                string msg = BSM.ESSRequestLeave(ID, URL);

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderEmpLeave.TranNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.HeaderEmpLeave.TranNo.ToString();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    mess.DocumentNumber = BSM.HeaderEmpLeave.TranNo.ToString();
                    if (!string.IsNullOrEmpty(BSM.MessageError))
                    {
                        mess.Description = string.Format(mess.Description, BSM.MessageError);
                    }
                    if (!string.IsNullOrEmpty(BSM.MessageError) && !string.IsNullOrEmpty(BSM.MessageError2))
                    {
                        mess.Description = string.Format(mess.Description, BSM.MessageError, BSM.MessageError2);
                    }
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.HeaderEmpLeave = new HREmpLeave();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            string re = id;
            if (id == "null") id = null;
            if (id != null)
            {
                BSM.ListApproverLeave = new List<HRApproverLeave>();
                int TranNo = Convert.ToInt32(id);
                BSM.HeaderEmpLeave = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo);


                string Status = BSM.HeaderEmpLeave.Status;
                if (BSM.HeaderEmpLeave != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.HeaderEmpLeave.EmpCode);

                    string DocumentNo = BSM.HeaderEmpLeave.Increment.ToString();
                    var LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == BSM.HeaderEmpLeave.LeaveCode);
                    var LeaveB = DB.HREmpLeaveBs.Where(w => w.EmpCode == BSM.HeaderEmpLeave.EmpCode && w.InYear == BSM.HeaderEmpLeave.ToDate.Year).ToList();
                    string LeaveCode = BSM.HeaderEmpLeave.LeaveCode;
                    if (LeaveType.IsParent == true)
                    {
                        LeaveCode = LeaveType.Parent;
                    }
                    HREmpLeaveB LeaveBanace = LeaveB.FirstOrDefault(w => w.LeaveCode == LeaveCode);
                    Session["LEAVE_TYEP"] = LeaveType.Code;
                    BSM.EmpLeaveB = LeaveBanace;
                    BSM.ListEmpLeaveD = DB.HREmpLeaveDs.Where(x => x.LeaveTranNo == BSM.HeaderEmpLeave.Increment && x.EmpCode == BSM.HeaderEmpLeave.EmpCode).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == DocumentNo && w.DocumentType == "LR").OrderBy(w => w.ApproveLevel).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, GenerateLeaveObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                    if (Session[PATH_FILE] != null)
                    {
                        BSM.HeaderEmpLeave.Attachment = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        collection.HeaderEmpLeave.Attachment = BSM.HeaderEmpLeave.Attachment;
                    }
                    BSM.HeaderEmpLeave = collection.HeaderEmpLeave;
                }
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditLeaveRequest(TranNo, true);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    mess.DocumentNumber = id;
                    if (!string.IsNullOrEmpty(BSM.MessageError))
                    {
                        mess.Description = string.Format(mess.Description, BSM.MessageError);
                    }
                    if (!string.IsNullOrEmpty(BSM.MessageError) && !string.IsNullOrEmpty(BSM.MessageError2))
                    {
                        mess.Description = string.Format(mess.Description, BSM.MessageError, BSM.MessageError2);
                    }
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                GenerateLeaveObject Del = new GenerateLeaveObject();
                int TranNo = Convert.ToInt32(id);
                string msg = Del.DeleteLeave(TranNo);
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
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                BSM.ListApproverLeave = new List<HRApproverLeave>();
                int TranNo = Convert.ToInt32(id);
                BSM.HeaderEmpLeave = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo);

                string Status = BSM.HeaderEmpLeave.Status;
                if (Session[PATH_FILE] != null)
                {
                    BSM.HeaderEmpLeave.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (BSM.HeaderEmpLeave != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.HeaderEmpLeave.EmpCode);
                    string DocumentNo = BSM.HeaderEmpLeave.Increment.ToString();
                    BSM.ListEmpLeaveD = DB.HREmpLeaveDs.Where(x => x.LeaveTranNo == BSM.HeaderEmpLeave.Increment && x.EmpCode == BSM.HeaderEmpLeave.EmpCode).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == DocumentNo && w.DocumentType == "LR").OrderBy(w => w.ApproveLevel).ToList();

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
        public ActionResult Approve(string id,string Remark)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamLeaveRequest/";
                string msg = BSM.approveTheDoc(id, URL, Remark);
                //string msg = BSM.ApproveReqLeave(id, URL);
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
        public ActionResult Reject(string id,string Remark)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                GenerateLeaveObject BSM = new GenerateLeaveObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectLeave(id, Remark, false);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
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

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            //ActionName = "Details";
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSD = new GenerateLeaveObject();
            if (id.ToString() != "null")
            {
                string sms = "";// BSD.CancelLeave(id, false);
                if (sms == "OK")
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion
        public ActionResult EditLeave(HREmpLeaveD MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContext();
                    var ListEmpLeaveD = BSM.ListEmpLeaveD.Where(w => w.LineItem == MModel.LineItem).ToList();
                    if (ListEmpLeaveD.Count > 0)
                    {
                        var objUpdate = ListEmpLeaveD.First();
                        if (MModel.StartTime.HasValue)
                        {
                            var totals = MModel.EndTime.Value.Subtract(MModel.StartTime.Value).TotalHours;
                            objUpdate.StartTime = MModel.StartTime;
                            objUpdate.EndTime = MModel.EndTime;
                            objUpdate.LHour = (decimal)totals;
                            var para = DB.ATPolicies.FirstOrDefault();
                            if (para != null && para.LHourByRate == true)
                            {
                                var totalMin = (MModel.EndTime.Value - MModel.StartTime.Value).TotalMinutes;
                                var LPolicy = DB.HRLeaveHourPolicies.FirstOrDefault(w => w.From < totalMin && w.To >= totalMin);
                                if (LPolicy != null) objUpdate.LHour = LPolicy.Rate / 60m;
                            }
                        }
                        objUpdate.Remark = MModel.Remark;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
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
            DataSelector();
            return PartialView("GridItemDetails", BSM);
        }
        public ActionResult EditLeaveEdit(HREmpLeaveD MModel)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContext();
                    var ListEmpLeaveD = BSM.ListEmpLeaveD.Where(w => w.LineItem == MModel.LineItem).ToList();
                    if (ListEmpLeaveD.Count > 0)
                    {
                        var objUpdate = ListEmpLeaveD.First();
                        objUpdate.Remark = MModel.Remark;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
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
            DataSelector();
            return PartialView("GridItemEdit", BSM.ListEmpLeaveD);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpLeave);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM);
        }
        public ActionResult GridItemEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemEdit";
            return PartialView("GridItemEdit", BSM.ListEmpLeaveD);
        }
        public ActionResult GridItemViewDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemViewDetails";
            return PartialView("GridItemViewDetails", BSM);
        }

        public ActionResult GridItemApprover()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemApprover";
            return PartialView("GridItemApprover", BSM.ListApproverLeave);
        }
        #region "Ajax Approval"
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
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
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
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
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        [HttpPost]
        public ActionResult ShowData(string ID, DateTime FromDate, DateTime ToDate, string EmpCode, string Units, string Action)
        {

            ActionName = Action;

            var Policy = DB.ATPolicies.FirstOrDefault();
            DateTime From = Policy.LFromDate;
            int year = FromDate.Year;
            if (From.Month == 12 && FromDate.Month == From.Month && FromDate.Day >= From.Day)
                year = year + 1;

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListEmpLeaveD = new List<HREmpLeaveD>();
            }
            if (EmpCode == "")
            {
                var rs1 = new { MS = "INVALIT_EMPLOYEE" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (ID == "" || ID == null)
            {
                var rs1 = new { MS = "LeaveType_INVALIT" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            BSM.Units = Units;
            ClsValidateLeave Leave = new ClsValidateLeave();
            if (EmpCode != null)
            {
                string LeaveCode = ID;
                BSM.ListEmpLeaveD = Leave.Get_LeaveDay(EmpCode, FromDate, ToDate, ID, Units);
                HRLeaveType LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == ID);
                Session["LEAVE_TYEP"] = LeaveType.Code;
                //var LeaveBanace = DB.HREmpLeaveBs.FirstOrDefault(x => x.EmpCode == EmpCode && x.InYear == year && x.LeaveCode == ID);
                if (LeaveType.IsParent && !string.IsNullOrEmpty(LeaveType.Parent))
                {
                    LeaveCode = LeaveType.Parent;
                }
                Session["LEAVE_TYEP"] = LeaveType.Code;
                Session[Index_Sess_Obj + ActionName] = BSM;
                var LeaveBanace = DB.HREmpLeaveBs.FirstOrDefault(x => x.EmpCode == EmpCode
                && x.InYear == year && x.LeaveCode == LeaveCode);
                if (LeaveBanace == null)
                {
                    var LB = new { MS = "Pleas Generate Leave Entitle" };
                    return Json(LB, JsonRequestBehavior.DenyGet);
                }
                BSM.EmpLeaveB = LeaveBanace;
                var leaveCount = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Leave.ToString()).Count();
                var balance = BSM.EmpLeaveB.Balance ?? 0;
                decimal Bal_ = balance - leaveCount;
                if (ID == "MS" && BSM.HeaderStaff.CompanyCode == "LEGB")
                {
                    Bal_ = 0;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    NoDay = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Leave.ToString()).Count(),
                    NoPH = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.PH.ToString()).Count(),
                    NoRest = BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Rest.ToString()).Count(),
                    Balance = Bal_,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
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
        public ActionResult ShowUNITS(string ID, string Action)
        {
            ActionName = Action;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            var message = SYConstant.OK;
            if (ID == "Hours")
            {
                SYSetting _list = DB.SYSettings.FirstOrDefault(w => w.SettingName == "REQUEST_HOURS");
                if (_list != null)
                {
                    if (_list.SettinValue == "FALSE")
                    {
                        message = "Not allow request by hours!";
                        //var rs1 = new { MS = "Not allow request by hours!" };
                        //return Json(rs1, JsonRequestBehavior.DenyGet);
                    }
                    ID = "Day";
                }
            }
            BSM.Units = ID;

            Session[Index_Sess_Obj + ActionName] = BSM;
            var rs = new
            {
                MS = message,
                Units = ID
            };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ReceiptTotal(string Action)
        {
            ActionName = Action;
            var BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
                updateTotalReceipt(BSM, Action);
                decimal No_ = (decimal)BSM.HeaderEmpLeave.NoDay;
                if (BSM.EmpLeaveB.LeaveCode == "MS" && BSM.HeaderStaff.CompanyCode == "LEGB")
                {
                    BSM.EmpLeaveB.Balance = 0;
                    No_ = 0;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    NoDay = BSM.HeaderEmpLeave.NoDay,
                    NoPH = BSM.HeaderEmpLeave.NoPH,
                    NoRest = BSM.HeaderEmpLeave.NoRest,
                    Balance = BSM.EmpLeaveB.Balance - No_,
                };

                return Json(result, (JsonRequestBehavior)1);
            }
            var data1 = new { MS = "FAIL" };
            return Json(data1, (JsonRequestBehavior)1);
        }
        private void updateTotalReceipt(GenerateLeaveObject BSM, string Action)
        {
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            if (BSM.HeaderEmpLeave == null)
            {
                BSM.HeaderEmpLeave = new HREmpLeave();
            }
            else
            {
                BSM.HeaderEmpLeave.NoDay = 0;
                BSM.HeaderEmpLeave.NoPH = 0;
                BSM.HeaderEmpLeave.NoRest = 0;
            }
            double WHour = 0;
            var _PayPram = DB.HRStaffProfiles.Where(w => w.EmpCode == BSM.HeaderStaff.EmpCode).ToList();
            if (_PayPram.Count() > 0)
            {
                var obj = _PayPram.First().PayParam;
                var Pay = DB.PRParameters.Find(obj);
                WHour = Convert.ToDouble(Pay.WHOUR);
            }
            double LHour = 0;
            foreach (var item in BSM.ListEmpLeaveD)
            {
                LHour = WHour;
                if (BSM.Units != "Day")
                {
                    LHour = (double)item.LHour;
                }
                if (item.Remark == "Morning" || item.Remark == "Afternoon")
                {
                    LHour = Convert.ToDouble(WHour / 2);
                }
                LHour = LHour / WHour;
                if (item.Status == "Leave")
                    BSM.HeaderEmpLeave.NoDay += LHour;
                else if (item.Status == "PH")
                    BSM.HeaderEmpLeave.NoPH += LHour;
                else if (item.Status == "Rest")
                    BSM.HeaderEmpLeave.NoRest += LHour;
            }
        }
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath; ;
            return null;
        }

        public ActionResult LeaveType()
        {
            UserSession();
            UserConfListAndForm();
            if (Session["EmpCode"] != null)
            {
                string empCode = Session["EmpCode"].ToString();
                var Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == empCode);
                DateTime data = DateTime.Now;
                DateTime EndDate = new DateTime(data.Year, 12, 31);
                DateTime StartDate = Convert.ToDateTime(Staff.LeaveConf);
                int WorkMonth = DateTimeHelper.CountMonth(StartDate, EndDate);
                List<HRLeaveType> ListLeave = DB.HRLeaveTypes.ToList();
                ListLeave = GenerateLeaveObject.GetLeaveType(ListLeave, Staff.TemLeave, Staff.Sex, WorkMonth, true);
                ViewData["LeaveTypes_SELECT"] = ListLeave.ToList();

                return PartialView("LeaveType_");
            }
            return null;
        }

        private void DataSelector()
        {
            SYDataList objList = new SYDataList("LEAVE_TIME");
            if (Session["LEAVE_TYEP"] != null)
            {
                string _Leave = Session["LEAVE_TYEP"].ToString();
                var _listLeave = DB.HRLeaveTypes.Where(w => w.Code == _Leave).ToList();
                if (_listLeave.Where(w => w.IsParent == true).Any())
                {
                    //objList.ListData = objList.ListData.Where(w => w.SelectValue != "FullDay").ToList();
                    objList.ListData = objList.ListData.ToList();
                }
            }
            ViewData["LEAVE_TIME_SELECT"] = objList.ListData;

            //ViewData["LeaveTypes_SELECT"] = DB.HRLeaveTypes.ToList();
            var ListBranch = SYConstant.getBranchDataAccess();
            DateTime date = DateTime.Now.AddMonths(-1);
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            var ListStaff = new List<HRStaffProfile>();
            if (staff != null)
            {
                var _listEmp = DB.HRStaffProfiles.Where(w => w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode 
                || w.FirstLine2 == staff.EmpCode || w.SecondLine2 == staff.EmpCode  || w.EmpCode == staff.EmpCode).ToList();
                _listEmp = _listEmp.Where(w => w.Status == "A" || w.DateTerminate > DateTime.Now).ToList();
                if (_listEmp.Count > 0)
                {
                    ListStaff = _listEmp.ToList();
                }
                else
                {
                    ListStaff = DB.HRStaffProfiles.AsEnumerable().Where(w => ListBranch.AsEnumerable().Where(x => x.Code == w.Branch).Any()).ToList();
                    ListStaff = ListStaff.Where(w => w.DEPT == staff.DEPT && (w.Status == "A" || (w.DateTerminate > date && w.Status == "I"))).ToList();
                }
            }
            ViewData["UNITS_SELECT"] = ClsUnits.LoadUnit();
            ViewData["STAFF_SELECT"] = ListStaff.ToList();

            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS_APPROVAL"] = objList.ListData;
        }
    }
}
