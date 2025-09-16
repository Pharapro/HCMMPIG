using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class MyLeaveEntitledController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000004";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyLeaveEntitled/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public MyLeaveEntitledController()
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

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            BSM.FInYear = new Core.FT.FTFilerIndex();
            BSM.FInYear.InYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);

            if (staff != null)
            {
                var ListEmpLeaveB = DB.HREmpLeaveBs.Where(w => w.InYear == BSM.FInYear.InYear).ToList();
                var _listEmp = DB.HRStaffProfiles.Where(w => w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode).ToList();
                ListEmpLeaveB = ListEmpLeaveB.Where(w => _listEmp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
                BSM.ListEmpLeaveB = ListEmpLeaveB.ToList();
            }

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(GenerateLeaveObject BSM)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.Generate_Leave_Cu(BSM.FInYear.InYear);
            BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);

            if (staff != null)
            {
                var ListEmpLeaveB = DB.HREmpLeaveBs.Where(w => w.InYear == BSM.FInYear.InYear).ToList();
                var _listEmp = DB.HRStaffProfiles.Where(w => w.FirstLine == staff.EmpCode || w.SecondLine == staff.EmpCode || w.HODCode == staff.EmpCode).ToList();

                ListEmpLeaveB = ListEmpLeaveB.Where(w => _listEmp.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
                ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
                BSM.ListEmpLeaveB = ListEmpLeaveB.ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ListEmpLeaveB = new List<HREmpLeaveB>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpLeaveB);
        }
        #endregion

        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpLeaveB);
        }
        private void DataSelector()
        {
            ViewData["LeaveTypes_SELECT"] = DB.HRLeaveTypes.ToList();
        }
    }
}
