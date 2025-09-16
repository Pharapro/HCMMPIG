using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class HRPurchaseRequestController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000008";
        private const string URL_SCREEN = "/SelfService/MyTeam/HRPurchaseRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "RequestNumber;Status";
        private string PARAM_INDEX = "RequestNumber;Status";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        private string _LOCATION_ = "_LOCATION2_";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _MATCLASS_ = "_MATCLASS2_";
        private string PORequestType = "PR001";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        public HRPurchaseRequestController()
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
            DataSelector();
            UserConfList(this.KeyName);
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListPORequest = DBV.HR_PR_VIEW.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            UserSession();
            DataSelector();
            UserConfList(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListPORequest);

        }
        #endregion
        #region Create 
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = new HRPORequest();
            BSM.ListPRItem = new List<HRPORequestItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.DocumentType = PORequestType;
            BSM.Header.DocumentDate = DateTime.Now;
            BSM.Header.ExtectedDate = DateTime.Now;
            BSM.Header.TotalQty = 0;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSM;
            LoadSession(PORequestType, BSM.Header.Branch);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsHRPurchaseRequest BSM)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();


            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                    BSM.ListPRItem = obj.ListPRItem;
                    BSM.ListApproval = obj.ListApproval;
                }
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CreatePORequest();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.RequestNumber;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region Edit 
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            UserConfListAndForm();
            // ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
            if (BSM.Header != null)
            {
                BSM.ListPRItem = DB.HRPORequestItems.Where(w => w.RequestNumber == id).ToList();
                BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSM.Header.DocumentType).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                LoadSession(PORequestType, BSM.Header.Branch);
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MATERIAL_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string id, ClsHRPurchaseRequest collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            var BSD = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            collection.ListPRItem = BSD.ListPRItem;
            collection.ListApproval = BSD.ListApproval;
            //  ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ModelState.IsValid)
            {
                collection.ScreenID = SCREEN_ID;
                string msg = collection.EditPORequest(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.RequestNumber;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    LoadSession(PORequestType, collection.Header.Branch);
                    return View(collection);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(collection);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            ClsHRPurchaseRequest BSD = new ClsHRPurchaseRequest();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;

            BSD.Header = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);

            if (BSD.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSD.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSD.Header.DocumentType).ToList();

            BSD.ListPRItem = DB.HRPORequestItems.Where(w => w.RequestNumber == BSD.Header.RequestNumber).ToList();

            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            UserSession();
            ClsHRPurchaseRequest BSD = new ClsHRPurchaseRequest();
            ViewData[SYConstant.PARAM_ID] = id;
            UserConfListAndForm();
            BSD.Header = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
            //Update to database

            if (BSD.Header != null)
            {
                string sms = BSD.DeletePORequest(BSD.Header.RequestNumber);
                //  BSD.ListPRItem = DB.HRPORequestItems.Where(w => w.RequestNumber == id).ToList();
                if (sms == SYSConstant.OK)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = null;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("ATT", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
        }
        #endregion

        #region Create gird Attribute
        public ActionResult GridItem()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItem", BSM.ListPRItem);
        }
        public ActionResult CreateItem(HRPORequestItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                try
                {
                    if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                    {
                        var check = BSM.ListPRItem.Where(w => w.ItemCode == ModelObject.ItemCode).ToList();
                        int line = 0;
                        if (BSM.ListPRItem.Count > 0)
                        {
                            line = BSM.ListPRItem.Last().LineItem;
                        }
                        ModelObject.LineItem = line;
                        if (check.Count() == 0)
                        {
                            if (Session[PATH_FILE] != null)
                            {
                                ModelObject.Attachment = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                            }
                            ModelObject.LineItem = (line + 1);
                            BSM.ListPRItem.Add(ModelObject);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED");
                        }
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("LIST_NE", user.Lang);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataSelector();


            return PartialView("GridItem", BSM.ListPRItem);
        }
        public ActionResult EditItem(HRPORequestItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListPRItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (Session[PATH_FILE] != null)
                {
                    ModelObject.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    ModelObject.Attachment = listCheck.FirstOrDefault().Attachment;
                }
                if (listCheck.ToList().Count > 0)
                {
                    var objUpdate = listCheck.First();
                    objUpdate.ItemDescription = ModelObject.ItemDescription;
                    objUpdate.ItemCode = ModelObject.ItemCode;
                    objUpdate.Unit = ModelObject.Unit;
                    objUpdate.Quantity = ModelObject.Quantity;
                    objUpdate.Brand = ModelObject.Brand;
                    objUpdate.Remark = ModelObject.Remark;
                    objUpdate.Attachment = ModelObject.Attachment;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            DataSelector();
            return PartialView("GridItem", BSM.ListPRItem);
        }
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPRItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListPRItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM.ListPRItem);
        }
        #endregion

        #region Edit Grid Attibute
        public ActionResult CreateItemEdit(HRPORequestItem ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            //DateTime today = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var check = BSM.ListPRItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                    int line = BSM.ListPRItem.Count();
                    if (check.Count() == 0)
                    {
                        ModelObject.LineItem = (line + 1);
                        BSM.ListPRItem.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemEdit", BSM.ListPRItem);
        }
        public ActionResult EditItemEdit(HRPORequestItem ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPRItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    objCheck.First().ItemDescription = ModelObject.ItemDescription;
                    objCheck.First().Unit = ModelObject.Unit;
                    objCheck.First().Quantity = ModelObject.Quantity;
                    objCheck.First().Brand = ModelObject.Brand;
                    objCheck.First().Remark = ModelObject.Remark;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemEdit", BSM.ListPRItem);
        }
        public ActionResult DeleteItemEdit(int LineItem)
        {
            ActionName = "Edit";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPRItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListPRItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemEdit", BSM.ListPRItem);
        }
        public ActionResult GridItemEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemEdit", BSM.ListPRItem);
        }
        #endregion

        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (id != null)
            {
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var Objmatch = DBV.HR_PR_VIEW.FirstOrDefault(w => w.RequestNumber == BSM.Header.RequestNumber);

                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.Header.DocumentType);
                    string wfMsg = "";
                    if (excfObject != null)
                    {
                        SYWorkFlowEmailObject wfo =
                            new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                 UserType.N, BSM.Header.Status);
                        wfo.SelectListItem = new SYSplitItem(id);
                        wfo.User = BSM.User;
                        wfo.BS = BSM.BS;
                        wfo.ScreenId = SCREEN_ID;
                        wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.ListObjectDictionary.Add(Objmatch);
                        HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.RequestNumber, "");
                        if (Staff.Email != null && Staff.Email != "")
                        {
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result = wfo.InsertProcessWorkFlow(Staff);
                            wfMsg = wfo.getErrorMessage(result);
                        }
                    }
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + wfMsg;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Approve(string id)
        {
            UserSession();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (id != null)
            {

                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.approveTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #region "Upload"
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
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            // Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            objFile.ObjectTemplate.UpoadPath = objFile.ObjectTemplate.UpoadPath.Replace("~", "").Replace("/", "/");
            string fileNameUpload = SYUrl.getBaseUrl() + objFile.ObjectTemplate.UpoadPath;
            Session[PATH_FILE] = fileNameUpload;
            return null;
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
            //UserMVC();
            var SD = DB.HRPORequests.FirstOrDefault(w => w.RequestNumber == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    Rpt_PurchaseRequestForm reportModel = new Rpt_PurchaseRequestForm();

                    reportModel.Parameters["RequestNumber"].Value = SD.RequestNumber;
                    reportModel.Parameters["RequestNumber"].Visible = false;

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
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            Rpt_PurchaseRequestForm reportModel = new Rpt_PurchaseRequestForm();
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (id.ToString() != "null")
            {
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CancelPR(id);
                //   string cancelled = SYDocumentStatus.CANCELLED.ToString();

                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
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

        #region "Ajax Approval"
        public ActionResult SelectParam(string docType, string location)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(SCREEN_ID, docType, location);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        if (BSM.ListApproval.Count == 0)
                        {
                            ModelObject.ID = 1;
                        }
                        else
                        {
                            ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        }
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
            DataSelector();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
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
            DataSelector();

            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion

        #region Calculation
        public ActionResult Refreshvalue(string id, string action)
        {
            ActionName = action;
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRPORequest();
                }

                BSM.Header.TotalQty = 0;
                foreach (var item in BSM.ListPRItem)
                {
                    BSM.Header.TotalQty += Convert.ToDecimal(item.Quantity);
                }

                var result = new
                {
                    MS = SYConstant.OK,
                    TotalQty = BSM.Header.TotalQty,
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion

        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseRequest BSM = new ClsHRPurchaseRequest();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseRequest)Session[Index_Sess_Obj + ActionName];
            }
            //     DataList(BSM.Header);
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        private void DataSelector()
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["CURRENCY_SELECT"] = DB.HRCurrencies.ToList();
            ViewData["REQUEST_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SMS.HRBranches.ToList();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["ITEM_SELECT"] = DB.HRMaterials.ToList();
            var objWf = new ExWorkFlowPreference();
            var location = "";
            if (Session[_LOCATION_] != null)
            {
                location = Session[_LOCATION_].ToString();
            }
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location, SCREEN_ID);

        }
        private void LoadSession(string docType, string location)
        {
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
        }
    }
}