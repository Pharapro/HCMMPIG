using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.RCM
{

    public class HRMedicalChkController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRE0000010";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRMedicalChk/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HRMedicalChkController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

		#region "List" 
		public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            await DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRMedicalChkObject BSM = new HRMedicalChkObject();
            BSM.ListHeader = new List<HREmpPhischk>();
            BSM.ListHeader = DB.HREmpPhischks.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
		public async Task<ActionResult> PartialList()
        {
            ActionName = "Index";
            HRMedicalChkObject BSM = new HRMedicalChkObject();
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRMedicalChkObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
		#endregion
		#region Create
		public async Task<ActionResult> Create()
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();

            HRMedicalChkObject BSD = new HRMedicalChkObject();

            BSD.Header = new HREmpPhischk();
            BSD.ListHeader = new List<HREmpPhischk>();
            BSD.Header.CHKDate = DateTime.Now;
            BSD.HeaderStaff = new HR_STAFF_VIEW();
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
		public async Task<ActionResult> Create(HRMedicalChkObject collection)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (HRMedicalChkObject)Session[Index_Sess_Obj + ActionName];

            if (ModelState.IsValid)
            {
                string msg = collection.saveEmpMChk();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?TranNo=" + mess.DocumentNumber;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;

                    BSM = await NewMChk();

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
		#region Edit
		public async Task<ActionResult> Edit(int TranNo)
        {
            ActionName = "Edit";
            UserSession();
            await DataSelector();
            HRMedicalChkObject BSM = new HRMedicalChkObject();
            UserConfListAndForm();
            BSM.Header = DB.HREmpPhischks.FirstOrDefault(w => w.TranNo == TranNo);
            if (BSM.Header != null)
            {
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.SingleOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MATERIAL_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
		public async Task<ActionResult> Edit(int TranNo, HRMedicalChkObject collection)
        {
            ActionName = "Edit";
            UserSession();
            await DataSelector();
            UserConfListAndForm();

            HRMedicalChkObject BSM = new HRMedicalChkObject();

            BSM = (HRMedicalChkObject)Session[Index_Sess_Obj + ActionName];
            collection.ScreenId = SCREEN_ID;

            if (ModelState.IsValid)
            {
                string msg = collection.UpdEmpMChk(TranNo);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }

                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = TranNo.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?TranNo=" + mess.DocumentNumber;
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
		#region Details
		public async Task<ActionResult> Details(int TranNo)
        {
            ActionName = "Details";
            UserSession();
            HRMedicalChkObject BSM = new HRMedicalChkObject();
            await DataSelector();
            ViewData[SYConstant.PARAM_ID] = TranNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            BSM.Header = DB.HREmpPhischks.FirstOrDefault(w => w.TranNo == TranNo);
            var chkData = DB.HREmpPhischks.Where(w => w.TranNo == TranNo).Select(x => x.EmpCode);
            if (BSM.Header == null)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("Error");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            BSM.HeaderStaff = DBV.HR_STAFF_VIEW.SingleOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
            UserConfForm(SYActionBehavior.VIEW);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region Delete  
        public ActionResult Delete(int TranNo)
        {
            UserSession();
            HRMedicalChkObject BSM = new HRMedicalChkObject();
            if (TranNo > 0)
            {

                BSM.Header = DB.HREmpPhischks.FirstOrDefault(w => w.TranNo == TranNo);
                string msg = BSM.dtEmpMCkh(TranNo);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
		#endregion
		#region private code
		private async Task<HRMedicalChkObject> NewMChk()
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();

            HRMedicalChkObject BSD = new HRMedicalChkObject();
            BSD.Header = new HREmpPhischk();
            BSD.ListHeader = new List<HREmpPhischk>();
            BSD.Header.CHKDate = DateTime.Now;

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return BSD;
        }
        public ActionResult ShowDataEmp(String EmpCode, string Action)
        {
            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    EmpName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmployeeType,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.Level,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            else
            {
                var rs = new { MS = SYConstant.FAIL };
                return Json(rs, JsonRequestBehavior.DenyGet);
            }
        }
		private async Task DataSelector()
        {
			HRStaffProfileObject Staff = new HRStaffProfileObject();
			ViewData["STAFF_SELECT"] = await Staff.LoadData();
			ViewData["HOSP_SELECT"] = DB.HRHospitals.ToList();
            ViewData["MEDICALTYPE_SELECT"] = DB.HRMedicalTypes.ToList();
            ViewData["PHCHKRESULT_SELECT"] = DB.HRPHCKHResults.ToList();
        }
        #endregion
    }
}
