using DevExpress.CodeParser;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PREmpAdvPayController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000009";
        private const string URL_SCREEN = "/PR/PRM/PREmpAdvPay/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PREmpAdvPayController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            PREmpADVPay BSM = new PREmpADVPay();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpADVPay)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = new List<PRADVPay>();

            var _Branch = SYConstant.getBranchDataAccess();
            var Staff = DB.HRStaffProfiles.AsEnumerable().Where(x => _Branch.Where(w => w.Code == x.Branch).Any()).ToList();
            var list = DB.PRADVPays.AsEnumerable().Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();
            if (list.Any()) BSM.ListHeader = list;

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpADVPay BSM = new PREmpADVPay();
            BSM.ListHeader = new List<PRADVPay>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpADVPay)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult Create()
        {
            ActionName = "Create";
            Datastaff();
            UserSession();
            PREmpADVPay BSM = new PREmpADVPay();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PRADVPay();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.Header.TranDate = DateTime.Now;
            BSM.Header.Amount = 0;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        [HttpPost]
        public ActionResult Create(PREmpADVPay collection)
        {
            ActionName = "Create";
            UserSession();
            Datastaff();
            UserConfListAndForm(this.KeyName);
            PREmpADVPay BSM = new PREmpADVPay();
            BSM = (PREmpADVPay)Session[Index_Sess_Obj + ActionName];
            BSM.Filter = collection.Filter;
            BSM.Header = collection.Header;
            BSM.EmpCode = collection.Filter.EmpCode;
            BSM.ScreenId = SCREEN_ID;
            try
            {
                string msg = BSM.CreateAdvPay();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new PRADVPay();
                    BSM.ListHeader = new List<PRADVPay>();
                    BSM.EmpCode = "";
                    BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                    BSM.Header.TranDate = DateTime.Now;
                    BSM.Header.Amount = 0;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            Datastaff();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                PREmpADVPay BSM = new PREmpADVPay();
                int trn = 0;
                trn = Convert.ToInt32(id);
                BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                BSM.Header = DB.PRADVPays.Find(trn);
                if (BSM.Header != null)
                {
                    var res = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    if (res != null)
                    {
                        BSM.Filter.EmpCode = res.EmpCode;
                        BSM.Filter.AllName = res.AllName;
                        BSM.Filter.EmpType = res.EmployeeType;
                        BSM.Filter.Division = res.Division;
                        BSM.Filter.Department = res.Department;
                        BSM.Filter.Section = res.Section;
                        BSM.Filter.Position = res.Position;
                        BSM.Filter.Level = res.Level;
                        BSM.Filter.StartDate = Convert.ToDateTime(res.StartDate);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PREmpADVPay collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PREmpADVPay();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpADVPay)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = collection.Filter;
                BSM.Header = collection.Header;
            }

            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditAdvPay(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            //EE001= Message load Error data not complate
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id == "null") id = null;
            if (id != null)
            {
                int trn = Convert.ToInt32(id);
                PREmpADVPay GLA = new PREmpADVPay();
                string msg = GLA.DeleteAdvPay(trn);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("ADVPAY_DEL", user.Lang);
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

        public ActionResult ShowData(string ID, string EmpCode)
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
                    Division = EmpStaff.Division,
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
            string ALLType = "ALLW";
            ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
        private void Datastaff()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;
        }
    }
}