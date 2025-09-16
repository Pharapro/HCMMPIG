using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Humica.EF;
using Humica.Models.SY;
using DevExpress.DataProcessing;
using System.Data;
using Humica.Core.DB;
using Humica.Training;
using Humica.Core.FT;
using Humica.Training.DB;

namespace Humica.Controllers.MyTeam
{
    public class ESSMTTrainingSurveyController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000025";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSMTTrainingSurvey/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "SurveyID";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        HumicaDBContextTR DBD = new HumicaDBContextTR();

        public ESSMTTrainingSurveyController()
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
            DataSelector();
            UserConfListAndForm(this.KeyName);
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.filter = new FTDGeneralAccount();
            //BSM.ListEmpStaff = new List<TRTrainingEmployee>();
            BSM.ListHeader = DBD.TRTrainingSurveys.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(TrainingSurveyObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.ListHeader = DBD.TRTrainingSurveys.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
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
                TrainingSurveyObject BSM = new TrainingSurveyObject();
                BSM.Header = DBD.TRTrainingSurveys.FirstOrDefault(w => w.SurveyID == ID);
                if (BSM.Header != null)
                {
                    BSM.ListSurveyRating = new List<TRSurveyRating>();
                    BSM.ListRegion = DBD.TRSurveyRegions.ToList();
                    if (BSM.ListRegion.Count() > 0)
                    {
                        var quiz = DBD.TRSurveyFactors.ToList();
                        var Rating = DBD.TRSurveyRatings.ToList();

                        var Applicant = DBD.TREmpSurveyScores.Where(w => w.SurveyID == BSM.Header.SurveyID).ToList();

                        BSM.ListScore = Applicant.ToList();
                        BSM.ListFactor = quiz.Where(w => BSM.ListRegion.Where(x => x.Code == w.Region).Any()).ToList();
                        BSM.ListSurveyRating = Rating.ToList();

                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]

        public ActionResult Edit(string id, TrainingSurveyObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];
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
        #region Details
        public ActionResult Details(string id)
        {

            ActionName = "Details";
            UserSession();
            DataSelector();
            //UserConfListAndForm();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            BSM.Header = new TRTrainingSurvey();
            BSM.ListFactor = new List<TRSurveyFactor>();
            BSM.ListScore = new List<TREmpSurveyScore>();
            BSM.ListRegion = new List<TRSurveyRegion>();
            BSM.ListSurveyRating = new List<TRSurveyRating>();
            BSM.Header = DBD.TRTrainingSurveys.FirstOrDefault(w => w.SurveyID == id);
            if (BSM.Header != null)
            {
                var _Interview = DBD.TRTrainingSurveys.FirstOrDefault(w => w.SurveyID == id);
                if (_Interview != null)
                {
                    BSM.Header = _Interview;
                    BSM.ListRegion = DBD.TRSurveyRegions.ToList();
                    var Applicant = DBD.TREmpSurveyScores.Where(w => w.SurveyID == BSM.Header.SurveyID).ToList();
                    var EmpRating = DBD.TRSurveyRatings.ToList();
                    BSM.ListSurveyRating = EmpRating.ToList();
                    if (Applicant.Count() > 0)
                    {
                        BSM.ListScore = Applicant.ToList();
                    }
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Details
        #region 'PrivateCode'
        public ActionResult ShowDataEmp(string EmpCode, string Action)
        {

            ActionName = Action;
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];
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
            ActionName = Action;
            UserSession();
            UserConfListAndForm();
            TrainingSurveyObject BSM = new TrainingSurveyObject();
            BSM.ListScore = new List<TREmpSurveyScore>();
            var Result = "";
            if (Value != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];

                    string[] sp = Value.Split('_');
                    string Code = sp[0].ToString();
                    int EvID = Convert.ToInt32(sp[1]);
                    int Score = Convert.ToInt32(sp[2]);
                    var checkexist = BSM.ListScore.Where(w => w.Code == Code).ToList();
                    if (checkexist.Count == 0)
                    {
                        var obj = new TREmpSurveyScore();
                        obj.Code = Code;
                        obj.Region = Region;
                        obj.Score = Score;
                        BSM.ListScore.Add(obj);
                    }
                    else
                    {
                        checkexist.First().Code = Code;
                        checkexist.First().Score = Score;
                    }
                    var Resul = BSM.ListScore.ToList().Sum(w => w.Score);
                    var result = new
                    {
                        MS = SYConstant.OK,
                        Total = Resul.Value
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult CommentValue(string Code, string Value, string Region, string Action)
        {
            ActionName = Action;
            UserSession();
            UserConfListAndForm();

            TrainingSurveyObject BSM = new TrainingSurveyObject();
            BSM.ListScore = new List<TREmpSurveyScore>();
            if (Value != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingSurveyObject)Session[Index_Sess_Obj + ActionName];
                    var checkexist = BSM.ListScore.Where(w => w.Code == Code).ToList();
                    if (checkexist.Count == 0)
                    {
                        var obj = new TREmpSurveyScore();
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
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var sr = DBD.TRTrainingSurveys.ToList();
            var EmpAtt = DBD.TRTrainingAttendances.Where(w => w.IsAttend == true).ToList();
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.AsEnumerable().Where(w => EmpAtt.Where(x => x.EmpCode == w.EmpCode && !sr.Where(y => y.AssignedTo == x.EmpCode).Any()).Any()).ToList();
            ViewData["IVITATION_SELECT"] = DBD.TRTrainingInvitations.ToList();
        }
        #endregion
    }
}


