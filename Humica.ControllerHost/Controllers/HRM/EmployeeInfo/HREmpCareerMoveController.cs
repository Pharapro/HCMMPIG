using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Employee;
using Humica.Logic;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HREmpCareerMoveController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000018";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HREmpCareerMove/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";

        IClsEmpCareerMovement BSM;
        public HREmpCareerMoveController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEmpCareerMovement();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            BSM.OnLoandCareerMovement();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsEmployee collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            BSM.Filter = collection.Filter;
            BSM.OnLoandCareerMovement();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpCareerMove);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(KeyName);
            BSM.OnLoandindEmployee();
            return PartialView("PartialProcess", BSM);
        }
        #endregion

        #region "Create"
        public ActionResult Create(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading(ID);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsEmpCareerMovement collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            BSM.ScreenId = SCREEN_ID;
            var msg = BSM.Create();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.ID.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            //return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            return View(BSM);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            DataList();
            if (!string.IsNullOrEmpty(id))
            {
                BSM.OnDetailLoading(id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string id, ClsEmpCareerMovement collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                string msg = "";
                BSM.ScreenId = SCREEN_ID;
                int ID = Convert.ToInt32(id);
                msg = BSM.Update(ID);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.ID.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion

        #region "Details"
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector(true);
            DataList();
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            ViewData[ClsConstant.IS_READ_ONLY1] = true;
            ViewData[ClsConstant.IS_SALARY] = true;
            ViewData[SYConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                BSM.OnDetailLoading(ID);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        //#region "Delete"
        //public ActionResult Delete(string id)
        //{
        //    UserSession();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    BSM.ScreenId = SCREEN_ID;
        //    string msg = BSM.DeleteEmp(id);
        //    if (msg == SYConstant.OK)
        //    {
        //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
        //    }
        //    else
        //    {
        //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //    }
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}
        //#endregion
        #region UnderManager
        public ActionResult GridUnderManager()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridUnderManager", BSM);
        }
        public ActionResult EditUM(HREmpUnderManager MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModify(MModel, SYActionBehavior.EDIT.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridUnderManager", BSM);
        }
        public ActionResult DeleteUM(HREmpUnderManager MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModify(MModel, SYActionBehavior.DELETE.ToString());
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridUnderManager", BSM);
        }
        [HttpPost]
        public string AddEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                var msg = BSM.OnGridModifyAddEmp(EmpCode, "");
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return SYConstant.OK;
                }
                else
                {
                    return SYMessages.getMessage(msg, user.Lang);
                }
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }
        #endregion
        public ActionResult EmployeeSearch()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialEmployeeSearch", BSM);
        }
        public ActionResult EmployeeSearchAdd()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialEmployeeSearchAdd", BSM);
        }
        public ActionResult Refreshvalue(string id, string Increase)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                decimal Inc = Convert.ToDecimal(Increase);
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                BSM.Increase = Increase;
                if(BSM.IsSalary)
                //if (BSM.Increase != "#####")
                    BSM.NewSalary = (Convert.ToDecimal(BSM.OldSalary) + Inc).ToString();
                else
                    BSM.Header.NewSalary = 0;
                var result = new
                {
                    MS = SYConstant.OK,
                    NewSalary = BSM.NewSalary
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult RefreshEmp(string Dept)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                BSM.OnLoadingEmpByDept(Dept);
                var result = new
                {
                    MS = SYConstant.OK,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult AddEmpUM(string DocType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID1] = true;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            if (DocType != null)
            {
                var msg = BSM.OnGridModifyAdd(DocType);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    var result = new
                    {
                        MS = SYConstant.OK,
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var rs1 = new { MS = msg };
                    return Json(rs1, JsonRequestBehavior.DenyGet);
                }
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult TransferTo(string DocType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID1] = true;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
            }
            if (DocType != null)
            {
                var msg = BSM.OnGridModifyTransfer(BSM.EmpID, DocType);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    var result = new
                    {
                        MS = SYConstant.OK,
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var rs1 = new { MS = msg };
                    return Json(rs1, JsonRequestBehavior.DenyGet);
                }
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataList()
        {
            //foreach (var data in BSM.OnDataListLoading())
            //{
            //    ViewData[data.Key] = data.Value;
            //}
        }
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmpCareerMovement)Session[Index_Sess_Obj + ActionName];
                BSM.EmpID = EmpCode;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }
        public ActionResult SelectCriteriaType(string Code)
        {
            ClsFilterJob FilterJob = new ClsFilterJob();
            ViewData["SEPARATE_SELECT"] = FilterJob.LoadSeperateType(Code).ToList();
            var data = new
            {
                MS = SYConstant.OK,
                DATA = ViewData["SEPARATE_SELECT"]
            };
            return Json(data, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            foreach (var data in BSM.OnDataJobLoading(IsDetail))
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}