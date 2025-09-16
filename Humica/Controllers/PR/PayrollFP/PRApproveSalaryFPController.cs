using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Logic.PR;
using Humica.Models.Report.Payroll;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Humica.Logic.PR.PRFirstPaySalaryGeneration;

namespace HR.Controllers.PR.PRM
{

    public class PRApproveSalaryFPController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000032";
        private const string URL_SCREEN = "/PR/PRM/PRApproveSalaryFP/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ASNumber;Status";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        HumicaDBContext DB = new HumicaDBContext();
        public PRApproveSalaryFPController()
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
            DataList();

            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            IEnumerable<HisPendingAppSalaryFP> ListOTRequest = DB.HisPendingAppSalaryFPs.ToList();
            BSM.ListSalaryApproveFp = new List<SalaryApprovalFP>();
            BSM.ListApproveSalary = new List<HISApproveSalaryFD>();
            IEnumerable<ExCfWFSalaryApprover> ListApp = DB.ExCfWFSalaryApprovers.ToList();
            BSM.ListApproveSalary = DB.HISApproveSalaryFDs.ToList();
            BSM.ListAppSalaryPending = DB.HisPendingAppSalaryFPs.AsEnumerable().Where(w => w.IsLock == false && !BSM.ListApproveSalary.Where(x => x.PeriodID == w.PeriodID
            && x.Status != SYDocumentStatus.REJECTED.ToString()).Any()).ToList();
            var staff = DB.HISGenSalaries.FirstOrDefault(w=> w.EmpCode== user.UserName );
            
            ListApp = ListApp.Where(w => w.Employee == user.UserName).ToList();


