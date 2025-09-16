using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{

    public class LeaveEntitledController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000001";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/LeaveEntitled/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();

        public LeaveEntitledController()
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
            BSM.FInYear = new Core.FT.FTFilerIndex();
            BSM.FInYear.InYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (GenerateLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Generate_Leave_Cu(BSM.FInYear.InYear);
            var ListEmpLeaveB = DB.HREmpLeaveBs.ToList();
            ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
            BSM.ListEmpLeaveB = ListEmpLeaveB.Where(w => w.EmpCode == BSM.User.UserName && w.InYear == BSM.FInYear.InYear).ToList();
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
            var ListEmpLeaveB = DB.HREmpLeaveBs.ToList();
            ListEmpLeaveB.Where(w => w.ForWardExp.Value.Year == 1900).ToList().ForEach(x => x.ForWardExp = null);
            BSM.ListEmpLeaveB = ListEmpLeaveB.Where(w => w.EmpCode == BSM.User.UserName && w.InYear == BSM.FInYear.InYear).ToList();
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
