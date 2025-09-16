using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.Performance
{
    public class ESSEvaluationFormController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESSE000001";
        private const string URL_SCREEN = "/SelfService/Performance/ESSEvaluationForm/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EvaluateID;Status";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public ESSEvaluationFormController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListEvaluatePending = new List<HR_VIEW_EmpEvaluationForm>();
            BSM.ListEvaluate = new List<HR_VIEW_EmpEvaluationForm>();
            string open = SYDocumentStatus.OPEN.ToString();
            string pending = SYDocumentStatus.PENDING.ToString();
            string processing = SYDocumentStatus.PROCESSING.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            string cancelled = SYDocumentStatus.CANCELLED.ToString();
            var _ListEvaluate = DBV.HR_VIEW_EmpEvaluationForm.ToList();
            var _ListEvaluatePending = DBV.HR_VIEW_EmpEvaluationForm.ToList();
            BSM.ListEvaluate = _ListEvaluate.Where(x => x.AssignedTo == BSM.User.UserName && x.Status != pending && x.Status != open).OrderByDescending(x => x.EvaluateDate).ToList();
            BSM.ListEvaluatePending = _ListEvaluatePending.Where(x => x.AssignedTo == BSM.User.UserName && x.Status == pending).OrderByDescending(x => x.EvaluateDate).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEvaluate);
        }
        public ActionResult PendingList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PendingList", BSM.ListEvaluatePending);
        }
        #endregion
        #region 'Edit'
        public ActionResult Edit(string EvaluateID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EvaluateID;

            if (EvaluateID != null)
            {
                HREmpEvaluateObject BSM = new HREmpEvaluateObject();

                BSM.Header = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == EvaluateID);
                string cancelled = SYDocumentStatus.CANCELLED.ToString();
                string approved = SYDocumentStatus.APPROVED.ToString();
                if (BSM.Header != null)
                {
                    if (BSM.Header.Status == cancelled || BSM.Header.Status == approved)
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    else
                    {
                        BSM.ListRegion = DB.HREvaluateRegions.Where(w => w.EvaluateType == BSM.Header.EvaluateType).ToList();
                        if (BSM.ListRegion.Count() > 0)
                        {
                            var quiz = DB.HREvaluateFactors.ToList();
                            var Rating = DB.HREmpEvalRatings.ToList();
                            BSM.ListScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == BSM.Header.EvaluateID).ToList();
                            BSM.ListFactor = quiz.Where(w => BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                            BSM.ListEmpRating = Rating.Where(w => w.EvaluateID == BSM.Header.EvaluateID && BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                        }
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string EvaluateID, HREmpEvaluateObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EvaluateID;
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (EvaluateID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditEvaluateForm(EvaluateID);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = EvaluateID;
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
        #region 'Details'
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                HREmpEvaluateObject BSM = new HREmpEvaluateObject();
                BSM.Header = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
                if (BSM.Header != null)
                {
                    BSM.ListRegion = DB.HREvaluateRegions.Where(w => w.EvaluateType == BSM.Header.EvaluateType).ToList();
                    if (BSM.ListRegion.Count() > 0)
                    {
                        var quiz = DB.HREvaluateFactors.ToList();
                        var Rating = DB.HREmpEvalRatings.ToList();
                        BSM.ListScore = DB.HREmpEvaluateScores.Where(w => w.EvaluateID == BSM.Header.EvaluateID).ToList();
                        BSM.ListFactor = quiz.Where(w => BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                        BSM.ListEmpRating = Rating.Where(w => w.EvaluateID == BSM.Header.EvaluateID && BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Details
        #region 'Status'
        public ActionResult Approve(string id)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ApproveDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Code'
        [HttpPost]
        public ActionResult CheckValue(string Value, string Region, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];

                string[] sp = Value.Split('_');
                string Code = sp[0].ToString();
                int EvID = Convert.ToInt32(sp[1]);
                int Score = Convert.ToInt32(sp[2]);

                var checkexist = BSM.ListScore.Where(w => w.Code == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new HREmpEvaluateScore();
                    obj.Code = Code;
                    obj.Region = Region;
                    obj.RatingID = EvID;
                    obj.Score = Score;
                    BSM.ListScore.Add(obj);
                }
                else
                {
                    checkexist.First().Code = Code;
                    checkexist.First().RatingID = EvID;
                    checkexist.First().Score = Score;
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
        [HttpPost]
        public ActionResult CommentValue(string Code, string Value, string Region, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];

                var checkexist = BSM.ListScore.Where(w => w.Code == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new HREmpEvaluateScore();
                    obj.Code = Code;
                    obj.Region = Region;
                    obj.Remark = Value;
                    BSM.ListScore.Add(obj);
                }
                else
                {
                    checkexist.First().Code = Code;
                    checkexist.First().Remark = Value;
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
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["APPRTYPE_SELECT"] = DB.HREvaluateTypes.ToList();
        }
        #endregion
    }
}
