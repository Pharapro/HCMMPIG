using Humica.Attendance;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.SelfService.LeaveBalance
{
    public class ESSATRequestMissScanController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000031";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSATRequestMissScan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocumentNo";
        private string DOCTYPE = "RLE01";
        IClsATRequestMissScan BSM;
        IUnitOfWork unitOfWork;

        public ESSATRequestMissScanController()
            : base()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsATRequestMissScan();
            BSM.OnLoad();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATEmpMissscan>();

            BSM.ListHeader = unitOfWork.Set<ATEmpMissscan>().Where(w => w.MissscanDate.Year == BSM.FInYear.INYear && w.EmpCode == BSM.User.UserName).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsATRequestMissScan collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.User = SYSession.getSessionUser();
            BSM.ListHeader = new List<ATEmpMissscan>();
            BSM.FInYear = collection.FInYear;
            var ListLeave = unitOfWork.Set<ATEmpMissscan>().Where(w => w.MissscanDate.Year == BSM.FInYear.INYear && w.EmpCode == BSM.User.UserName).OrderByDescending(x => x.MissscanDate).ToList();
            BSM.ListHeader = ListLeave.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<ATEmpMissscan>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.User = SYSession.getSessionUser();
            BSM.Header = new ATEmpMissscan();
            DateTime DateNow = DateTime.Now;
            BSM.Header.Time = DateTime.Now;
            BSM.Header.EmpCode = user.UserName;

            var emp = unitOfWork.Set<HR_STAFF_VIEW>().Where(w => w.EmpCode == BSM.User.UserName).ToList();
            if (emp.Count > 0)
            {
                BSM.HeaderStaff = emp.FirstOrDefault(x => x.EmpCode == BSM.User.UserName);
                BSM.Header = new ATEmpMissscan();
                BSM.Header.MissscanDate = DateTime.Now;
                BSM.Header.RequestDate = DateTime.Now;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(string ID, ClsATRequestMissScan collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (BSM.Header != null)
            {
                string URL = SYUrl.getBaseUrl() + "/SelfService/ESSATRequestMissScan/Details/";
                BSM.DocType = DOCTYPE;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.ESSCreate(URL);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocumentNo.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + BSM.Header.DocumentNo.ToString();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region Edit
        public ActionResult Edit(string DocNo)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = DocNo;
            if (DocNo != null)
            {
                var msg = BSM.OnEditLoading(DocNo);
                if (msg != SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string DocNo, ClsATRequestMissScan collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID] = DocNo;

            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Update(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = DocNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Details"
        public ActionResult Details(string DocNo)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYSConstant.PARAM_ID] = DocNo;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = unitOfWork.Set<ATEmpMissscan>().FirstOrDefault(w => w.DocumentNo == DocNo);
            if (BSM.Header != null)
            {
                BSM.ListApproval = unitOfWork.Repository<ExDocApproval>().Where(w => w.DocumentNo == DocNo && w.DocumentType ==BSM.Header.RequestType ).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsATRequestMissScan)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            DataSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("Request_Late_Early");
            ViewData["REQUEST_SELECT"] = objList.ListData.Where(w=>w.SelectValue == "MISSSCAN");
            SYDataList objListt = new SYDataList("Request_Misscan");
            ViewData["MISSSCAN_SELECT"] = objListt.ListData;
        }
    }
}
