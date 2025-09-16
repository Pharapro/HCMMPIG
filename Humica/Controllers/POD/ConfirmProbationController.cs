using Humica.EF.Models.SY;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humica.EF;
using SunPeople.Controllers;
using Humica.Logic.PR;
using SunPeople.Models.SY;
using SunPeople.Models.Report.Payroll;
using System.IO;
using System;
using Humica.POD.Models.HR;
using Humica.POD.Models;
using Humica.POD;
using DevExpress.Web.Mvc;
using HUMICA.Models.Report.Payroll;
using Humica.EF.MD;
using HUMICA.Models.Report.HRM;
using Humica.Logic.MD;
using Humica.Core.Process;
using Humica.Models.SY;

namespace HR.Controllers.POD
{

    public class ConfirmProbationController : MasterSaleController
    {

        private const string SCREEN_ID = "POD0000001";
        private const string URL_SCREEN = "/POD/ConfirmProbation/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _LOCATION_ = "_LOCATION_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        PODEnity DB= new PODEnity();
        DBBusinessProcess DBX = new DBBusinessProcess();
        DBSystemHREntities DBI = new DBSystemHREntities();
        public FRM_probation FRM_probation { get; private set; }

        public ConfirmProbationController()
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

            ConfirmProbationObject BSM = new ConfirmProbationObject();
            BSM.ListHeader = new List<HRComfirmProbation>();
            BSM.ListHeader = DB.HRComfirmProbations.ToList();
            //BSM.ListApprove = DBX.ExDocApprovals.Where(w => w.DocNo == BSM.Header.CPID).ToList();
            var listStaff = DBX.HRStaffProfiles.ToList();
            var listPro = BSM.ListHeader.ToList();
            //listStaff = listStaff.Where(x=>listPro.Where(w=>w.EmpCode !=x.EmpCode).Any()).ToList();
            listStaff= listStaff.Where(w => listPro.All(x => x.EmpCode != w.EmpCode) && w.CareerDesc=="NEWJOIN" && w.Status=="A").ToList();
            //BSM.ListStaff = null;
            BSM.ListStaff = listStaff;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[_DOCTYPE_] = "";
            Session[_LOCATION_] = "";
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }


        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            BSM.ListHeader = new List<HRComfirmProbation>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(this.KeyName);
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialProcess", BSM.ListStaff);
        }
        #endregion
        #region "Create"
        public ActionResult Create(string EmpCode)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (EmpCode != null)
            {
                ConfirmProbationObject BSM = new ConfirmProbationObject();
                BSM.Header = new HRComfirmProbation();
                BSM.ListHeader = new List<HRComfirmProbation>();
                BSM.ListStaff = new List<HRStaffProfile>();
                BSM.ListApprove = new List<ExDocApproval>();
                BSM.Staff = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
                if (BSM.Staff != null)
                {
                    //BSM.Staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    BSM.Header.EmpCode = BSM.Staff.EmpCode;
                    BSM.Header.EmpName = BSM.Staff.AllName;
                    BSM.Header.Branch = BSM.Staff.Branch;
                    BSM.Header.Position = BSM.Staff.JobCode;
                    BSM.Header.Department = BSM.Staff.DEPT;
                    BSM.Header.StartDate = BSM.Staff.StartDate;
                    BSM.Header.Probation = BSM.Staff.Probation;
                    BSM.Header.LevelCode = BSM.Staff.LevelCode;
                    BSM.Header.Salary = BSM.Staff.Salary;
                    //BSM.Header.DocType = "SALARY";
                    BSM.Header.Status= SYDocumentStatus.OPEN.ToString();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Create(string EmpCode, ConfirmProbationObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EmpCode;
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (EmpCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.Attach_File = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.Header = collection.Header;
                BSM.Staff = collection.Staff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateProbation(EmpCode);
                if (msg == SYConstant.OK)
                {
                    DB = new PODEnity();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    //mess.DocumentNumber = Convert.ToInt32(id).ToString();
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
        #region Edit
        public ActionResult Edit(string EmpCode)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EmpCode;

            if (EmpCode != null)
            {
                ConfirmProbationObject BSM = new ConfirmProbationObject();
                BSM.Header = new HRComfirmProbation();
                //int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HRComfirmProbations.FirstOrDefault(w => w.EmpCode == EmpCode);
                BSM.ListApprove = DBX.ExDocApprovals.Where(w => w.DocumentNo ==BSM.Header.CPID).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string EmpCode, HRComfirmProbation collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EmpCode;
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (EmpCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection;
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Attach_File = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    collection.Attach_File = BSM.Header.Attach_File;
                }
                BSM.ScreenId = SCREEN_ID;
                //int TranNo = ;
                string msg = BSM.EditProbation(EmpCode);
                if (msg == SYConstant.OK)
                {
                    DB = new PODEnity();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    //mess.DocumentNumber = Convert.ToInt32(id).ToString();
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
        #region Delete
        public ActionResult Delete(string EmpCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (EmpCode != null||EmpCode!="")
            {
                ConfirmProbationObject Del = new ConfirmProbationObject();
                string msg = Del.DeleteProbation(EmpCode);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Request
        public ActionResult RequestForApproval(string EmpCode)
        {
            UserSession();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (EmpCode != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(EmpCode);
                if (msg == SYConstant.OK)
                {
                    
                    /*------------------WorkFlow--------------------------------*/
                    var excfObject = DB.ExCfWorkFlowItems.Find(SCREEN_ID, "SALARY");
                    string wfMsg = "";
                    string URL = SYUrl.getBaseUrl() + "/POD/ConfirmProbation/Details/";
                    URL += BSM.Header.EmpCode;
                    if (excfObject != null)
                    {
                        SYWorkFlowEmailObject wfo =
                            new SYWorkFlowEmailObject(excfObject.ApprovalFlow, WorkFlowType.REQUESTER,
                                 UserType.N, BSM.Header.Status);
                        wfo.SelectListItem = new SYSplitItem(EmpCode);
                        wfo.User = BSM.User;
                        wfo.BS = BSM.BS;
                        wfo.UrlView = SYUrl.getBaseUrl();
                        wfo.ScreenId = SCREEN_ID;
                        wfo.Action = SYDocumentStatus.PENDING.ToString();
                        HRStaffProfile Staff = BSM.getNextApprover(BSM.Header.EmpCode, "");
                        var _staff = DBX.HRStaffProfiles.Where(w => w.EmpCode == EmpCode).Select(x => x.HODCode);
                        var _HOD = DBX.HRStaffProfiles.FirstOrDefault(w => _staff.Contains(w.EmpCode));
                        FRM_WorkerPayRoll sa = new FRM_WorkerPayRoll();
                        var objRpt = DBX.CFReportObjects.Where(w => w.ScreenID == "POD0000001"
                            && w.DocType == "SALARY" ).ToList();
                        if (objRpt.Count > 0)
                        {
                            sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                        }
                        Session[this.Index_Sess_Obj + this.ActionName] = sa;
                        string fileName = Server.MapPath("~/Content/TEMPLATE/" + EmpCode + ".pdf");
                        string UploadDirectory = Server.MapPath("~/Content/TEMPLATE");
                        if (!Directory.Exists(UploadDirectory))
                        {
                            Directory.CreateDirectory(UploadDirectory);
                        }
                        
                    }
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = EmpCode;
                    mess.Description = mess.Description + wfMsg;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + EmpCode;
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
        public ActionResult Approve(string CPID,string EMPCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }

            if (CPID != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string URL = SYUrl.getBaseUrl() + "/POD/ConfirmProbation/Details/";
                string msg = BSM.ApproveCP(CPID, URL);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    //mess.Description = mess.Description + ". " + BSM.MessageError;
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
        #endregion Approve
        #region "Print"
        public ActionResult Print(string CPID)
        {
            ActionName = "Print";
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = CPID;
            UserMVCReportView();
            return View("ReportView");
        }

        public ActionResult DocumentViewerPartial(string CPID)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = DB.HRComfirmProbations.FirstOrDefault(w => w.CPID == CPID);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = CPID;
                    //string Company = DB.SYHRCompanies.FirstOrDefault().CompENG;
                    FRM_probation reportModel = new FRM_probation();

                    reportModel.Parameters["CPID"].Value = SD.CPID;
                    reportModel.Parameters["CPID"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = CPID;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string CPID)
        {
            ActionName = "Print";
            var SD = DB.HRComfirmProbations.FirstOrDefault(w => w.CPID == CPID);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = CPID;
            if (SD != null)
            {
                FRM_probation reportModel = new FRM_probation();
                reportModel.Parameters["CPID"].Value = SD.CPID;
                reportModel.Parameters["CPID"].Visible = false;
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        #region "Cancel"
        public ActionResult Cancel(string EmpCode)
        {
            this.UserSession();
            UserConfListAndForm();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            string sms = BSM.CancelAppSalary(EmpCode);
            if (sms == SYConstant.OK)
            {
                var mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                mess.Description = mess.Description + ". ";
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Detail
        public ActionResult Details(string EmpCode)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = EmpCode;

            if (EmpCode != null)
            {
                ConfirmProbationObject BSM = new ConfirmProbationObject();
                BSM.Header = new HRComfirmProbation();
                //int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.HRComfirmProbations.FirstOrDefault(w => w.EmpCode == EmpCode);
                BSM.ListApprove = DBX.ExDocApprovals.Where(w => w.DocumentNo == BSM.Header.CPID).ToList();
                //BSM.HeaderStaff = DB.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion Detail
        public ActionResult GridItemApprove()
        {

            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();

            return PartialView("GridItemApprove", BSM.ListApprove);
        }

        public ActionResult GridItemApproveD()
        {

            ActionName = "Edits";
            UserSession();
            UserConfListAndForm();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();

            return PartialView("GridItemApproveDetail", BSM.ListApprove);
        }
        public ActionResult GridItemApproveEDIT()
        {

            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            return PartialView("GridItemApproveEdit", BSM.ListApprove);
        }
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        if (BSM.ListApprove.Count == 0)
                        {
                            ModelObject.ID = 1;
                        }
                        else
                        {
                            ModelObject.ID = BSM.ListApprove.Max(w => w.ID) + 1;
                        }
                        //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                        ModelObject.DocumentNo = "N/A";
                        BSM.ListApprove.Add(ModelObject);
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
            //DataList();

            return PartialView("GridItemApprove", BSM.ListApprove);
        }
        

        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DBX.CFUploadPaths.Find("IMG_UPLOAD");
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
        public ActionResult ShowDataEmp(string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DB.HRComfirmProbations;
            var EmpStaff = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    EmpName = EmpStaff.AllName,
                    Branch = EmpStaff.Branch,
                    DIV = EmpStaff.Division,
                    Dep = EmpStaff.DEPT,
                    Pos = EmpStaff.JobCode,
                    Level = EmpStaff.LevelCode,
                    StartDate = EmpStaff.StartDate,
                    Pro = EmpStaff.Probation,

                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult SelectParam(string docType , string location)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            var rs = new { MS = SYConstant.OK };
            ActionName = "Create";
            ConfirmProbationObject BSM = new ConfirmProbationObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ConfirmProbationObject)Session[Index_Sess_Obj + ActionName];
                //BSM.SetAutoApproval(SCREEN_ID, docType);
                BSM.SetAutoApproval(docType, location, SCREEN_ID);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData["STAFF_SELECT"] = DBX.HRStaffProfiles.ToList();
            ViewData["POST_SELECT"] = DBI.HRPositions.ToList();
            ViewData["DOCUMENT_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            var objWf = new Humica.POD.Models.ExWorkFlowPreference();
            var location ="";

            if (Session[_LOCATION_] != null)
            {
                location = Session[_LOCATION_].ToString();
            }
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location, SCREEN_ID);

        }
    }
}
    