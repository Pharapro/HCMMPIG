using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.TrainingN.Process
{
    public class ESSEmployeeTrainingController : EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "TRA0000003";
        private const string URL_SCREEN = "/Training/Process/ESSEmployeeTraining/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo;Status";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        Core.DB.HumicaDBViewContext DBStaff = new Core.DB.HumicaDBViewContext();
        public ESSEmployeeTrainingController()
           : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region Index
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            BSM.FTTraining = new Core.FT.FTTraining();
            DateTime InYear = DateTime.Now;
            string Pending = SYDocumentStatus.PENDING.ToString();
            BSM.FTTraining.INYear = InYear.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                BSM.FTTraining = obj.FTTraining;
            }
            BSM.ListTrainee = await DBX.TRTrainingEmployees.Where(w => w.InYear == BSM.FTTraining.INYear && w.EmpCode == user.UserName && w.Status == "ACCEPTED").ToListAsync();
            BSM.ListPendingTrainee = await DBX.TRTrainingEmployees.Where(w => w.InYear == BSM.FTTraining.INYear && w.EmpCode == user.UserName && w.Status == Pending).ToListAsync();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(ESSTrainingInvitationObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            BSM.FTTraining = new Core.FT.FTTraining();
            BSM.FTTraining.INYear = collection.FTTraining.INYear;
            BSM.FTTraining.Course = collection.FTTraining.Course;
            string Pending = SYDocumentStatus.PENDING.ToString();
            var ListTrainee = DBX.TRTrainingEmployees.Where(x => x.InYear == BSM.FTTraining.INYear && x.IsAccept == true && x.EmpCode == user.UserName).ToList();
            if (!string.IsNullOrEmpty(BSM.FTTraining.Course))
            {
                ListTrainee = ListTrainee.Where(x => x.CourseID == BSM.FTTraining.Course).ToList();
            }
            BSM.ListTrainee = ListTrainee;
            BSM.ListPendingTrainee = await DBX.TRTrainingEmployees.Where(w => w.InYear == BSM.FTTraining.INYear && w.EmpCode == user.UserName && w.Status == Pending).ToListAsync();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialTrainee()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialTrainee", BSM.ListTrainee);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListPendingTrainee);
        }
        #endregion
        #region Details
        public async Task<ActionResult> Details(string id)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            BSM.Agenda = new TRTrainingAgenda();
            if (id != null) {
                var ListStaffDept = DB.HRStaffProfiles.Where(w => w.HODCode == user.UserName).ToList();
                BSM.Header = await DBX.TRTrainingInvitations.FirstOrDefaultAsync(w => w.TrainNo.ToString() == id);

                if (BSM.Header != null)
                {
                    var ListTrainee = DBX.TRTrainingEmployees.Where(w => w.TrainNo == BSM.Header.TrainNo.ToString()).ToList();
                    var Agenda = DBX.TRTrainingAgendas.FirstOrDefault(w => w.CalendarID.ToString() == id);
                    BSM.ListTrainee = ListTrainee
                    .Where(w => ListStaffDept.Any(x => x.EmpCode == w.EmpCode))
                    .ToList();

                    BSM.HeaderTrainee = ListTrainee.Where(w => w.EmpCode == user.UserName).FirstOrDefault();
                    BSM.Agenda = Agenda;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Accept
        public async Task<ActionResult> Accept(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ActionName = "Index";
            ViewData[SYSConstant.PARAM_ID] = id;
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            if (id != null)
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string msg = await BSM.AcceptTheDoc(id, fileName);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_ACC", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Cancel"
        public async Task<ActionResult> Cancel(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ActionName = "Index";
            ViewData[SYSConstant.PARAM_ID] = id;
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            if (id != null)
            {
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string msg = await BSM.CancelInvit(id, fileName);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_ACC", user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        private async Task DataSelector()
        {
            ViewData["TRAINING_COURSE"] = await DBX.TRTrainingCourses.ToListAsync();
            ViewData["COURSE_CATEGORY"] = await DBX.TRCourseCategories.ToListAsync();
            ViewData["TRAINING_TYPE"] = await DBX.TRTrainingTypes.ToListAsync();
            ViewData["Group_List_ALL"] = await DBX.TRTrainingRequirements.Where(w => w.Category == "G").ToListAsync();
            ViewData["Requirement_List"] = await DBX.TRTrainingRequirements.Where(w => w.Category == "R").ToListAsync();
            ViewData["Venue_List"] = await DBX.TRTrainingRequirements.Where(w => w.Category == "V").ToListAsync();
        }
    }
}