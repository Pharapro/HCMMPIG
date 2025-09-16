using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Integration.EF.Models;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class LeaveRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000002";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/LeaveRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public LeaveRequestController()
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

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.FInYear = new Core.FT.FTFilerIndex();
            BSM.FInYear.InYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            var listWordFlow = DB.HRWorkFlowLeaves.ToList();
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeave = new List<HREmpLeave>();
            var ListLeave = DB.HREmpLeaves.Where(w => w.FromDate.Year == BSM.FInYear.InYear && w.EmpCode == BSM.User.UserName).OrderByDescending(x => x.FromDate).ToList();
            foreach (var item in ListLeave)
            {
                var result = listWordFlow.FirstOrDefault(w => w.Code == item.Status);
                if (result != null)
                {
                    item.Status = result.Description;
                }
                BSM.ListEmpLeave.Add(item);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(GenerateLeaveObject collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListEmpLeave = new List<HREmpLeave>();
            var listWordFlow = DB.HRWorkFlowLeaves.ToList();
            BSM.FInYear = collection.FInYear;
            var ListLeave = DB.HREmpLeaves.Where(w => w.FromDate.Year == collection.FInYear.InYear && w.EmpCode == BSM.User.UserName).ToList();
            ListLeave = ListLeave.Where(w => w.EmpCode == BSM.User.UserName).OrderByDescending(x => x.FromDate).ToList();
            foreach (var item in ListLeave)
            {
                var result = listWordFlow.FirstOrDefault(w => w.Code == item.Status);
                if (result != null)
                {
                    item.Status = result.Description;
                }
                BSM.ListEmpLeave.Add(item);
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
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpLeaveB);
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

            var emp = DBV.HR_STAFF_VIEW.Where(w => w.EmpCode == BSM.User.UserName).ToList();
            if (emp.Count > 0)
            {
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.EmpLeaveB = new HREmpLeaveB();
                BSM.HeaderEmpLeave = new HREmpLeave();
                BSM.HeaderEmpLeave.Urgent = false;
                BSM.HeaderEmpLeave.FromDate = DateTime.Now;
                BSM.HeaderEmpLeave.ToDate = DateTime.Now;
                BSM.HeaderEmpLeave.Units = "Day";
                BSM.Units = "Day";
                Session["LEAVE_TYEP"] = null;
                Session[PATH_FILE] = null;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
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
                if (Session[PATH_FILE] != null)
                {
                    BSM.HeaderEmpLeave.Attachment = Session[PATH_FILE].ToString();
                }
            }
            if (ModelState.IsValid)
            {
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
                    if (!string.IsNullOrEmpty(BSM.MessageError)&& !string.IsNullOrEmpty(BSM.MessageError2))
                    {
                        mess.Description = string.Format(mess.Description, BSM.MessageError, BSM.MessageError2);
                    }
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    //ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string id,int TranNo)
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
                BSM.HeaderEmpLeave = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo && w.EmpCode == id);
                if (Session[PATH_FILE] != null)
                {
                    BSM.HeaderEmpLeave.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.HeaderEmpLeave.EmpCode);

                if (BSM.HeaderEmpLeave != null)
                {
                    string DocumentNo = BSM.HeaderEmpLeave.Increment.ToString();
                    var LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == BSM.HeaderEmpLeave.LeaveCode);
                    var LeaveB = DB.HREmpLeaveBs.Where(w => w.EmpCode == BSM.HeaderEmpLeave.EmpCode && w.InYear == BSM.HeaderEmpLeave.ToDate.Year).ToList();
                    string LeaveCode = BSM.HeaderEmpLeave.LeaveCode;
                    if (LeaveType.IsParent == true)
                    {
                        LeaveCode = LeaveType.Parent;
                    }
                    BSM.Units = BSM.HeaderEmpLeave.Units;
                    Session["LEAVE_TYEP"] = null;
                    HREmpLeaveB LeaveBanace = LeaveB.FirstOrDefault(w => w.LeaveCode == LeaveCode);
                    BSM.EmpLeaveB = LeaveBanace;
                    BSM.ListEmpLeaveD = DB.HREmpLeaveDs.Where(x => x.LeaveTranNo == BSM.HeaderEmpLeave.Increment).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == DocumentNo && w.DocumentType == "LR").ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id,int TranNo, GenerateLeaveObject collection)
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
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                BSM.ListApproval = new List<ExDocApproval>();
                int TranNo = Convert.ToInt32(id);
                BSM.HeaderEmpLeave = DB.HREmpLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.HeaderEmpLeave.EmpCode);

                var ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == BSM.HeaderEmpLeave.Increment.ToString() 
                                    && w.DocumentType == "LR").OrderBy(w => w.ApproveLevel).ToList();
                BSM.ListApproval = ListApproval.ToList();

                BSM.ListEmpLeaveD = DB.HREmpLeaveDs.Where(x => x.LeaveTranNo == BSM.HeaderEmpLeave.Increment && x.EmpCode == BSM.HeaderEmpLeave.EmpCode).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult RequestforCancel(string id, string Remark)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID] = id;
            GenerateLeaveObject BSD = new GenerateLeaveObject();
            if (id == "null") id = null;
            if (id != null)
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamLeaveRequest/";
                string sms = BSD.RequestCancel(id, URL, Remark);
                if (sms == "OK")
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN );
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
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
            return PartialView("GridItemApprover", BSM.ListApproval);
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
        [HttpPost]
        public ActionResult ShowData(string ID, DateTime FromDate, string Units, DateTime ToDate, string Action)
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
            if (ID == "" || ID == null)
            {
                var rs1 = new { MS = "LeaveType_INVALIT" };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            ClsValidateLeave Leave = new ClsValidateLeave();
            if (BSM.HeaderStaff != null)
            {
                string LeaveCode = ID;
                BSM.ListEmpLeaveD = Leave.Get_LeaveDay(BSM.HeaderStaff.EmpCode, FromDate, ToDate, ID, Units);
                HRLeaveType LeaveType = DB.HRLeaveTypes.FirstOrDefault(w => w.Code == ID);
                Session["LEAVE_TYEP"] = LeaveType.Code;
                if (LeaveType.IsParent && !string.IsNullOrEmpty(LeaveType.Parent))
                {
                    LeaveCode = LeaveType.Parent;
                }
                var LeaveBanace = DB.HREmpLeaveBs.FirstOrDefault(x => x.EmpCode == BSM.HeaderStaff.EmpCode
                && x.InYear == year && x.LeaveCode == LeaveCode);
                if (LeaveBanace == null)
                {
                    var LB = new { MS = "Pleas Generate Leave Entitle" };
                    return Json(LB, JsonRequestBehavior.DenyGet);
                }
                BSM.EmpLeaveB = LeaveBanace;
                decimal? Bal_ = BSM.EmpLeaveB.Balance - BSM.ListEmpLeaveD.Where(w => w.Status == SYDocumentStatus.Leave.ToString()).Count();
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
                    ToDate = ToDate,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
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
            BSM.Units = ID;

            Session[Index_Sess_Obj + ActionName] = BSM;
            var rs = new
            {
                MS = SYConstant.OK,
                Units = ID
            };
            return Json(rs, JsonRequestBehavior.DenyGet);
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
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }
        private void DataSelector()
        {
            UserSession();
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
            var emp = DB.HRStaffProfiles.Where(w => w.EmpCode == user.UserName).FirstOrDefault();
            if (emp != null)
            {
                DateTime data = DateTime.Now;
                DateTime EndDate = new DateTime(data.Year, 12, 31);
                DateTime StartDate = Convert.ToDateTime(emp.LeaveConf);
                int WorkMonth = DateTimeHelper.CountMonth(StartDate, EndDate);
                var LstLeaveType = DB.HRLeaveTypes.ToList();
                LstLeaveType = GenerateLeaveObject.GetLeaveType(LstLeaveType, emp.TemLeave, emp.Sex, WorkMonth, true);

                var _listEmp = DB.HRStaffProfiles.Where(w => w.FirstLine == emp.EmpCode || w.SecondLine == emp.EmpCode || w.HODCode == emp.EmpCode
               || w.FirstLine2 == emp.EmpCode || w.SecondLine2 == emp.EmpCode || w.EmpCode == emp.EmpCode).ToList();
                _listEmp = _listEmp.Where(w => w.Status == "A" || w.DateTerminate > DateTime.Now).ToList();
                ViewData["LeaveTypes_SELECT"] = LstLeaveType.ToList();
                ViewData["STAFF_SELECT"] = _listEmp;

            }
            ViewData["UNITS_SELECT"] = ClsUnits.LoadUnit();
        }
    }
}