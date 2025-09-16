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

namespace HR.Controllers.PR.PRM
{

    public class PRApproveSalaryController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000022";
        private const string URL_SCREEN = "/PR/PRM/PRApproveSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ASNumber;Status";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        HumicaDBContext DB = new HumicaDBContext();
        public PRApproveSalaryController()
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
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            BSM.Filter = new Humica.Core.FT.FTFilerPayroll();
            BSM.ListApproveSalary = new List<HISApproveSalary>();
            BSM.ListSalaryApprove= new List<SalaryApproval>();           
            IEnumerable<ExCfWFSalaryApprover> ListApp = DB.ExCfWFSalaryApprovers.ToList();
            IEnumerable<HisPendingAppSalary> ListOTRequest = DB.HisPendingAppSalaries.ToList();
            var Statuses = new List<string>
            {
                SYDocumentStatus.OPEN.ToString(),
                SYDocumentStatus.PENDING.ToString(),
                SYDocumentStatus.APPROVED.ToString(),
            };

            BSM.ListApproveSalary = DB.HISApproveSalaries.ToList();
            BSM.ListAppSalaryPending = DB.HisPendingAppSalaries.AsEnumerable().Where(w => w.IsLock == false && !BSM.ListApproveSalary.Where(x => x.PeriodID == w.PeriodID 
                && Statuses.Contains(x.Status)).Any()).ToList();
            var staff = DB.HISGenSalaries.FirstOrDefault(w => w.EmpCode == user.UserName);
            ListApp = ListApp.Where(w => w.Employee == user.UserName).ToList();
            foreach (var item in ListOTRequest)
            {
                if (ListApp.Count() == 0)
                {
                    SalaryApproval _OT = new SalaryApproval();
                    _OT.PayPeriodID = item.PeriodID;
                    _OT.FromDate = item.FromDate;
                    _OT.ToDate = item.ToDate;
                    _OT.IsLock = item.IsLock;
                    BSM.ListSalaryApprove.Add(_OT);
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
            DataList();
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            BSM.ListApproveSalary = new List<HISApproveSalary>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("List", BSM.ListApproveSalary);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            DataList();
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
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
            StaffSelect();
            DataList();
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            var SalaryPending = DB.HisPendingAppSalaries.Where(w => w.IsLock == false && w.PeriodID == PeriodID).ToList();

            if (SalaryPending.Count > 0)
            {
                ViewData[SYConstant.PARAM_ID] = PeriodID;
                if (PeriodID == null)
                {
                    DataList();
                    BSM.HeaderAppSalary = new HISApproveSalary();
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.HeaderAppSalary.DocumentDate = DateTime.Now;
                }
                else
                {
                    BSM.HeaderAppSalary = new HISApproveSalary();
                    BSM.HeaderAppSalary.PeriodID = PeriodID;
                    if (user.UserName != null)
                        BSM.HeaderAppSalary.Requestor = user.UserName;
                    BSM.HeaderAppSalary.Status = SYDocumentStatus.OPEN.ToString();
                    BSM.HeaderAppSalary.DocumentDate = DateTime.Now.Date;
                    BSM.ListApproval = new List<ExDocApproval>();
                    BSM.ScreenId = SCREEN_ID;
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
        public ActionResult Create(PRGenerate_Salary BSM)
        {
            UserSession();
            //DataSelector();
            UserConfForm(SYActionBehavior.ADD);

            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
                BSM.ListApproval = obj.ListApproval;
            }
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateAppSalary();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderAppSalary.PeriodID.ToString();
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
        public ActionResult Details(string PeriodID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = PeriodID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (PeriodID == "null") PeriodID = null;
            if (PeriodID != null)
            {
                PRGenerate_Salary BSM = new PRGenerate_Salary();
                int ID = Convert.ToInt32(PeriodID);
                BSM.HeaderAppSalary = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == ID);
                if (BSM.HeaderAppSalary != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == BSM.HeaderAppSalary.ASNumber && w.DocumentType == BSM.HeaderAppSalary.DocumentType).ToList();

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Request
        public ActionResult RequestForApproval(string PeriodID)
        {
            UserSession();
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (PeriodID == "null") PeriodID = null;
            if (PeriodID != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(PeriodID);
                if (msg == SYConstant.OK)
                {
                    int ID = Convert.ToInt32(PeriodID);
                    var Objmatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == ID);
                    if (Objmatch != null)
                    {
                        /*------------------WorkFlow--------------------------------*/
                        var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, BSM.HeaderAppSalary.DocumentType);
                        string wfMsg = "";
                        string URL = SYUrl.getBaseUrl() + "/PR/PRM/PRApproveSalary/Details/";
                        if (excfObject != null)
                        {
                            SYWorkFlowEmailObject wfo =
                                new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                     UserType.N, BSM.HeaderAppSalary.Status);
                            wfo.SelectListItem = new SYSplitItem(PeriodID);
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
                                    RptMonthlyPay sa = new RptMonthlyPay();
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
                                    sa.Parameters["Period"].Value = ID;
                                    sa.Parameters["Period"].Visible = false;
                                    sa.Parameters["UserName"].Value = "Humica";
                                    sa.Parameters["UserName"].Visible = false;

                                    Session[this.Index_Sess_Obj + this.ActionName] = sa;

                                    // Create and save the payroll report PDF
                                    string fileName = $"PayrollDetails_{code}_{Objmatch.PayInMonth.ToString("MMMM-yyyy")}.pdf";
                                    string filePath = Server.MapPath("~/Content/UPLOAD/" + fileName);
                                    sa.ExportToPdf(filePath);
                                    zip.CreateEntryFromFile(filePath, fileName);

                                    //Second report: Summary report
                                    //RptSummaryPay saSummary = new RptSummaryPay();
                                    //var objRptSummary = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00002" 
                                    //    && w.DocType == "Summary" && w.IsActive == true).ToList();
                                    //if (objRptSummary.Count > 0)
                                    //{
                                    //    saSummary.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRptSummary.First().ReportObject);
                                    //}

                                    //saSummary.Parameters["Company"].Value = code;
                                    //saSummary.Parameters["Company"].Visible = false;
                                    //saSummary.Parameters["Period"].Value = Objmatch.PayInMonth;
                                    //saSummary.Parameters["Period"].Visible = false;
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
                                URL += PeriodID;
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
                                string subject = "Request Approval Salary";
                                WorkFlowResult result = wfo.ApproProcessWorkFlow(Staff, filePaths, subject, body);
                                wfMsg = wfo.getErrorMessage(result);
                            }
                            else
                            {
                                wfMsg = wfo.getErrorMessage(WorkFlowResult.EMAIL_NOT_SEND);
                            }
                        }
                        var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                        mess.DocumentNumber = PeriodID;
                        mess.Description = mess.Description + wfMsg;
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?PeriodID=" + PeriodID;
                        Session[Index_Sess_Obj + ActionName] = null;
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approve
        public ActionResult Approve(string PeriodID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (PeriodID == "null") PeriodID = null;
            if (PeriodID != null)
            {
                PRGenerate_Salary BSM = new PRGenerate_Salary();
                int ID = Convert.ToInt32(PeriodID);
                var Objmatch = DB.HISApproveSalaries.FirstOrDefault(w => w.PeriodID == ID);
                BSM.ScreenId = SCREEN_ID;
                string URL = SYUrl.getBaseUrl() + "/PR/PRM/PRApproveSalary/Details/";
                try
                {
                    // Create upload directory if it doesn't exist
                    string uploadDirectory = Server.MapPath("~/Content/UPLOAD");
                    if (!Directory.Exists(uploadDirectory))
                    {
                        Directory.CreateDirectory(uploadDirectory);
                    }


                    // Call the method to approve the document
                    string msg = BSM.approveTheDoc(ID); // Assuming you want to send the zip file

                    // Handle the response message
                    if (msg == SYConstant.OK)
                    {
                        // Create a zip file
                        string zipFileName = Path.Combine(uploadDirectory, $"{PeriodID}_Reports.zip");
                        using (var zip = new ZipArchive(new FileStream(zipFileName, FileMode.Create), ZipArchiveMode.Create))
                        {
                            // Loop through each company in SYConstant.Company_Condition
                            string[] Company = SYConstant.Company_Condition.Split(',');
                            foreach (var code in Company)
                            {
                                if (string.IsNullOrEmpty(code)) continue;
                                // First report: Details report
                                RptMonthlyPay saDetails = new RptMonthlyPay();
                                var objRptDetails = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00002" && w.DocType == "Details" && w.IsActive == true).ToList();
                                string ReportTypes = "Details";
                                var reportHelper = new clsReportHelper();
                                string path = reportHelper.Get_Path(SCREEN_ID, ReportTypes);
                                if (!string.IsNullOrEmpty(path))
                                {
                                    saDetails.LoadLayoutFromXml(path);
                                }

                                saDetails.Parameters["Company"].Value = code;
                                saDetails.Parameters["Company"].Visible = false;
                                saDetails.Parameters["Period"].Value = ID;
                                saDetails.Parameters["Period"].Visible = false;
                                saDetails.Parameters["UserName"].Value = "Humica";
                                saDetails.Parameters["UserName"].Visible = false;

                                // Create the details report file
                                string filePayroll = Path.Combine(uploadDirectory, $"PayrollDetails_{code}_{Objmatch.PayInMonth:MMMM-yyyy}.pdf");
                                saDetails.ExportToPdf(filePayroll);
                                zip.CreateEntryFromFile(filePayroll, Path.GetFileName(filePayroll));

                                //RptSummaryPay saSummary = new RptSummaryPay();
                                //var objRptSummary = DB.CFReportObjects.Where(w => w.ScreenID == "RPTPR00002"
                                //    && w.DocType == "Summary" && w.IsActive == true).ToList();
                                //if (objRptSummary.Count > 0)
                                //{
                                //    saSummary.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRptSummary.First().ReportObject);
                                //}

                                //saSummary.Parameters["Company"].Value = code;
                                //saSummary.Parameters["Company"].Visible = false;
                                //saSummary.Parameters["Period"].Value = Objmatch.PayInMonth;
                                //saSummary.Parameters["Period"].Visible = false;
                                //saSummary.Parameters["UserName"].Value = "Humica";
                                //saSummary.Parameters["UserName"].Visible = false;

                                //// Create and save the fringe benefit report PDF
                                //string filSummaryfitName = $"PayrollSummary_{code}_{Objmatch.PayInMonth.ToString("MMMM-yyyy")}.pdf";
                                //string fileSummaryPath = Server.MapPath("~/Content/UPLOAD/" + filSummaryfitName);
                                //saSummary.ExportToPdf(fileSummaryPath);
                                //zip.CreateEntryFromFile(fileSummaryPath, filSummaryfitName);
                            }
                        }
                        BSM.SendEmail(ID, zipFileName, BSM.ApproveLevel, URL);
                        var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                        mess.Description += ". " + BSM.MessageError;
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                }
                catch (IOException ex)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("FILE_IN_USE", user.Lang);
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("UNKNOWN_ERROR", user.Lang);
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Reject"
        public ActionResult Reject(string PeriodID)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (PeriodID == "null") PeriodID = null;
            if (PeriodID != null)
            {
                PRGenerate_Salary BSM = new PRGenerate_Salary();
                BSM.ScreenId = SCREEN_ID;
                int ID = Convert.ToInt32(PeriodID);
                string msg = BSM.RejectSalary(ID);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_REJECT", user.Lang);
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
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult Download(int PeriodID)
        {
            ActionName = "Index";
            UserSession();
            var report = DB.HisPendingAppSalaries.FirstOrDefault(w => w.PeriodID == PeriodID);

            if (report != null)
            {
                DateTime ToDate = new DateTime(report.ToDate.Year, report.ToDate.Month, 1);
                string zipFileName = Server.MapPath("~/Content/UPLOAD/MonthlyPayReports.zip");
                string uploadPath = Server.MapPath("~/Content/UPLOAD");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string[] Company = SYConstant.Company_Condition.Split(',');

                using (var zip = new ZipArchive(new FileStream(zipFileName, FileMode.Create), ZipArchiveMode.Create))
                {
                    foreach (var code in Company)
                    {
                        if (string.IsNullOrEmpty(code)) continue;
                        RptMonthlyPay sa = new RptMonthlyPay();
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
                        sa.Parameters["Period"].Value = PeriodID;
                        sa.Parameters["Period"].Visible = false;
                        sa.Parameters["UserName"].Value = user.UserName;
                        sa.Parameters["UserName"].Visible = false;

                        Session[this.Index_Sess_Obj + this.ActionName] = sa;

                        // Create and save the payroll report PDF
                        string fileName = $"{code}_PayrollDetails_{ToDate.ToString("MMMM-yyyy")}.pdf";
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
                        //saSummary.Parameters["Period"].Value = ToDate;
                        //saSummary.Parameters["Period"].Visible = false;
                        //saSummary.Parameters["UserName"].Value = user.UserName;
                        //saSummary.Parameters["UserName"].Visible = false;

                        //// Create and save the fringe benefit report PDF
                        //string filSummaryfitName = $"{code}_PayrollSummary_{ToDate.ToString("MMMM-yyyy")}.pdf";
                        //string fileSummaryPath = Server.MapPath("~/Content/UPLOAD/" + filSummaryfitName);
                        //saSummary.ExportToPdf(fileSummaryPath);
                        //zip.CreateEntryFromFile(fileSummaryPath, filSummaryfitName);
                    }
                }

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/zip";
                Response.AddHeader("content-disposition", "attachment;filename=MonthlyPayReports.zip");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.WriteFile(zipFileName);
                Response.End();
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #region "Cancel"
        public ActionResult Cancel(string PeriodID)
        {
            this.UserSession();
            UserConfListAndForm();
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            int ID = Convert.ToInt32(PeriodID);
            string sms = BSM.CancelAppSalary(ID);
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
        #region "Ajax Approval"
        public ActionResult SelectParam(string docType)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
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
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
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
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
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
            PRGenerate_Salary BSM = new PRGenerate_Salary();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRGenerate_Salary)Session[Index_Sess_Obj + ActionName];
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
            var objWf = new ExWorkFlowPreference();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, SCREEN_ID);
            ViewData["PERIODID_SELECT"] = DB.PRPayPeriodItems.OrderByDescending(w => w.PeriodID).ToList();

        }
        private void StaffSelect()
        {
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
        }
    }
}
