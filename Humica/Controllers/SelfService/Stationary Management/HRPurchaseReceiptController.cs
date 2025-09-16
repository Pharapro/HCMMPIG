using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class HRPurchaseReceiptController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000017";
        private const string URL_SCREEN = "/SelfService/MyTeam/HRPurchaseReceipt/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ReceiptNo;Status";
        private string PARAM_INDEX = "ReceiptNo;Status";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        private string _LOCATION_ = "_LOCATION2_";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _MATCLASS_ = "_MATCLASS2_";
        private string PORequestType = "RC001";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        public HRPurchaseReceiptController()
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
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            BSM.ListPO = new List<HR_PO_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DBV.HR_POReceipt_View.ToList();
            var ListPO = DBV.HR_PO_VIEW.ToList();
            BSM.ListPO = ListPO.Where(w => w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            UserSession();
            DataSelector();
            UserConfList(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);

        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListPO);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            BSM.Header = new HRPOReceipt();
            BSM.ListRCItem = new List<HRPOReceiptItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.DocumentType = PORequestType;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.DocumentDate = DateTime.Now;
            var DocType = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == SCREEN_ID);
            if (DocType != null)
            {
                BSM.Header.DocumentType = DocType.DocType;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            LoadSession(PORequestType, BSM.Header.Branch);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsHRPurchaseReceipt BSM)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();

            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    var obj = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                    BSM.ListRCItem = obj.ListRCItem;
                    BSM.ListApproval = obj.ListApproval;
                }
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CreateHRPReceipt();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ReceiptNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new HRPOReceipt();
                    BSM.ListRCItem = new List<HRPOReceiptItem>();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.Header.NetAmount = 0;
                    BSM.Header.TotalAmount = 0;
                    BSM.Header.TotalDiscount = 0;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();

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

        #region "Create gird Attribute"
        public ActionResult GridItem()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            //BSM.ListRCItem = new List<HRPOReceiptItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItem", BSM.ListRCItem);
        }
        public ActionResult CreateItem(HRPOReceiptItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                }
                if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                {
                    var check = BSM.ListRCItem.Where(w => w.LineItem == ModelObject.LineItem || w.ItemCode == ModelObject.ItemCode).ToList();
                    int line = BSM.ListRCItem.Count();
                    ModelObject.LineItem = line;
                    if (check.Count() == 0)
                    {
                        if (Session[PATH_FILE] != null)
                        {
                            ModelObject.Attachment = Session[PATH_FILE].ToString();
                            Session[PATH_FILE] = null;
                        }
                        ModelObject.LineItem = (line + 1);
                        //Calculation
                        ModelObject.NetAmount = (decimal)(double)(ModelObject.Quantity * ModelObject.UnitPrice);
                        ModelObject.Amount = (decimal)(double)(ModelObject.NetAmount - ModelObject.Discount);

                        BSM.ListRCItem.Add(ModelObject);
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
            DataSelector();
            return PartialView("GridItem", BSM.ListRCItem);
        }
        public ActionResult EditItem(HRPOReceiptItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                    }
                    if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                    {
                        var objUpdate = BSM.ListRCItem.Where(w => w.LineItem == ModelObject.LineItem).First();

                        objUpdate.Quantity = ModelObject.Quantity;
                        objUpdate.UnitPrice = ModelObject.UnitPrice;
                        objUpdate.Discount = ModelObject.Discount;
                        objUpdate.PercentageDiscount = ModelObject.PercentageDiscount;
                        objUpdate.NetAmount = ModelObject.NetAmount;
                        objUpdate.LineReference = ModelObject.LineReference;
                        objUpdate.Unit = ModelObject.Unit;
                        objUpdate.VatTaxable = ModelObject.VatTaxable;
                        objUpdate.Remark = ModelObject.Remark;
                        objUpdate.NetAmount = ModelObject.NetAmount = (decimal)(double)(ModelObject.Quantity * ModelObject.UnitPrice);
                        objUpdate.Amount = ModelObject.Amount = (decimal)(double)(ModelObject.NetAmount - ModelObject.Discount);
                        if (Session[PATH_FILE] != null)
                        {
                            objUpdate.Attachment = Session[PATH_FILE].ToString();
                            Session[PATH_FILE] = null;
                        }
                        Session[Index_Sess_Obj + ActionName] = BSM;
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
            DataSelector();
            return PartialView("GridItem", BSM.ListRCItem);
        }
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListRCItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM.ListRCItem);
        }
        #endregion

        #region "Create Multi Ref"
        public ActionResult CreateMultiRef(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            string approved = SYDocumentStatus.APPROVED.ToString();
            BSM.ListPO = DBV.HR_PO_VIEW.Where(w => w.Status == approved).ToList();
            if (BSM.ListPO.Count > 0)
            {
                ViewData[SYConstant.PARAM_ID] = id;
                if (id == null)
                {
                    DataSelector();
                    BSM.Header = new HRPOReceipt();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.ListRCItem = new List<HRPOReceiptItem>();
                }
                else
                {
                    string open = SYDocumentStatus.OPEN.ToString();
                    string approve = SYDocumentStatus.APPROVED.ToString();
                    BSM.ListPOItem = (from a in DB.HRPOHeaders
                                      join b in DB.HRPODetails on a.PONumber equals b.PONumber
                                      where a.Status == approve
                                      select b).ToList();
                    BSM.Header = new HRPOReceipt();
                    BSM.Header.IsReturn = false;
                    BSM.ListRCItem = new List<HRPOReceiptItem>();
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now.Date;

                    var DocType = DB.ExCfWorkFlowItems.FirstOrDefault(w => w.ScreenID == SCREEN_ID);
                    if (DocType != null) BSM.Header.DocumentType = DocType.DocType;

                    var _obj = DB.HRPOHeaders.FirstOrDefault(x => x.PONumber == id);
                    if (_obj != null) BSM.Header.DocurementReference = _obj.PONumber;

                    var _branch = DB.HRPOHeaders.FirstOrDefault(x => x.Branch == id);
                    if (_branch != null) BSM.Header.Branch = _branch.Branch;

                    BSM.ListApproval = new List<ExDocApproval>();
                    string msg = BSM.isValidReference(id);

                    if (msg != SYConstant.OK)
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    BSM.Header.NetAmount = (decimal)BSM.ListRCItem.Sum(w => w.NetAmount);
                    BSM.Header.TotalAmount = (decimal)BSM.ListRCItem.Sum(w => w.Amount);
                    BSM.Header.TotalDiscount = (decimal)BSM.ListRCItem.Sum(w => w.Discount);
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
        public ActionResult CreateMultiRef(ClsHRPurchaseReceipt BSM, string id)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                BSM.ListRCItem = obj.ListRCItem;
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.CreateHRPReceipt();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ReceiptNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new ClsHRPurchaseReceipt();
                    BSM.Header = new HRPOReceipt();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.Header.DocumentType = PORequestType;
                    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.Header.DocumentDate = DateTime.Now;
                    BSM.ListRCItem = new List<HRPOReceiptItem>();

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

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DataSelector();
            if (id != null)
            {
                ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
                BSM.Header = DB.HRPOReceipts.Find(id);
                if (BSM.Header != null)
                {
                    BSM.ListRCItem = DB.HRPOReceiptItems.Where(x => x.ReceiptNo == id).ToList();
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSM.Header.DocumentType).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    LoadSession(PORequestType, BSM.Header.Branch);
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClsHRPurchaseReceipt collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            var BSD = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            collection.ListRCItem = BSD.ListRCItem;
            collection.ListApproval = BSD.ListApproval;
            if (ModelState.IsValid)
            {
                collection.ScreenID = SCREEN_ID;
                string msg = collection.EditHRPReceipt(id);
                if (msg != SYSConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = collection;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);

                }
                BSD.Header = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
                if (BSD.Header != null)
                {
                    BSD.ListRCItem = DB.HRPOReceiptItems.Where(w => w.ReceiptNo == id).ToList();

                }

                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = collection.Header.ReceiptNo;
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

        #region "Edit Grid Attibute"
        public ActionResult GridItemEdit()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemEdit", BSM);
        }
        public ActionResult CreateItemEdit(HRPOReceiptItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                    }
                    if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                    {
                        var check = BSM.ListRCItem.Where(w => w.LineItem == ModelObject.LineItem || w.ItemCode == ModelObject.ItemCode).ToList();
                        int line = BSM.ListRCItem.Count();
                        ModelObject.LineItem = line;
                        if (check.Count() == 0)
                        {
                            if (Session[PATH_FILE] != null)
                            {
                                ModelObject.Attachment = Session[PATH_FILE].ToString();
                                Session[PATH_FILE] = null;
                            }
                            ModelObject.LineItem = (line + 1);
                            //Calculation
                            ModelObject.NetAmount = (decimal)(double)(ModelObject.Quantity * ModelObject.UnitPrice);
                            ModelObject.Amount = (decimal)(double)(ModelObject.NetAmount - ModelObject.Discount);

                            BSM.ListRCItem.Add(ModelObject);
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

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemEdit", BSM);
        }
        public ActionResult EditItemEdit(HRPOReceiptItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                    }
                    if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.Unit))
                    {
                        var objUpdate = BSM.ListRCItem.Where(w => w.LineItem == ModelObject.LineItem).First();

                        objUpdate.Quantity = ModelObject.Quantity;
                        objUpdate.UnitPrice = ModelObject.UnitPrice;
                        objUpdate.Discount = ModelObject.Discount;
                        objUpdate.PercentageDiscount = ModelObject.PercentageDiscount;
                        objUpdate.NetAmount = ModelObject.NetAmount;
                        objUpdate.LineReference = ModelObject.LineReference;
                        objUpdate.Unit = ModelObject.Unit;
                        objUpdate.VatTaxable = ModelObject.VatTaxable;
                        objUpdate.Remark = ModelObject.Remark;
                        objUpdate.NetAmount = ModelObject.NetAmount = (decimal)(double)(ModelObject.Quantity * ModelObject.UnitPrice);
                        objUpdate.Amount = ModelObject.Amount = (decimal)(double)(ModelObject.NetAmount - ModelObject.Discount);
                        if (Session[PATH_FILE] != null)
                        {
                            objUpdate.Attachment = Session[PATH_FILE].ToString();
                            Session[PATH_FILE] = null;
                        }
                        Session[Index_Sess_Obj + ActionName] = BSM;
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
            DataSelector();
            return PartialView("GridItemEdit", BSM);
        }
        public ActionResult DeleteItemEdit(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRCItem.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListRCItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItemEdit", BSM);

        }
        #endregion

        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            ClsHRPurchaseReceipt BSD = new ClsHRPurchaseReceipt();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;

            BSD.Header = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);

            if (BSD.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSD.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSD.Header.DocumentType).ToList();

            BSD.ListRCItem = DB.HRPOReceiptItems.Where(w => w.ReceiptNo == BSD.Header.ReceiptNo).ToList();

            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        #endregion

        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            ClsHRPurchaseReceipt BSD = new ClsHRPurchaseReceipt();
            ViewData[SYConstant.PARAM_ID] = id;
            UserConfListAndForm();
            BSD.Header = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
            //Update to database
            if (BSD.Header != null)
            {
                string sms = BSD.DeleteHRReceipt(BSD.Header.ReceiptNo);
                BSD.ListRCItem = DB.HRPOReceiptItems.Where(w => w.ReceiptNo == id).ToList();
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

        #region "Release Document"
        public ActionResult ReleaseDoc(string id)
        {
            UserSession();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (id != null)
            {

                BSM.ScreenID = SCREEN_ID;
                string msg = BSM.ReleaseTheDoc(id);
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
        #endregion

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
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

        #region "Print"
        /*        public ActionResult Print(string id)
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
                    UserMVC();
                    var SD = DB.HRPOReceipts.FirstOrDefault(w => w.ReceiptNo == id);
                    if (SD != null)
                    {
                        try
                        {
                            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                            RptResuestPayment reportModel = new RptResuestPayment();
                            reportModel.Parameters["RequestNumber"].Value = SD.ReceiptNo;
                            reportModel.Parameters["RequestNumber"].Visible = false;

                            Session[Index_Sess_Obj + ActionName] = reportModel;

                            return PartialView("PrintForm", reportModel);
                        }
                        catch (Exception e)
                        {
                            *//*------------------SaveLog----------------------------------*//*
                            SYEventLog log = new SYEventLog();
                            log.ScreenId = SCREEN_ID;
                            log.UserId = user.UserID.ToString();
                            log.DocurmentAction = id;
                            log.Action = SYActionBehavior.ADD.ToString();

                            SYEventLogObject.saveEventLog(log, e, true);
                            *//*----------------------------------------------------------*//*
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
                }*/
        #endregion

        #region "Calculation"
        public ActionResult Refreshvalue(string id, string action)
        {
            ActionName = action;
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRPOReceipt();
                }

                BSM.Header.NetAmount = 0;
                BSM.Header.TotalAmount = 0;
                BSM.Header.TotalDiscount = 0;

                foreach (var item in BSM.ListRCItem)
                {
                    BSM.Header.NetAmount += Convert.ToDecimal(item.NetAmount);
                    BSM.Header.TotalAmount += Convert.ToDecimal(item.Amount);
                    BSM.Header.TotalDiscount += Convert.ToDecimal(item.Discount);
                }

                var result = new
                {
                    MS = SYConstant.OK,
                    TOTALAmount = BSM.Header.TotalAmount,
                    TOTAlNetAmount = BSM.Header.NetAmount,
                    TOTAlDiscount = BSM.Header.TotalDiscount,
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
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }

        #region "Ajax Approval"
        public ActionResult SelectParam(string docType, string location)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
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
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
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
            ClsHRPurchaseReceipt BSM = new ClsHRPurchaseReceipt();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRPurchaseReceipt)Session[Index_Sess_Obj + ActionName];
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

        private void DataSelector()
        {
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