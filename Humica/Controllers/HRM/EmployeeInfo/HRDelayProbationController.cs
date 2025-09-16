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

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRDelayProbationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000007";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRDelayProbation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HRDelayProbationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region List
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListDP = new List<HRDelayProbation>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            await BSM.LoadDataDelayProbation();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListDP);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSM.ListHeader = new List<HR_Family_View>();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM);
        }
		#endregion
		#region Create
		public async Task<ActionResult> Create()
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListDP = new List<HRDelayProbation>();
            BSM.HeaderDP = new HRDelayProbation();
            BSM.HeaderDP.Day = 0;
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
		public async Task<ActionResult> Create(HRStaffProfileObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
			await DataSelector();
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.HeaderDP = collection.HeaderDP;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreatProbation();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderDP.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.HeaderDP = new HRDelayProbation();
                    BSM.HeaderDP.Day = 0;
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
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
		#region Edit
		public async Task<ActionResult> Edit(string ID)
        {
            ActionName = "Edit";
            UserSession();
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                HRStaffProfileObject BSM = new HRStaffProfileObject();
                BSM.HeaderDP = new HRDelayProbation();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.HeaderDP = DB.HRDelayProbations.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.HeaderDP != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.HeaderDP.EmpCode);
                    List<HRDelayProbation> listEmpfa = DB.HRDelayProbations.Where(x => x.EmpCode == BSM.HeaderDP.EmpCode).ToList();
                    BSM.ListDP = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]

		public async Task<ActionResult> Edit(string id, HRStaffProfileObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            await DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.HeaderDP = collection.HeaderDP;
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditProbation(TranNo);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    // BSM.ListHeader = DB.HREmpFamilies.ToList();

                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
		#endregion
		#region "Delete"
		public async Task<ActionResult> Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                HRStaffProfileObject Del = new HRStaffProfileObject();
                string msg = Del.DeletePro(TranNo);
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
		public async Task<ActionResult> Details(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                HRStaffProfileObject BSM = new HRStaffProfileObject();
                BSM.HeaderDP = new HRDelayProbation();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(id);
                BSM.HeaderDP = DB.HRDelayProbations.FirstOrDefault(w => w.TranNo == TranNo);
                if (BSM.HeaderDP != null)
                {
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.HeaderDP.EmpCode);
                    List<HRDelayProbation> listEmpfa = DB.HRDelayProbations.Where(x => x.EmpCode == BSM.HeaderDP.EmpCode).ToList();
                    BSM.ListDP = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
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
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    EndProbation = EmpStaff.Probation
                };
                GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = DB.HRDelayProbations;
                List<HRDelayProbation> listEmpFa = resualt.Where(x => x.EmpCode == EmpCode).ToList();
                BSM.ListDP = listEmpFa.OrderByDescending(x => x.TranNo).ToList();
                //int tranNo = Convert.ToInt32(listEmpFa.Max(w => w.TranNo));
                // BSM.IsValidChecking(tranNo);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        public ActionResult Refreshvalue(string EmpCode, string Month, string Action)
        {
            ActionName = Action;
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.HeaderDP = new HRDelayProbation();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                int Inc = Convert.ToInt32(Month);
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.HeaderDP == null)
                    BSM.HeaderDP = new HRDelayProbation();
                BSM.HeaderDP.Day = Inc;
                var Stafff = DBV.HR_STAFF_VIEW.ToList();
                HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
                //BSM.HeaderDP.Probation = BSM.HeaderCareer.OldSalary + Inc;
                if (EmpStaff != null)
                {
                    var result = new
                    {
                        MS = SYConstant.OK,
                        EndProbation = EmpStaff.Probation.Value.AddMonths(Inc)
                    };

                    return Json(result, JsonRequestBehavior.DenyGet);
                }
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private async Task DataSelector()
        {
            HRStaffProfileObject Staff = new HRStaffProfileObject();

            ViewData["STAFF_SELECT"] = await Staff.LoadData();
        }
    }
}