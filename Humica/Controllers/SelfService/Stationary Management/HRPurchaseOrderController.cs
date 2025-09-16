using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class HRPurchaseOrderController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000007";
        private const string URL_SCREEN = "/SelfService/MyTeam/HRPurchaseOrder/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "PONumber;Status";
        private string _LOCATION_ = "_LOCATION2_";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string PORequestType = "PO001";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HRPurchaseOrderController()
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
            DataList();
            FormPOObject BSM = new FormPOObject();
            BSM.ListPR = new List<HR_PR_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DBV.HR_PO_VIEW.ToList();
            var ListPR = DBV.HR_PR_VIEW.ToList();
            BSM.ListPR = ListPR.Where(w => w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataList();
            FormPOObject BSM = new FormPOObject();
            BSM.ListHeader = new List<HR_PO_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListPR);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            FormPOObject BSM = new FormPOObject();
            BSM.Header = new HRPOHeader();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.DocumentType = PORequestType;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.DocumentDate = DateTime.Now;
            BSM.Header.PromisedDate = DateTime.Now;
            BSM.ListDetail = new List<HRPODetail>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(FormPOObject BSM)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListDetail = obj.ListDetail;
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateHRPO();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.PONumber.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

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
        #region "Create Multi Ref"
        public ActionResult CreateMultiRef(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            string approved = SYDocumentStatus.APPROVED.ToString();
            BSM.ListPR = DBV.HR_PR_VIEW.Where(w => w.Status == approved).ToList();
            // BSM.ListHistory = new List<POHistory>();

            if (BSM.ListPR.Count > 0)
            {
                ViewData[SYConstant.PARAM_ID] = id;
                if (id == null)
                {
                    //  DataSelector();
                    DataList();
                    BSM.Header = new HRPOHeader();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.PromisedDate = DateTime.Now;
                    BSM.ListDetail = new List<HRPODetail>();
                }
                else
                {
                    string open = SYDocumentStatus.OPEN.ToString();
                    string approve = SYDocumentStatus.APPROVED.ToString();
                    BSM.ListPRItem = (from a in DB.HRPORequests
                                      join b in DB.HRPORequestItems on a.RequestNumber equals b.RequestNumber
                                      where a.Status == approve
                                      select b).ToList();
                    BSM.Header = new HRPOHeader();
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now.Date;

                    var DocType = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == SCREEN_ID);
                    if (DocType != null) BSM.Header.DocumentType = DocType.DocType;

                    var _obj = DB.HRPORequests.FirstOrDefault(x => x.RequestNumber == id);
                    if (_obj != null) BSM.Header.DocumentReference = _obj.RequestNumber;

                    var _branch = DB.HRPORequests.FirstOrDefault(x => x.Branch == id);
                    if (_branch != null) BSM.Header.Branch = _branch.Branch;

                    var _requestor = DB.HRPORequests.FirstOrDefault(x => x.Requestor == id);
                    if (_requestor != null) BSM.Header.Requestor = _requestor.Requestor;

                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.ScreenId = SCREEN_ID;
                    string msg = BSM.isValidReference(id);

                    //  BSM.updateTotal(BSM.Header, BSM.ListDetail);
                    if (msg != SYConstant.OK)
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    BSM.Header.Total = BSM.ListDetail.Sum(w => w.Amount);
                    // BSM.Header.TotalAmount = BSM.ListItemInsert.Sum(w => w.Amount);
                    DataList();
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("NO_REQUEST");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            return View(BSM);

        }

        [HttpPost]
        public ActionResult CreateMultiRef(FormPOObject BSM, string id)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListDetail = obj.ListDetail;
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateHRPO();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.PONumber.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new FormPOObject();
                    BSM.Header = new HRPOHeader();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.PromisedDate = DateTime.Now;
                    BSM.ListDetail = new List<HRPODetail>();
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

        public ActionResult GridItemsPRSelection()
        {
            ActionName = "Create";
            UserSession();
            UserConfList(this.KeyName);
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsPRSelection", BSM.ListPRItem);
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
                FormPOObject BSM = new FormPOObject();
                BSM.Header = DB.HRPOHeaders.Find(ID);
                if (BSM.Header != null)
                {
                    BSM.ListDetail = DB.HRPODetails.Where(w => w.PONumber == ID).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == ID && w.DocumentType == BSM.Header.DocumentType).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            DataList();
            if (ID != null)
            {
                FormPOObject BSM = new FormPOObject();
                BSM.Header = DB.HRPOHeaders.Find(ID);
                if (BSM.Header != null)
                {
                    BSM.ListDetail = DB.HRPODetails.Where(x => x.PONumber == ID).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == ID && w.DocumentType == BSM.Header.DocumentType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string ID, FormPOObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    collection.ListDetail = obj.ListDetail;
                    collection.ListApproval = obj.ListApproval;
                }
                collection.ScreenId = SCREEN_ID;
                string msg = collection.EditHRPO(ID);
                if (msg == SYConstant.OK)
                {

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.PONumber;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
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
        #region "Delete"
        public ActionResult Delete(string ID)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            if (ID != null)
            {
                FormPOObject Del = new FormPOObject();
                string msg = Del.deleteHRPOHeader(ID);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("LEAVE_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            FormPOObject BSM = new FormPOObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var Objmatch = DBV.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == BSM.Header.PONumber);

                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.Header.DocumentType);
                    string wfMsg = "";
                    if (excfObject != null)
                    {
                        Humica.Core.SY.SYWorkFlowEmailObject wfo =
                            new Humica.Core.SY.SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                 UserType.N, BSM.Header.Status);
                        wfo.SelectListItem = new SYSplitItem(id);
                        wfo.User = BSM.User;
                        wfo.BS = BSM.BS;
                        wfo.ScreenId = SCREEN_ID;
                        wfo.Module = "HR";// CModule.PURCHASE.ToString();
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.ListObjectDictionary = new List<object>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.PONumber, "");
                        if (Staff.Email != null && Staff.Email != "")
                        {
                            wfo.ListObjectDictionary.Add(Staff);
                            wfo.ListObjectDictionary.Add(Objmatch);
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
            FormPOObject BSM = new FormPOObject();
            if (id != null)
            {

                BSM.ScreenId = SCREEN_ID;
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

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            FormPOObject BSM = new FormPOObject();
            if (id.ToString() != "null")
            {
                string msg = BSM.CancelPO(id);
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

        #region "Ajax select item for time"
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListDetail);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateD(HRPODetail ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                try
                {
                    if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                    {
                        var check = BSM.ListDetail.Where(w => w.LineNbr == ModelObject.LineNbr || w.ItemCode == ModelObject.ItemCode).ToList();
                        int line = BSM.ListDetail.Count();
                        ModelObject.LineNbr = line;
                        if (check.Count() == 0)
                        {
                            if (Session[PATH_FILE] != null)
                            {
                                ModelObject.Attachment = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                            }
                            ModelObject.LineNbr = (line + 1);
                            ModelObject.SubTotal = (decimal)(ModelObject.Qty * ModelObject.Amount);
                            BSM.ListDetail.Add(ModelObject);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            //Session[Index_Sess_Obj + ActionName] = BSM;
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
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataList();

            return PartialView("GridItems", BSM.ListDetail);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditD(HRPODetail ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var objUpdate = BSM.ListDetail.Where(w => w.LineNbr == ModelObject.LineNbr).First();

                    objUpdate.Descr = ModelObject.Descr;
                    objUpdate.Qty = ModelObject.Qty;
                    objUpdate.Unit = ModelObject.Unit;
                    objUpdate.Amount = ModelObject.Amount;
                    objUpdate.SubTotal = (decimal)(double)(ModelObject.Qty * ModelObject.Amount);
                    if (Session[PATH_FILE] != null)
                    {
                        objUpdate.Attachment = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
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

            return PartialView("GridItems", BSM.ListDetail);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteD(int LineNbr)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListDetail.Where(w => w.LineNbr == LineNbr).ToList();
                    if (checkList.Count == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }

                    if (error == 0)
                    {
                        BSM.ListDetail.Remove(checkList.First());
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

            return PartialView("GridItems", BSM.ListDetail);
        }
        #endregion
        #region "Ajax select edit"
        public ActionResult GridItemsEdit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItemsEdit", BSM.ListDetail);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateDE(HRPODetail ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    }
                    int line = 0;
                    if (BSM.ListDetail.Count == 0)
                    {
                        line = 1;
                    }
                    else
                    {
                        line = BSM.ListDetail.Max(w => w.LineNbr);
                        line = line + 1;
                    }
                    ModelObject.LineNbr = line;

                    ModelObject.SubTotal = (decimal)(ModelObject.Qty * ModelObject.Amount);

                    BSM.ListDetail.Add(ModelObject);
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

            return PartialView("GridItemsEdit", BSM.ListDetail);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditDE(HRPODetail ModelObject)
        {

            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var objUpdate = BSM.ListDetail.Where(w => w.LineNbr == ModelObject.LineNbr).First();

                    objUpdate.Descr = ModelObject.Descr;
                    objUpdate.Qty = ModelObject.Qty;
                    objUpdate.Unit = ModelObject.Unit;
                    objUpdate.Amount = ModelObject.Amount;
                    objUpdate.SubTotal = ModelObject.SubTotal = (decimal)(double)(ModelObject.Qty * ModelObject.Amount);

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

            return PartialView("GridItemsEdit", BSM.ListDetail);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDE(int LineNbr)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var error = 0;

                    var checkList = BSM.ListDetail.Where(w => w.LineNbr == LineNbr).ToList();
                    if (checkList.Count == 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                        error = 1;
                    }
                    if (error == 0)
                    {
                        BSM.ListDetail.Remove(checkList.First());
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

            return PartialView("GridItemsEdit", BSM.ListDetail);
        }


        #endregion
        #region GridItemDetails
        public ActionResult GridItemDetails()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            FormPOObject BSM = new FormPOObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDetails";
            return PartialView("GridItemDetails", BSM.ListDetail);
        }
        #endregion

        #region Calculation
        public ActionResult Refreshvalue(string id, string action)
        {
            ActionName = action;
            FormPOObject BSM = new FormPOObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                //   decimal subtotal = Convert.ToDecimal(subTotal);
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRPOHeader();
                }
                BSM.Header.Total = 0;
                foreach (var item in BSM.ListDetail)
                {
                    BSM.Header.Total += item.SubTotal;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    TOTAL = BSM.Header.Total
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
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
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
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
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
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
            DataList();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
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
            var SD = DB.HRPOHeaders.FirstOrDefault(w => w.PONumber == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    RPT_PURCHASE_ORDER reportModel = new RPT_PURCHASE_ORDER();

                    reportModel.Parameters["PONumber"].Value = SD.PONumber;
                    reportModel.Parameters["PONumber"].Visible = false;

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
            RPT_PURCHASE_ORDER reportModel = new RPT_PURCHASE_ORDER();
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            FormPOObject BSM = new FormPOObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (FormPOObject)Session[Index_Sess_Obj + ActionName];
            }
            //     DataList(BSM.Header);
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        private void DataList()
        {
            ViewData["PO_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
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
    }
}