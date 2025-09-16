using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.LM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class ESSCompenLeaveController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000018";
        private const string URL_SCREEN = "/SelfService/LeaveBalance/ESSCompenLeave/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ESSCompenLeaveController()
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

            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.FInYear = obj.FInYear;
            }
            BSM.ListHeader = new List<HRClaimLeave>();
            BSM.ListHeader = DB.HRClaimLeaves.Where(w => w.WorkingDate.Year == BSM.FInYear.INYear && w.EmpCode == user.UserName).OrderByDescending(x => x.WorkingDate).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClaimLeaveObject collection)
        {
            DataSelector();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.ListHeader = new List<HRClaimLeave>();
            BSM.FInYear = collection.FInYear;
            BSM.ListHeader = DB.HRClaimLeaves.Where(w => w.WorkingDate.Year == BSM.FInYear.INYear && w.EmpCode == user.UserName).OrderByDescending(x => x.WorkingDate).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<HRClaimLeave>();

            var emp = DBV.HRStaffProfiles.Where(w => w.EmpCode == BSM.User.UserName).ToList();
            if (emp.Count > 0)
            {
                BSM.Header_Staff = emp.FirstOrDefault();
                BSM.Header = new HRClaimLeave();
                BSM.ListHeader = new List<HRClaimLeave>();
                BSM.Header.WorkingDate = DateTime.Now;
                BSM.Header.WorkingHour = 8;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ClaimLeaveObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            var BSM = new ClaimLeaveObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                BSM.Header.EmpCode = BSM.Header_Staff.EmpCode;
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string URL = SYUrl.getBaseUrl() + "/SelfService/MyTeam/MyTeamLeaveRequest/Details/";
                string msg = BSM.ClaimLeave(fileName, true);

                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSM.Header.TranNo.ToString();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(id))
            {
                BSM.ListApproval = new List<ExDocApproval>();
                int TranNo = Convert.ToInt32(id);
                var ListStaff = DBV.HR_STAFF_VIEW.ToList();
                ListStaff = ListStaff.Where(w => w.DateTerminate == null).ToList();
                BSM.Header = DB.HRClaimLeaves.FirstOrDefault(w => w.TranNo == TranNo);
                BSM.HeaderStaff = ListStaff.FirstOrDefault(x => x.EmpCode == BSM.Header.EmpCode);
                BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == BSM.Header.TranNo.ToString()).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        public ActionResult GridItemApprover()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ClaimLeaveObject BSM = new ClaimLeaveObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClaimLeaveObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemApprover";
            return PartialView("GridItemApprover", BSM.ListApproval);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("CLAIM_LEAVE");
            ViewData["WORKINGTYPE_SELECT"] = objList.ListData;
            ViewData["LEAVETYPE_SELECT"] = DB.HRLeaveTypes.Where(x => x.IsParent != true).ToList().OrderBy(w => w.Description);
        }
    }
}