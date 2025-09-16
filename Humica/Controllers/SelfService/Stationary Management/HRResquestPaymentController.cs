using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.Report;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class HRResquestPaymentController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000006";
        private const string URL_SCREEN = "/SelfService/MyTeam/HRResquestPayment/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "RPNumber;Status";
        private string PARAM_INDEX = "RPNumber;Status";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        private string _LOCATION_ = "_LOCATION2_";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _MATCLASS_ = "_MATCLASS2_";
        private string PORequestType = "RP001";
        public HRResquestPaymentController()
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListPayment = DBV.HR_RP_View.ToList();
            var ListPO = DBV.HR_PO_VIEW.ToList();
            BSM.ListPO = ListPO.Where(w => w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult PartialList()
        {
            ActionName = "Index";
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            UserSession();
            DataSelector();
            UserConfList(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListPayment);

        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListPO);
        }
        #endregion
        #region Create 
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = new HRReqPayment();
            BSM.ListPItem = new List<HRReqPaymentItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.DocumentType = PORequestType;
            BSM.Header.DocumentDate = DateTime.Now;
            BSM.Header.DeliveryDate = DateTime.Now;
            BSM.Header.PaymentVendor = false;
            BSM.Header.PayymenyStaff = false;
            BSM.Header.Advance = false;
            BSM.Header.SettlementAdvance = false;
            BSM.Header.TotalAmountReq = 0;
            BSM.Header.TotalAmountRev = 0;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.AdvanceDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            LoadSession(PORequestType, BSM.Header.Branch);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsHRResquestPayment BSM)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();

            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
                    BSM.ListPItem = obj.ListPItem;
                    BSM.ListApproval = obj.ListApproval;
                }
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CreateHRRPayment();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.RPNumber;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new HRReqPayment();
                    BSM.ListPItem = new List<HRReqPaymentItem>();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.DeliveryDate = DateTime.Now;
                    BSM.Header.PaymentVendor = false;
                    BSM.Header.PayymenyStaff = false;
                    BSM.Header.Advance = false;
                    BSM.Header.SettlementAdvance = false;
                    BSM.Header.TotalAmountReq = 0;
                    BSM.Header.TotalAmountRev = 0;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.AdvanceDate = DateTime.Now;

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    return View(BSM);
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
            DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            string approved = SYDocumentStatus.APPROVED.ToString();
            BSM.ListPO = DBV.HR_PO_VIEW.Where(w => w.Status == approved).ToList();
            if (BSM.ListPO.Count > 0)
            {
                ViewData[SYConstant.PARAM_ID] = id;
                if (id == null)
                {

                    DataSelector();
                    BSM.Header = new HRReqPayment();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.DeliveryDate = DateTime.Now;
                    BSM.Header.AdvanceDate = DateTime.Now;
                    BSM.ListPItem = new List<HRReqPaymentItem>();
                }
                else
                {
                    string open = SYDocumentStatus.OPEN.ToString();
                    string approve = SYDocumentStatus.APPROVED.ToString();
                    BSM.ListPOItem = (from a in DB.HRPOHeaders
                                      join b in DB.HRPODetails on a.PONumber equals b.PONumber
                                      where a.Status == approve
                                      select b).ToList();
                    BSM.Header = new HRReqPayment();
                    BSM.Header.PaymentVendor = false;
                    BSM.Header.PayymenyStaff = false;
                    BSM.Header.Advance = false;
                    BSM.Header.SettlementAdvance = false;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now.Date;
                    BSM.Header.DeliveryDate = DateTime.Now;
                    BSM.ListApproval = new List<ExDocApproval>();
                    string msg = BSM.isValidReference(id);

                    if (msg != SYConstant.OK)
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    BSM.Header.TotalAmountReq = (decimal)BSM.ListPItem.Sum(w => w.AmountReq);
                    BSM.Header.TotalAmountRev = (decimal)BSM.ListPItem.Sum(w => w.AmountRev);
                    DataSelector();
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
        public ActionResult CreateMultiRef(ClsHRResquestPayment BSM, string id)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
                BSM.ListPItem = obj.ListPItem;
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CreateHRRPayment();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.RPNumber.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new ClsHRResquestPayment();
                    BSM.Header = new HRReqPayment();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.DeliveryDate = DateTime.Now;
                    BSM.Header.PaymentVendor = false;
                    BSM.Header.PayymenyStaff = false;
                    BSM.Header.Advance = false;
                    BSM.Header.SettlementAdvance = false;
                    BSM.Header.TotalAmountReq = 0;
                    BSM.Header.TotalAmountRev = 0;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.AdvanceDate = DateTime.Now;
                    Session[Index_Sess_Obj + ActionName] = BSM;
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsPRSelection", BSM.ListPOItem);
        }

        #endregion

        #region Edit 
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            UserConfListAndForm();
            // ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
            if (BSM.Header != null)
            {
                BSM.ListPItem = DB.HRReqPaymentItems.Where(w => w.RPNumber == id).ToList();
                BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSM.Header.DocumentType).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                LoadSession(PORequestType, BSM.Header.Branch);
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MATERIAL_NE");

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string id, ClsHRResquestPayment collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            var BSD = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            collection.ListPItem = BSD.ListPItem;
            collection.ListApproval = BSD.ListApproval;
            //  ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ModelState.IsValid)
            {
                collection.ScreenID = SCREEN_ID;
                string msg = collection.EditHRRPayment(id);
                if (msg != SYSConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = collection;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);

                }
                BSD.Header = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
                if (BSD.Header != null)
                {
                    BSD.ListPItem = DB.HRReqPaymentItems.Where(w => w.RPNumber == id).ToList();

                }

                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = collection.Header.RPNumber;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

                return View(collection);


            }
            else
            {
                var errors = ViewData.ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            ClsHRResquestPayment BSD = new ClsHRResquestPayment();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;

            BSD.Header = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);

            if (BSD.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSD.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSD.Header.DocumentType).ToList();

            BSD.ListPItem = DB.HRReqPaymentItems.Where(w => w.RPNumber == BSD.Header.RPNumber).ToList();

            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        #endregion
        #region Delete
        public ActionResult Delete(string id)
        {
            UserSession();
            ClsHRResquestPayment BSD = new ClsHRResquestPayment();
            ViewData[SYConstant.PARAM_ID] = id;
            UserConfListAndForm();
            BSD.Header = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
            //Update to database

            if (BSD.Header != null)
            {
                string sms = BSD.DeleteHRRPayment(BSD.Header.RPNumber);
                BSD.ListPItem = DB.HRReqPaymentItems.Where(w => w.RPNumber == id).ToList();
                if (sms == SYSConstant.OK)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
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
            //DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItem", BSM);
        }
        public ActionResult CreateItem(HRReqPaymentItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            //DateTime today = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {

                    var check = BSM.ListPItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                    int line = BSM.ListPItem.Count();
                    if (check.Count() == 0)
                    {

                        ModelObject.LineItem = (line + 1);
                        BSM.ListPItem.Add(ModelObject);

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
            return PartialView("GridItem", BSM);
        }
        public ActionResult EditItem(HRReqPaymentItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    objCheck.First().Description = ModelObject.Description;
                    objCheck.First().AmountReq = ModelObject.AmountReq;
                    objCheck.First().AmountRev = ModelObject.AmountRev;
                    objCheck.First().SupportingDoc = ModelObject.SupportingDoc;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM);
        }
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListPItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM);
        }
        #endregion
        #region Edit Grid Attibute
        public ActionResult CreateItemEdit(HRReqPaymentItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            //DateTime today = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {

                    var check = BSM.ListPItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                    int line = BSM.ListPItem.Count();
                    if (check.Count() == 0)
                    {

                        ModelObject.LineItem = (line + 1);
                        BSM.ListPItem.Add(ModelObject);

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
            return PartialView("GridItemEdit", BSM);
        }
        public ActionResult EditItemEdit(HRReqPaymentItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    objCheck.First().Description = ModelObject.Description;
                    objCheck.First().AmountReq = ModelObject.AmountReq;
                    objCheck.First().AmountRev = ModelObject.AmountRev;
                    objCheck.First().SupportingDoc = ModelObject.SupportingDoc;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemEdit", BSM);
        }
        public ActionResult DeleteItemEdit(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListPItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListPItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemEdit", BSM);

        }
        public ActionResult GridItemEdit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemEdit", BSM);
        }
        #endregion
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (id != null)
            {
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.Header.DocumentType);
                    string wfMsg = "";
                    if (excfObject != null)
                    {
                        var Objmatch = DBV.HR_RP_View.FirstOrDefault(w => w.RPNumber == BSM.Header.RPNumber);

                        Humica.Core.SY.SYWorkFlowEmailObject wfo =
                            new Humica.Core.SY.SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
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
                        HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.RPNumber, "");
                        if (Staff.Email != null && Staff.Email != "")
                        {
                            wfo.ListObjectDictionary.Add(Staff);
                            WorkFlowResult result = wfo.InsertProcessWorkFlow(Staff); wfMsg = wfo.getErrorMessage(result);
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
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
            var SD = DB.HRReqPayments.FirstOrDefault(w => w.RPNumber == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    RptResuestPayment reportModel = new RptResuestPayment();
                    reportModel.Parameters["RequestNumber"].Value = SD.RPNumber;
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
            RptResuestPayment reportModel = new RptResuestPayment();
            return ReportViewerExtension.ExportTo(reportModel);
        }


        #endregion
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (id.ToString() != "null")
            {
                string msg = BSM.CancelRP(id);
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
                BSM.ScreenID = SCREEN_ID;
                BSM.SetAutoApproval(docType, location);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRReqPayment();
                }

                BSM.Header.TotalAmountRev = 0;
                BSM.Header.TotalAmountReq = 0;
                foreach (var item in BSM.ListPItem)
                {
                    BSM.Header.TotalAmountReq += Convert.ToDecimal(item.AmountReq);
                    BSM.Header.TotalAmountRev += Convert.ToDecimal(item.AmountRev);
                }

                var result = new
                {
                    MS = SYConstant.OK,
                    TOTALRev = BSM.Header.TotalAmountRev,
                    TOTALReq = BSM.Header.TotalAmountReq
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
            ClsHRResquestPayment BSM = new ClsHRResquestPayment();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRResquestPayment)Session[Index_Sess_Obj + ActionName];
            }
            //     DataList(BSM.Header);
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        private void DataSelector()
        {
            ViewData["CURRENCY_SELECT"] = DB.HRCurrencies.ToList();
            ViewData["REQUEST_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SMS.HRBranches.ToList();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
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