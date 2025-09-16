using DevExpress.DataProcessing;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HREmpSelfEvaluationController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000007";
        private const string URL_SCREEN = "/HRM/Appraisal/HREmpSelfEvaluation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EvaluationCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HREmpSelfEvaluationController()
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
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HREmpSelfEvaluations.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            BSM.Header = new HREmpSelfEvaluation();
            BSM.ListItem = new List<HREmpSelfEvaluationItem>();
            BSM.ListSelfAssItem = new List<HREvalSelfEvaluate>();
            BSM.Header.EvaluationDate = DateTime.Now;
            BSM.ListSelfAssItem = DB.HREvalSelfEvaluates.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HRSelfEvaluationObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;
                string msg = BSM.CreateSelfEval();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = collection.Header.EvaluationCode.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;

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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (ID != null)
            {
                HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
                BSM.Header = DB.HREmpSelfEvaluations.FirstOrDefault(w => w.EvaluationCode == ID);
                BSM.ListItem = new List<HREmpSelfEvaluationItem>();
                BSM.ListSelfAssItem = DB.HREvalSelfEvaluates.ToList();
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.HREmpSelfEvaluationItems.Where(w => w.EvaluationCode == ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]

        public ActionResult Edit(string id, HRSelfEvaluationObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditEval(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
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
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                //int TranNo = Convert.ToInt32(id);
                HREmpEvaluateObject Del = new HREmpEvaluateObject();
                string msg = Del.DeleteAppr(id);
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
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
                BSM.Header = DB.HREmpSelfEvaluations.FirstOrDefault(w => w.EvaluationCode == id);
                BSM.ListItem = new List<HREmpSelfEvaluationItem>();
                BSM.ListSelfAssItem = DB.HREvalSelfEvaluates.ToList();
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.HREmpSelfEvaluationItems.Where(w => w.EvaluationCode == id).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Details
        public ActionResult ShowDataEmp(string ID, string EmpCode, string Action)
        {

            ActionName = Action;
            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            BSM.Header = new HREmpSelfEvaluation();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];
            }

            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    POST = EmpStaff.Position,
                    NAME = EmpStaff.AllName,
                    DEPT = EmpStaff.Department,
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult CommentValue(string Code, string Value, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HRSelfEvaluationObject BSM = new HRSelfEvaluationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRSelfEvaluationObject)Session[Index_Sess_Obj + ActionName];

                var checkexist = BSM.ListItem.Where(w => w.QuestionCode == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new HREmpSelfEvaluationItem();
                    obj.QuestionCode = Code;
                    obj.Comment = Value;
                    BSM.ListItem.Add(obj);
                }
                else
                {
                    checkexist.First().QuestionCode = Code;
                    checkexist.First().Comment = Value;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.Where(w => w.StatusCode == "A").ToList();
            //ViewData["APPRTYPE_SELECT"] = DB.HREvaluateTypes.ToList();
        }
    }
}