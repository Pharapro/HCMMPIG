using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class TRTrainingCalenderController : EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TR00000007";
        private const string URL_SCREEN = "/Training/Process/TRTrainingCalender/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string KeyName = "TrainNo";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
        public TRTrainingCalenderController() : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region Index
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            BSM.ListPlan = new List<ClsTrainingPlan>();
            BSM.ListPlan = BSM.LoadDataTrainingPlan();
            BSM.ListTrainingCalender = DBX.TRTrainingCalenders.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session["TrainerTypeType"] = null;
            Session["TrainerInfoID"] = null;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListTrainingCalender);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPlan);
        }
        #endregion

        #region Create MultiRef
        public ActionResult CreateMultiRef(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            BSM.HeaderCalender = new TRTrainingCalender();
           
            BSM.GetDataTrainingPlan(id);
            Session[Index_Sess_Obj + ActionName] = BSM;
            BSM.HeaderCalender.StartDate = DateTime.Now;
            BSM.HeaderCalender.EndDate = DateTime.Now;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult CreateMultiRef(TrainingCalenderObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREENID;
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = Session[Index_Sess_Obj + ActionName] as TrainingCalenderObject;
                        BSM.HeaderCalender = collection.HeaderCalender;
                    }
                    string msg = BSM.CreateCalender();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.HeaderCalender.TrainNo.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                    }
                    else
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

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

        }
        #endregion

        #region Edit
        public async Task<ActionResult> Edit(int? id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            if (id != null)
            {
                BSM.HeaderCalender = new TRTrainingCalender();
                BSM.ListAgenda = new List<TRTrainingAgenda>();
                BSM.HeaderCalender = await DBX.TRTrainingCalenders.FirstOrDefaultAsync(w => w.TrainNo == id);
                if (BSM.HeaderCalender != null)
                {
                    BSM.ListAgenda = await DBX.TRTrainingAgendas.Where(x => x.CalendarID == id).ToListAsync();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, TrainingCalenderObject collection)
        {
            try
            {
                ActionName = "Create";
                this.ScreendIDControl = SCREENID;
                UserSession();
                UserConfListAndForm(this.KeyName);
                DataSelector();
                ViewData[SYSConstant.PARAM_ID1] = false;
                TrainingCalenderObject BSM = new TrainingCalenderObject();
                if (id != null)
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
                        BSM.HeaderCalender = collection.HeaderCalender;
                    }
                    BSM.ScreenId = SCREENID;
                    string msg = BSM.EditCalender(id);
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
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            BSM.HeaderCalender = new TRTrainingCalender();
            BSM.ListAgenda = new List<TRTrainingAgenda>();

            BSM.HeaderCalender = await DBX.TRTrainingCalenders.FirstOrDefaultAsync(w => w.TrainNo == id);
            if (BSM.HeaderCalender != null)
            {
                BSM.ListAgenda = await DBX.TRTrainingAgendas.Where(x => x.CalendarID == id).ToListAsync();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int? id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (id != null)
            {
                int TranNo = Convert.ToInt32(id);
                TrainingCalenderObject Del = new TrainingCalenderObject();
                string msg = Del.DeleteCalender(TranNo);
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
        public ActionResult ReleaseDoc(int id)
        {
            UserSession();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            if (id != null)
            {
                BSM.ScreenId = SCREENID;
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
        public ActionResult Closed(int id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (id != null)
            {
                TrainingCalenderObject BSM = new TrainingCalenderObject();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREENID;
                string msg = BSM.ClosedTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("CLOSED_EN", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #region gridview session
        public ActionResult gvAgenda()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            BSM.ListAgenda = new List<TRTrainingAgenda>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingCalenderObject;
            }
            return PartialView("gvAgenda", BSM.ListAgenda);
        }
        public ActionResult CreateAgenda(TRTrainingAgenda ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
                    }
                    if (BSM.ListAgenda.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        int LineItem = BSM.ListAgenda.Max(w => w.LineItem);
                        ModelObject.LineItem = LineItem + 1;
                    }
                    BSM.ListAgenda.Add(ModelObject);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session["TrainerTypeType"] = ModelObject.TrainerType;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("gvAgenda", BSM.ListAgenda);
        }
        public ActionResult EditAgenda(TRTrainingAgenda objModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                try
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingCalenderObject;
                    BSM.EditSession(objModel);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            Session["TrainerTypeType"] = objModel.TrainerType;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("gvAgenda", BSM.ListAgenda);
        }
        public ActionResult DeleteAgenda(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingCalenderObject BSM = new TrainingCalenderObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingCalenderObject)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListAgenda.Count > 0)
                {
                    var objCheck = BSM.ListAgenda.FirstOrDefault(w => w.LineItem == LineItem);
                    BSM.ListAgenda.Remove(objCheck);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            return PartialView("gvAgenda", BSM.ListAgenda);
        }
        #endregion
        public ActionResult SelectTrainnerType(string Actionname, string TrainerType)
        {
            ActionName = Actionname;
            Session["TrainerTypeType"] = TrainerType;
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);

        }
        public string cboTrainerID_SelectedIndexChanged(int? TrainerID)
        {
            UserSession();
            UserConfListAndForm();
            Session["TrainerInfoID"] = TrainerID;
            return SYConstant.OK;
        }

        private void DataSelector()
        {
            //ViewData["INYEAR"] = GetTrainingCatalogues();
            ViewData["STAFF_VIEW"] = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            ViewData["TRAINING_COURSE"] = DBX.TRTrainingCourses.ToList();
            ViewData["TRAINING_TYPE"] = DBX.TRTrainingTypes.ToList();
            ViewData["COURSE_CATEGORY"] = DBX.TRCourseCategories.ToList();
            //ViewData["VENUE"] = DBX.TRTrainingVenue.ToList();
            //SYDataList objList = new SYDataList("TRAINING_SESSION");
            //ViewData["SESSION"] = objList.ListData;
            //var list_ = objList.ListData;
            //SYDataList DL = new SYDataList("TRAINER_TYPE");
            ViewData["LIST_TRAINERTYPE"] = DBX.TRTrainerType.ToList();

            ViewData["Topic"] = DBX.TRTopics.ToList();
            ViewData["MONTH_SELECE"] = ClsMonthOfYear.LoadMonth();
        }
    }
}
