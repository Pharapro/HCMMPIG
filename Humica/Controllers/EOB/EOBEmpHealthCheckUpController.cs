using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{

    public class EOBEmpHealthCheckUpController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "EOB0000004";
        private const string URL_SCREEN = "/EOB/EOBEmpHealthCheckUp/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public EOBEmpHealthCheckUpController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            BSM.ListHeader = new List<EOBEmpHealthCheckUp>();
            BSM.ListHeader = DB.EOBEmpHealthCheckUps.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
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
            EOBHealthChkUpObject BSD = new EOBHealthChkUpObject();
            BSD.Header = new EOBEmpHealthCheckUp();
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(EOBHealthChkUpObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];

            if (ModelState.IsValid)
            {
                string msg = collection.createChkUp();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.EmpCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?EmpCode=" + mess.DocumentNumber;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    BSM.Header = new EOBEmpHealthCheckUp();
                    BSM.ListItem = new List<EOBEmpHealthCheckUpRecord>();

                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(collection);
        }
        #endregion 
        #region 'Edit'
        public ActionResult Edit(string EmpCode)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            UserConfListAndForm();

            BSM.Header = DB.EOBEmpHealthCheckUps.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (BSM.Header != null)
            {
                BSM.staffProfile = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
                BSM.ListItem = DB.EOBEmpHealthCheckUpRecords.Where(w => w.EmpCode == EmpCode).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string EmpCode, EOBHealthChkUpObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();

            BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;
            collection.ListItem = BSM.ListItem;
            if (Session[PATH_FILE] != null)
            {
                collection.Header.AttachmentRef = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            else
            {
                collection.Header.AttachmentRef = BSM.Header.AttachmentRef;
            }
            if (ModelState.IsValid)
            {
                string msg = collection.UpdChkUp(EmpCode);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = EmpCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?EmpCode=" + mess.DocumentNumber;
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
        #region 'GridItems'
        public ActionResult GridItems()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult CreateRecord(EOBEmpHealthCheckUpRecord ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
                    }
                    if (BSM.ListItem.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListItem.Max(w => w.LineItem) + 1;
                    }
                    BSM.ListItem.Add(ModelObject);
                    Session[Index_Sess_Obj + ActionName] = BSM;
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
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult EditRecord(EOBEmpHealthCheckUpRecord ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    objCheck.First().MedicalCheck = ModelObject.MedicalCheck;
                    objCheck.First().Remark = ModelObject.Remark;
                    objCheck.First().Result = ModelObject.Result;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult DeleteRecord(int LineItem)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM.ListItem);
        }
        #endregion
        #region 'GridItemsDetails'
        public ActionResult GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (EOBHealthChkUpObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItemsDetails", BSM.ListItem);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string EmpCode)
        {
            ActionName = "Details";
            UserSession();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = EmpCode;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = DB.EOBEmpHealthCheckUps.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (BSM.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSM.staffProfile = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            BSM.ListItem = DB.EOBEmpHealthCheckUpRecords.Where(w => w.EmpCode == EmpCode).ToList();
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion 
        #region 'Delete'  
        public ActionResult Delete(string EmpCode)
        {
            UserSession();
            EOBHealthChkUpObject BSM = new EOBHealthChkUpObject();
            BSM.Header = DB.EOBEmpHealthCheckUps.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (BSM.Header != null)
            {
                string msg = BSM.deleteCheckUp(EmpCode);

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
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion 
        #region 'Print'
        public ActionResult Print(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = id;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            var obj = DB.EOBEmpHealthCheckUps.Where(w => w.EmpCode == id).ToList();
            if (obj.Count > 0)
            {
                try
                {
                    ViewData[SYSConstant.PARAM_ID] = id;
                    FRMHealthCheckUp FRMHealthCheckUp = new FRMHealthCheckUp();
                    FRMHealthCheckUp.Parameters["EmpCode"].Value = obj.First().EmpCode;
                    FRMHealthCheckUp.Parameters["EmpCode"].Visible = false;
                    Session[this.Index_Sess_Obj + this.ActionName] = FRMHealthCheckUp;
                    return PartialView("PrintForm", FRMHealthCheckUp);
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = "EOB0000004",
                        UserId = this.user.UserID.ToString(),
                        DocurmentAction = id,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string EmpCode)
        {
            ActionName = "Print";
            FRMHealthCheckUp reportModel = (FRMHealthCheckUp)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = EmpCode;
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion
        #region 'Upload'
        [HttpPost]
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
        #endregion
        #region 'private code'
        private void DataSelector()
        {
            //ViewData["DEPT_SELECT"] = DH.HRDepartments.ToList().OrderBy(w => w.Description);
            //ViewData["POST_SELECT"] = DH.HRPositions.ToList().OrderBy(w => w.Description);
            ViewData["CHKUPTYPE_SELECT"] = DB.HRMedicalTypes.ToList().OrderBy(w => w.Description);
            ViewData["HP_SELECT"] = DB.HRHospitals.ToList().OrderBy(w => w.Description);
            ViewData["EMP_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
        }
        #endregion 
    }
}
