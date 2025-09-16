using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class DTrainingDocumentController : Humica.EF.Controllers.MasterSaleController
    {
        // GET: NServiceDemandApproval
        const string SCREENID = "TRD000002";
        private const string URL_SCREEN = "/Training/Process/DTrainingDocument/";
        private string status = SYDocumentStatus.PENDING.ToString();
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string KeyName = "ID";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        Humica.Core.DB.HumicaDBViewContext DBV = new Humica.Core.DB.HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public DTrainingDocumentController()
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
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DTrainingDocument");
            UserConfFormFitler();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }

            string APPROVED = ExtStatus.APPROVED.ToString();
            string released = ExtStatus.RELEASED.ToString();
            string started = ExtStatus.TRAINING.ToString();
            string finised = ExtStatus.FINISHED.ToString();

            BSM.ListModule = new List<TrainigModule>();
            var chst = DBV.HRStaffProfiles.Where(w => w.EmpCode == user.UserName).ToList();
            var md = new List<TrainigModule>();
            var dats = DateTime.Now;
            var trmodule = DBX.TrainigModule.ToList();
            var trpro = DBX.TrainingProgram.ToList();

            BSM.TrainingBoard = new List<TrainigModuleTemp>();
            if (chst.Count > 0)
            {
                string stid = chst.First().EmpCode.ToString();
                //var trapp = DBX.TrainingCourseItem.Where(w => EntityFunctions.TruncateTime(w.Startdate) <= EntityFunctions.TruncateTime(dats) && EntityFunctions.TruncateTime(w.Enddate) >= EntityFunctions.TruncateTime(dats) && (w.Status == released || w.Status == started) && w.StaffID == stid).ToList();
                var trapp = DBX.TRTrainingEmployees.Where(w => w.EmpCode == stid).ToList();
                md = trmodule.Where(w => trapp.Where(x => x.TrainingType == w.TrainingType && x.CourseID == w.CourseCode).Any()).OrderBy(w => w.DayTerm).ToList();
                var ab = md.Select(w => new { w.TrainingType, w.Description }).Distinct().ToList();
                var exam = DBX.TrainingExamHeader.Where(w => w.IsFinalTest == false && w.IsInitialTest == true && w.Type == 67 && w.StaffID == stid).ToList();
                BSM.ListModule = md.Where(w => exam.Where(x => w.TrainingType == x.TrainingType && w.CourseCode == x.Course && w.Topic == x.Topic).Any()).ToList();


            }

            //Training Board
            var trmaster = DBX.TRTrainingCourses.ToList();
            foreach (var r in md.OrderBy(w => w.DayTerm).ToList())
            {
                var obj = new TrainigModuleTemp();
                obj.Topic = r.Topic;
                obj.Description = r.Description;
                obj.TrainingType = r.TrainingType;
                obj.Coursecode = r.CourseCode;
                var check = trmaster.Where(w => w.Code == obj.Coursecode).ToList();
                if (check.Count > 0)
                {
                    obj.Coursename = check.First().Description;
                }
                obj.Timer1 = r.Timer;
                obj.Timer2 = r.Timer;
                obj.DayTerm = r.DayTerm;
                obj.Startdate = r.Startdate;
                obj.Enddate = r.Enddate;
                obj.ID = r.ID;
                BSM.TrainingBoard.Add(obj);
            }
            //BSM.ListCourse = DBP.TrainingCourses.Where(w=>w.Branch=="SV" && w.Status== released).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(TrainningObject collection)
        {
            UserSession();
            DataListSelector();
            this.ActionName = "Index";
            UserConfListAndForm(this.KeyName);
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Course = collection.Course;

            string APPROVED = ExtStatus.APPROVED.ToString();
            string released = ExtStatus.RELEASED.ToString();
            string started = ExtStatus.TRAINING.ToString();
            string finised = ExtStatus.FINISHED.ToString();

            BSM.ListModule = new List<TrainigModule>();

            var md = new List<TrainigModule>();
            var dats = DateTime.Now;
            var trmodule = DBX.TrainigModule.Where(w => dats >= EntityFunctions.TruncateTime(w.Startdate) && dats <= EntityFunctions.TruncateTime(w.Enddate)).ToList();
            var trpro = DBX.TrainingProgram.ToList();
            var chst = DBV.HRStaffProfiles.Where(w => w.EmpCode == user.UserName).ToList();
            if (chst.Count > 0)
            {
                string stid = chst.First().EmpCode.ToString();
                var trapp = DBX.TrainingCourseItem.Where(w => EntityFunctions.TruncateTime(w.Startdate) <= EntityFunctions.TruncateTime(dats) && EntityFunctions.TruncateTime(w.Enddate) >= EntityFunctions.TruncateTime(dats) && (w.Status == released || w.Status == started) && w.StaffID == stid).ToList();
                md = trmodule.Where(w => trapp.Where(x => x.Program == w.TrainingType && x.Course == w.CourseCode).Any()).ToList();
                var ab = md.Select(w => new { w.TrainingType, w.Description }).Distinct().ToList();
                var exam = DBX.TrainingExamHeader.Where(w => w.IsFinalTest == false && w.IsInitialTest == true && w.Type == 67 && w.StaffID == stid).ToList();
                BSM.ListModule = md.Where(w => exam.Where(x => w.TrainingType == x.TrainingType && w.CourseCode == x.Course && w.Topic == x.Topic).Any()).ToList();
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DTrainingDocument");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListModule);
        }
        public ActionResult TrainingBoard()
        {
            this.ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DTrainingDocument");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("TrainingBoard", BSM.TrainingBoard);
        }
        #endregion

        #region Detials
        public ActionResult Details(string id, string dlr)
        {
            ActionName = "Details";
            this.UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "DTrainingPrograme");
            DataListSelector();
            TrainningObject BSD = new TrainningObject();
            string OPEN = SYDocumentStatus.OPEN.ToString();
            BSD.RequestHeader = DBX.TrainingRequestHeader.SingleOrDefault(w => w.RequestCode == id);
            if (BSD.RequestHeader != null)
            {
                BSD.ListRequestItem = DBX.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode).ToList();
                ViewData[SYConstant.PARAM_ID] = id;
                var Mont = DateTime.Now.Month;
                Session[Index_Sess_Obj + ActionName] = BSD;
                return View(BSD);
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
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
        public ActionResult District()
        {
            UserSession();
            if (Session["Program"] != null)
            {
                string Program = Session["Program"].ToString();
                var District = DBX.TRTrainingInvitations.Where(w => w.CourseID == Program).ToList();
                ViewData["Course_List"] = District.ToList();
                return PartialView("District");
            }
            return null;
        }

        #region "Private Code"

        private void DataListSelector()
        {
            //SYDataList DL = new SYDataList("YEAR_SELECT");
            //ViewData["YEAR_SELECT"] = DL.ListData;

            //DL = new SYDataList("STATUS_APPROVAL");
            //ViewData["STATUS_APPROVAL"] = DL.ListData;

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
            //ViewData["Course_List"] = DBX.TrainingCourse.ToList();
            //ViewData["Program_List"] = DBX.TrainingProgram.ToList();
            //ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();
            //var ListReqData = DBX.TRTrainingRequirement.ToList();
            //ViewData["Target_List"] = ListReqData.Where(w => w.Category == "T").ToList();
            //ViewData["Venue_List"] = ListReqData.Where(w => w.Category == "V").ToList();
            //ViewData["Dayterm_List"] = ListReqData.Where(w => w.Category == "D").ToList();
            //ViewData["Group_List"] = ListReqData.Where(w => w.Category == "G").ToList();
            //ViewData["Requirement_List"] = ListReqData.Where(w => w.Category == "R").ToList();
            //ViewData["Staff_List"] = DBV.HR_STAFF_VIEW.ToList();


            ViewData["Course_List"] = DBX.TRTrainingCourses.ToList();
            ViewData["Training_List"] = DBX.TRTrainingTypes.ToList();

        }
        #endregion
    }
}