using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.RCM;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.HRM.RCM
{
    public class RCMPShortListController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "RCM0000004";
        private const string URL_SCREEN = "/HRM/RCM/RCMPShortList/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApplicantID";
        IClsRCMPShortList BSM;
        IUnitOfWork unitOfWork;
        public RCMPShortListController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMPShortList();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.Filtering = new FilterShortLsit();
            BSM.ListHeader = new List<RCMApplicant>();
            BSM.Filtering.Status = "ALL";
            var _listApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().OrderByDescending(w => w.ApplicantID).ToList();
            if (_listApplicant.Any())
                BSM.ListHeader = _listApplicant.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsRCMPShortList collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Filtering.Vacancy = collection.Filtering.Vacancy;
            BSM.Filtering.Status = collection.Filtering.Status;
            var _listApplicant = new List<RCMApplicant>();
            if (string.IsNullOrEmpty(BSM.Filtering.Vacancy))
            {
                _listApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().ToList();
            }
            else
                _listApplicant = unitOfWork.Repository<RCMApplicant>().Queryable().Where(w => w.VacNo == BSM.Filtering.Vacancy).ToList();
            if (BSM.Filtering.Status == "ALL")
                BSM.ListHeader = _listApplicant.ToList();
            else
                BSM.ListHeader = _listApplicant.Where(w => w.ShortList == BSM.Filtering.Status).ToList();
            collection.ListHeader = BSM.ListHeader;
            Session[Index_Sess_Obj + ActionName] = collection;
            return View(collection);
        }
        #endregion 
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.ListHeader = new List<RCMApplicant>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
        }
        #endregion
        #region 'Status'
        public ActionResult Pass(string ApplicantIDs)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ApplicantIDs != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
                    BSM.ScreenId = SCREEN_ID;
                    string msg = BSM.Passed(ApplicantIDs);
                    if (msg == SYConstant.OK)
                    {
                        var mess = SYMessages.getMessageObject("SHORTLIST_PASS", user.Lang);
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

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Keep(string ApplicantIDs)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ApplicantIDs != null)
            {
                
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Kept(ApplicantIDs);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("SHORTLIST_KEPT", user.Lang);
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
        public ActionResult Fail(string ApplicantIDs)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ApplicantIDs != null)
            {
                
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Fail(ApplicantIDs);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("SHORTLIST_FAIL", user.Lang);
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
        public ActionResult Reject(string ApplicantIDs)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (ApplicantIDs != null)
            {
                
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsRCMPShortList)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(ApplicantIDs);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("SHORTLIST_REJ", user.Lang);
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
        #endregion
        #region 'private code'
        private void DataSelector()
        {
            var objList = new SYDataList("SHORTLIST_STATUS");
            ViewData["SHORTLIST_STATUS"] = objList.ListData;
            objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            var completed = SYDocumentStatus.COMPLETED.ToString();
            ViewData["VACANCY_SELECT"] = unitOfWork.Repository<RCMVacancy>().Queryable().Where(w => w.Status != completed).ToList().OrderBy(w => w.Code);
            ViewData["POSITION_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["WORKTYPE_SELECT"] = unitOfWork.Repository<HRWorkingType>().Queryable().ToList().OrderBy(w => w.Description);
            objList = new SYDataList("STAFF_TYPE");
            ViewData["STAFFTYPE_SELECT"] = objList.ListData;
        }
        #endregion
    }
}