            foreach (var item in ListOTRequest)
            {
                
                if (ListApp.Count() == 0)
                {
                    SalaryApprovalFP _OT = new SalaryApprovalFP();
                    _OT.PeriodID = item.PeriodID;
                    _OT.FromDate = item.FromDate;
                    _OT.ToDate = item.ToDate;                   
                    BSM.ListSalaryApproveFp.Add(_OT);
                }
            }          
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            BSM.ListApproveSalary = new List<HISApproveSalaryFD>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListApproveSalary);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListAppSalaryPending);
        }
        #endregion
        #region "Create"
        public ActionResult Create(int PeriodID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            var SalaryPending = DB.HisPendingAppSalaryFPs.Where(w => w.PeriodID == PeriodID && w.IsLock == false).ToList();

            if (SalaryPending.Count > 0)
            {
                ViewData[SYConstant.PARAM_ID] = PeriodID;
                if (PeriodID == null)
                {
                    DataList();
                    BSM.HeaderAppSalary = new HISApproveSalaryFD();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.HeaderAppSalary.DocumentDate = DateTime.Now;
                }
                else
                {
                    BSM.HeaderAppSalary = new HISApproveSalaryFD();
                    BSM.HeaderAppSalary.PeriodID = PeriodID;
                    if (user.UserName != null)
                        BSM.HeaderAppSalary.Requestor = user.UserName;
                    BSM.HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.HeaderAppSalary.DocumentDate = DateTime.Now.Date;
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.ScreenId = SCREEN_ID;
                    DataList();
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("NO_REQUEST");
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            return View(BSM);

        }

        [HttpPost]
        public ActionResult Create(PRFirstPaySalaryGeneration BSM)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateAppSalary();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderAppSalary.ASNumber.ToString();
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
                PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
                BSM.HeaderAppSalary = DB.HISApproveSalaryFDs.Find(ID);
                if (BSM.HeaderAppSalary != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == ID && w.DocumentType == BSM.HeaderAppSalary.DocumentType).ToList();

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
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var Objmatch = DB.HISApproveSalaryFDs.FirstOrDefault(w => w.ASNumber == id);

                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.HeaderAppSalary.DocumentType);
                    string wfMsg = "";
                    string URL = SYUrl.getBaseUrl() + "/PR/PRM/PRApproveSalaryFP/Details/";
                    if (excfObject != null)
                    {
                        SYWorkFlowEmailObject wfo =
                            new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                 UserType.N, BSM.HeaderAppSalary.Status);
                        wfo.SelectListItem = new SYSplitItem(id);
                        wfo.User = BSM.User;
                        wfo.BS = BSM.BS;
                        wfo.UrlView = SYUrl.getBaseUrl();
                        wfo.ScreenId = SCREEN_ID;
                        wfo.Module = "HR";
                        wfo.ListLineRef = new List<BSWorkAssign>();
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        HRStaffProfile Staff = BSM.getNextApprover(BSM.HeaderAppSalary.ASNumber, "");

                        string[] Company = SYConstant.Company_Condition.Split(',');
                        string zipFilePath = Server.MapPath("~/Content/UPLOAD/" + "_Payroll_" + Objmatch.PayInMonth.ToString("MMMM-yyyy") + ".zip");

                        using (var zip = new ZipArchive(new FileStream(zipFilePath, FileMode.Create), ZipArchiveMode.Create))
                        {
                            foreach (var code in Company)
                            {
                                if (string.IsNullOrEmpty(code)) continue;
                                RptMonthlyFirstPay sa = new RptMonthlyFirstPay();
                                var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00002"
                                    && w.DocType == "Details" && w.IsActive == true).ToList();
                                string ReportTypes = "Details";
                                var reportHelper = new clsReportHelper();
                                string path = reportHelper.Get_Path(SCREEN_ID, ReportTypes);
                                if (!string.IsNullOrEmpty(path))
                                {
                                    sa.LoadLayoutFromXml(path);
                                }
                                sa.Parameters["Company"].Value = code;
                                sa.Parameters["Company"].Visible = false;
                                sa.Parameters["Period"].Value = Objmatch.PeriodID;
                                sa.Parameters["Period"].Visible = false;
                                sa.Parameters["UserName"].Value = "Humica";
                                sa.Parameters["UserName"].Visible = false;

                                Session[this.Index_Sess_Obj + this.ActionName] = sa;

                                // Create and save the payroll report PDF
                                string fileName = $"PayrollDetails_{code}_{Objmatch.PayInMonth.ToString("MMMM-yyyy")}.pdf";
                                string filePath = Server.MapPath("~/Content/UPLOAD/" + fileName);
                                sa.ExportToPdf(filePath);
                                zip.CreateEntryFromFile(filePath, fileName);

                                ////Second report: Summary report
                                //RptSummaryPay saSummary = new RptSummaryPay();
                                //var objRptSummary = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00002"
                                //    && w.DocType == "Summary" && w.IsActive == true).ToList();
                                //if (objRptSummary.Count > 0)
                                //{
                                //    saSummary.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRptSummary.First().ReportObject);
                                //}

                                //saSummary.Parameters["Company"].Value = code;
                                //saSummary.Parameters["Company"].Visible = false;
                                //saSummary.Parameters["InMonth"].Value = Objmatch.PayInMonth;
                                //saSummary.Parameters["InMonth"].Visible = false;
                                //saSummary.Parameters["UserName"].Value = "Humica";
                                //saSummary.Parameters["UserName"].Visible = false;

                                //// Create and save the fringe benefit report PDF
                                //string filSummaryfitName = $"PayrollSummary_{code}_{Objmatch.PayInMonth.ToString("MMMM-yyyy")}.pdf";
                                //string fileSummaryPath = Server.MapPath("~/Content/UPLOAD/" + filSummaryfitName);
                                //saSummary.ExportToPdf(fileSummaryPath);
                                //zip.CreateEntryFromFile(fileSummaryPath, filSummaryfitName);
                            }
                        }

                        if (!string.IsNullOrEmpty(Staff.Email))
                        {
                            URL += id;
                            wfo.ListObjectDictionary = new List<object>();
                            wfo.ListObjectDictionary.Add(Objmatch);
                            wfo.ListObjectDictionary.Add(Staff);
                            string Requester = user.UserName;
                            var _staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == Objmatch.Requestor);
                            if (_staff != null) Requester = _staff.AllName;

                            // Prepare the file paths for the ZIP
                            string[] filePaths = new string[] { zipFilePath };
                            string description = string.Format(
                                  "Dear {0} {1},<br />" +
                                  "I hope this email finds you well. I would like to request you preview and approve the payroll list for {2:MMMM-yyyy}.<br />" +
                                  "Please click on the link as below to log in for preview the payroll list.<br /><br />" +
                                  "Best regards,<br /><br />{3}<br /><br />" +
                                  "Please login at <a href='{4}'>URL</a>",
                                  Staff.Title, Staff.AllName, Objmatch.PayInMonth, Requester, URL
                              );

                            string body = description;
                            string subject = "Request Approval Salary First Pay";
                            WorkFlowResult result = wfo.ApproProcessWorkFlow(Staff, filePaths, subject, body);
                            wfMsg = wfo.getErrorMessage(result);
                        }
                        else
                        {
                            wfMsg = wfo.getErrorMessage(WorkFlowResult.EMAIL_NOT_SEND);
                        }
                    }
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
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
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
        public ActionResult Download(int PeriodID)
        {
            ActionName = "Index";
            UserSession();
            var report = DB.HisPendingAppSalaryFPs.FirstOrDefault(w => w.PeriodID == PeriodID);
            if (report != null)
            {
                RptMonthlyFirstPay sa = new RptMonthlyFirstPay();
                DateTime ToDate = new DateTime(report.ToDate.Year, report.ToDate.Month, 1);
                var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00017"
                     && w.IsActive == true).ToList();
                string ReportTypes = "Details";
                var reportHelper = new clsReportHelper();
                string path = reportHelper.Get_Path(SCREEN_ID, ReportTypes);
                if (!string.IsNullOrEmpty(path))
                {
                    sa.LoadLayoutFromXml(path);
                }
                sa.Parameters["Branch"].Value = SYConstant.Branch_Condition;
                sa.Parameters["Branch"].Visible = false;
                sa.Parameters["Period"].Value = PeriodID;
                sa.Parameters["Period"].Visible = false;

                string fileName = Server.MapPath("~/Content/UPLOAD/MonthlyFirstPay.xls");
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                if (!Directory.Exists(UploadDirectory))
                {
                    Directory.CreateDirectory(UploadDirectory);
                }
                sa.ExportToXls(fileName);
                //var _ReportStore = report.FirstOrDefault();
                //string name = _ReportStore.PathStore;
                //string FileSource = Server.MapPath(_ReportStore.PathStore);

                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=" + _ReportStore.ObjectName + ".repx");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.WriteFile(name);
                //Response.End();

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=MonthLyPay.xls");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.WriteFile(fileName);
                Response.End();
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            UserConfListAndForm();
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            string sms = BSM.CancelAppSalary(id);
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
        #region "Reject"
        public ActionResult Reject(string id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id != null)
            {
                PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RejectSalary(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    mess.Description = mess.Description + ". " + BSM.MessageCode;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);

        }
        #endregion
        #region "Ajax Approval"
        public ActionResult SelectParam(string docType)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(SCREEN_ID, docType);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridApproval", BSM.ListApproval);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        if (BSM.ListApproval.Count == 0)
                        {
                            ModelObject.ID = 1;
                        }
                        else
                        {
                            ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        }
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
            PRFirstPaySalaryGeneration BSM = new PRFirstPaySalaryGeneration();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRFirstPaySalaryGeneration)Session[Index_Sess_Obj + ActionName];
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
        #endregion
        private void DataList()
        {
            ViewData["DOCUMENT_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SYConstant.getBranchDataAccess();
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["PEROID_SELECT"] = DB.PRPayPeriodItems.ToList().OrderByDescending(w => w.EndDate);
            var objWf = new ExWorkFlowPreference();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, SCREEN_ID);
            // ViewData["PERIODID_SELECT"] = DB.PRpayperiods.OrderByDescending(w => w.PayPeriodId).ToList();

        }
    }
}
