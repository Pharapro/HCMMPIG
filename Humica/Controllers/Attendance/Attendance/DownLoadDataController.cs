using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{

    public class DownLoadDataController : Humica.EF.Controllers.MasterSaleController
    {
        private static string Error = "";
        private const string SCREEN_ID = "ATM0000001";
        private const string URL_SCREEN = "/Attendance/Attendance/DownLoadData/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "DeviceID";

        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SM = new SMSystemEntity();

        public DownLoadDataController()
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
            DataSelector();

            DevSettingObject BSM = new DevSettingObject();
            BSM.Filter = new Humica.Core.FT.FTFilterInOut();
            BSM.Filter.FromDate = DateTime.Now;
            BSM.Filter.ToDate = DateTime.Now;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
            }

            var ListHeader = DB.ATDevSettings.ToList();
            var Branch = SYConstant.getBranchDataAccess();
            BSM.ListHeader = ListHeader.ToList();
            BSM.Progress = ListHeader.Count();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }


        #endregion
        public ActionResult Download(DateTime FromDate, DateTime ToDate)
        {
            ActionName = "Index";
            UserSession();
            //UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            DevSettingObject BSM = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
            }
            string id = BSM.IPAddress;
            //string msg = objDev.DownloadData(id,FromDate,ToDate);
            //string[] IP = id.Split(';');
            //foreach (var Code in IP)
            //{
            //    if (Code.Trim() != "")
            //    {
            //        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => this.ReleaseDocWaiting(Code, FromDate, ToDate, cancellationToken));
            //        int len = id.Split(';').Length;
            //        string actionDesc = SYMessages.getMessage("RELEASE_DOC") + " " + SYActionObject.getTitleScreen(SCREEN_ID);

            //        InsertWaitingProcess(len, actionDesc, SYMessages.getMessage("PROCESSING"));

            //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WAITING_PRO", user.Lang);

            //    }
            //}

            var ListWaiting = SM.SYWaitingBackgrounds.Where(w => w.UserName == user.UserName && w.ScreenId == SCREEN_ID && w.IsFinish == false).ToList();
            if (ListWaiting.Count() == 0)
            {
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => this.ReleaseDocWaitingAsync(id, FromDate, ToDate, cancellationToken));
                int len = id.Split(';').Length;
                string actionDesc = SYActionObject.getTitleScreen(SCREEN_ID);

                InsertWaitingProcess(len, actionDesc, SYMessages.getMessage("PROCESSING"), id);

                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WAITING_PRO", user.Lang);
            }
            else
            {
                var ListWaitingItem = SM.SYWaitingBackgroundItems.Where(w => w.UserName == user.UserName && w.ScreenId == SCREEN_ID && w.IsFinish == false).ToList();
                if (ListWaiting.Count() > 0)
                {
                    id = ListWaitingItem.First().DocumentReference;
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => this.ReleaseDocWaitingAsync(id, FromDate, ToDate, cancellationToken));
                }
                //  Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("WAITING_FOR_PRO", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #region "Import"
        public ActionResult GridItemsInOut()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            DevSettingObject objStaff = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
                if (objStaff.ListInOut != null)
                {
                    objStaff.ListInOut.Clear();
                }
            }
            if (objStaff.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in objStaff.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }


                DataTable dtHeader = excel.GenerateExcelData();
                objStaff.ListInOut = new List<ATInOut>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new ATInOut();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.CardNo = dtHeader.Rows[i][1].ToString();
                        objHeader.FullDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                        objHeader.STATUS = 3;
                        objStaff.ListInOut.Add(objHeader);
                    }
                }


            }
            //BSM.ListDepreMethod = DB.ExFADepreciationMethods.ToList();
            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView("GridItemsInOut", objStaff);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "DownLoadData", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
                if (objStaff.ListInOut != null)
                {
                    objStaff.ListInOut.Clear();
                }
            }

            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (objStaff.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in objStaff.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }

                DataTable dtHeader = excel.GenerateExcelData();
                objStaff.ListInOut = new List<ATInOut>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new ATInOut();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.CardNo = dtHeader.Rows[i][1].ToString();
                        objHeader.FullDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                        objHeader.STATUS = 3;
                        objStaff.ListInOut.Add(objHeader);
                    }
                }
            }
            Session[Index_Sess_Obj + ActionName] = objStaff;
            return View(objStaff);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "DownLoadData", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("INOUT"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "INOUT";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);


            var objStaff = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
            }


            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListInOut = new List<ATInOut>();

            Session[Index_Sess_Obj + ActionName] = objStaff;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "DownLoadData", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
            }


            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListInOut = new List<ATInOut>();

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
                var objStaff = new DevSettingObject();
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
                            objStaff.ListInOut = new List<ATInOut>();
                            //objStaff.ListItem = new List<HRStaffProfile>();
                            //objStaff.ListPlanItem = new List<HLContractPlanItem>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new ATInOut();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.CardNo = dtHeader.Rows[i][1].ToString();
                                objHeader.FullDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                                objHeader.STATUS = 3;
                                objStaff.ListInOut.Add(objHeader);
                            }

                            msg = objStaff.uploadInOut();
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
            string fileName = Server.MapPath("~/Content/TEMPLATE/INOUT_TEMPLATE.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=INOUT_TEMPLATE.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion
        [HttpPost]
        public string getEmpCode(string EmpCode)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();

            DevSettingObject BSM = new DevSettingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DevSettingObject)Session[Index_Sess_Obj + ActionName];

                BSM.IPAddress = EmpCode;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_IP");
            }
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            DevSettingObject BSM = new DevSettingObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DevSettingObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListHeader);
        }
        private void DataSelector()
        {

        }
        public async Task ReleaseDocWaitingAsync(string IP, DateTime FromDate, DateTime ToDate, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                UserSession();
                DevSettingObject param = new DevSettingObject();
                string userName = user.UserName;
                param.User = user;
                param.BS = bs;
                Error = "";
                if (DevSettingObject.ListProgresses == null)
                {
                    DevSettingObject.ListProgresses = new List<ListProgress>();
                }
                if (IP != null)
                {
                    if (DevSettingObject.ListProgresses.Where(w => w.UserName == user.UserName).ToList().Count() == 0)
                    {
                        DevSettingObject.ListProgresses.Add(new ListProgress { UserName = user.UserName, Progress = 0, Percen = 0 });
                    }
                    else
                    {
                        DevSettingObject.ListProgresses.Where(w => w.UserName == user.UserName).ToList().ForEach(x => x.Percen = 0);
                    }
                    string msg = await param.DownloadData(IP, FromDate, ToDate);
                    if (msg != SYConstant.OK)
                    {
                        Error = "CANNOT_CON_EN";
                        ProcessFail(Error);
                    }
                    else
                    {
                        if (param.MessageError.Length > 0)
                            Error = " (Error :" + param.MessageError + " )";
                        ProcessDone(SYMessages.getMessage("DOWNLOAD_EN") + Error);
                    }
                }
                else
                {
                    ProcessFail(SYMessages.getMessage("DOC_ERQ"));
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(10000);
                ProcessFail();
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = ScreendIDControl;
                log.UserId = user.UserID.ToString();
                log.DocurmentAction = "MASTER_QUOTA";
                log.Action = SYActionBehavior.EDIT.ToString();

                SYEventLogObject.saveEventLog(log, e, true);

                /*----------------------------------------------------------*/
            }
        }
        public ActionResult ShowProcess()
        {
            decimal P = 0;
            UserSession();
            if (DevSettingObject.ListProgresses != null)
            {
                P = DevSettingObject.ListProgresses.Where(w => w.UserName == user.UserName).Sum(x => x.Percen);
            }
            var ListWaiting = SM.SYWaitingBackgrounds.Where(w => w.UserName == user.UserName && w.ScreenId == SCREEN_ID && w.IsFinish == false).ToList();
            if (ListWaiting.Count == 0)
            {
                var mess = SYMessages.getMessageObject("DOWNLOAD_EN", user.Lang);
                if (Error.Length > 0)
                    mess.Description = mess.Description + Error;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                var result1 = new
                {
                    MS = SYConstant.FAIL,
                    Percen = P
                };
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
            var result = new
            {
                MS = SYConstant.OK,
                Percen = P
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
