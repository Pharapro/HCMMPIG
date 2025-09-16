using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EForm;
using Humica.Models.SY;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.Performance
{
    public class ESSSelfAssessmentController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESSA000008";
        private const string URL_SCREEN = "/SelfService/Performance/ESSSelfAssessment/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "PortalNo";
        IClsEFEmpPortal BSM;
        public ESSSelfAssessmentController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEFEmpPortal();
            BSM.OnLoad();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexLoading(true);
            BSM.OnIndexLoadingPending();
            BSM.ListHeader = BSM.ListHeader.Where(w => !BSM.ListAssessmentPending.Where(x => x.PortalNo == w.PortalNo).Any()).ToList();
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
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PendingList()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PendingList", BSM.ListAssessmentPending);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID != null)
            {
                BSM.OnEditLoading(ID);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]

        public ActionResult Edit(string id, ClsEFEmpPortal collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Update(id,true);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
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
        #endregion
       
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (id != null)
            {
                BSM.OnEditLoading(id);
                if (BSM.Header != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Details
        #region Action
        public ActionResult Approve(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ApproveDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        [HttpPost]
        public ActionResult CheckValue(string Value, string Code, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];
                string[] sp = Value.Split('_');
                string QuestionCode = sp[0].ToString();
                int LineItem = Convert.ToInt32(sp[1]);

                var checkexist = BSM.ListItem.Where(w => w.QuestionCode == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new EFEmpPortalItem();
                    obj.QuestionCode = Code;
                    obj.CorrectValue = LineItem.ToString();
                    BSM.ListItem.Add(obj);
                }
                else
                {
                    checkexist.First().CorrectValue = LineItem.ToString();
                }
                var result = new
                {
                    MS = SYConstant.OK,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult CommentValue(string Code, string Value, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEFEmpPortal)Session[Index_Sess_Obj + ActionName];

                var checkexist = BSM.ListItem.Where(w => w.QuestionCode == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new EFEmpPortalItem();
                    obj.QuestionCode = Code;
                    obj.Comment = Value;
                    BSM.ListItem.Add(obj);
                }
                else
                {
                    checkexist.First().QuestionCode = Code;
                    checkexist.First().Comment = Value;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}