using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Atts;
using Humica.Logic.HR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ATAssignEmpBatchController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATM0000009";
        private const string URL_SCREEN = "/Attendance/Attendance/ATAssignEmpBatch/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ATAssignEmpBatchController()
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
            DataSelector();

            ATBatchObject BSM = new ATBatchObject();
            BSM.ListStaffs = new List<HRStaffProfile>();
            BSM.Filter = new Core.FT.FTFilterAttendance();
            BSM.Filter.Status = SYDocumentStatus.A.ToString();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                BSM = obj;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATBatchObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            var staff = HRStaffProfileObject.LoadAllStaff(BSM.Filter);
            
            if (BSM.Filter.Status != "I/A")
            {
                staff = staff.Where(w => w.Status == BSM.Filter.Status).ToList();
            }
            BSM.ListStaffs = staff.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ATBatchObject BSM = new ATBatchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListStaffs);
        }

        #endregion

        public ActionResult AssignStaff(string EmpCode, string Batch)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ATBatchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATBatchObject)Session[Index_Sess_Obj + ActionName];
                if (Batch == "null") Batch = "";
                BSM.Filter.Shift = Batch;
            }
            if (EmpCode != "")
            {
                var msg = BSM.AssStaffRoster(EmpCode, BSM.Filter.Shift);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }


        private void DataSelector()
        {
            var objStatus = new SYDataList("STATUS_EMPLOYEE");
            ViewData["STATUS_EMPLOYEE"] = objStatus.ListData;

            ViewData["BATCH_SELECT"] = DB.ATBatches.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["BUSINESSUNIT_SELECT"] = ClsFilter.LoadBusinessUnit();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["OFFICE_SELECT"] = ClsFilter.LoadOffice();
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData["GROUP_SELECT"] = ClsFilter.LoadGroups();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
        }
    }
}
