using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Attendance.Attendance
{

    public class ATSetRosterController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000008";
        private const string URL_SCREEN = "/Attendance/Attendance/ATSetRoster/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity DBA = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public ATSetRosterController()
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

            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.Filter = new FTFilterEmployee();
            BSM.ListStaffs = new List<HRStaffProfile>();
            BSM.Filter.FromDate = DateTime.Now;
            BSM.Filter.ToDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATEmpSchObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            var staff = DB.HRStaffProfiles.ToList();
            var staffcheck = staff.ToList();

            if (BSM.Filter.Branch != null)
                staffcheck = staffcheck.Where(w => w.Branch == BSM.Filter.Branch).ToList();
            if (BSM.Filter.Locations != null)
                staffcheck = staffcheck.Where(w => w.LOCT == BSM.Filter.Locations).ToList();
            if (BSM.Filter.Division != null)
                staffcheck = staffcheck.Where(w => w.Division == BSM.Filter.Division).ToList();
            if (BSM.Filter.Department != null)
                staffcheck = staffcheck.Where(w => w.DEPT == BSM.Filter.Department).ToList();

            staffcheck = staffcheck.Where(w => w.StartDate >= BSM.Filter.FromDate.Date && w.StartDate <= BSM.Filter.ToDate.Date).ToList();
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
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListStaffs);
        }

        #endregion
        public ActionResult Generate(string shift, string EmpCode)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }

            if (shift != null)
            {
                var msg = BSM.Set_Shift(BSM.Filter.FromDate, BSM.Filter.ToDate, EmpCode, shift);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        private void DataSelector()
        {
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["SHIFT_SELECT"] = DB.ATShifts.ToList();
            ViewData["BRANCHES_SELECT"] = DBA.HRBranches.ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
        }
    }
}
