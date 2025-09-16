using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PREmpWorkingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000030";
        private const string URL_SCREEN = "/PR/PRM/PREmpWorking/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();
        public new SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }

        public PREmpWorkingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);

            PREmpWorkingObject BSM = new PREmpWorkingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
            }
            var ListHeaders = DB.PREmpWorkings.ToList();
            BSM.ListHeader = ListHeaders.OrderBy(w => w.EmpCode).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            PREmpWorkingObject BSM = new PREmpWorkingObject();
            BSM.ListHeader = new List<PREmpWorking>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader.OrderBy(w => w.EmpCode).ToList());
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PREmpWorkingObject BSM = new PREmpWorkingObject();
            BSM.Header = new PREmpWorking();
            BSM.Header.PayPeriodId = 1;
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;
            BSM.ListHeader = new List<PREmpWorking>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PREmpWorkingObject collection)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ActionName = "Create";
            PREmpWorkingObject BSM = new PREmpWorkingObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                    //BSM.Staff = collection.Staff;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateEmp();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM = new PREmpWorkingObject();
                    BSM.Header = new PREmpWorking();
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

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
            if (ID == "null") ID = null;
            if (ID != null)
            {
                PREmpWorkingObject BSM = new PREmpWorkingObject();
                BSM.Header = new PREmpWorking();
                //BSM.Header = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.PREmpWorkings.SingleOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    //BSM.Header = DB..SingleOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, PREmpWorkingObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PREmpWorkingObject BSM = new PREmpWorkingObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                int TranNo = Convert.ToInt32(id);
                string msg = BSM.EditEmp(TranNo);
                if (msg == SYConstant.OK)
                {
                    DB = new HumicaDBContext();
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
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id.ToString() != "null")
            {
                PREmpWorkingObject Del = new PREmpWorkingObject();
                string msg = Del.DeleteEmp(id);
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
        //public ActionResult GridItem()
        //{
        //    ActionName = "Import";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataSelector();
        //    PREmpWorkingObject objStaff = new PREmpWorkingObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        objStaff = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
        //        if (objStaff.listImport != null)
        //        {
        //            objStaff.listImport.Clear();
        //        }
        //    }
        //    if (objStaff.ListTemplate.Count > 0)
        //    {
        //        SYExcel excel = new SYExcel();
        //        foreach (var read in objStaff.ListTemplate.ToList())
        //        {
        //            excel.FileName = read.UpoadPath;
        //        }


        //        DataTable dtHeader = excel.GenerateExcelData();
        //        objStaff.listImport = new List<PREmpWorking>();

        //        if (dtHeader != null)
        //        {
        //            for (int i = 0; i < dtHeader.Rows.Count; i++)
        //            {
        //                var objHeader = new PREmpWorking();
        //                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
        //                objHeader.InYear = Convert.ToInt32(dtHeader.Rows[i][2].ToString());
        //                objHeader.InMonth = Convert.ToInt32(dtHeader.Rows[i][3].ToString());
        //                objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][4].ToString());
        //                objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][5].ToString());
        //                objHeader.PayPeriodId = Convert.ToInt32(dtHeader.Rows[i][6].ToString());
        //                objHeader.Amount = Convert.ToInt32(dtHeader.Rows[i][7].ToString());
        //                objHeader.Hours = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][8]);
        //                objHeader.Remark = dtHeader.Rows[i][9].ToString();
        //                objStaff.listImport.Add(objHeader);
        //            }
        //        }
        //    }
        //    //BSM.ListDepreMethod = DB.ExFADepreciationMethods.ToList();
        //    Session[Index_Sess_Obj + ActionName] = objStaff;
        //    return PartialView("GridItem", objStaff);
        //}
        #region Import
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(this.KeyName);
            var obj = new PREmpWorkingObject();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PREmpWorking", SYSConstant.DEFAULT_UPLOAD_LIST);
            obj.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate).ToList();
            Session[Index_Sess_Obj + ActionName] = obj;
            return View(obj);

        }
        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PREmpWorking", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("EMP_WORKING"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "EMP_WORKING";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
              sfi.ValidationSettings,
              sfi.uc_FileUploadComplete);
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PREmpWorking", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new PREmpWorkingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
            }


            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.listImport = new List<PREmpWorking>();

            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            ActionName = "Import";
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            if (obj != null)
            {
                var DBB = new HumicaDBContext();
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                var objStaff = new PREmpWorkingObject();
                if (obj.IsGenerate == true)
                {
                    SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }
                if (dtHeader != null)
                {
                    try
                    {
                        string msg = SYConstant.OK;
                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {
                            objStaff.listImport = new List<PREmpWorking>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new PREmpWorking();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.InYear = Convert.ToInt32(dtHeader.Rows[i][2].ToString());
                                objHeader.InMonth = Convert.ToInt32(dtHeader.Rows[i][3].ToString());
                                objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][4].ToString());
                                objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][5].ToString());
                                objHeader.PayPeriodId = Convert.ToInt32(dtHeader.Rows[i][6].ToString());
                                objHeader.ActWorkDay = Convert.ToInt32(dtHeader.Rows[i][7].ToString());
                                objHeader.Hours = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][8]);
                                objHeader.Remark = dtHeader.Rows[i][9].ToString();
                                objStaff.listImport.Add(objHeader);
                            }

                            msg = objStaff.uploadEmpW();
                            if (msg == SYConstant.OK)
                            {

                                obj.Message = SYConstant.OK;
                                //obj.DocumentNo = DocBatch.NextNumberRank;
                                SYMessages mess = SYMessages.getMessageObject("IMPORTED", user.Lang);
                                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                                obj.IsGenerate = true;
                                DBB.MDUploadTemplates.Attach(obj);
                                DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                                DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DBB.SaveChanges();

                            }
                            else
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + objStaff.MessageError;
                                obj.IsGenerate = false;
                                DB.MDUploadTemplates.Attach(obj);
                                DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DB.SaveChanges();
                                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessage(msg, user.Lang) + "(" + objStaff.MessageError + ")";
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }


                        }
                    }
                    catch (DbUpdateException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();
                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                }

            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }

        public ActionResult DownloadTemplate()
        {
            string fileName = Server.MapPath("~/Content/TEMPLATE/WORKING_TEMPLATE.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=WORKING_TEMPLATE.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion

        public ActionResult Details(string ID)
        {
            //ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                PREmpWorkingObject BSM = new PREmpWorkingObject();
                BSM.Header = new PREmpWorking();
                //BSM.Header = new HR_STAFF_VIEW();
                int TranNo = Convert.ToInt32(ID);
                BSM.Header = DB.PREmpWorkings.SingleOrDefault(w => w.TranNo == TranNo);
                if (BSM.Header != null)
                {
                    //BSM.Header = DB..SingleOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);

            PREmpWorkingObject BSM = new PREmpWorkingObject();
            BSM.ListHeader = DB.PREmpWorkings.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        public ActionResult ShowData(DateTime FromDate, DateTime ToDate)
        {

            ActionName = "Create";
            PREmpWorkingObject BSM = new PREmpWorkingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PREmpWorkingObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = new PREmpWorking();
            }
            int monthsApart = 12 * (ToDate.Year - FromDate.Year) + ToDate.Month - FromDate.Month + ToDate.Day - FromDate.Day + 1;
            var result = new
            {
                MS = SYConstant.OK,
                Pay = monthsApart,
            };

            // var rs = new { MS = SYConstant.FAIL };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {

            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
        }
    }
}