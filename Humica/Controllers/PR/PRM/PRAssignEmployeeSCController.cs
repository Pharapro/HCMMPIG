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
    public class PRAssignEmployeeSCController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000010";
        private const string URL_SCREEN = "/PR/PRM/PRAssignEmployeeSC/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public PRAssignEmployeeSCController()
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

            PREmpSVCObject BSM = new PREmpSVCObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListHeader = DBV.PR_EMPSVC_VIEW.ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpSVCObject BSM = new PREmpSVCObject();
            BSM.ListHeader = new List<PR_EMPSVC_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PREmpSVCObject BSM = new PREmpSVCObject();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PREmpSVC();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.Header.StartDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        [HttpPost]
        public ActionResult Create(PREmpSVCObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpSVCObject BSM = new PREmpSVCObject();
            BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
            BSM.Filter = collection.Filter;
            BSM.Header = collection.Header;
            BSM.EmpCode = collection.Filter.EmpCode;
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                try
                {
                    string msg = BSM.CreateSVCs();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.Header.TranNo.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        BSM.Header = new PREmpSVC();
                        BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                        BSM.Header.StartDate = DateTime.Now;
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
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        public ActionResult Edit(int id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                PREmpSVCObject BSM = new PREmpSVCObject();
                BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
                BSM.Header = new PREmpSVC();
                if (BSM.Header != null)
                {
                    BSM.Header = DB.PREmpSVCs.Find(id);
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
                        BSM.Filter.EndProbation = res.Probation.Value;
                        string StatusV = "";
                        string ResignDatev = "";
                        if (res.TerminateStatus == "")
                        { StatusV = "Active"; }
                        else { StatusV = "Inactive"; ResignDatev = res.DateTerminate.Value.ToString("dd-MMM-yyyy"); }
                        BSM.Filter.Status = StatusV;
                        BSM.Filter.ResignDate = ResignDatev;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PREmpSVCObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PREmpSVCObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpSVCObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = collection.Filter;
                BSM.Header = collection.Header;
            }

            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditSVC(id);
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
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != 0)
            {
                PREmpSVCObject GLA = new PREmpSVCObject();
                string msg = GLA.DeleteEmpSVCs(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SVC_DEL", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("SVC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }

        public ActionResult ShowData(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                string StatusV = "";
                string ResignDatev = "";
                if (EmpStaff.TerminateStatus == "")
                { StatusV = "Active"; }
                else { StatusV = "Inactive"; ResignDatev = EmpStaff.DateTerminate.Value.ToString("dd-MMM-yyyy"); }
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.Level,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    Status = StatusV,
                    ResignDate = ResignDatev,
                    EndProbation = EmpStaff.Probation,
                    StartDateSVC = EmpStaff.StartDate
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult chkInactive(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            var EMPSVC = DB.PREmpSVCs.FirstOrDefault(w => w.EmpCode == EmpCode);
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                string StatusV = "";
                DateTime ResignDatev = new DateTime();
                if (EMPSVC == null)
                {
                    if (EmpStaff.TerminateStatus == "")
                        ResignDatev = DateTime.Now;
                    else
                        ResignDatev = EmpStaff.DateTerminate.Value;
                }
                else
                {
                    if (EMPSVC.EndDate.Value.Year == 5000)
                        ResignDatev = DateTime.Now;
                    else ResignDatev = EMPSVC.EndDate.Value;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    ResignDate = ResignDatev
                };

                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {

            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            string ALLType = "ALLW";
            ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
    }
}