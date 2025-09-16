using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.SY;
using Humica.Performance;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRAppraisalReveiwController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000013";
        private const string URL_SCREEN = "/HRM/Appraisal/HRAppraisalReveiw/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo;Status";
        private string PATH_FILE = "123123123123123";
        protected IClsAppraisel BSM;
        IUnitOfWork unitOfWork;
        public HRAppraisalReveiwController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAppraisel();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnReveiwIndexLoading();
            BSM.OnReveiwIndexLoadingPending();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListAppProcess);
        }
        public ActionResult PendingList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PendingList", BSM.ListAppraisaPending);
        }
        #endregion
        #region 'Create'
        public ActionResult Create(string ApprID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            ViewData[SYSConstant.PARAM_ID] = ApprID;

            if (ApprID != null)
            {
                BSM.GetDateAppraisal(ApprID);
                if (!string.IsNullOrEmpty(BSM.HeaderProcess.EmpCode))
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create(string APPID, ClsAppraisel collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = APPID;
            if (collection.HeaderProcess.DocumentRef != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsAppraisel)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.HeaderProcess.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.HeaderProcess = collection.HeaderProcess;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateAppraisalReview(BSM.HeaderProcess.DocumentRef);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
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
        #endregion
        #region 'Details'
        public ActionResult Details(int id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            BSM.OnDetailLoadingReview(id);
            if (BSM.HeaderProcess != null)
            {
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        public ActionResult ReleaseDoc(int id)
        {
            UserSession();
            BSM.ScreenId = SCREEN_ID;

            string msg = BSM.ReleaseDocReview(id);
            if (msg == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_RELEASED", user.Lang);
                mess.DocumentNumber = id.ToString();
                mess.Description = mess.Description;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                Session[Index_Sess_Obj + ActionName] = null;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
        }
        public ActionResult Closed(int id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.ClosedTheDocReview(id);
            if (msg == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("CLOSED_EN", user.Lang);
                mess.Description = mess.Description;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region 'Attachment'
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
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
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListAppProcessItem);
        }
        #region 'Code'
        private void DataSelector()
        {
            ViewData["APPRTYPE_SELECT"] = unitOfWork.Set<HRApprType>().ToList();
            SYDataList objList = new SYDataList("EVALUATE_PROCESS");
            ViewData["EVALPROCESS_SELECT"] = objList.ListData;
        }
        #endregion
    }
}
