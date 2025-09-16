using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.DS;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Dress
{

    public class HRDressReceiveController : EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "DS0000005";
        private const string URL_SCREEN = "/HRM/Dress/HRDressReceive/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _LOCATION_ = "_LOCATION_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBViewContext DBV = new HumicaDBViewContext();
        HumicaDBContext DHX = new HumicaDBContext();
        public HRDressReceiveController()
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

            DressReceiveObject BSM = new DressReceiveObject();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            BSM.ListHeader = new List<HRDressReceive>();
            BSM.ListHeader = DHX.HRDressReceives.ToList();
            BSM.FInYear.Status = SYDocumentStatus.APPROVED.ToString();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(DressReceiveObject collection)
        {
            DataList();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DressReceiveObject BSM = new DressReceiveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.FInYear = collection.FInYear;
            var ListStaff = DBV.HR_STAFF_VIEW.ToList();
            BSM.ListItem = new List<HRDressReceiveItem> { new HRDressReceiveItem { ReturnItem = 0, TransferItem = 0 } };
            //ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.BranchID).Any()).ToList();
            var ListEmpReq = DHX.HRDressReceives.ToList();
            ListEmpReq = ListEmpReq.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            var ListLeave = ListEmpReq.Where(w => w.ReceiveDATE.Year == collection.FInYear.INYear).ToList();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string Cancel = SYDocumentStatus.CANCELLED.ToString();

            BSM.ListHeader = DHX.HRDressReceives.ToList();//.Where(x => x.Status == approved || x.Status == Cancel)

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
            }

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfListAndForm();
            DressReceiveObject BSM = new DressReceiveObject();
            BSM.ListItem = new List<HRDressReceiveItem>();
            BSM.ListStaff = DBV.HR_STAFF_VIEW.ToList();
            BSM.Header = new HRDressReceive();
            //BSM.Header.Status= SYDocumentStatus.OPEN.ToString();
            BSM.Header.ReceiveDATE = DateTime.Now;
            BSM.Header.FlexStatus = 0;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.User = SYSession.getSessionUser();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(DressReceiveObject collection)
        {

            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            var BSM = new DressReceiveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
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
                    BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateDR();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.DocNO;
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
        public ActionResult Edit(String id)
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressReceiveObject BSM = new DressReceiveObject();
            if (id != null)
            {
                BSM.Header = new HRDressReceive();
                BSM.ListHeader = new List<HRDressReceive>();
                BSM.ListItem = new List<HRDressReceiveItem>();
                BSM.Header = DHX.HRDressReceives.FirstOrDefault(x => x.DocNO == id);
                if (BSM.Header != null)
                {
                    BSM.ListItem = DHX.HRDressReceiveItems.Where(w => w.DocNO == id).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, DressReceiveObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressReceiveObject BSM = new DressReceiveObject();
            if (id != null)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
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
        #region "Delete"
        public ActionResult Delete(string DocNO)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            if (DocNO == "null") DocNO = null;
            if (DocNO != null)
            {
                DressReceiveObject Del = new DressReceiveObject();
                string msg = Del.DeleteDR(DocNO);
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
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            DressReceiveObject BSM = new DressReceiveObject();
            if (ID != null)
            {
                BSM.Header = new HRDressReceive();
                BSM.ListHeader = new List<HRDressReceive>();
                BSM.ListItem = new List<HRDressReceiveItem>();
                BSM.Header = DHX.HRDressReceives.FirstOrDefault(x => x.DocNO == ID);
                if (BSM.Header != null)
                {
                    BSM.ListItem = DHX.HRDressReceiveItems.Where(w => w.DocNO == ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                //List<HRDressRequest> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                //BSM.ListHeader = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult SelectParam(string docType)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            DressReceiveObject BSM = new DressReceiveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DressReceiveObject BSM = new DressReceiveObject();
            BSM.ListHeader = new List<HRDressReceive>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }

        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DHX.CFUploadPaths.Find("IMG_UPLOAD");
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
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressReceiveObject BSM = new DressReceiveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];

            }
            DataList();
            return PartialView("GridReceivedItem", BSM.ListItem);
        }
        public ActionResult TotalItem(string Action)
        {
            ActionName = Action;
            var BSM = new DressReceiveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                int Amount = 0;
                foreach (var item in BSM.ListItem)
                {
                    Amount += item.QTY;
                }
                BSM.Header.ReceiveQTY = Amount;
                var result = new
                {
                    MS = SYConstant.OK,
                    DeduAmount = BSM.Header.ReceiveQTY,
                };

                return Json(result, (JsonRequestBehavior)1);
            }
            var data1 = new { MS = "FAIL" };
            return Json(data1, (JsonRequestBehavior)1);
        }
        public ActionResult CreateItem(HRDressReceiveItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressReceiveObject BSM = new DressReceiveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];

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

            return PartialView("GridReceivedItem", BSM.ListItem);
        }
        public ActionResult EditItem(HRDressReceiveItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressReceiveObject BSM = new DressReceiveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var Listitem = BSM.ListItem.Where(w => w.ItemCode == ModelObject.ItemCode).ToList();
                    if (Listitem.Count > 0)
                    {
                        var objUpdate = Listitem.First();
                        objUpdate.QTY = ModelObject.QTY;
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
            return PartialView("GridReceivedItem", BSM.ListItem);
        }
        public ActionResult DeleteItem(string ItemCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressReceiveObject BSM = new DressReceiveObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressReceiveObject)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListItem.Where(w => w.ItemCode == ItemCode).ToList();
                    if (BSM.ListItem.Count > 0)
                    {
                        var objDel = BSM.ListItem.Where(w => w.ItemCode == ItemCode).First();
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

            return PartialView("GridReceivedItem", BSM.ListItem);
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
                    Branch = EmpStaff.Branch,
                    Position = EmpStaff.Position,

                };

                //GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);

        }
        private void DataList()
        {
            ViewData["DOCUMENT_TYPE"] = DHX.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SYConstant.getBranchDataAccess();
            ViewData["STAFF_SELECT"] = DHX.HRStaffProfiles.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["ITEM_SELECT"] = DHX.HRUniforms.ToList();
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
