using DevExpress.DataProcessing;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using HUMICA.Models.Report.Appraisal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HREvaluationFormController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000005";
        private const string URL_SCREEN = "/HRM/Appraisal/HREvaluationForm/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EvaluateID;Status";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public HREvaluationFormController()
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
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HREmpEvaluates.ToList();
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
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create(string EmpCode, string AppType)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.Header = new HREmpEvaluate();
            BSM.ListFactor = new List<HREvaluateFactor>();
            BSM.ListEvaluateRating = new List<HREvaluateRating>();
            BSM.ListRegion = new List<HREvaluateRegion>();
            BSM.Header.EvaluateDate = DateTime.Now;
            BSM.Header.EvalFromDate = DateTime.Now;
            BSM.Header.EvalToDate = DateTime.Now;
            BSM.Header.EvaluateType = AppType;
            var Staff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Staff != null)
            {
                BSM.Header.EmpCode = EmpCode;
                BSM.Header.EmpName = Staff.AllName;
                BSM.Header.Position = Staff.Position;
            }
            BSM.ListRegion = DB.HREvaluateRegions.Where(w => w.EvaluateType == AppType).ToList();
            if (BSM.ListRegion.Count() > 0)
            {
                var quiz = DB.HREvaluateFactors.ToList();
                var Rating = DB.HREvaluateRatings.ToList();
                BSM.ListFactor = quiz.Where(w => BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                BSM.ListEvaluateRating = Rating.Where(w => BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            BSM.ListScore = new List<HREmpEvaluateScore>();
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(HREmpEvaluateObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;
                string msg = BSM.CreateAppr();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = collection.Header.EvaluateID.ToString();
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber;
                    BSM.ListScore = new List<HREmpEvaluateScore>();
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
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
                HREmpEvaluateObject BSM = new HREmpEvaluateObject();

                BSM.Header = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == ID);
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
        [HttpPost]

        public ActionResult Edit(string id, HREmpEvaluateObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditAppr(id);
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
            ViewData[ClsConstant.IS_READ_ONLY] = true;

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
        #region "Print"
        public ActionResult Print(string id)
        {
            ActionName = "Print";
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptEmpEvaluate();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["EvaluateID"].Value = SD.EvaluateID;
                    sa.Parameters["EvaluateID"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    //log.DocurmentAction = ;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";

            if (Session[Index_Sess_Obj] != null)
            {
                RptEmpEvaluate reportModel = (RptEmpEvaluate)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region 'Status'
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    //var Objmatch = DB.HR_PO_VIEW.FirstOrDefault(w => w.PONumber == BSM.Header.PONumber);

                    ///*------------------WorkFlow--------------------------------*/
                    //var excfObject = DH.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.Header.DocumentType);
                    //string wfMsg = "";
                    //if (excfObject != null)
                    //{
                    //    Humica.Core.SY.SYWorkFlowEmailObject wfo =
                    //        new Humica.Core.SY.SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                    //             UserType.N, BSM.Header.Status);
                    //    wfo.SelectListItem = new SYSplitItem(id);
                    //    wfo.User = BSM.User;
                    //    wfo.BS = BSM.BS;
                    //    wfo.ScreenId = SCREEN_ID;
                    //    wfo.Module = "HR";// CModule.PURCHASE.ToString();
                    //    wfo.ListLineRef = new List<BSWorkAssign>();
                    //    wfo.ListObjectDictionary = new List<object>();
                    //    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    //    HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.PONumber, "");
                    //    if (Staff.Email != null && Staff.Email != "")
                    //    {
                    //        wfo.ListObjectDictionary.Add(Staff);
                    //        wfo.ListObjectDictionary.Add(Objmatch);
                    //        WorkFlowResult result = wfo.InsertProcessWorkFlow(Staff);
                    //        wfMsg = wfo.getErrorMessage(result);
                    //    }
                    //}
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;// + wfMsg;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Cancel(string id)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CancelDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("CANCELLED", user.Lang);
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
        #region 'PrivateCode'
        public ActionResult ShowDataEmp(string EmpCode, string Action)
        {

            ActionName = Action;
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    POST = EmpStaff.Position,
                    NAME = EmpStaff.AllName
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
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