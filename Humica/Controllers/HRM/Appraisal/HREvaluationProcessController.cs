using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HREvaluationProcessController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000008";
        private const string URL_SCREEN = "/HRM/Appraisal/HREvaluationProcess/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EvaluateID;Status";
        private string PATH_FILE = "123123123123123";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public HREvaluationProcessController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();

            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListEvaluatePending = new List<HR_VIEW_EmpEvaluationForm>();
            BSM.ListEvaluateProcess = new List<HREmpEvaluateProcess>();
            string approved = SYDocumentStatus.APPROVED.ToString();
            var _ListEvaluatePending = DBV.HR_VIEW_EmpEvaluationForm.ToList();
            var _ListEvalProcess = DB.HREmpEvaluateProcesses.ToList();
            BSM.ListEvaluatePending = _ListEvaluatePending.Where(x => x.Status == approved).OrderByDescending(x => x.EvaluateDate).ToList();
            BSM.ListEvaluateProcess = _ListEvalProcess;

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEvaluateProcess);
        }
        public ActionResult PendingList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PendingList", BSM.ListEvaluatePending);
        }
        #endregion
        #region 'Create'
        public ActionResult Create(string EvaluateID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EvaluateID;

            if (EvaluateID != null)
            {
                HREmpEvaluateObject BSM = new HREmpEvaluateObject();

                BSM.Header = DB.HREmpEvaluates.FirstOrDefault(w => w.EvaluateID == EvaluateID);
                string completed = SYDocumentStatus.COMPLETED.ToString();
                if (BSM.Header != null)
                {
                    if (BSM.Header.Status == completed)
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                    else
                    {
                        BSM.EvalProcess = new HREmpEvaluateProcess();
                        BSM.EvalProcess.EvaluateDate = DateTime.Now;
                        BSM.EvalProcess.Increase = 0;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create(string EvaluateID, HREmpEvaluateObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EvaluateID;
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (EvaluateID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (HREmpEvaluateObject)Session[Index_Sess_Obj + ActionName];
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.EvalProcess.Attachment = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                collection.Header = BSM.Header;
                BSM.EvalProcess = collection.EvalProcess;
                BSM.ScreenId = SCREEN_ID;
                string msg = collection.EditEvaluateProcess(EvaluateID);
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
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            if (id != null)
            {
                HREmpEvaluateObject BSM = new HREmpEvaluateObject();
                BSM.EvalProcess = DB.HREmpEvaluateProcesses.FirstOrDefault(w => w.EvaluateID == id);
                if (BSM.EvalProcess != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Details
        #region 'Attachment'
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
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
        #region 'Code'
        private void DataSelector()
        {
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["APPRTYPE_SELECT"] = DB.HREvaluateTypes.ToList();
            SYDataList objList = new SYDataList("EVALUATE_PROCESS");
            ViewData["EVALPROCESS_SELECT"] = objList.ListData;
        }
        #endregion
    }
}
