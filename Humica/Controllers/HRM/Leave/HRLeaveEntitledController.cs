using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Leave
{
    public class HRLeaveEntitledController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRL0000004";
        private const string URL_SCREEN = "/HRM/Leave/HRLeaveEntitled/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public HRLeaveEntitledController()
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
            //DataSelector();

            GenerateLeaveObject BSM = new GenerateLeaveObject();
            BSM.FInYear = new FTFilerIndex();
            BSM.FInYear.InYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Generate_Leave_Cu(BSM.FInYear.InYear);
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DB.HRStaffProfiles.Where(w => (w.Status == "A" || (w.Status == "I" && w.DateTerminate.Year >= BSM.FInYear.InYear))).ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var ListEmpLeaveB = DB.HREmpLeaveBs.Where(w => w.InYear == BSM.FInYear.InYear).ToList();
            ListEmpLeaveB = ListEmpLeaveB.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
            BSM.ListEmpLeaveB = ListEmpLeaveB.OrderBy(w => w.EmpCode).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(GenerateLeaveObject BSM)
        {
            //DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.Generate_Leave_Cu(BSM.FInYear.InYear);
            var ListBranch = SYConstant.getBranchDataAccess();
            var ListStaff = DB.HRStaffProfiles.ToList();
            ListStaff = ListStaff.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            var ListEmpLeaveB = DB.HREmpLeaveBs.Where(w => w.InYear == BSM.FInYear.InYear).ToList();
            ListEmpLeaveB = ListEmpLeaveB.Where(w => ListStaff.Where(x => x.EmpCode == w.EmpCode).Any()).ToList();
            ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
            BSM.ListEmpLeaveB = ListEmpLeaveB.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
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
            //DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmpLeaveB);
        }
    }
}
