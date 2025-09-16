using DevExpress.ClipboardSource.SpreadsheetML;
using DevExpress.Web;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Web.UI;

namespace Humica.Models.SY
{
    public partial class SYFileImport : IClsApplication
    {
        IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public SYFileImport(CFUploadPath confPath)
        {
            UploadDirectory = confPath.PathDirectory;
        }
        public string UploadDirectory { get; set; }
        public string ScreenID { get; set; }
        public string UploadBy { get; set; }
        public string DocumentsDirectory { get; set; }
        public string TempDirectory { get; set; }
        public string AttachmentMulti { get; set; }
        public string Attachment_IMG { get; set; }
        public string DocumentMulti { get; set; }
        public MDUploadTemplate ObjectTemplate { get; set; }

        public readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".bmp", ".png", ".xlsx", ".xls", ".doc", ".docx", ".pdf", ".repx", ".pptx" },
            //MaxFileSize = 1024000000,
            MaxFileSize = Convert.ToInt64(SYSettings.getSetting("MAX_FILE_SIZE_ATTACHMENT").SettinValue)
        };

        public void uc_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                //string resultFilePath = HttpContext.Current.Request.MapPath(UploadDirectory + e.UploadedFile.FileName);
                string resultFilePath = this.UploadDirectory + e.UploadedFile.FileName;


                HumicaDBContext DB = new HumicaDBContext();
                this.ObjectTemplate.UpoadPath = resultFilePath;
                this.ObjectTemplate.UploadName = e.UploadedFile.FileName;

                this.ObjectTemplate.ContentLength = e.UploadedFile.ContentLength;
                this.ObjectTemplate.ContentType = e.UploadedFile.ContentType;

                DB.MDUploadTemplates.Add(this.ObjectTemplate);
                try
                {
                    if (!Directory.Exists(this.UploadDirectory))
                    {
                        Directory.CreateDirectory(this.UploadDirectory);
                    }
                    e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    }



                }
                catch (DbEntityValidationException ee)
                {

                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee);
                    /*----------------------------------------------------------*/

                    e.IsValid = false;
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    e.CallbackData = e.ErrorText;
                }
                catch (Exception ee)
                {

                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();
                    SYEventLogObject.saveEventLog(log, ee, true);
                    /*----------------------------------------------------------*/

                    e.IsValid = false;
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    e.CallbackData = e.ErrorText;
                }
            }
        }

        public void uc_FileUploadCompleteFile(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                string resultFilePath = this.UploadDirectory + e.UploadedFile.FileName;

                string filename = e.UploadedFile.FileName;

                HumicaDBContext DB = new HumicaDBContext();
                HumicaDBContext DBNew = new HumicaDBContext();
                this.ObjectTemplate.UpoadPath = resultFilePath;
                this.ObjectTemplate.UploadName = filename;
                this.ObjectTemplate.UploadDate = DateTime.Now;
                this.ObjectTemplate.ContentLength = e.UploadedFile.ContentLength;
                this.ObjectTemplate.ContentType = e.UploadedFile.ContentType;
                DB.MDUploadTemplates.Add(this.ObjectTemplate);
                try
                {
                    string filenameSave = System.Web.HttpContext.Current.Request.MapPath(resultFilePath);
                    filenameSave = Path.Combine(UploadDirectory, filenameSave);
                    string strPath = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    e.UploadedFile.SaveAs(filenameSave);
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        e.CallbackData = SYUrl.getBaseUrl() + this.ObjectTemplate.UpoadPath.Replace("~", "");
                    }
                    else
                    {
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = ObjectTemplate.ScreenId;
                        log.UserId = ObjectTemplate.UploadBy;
                        log.LogErrorMessage = e.ErrorText;
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    }
                    if (e.UploadedFile.IsValid == true)
                    {
                        DBNew.SaveChanges();
                        int row = DB.SaveChanges();
                    }

                }
                catch (DbUpdateException ee)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee, true);
                    /*----------------------------------------------------------*/

                }
                catch (DbEntityValidationException ee)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee);

                }
                catch (Exception ee)
                {

                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee, true);
                    /*----------------------------------------------------------*/

                    e.IsValid = false;
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    e.CallbackData = e.ErrorText;
                }
            }
        }
        public void uc_FileUploadCompleteMultiFile(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                DateTime CurrentDate = DateTime.Now;
                string Temporary = "//Temporary//" + CurrentDate.ToString("dd-MM-yyyy") + "//";
                string resultFilePath = this.UploadDirectory + Temporary + e.UploadedFile.FileName;
                string filename = e.UploadedFile.FileName;
                try
                {
                    string filenameSave = System.Web.HttpContext.Current.Request.MapPath(resultFilePath);
                    filenameSave = Path.Combine(UploadDirectory, filenameSave);
                    string strPath = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory + Temporary);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    e.UploadedFile.SaveAs(filenameSave);
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        e.CallbackData = resultFilePath.Replace("~", "");
                    }
                    if (e.UploadedFile.IsValid == true)
                    {
                        AttachmentMulti += resultFilePath + ";";
                        DocumentMulti += filename + ";";
                    }
                }
                catch (DbUpdateException ee)
                {
                    e.ErrorText = ClsEventLog.Save_EventLogs(ScreenID, UploadBy, "UPLOAD", SYActionBehavior.ADD.ToString(), ee, true);
                    e.IsValid = false;
                    e.CallbackData = e.ErrorText;
                }
                catch (DbEntityValidationException ee)
                {
                    e.ErrorText = ClsEventLog.Save_EventLogs(ScreenID, UploadBy, "UPLOAD", SYActionBehavior.ADD.ToString(), ee, true);
                    e.IsValid = false;
                    e.CallbackData = e.ErrorText;
                }
                catch (Exception ee)
                {
                    e.ErrorText = ClsEventLog.Save_EventLogs(ScreenID, UploadBy, "UPLOAD", SYActionBehavior.ADD.ToString(), ee, true);
                    e.IsValid = false;
                    e.CallbackData = e.ErrorText;
                }
            }
        }
        public void uc_FileUploadFile(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                string resultFilePath = this.UploadDirectory + e.UploadedFile.FileName;

                string filename = e.UploadedFile.FileName;

                this.ObjectTemplate.UpoadPath = resultFilePath;
                try
                {
                    if (!Directory.Exists(this.UploadDirectory))
                    {
                        Directory.CreateDirectory(this.UploadDirectory);
                    }
                    string filenameSave = resultFilePath;// System.Web.HttpContext.Current.Request.MapPath(resultFilePath);
                    filenameSave = Path.Combine(UploadDirectory, filenameSave);
                    e.UploadedFile.SaveAs(filenameSave);
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        // e.CallbackData = SYUrl.getBaseUrl() + this.ObjectTemplate.UpoadPath.Replace("~", "");
                    }
                    else
                    {
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = ObjectTemplate.ScreenId;
                        log.UserId = ObjectTemplate.UploadBy;
                        log.LogErrorMessage = e.ErrorText;
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    }
                    if (e.UploadedFile.IsValid == true)
                    {
                        //DBNew.SaveChanges();
                        //int row = DB.SaveChanges();
                    }

                }
                catch (DbUpdateException ee)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee, true);
                    /*----------------------------------------------------------*/

                }
                catch (DbEntityValidationException ee)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee);

                }
                catch (Exception ee)
                {

                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = ObjectTemplate.ScreenId;
                    log.UserId = ObjectTemplate.UploadBy;
                    log.DocurmentAction = "UPLOAD";
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, ee, true);
                    /*----------------------------------------------------------*/

                    e.IsValid = false;
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    e.CallbackData = e.ErrorText;
                }



            }
        }
        public void uc_FileUploadComplete_IMG(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                //string resultFilePath = "";
                DateTime CurrentDate = DateTime.Now;
                string Temporary = "//Temporary//" + CurrentDate.ToString("dd-MM-yyyy") + "//";
                string resultFilePath = this.UploadDirectory + Temporary + e.UploadedFile.FileName;
                //string[] sp = e.UploadedFile.FileName.Split('.');

                //string extendtion = "";
                //string filename = "";
                //if (sp.Length > 0)
                //{
                //    extendtion = sp[sp.Length - 1];
                //    //filename = this.TokenKey + "." + extendtion;
                //    resultFilePath = Path.Combine(UploadDirectory, filename);
                //}
                //else
                //{
                //    filename = e.UploadedFile.FileName;
                //    resultFilePath = Path.Combine(UploadDirectory, filename);
                //}

                //HumicaDBContext DB = new HumicaDBContext();
                //this.ObjectTemplate.UpoadPath = resultFilePath;
                //this.ObjectTemplate.UploadName = filename;
                //this.ObjectTemplate.TokenCode = TokenKey;
                //this.ObjectTemplate.UploadDate = DateTime.Now;
                //this.ObjectTemplate.ContentLength = e.UploadedFile.ContentLength;
                //this.ObjectTemplate.ContentType = e.UploadedFile.ContentType;
                //this.ObjectTemplate.TokenCode = TokenKey;
                //this.ObjectTemplate.Remove = "X";

                //DB.MDUploadImages.Add(this.ObjectTemplate);
                try
                {
                    string filenameSave = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory + Temporary + e.UploadedFile.FileName);
                    //e.UploadedFile.SaveAs(resultFilePath, true);//Code Central Mode - Uncomment This Line

                    //string filename = string.Format("file-.jpg", e.UploadedFile.FileName);
                    filenameSave = Path.Combine(UploadDirectory, filenameSave);
                    string strPath = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory + Temporary);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    e.UploadedFile.SaveAs(filenameSave);


                    //e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line
                    IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                    if (urlResolver != null)
                    {
                        e.CallbackData = SYUrl.getBaseUrl() + resultFilePath.Replace("~", "");
                        Attachment_IMG = resultFilePath;
                    }
                    //else
                    //{
                    //    SYEventLog log = new SYEventLog();
                    //    log.ScreenId = ObjectTemplate.ScreenId;
                    //    log.UserId = ObjectTemplate.UploadBy;
                    //    log.LogErrorMessage = e.ErrorText;
                    //    log.DocurmentAction = "UPLOAD";
                    //    log.Action = SYActionBehavior.ADD.ToString();

                    //    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                    //}
                    //if (e.UploadedFile.IsValid == true)
                    //{
                    //    //int row = DB.SaveChanges();
                    //}
                }
                catch (Exception ee)
                {
                    e.ErrorText = ClsEventLog.Save_EventLogs(ScreenID, UploadBy, "UPLOAD", SYActionBehavior.ADD.ToString(), ee, true);
                    e.IsValid = false;
                    e.CallbackData = e.ErrorText;
                }
            }
        }
    }
}