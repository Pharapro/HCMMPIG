using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.RCM;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.RCM
{
    public class RCMInterviewChklistController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RCM0000006";
        private const string URL_SCREEN = "/HRM/RCM/RCMInterviewChklist/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApplicantID";
        IClsRCMCheckList BSM;
        IUnitOfWork unitOfWork;
        public RCMInterviewChklistController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMCheckList();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            BSM.Filtering = new FilterCandidate();
            BSM.ListCandidate = new List<RCMApplicant>();
            BSM.ListInterViewer = new List<RCMVInterviewer>();
            BSM.ListInt = new List<RCMPInterview>();
            BSM.Filtering.IntVStep = 1;
            var Intv = unitOfWork.Repository<RCMPInterview>().Queryable().ToList().OrderByDescending(w => w.TranNo).ToList();
            if (Intv.Any()) BSM.ListInt = Intv;
            string Open = SYDocumentStatus.OPEN.ToString();
            var Cand = unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => w.IntVStatus == Open && w.IntvStep == 1).ToList();
            if (Cand.Any()) BSM.ListCandidate = Cand;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsRCMCheckList collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            string Open = SYDocumentStatus.OPEN.ToString();
            BSM.Filtering.Vacancy = collection.Filtering.Vacancy;
            BSM.Filtering.IntVStep = collection.Filtering.IntVStep;
            var _listApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => w.IntvStep == BSM.Filtering.IntVStep && w.VacNo == BSM.Filtering.Vacancy).ToList();
            BSM.ListCandidate = _listApplicant.Where(w => w.IntVStatus == Open && w.IntvStep == 1).ToList();
            BSM.ListInt = unitOfWork.Repository<RCMPInterview>().Queryable().Where(w => w.IntVStep == BSM.Filtering.IntVStep && w.VacNo == BSM.Filtering.Vacancy).ToList();
            collection.ListCandidate = BSM.ListCandidate;
            collection.ListInt = BSM.ListInt;
            Session[Index_Sess_Obj + ActionName] = collection;
            return View(collection);
        }
        #endregion 
        #region 'Create'
        public ActionResult Create(string ApplicantID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            BSM.Header = new RCMPInterview();
            BSM.ListQuestionnair = new List<RCMIntVQuestionnaire>();
            BSM.ListInterViewer = new List<RCMVInterviewer>();
            var _App = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
            if (_App != null)
            {
                BSM.VacNo = _App.VacNo;
                BSM.IntVStep = _App.IntvStep;
                BSM.Header.ApplicantID = _App.ApplicantID;
                BSM.Header.CandidateName = _App.AllName;
                BSM.Header.ApplyPost = _App.ApplyPosition;
                BSM.Header.IntVStep = Convert.ToInt32(_App.IntvStep);
                BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
                BSM.Header.StartTime = DateTime.Now;
                BSM.Header.EndTime = DateTime.Now;
                BSM.ExpectSalary = Convert.ToDecimal(_App.ExpectSalary);
                BSM.WorkType = _App.WorkingType;
                BSM.Gender = _App.Gender;
                BSM.Header.AppointmentDate = DateTime.Now;
                BSM.Header.AlertToInterviewer = "EM";
                var _ListQuest = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().ToList();
                _ListQuest = _ListQuest.Where(w => w.Position == _App.ApplyPosition && w.Step == _App.IntvStep).ToList();
                if (_ListQuest.Count() > 0)
                {
                    int num = 0;
                    foreach (var read in _ListQuest)
                    {
                        var _Q = new RCMIntVQuestionnaire();
                        _Q.IntVStep = Convert.ToInt32(read.Step);
                        _Q.LineItem = num + 1;
                        _Q.Description = read.Description;
                        _Q.QType = read.Position;
                        BSM.ListQuestionnair.Add(_Q);
                        num++;
                    }
                }
                var _ListIntVer = unitOfWork.Repository<RCMVInterviewer>().Queryable().ToList();
                _ListIntVer = _ListIntVer.Where(w => w.Code == _App.VacNo && w.IntStep == _App.IntvStep).ToList();
                if (_ListIntVer.Count() > 0)
                {
                    BSM.ListInterViewer = _ListIntVer.ToList();
                }
                UserConfListAndForm();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create(ClsRCMCheckList collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            var BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];

            collection.ListQuestionnair = BSM.ListQuestionnair.ToList();
            collection.ListInterViewer = BSM.ListInterViewer.ToList();
            collection.IntVStep = BSM.IntVStep;
            collection.VacNo = BSM.VacNo;

            if (ModelState.IsValid)
            {
                string msg = collection.createJobIntV();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    //mess.DocumentNumber = collection.Header.ApplicantID;
                    //mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?ApplicantID=" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(collection);
                }
                else
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }
            }
            Session[Index_Sess_Obj + this.ActionName] = collection;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion 
        #region 'Edit'
        public ActionResult Edit(string TranNo)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (TranNo == "null") TranNo = null;
            if (!string.IsNullOrEmpty(TranNo))
            {
                int Tran = Convert.ToInt32(TranNo);
                BSM.Header = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.TranNo == Tran);

                if (BSM.Header != null)
                {
                    var chkData = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == BSM.Header.ApplicantID);
                    BSM.VacNo = chkData.VacNo;
                    BSM.IntVStep = chkData.IntvStep;
                    BSM.WorkType = chkData.WorkingType;
                    BSM.Gender = chkData.Gender;
                    BSM.ExpectSalary = Convert.ToDecimal(chkData.ExpectSalary);
                    BSM.ListQuestionnair = unitOfWork.Repository<RCMIntVQuestionnaire>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID && w.IntVStep == BSM.Header.IntVStep).ToList();
                    BSM.ListInterViewer = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.Code == BSM.VacNo && w.IntStep == BSM.Header.IntVStep).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string TranNo, ClsRCMCheckList collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();



            BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];

            collection.ListQuestionnair = BSM.ListQuestionnair;
            collection.ListInterViewer = BSM.ListInterViewer;
            collection.ScreenId = SCREEN_ID;

            if (ModelState.IsValid)
            {
                string msg = collection.updateCheckList(TranNo);

                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(collection);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                //mess.DocumentNumber = ApplicantID;
                //mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?ApplicantID=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);

            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion 
        #region 'Details'
        public ActionResult Details(string TranNo)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYConstant.PARAM_ID] = TranNo;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (TranNo == "null") TranNo = null;
            if (!string.IsNullOrEmpty(TranNo))
            {
                int Tran = Convert.ToInt32(TranNo);
                BSM.Header = unitOfWork.Repository<RCMPInterview>().Queryable().FirstOrDefault(w => w.TranNo == Tran);

                if (BSM.Header != null)
                {
                    var chkData = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == BSM.Header.ApplicantID);
                    BSM.VacNo = chkData.VacNo;
                    BSM.IntVStep = chkData.IntvStep;
                    BSM.WorkType = chkData.WorkingType;
                    BSM.Gender = chkData.Gender;
                    BSM.ExpectSalary = Convert.ToDecimal(chkData.ExpectSalary);
                    BSM.ListQuestionnair = unitOfWork.Repository<RCMIntVQuestionnaire>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID && w.IntVStep == BSM.Header.IntVStep).ToList();
                    BSM.ListInterViewer = unitOfWork.Repository<RCMVInterviewer>().Queryable().Where(w => w.Code == BSM.VacNo && w.IntStep == BSM.Header.IntVStep).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            BSM.ListInt = new List<RCMPInterview>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM.ListInt);
        }
        public ActionResult _Candidate()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            BSM.ListCandidate = new List<RCMApplicant>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Candidate", BSM.ListCandidate);
        }
        public ActionResult _Question()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Question", BSM.ListQuestionnair);
        }
        public ActionResult CreateItem(RCMIntVQuestionnaire ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (BSM.ListQuestionnair.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListQuestionnair.Max(w => w.LineItem) + 1;
                    }

                    BSM.ListQuestionnair.Add(ModelObject);
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

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Question", BSM.ListQuestionnair);
        }
        public ActionResult EditItem(RCMIntVQuestionnaire ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];


                var objCheck = BSM.ListQuestionnair.Where(w => w.LineItem == ModelObject.LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    objCheck.First().Description = ModelObject.Description;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;


            }
            return PartialView("_Question", BSM.ListQuestionnair);
        }
        public ActionResult DeleteItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListQuestionnair.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListQuestionnair.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Question", BSM.ListQuestionnair);
        }
        public ActionResult _InterviewerAssign()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_InterviewerAssign", BSM.ListInterViewer);
        }
        public ActionResult CreateInt(RCMVInterviewer ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var _list = BSM.ListInterViewer.ToList();
                    var objCheck = _list.FirstOrDefault(w => w.IntStep == ModelObject.IntStep && w.EmpCode == ModelObject.EmpCode);
                    if (objCheck != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                    }
                    else
                    {
                        if (_list.Count == 0)
                        {
                            ModelObject.LineItem = 1;
                        }
                        else
                        {
                            ModelObject.LineItem = _list.Max(w => w.LineItem) + 1;
                        }

                        BSM.ListInterViewer.Add(ModelObject);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
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

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_InterviewerAssign", BSM.ListInterViewer);
        }
        public ActionResult DeleteInt(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListInterViewer.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListInterViewer.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_InterviewerAssign", BSM.ListInterViewer);
        }
        #endregion
        #region 'Print'
        public ActionResult Print(string TranNo)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            ViewData[SYSConstant.PARAM_ID] = TranNo;
            this.UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string TranNo)
        {
            this.UserSession();
            this.UserConf(ActionBehavior.VIEWONLY);
            this.ActionName = "Print";
            if (TranNo == "null") TranNo = null;
            if (!string.IsNullOrEmpty(TranNo))
            {
                int Tran = Convert.ToInt32(TranNo);
                var obj = unitOfWork.Repository<RCMPInterview>().Queryable().Where(w => w.TranNo == Tran).ToList();
                if (obj.Count > 0)
                {
                    try
                    {
                        ViewData[SYSConstant.PARAM_ID] = Tran;
                        FRMInterviewChklstTakeNote sa = new FRMInterviewChklstTakeNote();
                        sa.Parameters["TranNo"].Value = obj.FirstOrDefault().TranNo;
                        sa.Parameters["TranNo"].Visible = false;
                        Session[this.Index_Sess_Obj + this.ActionName] = sa;
                        return PartialView("PrintForm", sa);
                    }
                    catch (Exception ex)
                    {
                        SYEventLogObject.saveEventLog(new SYEventLog()
                        {
                            ScreenId = "RCM0000006",
                            UserId = this.user.UserID.ToString(),
                            DocurmentAction = Tran.ToString(),
                            Action = SYActionBehavior.ADD.ToString()
                        }, ex, true);
                    }
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string JobID)
        {
            ActionName = "Print";
            FRMInterviewChklstTakeNote reportModel = (FRMInterviewChklstTakeNote)Session[Index_Sess_Obj + ActionName];
            ViewData[SYSConstant.PARAM_ID] = JobID;
            return ReportViewerExtension.ExportTo(reportModel);
        }
        #endregion
        public ActionResult ReleaseDoc(string id)
        {
            UserSession();
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMCheckList)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;

                int Tran = Convert.ToInt32(id);
                string msg = BSM.ReleaseDoc(Tran);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + BSM.ErrorMessage;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region 'Private Code'
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("INTV_STATUS");
            ViewData["STATUS_SELECT"] = objList.ListData;
            ViewData["POSITION_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["EMPCODE_SELECT"] = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().ToList();
            ViewData["SELECT"] = unitOfWork.Repository<RCMApplicant>().Queryable().ToList();
            objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            ViewData["WORKTYPE_SELECT"] = unitOfWork.Repository<HRWorkingType>().Queryable().ToList().OrderBy(w => w.Description);
            var completed = SYDocumentStatus.COMPLETED.ToString();
            ViewData["VACANCY_SELECT"] = unitOfWork.Repository<RCMVacancy>().Queryable().Where(w => w.Status != completed).ToList().OrderBy(w => w.Code);
            objList = new SYDataList("STAFF_TYPE");
            ViewData["STAFFTYPE_SELECT"] = objList.ListData;
        }
        #endregion 
    }
}