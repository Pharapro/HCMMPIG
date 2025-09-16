using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.RCM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Humica.EF.Repo;

namespace Humica.Controllers.HRM.RCM
{
    public class RCMApplicantController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "RCM0000003";
        private const string URL_SCREEN = "/HRM/RCM/RCMApplicant/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApplicantID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_IDENT = "identity";

        IClsRCMApplicant BSM;
        IUnitOfWork unitOfWork;
        public RCMApplicantController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMApplicant();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        #region List 
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<RCMApplicant>();
            BSM.Filters = new Core.FT.Filters();
            BSM.Filtering = new Filtering();
            BSM.Filters.Status = "All";
            BSM.ListHeader = unitOfWork.Repository<RCMApplicant>().Queryable().OrderByDescending(w => w.ApplicantID).ToList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> Index(ClsRCMApplicant collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }

            BSM.Filters = collection.Filters;
            DateTime fromDate = BSM.Filters.FromDate.HasValue ? BSM.Filters.FromDate.Value : Core.DateTimeHelper.MinValue;
            DateTime toDate = BSM.Filters.ToDate.HasValue ? BSM.Filters.ToDate.Value : Core.DateTimeHelper.MaxValue;
            var _listApplicant = await unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => EntityFunctions.TruncateTime(w.ApplyDate) >= fromDate.Date &&
                EntityFunctions.TruncateTime(w.ApplyDate) <= toDate.Date).ToListAsync();
            if (BSM.Filters.Status?.ToUpper() == "ALL")
            {
                BSM.ListHeader = _listApplicant.ToList();
            }
            else
            {
                BSM.ListHeader = _listApplicant.Where(w => w.ShortList == BSM.Filters.Status).ToList();
            }
            collection.ListHeader = BSM.ListHeader;
            Session[Index_Sess_Obj + ActionName] = collection;
            return View(collection);
        }
        public ActionResult GridItem()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItem", BSM.ListHeader);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            //BSM.ListRecruitRequest = new List<RCM_RecruitRequest_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.Header = new RCMApplicant();
            BSM.ListHeader = new List<RCMApplicant>();
            BSM.ListDependent = new List<RCMADependent>();
            BSM.ListEdu = new List<RCMAEdu>();
            BSM.ListLang = new List<RCMALanguage>();
            BSM.ListTraining = new List<RCMATraining>();
            BSM.ListWorkHistory = new List<RCMAWorkHistory>();
            BSM.ListRef = new List<RCMAReference>();
            BSM.ListIdentity = new List<RCMAIdentity>();
            BSM.Header.ApplyDate = DateTime.Now;
            BSM.Header.ExpectSalary = 0;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsRCMApplicant collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            DataSelector();
            var BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

            BSM.Header = collection.Header;
            collection = BSM;
            string savedFilePath = "";
            if (Session[PATH_FILE] != null)
            {
                collection.Header.ResumeFile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            collection.ScreenId = SCREEN_ID;
            string URL = SYUrl.getBaseUrl() + "/HRM/RCM/RCMApplicant/Details/";
            string msg = collection.createApplicant(savedFilePath, URL);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = collection.Header.ApplicantID;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?ApplicantID=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
            }
            else
            {
                Session[Index_Sess_Obj + this.ActionName] = collection;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
        }
        #endregion 
        #region Edit
        public ActionResult Edit(string ApplicantID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (ApplicantID == "null") ApplicantID = null;
            if (!string.IsNullOrEmpty(ApplicantID))
            {
                BSM.Header = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (BSM.Header != null)
                {
                    BSM.ListDependent = unitOfWork.Repository<RCMADependent>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                    BSM.ListEdu = unitOfWork.Repository<RCMAEdu>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    BSM.ListLang = unitOfWork.Repository<RCMALanguage>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    BSM.ListTraining = unitOfWork.Repository<RCMATraining>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    BSM.ListWorkHistory = unitOfWork.Repository<RCMAWorkHistory>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    BSM.ListRef = unitOfWork.Repository<RCMAReference>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    BSM.ListIdentity = unitOfWork.Repository<RCMAIdentity>().Queryable().Where(w => w.ApplicantID == BSM.Header.ApplicantID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string ApplicantID, ClsRCMApplicant collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            var BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            BSM.Header = collection.Header;
            collection = BSM;
            if (Session[PATH_FILE] != null)
            {
                collection.Header.ResumeFile = Session[PATH_FILE].ToString();
                Session[PATH_FILE] = null;
            }
            collection.ScreenId = SCREEN_ID;
            string msg = collection.updateApplicant(ApplicantID);

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = ApplicantID;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?ApplicantID=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(collection);
            }
            else
            {
                SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = collection;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return View(collection);
            }
        }
        #endregion 
        #region Details
        public ActionResult Details(string ApplicantID)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            if (ApplicantID == "null") ApplicantID = null;
            if (!string.IsNullOrEmpty(ApplicantID))
            {
                ViewData[SYConstant.PARAM_ID] = ApplicantID;
                ViewData[ClsConstant.IS_READ_ONLY] = true;
                BSM.Header = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (BSM.Header == null)
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                BSM.ListDependent = unitOfWork.Repository<RCMADependent>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListIdentity = unitOfWork.Repository<RCMAIdentity>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListEdu = unitOfWork.Repository<RCMAEdu>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListLang = unitOfWork.Repository<RCMALanguage>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListTraining = unitOfWork.Repository<RCMATraining>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListWorkHistory = unitOfWork.Repository<RCMAWorkHistory>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                BSM.ListRef = unitOfWork.Repository<RCMAReference>().Queryable().Where(w => w.ApplicantID == ApplicantID).ToList();
                UserConfForm(SYActionBehavior.VIEW);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Delete  
        public ActionResult Delete(string ApplicantID)
        {
            UserSession();
            if (ApplicantID == "null") ApplicantID = null;
            if (!string.IsNullOrEmpty(ApplicantID))
            {
                BSM.Header = unitOfWork.Repository<RCMApplicant>().Queryable().FirstOrDefault(w => w.ApplicantID == ApplicantID);
                if (BSM.Header != null)
                {
                    string msg = BSM.deleteApplicant(ApplicantID);

                    if (msg == SYConstant.OK)
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Send CV
        public ActionResult SendCV(string ApplicantID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ApplicantID == "null") ApplicantID = null;
            if (!string.IsNullOrEmpty(ApplicantID))
            {

                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                }
                string URL = SYUrl.getBaseUrl() + "/HRM/RCM/RCMApplicant/Details/";
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.SENTCV(ApplicantID, URL);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    SYMessages mess = SYMessages.getMessageObject("SEND_CV", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
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
        #endregion
        #region Edu
        public ActionResult _Edu()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("_Edu", BSM.ListEdu);
        }
        public ActionResult _EduDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_EduDetails", BSM);
        }
        public ActionResult CreateEdu(RCMAEdu ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(ModelObject.EduType))
            {
                try
                {
                    if (BSM.ListEdu.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else if (ModelObject.EduType == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_EDUTYPE", user.Lang);
                    }
                    else if (ModelObject.EduCenter == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_EDUC", user.Lang);
                    }
                    else if (ModelObject.Major == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_MAJOR", user.Lang);
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListEdu.Max(w => w.LineItem) + 1;
                    }

                    BSM.ListEdu.Add(ModelObject);
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
            return PartialView("_Edu", BSM.ListEdu);
        }
        public ActionResult EditEdu(RCMAEdu ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                var objCheck = BSM.ListEdu.FirstOrDefault(w => w.LineItem == ModelObject.LineItem);
                if (objCheck != null)
                {
                    objCheck.EduType = ModelObject.EduType;
                    objCheck.EduCenter = ModelObject.EduCenter;
                    objCheck.Major = ModelObject.Major;
                    objCheck.Result = ModelObject.Result;
                    objCheck.StartDate = ModelObject.StartDate;
                    objCheck.EndDate = ModelObject.EndDate;
                    objCheck.Remark = ModelObject.Remark;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;

            }
            return PartialView("_Edu", BSM.ListEdu);
        }
        public ActionResult DeleteEdu(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListEdu.FirstOrDefault(w => w.LineItem == LineItem);

                if (objCheck != null)
                {
                    BSM.ListEdu.Remove(objCheck);
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Edu", BSM.ListEdu);
        }
        #endregion
        #region Lang
        public ActionResult _Lang()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Lang", BSM.ListLang);
        }
        public ActionResult _LangDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_LangDetails", BSM);
        }
        public ActionResult CreateLang(RCMALanguage ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(ModelObject.Lang))
            {
                try
                {
                    var objCheck = BSM.ListLang.Where(w => w.Lang == ModelObject.Lang).ToList();
                    if (objCheck.Count > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
                    else if (ModelObject.Lang == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_LANG", user.Lang);
                    }
                    else
                    {
                        BSM.ListLang.Add(ModelObject);
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
            return PartialView("_Lang", BSM.ListLang);
        }
        public ActionResult EditLang(RCMALanguage ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListLang.FirstOrDefault(w => w.Lang == ModelObject.Lang);
                if (objCheck != null)
                {
                    objCheck.Speaking = ModelObject.Speaking;
                    objCheck.Reading = ModelObject.Reading;
                    objCheck.Listening = ModelObject.Listening;
                    objCheck.Writing = ModelObject.Writing;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Lang", BSM.ListLang);
        }
        public ActionResult DeleteLang(string Lang)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListLang.FirstOrDefault(w => w.Lang == Lang);

                if (objCheck != null)
                {
                    BSM.ListLang.Remove(objCheck);
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Lang", BSM.ListLang);
        }
        #endregion 
        #region Training
        public ActionResult _Training()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("_Training", BSM.ListTraining);
        }
        public ActionResult _TrainingDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("_TrainingDetails", BSM);
        }
        public ActionResult CreateTraining(RCMATraining ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }

            if (!string.IsNullOrEmpty(ModelObject.TrainingTopic))
            {
                try
                {
                    if (BSM.ListTraining.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListTraining.Max(w => w.LineItem) + 1;
                    }
                    if (ModelObject.TrainingTopic == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_TRAINTOPIC", user.Lang);
                    }
                    else
                    {
                        BSM.ListTraining.Add(ModelObject);
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
            return PartialView("_Training", BSM.ListTraining);
        }
        public ActionResult EditTraining(RCMATraining ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                var objCheck = BSM.ListTraining.FirstOrDefault(w => w.LineItem == ModelObject.LineItem);
                if (objCheck != null)
                {
                    objCheck.TrainingTopic = ModelObject.TrainingTopic;
                    objCheck.TrainingProvider = ModelObject.TrainingProvider;
                    objCheck.TrainingPlace = ModelObject.TrainingPlace;
                    objCheck.FromDate = ModelObject.FromDate;
                    objCheck.ToDate = ModelObject.ToDate;
                    objCheck.Remark = ModelObject.Remark;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Training", BSM.ListTraining);
        }
        public ActionResult DeleteTraining(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListTraining.FirstOrDefault(w => w.LineItem == LineItem);

                if (objCheck != null)
                {
                    BSM.ListTraining.Remove(objCheck);
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Training", BSM.ListTraining);
        }
        #endregion
        #region Experience
        public ActionResult _Experience()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Experience", BSM.ListWorkHistory);
        }
        public ActionResult _ExperienceDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_ExperienceDetails", BSM);
        }
        public ActionResult CreateExperience(RCMAWorkHistory ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(ModelObject.Company))
            {
                try
                {
                    if (BSM.ListWorkHistory.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListWorkHistory.Max(w => w.LineItem) + 1;
                    }
                    BSM.ListWorkHistory.Add(ModelObject);
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
            return PartialView("_Experience", BSM.ListWorkHistory);
        }
        public ActionResult EditExperience(RCMAWorkHistory ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                var objCheck = BSM.ListWorkHistory.FirstOrDefault(w => w.LineItem == ModelObject.LineItem);
                if (objCheck != null)
                {
                    objCheck.Company = ModelObject.Company;
                    objCheck.Position = ModelObject.Position;
                    objCheck.FromDate = ModelObject.FromDate;
                    objCheck.ToDate = ModelObject.ToDate;
                    objCheck.SupervisorName = ModelObject.SupervisorName;
                    objCheck.SupervisorPhone = ModelObject.SupervisorPhone;
                    objCheck.LeaveReason = ModelObject.LeaveReason;
                    objCheck.Duties = ModelObject.Duties;
                    objCheck.StartSalary = ModelObject.StartSalary;
                    objCheck.EndSalary = ModelObject.EndSalary;
                    objCheck.Remark = ModelObject.Remark;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;

            }
            return PartialView("_Experience", BSM.ListWorkHistory);
        }
        public ActionResult DeleteExperience(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListWorkHistory.FirstOrDefault(w => w.LineItem == LineItem);

                if (objCheck != null)
                {
                    BSM.ListWorkHistory.Remove(objCheck);
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Experience", BSM.ListWorkHistory);
        }
        #endregion
        #region Reference
        public ActionResult _Reference()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Reference", BSM.ListRef);
        }
        public ActionResult _ReferenceDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_ReferenceDetails", BSM);
        }
        public ActionResult CreateRef(RCMAReference ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            if (!string.IsNullOrEmpty(ModelObject.RefName))
            {
                try
                {
                    if (BSM.ListRef.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else if (ModelObject.RefName == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_REFNAME", user.Lang);
                    }
                    else if (ModelObject.Phone1 == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_REFPHONE", user.Lang);
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListRef.Max(w => w.LineItem) + 1;
                    }

                    BSM.ListRef.Add(ModelObject);
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
            return PartialView("_Reference", BSM.ListRef);
        }
        public ActionResult EditRef(RCMAReference ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRef.FirstOrDefault(w => w.LineItem == ModelObject.LineItem);

                if (objCheck != null)
                {
                    objCheck.RefName = ModelObject.RefName;
                    objCheck.Company = ModelObject.Company;
                    objCheck.Occupation = ModelObject.Occupation;
                    objCheck.Phone1 = ModelObject.Phone1;
                    objCheck.Phone2 = ModelObject.Phone2;
                    objCheck.Address = ModelObject.Address;
                    objCheck.Email = ModelObject.Email;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Reference", BSM.ListRef);
        }
        public ActionResult DeleteRef(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListRef.FirstOrDefault(w => w.LineItem == LineItem);

                if (objCheck != null)
                {
                    BSM.ListRef.Remove(objCheck);
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Reference", BSM.ListRef);
        }
        #endregion
        #region Identify
        public ActionResult _Identity()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Identity", BSM.ListIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateIden(RCMAIdentity ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                    if (ModelObject.IdentityType == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE_DOCTYPE", user.Lang);
                    }

                    if (Session[PATH_IDENT] != null)
                    {
                        ModelObject.Attachment = Session[PATH_IDENT].ToString();
                        Session[PATH_IDENT] = null;
                    }
                    var List = BSM.ListIdentity.Where(w => w.IdentityType == ModelObject.IdentityType);
                    if (List.Count() > 0)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST", user.Lang);
                    }
                    else
                    {
                        BSM.ListIdentity.Add(ModelObject);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataSelector();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Identity", BSM.ListIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIden(RCMAIdentity ObjType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListIdentity.Where(w => w.IdentityType == ObjType.IdentityType).ToList();
                if (Session[PATH_IDENT] != null)
                {
                    ObjType.Attachment = Session[PATH_IDENT].ToString();
                    Session[PATH_IDENT] = null;
                }
                else
                {
                    ObjType.Attachment = listCheck.FirstOrDefault().Attachment;
                }
                if (listCheck.ToList().Count > 0)
                {
                    var objUpdate = listCheck.First();
                    objUpdate.IDCardNo = ObjType.IDCardNo;
                    objUpdate.IssueDate = ObjType.IssueDate;
                    objUpdate.ExpiryDate = ObjType.ExpiryDate;
                    objUpdate.Attachment = ObjType.Attachment;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            DataSelector();
            return PartialView("_Identity", BSM.ListIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteIden(string IdentityType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
                }
                var error = 0;

                var checkList = BSM.ListIdentity.Where(w => w.IdentityType == IdentityType).ToList();
                if (checkList.Count == 0)
                {
                    ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                    error = 1;
                }

                if (error == 0)
                {
                    BSM.ListIdentity.Remove(checkList.First());
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            DataSelector();

            return PartialView("_Identity", BSM.ListIdentity);
        }
        public ActionResult UploadControlCallbackActionIdentity(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadIdentify",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_IDENT] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion
        #region Upload
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();
            var path = unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);
            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;
            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }

        #endregion
        #region private code
        public ActionResult ShowData(string Code, string Action)
        {
            ActionName = Action;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMApplicant)Session[Index_Sess_Obj + ActionName];
            }
            var _vac = unitOfWork.Repository<RCMVacancy>().Queryable().FirstOrDefault(w => w.Code == Code);
            if (_vac != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Branch = _vac.Branch,
                    Post = _vac.Position,
                    Dept = _vac.Dept,
                    WorkType = _vac.WorkingType
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            var objLists = new SYDataList("SHORTLIST_STATUS");
            ViewData["SHORTLIST_STATUS"] = objLists.ListData;
            var IdentityTye = new SYDataList("IdentityTye");
            ViewData["IdentityTye_SELECT"] = IdentityTye.ListData;
            SYDataList objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            objList = new SYDataList("INITIAL");
            ViewData["INITIAL_SELECT"] = objList.ListData;
            objList = new SYDataList("MARITAL");
            ViewData["MARITAL_SELECT"] = objList.ListData;
            objList = new SYDataList("LANG_SKILLS");
            ViewData["LANGSKILLS_SELECT"] = objList.ListData;
            ViewData["BRANCHES_SELECT"] = unitOfWork.Repository<HRBranch>().Queryable().ToList();
            ViewData["COUNTRY_SELECT"] = unitOfWork.Repository<HRCountry>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["NATION_SELECT"] = unitOfWork.Repository<HRNation>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["RelationshipType_LIST"] = unitOfWork.Repository<HRRelationshipType>().Queryable().ToList();
            ViewData["HREmpEduType_LIST"] = unitOfWork.Repository<HREduType>().Queryable().ToList();
            var Processing = SYDocumentStatus.PROCESSING.ToString();
            ViewData["VACANCY_SELECT"] = unitOfWork.Repository<RCMVacancy>().Queryable().Where(m => m.Status == Processing).OrderByDescending(w => w.Code).ToList();
            ViewData["LANG_SELECT"] = unitOfWork.Repository<RCMSLang>().Queryable().ToList();
            ViewData["DEPT_SELECT"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList();
            ViewData["POSITION_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList();
            ViewData["RECEIVED_SELECT"] = unitOfWork.Repository<RCMSAdvertise>().Queryable().ToList().OrderBy(w => w.Company);
            ViewData["WORKTYPE_SELECT"] = unitOfWork.Repository<HRWorkingType>().Queryable().ToList().OrderBy(w => w.Description);
        }
        #endregion 
    }
}
