using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class PRInsuranceController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRB0000002";
        private const string URL_SCREEN = "/PR/PRM/PRInsurance/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo;";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public PRInsuranceController()
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
            UserConfListAndForm(this.KeyName);

            HRInsuranceObject BSM = new HRInsuranceObject();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeaders = DB.HREmpInsurances.ToList();
            BSM.ListHeader = ListHeaders.OrderBy(w => w.EmpCode).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.ListHeader = new List<HREmpInsurance>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader.OrderBy(w => w.EmpCode).ToList());
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.Header = new HREmpInsurance();
            BSM.Header.StartDate = DateTime.Now;
            BSM.Header.EndDate = DateTime.Now;
            BSM.Header.Amount = 0;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HRInsuranceObject collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
            HRInsuranceObject BSM = new HRInsuranceObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    BSM.HeaderStaff = collection.HeaderStaff;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEmp();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new HRInsuranceObject();
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    BSM.Header = new HREmpInsurance();
                    BSM.Header.StartDate = DateTime.Now;
                    BSM.Header.EndDate = DateTime.Now;
                    BSM.Header.Amount = 0;
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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region "Edit"
        public ActionResult Edit(int id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.Header = new HREmpInsurance();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header = DB.HREmpInsurances.FirstOrDefault(w => w.TranNo == id);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, HRInsuranceObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            // ViewData[SYSConstant.PARAM_ID] = id;
            HRInsuranceObject BSM = new HRInsuranceObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRInsuranceObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.EditEmp(id);
            if (msg == SYConstant.OK)
            {
                DB = new HumicaDBContext();
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Delete"
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            HRInsuranceObject Del = new HRInsuranceObject();
            string msg = Del.DeleteEmp(id);
            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Details"
        public ActionResult Details(int id)
        {
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = id;

            HRInsuranceObject BSM = new HRInsuranceObject();
            BSM.Header = new HREmpInsurance();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header = DB.HREmpInsurances.FirstOrDefault(w => w.TranNo == id);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
                //   }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
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
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.Where(w => w.Status == "A").ToList();
            ViewData["InsuranceTypes_SELECT"] = DB.HRInsuranceTypes.ToList();
            ViewData["InsuranceCompany_SELECT"] = DB.HRInsuranceCompanies.Where(w => w.IsActive == true).ToList();
        }
    }
}
