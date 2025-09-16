using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Logic.POD;
using Humica.Models.SY;
using HUMICA.Models.Report.Payroll;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.POD
{

    public class IncreaseSalaryController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "POD0000002";
        private const string URL_SCREEN = "/POD/IncreaseSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "DocNo";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public IncreaseSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            Increase_SalaryObject BSM = new Increase_SalaryObject();
            BSM.ListHeader = new List<HRIncreaseSalary>();
            BSM.ListHeaderStaff = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            BSM.ListHeader = DB.HRIncreaseSalaries.ToList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            BSM.ListHeader = new List<HRIncreaseSalary>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListHeaderStaff);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            BSM.Header = new HRIncreaseSalary();
            BSM.ListHeaderHis = new List<HRIncreaseSalary>();
            BSM.ListHeaderStaff = new List<HRStaffProfile>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.EffecDate = DateTime.Now;
            BSM.Header.DocumentDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(Increase_SalaryObject BSM, string id)
        {

            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.ADD);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateAppSalary();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
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
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            string re = id;
            if (id == "null") id = null;
            if (id != null)
            {
                BSM.Header = new HRIncreaseSalary();
                BSM.ListHeaderHis = new List<HRIncreaseSalary>();
                BSM.ListApproval = new List<ExDocApproval>();
                BSM.Header = DB.HRIncreaseSalaries.FirstOrDefault(x => x.DocNo == id);
                if (BSM.Header != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == BSM.Header.DocType).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                var resualt = DB.HRIncreaseSalaries;
                List<HRIncreaseSalary> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.ListHeaderHis = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, Increase_SalaryObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditIncreaseSal(id);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
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
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ID != null)
            {
                Increase_SalaryObject BSM = new Increase_SalaryObject();
                BSM.Header = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == ID);
                var resualt = DB.HRIncreaseSalaries;
                List<HRIncreaseSalary> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.ListHeaderHis = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                if (BSM.Header != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == ID && w.DocumentType == BSM.Header.DocType).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Request
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var Objmatch = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);

                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.Header.DocType);
                    string wfMsg = "";
                    string URL = SYUrl.getBaseUrl() + "/POD/IncreaseSalary/Details/";
                    //if (excfObject != null)
                    //{
                    //    SYWorkFlowEmailObject wfo =
                    //        new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                    //             UserType.N, BSM.Header.Status);
                    //    wfo.SelectListItem = new SYSplitItem(id);
                    //    wfo.User = BSM.User;
                    //    wfo.BS = BSM.BS;
                    //    wfo.UrlView = SYUrl.getBaseUrl();
                    //    wfo.ScreenId = SCREEN_ID;
                    //    wfo.Module = "HR";
                    //    wfo.ListLineRef = new List<BSWorkAssign>();
                    //    wfo.Action = SYDocumentStatus.PENDING.ToString();
                    //    HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.DocNo, "");
                    //    FRM_increse_salary sa = new FRM_increse_salary();

                    //    sa.Parameters["DocNo"].Value = id;
                    //    sa.Parameters["DocNo"].Visible = false;
                    //    Session[this.Index_Sess_Obj + this.ActionName] = sa;
                    //    string fileName = Server.MapPath("~/Content/UPLOAD/" + id + "_" + Objmatch.DocumentDate.ToString("MMMM-yyyy") + ".pdf");
                    //    string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    //    if (!Directory.Exists(UploadDirectory))
                    //    {
                    //        Directory.CreateDirectory(UploadDirectory);
                    //    }
                    //    sa.ExportToPdf(fileName);
                    //    if (Staff.Email != null && Staff.Email != "")
                    //    {
                    //        wfo.ListObjectDictionary = new List<object>();
                    //        wfo.ListObjectDictionary.Add(Objmatch);
                    //        wfo.ListObjectDictionary.Add(Staff);
                    //        var _staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Objmatch.Requestor);
                    //        Objmatch.Requestor = _staff.AllName;
                    //        string[] filePaths = new string[] { fileName };
                    //        WorkFlowResult result = wfo.ApproProcessWorkFlow(Staff, filePaths, "Increase","Request Incarease salary");
                    //        wfMsg = wfo.getErrorMessage(result);
                    //    }
                    //    else
                    //    {
                    //        wfMsg = wfo.getErrorMessage(WorkFlowResult.EMAIL_NOT_SEND);
                    //    }
                    //}
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description + wfMsg;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approve
        public ActionResult Approve(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.approveTheDoc(id);
            if (msg == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                mess.Description = mess.Description + ". " + BSM.MessageCode;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            string sms = BSM.CancelAppIncSalary(id);
            if (sms == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                mess.Description = mess.Description + ". " + BSM.MessageCode;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Print"
        public ActionResult Print(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    string Company = SMS.SYHRCompanies.FirstOrDefault().CompENG;
                    FRM_increse_salary reportModel = new FRM_increse_salary();

                    reportModel.Parameters["DocNo"].Value = SD.DocNo;
                    reportModel.Parameters["DocNo"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";
            var SD = DB.HRIncreaseSalaries.FirstOrDefault(w => w.DocNo == id);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            if (SD != null)
            {
                FRM_increse_salary reportModel = new FRM_increse_salary();
                reportModel.Parameters["DocNo"].Value = SD.DocNo;
                reportModel.Parameters["DocNo"].Visible = false;
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region Ajax Approval
        public ActionResult SelectParam(string docType)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(SCREEN_ID, docType);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion
        #region Grid
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeaderHis);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult GridApprovalEdit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApprovalEdit", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        //if (BSM.ListApproval.Count == 0)
                        //{
                        //    ModelObject.ID = 1;
                        //}
                        //else
                        //{
                        //    ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        //}
                        //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                        ModelObject.DocumentNo = "N/A";
                        BSM.ListApproval.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataList();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListApproval.Where(w => w.Approver == Approver).ToList();
                    if (BSM.ListApproval.Count > 0)
                    {
                        var objDel = BSM.ListApproval.Where(w => w.Approver == Approver).First();
                        BSM.ListApproval.Remove(objDel);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("APPROVER_NE");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataList();

            return PartialView("GridApproval", BSM.ListApproval);
        }

        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            Increase_SalaryObject BSM = new Increase_SalaryObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
            }
            //     DataList(BSM.Header);
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        #endregion
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            UserSession();
            ActionName = "Create";
            var Stafff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (Stafff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = Stafff.AllName,
                    Branch = Stafff.Branch,
                    Division = Stafff.Division,
                    DEPT = Stafff.DEPT,
                    Position = Stafff.JobCode,
                    LevelCode = Stafff.LevelCode,
                    Salary = Stafff.Salary,
                    Effective = Stafff.EffectDate,
                };
                GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            Increase_SalaryObject BSM = new Increase_SalaryObject();
            BSM.ListHeaderHis = new List<HRIncreaseSalary>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (Increase_SalaryObject)Session[Index_Sess_Obj + ActionName];
                var resualt = DB.HRIncreaseSalaries.Where(w => w.EmpCode == EmpCode).ToList();
                BSM.ListHeaderHis = resualt;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        private void DataList()
        {
            ViewData["DOCUMENT_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SYConstant.getBranchDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            var objWf = new ExWorkFlowPreference();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, SCREEN_ID);

        }
    }
}
