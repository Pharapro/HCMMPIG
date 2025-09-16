using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.DS;
using Humica.Logic.MD;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Dress
{

    public class HRDressRequestController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "DS0000004";
        private const string URL_SCREEN = "/HRM/Dress/HRDressRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _LOCATION_ = "_LOCATION_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRDressRequestController()
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
            DataList();
            UserConfListAndForm(this.KeyName);

            DressRequestObject BSM = new DressRequestObject();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            BSM.ListHeader = new List<HRDressRequest>();
            BSM.ListHeader = DB.HRDressRequests.ToList();
            //    Where(x=>x.Status== "CANCELLED"||x.Status=="APPROVED").ToList();
            //BSM.ListHeaderPending = DB.HRDressRequest.Where(w => w.Status == "OPEN" || w.Status == "PENDING").ToList();
            //&& x.EmpCode == SYSession.getSessionUser().ToString()
            BSM.FInYear.Status = SYDocumentStatus.APPROVED.ToString();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(DressRequestObject collection)
        {
            DataList();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DressRequestObject BSM = new DressRequestObject();
            BSM.User = SYSession.getSessionUser();
            //BSM.ListHeaderPending = new List<HRDressRequest>();
            BSM.FInYear = collection.FInYear;
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();

            var ListEmpReq = DB.HRDressRequests.ToList();
            ListEmpReq = ListEmpReq.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            var ListLeave = ListEmpReq.Where(w => w.RequestDate.Year == collection.FInYear.INYear).ToList();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            BSM.ListHeader = DB.HRDressRequests.ToList();//.Where(x => x.Status == approved || x.Status == Cancel)

            if (BSM.FInYear.Status != null)
            {
                if (BSM.FInYear.Status == SYDocumentStatus.PENDING.ToString())
                {
                    BSM.ListHeader = ListLeave.Where(x => x.Status == "PENDING").ToList();
                }
                if (BSM.FInYear.Status == SYDocumentStatus.APPROVED.ToString())
                {
                    BSM.ListHeader = ListLeave.Where(x => x.Status == "APPROVED").ToList();
                }
                if (BSM.FInYear.Status == SYDocumentStatus.CANCELLED.ToString())
                {
                    BSM.ListHeader = ListLeave.Where(x => x.Status == "CANCELLED").ToList();
                }
                if (BSM.FInYear.Status == SYDocumentStatus.OPEN.ToString())
                {
                    BSM.ListHeader = ListLeave.Where(x => x.Status == "OPEN").ToList();
                }
                //else
                //{
                //    BSM.ListHeader = ListLeave.Where(w => w.Status == "OPEN").ToList();
                //}
            }
            //ListLeave = ListLeave.OrderByDescending(x => x.RequestDate).ToList();

            //BSM.ListHeader = ListLeave.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DressRequestObject BSM = new DressRequestObject();
            BSM.ListHeader = new List<HRDressRequest>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                //BSM.ListHeader.Where(w => w.Status == "OPEN" || w.Status== "PENDING").ToList();
            }
            return PartialView("PartialProcess", BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ListItem = new List<HRDressRequestItem>();
            BSM.User = SYSession.getSessionUser();
            BSM.Header = new HRDressRequest();
            BSM.Header.RequestDate = DateTime.Now;
            //var staff = DB.HRStaffProfiles.ToList();/*Where(w => w.EmpCode==BSM.User.UserName)*/
            //var obj = DB.HR_STAFF_VIEW.FirstOrDefault();//FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
            //if (staff.Count > 0)
            //{
            //    BSM.HeaderStaff = DB.HR_STAFF_VIEW.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
            //    BSM.Header = new HRDressRequest();
            //    BSM.Header.EmpCode = obj.EmpCode;
            //    BSM.Header.EmpName = obj.AllName;
            //    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            //    BSM.Header.RequestDate = DateTime.Now;
            //}
            //else
            //{
            //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SUP_CANT", user.Lang);
            //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            //}
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(DressRequestObject collection)
        {

            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            var BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                collection.ListApproval = BSM.ListApproval;
                BSM.Header = collection.Header;
                //obj.HeaderStaff= BSM.HeaderStaff;
                collection.ListItem = BSM.ListItem;
            }
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateDR();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
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
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressRequestObject BSM = new DressRequestObject();
            string re = id;
            if (id == "null") id = null;
            if (id != null)
            {
                BSM.Header = new HRDressRequest();
                BSM.ListHeader = new List<HRDressRequest>();
                //BSM.ListHeaderPending = new List<HRDressRequest>();
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.ListItem = new List<HRDressRequestItem>();
                BSM.Header = DB.HRDressRequests.FirstOrDefault(x => x.DocNo == id);
                if (BSM.Header != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSM.Header.DocType).ToList();
                    BSM.ListItem = DB.HRDressRequestItems.Where(w => w.DocNo == id).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                var resualt = DB.HRDressRequests.ToList();
                List<HRDressRequest> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.ListHeader = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, DressRequestObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressRequestObject BSM = new DressRequestObject();
            if (id != null)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditDR(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Details"
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ID != null)
            {
                DressRequestObject BSM = new DressRequestObject();
                BSM.Header = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == ID);
                var resualt = DB.HRDressRequests.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                List<HRDressRequest> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.ListHeader = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                if (BSM.Header != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == ID && w.DocumentType == BSM.Header.DocType).ToList();
                    BSM.ListItem = DB.HRDressRequestItems.Where(w => w.DocNo == ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string DocNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                DressRequestObject Del = new DressRequestObject();
                string msg = Del.DeleteDR(DocNo);
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
        #region Request
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            DressRequestObject BSM = new DressRequestObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approve
        public ActionResult Approve(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.approveTheDoc(id);
            if (msg == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                mess.Description = mess.Description + ". " + BSM.MessageCode;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            string sms = BSM.CancelDR(id);
            if (sms == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                mess.Description = mess.Description + ". " + BSM.MessageCode;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Print"
        public ActionResult Print(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    FRM_DessRequest reportModel = new FRM_DessRequest();

                    reportModel.Parameters["DocNo"].Value = SD.DocNo;
                    reportModel.Parameters["DocNo"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";
            var SD = DB.HRDressRequests.FirstOrDefault(w => w.DocNo == id);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            if (SD != null)
            {
                FRM_DessRequest reportModel = new FRM_DessRequest();
                reportModel.Parameters["DocNo"].Value = SD.DocNo;
                reportModel.Parameters["DocNo"].Visible = false;
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region Ajax Approval
        public ActionResult SelectParam(string docType, string location)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(docType, location, SCREEN_ID);
                BSM.SetAutoItem();
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion
        #region Grid_Item
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItems", BSM.ListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateItem(HRDressRequestItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidItem(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        //if (BSM.ListApproval.Count == 0)
                        //{
                        //    ModelObject.ID = 1;
                        //}
                        //else
                        //{
                        //    ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        //}
                        //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                        BSM.ListItem.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
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
            DataList();

            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult EditItem(HRDressRequestItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var Listitem = BSM.ListItem.Where(w => w.ItemName == ModelObject.ItemName).ToList();
                    if (Listitem.Count > 0)
                    {
                        var objUpdate = Listitem.First();
                        objUpdate.Qty = ModelObject.Qty;
                        objUpdate.Description = ModelObject.Description;
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
            DataList();
            return PartialView("GridItems", BSM.ListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteItem(string ItemName)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListItem.Where(w => w.ItemName == ItemName).ToList();
                    if (BSM.ListItem.Count > 0)
                    {
                        var objDel = BSM.ListItem.Where(w => w.ItemName == ItemName).First();
                        BSM.ListItem.Remove(objDel);
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
            DataList();

            return PartialView("GridItems", BSM.ListItem);
        }
        #endregion
        #region Grid_Approver
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult GridApprovalEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApprovalEdit", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        //if (BSM.ListApproval.Count == 0)
                        //{
                        //    ModelObject.ID = 1;
                        //}
                        //else
                        //{
                        //    ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        //}
                        //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                        ModelObject.DocumentNo = "N/A";
                        BSM.ListApproval.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
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
            DataList();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressRequestObject BSM = new DressRequestObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];
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
            DataList();

            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion

        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        [HttpPost]
        public string getEmpCode(string DocNo, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            DressRequestObject BSM = new DressRequestObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressRequestObject)Session[Index_Sess_Obj + ActionName];

                BSM.Header.DocNo = DocNo;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH");
            }
        }

        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Name = EmpStaff.AllName,
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);

        }

        private void DataList()
        {
            ViewData["DOCUMENT_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SYConstant.getBranchDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["ITEM_SELECT"] = DB.HRUniforms.ToList();
            var objWf = new ExWorkFlowPreference();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            var location = "";
            if (Session[_LOCATION_] != null)
            {
                location = Session[_LOCATION_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location, SCREEN_ID);

            SYDataList objList = new SYDataList("LEAVE_TIME");
            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS"] = objList.ListData;

        }
    }
}
