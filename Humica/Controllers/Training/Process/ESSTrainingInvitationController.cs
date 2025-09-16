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
    public class ESSTrainingInvitationController : EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "TRA0000002";
        private const string URL_SCREEN = "/Training/Process/ESSTrainingInvitation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo;Status";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        HumicaDBContext DB = new HumicaDBContext();
        public SMSystemEntity DP = new SMSystemEntity();
        Core.DB.HumicaDBViewContext DBStaff = new Core.DB.HumicaDBViewContext();
        public ESSTrainingInvitationController()
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
            BSM.ListInviPending = new List<TRTrainingInvitation>();
            BSM.ListPending = new List<TRPendingDeptInvit>();
            BSM.ListApproved = new List<TRPendingDeptInvit>();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            string Approved = SYDocumentStatus.APPROVED.ToString();
            
            BSM.FTTraining = new Core.FT.FTTraining();
            DateTime InYear = DateTime.Now;
            BSM.FTTraining.INYear = InYear.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                BSM.FTTraining = obj.FTTraining;
            }
            var HeadOfDept = DB.HRStaffProfiles.Where(w => w.HODCode == user.UserName).FirstOrDefault();
            if (HeadOfDept != null)
            {
                BSM.ListApproved = await DBX.TRPendingDeptInvits.Where(w => w.InYear == BSM.FTTraining.INYear && w.Status == Approved && HeadOfDept.DEPT == w.Department).ToListAsync();
                BSM.ListPending = await DBX.TRPendingDeptInvits.Where(w => w.Status == PENDING && w.Department == HeadOfDept.DEPT).ToListAsync();
            }
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
            BSM.ListInviPending = new List<TRTrainingInvitation>();
            BSM.ListPending = new List<TRPendingDeptInvit>();
            BSM.ListApproved = new List<TRPendingDeptInvit>();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            string PENDING = SYDocumentStatus.PENDING.ToString();
            string Approved = SYDocumentStatus.APPROVED.ToString();
            
            BSM.FTTraining = new Core.FT.FTTraining();
            DateTime InYear = DateTime.Now;
            BSM.FTTraining.INYear = InYear.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                BSM.FTTraining = obj.FTTraining;
            }
            var HeadOfDept = DB.HRStaffProfiles.Where(w => w.HODCode == user.UserName).FirstOrDefault();
            if (HeadOfDept != null)
            {
                BSM.ListApproved = await DBX.TRPendingDeptInvits.Where(w => w.InYear == BSM.FTTraining.INYear && w.Status == Approved && HeadOfDept.DEPT == w.Department).ToListAsync();
                BSM.ListPending = await DBX.TRPendingDeptInvits.Where(w => w.Status == PENDING && w.Department == HeadOfDept.DEPT).ToListAsync();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
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
            return PartialView("PartialProcess", BSM.ListPending);
        }
        #endregion
        #region Details
        public async Task<ActionResult> Details(int id)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ActionName = "Details";
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            if (id != null) {
                var ListStaffDept = DB.HRStaffProfiles.Where(w => w.HODCode == user.UserName).ToList();
                BSM.Header = await DBX.TRTrainingInvitations.FirstOrDefaultAsync(w => w.TrainNo == id);

                if (BSM.Header != null)
                {
                    var ListTrainee = DBX.TRTrainingEmployees.Where(w => w.TrainNo == BSM.Header.TrainNo.ToString()).ToList();
                    BSM.ListTrainee = ListTrainee
                    .Where(w => ListStaffDept.Any(x => x.EmpCode == w.EmpCode))
                    .ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approve
        public ActionResult Approve(int id)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            ActionName = "Details";
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ESSTrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string msg = BSM.approveTheDoc(id, fileName);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Cancel"
        public ActionResult Cancel(int id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ESSTrainingInvitationObject BSM = new ESSTrainingInvitationObject();
            if (id.ToString() != "null")
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CancelTheDoc(id);

                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id.ToString();
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);

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