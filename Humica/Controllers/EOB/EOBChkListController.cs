using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBChkListController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "EOB0000003";
        private const string URL_SCREEN = "/EOB/EOBChkList/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public EOBChkListController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();
            BSM.ListHeader = new List<EOBEmpChkLst>();
            BSM.ListHeader = DB.EOBEmpChkLsts.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (OnBoardChkLstObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            UserConfListAndForm();
            OnBoardChkLstObject BSD = new OnBoardChkLstObject();
            BSD.Header = new EOBEmpChkLst();
            BSD.Header.DocumentDate = DateTime.Now;
            BSD.ListCheck = new List<EOBSChkLst>();
            BSD.ListCheckItem = new List<EOBSChkLstItem>();
            var chk = DB.EOBSChkLsts.Where(w => w.IsActive == true).ToList();
            var chkitem = DB.EOBSChkLstItems.Where(w => w.IsActive == true).ToList();
            if (chk.Count > 0)
            {
                BSD.ListCheck = chk.Where(w => chkitem.Where(x => x.Code == w.Code).Any()).ToList();
                BSD.ListCheckItem = chkitem.Where(w => chk.Where(x => x.Code == w.Code).Any()).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(OnBoardChkLstObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (OnBoardChkLstObject)Session[Index_Sess_Obj + ActionName];

            if (BSM != null)
            {
                BSM.Header = collection.Header;
                BSM.CheckedItem = collection.CheckedItem;
                string msg = BSM.OBCheckList(BSM.CheckedItem);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.EmpCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            return View(BSM);
        }
        #endregion 
        #region 'Edit'
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();
            UserConfListAndForm();

            BSM.Header = DB.EOBEmpChkLsts.FirstOrDefault(w => w.EmpCode == id);
            BSM.ListItem = DB.EOBEmpChkLstItems.Where(w => w.EmpCode == id).ToList();
            var chk = DB.EOBSChkLsts.Where(w => w.IsActive == true).ToList();
            var chkitem = DB.EOBSChkLstItems.Where(w => w.IsActive == true).ToList();

            if (chk.Count > 0)
            {
                BSM.ListCheck = chk.Where(w => chkitem.Where(x => x.Code == w.Code).Any()).ToList();
                BSM.ListCheckItem = chkitem.Where(w => chk.Where(x => x.Code == w.Code).Any()).ToList();
            }
            if (BSM.Header != null)
            {
                BSM.staffProfile = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, OnBoardChkLstObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            OnBoardChkLstObject BSM = new OnBoardChkLstObject();

            BSM = (OnBoardChkLstObject)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;

            if (ModelState.IsValid)
            {
                BSM.Header = collection.Header;
                BSM.CheckedItem = collection.CheckedItem;
                string msg = BSM.updChkLst(BSM.CheckedItem);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);

                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);

            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYConstant.PARAM_ID] = id;
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();
            BSM.Header = DB.EOBEmpChkLsts.FirstOrDefault(w => w.EmpCode == id);
            BSM.ListItem = DB.EOBEmpChkLstItems.Where(w => w.EmpCode == id).ToList();
            var chk = DB.EOBSChkLsts.Where(w => w.IsActive == true).ToList();
            var chkitem = DB.EOBSChkLstItems.Where(w => w.IsActive == true).ToList();

            if (chk.Count > 0)
            {
                BSM.ListCheck = chk.Where(w => chkitem.Where(x => x.Code == w.Code).Any()).ToList();
                BSM.ListCheckItem = chkitem.Where(w => chk.Where(x => x.Code == w.Code).Any()).ToList();
            }
            if (BSM.Header != null)
            {
                BSM.staffProfile = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Delete'  
        public ActionResult Delete(string id)
        {
            UserSession();
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();
            BSM.Header = DB.EOBEmpChkLsts.FirstOrDefault(w => w.EmpCode == id);
            if (BSM.Header != null)
            {
                string msg = BSM.deleteChkLst(id);

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
        #region 'private code'
        public ActionResult ShowDataEmp(string EmpCode, string Action)
        {
            ActionName = Action;
            OnBoardChkLstObject BSM = new OnBoardChkLstObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (OnBoardChkLstObject)Session[Index_Sess_Obj + ActionName];
            }

            var Stafff = DBV.HR_STAFF_VIEW.ToList();
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            var _ListAsset = DB.HRStaffProfiles.Where(w => w.EmpCode == EmpCode).ToList();
            BSM.ListstaffProfile = _ListAsset;

            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
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
            var status = SYDocumentStatus.A.ToString();
            var listBranch = SYConstant.getBranchDataAccess();
            var ListStaffView = DBV.HR_STAFF_VIEW.Where(w => w.StatusCode == status).ToList();
            var AccLevel = SYConstant.getLevelDataAccess();
            ListStaffView = ListStaffView.Where(w => listBranch.Where(x => x.Code == w.BranchID).Any()).ToList();
            ListStaffView = ListStaffView.Where(w => AccLevel.Where(x => x.Code == w.LevelCode).Any()).ToList();
            ViewData["EMP_SELECT"] = ListStaffView.ToList();
        }
        #endregion 

    }
}
