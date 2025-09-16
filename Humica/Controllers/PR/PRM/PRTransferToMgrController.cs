using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRTransferToMgrController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000028";
        private const string URL_SCREEN = "/PR/PRM/PRTransferToMgr/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRTransferToMgrController()
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
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            BSM.ListHeader = new List<PRTransferToMgr>();
            BSM.ListHeader = DB.PRTransferToMgrs.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
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

            PRTransferToMgrObject BSD = new PRTransferToMgrObject();

            BSD.Header = new PRTransferToMgr();
            BSD.ListItem = new List<PRTransferToMgrItem>();
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(PRTransferToMgrObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                string msg = BSM.TransferToMgr();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Edit'
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            UserConfListAndForm();

            if (id != "null")
            {
                int ID = Convert.ToInt32(id);
                BSM.Header = DB.PRTransferToMgrs.FirstOrDefault(w => w.ID == ID);
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.PRTransferToMgrItems.Where(w => w.ID == BSM.Header.Increment).ToList();
                    var ChkEmp = BSM.ListItem.Select(x => x.EmpCode);
                    var LstStaff = DB.HRStaffProfiles.Where(w => w.BankName == "CC" && w.Branch == BSM.Header.Branch).ToList();
                    var LstVStaff = DBV.HR_STAFF_VIEW.ToList();
                    var _Staff = LstVStaff.Where(w => LstStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();

                    if (_Staff.Count > 0)
                    {
                        BSM.ListView = _Staff.ToList();
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PRTransferToMgrObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;
            if (ModelState.IsValid)
            {
                string msg = BSM.upd(id);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber; ;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.VIEW);
            DataSelector();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            int ID = Convert.ToInt32(id);
            BSM.Header = DB.PRTransferToMgrs.FirstOrDefault(w => w.ID == ID);
            if (BSM.Header != null)
            {

                BSM.ListItem = DB.PRTransferToMgrItems.Where(w => w.ID == BSM.Header.Increment).ToList();

                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Delete'  
        public ActionResult Delete(string id)
        {
            UserSession();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            if (id != "null")
            {
                int ID = Convert.ToInt32(id);
                BSM.Header = DB.PRTransferToMgrs.FirstOrDefault(w => w.ID == ID);
                if (BSM.Header != null)
                {
                    string msg = BSM.delete(id);

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
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'GridItems'
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItemDetails", BSM);
        }
        public ActionResult CreateItem(PRTransferToMgrItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.EmpCode == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EMP_NEEDED", user.Lang);
                    }
                    var chkEmp = BSM.ListItem.FirstOrDefault(w => w.EmpCode == ModelObject.EmpCode);
                    if (chkEmp != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("Duplicate", user.Lang);
                    }
                    else
                    {
                        BSM.ListItem.Add(ModelObject);
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
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        public ActionResult EditItem(PRTransferToMgrItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.EmpCode == ModelObject.EmpCode).ToList();
                if (objCheck.Count > 0)
                {
                    objCheck.First().EmpCode = ModelObject.EmpCode;
                    objCheck.First().EmpName = ModelObject.EmpName;
                    objCheck.First().Dept = ModelObject.Dept;
                    objCheck.First().JobCode = ModelObject.JobCode;

                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult DeleteItem(string EmpCode)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.EmpCode == EmpCode).ToList();

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
            return PartialView("GridItems", BSM);
        }
        #endregion 
        #region 'private code'
        public ActionResult ShowData(string EmpCode, string Action)
        {
            ActionName = Action;

            PRTransferToMgrObject BSM = new PRTransferToMgrObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];
            }
            var _BranchMgr = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);

            if (_BranchMgr != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Name = _BranchMgr.AllName,
                    Branch = _BranchMgr.Branch,
                    Post = _BranchMgr.JobCode,
                    Dept = _BranchMgr.DEPT,
                    BankAcc = _BranchMgr.BankAcc
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult ShowDataItem(string MgrCode, string Action)
        {

            ActionName = Action;
            PRTransferToMgrObject BSM = new PRTransferToMgrObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRTransferToMgrObject)Session[Index_Sess_Obj + ActionName];

                if (BSM.ListItem != null)
                {
                    BSM.ListItem.Clear();
                }
                var LstStaff = DB.HRStaffProfiles.ToList();
                var staff = LstStaff.FirstOrDefault(w => w.EmpCode == MgrCode);
                var LstVStaff = DBV.HR_STAFF_VIEW.ToList();
                var _Staff = LstVStaff.Where(w => LstStaff.Where(x => x.EmpCode == w.EmpCode
                && x.Branch == staff.Branch && x.BankName == "CC").Any()).ToList();

                if (_Staff.Count > 0)
                {
                    BSM.ListView = _Staff.ToList();
                }
                else
                {
                    if (BSM.ListItem != null)
                    {
                        BSM.ListItem.Clear();
                    }
                    var result1 = new { MS = SYConstant.NA };
                    return Json(result1, JsonRequestBehavior.DenyGet);
                }
            }
            var result = new { MS = SYConstant.OK, };

            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData["EMPCODE_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["BRANCHES_SELECT"] = SMS.HRBranches.ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
        }
        #endregion 
    }
}
