using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.PR;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Attendance.Attendance
{

    public class PRAssignGLController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000017";
        private const string URL_SCREEN = "/PR/PRM/PRAssignGL/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRAssignGLController()
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

            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ListStaffs = new List<HRStaffProfile>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PRGLmappingObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            var staff = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            var staffcheck = staff.ToList();

            if (BSM.Branch != null)
            {
                staffcheck = staffcheck.Where(w => w.Branch == BSM.Branch).ToList();
            }

            if (BSM.Division != null)
            {
                staffcheck = staffcheck.Where(w => w.Division == BSM.Division).ToList();
            }

            if (BSM.Department != null)
            {
                staffcheck = staffcheck.Where(w => w.DEPT == BSM.Department).ToList();
            }

            BSM.ListStaffs = staffcheck.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;


            //if(BSM.EmpCode != null)
            //{
            //    string[] multiArrayBill = BSM.EmpCode.Split(';');

            //    foreach (var item in multiArrayBill)
            //    {
            //        if (item.Trim() != "")
            //        {
            //            string EmpCodeS = item.ToString();
            //            var StaffUpdate = staff.Where(w=>w.EmpCode== EmpCodeS).First();
            //            if(StaffUpdate != null)
            //            {
            //                StaffUpdate.GrpGLAcc = BSM.GLMCode;
            //                DB.Entry(StaffUpdate).State = EntityState.Modified;
            //            }
            //        }
            //    }
            //    DB.SaveChanges();

            //    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
            //    mess.DocumentNumber = "";
            //    //mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
            //    Session[SYConstant.MESSAGE_SUBMIT] = mess;
            //    BSM.EmpCode = null;
            //    Session[Index_Sess_Obj + ActionName] = BSM;
            //    return View(BSM);
            //}
            //Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListStaffs);
        }

        #endregion

        public ActionResult AssignStaff(string EmpCode, string GL)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new PRGLmappingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            if (EmpCode != "")
            {

                var msg = BSM.SetStaffGL(EmpCode, GL);

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
            //ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["BRANCHES_SELECT"] = SMS.HRBranches.ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData["GLMAPPING_SELECT"] = DB.PRCostCenters.ToList();//.Select(p => new { p.GLMCode, p.Description })                .Distinct().ToList();
        }
    }
}
