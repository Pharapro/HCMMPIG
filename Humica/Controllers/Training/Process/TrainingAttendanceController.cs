using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class TrainingAttendanceController : EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TR00000003";
        private const string URL_SCREEN = "/Training/Process/TrainingAttendance/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string KeyName = "TrainNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SMSystemEntity DP = new SMSystemEntity();
        Core.DB.HumicaDBContext DBStaff = new Core.DB.HumicaDBContext();
        public TrainingAttendanceController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListHeader = new List<TRTrainingAttendance>();
            BSM.ListHeader = DBX.TRTrainingAttendances.ToList();
            BSM.ListTrainee = new List<TRTrainingEmployee>();

            var RequestHeader = DBX.TrainingRequestHeader.ToList();
            string Approve = SYDocumentStatus.APPROVED.ToString();
            BSM.ListTrainee = DBX.TRTrainingEmployees.AsEnumerable().Where(x => x.IsAccept == true && RequestHeader.Where(z => z.Status == Approve).Any() && !BSM.ListHeader.Where(w => x.ID == w.ID && x.TrainNo.ToString() == w.TrainNo && x.CalendarID == w.CalendarID
             && x.LineItem == w.LineItem).Any()).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialTrainee()
        {
            ActionName = "Index";
            UserSession();
            //DataSelector();
            UserConfList(KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialTrainee", BSM.ListTrainee);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListHeader = new List<TRTrainingAttendance>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListHeader);
        }
        #region edit
        public async Task<ActionResult> Edit(int TrainNo, int CalendarID, int LineItem)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingProcessObject BSM = new TrainingProcessObject();
            string TrainNo_ = TrainNo.ToString();
            if (!string.IsNullOrEmpty(TrainNo_))
            {
                BSM.TrainingAttendance = new TRTrainingAttendance();
                BSM.TrainingAttendance = await DBX.TRTrainingAttendances.FindAsync(TrainNo, CalendarID, LineItem);
                if (BSM.TrainingAttendance != null)
                {
                    var TrainingAttendance = DBX.TRTrainingAttendances;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string TrainNo, int CalendarID, int LineItem, TrainingProcessObject trainingProcessObject)
        {
            try
            {
                ActionName = "Edit";
                this.ScreendIDControl = SCREENID;
                UserSession();
                UserConfListAndForm(this.KeyName);
                //DataSelector();
                ViewData[SYSConstant.PARAM_ID1] = false;
                TrainingProcessObject BSM = new TrainingProcessObject();
                string trianNo = TrainNo.ToString();
                if (!string.IsNullOrEmpty(trianNo))
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                        BSM.TrainingAttendance = trainingProcessObject.TrainingAttendance;
                    }
                    BSM.ScreenId = SCREENID;
                    string msg = BSM.EditTrainingAttendance(TrainNo, CalendarID, LineItem);
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        // mess.DocumentNumber = TrainNo.ToString() + CalendarID + LineItem;
                        // mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?TrainNo=" + mess.DocumentNumber;
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
        #region delete
        public ActionResult Delete(string TrainNo, int CalendarID, int LineItem)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            //DataSelector();
            string TrainNo_ = TrainNo.ToString();
            if (!string.IsNullOrEmpty(TrainNo_))
            {
                TrainingProcessObject Del = new TrainingProcessObject();
                string msg = Del.DeleteTrainingAttendance(TrainNo, CalendarID, LineItem);
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
        #region view
        public async Task<ActionResult> Details(int TrainNo, int CalendarID, int LineItem)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID1] = true;
            ViewData[Humica.EF.Models.SY.SYConstant.PARAM_ID] = TrainNo;
            string TrainNo_ = TrainNo.ToString();
            if (!string.IsNullOrEmpty(TrainNo_))
            {
                TrainingProcessObject BSM = new TrainingProcessObject();
                BSM.TrainingAttendance = new TRTrainingAttendance();
                BSM.TrainingAttendance = await DBX.TRTrainingAttendances.FindAsync(TrainNo, CalendarID, LineItem);
                if (BSM.TrainingAttendance != null)
                {
                    var objTrainingSession = DBX.TRTrainingSessions;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Ajex
        public ActionResult Attendence(string ID,string TrainNo, string CalendarID, string LineItem)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            //DataSelector();

            if (!string.IsNullOrEmpty(TrainNo) && !string.IsNullOrEmpty(CalendarID) && !string.IsNullOrEmpty(LineItem))
            {
                TrainingProcessObject BSM = new TrainingProcessObject();
                string msg = BSM.Att(ID,TrainNo, CalendarID, LineItem, "", 1);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
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
        public ActionResult Absent(string ID,string TrainNo, string CalendarID, string LineItem, string Remark)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            //DataSelector();

            if (!string.IsNullOrEmpty(TrainNo) && !string.IsNullOrEmpty(CalendarID) && !string.IsNullOrEmpty(LineItem))
            {
                TrainingProcessObject BSM = new TrainingProcessObject();
                string msg = BSM.Att(ID,TrainNo, CalendarID, LineItem, Remark, 0);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("MS001", user.Lang);
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

    }
}
