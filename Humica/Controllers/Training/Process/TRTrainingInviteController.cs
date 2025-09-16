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
    public class TRTrainingInviteController : EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "TR00000008";
        private const string URL_SCREEN = "/Training/Process/TRTrainingInvite/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo;Status";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SMSystemEntity DP = new SMSystemEntity();
        Core.DB.HumicaDBViewContext DBStaff = new Core.DB.HumicaDBViewContext();
        public TRTrainingInviteController()
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
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.ListCalender = new List<TRTrainingCalender>();
            string Status = SYDocumentStatus.RELEASED.ToString();
            BSM.ListCalender = await DBX.TRTrainingCalenders.Where(w => w.Status == Status).ToListAsync();
            BSM.FTTraining = new Core.FT.FTTraining();
            var Agenda = await DBX.TRTrainingAgendas.ToListAsync();
            var ListCalender = await DBX.TRTrainingCalenders.Where(w => w.Status == Status && (string.IsNullOrEmpty(w.Restatus) || w.Restatus != "Created")).ToListAsync();
            ListCalender = ListCalender.Where(w => Agenda.Any(x => w.TrainNo == x.CalendarID)).ToList();
            BSM.ListCalender = ListCalender;

            DateTime InYear = DateTime.Now;
            BSM.FTTraining.INYear = InYear.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                BSM.FTTraining = obj.FTTraining;
            }
            //BSM.ListTrainee = await DBX.TRTrainingEmployees.Where(w => w.InYear == BSM.FTTraining.INYear).ToListAsync();
            BSM.ListInvitation = await DBX.TRTrainingInvitations.Where(w => w.InYear == BSM.FTTraining.INYear).ToListAsync();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(TrainingInvitationObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.ListCalender = new List<TRTrainingCalender>();
            string Status = SYDocumentStatus.RELEASED.ToString();
            BSM.ListCalender = await DBX.TRTrainingCalenders.Where(w => w.Status == Status).ToListAsync();
            BSM.FTTraining = new Humica.Core.FT.FTTraining();
            BSM.FTTraining.INYear = collection.FTTraining.INYear;
            BSM.FTTraining.Course = collection.FTTraining.Course;
            BSM.ListInvitation = await DBX.TRTrainingInvitations.Where(w => w.InYear == BSM.FTTraining.INYear).ToListAsync();
            if (!string.IsNullOrEmpty(BSM.FTTraining.Course))
            {
                BSM.ListInvitation = BSM.ListInvitation.Where(x => x.CourseID == BSM.FTTraining.Course).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialTrainee()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainingInvitationObject BSM = new TrainingInvitationObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialTrainee", BSM.ListInvitation);
        }

        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainingInvitationObject BSM = new TrainingInvitationObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListCalender);
        }
        #endregion

        #region Create
        public async Task<ActionResult> Create(int id)
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfListAndForm(this.KeyName);
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.HeaderCalender = new TRTrainingCalender();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            BSM.ListPendingDeptInvit = new List<TRPendingDeptInvit>();
            await BSM.GetCalender(id);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(TrainingInvitationObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingInvitationObject BSM = new TrainingInvitationObject();

            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingInvitationObject;
                BSM.Header = collection.Header;
            }
            string msg = BSM.CreateTrainee();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.Header.TrainNo.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(int id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            BSM.ListEmpCode = new List<string>();
            BSM.Header = await DBX.TRTrainingInvitations.FirstOrDefaultAsync(w => w.TrainNo == id);
            if (BSM.Header != null)
            {
                var ListTrainee = await DBX.TRTrainingEmployees.Where(w => w.CalendarID == BSM.Header.CalendarID).ToListAsync();
                BSM.ListTrainee = ListTrainee.Where(w => w.TrainNo == BSM.Header.TrainNo.ToString()).ToList();
                ListTrainee.ToList().ForEach(w => BSM.ListEmpCode.Add(w.EmpCode));
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, TrainingInvitationObject collection)
        {
            try
            {
                ActionName = "Create";
                UserSession();
                UserConfListAndForm(this.KeyName);
                await DataSelector();
                ViewData[SYSConstant.PARAM_ID1] = false;
                TrainingInvitationObject BSM = new TrainingInvitationObject();
                if (id != null)
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
                        BSM.Header = collection.Header;
                    }
                    BSM.ScreenId = SCREEN_ID;
                    string msg = BSM.EditTrainee(id);
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = id.ToString();
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
            catch
            {
                return View();
            }
        }
        #endregion 

        #region Details
        public async Task<ActionResult> Details(int id)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.ListTrainee = new List<TRTrainingEmployee>();
            BSM.Header = await DBX.TRTrainingInvitations.FirstOrDefaultAsync(w => w.TrainNo == id);
            if (BSM.Header != null)
            {
                BSM.ListTrainee = await DBX.TRTrainingEmployees.Where(w => w.TrainNo == BSM.Header.TrainNo.ToString()).ToListAsync();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        public ActionResult RequestForApproval(int id)
        {
            UserSession();
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string URL = SYUrl.getBaseUrl() + "/Training/Process/ESSEmployeeTraining/Details/";
                string msg = BSM.requestToApprove(id, fileName, URL);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public ActionResult Approve(int id)
        {
            UserSession();
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (id != null)
            {

                BSM.ScreenId = SCREEN_ID;
                string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");
                string msg = BSM.approveTheDoc(id,fileName);
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
        public ActionResult ReleaseDoc(int id)
        {
            UserSession();
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ReleaseTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (id.ToString() != "null")
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CancelTheDoc(id);

                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
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

        #region trainee(s)
        public ActionResult PartialEmployeeSearch()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialEmployeeSearch", BSM);

        }
        public async Task<ActionResult> gvemployeeselector()
        {
            ActionName = "Create";
            UserSession();
            await DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            else
            {
                BSM.ListStaff = new List<Core.DB.HRStaffProfile>();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";

            await BSM.GetAllStaff();
            return PartialView("gvemployeeselector", BSM);

        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingInvitationObject BSM = new TrainingInvitationObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingInvitationObject;
                if (BSM.ListTrainee != null)
                {
                    BSM.ListTrainee = BSM.ListTrainee.OrderBy(x => x.EmpCode).ToList();
                }
            }

            return PartialView("GridItems", BSM);
        }
        [HttpPost]
        public ActionResult DeleteTrainee(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingInvitationObject BSM = new TrainingInvitationObject();

            if (LineItem != 0)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingInvitationObject;
                    if (BSM.ListTrainee.Count > 0)
                    {
                        var objCheck = BSM.ListTrainee.FirstOrDefault(w => w.LineItem == LineItem);
                        BSM.ListTrainee.Remove(objCheck);
                        BSM.ListTrainee.OrderBy(x => x.EmpCode);
                        BSM.ListEmpCode.Remove(objCheck.EmpCode);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
        }
        public async Task<ActionResult> SelectedTrainee(string EmpCode)
        {

            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingInvitationObject BSM = new TrainingInvitationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
            }
            if (EmpCode != null)
            {
                string[] empCodes = EmpCode.Split(',');
                empCodes.ToList().ForEach(x => BSM.ListEmpCode.Add(x));
                BSM.ListEmpCode = BSM.ListEmpCode.Distinct().ToList();
                var staff = await DBStaff.HRStaffProfiles.ToListAsync();
                List<Core.DB.HRDepartment> ListDepartment = await DBStaff.HRDepartments.ToListAsync();
                List<Core.DB.HRPosition> ListPosition = await DBStaff.HRPositions.ToListAsync();
                var list = staff.Where(w => empCodes.Contains(w.EmpCode));
                int i = BSM.ListTrainee.Count();
                foreach (var item in list)
                {
                    if (!BSM.ListTrainee.Where(w => w.EmpCode == item.EmpCode).Any())
                    {
                        i++;
                        string Department = "";
                        string Position = "";

                        if (!string.IsNullOrEmpty(item.DEPT))
                        {
                            var Dept = ListDepartment.FirstOrDefault(w => w.Code == item.DEPT);
                            if (Dept != null) Department = Dept.Description;
                        }
                        if (!string.IsNullOrEmpty(item.JobCode))
                        {
                            var Post = ListPosition.FirstOrDefault(w => w.Code == item.JobCode);
                            if (Post != null) Position = Post.Description;
                        }
                        BSM.ListTrainee.Add(new TRTrainingEmployee
                        {
                            LineItem = i,
                            EmpCode = item.EmpCode,
                            EmpName = item.AllName,
                            Department = Department,
                            Position = Position
                        });
                    }
                }
                BSM.ListTrainee.OrderBy(x => x.EmpCode);
                var result = new
                {
                    MS = SYConstant.OK,
                    TotalCapacity = BSM.ListTrainee.Count()
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion


        //public ActionResult Invite(string id)
        //{
        //    UserSession();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    DataSelector();
        //    ActionName = "Index";
        //    //UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID] = id;
        //    TrainingInvitationObject BSM = new TrainingInvitationObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainingInvitationObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    if (id != null)
        //    {
        //        string fileName = Server.MapPath("~/Content/TEMPLATE/humica-e0886-firebase-adminsdk-95iz2-87c45a528b.json");

        //        string URL = SYUrl.getBaseUrl() + "/Training/Process/TRTrainingEmployee/index/";
        //        string msg = BSM.InviteTheDoc(id, URL, fileName);
        //        if (msg == SYConstant.OK)
        //        {
        //            var mess = SYMessages.getMessageObject("INVITED_D", user.Lang);
        //            mess.DocumentNumber = id;
        //            //mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
        //            Session[Index_Sess_Obj + ActionName] = BSM;
        //            Session[SYConstant.MESSAGE_SUBMIT] = mess;
        //        }
        //        else
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //        }
        //        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //    }
        //    else
        //    {
        //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
        //    }
        //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}


        //#endregion
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