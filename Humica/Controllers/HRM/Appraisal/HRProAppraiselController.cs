using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.SY;
using Humica.Performance;
using HUMICA.Models.Report.Appraisal;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRProAppraiselController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRA0000001";
        private const string URL_SCREEN = "/HRM/Appraisal/HRProAppraisel/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ApprID;Status";
        private string DOCTYPE = "APP01";
        protected IClsAppraisel BSM;
        IUnitOfWork unitOfWork;
        public HRProAppraiselController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsAppraisel();
            BSM.OnLoad();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
            }
            BSM.OnIndexLoading();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsAppraisel BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnIndexLoading();
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
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create(string EmpCode, string AppType, string HOD, string Appraiser, string Appraiser2)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.OnCreatingLoading(AppType,EmpCode, HOD, Appraiser, Appraiser2);
            Session[Index_Sess_Obj + ActionName] = BSM;
            BSM.DocType = DOCTYPE;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsAppraisel collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
                }
                BSM.DocType = DOCTYPE;
                BSM.ScreenId = SCREEN_ID;
                BSM.Header = collection.Header;
                string msg = BSM.Create();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject("MS001", user.Lang);
                    mess_err.DocumentNumber = BSM.Header.ApprID;
                    mess_err.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess_err.DocumentNumber + "&ApprID=" + collection.Header.ApprID;
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }
                else
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    if (!string.IsNullOrEmpty(BSM.ErrorMessage))
                    {
                        mess_err.DocumentNumber += " " + BSM.ErrorMessage;
                    }
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            BSM.OnDetailLoading(id);
            if (BSM.Header != null)
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
        [HttpPost]

        public ActionResult Edit(string id, ClsAppraisel collection)
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
                    BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Update(id);
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
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id != null)
            {
                string msg = BSM.Delete(id);
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
        #region Details
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            BSM.OnDetailLoading(id);
            if (BSM.Header != null)
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
        #endregion
        #region "Print"
        public ActionResult Print(string id)
        {
            ActionName = "Print";
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
            var SD = unitOfWork.Set<HREmpAppraisal>().FirstOrDefault(w => w.ApprID == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    var sa = new RptFromAppraisal();
                    var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
                   && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    sa.Parameters["AppraisalID"].Value = SD.ApprID;
                    sa.Parameters["AppraisalID"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = sa;
                    Session[Index_Sess_Obj] = sa;
                    return PartialView("PrintForm", sa);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    //log.DocurmentAction = ;
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

            if (Session[Index_Sess_Obj] != null)
            {
                RptFromAppraisal reportModel = (RptFromAppraisal)Session[Index_Sess_Obj];
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        public ActionResult ShowDataEmp(string EmpCode)
        {
            var EmpStaff = unitOfWork.Set<HR_STAFF_VIEW>().FirstOrDefault(w => w.EmpCode == EmpCode);
            var Emp_Staff = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpCode);
            var HOD = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == EmpStaff.HODCode);
            if (HOD == null) HOD = new HRStaffProfile();
            var Line = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Emp_Staff.APPAppraisal);
            if (Line == null) Line = new HRStaffProfile();
            var SecondLine = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == Emp_Staff.APPAppraisal2);
            if (SecondLine == null) SecondLine = new HRStaffProfile();
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    DEPT = EmpStaff.Department,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate,
                    HODCode = EmpStaff.HODCode,
                    HODName = HOD.AllName,
                    LineCode = Line.EmpCode,
                    LineName = Line.AllName,
                    SecondCode = SecondLine.EmpCode,
                    SecondName = SecondLine.AllName,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #region "Import"
        //public ActionResult Import()
        //{
        //    UserSession();
        //    ActionName = "Import";
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRProAppraisel", SYSConstant.DEFAULT_UPLOAD_LIST);

        //    var objStaff = new ClsAppraisel();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        objStaff = (ClsAppraisel)Session[Index_Sess_Obj + ActionName];
        //        if (objStaff.ListScore != null)
        //        {
        //            objStaff.ListScore.Clear();
        //        }
        //        if (objStaff.ListHeader != null)
        //        {
        //            objStaff.ListHeader.Clear();
        //        }
        //    }

        //    objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

        //    if (objStaff.ListTemplate.Count > 0)
        //    {
        //        SYExcel excel = new SYExcel();
        //        foreach (var read in objStaff.ListTemplate.ToList())
        //        {
        //            excel.FileName = read.UpoadPath;
        //        }


        //        DataTable dtHeader = excel.GenerateExcelData();
        //        objStaff.ListScore = new List<HREmpAppraisalItem>();
        //        objStaff.ListHeader = new List<HREmpAppraisal>();
        //        if (dtHeader != null)
        //        {
        //            for (int i = 0; i < dtHeader.Rows.Count; i++)
        //            {
        //                var objHeader = new HREmpAppraisalItem();
        //                objHeader.Code = dtHeader.Rows[i][1].ToString();
        //                objHeader.Description = dtHeader.Rows[i][2].ToString();
        //                objHeader.SecDescription = dtHeader.Rows[i][3].ToString();
        //                objHeader.Score = (int?)SYSettings.getNumberValue(dtHeader.Rows[i][5].ToString());

        //                objStaff.ListScore.Add(objHeader);

        //            }
        //        }


        //    }


        //    Session[Index_Sess_Obj + ActionName] = objStaff;
        //    return View(objStaff);
        //}

        //[HttpPost]
        //public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        //{

        //    UserSession();
        //    ActionName = "Import";
        //    this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRProAppraisel", SYSConstant.DEFAULT_UPLOAD_LIST);
        //    SYFileImport sfi = new SYFileImport(DBV.CFUploadPaths.Find("EMPLOYEE"));
        //    sfi.ObjectTemplate = new MDUploadTemplate();
        //    sfi.ObjectTemplate.UploadDate = DateTime.Now;
        //    sfi.ObjectTemplate.ScreenId = SCREEN_ID;
        //    sfi.ObjectTemplate.UploadBy = user.UserName;
        //    sfi.ObjectTemplate.Module = "HR";
        //    sfi.ObjectTemplate.IsGenerate = false;

        //    UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
        //        sfi.ValidationSettings,
        //        sfi.uc_FileUploadComplete);


        //    var objStaff = new ClsAppraisel();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        objStaff = (ClsAppraisel)Session[Index_Sess_Obj + ActionName];
        //    }


        //    objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
        //    objStaff.ListScore = new List<HREmpAppraisalItem>();
        //    objStaff.ListHeader = new List<HREmpAppraisal>();


        //    Session[Index_Sess_Obj + ActionName] = objStaff;
        //    return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        //    //return null;
        //}
        //public ActionResult UploadList()
        //{
        //    UserSession();
        //    ActionName = "Import";
        //    this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
        //    UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRProAppraisel", SYSConstant.DEFAULT_UPLOAD_LIST);

        //    var objStaff = new ClsAppraisel();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        objStaff = (ClsAppraisel)Session[Index_Sess_Obj + ActionName];
        //    }


        //    objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
        //    objStaff.ListScore = new List<HREmpAppraisalItem>();
        //    objStaff.ListHeader = new List<HREmpAppraisal>();


        //    Session[Index_Sess_Obj + ActionName] = objStaff;
        //    return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        //}

        //[HttpGet]
        //public ActionResult GenerateUpload(int id)
        //{
        //    UserSession();
        //    MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
        //    if (obj != null)
        //    {
        //        var DBB = new HumicaDBContext();
        //        SYExcel excel = new SYExcel();
        //        excel.FileName = obj.UpoadPath;
        //        DataTable dtHeader = excel.GenerateExcelData();
        //        //DataTable dtItem = excel.GenerateExcelData(2);
        //        var objStaff = new ClsAppraisel();
        //        objStaff.Header = new HREmpAppraisal();
        //        //objStaff.c = CompanyCode;
        //        if (obj.IsGenerate == true)
        //        {
        //            SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
        //            Session[SYSConstant.MESSAGE_SUBMIT] = mess;
        //            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        //        }
        //        if (dtHeader != null)
        //        {
        //            try
        //            {
        //                string msg = SYConstant.OK;
        //                if (dtHeader.Rows.Count > 0) // Header
        //                {
        //                    objStaff.ListHeader = new List<HREmpAppraisal>();

        //                    var objHeader = new HREmpAppraisal();
        //                    objHeader.EmpCode = dtHeader.Rows[0][1].ToString();
        //                    //objHeader.Division = dtHeader.Rows[i][2].ToString();
        //                    objHeader.Position = dtHeader.Rows[2][1].ToString();
        //                    //objHeader.DateOfHire = SYSettings.getDateValue(dtHeader.Rows[i][4].ToString());
        //                    objHeader.AppraisalType = dtHeader.Rows[1][1].ToString();
        //                    objHeader.AppraiserDate = SYSettings.getDateValue(dtHeader.Rows[3][1].ToString());

        //                    objStaff.Header.EmpCode = objHeader.EmpCode;
        //                    objStaff.ListHeader.Add(objHeader);

        //                }

        //                DateTime create = DateTime.Now;
        //                if (dtHeader.Rows.Count > 0)  //Item
        //                {
        //                    //objStaff.ListHeader = new List<HREmpAppraisal>();
        //                    objStaff.ListScore = new List<HREmpAppraisalItem>();
        //                    //objStaff.ListPlanItem = new List<HLContractPlanItem>();
        //                    for (int i = 6; i < dtHeader.Rows.Count; i++)
        //                    {
        //                        var objHeader = new HREmpAppraisalItem();
        //                        objHeader.Code = dtHeader.Rows[i][0].ToString();
        //                        objHeader.Description = dtHeader.Rows[i][1].ToString();
        //                        objHeader.SecDescription = dtHeader.Rows[i][2].ToString();
        //                        objHeader.Score = (int?)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());

        //                        objStaff.ListScore.Add(objHeader);

        //                    }

        //                    msg = objStaff.uploadStaff();
        //                    if (msg == SYConstant.OK)
        //                    {
        //                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("IMPORTED", user.Lang);

        //                        if (obj != null)
        //                        {
        //                            DB.MDUploadTemplates.Remove(obj);
        //                            DB.SaveChanges();

        //                            if (System.IO.File.Exists(obj.UpoadPath))
        //                            {
        //                                System.IO.File.Delete(obj.UpoadPath);
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        //Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessage(msg, user.Lang) + "(" + objStaff.ErrorMessage + ")";
        //                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        //                    }


        //                }
        //            }
        //            catch (DbUpdateException e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = SCREEN_ID;
        //                log.UserId = user.UserID.ToString();
        //                log.DocurmentAction = "UPLOAD";
        //                log.Action = SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                /*----------------------------------------------------------*/
        //                obj.Message = e.Message;
        //                obj.IsGenerate = false;
        //                DB.MDUploadTemplates.Attach(obj);
        //                DB.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DB.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                /*------------------SaveLog----------------------------------*/
        //                SYEventLog log = new SYEventLog();
        //                log.ScreenId = SCREEN_ID;
        //                log.UserId = user.UserID.ToString();
        //                log.DocurmentAction = "UPLOAD";
        //                log.Action = SYActionBehavior.ADD.ToString();

        //                SYEventLogObject.saveEventLog(log, e, true);
        //                obj.Message = e.Message;
        //                obj.IsGenerate = false;
        //                DB.MDUploadTemplates.Attach(obj);
        //                DB.Entry(obj).Property(w => w.Message).IsModified = true;
        //                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
        //                DB.SaveChanges();
        //            }
        //        }

        //    }

        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        //}

        //public ActionResult DownloadTemplate()
        //{
        //    string fileName = Server.MapPath("~/Content/TEMPLATE/Appraisel_List.xlsx");
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment;filename=EMPLOYEE_TEMPLATE.xlsx");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.WriteFile(fileName);
        //    Response.End();
        //    return null;
        //}

        #endregion

        #region Action
        public ActionResult RequestForApproval(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.RequestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Cancel(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CancelDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("CANCELLED", user.Lang);
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
        public ActionResult Closed(string id)
        {
            UserSession();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CloseDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("CLOSED_EN", user.Lang);
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
        public ActionResult RatingChange(string Code, string Value, string Region, string Action,int Step)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];
                if (string.IsNullOrEmpty(Value)) Value = "0";
                int Rating = Convert.ToInt32(Value);
                if (BSM.RateMax < Rating) Rating = BSM.RateMax;
                var checkexist = BSM.ListScore.Where(w => w.Code == Code && w.Region == Region).ToList();
                if (checkexist != null)
                {
                    checkexist.First().RatingID = Rating;
                    if (Step == 1)
                    {
                        checkexist.First().ScoreAppraiser = Rating;
                    }
                    else
                    {
                        checkexist.First().ScoreAppraiser2 = Rating;
                    }
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    Scoring = Rating
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult CommentValue(string Code, string Comment, string Region, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsAppraisel)Session[Index_Sess_Obj + ActionName];

                var checkexist = BSM.ListScore.Where(w => w.Code == Code).ToList();
                if (checkexist.Count == 0)
                {
                    var obj = new HREmpAppraisalItem();
                    obj.Code = Code;
                    obj.Region = Region;
                    obj.Comment = Comment;
                    BSM.ListScore.Add(obj);
                }
                else
                {
                    checkexist.First().Code = Code;
                    checkexist.First().Comment = Comment;
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