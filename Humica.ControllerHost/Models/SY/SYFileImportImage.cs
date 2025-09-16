using DevExpress.Web;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Web.UI;

namespace Humica.Models.SY
{
    public partial class SYFileImportImage
    {
        public string UploadDirectory { get; set; }
        public string DocumentsDirectory { get; set; }
        public string TempDirectory { get; set; }
        public string TokenKey { get; set; }
        public MDUploadImage ObjectTemplate { get; set; }


        public SYFileImportImage(CFUploadPath confPath)
        {
            UploadDirectory = confPath.PathDirectory;
        }
        //public const string UploadDirectory = "E://DMS_FILE/Content/TEMPLATE/QUOTA/";
        //public const string UploadDirectory = "E:\\DMS\\DBSYSTEM\\DBSYSTEM\\Content\\UPLOAD\\QUOTA\\";

        public readonly UploadControlValidationSettings ValidationSettings = new UploadControlValidationSettings
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".doc", ".docx", ".bmp", ".png", ".pdf" },
            //MaxFileSize = 5242880,
            MaxFileSize = Convert.ToInt64(SYSettings.getSetting("MAX_FILE_SIZE_ATTACHMENT").SettinValue)
        };

        public void uc_FileUploadComplete(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                string resultFilePath = "";

                string[] sp = e.UploadedFile.FileName.Split('.');

                string extendtion = "";
                string filename = "";
                if (sp.Length > 0)
                {
                    extendtion = sp[sp.Length - 1];
                    filename = this.TokenKey + "." + extendtion;
                    resultFilePath = Path.Combine(UploadDirectory, filename);
                }
                else
                {
                    filename = e.UploadedFile.FileName;
                    resultFilePath = Path.Combine(UploadDirectory, filename);
                }

                HumicaDBContext DB = new HumicaDBContext();
                this.ObjectTemplate.UpoadPath = resultFilePath;
                this.ObjectTemplate.UploadName = filename;
                this.ObjectTemplate.TokenCode = TokenKey;
                this.ObjectTemplate.UploadDate = DateTime.Now;
                this.ObjectTemplate.ContentLength = e.UploadedFile.ContentLength;
                this.ObjectTemplate.ContentType = e.UploadedFile.ContentType;
                this.ObjectTemplate.TokenCode = TokenKey;
                this.ObjectTemplate.Remove = "X";

                DB.MDUploadImages.Add(this.ObjectTemplate);
                try
                {
                    string filenameSave = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory + this.TokenKey + "." + extendtion);
                    //e.UploadedFile.SaveAs(resultFilePath, true);//Code Central Mode - Uncomment This Line

                    //string filename = string.Format("file-.jpg", e.UploadedFile.FileName);
                    filenameSave = Path.Combine(UploadDirectory, filenameSave);
                    string strPath = System.Web.HttpContext.Current.Request.MapPath(UploadDirectory);
                    if (!Directory.Exists(strPath))
                    {
                        Directory.CreateDirectory(strPath);
                    }
                    e.UploadedFile.SaveAs(filenameSave);


                    //e.UploadedFile.SaveAs(resultFilePath);//Code Central Mode - Uncomment This Line
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
    }
}