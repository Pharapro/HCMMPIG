using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{

    public class DDoExamController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TRD000003";
        const string DOC_TYPE = "NSV03";
        private const string URL_SCREEN = "/Training/Process/DDoExam/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public DDoExamController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region LIST
        public ActionResult Index()
        {
            UserSession();
            DataListSelector();
            this.ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DDoExam");
            UserConfFormFitler();
            string APPROVED = ExtStatus.APPROVED.ToString();
            string released = ExtStatus.RELEASED.ToString();
            string training = ExtStatus.TRAINING.ToString();

            BSM.ListModuleTemp = new List<TrainigModuleTemp>();
            BSM.ListExamHeader = new List<TrainingExamHeader>();
            var ListStaff = DB.HRStaffProfiles.Where(w => w.EmpCode == user.UserName).ToList();
            var dats = DateTime.Now;
            var trmodule = DBX.TrainigModule.ToList();
            var md = new List<TrainigModule>();

            if (ListStaff.Count() > 0)
            {
                string stid = ListStaff.FirstOrDefault().EmpCode;
                //var trapp = DBX.TrainingCourseItem.Where(w => (w.Status == released || w.Status == training) && w.StaffID == stid && EntityFunctions.TruncateTime(w.Startdate) <= EntityFunctions.TruncateTime(dats) && EntityFunctions.TruncateTime(w.Enddate) >= EntityFunctions.TruncateTime(dats)).ToList();
                var trapp = DBX.TRTrainingEmployees.Where(w => w.EmpCode == stid).ToList();
                foreach (var r in trapp.ToList())
                {
                    foreach (var b in trmodule.Where(w => w.TrainingType == r.TrainingType && w.CourseCode == r.CourseID).OrderBy(w => w.DayTerm).ToList())
                    {
                        var obj = new TrainigModuleTemp();
                        //obj.Invcode = r.InvCode;
                        obj.Topic = b.Topic;
                        obj.Description = b.Description;
                        obj.TrainingType = b.TrainingType;
                        obj.Coursecode = b.CourseCode;
                        obj.DayTerm = b.DayTerm;
                        //obj.Startdate = r.Startdate;
                        //obj.Enddate = r.Enddate;
                        //obj.ID = r.ID;

                        //var exam = DBX.TrainingExamHeader.Where(w => w.IsFinalTest == true && w.IsInitialTest == true && w.Type == 67 && w.InvCode == r.InvCode && w.StaffID == stid).ToList();
                        var exam = DBX.TrainingExamHeader.Where(w => w.IsFinalTest == true && w.IsInitialTest == true && w.Type == 67 && w.StaffID == stid).ToList();
                        var ch = exam.Where(w => w.TrainingType == obj.TrainingType && w.Course == obj.Coursecode && w.Topic == obj.Topic && w.InvCode == obj.Invcode).ToList();
                        if (ch.Count == 0)
                        {
                            BSM.ListModuleTemp.Add(obj);
                        }

                    }
                    //if (r.FirstAttance == null)
                    //{
                    //    r.FirstAttance = DateTime.Now;
                    //}
                    //r.LastAttance = DateTime.Now;
                    //DBX.TrainingCourseItem.Attach(r);
                    //DBX.Entry(r).Property(w => w.FirstAttance).IsModified = true;
                    //DBX.Entry(r).Property(w => w.LastAttance).IsModified = true;
                }


                BSM.ListExamHeader = DBX.TrainingExamHeader.Where(w => w.StaffID == stid).ToList();
            }
            //BSM.ListCourse = DBX.TrainingCourse.Where(w => w.Branch == "SV" && w.Status == released).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session["ISFORCE"] = null;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DDoExam");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListExamHeader);
        }
        public ActionResult GridCourse()
        {
            this.ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DDoExam");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridCourse", BSM.ListModuleTemp);
        }
        #endregion


        #region Create
        public ActionResult Create(string CourseCode, string Program, string Module, string InvCode)
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSD = new TrainningObject();
            BSD.ExamHeader = new TrainingExamHeader();
            BSD.ListQuestion = new List<TRELearningQuestion>();
            BSD.ListAnswer = new List<TRELearningAnswer>();
            BSD.ExamHeader.ExamDate = DateTime.Now;
            BSD.ExamHeader.Course = CourseCode;
            BSD.ExamHeader.Topic = Module;
            BSD.ExamHeader.TrainingType = Program;
            BSD.ExamHeader.InvCode = InvCode;
            BSD.ExamHeader.IsInitialTest = true;
            BSD.ExamHeader.Result = ExtStatus.OPEN.ToString();
            var ans = DBX.TRELearningAnswer.Where(w => w.TrainingType == Program && w.CourseCode == CourseCode && w.Topic == Module).ToList();
            var quiz = DBX.TRELearningQuestion.Where(w => w.TrainingType == Program && w.CourseCode == CourseCode && w.Topic == Module).ToList();
            if (quiz.Count > 0)
            {
                BSD.ListQuestion = quiz;
                BSD.ListAnswer = ans.Where(w => quiz.Where(x => w.QuestionCode == x.QuestionCode).Any()).ToList();
            }
            var trmodule = DBX.TrainigModule.ToList();
            var md = DBX.TrainigModule.FirstOrDefault(w => w.TrainingType == Program && w.CourseCode == CourseCode && w.Topic == Module);
            //string checkmodule = "";
            if (md != null)
            {
                BSD.Timer = (int)md.Timer;
            }
            if (BSD.ListAnswer.Count == 0 || BSD.ListQuestion.Count == 0)
            {
                SYMessages mess_err = SYMessages.getMessageObject("ANSWER_QUIZ_EMPTY", user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = BSD;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(TrainningObject Collection, string CourseCode, string Program, string Module, string InvCode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataListSelector();
            TrainningObject BSD = new TrainningObject();
            BSD = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            BSD.ListRequestHeader = Collection.ListRequestHeader;
            BSD.AnswerSelected = Collection.AnswerSelected;
            if (ModelState.IsValid)
            {
                BSD.ScreenId = SCREENID;
                string msg = BSD.SAVEExam(BSD.AnswerSelected, InvCode);
                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSD;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSD);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSD.ExamHeader.ExamCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + BSD.ExamHeader.ExamCode;
                Session[Index_Sess_Obj + this.ActionName] = BSD;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(mess.NavigationUrl);

            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSD);
        }

        public ActionResult GridExamItems()
        {
            this.ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DDoExam");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridExamItems", BSM.ListAnswer);
        }
        #endregion

        #region Detials
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            this.UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DDoExam");
            DataListSelector();
            TrainningObject BSD = new TrainningObject();
            string OPEN = SYDocumentStatus.OPEN.ToString();


            var dats = DateTime.Now;
            var trmodule = DBX.TrainigModule.ToList();
            var md = new List<TrainigModule>();

            BSD.ExamHeader = DBX.TrainingExamHeader.FirstOrDefault(w => w.ExamCode == id && w.Type == 67);
            if (BSD.ExamHeader != null)
            {
                var ans = DBX.TRELearningAnswer.ToList();
                var quiz = DBX.TRELearningQuestion.Where(w => w.TrainingType == BSD.ExamHeader.TrainingType && w.CourseCode == BSD.ExamHeader.Course && w.Topic == BSD.ExamHeader.Topic).ToList();
                if (quiz.Count > 0)
                {
                    BSD.ListQuestion = quiz;
                    BSD.ListAnswer = ans.Where(w => quiz.Where(x => x.TrainingType == w.TrainingType && x.CourseCode == w.CourseCode && x.Topic == w.Topic && w.QuestionCode == x.QuestionCode).Any()).ToList();
                }

                BSD.ListExamItem = DBX.TrainingExamItem.Where(w => w.ExamCode == BSD.ExamHeader.ExamCode).ToList();
                ViewData[SYConstant.PARAM_ID] = id;
                var Mont = DateTime.Now.Month;


                BSD.IsFN = false;
                var lstCourse = DBX.TRTrainingInvitations.FirstOrDefault(w => w.CalendarID == Convert.ToInt32(BSD.ExamHeader.InvCode) && w.TrainingTypeID == BSD.ExamHeader.TrainingType && w.CourseID == BSD.ExamHeader.Course);
                var lstModule = DBX.TrainigModule.Where(w => w.TrainingType == BSD.ExamHeader.TrainingType && w.CourseCode == BSD.ExamHeader.Course).ToList();
                if (lstCourse != null)
                {
                    var checkexam = DBX.TrainingExamHeader.Where(w => w.TrainingType == BSD.ExamHeader.TrainingType && w.Course == BSD.ExamHeader.Course && w.IsInitialTest == true && w.IsFinalTest == true && w.Type == 67).ToList();
                    if (lstModule.Count > 0 && lstModule.Count == checkexam.Count)
                    {
                        //Result Initial
                        string pass = ExtStatus.PASS.ToString();
                        string fail = ExtStatus.FAILURED.ToString();
                        var passmodule = DBX.TrainingExamHeader.Where(w => w.Result == pass && w.TrainingType == BSD.ExamHeader.TrainingType && w.Course == BSD.ExamHeader.Course && w.IsInitialTest == true && w.IsFinalTest == false && w.Type == 67).ToList();
                        var failmodule = DBX.TrainingExamHeader.Where(w => w.Result == fail && w.TrainingType == BSD.ExamHeader.TrainingType && w.Course == BSD.ExamHeader.Course && w.IsInitialTest == true && w.IsFinalTest == false && w.Type == 67).ToList();
                        BSD.TotalModuleInitial = lstModule.Count;
                        BSD.PassModuleInitial = passmodule.Count;
                        BSD.FailModuleInitial = failmodule.Count;
                        BSD.StatusInitial = "";
                        decimal? pc = 0;
                        //decimal sumscore = passmodule.Sum(w => w.TotalScore);
                        pc = (decimal)BSD.PassModuleInitial / (decimal)BSD.TotalModuleInitial;
                        BSD.AcheiveInitial = (decimal)Math.Round((double)pc, 2) * 100;
                        if (BSD.AcheiveInitial >= lstCourse.Target && failmodule.Count == 0)
                        {
                            BSD.StatusInitial = pass;
                        }
                        else
                        {
                            BSD.StatusInitial = fail;
                        }

                        //Result Final
                        var passmodule1 = DBX.TrainingExamHeader.Where(w => w.Result == pass && w.TrainingType == BSD.ExamHeader.TrainingType && w.Course == BSD.ExamHeader.Course && w.IsInitialTest == true && w.IsFinalTest == true && w.Type == 67).ToList();
                        var failmodule1 = DBX.TrainingExamHeader.Where(w => w.Result == fail && w.TrainingType == BSD.ExamHeader.TrainingType && w.Course == BSD.ExamHeader.Course && w.IsInitialTest == true && w.IsFinalTest == true && w.Type == 67).ToList();
                        BSD.TotalModuleFinal = lstModule.Count;
                        BSD.PassModuleFinal = passmodule1.Count;
                        BSD.FailModuleFinal = failmodule1.Count;
                        BSD.StatusFinal = "";
                        decimal pc1 = 0;
                        //decimal sumscore1 = passmodule1.Sum(w => w.TotalScore);
                        pc1 = (decimal)BSD.PassModuleFinal / (decimal)BSD.TotalModuleFinal;
                        BSD.AcheiveFinal = (decimal)Math.Round((double)pc1, 2) * 100;
                        if (BSD.AcheiveFinal >= lstCourse.Target && failmodule1.Count == 0)
                        {
                            BSD.StatusFinal = pass;
                        }
                        else
                        {
                            BSD.StatusFinal = fail;
                        }
                        BSD.IsFN = true;
                    }
                }

                Session[Index_Sess_Obj + ActionName] = BSD;
                return View(BSD);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Details";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            //BSM.ListItems = new List<SPPOItem>();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemDetails", BSM.ListRequestItem);
        }
        #endregion

        [HttpPost]
        public string FitlerType(string code, int addType)
        {

            UserSession();
            //UserConfListAndForm();
            if (addType == 1)
            {
                Session["Program"] = code;
            }
            else if (addType == 2)
            {
                Session["Course"] = code;
            }

            return SYConstant.OK;
        }
        //public ActionResult District()
        //{
        //    UserSession();
        //    //UserConfListAndForm();
        //    if (Session["Program"] != null)
        //    {
        //        string Program = Session["Program"].ToString();
        //        var District = DBX.TrainingCourses.Where(w => w.ProgramCode == Program && w.Branch == "SV").ToList();
        //        ViewData["Course_List"] = District.ToList();
        //        return PartialView("District");
        //    }
        //    return null;
        //}


        #region "Private Code"
        private void DataListSelector()
        {
            //SYDataList DL = new SYDataList("YEAR_SELECT");
            //ViewData["YEAR_SELECT"] = DL.ListData;

            //DL = new SYDataList("STATUS_APPROVAL");
            //ViewData["STATUS_APPROVAL"] = DL.ListData;

            ////ViewData["DEALER_LIST"] = MDDMaterialObject.DealerList().ToList();
            //DL = new SYDataList("QT_RELEASE_TYPE");
            //ViewData["QT_RELEASE_TYPE"] = DL.ListData;
            //DL = new SYDataList("CALENDAR_TYPE");
            //ViewData["CALENDAR_TYPE"] = DL.ListData;
            //DL = new SYDataList("MONTH_SELECT");
            //ViewData["MONTH_SELECT"] = DL.ListData;
            //DL = new SYDataList("STATUS_APPROVAL");
            //ViewData["STATUS_APPROVAL"] = DL.ListData;
            //DL = new SYDataList("REPORT_TYPE");
            //ViewData["REPORT_TYPE"] = DL.ListData;
            //DL = new SYDataList("KI");
            //ViewData["YEAR_KI_SELECT"] = DL.ListData;
            //DL = new SYDataList("KI_QUATER");
            //ViewData["QUATER_KI_SELECT"] = DL.ListData;

            //ViewData["REQUEST_SELECT"] = new SYDataList("REQUEST_SELECT").ListData;

            //var listReason = DB.CFReasonCodes.Where(w => w.Indicator == "H").ToList();
            //foreach (var read in listReason)
            //{
            //    read.Description1 = read.Description2 + "-" + read.Description1;
            //}
            //ViewData["REASON_CANCEL"] = listReason;
            ViewData["Course_List"] = DBX.TRTrainingCourses.ToList();
            ViewData["Training_List"] = DBX.TRTrainingTypes.ToList();
            //ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();
            //var ListReqData = DBX.TRTrainingRequirement.ToList();
            //ViewData["Target_List"] = ListReqData.Where(w => w.Category == "T").ToList();
            //ViewData["Venue_List"] = ListReqData.Where(w => w.Category == "V").ToList();
            //ViewData["Dayterm_List"] = ListReqData.Where(w => w.Category == "D").ToList();
            //ViewData["Group_List"] = ListReqData.Where(w => w.Category == "G").ToList();
            //ViewData["Requirement_List"] = ListReqData.Where(w => w.Category == "R").ToList();
            ViewData["Staff_List"] = DB.HRStaffProfiles.ToList();
            ViewData["Module_List"] = DBX.TrainigModule.ToList();


        }
        #endregion


    }
}