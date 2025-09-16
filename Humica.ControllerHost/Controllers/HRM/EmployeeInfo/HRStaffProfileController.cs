using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Employee;
using Humica.Logic.HR;
using Humica.Models.SY;
using Humica.Report.HRM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRStaffProfileController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000001";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRStaffProfile/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILENAME = "12313123123sadfsdfsdfsdfANME";
        private string PATH_FILE2 = "12313123123sadfsdfsdfs12fxdf";
        private string PATH_FILE_Fimaly = "12313123123sadfsdfsdfsdf";
		private string PATH_FILE_Education = "12313123123sadfsdfsdfsdf";
		private string PATH_FILE_Contract = "12313123123sadfsdfsdfsdf";
		private string PATH_FILE_Disciplinary = "12313123123sadfsdfsdfsdf";
		private string PATH_FILE_Certificate = "12313123123sadfsdfsdfsdf";
		private string PATH_FILENAME_Filaly = "12313123123sadfsdfsdfsdfANME";
        //private string PATH_FILEFa = "PATH_FILEFa";
        //private string PATH_FILEBi = "PATH_FILEBi";

        IClsEmployee BSM;
        IUnitOfWork unitOfWork;
        public HRStaffProfileController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEmployee();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }
            BSM.OnLoandindEmployee();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsEmployee collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataList();
            BSM.Filter = collection.Filter;
            BSM.OnLoandindEmployee();
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
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListStaffView);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            SetSessionNULL();
			DataFamily();
			OnDataEducation();
			OnDataContract();
			OnDataDisciplinary();
			OnDataCertificate();
            OnDataMedical();
			BSM.OnCreatingLoading();
            var IsSalary = BSM.IsHideSalary(BSM.Header.LevelCode);
            if (IsSalary != true)
            {
                ViewData[ClsConstant.IS_SALARY] = false;
                BSM.Salary = "#####";
                BSM.IsTax = false;
            }
            else
            {
                ViewData[ClsConstant.IS_SALARY] = true;
                BSM.SalaryNSSF = "0";
                BSM.SalaryTax = "0";
                BSM.Salary = "0";
            }
            Session[PATH_FILE2] = null;
            Session[PATH_FILE] = null;
            Session[PATH_FILENAME] = null;
            //if (BSM.Salary == "#####")
            //{
            //    ViewData[ClsConstant.IS_SALARY] = false;
            //}
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsEmployee collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                BSM.Salary = collection.Salary;
                var msg = BSM.CreatStaff();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.EmpCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?Empcode=" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
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
        public ActionResult Edit(string Empcode)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector(true);
            DataList();
            DataFamily();
			OnDataEducation();
            OnDataContract();
            OnDataDisciplinary();
            OnDataCertificate();
			OnDataMedical();
			ViewData[ClsConstant.IS_READ_ONLY] = true;
            ViewData[ClsConstant.IS_SALARY] = true;
            Session["SessionFileFamily"] = null;
			Session["SessionFileIdentity"] = null;
            Session["SessionFileEdu"] = null;
            Session["SessionFileContract"] = null;
			Session["SessionFileDisciplinary"] = null;
			Session["SessionFileCertificate"] = null;
			if (Empcode != null)
            {
                BSM.OnDetailLoading(Empcode);
                FitlertDataProvince(BSM.Header.Province, BSM.Header.District, BSM.Header.Commune);
                BSM.IsTax = false;
                var isTax = SYSettings.getSetting("SALARYTAX");
                if (isTax != null)
                {
                    if (isTax.SettinValue == "YES") BSM.IsTax = true;
                }
                var IsSalary = BSM.IsHideSalary(BSM.Header.LevelCode);
                if (IsSalary != true)
                {
                    ViewData[ClsConstant.IS_SALARY] = false;
                    BSM.Salary = "#####";
                }
                else
                {
                    BSM.Salary = BSM.Header.Salary.ToString();
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("STAFF_NE", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string Empcode, ClsEmployee collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector(true);
            //    DataSelectStaff();
            UserConfListAndForm(this.KeyName);
            //    HRStaffProfileObject BSM = new HRStaffProfileObject();
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            ViewData[ClsConstant.IS_SALARY] = true;
			Session["SessionFileFamily"] = null;
			Session["SessionFileIdentity"] = null;
			Session["SessionFileEdu"] = null;
			Session["SessionFileContract"] = null;
			Session["SessionFileDisciplinary"] = null;
			Session["SessionFileCertificate"] = null;
			if (Empcode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                //        if (Session[PATH_FILEFa] != null)
                //        {
                //            BSM.Header.AttachmentJD = Session[PATH_FILEFa].ToString();
                //            Session[PATH_FILEFa] = null;
                //            BSM.Header.AttachJD = Session[PATH_FILEBi].ToString();
                //            Session[PATH_FILEBi] = null;
                //        }

                //        if (BSM.Header.AttachFile != null && Session[PATH_FILE] != null)
                //        {
                //                collection.Header.AttachFile = Session[PATH_FILE].ToString() +";"+ BSM.Header.AttachFile;
                //                Session[PATH_FILE] = null;
                //                collection.Header.Attachment = Session[PATH_FILE2].ToString() +";"+ BSM.Header.Attachment;
                //                Session[PATH_FILE2] = null;
                //        }
                //        BSM.Header = collection.Header;
                //        if (Session[PATH_FILE] != null)
                //        {
                //            BSM.Header.AttachFile = Session[PATH_FILE].ToString();
                //            Session[PATH_FILE] = null;
                //            BSM.Header.Attachment = Session[PATH_FILE2].ToString();
                //            Session[PATH_FILE2] = null;
                //        }


                //        if (BSM.Header.Email != null)
                //            BSM.Header.Email = collection.Header.Email.Trim();
                //        BSM.Header.EmpCode = Empcode;
                string msg = "";
                BSM.ScreenId = SCREEN_ID;

                msg = BSM.UpdateStaff(Empcode);
                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = Empcode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?Empcode=" + mess.DocumentNumber;
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
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion

        #region "Details"
        public ActionResult Details(string Empcode)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector(true);
            DataList();
			DataFamily();
			OnDataEducation();
			OnDataContract();
			OnDataDisciplinary();
			OnDataCertificate();
			OnDataMedical();
			ViewData[ClsConstant.IS_READ_ONLY] = true;
            ViewData[ClsConstant.IS_READ_ONLY1] = true;
            ViewData[ClsConstant.IS_SALARY] = true;
            ViewData[SYConstant.PARAM_ID] = Empcode;
            if (Empcode != null)
            {
                BSM.OnDetailLoading(Empcode);
                FitlertDataProvince(BSM.Header.Province, BSM.Header.District, BSM.Header.Commune);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("STAFF_NE", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            BSM.ScreenId = SCREEN_ID;
            string msg = BSM.DeleteEmp(id);
            if (msg == SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
		#endregion

		#region "Print"
		public ActionResult Print(string Empcode)
		{
			UserSession();
			UserConf(ActionBehavior.VIEWONLY);
			ViewData[Humica.EF.SYSConstant.PARAM_ID] = Empcode;
			UserMVCReportView();
			return View("ReportView");
		}
		public ActionResult DocumentViewerPartial(string Empcode)
		{
			UserSession();
			UserConf(ActionBehavior.VIEWONLY);
			ActionName = "Print";
			//UserMVC();
			var SD = unitOfWork.Set<HRStaffProfile>().FirstOrDefault(f => f.EmpCode == Empcode);
			if (SD != null)
			{
				try
				{
					ViewData[Humica.EF.SYSConstant.PARAM_ID] = Empcode;
					var sa = new FRM_EmpProfile();
					var objRpt = unitOfWork.Set<CFReportObject>().Where(w => w.ScreenID == SCREEN_ID
				    && w.IsActive == true).ToList();
					if (objRpt.Count > 0)
					{
						sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
					}
					sa.Parameters["EmpCode"].Value = SD.EmpCode;
					sa.Parameters["EmpCode"].Visible = false;
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
					log.DocurmentAction = Empcode;
					log.Action = SYActionBehavior.ADD.ToString();

					SYEventLogObject.saveEventLog(log, e, true);
					/*----------------------------------------------------------*/
				}
			}
			return null;
		}
		public ActionResult DocumentViewerExportTo(string Empcode)
		{
			ActionName = "Print";
			if (Session[Index_Sess_Obj] != null)
			{
				FRM_EmpProfile reportModel = (FRM_EmpProfile)Session[Index_Sess_Obj];
				return ReportViewerExtension.ExportTo(reportModel);
			}
			return null;
		}
		#endregion

		#region "Ajax Image"
		public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;
            UploadControlExtension.GetUploadedFiles("uc_image",
                sfi.ValidationSettings, sfi.uc_FileUploadComplete_IMG);
            Session["PATH_IMG"] = sfi.Attachment_IMG;
            return null;
        }
        public ActionResult UploadControlCallbackActionImageAddress(HttpPostedFileBase[] file_Uploader)
        {
            UserSession();
            DataSelector();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            var sfi = new SYFileImport(path);
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl_GPS",
                sfi.ValidationSettings,
                this.uc_FileUploadCompleteM);
            return null;
        }
		public ActionResult UploadControlCallbackActionImageQr()
		{
			UserSession();
			var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
			SYFileImport sfi = new SYFileImport(path);
			sfi.AttachmentMulti = "";
			sfi.DocumentMulti = "";
			sfi.ScreenID = SCREEN_ID;
			sfi.UploadBy = user.UserName;
			UploadControlExtension.GetUploadedFiles("qr_image",
				sfi.ValidationSettings, sfi.uc_FileUploadComplete_IMG);
			Session["PATH_IMGQr"] = sfi.Attachment_IMG;
			return null;
		}
		public void uc_FileUploadCompleteM(object sender, FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
                var objFile = new SYFileImport(path);

                string[] sp = e.UploadedFile.FileName.Split('.');

                string filename = "";
                if (sp.Length > 0)
                {
                    filename = e.UploadedFile.FileName;
                }
                else
                {
                    filename = e.UploadedFile.FileName;
                }
                string resultFilePath = Server.MapPath(path.PathDirectory + filename);


                e.UploadedFile.SaveAs(resultFilePath);

                IUrlResolutionService urlResolver = sender as IUrlResolutionService;
                if (urlResolver != null)
                {
                    e.CallbackData = urlResolver.ResolveClientUrl(resultFilePath);
                    Session[PATH_FILE2] += e.UploadedFile.FileName + ";";
                    Session[PATH_FILE] += path.PathDirectory.Replace("~", "") + "/" + filename + ";";
                }
                else
                {
                    e.ErrorText = SYMessages.getMessage("FILE_EXIST");
                }
            }
        }
        public string DeleteFile(string FileName)
        {
            try
            {
                string[] sp = FileName.Split('/');
                string file = sp[sp.Length - 1];
                string[] spf = file.Split('.');
                string sfile = spf[0];

                var obj = unitOfWork.Set<MDUploadImage>().FirstOrDefault(w => w.TokenCode == sfile);
                if (obj != null)
                {
                    unitOfWork.Delete(obj);
                    unitOfWork.Save();
                    //if (row > 0)
                    //{
                    var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
                    string root = HostingEnvironment.ApplicationPhysicalPath;
                    // obj.UpoadPath = obj.UpoadPath.Replace("~", "").Replace("/", "\\");
                    string fileNameDelete = root + obj.UpoadPath;
                    if (System.IO.File.Exists(fileNameDelete))
                    {
                        System.IO.File.Delete(fileNameDelete);
                    }
                    //}
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public ActionResult UploadControlCallbackActionSignature()
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;
            UploadControlExtension.GetUploadedFiles("uc_signature", sfi.ValidationSettings, sfi.uc_FileUploadComplete_IMG);
            Session["PATH_IMG_signature"] = sfi.Attachment_IMG;
            return null;
        }

        #endregion

        #region "Identify"
        public ActionResult _Identity()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Identity", BSM.ListEmpIdentity);
        }
        public ActionResult _IdentityDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_IdentityDetail", BSM.ListEmpIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateIden(HREmpIdentity ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    if (Session[PATH_FILE] != null)
                    {
                        ModelObject.Attachment = Session[PATH_FILE].ToString();
                        ModelObject.Document = Session[PATH_FILENAME].ToString();
                    }
                    var msg = BSM.OnGridModifyIdentity(ModelObject, SYActionBehavior.ADD.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[PATH_FILE] = null;
                        Session[PATH_FILENAME] = null;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataSelector();
            return PartialView("_Identity", BSM.ListEmpIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditIden(HREmpIdentity ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                bool IsEdit = false;
				bool IsAddNewFile = false;
				if (Session[PATH_FILE] != null)
				{
					ModelObject.Attachment = Session[PATH_FILE].ToString();
					if (Session[PATH_FILENAME] != null)
					{
						ModelObject.Document = Session[PATH_FILENAME].ToString();
					}
					IsAddNewFile = true;
					IsEdit = true;
				}
				else if (Session["SessionFileIdentity"] != null)
				{
					ModelObject.Attachment = Session["SessionFileIdentity"].ToString();
					IsEdit = true;
				}
                var msg = BSM.OnGridModifyIdentity(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit, IsAddNewFile);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[PATH_FILE] = null;
                    Session[PATH_FILENAME] = null;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            DataSelector();
            return PartialView("_Identity", BSM.ListEmpIdentity);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteIden(HREmpIdentity ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    var msg = BSM.OnGridModifyIdentity(ModelObject, SYActionBehavior.DELETE.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataSelector();
            return PartialView("_Identity", BSM.ListEmpIdentity);
        }

        public ActionResult UploadControlCallbackActionIdentity(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadIdentify",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteMultiFile);
            Session[PATH_FILE] = sfi.AttachmentMulti;
            Session[PATH_FILENAME] = sfi.DocumentMulti;
            return null;
        }

		[HttpPost]
		public ActionResult DeleteFileIdentity(long TranNo, string filePath)
		{
			try
			{
				var Identity = unitOfWork.Set<HREmpIdentity>().FirstOrDefault(f => f.TranNo == TranNo);
				if (Identity == null)
				{
					return Json(new { success = false, message = "Identity record not found." });
				}

				string updatedAttachFile = string.Empty;
				if (!string.IsNullOrEmpty(Identity.Attachment))
				{
					if (Session["SessionFileIdentity"] != null)
					{
						Identity.Attachment = Session["SessionFileIdentity"].ToString();
					}
					var filePaths = Identity.Attachment.Split(';').ToList();
					//filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
					filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					Identity.Attachment = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
					updatedAttachFile = Identity.Attachment;
					Session["SessionFileIdentity"] = updatedAttachFile;
				}

				return Json(new { success = true, updatedAttachFile = updatedAttachFile });
			}
			catch (Exception e)
			{
				return Json(new { success = false, message = e.Message });
			}
		}
		#endregion

		#region "JobDescription"
		public ActionResult GridJobDescription()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridJobDescription", BSM.ListEmpJobDescription);
        }
        public ActionResult GridJobDescriptionDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridJobDescriptionDetail", BSM.ListEmpJobDescription);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateJobDes(HREmpJobDescription ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    if (Session[PATH_FILE] != null)
                    {
                        ModelObject.Attachment = Session[PATH_FILE].ToString();
                        ModelObject.Document = Session[PATH_FILENAME].ToString();
                    }
                    var msg = BSM.OnGridModifyJobDescription(ModelObject, SYActionBehavior.ADD.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[PATH_FILE] = null;
                        Session[PATH_FILENAME] = null;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridJobDescription", BSM.ListEmpJobDescription);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditJobDes(HREmpJobDescription ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                bool IsEdit = false;
                if (Session[PATH_FILE] != null)
                {
                    ModelObject.Attachment = Session[PATH_FILE].ToString();
                    ModelObject.Document = Session[PATH_FILENAME].ToString();
                    IsEdit = true;
                }
                var msg = BSM.OnGridModifyJobDescription(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[PATH_FILE] = null;
                    Session[PATH_FILENAME] = null;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridJobDescription", BSM.ListEmpJobDescription);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteJobDes(HREmpJobDescription ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    var msg = BSM.OnGridModifyJobDescription(ModelObject, SYActionBehavior.DELETE.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridJobDescription", BSM.ListEmpJobDescription);
        }

        public ActionResult UploadControlCallbackActionJobDes(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadJobDes",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteMultiFile);
            Session[PATH_FILE] = sfi.AttachmentMulti;
            Session[PATH_FILENAME] = sfi.DocumentMulti;
            return null;
        }
        #endregion

        #region Fimaly
        public ActionResult GridFamilyDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataFamily();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridFamilyDetail", BSM.ListEmpFamily);
        }
        public ActionResult GridFamily()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataFamily();
			if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridFamily", BSM.ListEmpFamily);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateFamily(HREmpFamily ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataFamily();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    if (Session[PATH_FILE_Fimaly] != null)
                    {
                        ModelObject.AttachFile = Session[PATH_FILE_Fimaly].ToString();
                        ModelObject.Document = Session[PATH_FILENAME_Filaly].ToString();
                    }
                    var msg = BSM.OnGridModifyFamily(ModelObject, SYActionBehavior.ADD.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[PATH_FILE_Fimaly] = null;
                        Session[PATH_FILENAME_Filaly] = null;
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridFamily", BSM.ListEmpFamily);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditFamily(HREmpFamily ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataFamily();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                bool IsEdit = false;
				bool IsAddNewFile = false;
                if (Session[PATH_FILE_Fimaly] != null)
                {
                    ModelObject.AttachFile = Session[PATH_FILE_Fimaly].ToString();
                    if (Session[PATH_FILENAME_Filaly] != null)
                    {
						ModelObject.Document = Session[PATH_FILENAME_Filaly].ToString();
					}
					IsAddNewFile=true;
					IsEdit = true;
                }
				else if(Session["SessionFileFamily"] != null)
				{
					ModelObject.AttachFile = Session["SessionFileFamily"].ToString();
					IsEdit = true;
				}
				var msg = BSM.OnGridModifyFamily(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit,IsAddNewFile);
                if (msg == SYConstant.OK)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[PATH_FILE_Fimaly] = null;
                    Session[PATH_FILENAME_Filaly] = null;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridFamily", BSM.ListEmpFamily);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteFamily(HREmpFamily ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
			DataFamily();
			try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    var msg = BSM.OnGridModifyFamily(ModelObject, SYActionBehavior.DELETE.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridFamily", BSM.ListEmpFamily);
        }
        public ActionResult UploadControlCallbackActionFM(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.AttachmentMulti = "";
            sfi.DocumentMulti = "";
            sfi.ScreenID = SCREEN_ID;
            sfi.UploadBy = user.UserName;
            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadFM",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteMultiFile);
            Session[PATH_FILE_Fimaly] = sfi.AttachmentMulti;
            Session[PATH_FILENAME_Filaly] = sfi.DocumentMulti;
            return null;
        }

		[HttpPost]
		public ActionResult DeleteFileFamily(long TranNo, string filePath)
		{
			try
			{
				// Find the Family record
				var family = unitOfWork.Set<HREmpFamily>().FirstOrDefault(f => f.TranNo == TranNo);
				if (family == null)
				{
					return Json(new { success = false, message = "Family record not found." });
				}

				// Remove the specified filePath from the AttachFile field
				string updatedAttachFile = string.Empty;
				if (!string.IsNullOrEmpty(family.AttachFile))
				{
                    if (Session["SessionFileFamily"] != null)
                    {
                        family.AttachFile = Session["SessionFileFamily"].ToString();
					}
					var filePaths = family.AttachFile.Split(';').ToList();
					//filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
					filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					family.AttachFile = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
					updatedAttachFile = family.AttachFile;
					Session["SessionFileFamily"] = updatedAttachFile;
				}

				return Json(new { success = true, updatedAttachFile = updatedAttachFile });
			}
			catch (Exception e)
			{
				return Json(new { success = false, message = e.Message });
			}
		}

		#endregion
		#region Education
		public ActionResult GridEducationDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			OnDataEducation();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridEducationDetail", BSM.ListEducation);
		}
		public ActionResult GridEducation()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataEducation();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridEducation", BSM.ListEducation);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateEducation(HREmpEduc ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataEducation();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					if (Session[PATH_FILE_Education] != null)
					{
						ModelObject.AttachFile = Session[PATH_FILE_Education].ToString();
					}
					var msg = BSM.OnGridModifyEducation(ModelObject, SYActionBehavior.ADD.ToString());
					if (msg == SYConstant.OK)
					{
						Session[PATH_FILE_Education] = null;
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridEducation", BSM.ListEducation);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditEducation(HREmpEduc ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataEducation();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
				bool IsEdit = false;
				bool IsAddNewFile = false;
				if (Session[PATH_FILE_Education] != null)
				{
					ModelObject.AttachFile = Session[PATH_FILE_Education].ToString();
					IsEdit = true;
					IsAddNewFile = true;
				}
				else if (Session["SessionFileEdu"] != null)
				{
					ModelObject.AttachFile = Session["SessionFileEdu"].ToString();
					IsEdit = true;
				}
				var msg = BSM.OnGridModifyEducation(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit, IsAddNewFile);
				if (msg == SYConstant.OK)
				{
					Session[PATH_FILE_Education] = null;
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridEducation", BSM.ListEducation);
		}
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEducation(HREmpEduc ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
			OnDataEducation();
			try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    var msg = BSM.OnGridModifyEducation(ModelObject, SYActionBehavior.DELETE.ToString());
                    if (msg == SYConstant.OK)
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridEducation", BSM.ListEducation);
        }
        public ActionResult UploadControlCallbackActionEdu(HttpPostedFileBase file_Uploader)
		{
			UserSession();
			var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
			SYFileImport sfi = new SYFileImport(path);
			sfi.AttachmentMulti = "";
			sfi.DocumentMulti = "";
			sfi.ScreenID = SCREEN_ID;
			sfi.UploadBy = user.UserName;
			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadEdu",
				sfi.ValidationSettings,
				sfi.uc_FileUploadCompleteMultiFile);
			Session[PATH_FILE_Education] = sfi.AttachmentMulti;
			return null;
		}
		[HttpPost]
		public ActionResult DeleteFileEdu(long TranNo, string filePath)
		{
			try
			{
				var Edu = unitOfWork.Set<HREmpEduc>().FirstOrDefault(f => f.TranNo == TranNo);
				if (Edu == null)
				{
					return Json(new { success = false, message = "Education record not found." });
				}

				string updatedAttachFile = string.Empty;
				if (!string.IsNullOrEmpty(Edu.AttachFile))
				{
					if (Session["SessionFileEdu"] != null)
					{
						Edu.AttachFile = Session["SessionFileEdu"].ToString();
					}
					var filePaths = Edu.AttachFile.Split(';').ToList();
					//filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
					filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					Edu.AttachFile = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
					updatedAttachFile = Edu.AttachFile;
					Session["SessionFileEdu"] = updatedAttachFile;
				}

				return Json(new { success = true, updatedAttachFile = updatedAttachFile });
			}
			catch (Exception e)
			{
				return Json(new { success = false, message = e.Message });
			}
		}

		#endregion
		#region Contract
		public ActionResult GridContractDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			OnDataContract();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridContract", BSM.ListContract);
		}
		public ActionResult GridContract()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataContract();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridContract", BSM.ListContract);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateContract(HREmpContract ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataContract();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					if (Session[PATH_FILE_Contract] != null)
					{
						ModelObject.ContractPath = Session[PATH_FILE_Contract].ToString();
					}
					var msg = BSM.OnGridModifyContract(ModelObject, SYActionBehavior.ADD.ToString());
					if (msg == SYConstant.OK)
					{
						Session[PATH_FILE_Contract] = null;
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridContract", BSM.ListContract);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditContract(HREmpContract ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataContract();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
				bool IsEdit = false;
				bool IsAddNewFile = false;
				if (Session[PATH_FILE_Contract] != null)
				{
					ModelObject.ContractPath = Session[PATH_FILE_Contract].ToString();
					IsEdit = true;
					IsAddNewFile = true;
				}
				else if (Session["SessionFileContract"] != null)
				{
					ModelObject.ContractPath = Session["SessionFileContract"].ToString();
					IsEdit = true;
				}
				var msg = BSM.OnGridModifyContract(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit,IsAddNewFile);
				if (msg == SYConstant.OK)
				{
					Session[PATH_FILE_Contract] = null;
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridContract", BSM.ListContract);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteContract(HREmpContract ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataContract();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					var msg = BSM.OnGridModifyContract(ModelObject, SYActionBehavior.DELETE.ToString());
					if (msg == SYConstant.OK)
					{
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridContract", BSM.ListContract);
		}
		public ActionResult UploadControlCallbackActionContract(HttpPostedFileBase file_Uploader)
		{
			UserSession();
			var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
			SYFileImport sfi = new SYFileImport(path);
			sfi.AttachmentMulti = "";
			sfi.DocumentMulti = "";
			sfi.ScreenID = SCREEN_ID;
			sfi.UploadBy = user.UserName;
			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadContract",
				sfi.ValidationSettings,
				sfi.uc_FileUploadCompleteMultiFile);
			Session[PATH_FILE_Contract] = sfi.AttachmentMulti;
			return null;
		}

        [HttpPost]
        public ActionResult DeleteFileContract(long TranNo, string filePath)
        {
            try
            {
                var Contract = unitOfWork.Set<HREmpContract>().FirstOrDefault(f => f.TranNo == TranNo);
                if (Contract == null)
                {
                    return Json(new { success = false, message = "Contract record not found." });
                }

                string updatedAttachFile = string.Empty;
                if (!string.IsNullOrEmpty(Contract.ContractPath))
                {
                    if (Session["SessionFileContract"] != null)
                    {
						Contract.ContractPath = Session["SessionFileContract"].ToString();
                    }
                    var filePaths = Contract.ContractPath.Split(';').ToList();
                    //filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
                    filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					Contract.ContractPath = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
                    updatedAttachFile = Contract.ContractPath;
                    Session["SessionFileContract"] = updatedAttachFile;
                }

                return Json(new { success = true, updatedAttachFile = updatedAttachFile });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });
            }
        }

        #endregion
        #region Delay Probation
        public ActionResult GridProbationDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			//OnDataContract();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridProbationDetail", BSM.ListDelayProbation);
		}
		#endregion
		#region Disciplinary
		public ActionResult GridItemDiscipDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			OnDataDisciplinary();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemDiscipDetail", BSM.ListEmpDisciplinary);
		}
		public ActionResult GridItemDiscip()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataDisciplinary();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemDiscip", BSM.ListEmpDisciplinary);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateItemDiscip(HREmpDisciplinary ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataDisciplinary();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					if (Session[PATH_FILE_Disciplinary] != null)
					{
						ModelObject.AttachPath = Session[PATH_FILE_Disciplinary].ToString();
					}
					var msg = BSM.OnGridModifyDisciplinary(ModelObject, SYActionBehavior.ADD.ToString());
					if (msg == SYConstant.OK)
					{
						Session[PATH_FILE_Disciplinary] = null;
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemDiscip", BSM.ListEmpDisciplinary);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditItemDiscip(HREmpDisciplinary ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataDisciplinary();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
				bool IsEdit = false;
				bool IsAddNewFile = false;
				if (Session[PATH_FILE_Disciplinary] != null)
				{
					ModelObject.AttachPath = Session[PATH_FILE_Disciplinary].ToString();
					IsEdit = true;
					IsAddNewFile = true;
				}
				else if (Session["SessionFileDisciplinary"] != null)
				{
					ModelObject.AttachPath = Session["SessionFileDisciplinary"].ToString();
					IsEdit = true;
				}
				var msg = BSM.OnGridModifyDisciplinary(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit,IsAddNewFile);
				if (msg == SYConstant.OK)
				{
					Session[PATH_FILE_Disciplinary] = null;
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridItemDiscip", BSM.ListEmpDisciplinary);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteItemDiscip(HREmpDisciplinary ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataDisciplinary();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					var msg = BSM.OnGridModifyDisciplinary(ModelObject, SYActionBehavior.DELETE.ToString());
					if (msg == SYConstant.OK)
					{
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemDiscip", BSM.ListEmpDisciplinary);
		}
		public ActionResult UploadControlCallbackActionDisciplinary(HttpPostedFileBase file_Uploader)
		{
			UserSession();
			var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
			SYFileImport sfi = new SYFileImport(path);
			sfi.AttachmentMulti = "";
			sfi.DocumentMulti = "";
			sfi.ScreenID = SCREEN_ID;
			sfi.UploadBy = user.UserName;
			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadDisciplinary",
				sfi.ValidationSettings,
				sfi.uc_FileUploadCompleteMultiFile);
			Session[PATH_FILE_Disciplinary] = sfi.AttachmentMulti;
			return null;
		}

		[HttpPost]
		public ActionResult DeleteFileDisciplinary(long TranNo, string filePath)
		{
			try
			{
				var Contract = unitOfWork.Set<HREmpDisciplinary>().FirstOrDefault(f => f.TranNo == TranNo);
				if (Contract == null)
				{
					return Json(new { success = false, message = "Disciplinary record not found." });
				}

				string updatedAttachFile = string.Empty;
				if (!string.IsNullOrEmpty(Contract.AttachPath))
				{
					if (Session["SessionFileDisciplinary"] != null)
					{
						Contract.AttachPath = Session["SessionFileDisciplinary"].ToString();
					}
					var filePaths = Contract.AttachPath.Split(';').ToList();
					//filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
					filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					Contract.AttachPath = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
					updatedAttachFile = Contract.AttachPath;
					Session["SessionFileDisciplinary"] = updatedAttachFile;
				}

				return Json(new { success = true, updatedAttachFile = updatedAttachFile });
			}
			catch (Exception e)
			{
				return Json(new { success = false, message = e.Message });
			}
		}

		#endregion
		#region Certificate
		public ActionResult GridItemCertificateDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			OnDataCertificate();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemCertificateDetail", BSM.ListEmpCertificate);
		}
		public ActionResult GridItemCertificate()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataCertificate();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemCertificate", BSM.ListEmpCertificate);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateItemCertificate(HREmpCertificate ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataCertificate();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					if (Session[PATH_FILE_Certificate] != null)
					{
						ModelObject.AttachFile = Session[PATH_FILE_Certificate].ToString();
					}
					var msg = BSM.OnGridModifyCertificate(ModelObject, SYActionBehavior.ADD.ToString());
					if (msg == SYConstant.OK)
					{
						Session[PATH_FILE_Certificate] = null;
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemCertificate", BSM.ListEmpCertificate);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditItemCertificate(HREmpCertificate ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataCertificate();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
				bool IsEdit = false;
				bool IsAddNewFile = false;
				if (Session[PATH_FILE_Certificate] != null)
				{
					ModelObject.AttachFile = Session[PATH_FILE_Certificate].ToString();
					IsEdit = true;
					IsAddNewFile = true;
				}
				else if (Session["SessionFileCertificate"] != null)
				{
					ModelObject.AttachFile = Session["SessionFileCertificate"].ToString();
					IsEdit = true;
				}
				var msg = BSM.OnGridModifyCertificate(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit,IsAddNewFile);
				if (msg == SYConstant.OK)
				{
					Session[PATH_FILE_Certificate] = null;
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridItemCertificate", BSM.ListEmpCertificate);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteItemCertificate(HREmpCertificate ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataCertificate();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					var msg = BSM.OnGridModifyCertificate(ModelObject, SYActionBehavior.DELETE.ToString());
					if (msg == SYConstant.OK)
					{
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemCertificate", BSM.ListEmpCertificate);
		}
		public ActionResult UploadControlCallbackActionCertificate(HttpPostedFileBase file_Uploader)
		{
			UserSession();
			var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
			SYFileImport sfi = new SYFileImport(path);
			sfi.AttachmentMulti = "";
			sfi.DocumentMulti = "";
			sfi.ScreenID = SCREEN_ID;
			sfi.UploadBy = user.UserName;
			UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadCertificate",
				sfi.ValidationSettings,
				sfi.uc_FileUploadCompleteMultiFile);
			Session[PATH_FILE_Certificate] = sfi.AttachmentMulti;
			return null;
		}

		[HttpPost]
		public ActionResult DeleteFileCertificate(long TranNo, string filePath)
		{
			try
			{
				var Contract = unitOfWork.Set<HREmpCertificate>().FirstOrDefault(f => f.TranNo == TranNo);
				if (Contract == null)
				{
					return Json(new { success = false, message = "Certificate record not found." });
				}

				string updatedAttachFile = string.Empty;
				if (!string.IsNullOrEmpty(Contract.AttachFile))
				{
					if (Session["SessionFileCertificate"] != null)
					{
						Contract.AttachFile = Session["SessionFileCertificate"].ToString();
					}
					var filePaths = Contract.AttachFile.Split(';').ToList();
					//filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && path != filePath).ToList();
					filePaths = filePaths.Where(path => !string.IsNullOrEmpty(path) && Path.GetFileName(path) != Path.GetFileName(filePath)).ToList();
					Contract.AttachFile = string.Join(";", filePaths) + (filePaths.Any() ? ";" : "");
					updatedAttachFile = Contract.AttachFile;
					Session["SessionFileCertificate"] = updatedAttachFile;
				}

				return Json(new { success = true, updatedAttachFile = updatedAttachFile });
			}
			catch (Exception e)
			{
				return Json(new { success = false, message = e.Message });
			}
		}
		#endregion
		#region Medical
		public ActionResult GridItemMedicalDetail()
		{
			ActionName = "Details";
			UserSession();
			UserConfListAndForm();
			OnDataMedical();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemMedicalDetail", BSM.ListEmpMedical);
		}
		public ActionResult GridItemMedical()
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataMedical();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
			}
			return PartialView("GridItemMedical", BSM.ListEmpMedical);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateItemMedical(HREmpPhischk ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataMedical();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					var msg = BSM.OnGridModifyMedical(ModelObject, SYActionBehavior.ADD.ToString());
					if (msg == SYConstant.OK)
					{
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemMedical", BSM.ListEmpMedical);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult EditItemMedical(HREmpPhischk ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataMedical();
			if (Session[Index_Sess_Obj + ActionName] != null)
			{
				BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
				bool IsEdit = false;
				var msg = BSM.OnGridModifyMedical(ModelObject, SYActionBehavior.EDIT.ToString(), IsEdit);
				if (msg == SYConstant.OK)
				{
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
				else
				{
					ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
				}
				Session[Index_Sess_Obj + ActionName] = BSM;
			}
			return PartialView("GridItemMedical", BSM.ListEmpMedical);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteItemMedical(HREmpPhischk ModelObject)
		{
			ActionName = "Create";
			UserSession();
			UserConfListAndForm();
			OnDataMedical();
			try
			{
				if (Session[Index_Sess_Obj + ActionName] != null)
				{
					BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
					var msg = BSM.OnGridModifyMedical(ModelObject, SYActionBehavior.DELETE.ToString());
					if (msg == SYConstant.OK)
					{
						Session[Index_Sess_Obj + ActionName] = BSM;
					}
					else
					{
						ViewData["EditError"] = SYMessages.getMessage(msg, user.Lang);
					}
					Session[Index_Sess_Obj + ActionName] = BSM;
				}
			}
			catch (Exception e)
			{
				ViewData["EditError"] = e.Message;
			}
			return PartialView("GridItemMedical", BSM.ListEmpMedical);
		}
        #endregion
        #region "Saparate Bank"
        public ActionResult GridItemBank()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            if (string.IsNullOrEmpty(BSM.Header.LevelCode))
                BSM.Header.LevelCode = BSM.IsSalary;
            bool isSalaryVisible = BSM.IsHideSalary(BSM.Header.LevelCode);
            ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemBank", BSM.ListSaparate);
        }
        public ActionResult GridItemBankEdit()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            if (string.IsNullOrEmpty(BSM.Header.LevelCode))
                BSM.Header.LevelCode = BSM.IsSalary;
            bool isSalaryVisible = BSM.IsHideSalary(BSM.Header.LevelCode);
            ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemBankEdit", BSM.ListSaparate);
        }
        public ActionResult saparateDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            bool isSalaryVisible = BSM.IsHideSalary(BSM.Header.LevelCode);
            ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;
            return PartialView("GridItemBankDetial", BSM.ListSaparate);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBank(HREmpBankAcc ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    if (!string.IsNullOrEmpty(ModelObject.Company))
                    {
                        int id = 0;
                        if (BSM.ListSaparate.Count > 0) id = BSM.ListSaparate.Max(w => w.LineItem);
                        if (BSM.ListSaparate.Where(w => w.Company == ModelObject.Company).Count() == 0)
                        {
                            ModelObject.LineItem = id + 1;
                            BSM.ListSaparate.Add(ModelObject);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                        }
                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("INV_DOC");
                    }
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataSelector();
            return PartialView("GridItemBank", BSM.ListSaparate);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBank(HREmpBankAcc ObjType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListSaparate.Where(w => w.Company == ObjType.Company).ToList();
                var isSalaryVisible = BSM.IsHideSalary(BSM.Header.LevelCode);
                ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;

                if (listCheck.Count > 0)
                {
                    var objUpdate = listCheck.First();
                    objUpdate.Company = ObjType.Company;
                    objUpdate.BankName = ObjType.BankName;
                    objUpdate.AccountNo = ObjType.AccountNo;
                    objUpdate.AccountName = ObjType.AccountName;
                    objUpdate.Salary = ObjType.Salary;
                    objUpdate.IsTax = ObjType.IsTax;
                    objUpdate.IsNSSF = ObjType.IsNSSF;
                    objUpdate.Association = ObjType.Association;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }

            DataSelector();
            return PartialView("GridItemBank", BSM.ListSaparate);
        }
        public ActionResult EditBankEdit(HREmpBankAcc ObjType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListSaparate.Where(w => w.Company == ObjType.Company).ToList();
                var isSalaryVisible = BSM.IsHideSalary(BSM.Header.LevelCode);
                ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;

                if (listCheck.Count > 0)
                {
                    var objUpdate = listCheck.First();
                    objUpdate.Company = ObjType.Company;
                    objUpdate.BankName = ObjType.BankName;
                    objUpdate.AccountNo = ObjType.AccountNo;
                    objUpdate.AccountName = ObjType.AccountName;
                    objUpdate.Salary = ObjType.Salary;
                    objUpdate.IsTax = ObjType.IsTax;
                    objUpdate.IsNSSF = ObjType.IsNSSF;
                    objUpdate.Association = ObjType.Association;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }

            DataSelector();
            return PartialView("GridItemBankEdit", BSM.ListSaparate);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBank(HREmpBankAcc ObjType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                }
                var error = 0;

                var checkList = BSM.ListSaparate.Where(w => w.LineItem == ObjType.LineItem).ToList();
                if (checkList.Count == 0)
                {
                    ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                    error = 1;
                }

                if (error == 0)
                {
                    BSM.ListSaparate.Remove(checkList.First());
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            DataSelector();

            return PartialView("GridItemBank", BSM.ListSaparate);
        }
        #endregion
        #region "Import"
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(this.KeyName);
            SYUpLoadTemplate upLoadTemplate = new SYUpLoadTemplate();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRStaffProfile", SYSConstant.DEFAULT_UPLOAD_LIST);
            BSM.ListTemplate = upLoadTemplate.GetData(SCREEN_ID, user.UserName);

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRStaffProfile", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(unitOfWork.Set<CFUploadPath>().Find("STAFF"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRStaffProfile", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYUpLoadTemplate upLoadTemplate = new SYUpLoadTemplate();
            IEnumerable<MDUploadTemplate> listu = upLoadTemplate.GetData(SCREEN_ID, user.UserName);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }
        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = unitOfWork.Set<MDUploadTemplate>().Find(id);
            if (obj != null)
            {
                BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dt = excel.GenerateExcelData();
                if (obj.IsGenerate == true)
                {
                    SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }
                if (dt != null)
                {
                    BSM.ScreenId = ScreendIDControl;
                    try
                    {
                        List<ExCfFileTemplate> listVersion = unitOfWork.Set<ExCfFileTemplate>().Where(w => w.ScreenId == SCREEN_ID && w.IsActive == true).ToList();
                        if (listVersion.Count == 0)
                        {
                            SYMessages mess = SYMessages.getMessageObject("UPLOAD_CONF_NE", user.Lang);
                            Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                        }
                        ExCfFileTemplate objversion = listVersion.First();
                        List<ExCFUploadMapping> listTemplate = unitOfWork.Set<ExCFUploadMapping>().Where(w => w.ScreenID == SCREEN_ID && w.Version == objversion.Version).OrderBy(w => w.FiledIndex).ToList();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            List<ExCFUploadMapping> checkList = listTemplate.Where(w => w.FiledIndex == i).ToList();
                            if (checkList.Count > 0)
                            {
                                ExCFUploadMapping objCol = checkList.First();
                                dt.Columns[i].ColumnName = objCol.TableName + "." + objCol.FieldName;
                            }
                        }
                        foreach (IGrouping<string, ExCFUploadMapping> table in listTemplate.GroupBy(w => w.TableName))
                        {
                            if (table.Key == "HRStaffProfile")
                            {
                                BSM.ListStaffProfile = ClsConstant.ConvertDataTableName<HRStaffProfile>(dt, table.Key);
                            }
                            if (table.Key == "HREmpIdentity")
                            {
                                BSM.ListEmpIdentityUP = ClsConstant.ConvertDataTableName<HREmpIdentity>(dt, table.Key);
                            }
                        }
                        string msg = BSM.CreateMulti();
                        if (msg == SYConstant.OK)
                        {
                            obj.Message = SYConstant.OK;
                            obj.DocumentNo = DocBatch.NextNumberRank;
                            obj.IsGenerate = true;
                            unitOfWork.Update(obj);
                            unitOfWork.Save();
                        }
                        else
                        {
                            obj.Message = SYMessages.getMessage(msg);
                            obj.Message += ":" + BSM.MessageError;
                            obj.IsGenerate = false;
                            unitOfWork.Update(obj);
                            unitOfWork.Save();
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                        }
                    }
                    catch (Exception e)
                    {
                        obj.Message = ClsEventLog.Save_EventLogs(SCREEN_ID, user.UserName, "UPLOAD", SYActionBehavior.ADD.ToString(), e, true);
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                    }
                }
                else
                {
                    obj.Message = SYMessages.getMessage("EX_DT");
                    obj.IsGenerate = false;
                    unitOfWork.Update(obj);
                    unitOfWork.Save();
                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }
        public ActionResult DownloadTemplate()
        {
            UserSession();
            List<ExCfFileTemplate> listVersion = unitOfWork.Set<ExCfFileTemplate>().Where(w => w.ScreenId == SCREEN_ID && w.IsActive == true).ToList();
            if (listVersion.Count > 0)
            {
                ExCfFileTemplate objversion = listVersion.First();
                SYSGridSettingExport gSetting = new SYSGridSettingExport();
                gSetting.GridName = "DownloadTemplate" + SCREEN_ID;

                gSetting.CallBackControllerName = "";
                gSetting.CallBackActionName = "";
                gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
                gSetting.ListConfigure = SYSettings.getListConf(SCREEN_ID, user.Lang);
                GridViewExportFormat format = GridViewExportFormat.Xlsx;
                if (SYExportFile.ExportFormatsInfo.ContainsKey(format))
                {
                    List<ExCfFileTemplate> DList = new List<ExCfFileTemplate>();

                    return SYExportFile.ExportFormatsInfo[format](ClsConstant.CreateExportGridViewSettings(gSetting, SCREEN_ID, objversion.Version, objversion.Description),
                         DList);
                }
            }
            return RedirectToAction("Index");
        }

        #endregion
        [HttpPost]
        public ActionResult ShowData(DateTime StartDate, string ProType)
        {
            DateTime Probation = BSM.GetProbationDate(StartDate, ProType);
            DateTime LeaveCof = StartDate;
            var result = new
            {
                MS = SYConstant.OK,
                Probation = Probation,
                LeaveCof = LeaveCof,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }

		#region "Province"

		[HttpPost]
        public string FitlerType(string code, int addType)
        {

            UserSession();
            UserConfListAndForm();
            if (ClsInformation.IsNumeric(code))
            {
                if (addType == 1)
                {
                    Session["S_PROVINCE"] = code;
                }
                else if (addType == 2)
                {
                    Session["DISTRICT"] = code;
                }
                else if (addType == 3)
                {
                    Session["COMMUNE"] = code;
                }
            }

            return SYConstant.OK;
        }

        public ActionResult District()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "District" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Employee.ClsEmployee.District());
            });
        }

        public ActionResult Commune()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "Commune" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Employee.ClsEmployee.Commune());
            });

        }
        public ActionResult Village()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "Village" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Employee.ClsEmployee.Village());
            });
        }
        public void FitlertDataProvince(string PROVINCE, string DISTRICT, string COMMUNE)
        {
            FitlerType(PROVINCE, 1);
            FitlerType(DISTRICT, 2);
            FitlerType(COMMUNE, 3);
        }
        #endregion

        #region JobType
        [HttpPost]
        public ActionResult FilterLevel(string Level)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            var IsSalary = BSM.IsHideSalary(Level);
            BSM.Header.LevelCode = Level;
            Session[Index_Sess_Obj + ActionName] = BSM;
            var result = new
            {
                MS = SYConstant.OK,
                IsSalary = IsSalary,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        //[HttpPost]
        //public ActionResult JobType(string code, string addType)
        //{
        //    UserSession();
        //    UserConfListAndForm();
        //    if (!string.IsNullOrEmpty(code))
        //    {
        //        var _listCom = DB.HRCompanyGroups.Where(w => w.ParentWorkGroupID == addType).ToList();
        //        string Res = "";
        //        if (_listCom.Count() > 0)
        //        {
        //            var obj = _listCom.FirstOrDefault();
        //            Res = obj.WorkGroup;
        //            if (obj.WorkGroup == "Division")
        //                Session["Division"] = code;
        //            else if (obj.WorkGroup == "GroupDepartment")
        //                Session["GroupDepartment"] = code;
        //            else if (obj.WorkGroup == "Department")
        //                Session["Department"] = code;
        //            else if (obj.WorkGroup == "Position")
        //                Session["Position"] = code;
        //            else if (obj.WorkGroup == "Section")
        //                Session["Section"] = code;
        //            else if (obj.WorkGroup == "Level")
        //                Session["Level"] = code;
        //            else if (obj.WorkGroup == "JobGrade")
        //                Session["JobGrade"] = code;
        //        }
        //        var result = new
        //        {
        //            MS = Res,
        //        };
        //        return Json(result, JsonRequestBehavior.DenyGet);
        //    }
        //    var rs = new { MS = SYConstant.FAIL };
        //    return Json(rs, JsonRequestBehavior.DenyGet);
        //}
        //public ActionResult GetDivision()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetDivision" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{0}:{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.GetDivision());
        //    });
        //}
        //public ActionResult GetDepartment()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetDepartment" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.GetDepartment());
        //    });
        //}
        //public ActionResult GetPosition()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetPosition" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(HRStaffProfileObject.GetPosition());
        //    });
        //}
        //public ActionResult GetSection()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetSection" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(HRStaffProfileObject.GetSection());
        //    });
        //}
        //public ActionResult GetLevel()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetLevel" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(HRStaffProfileObject.GetLevel());
        //    });
        //}
        //public ActionResult GetJobGrade()
        //{
        //    UserSession();
        //    return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
        //    {
        //        cboProperties.CallbackRouteValues = new { Controller = "HRStaffProfile", Action = "GetJobGrade" };
        //        cboProperties.Width = Unit.Percentage(100);
        //        cboProperties.ValueField = "Code";
        //        cboProperties.TextField = "Description";
        //        cboProperties.TextFormatString = "{1}";
        //        cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
        //        cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
        //        cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
        //        cboProperties.BindList(HRStaffProfileObject.GetJobGrade());
        //    });
        //}
        //public void FitlertDataSession(string Branch, string Division, string GDept, string Dept,
        //  string Section, string Position, string Level, string Grade)
        //{
        //    JobType(Branch, "Branch");
        //    JobType(Division, "Division");
        //    JobType(GDept, "GroupDepartment");
        //    JobType(Dept, "Department");
        //    JobType(Section, "Section");
        //    JobType(Position, "Position");
        //    JobType(Level, "Level");
        //    JobType(Grade, "JobGrade");
        //}
        #endregion

        private void DataList()
        {
            foreach (var data in BSM.OnDataListLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
        private void DataSelector(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            foreach (var data in BSM.OnDataPersonal())
            {
                ViewData[data.Key] = data.Value;
            }
            foreach (var data in BSM.OnDataJobLoading(IsDetail))
            {
                ViewData[data.Key] = data.Value;
            }
            foreach (var data in BSM.OnDataLoadingOther())
            {
                ViewData[data.Key] = data.Value;
            }
            //foreach (var data in BSM.OnDataFamily())
            //{
            //    ViewData[data.Key] = data.Value;
            //}
			//foreach (var data in BSM.OnDataEducation())
			//{
			//	ViewData[data.Key] = data.Value;
			//}
			//foreach (var data in BSM.OnDataContract())
			//{
			//	ViewData[data.Key] = data.Value;
			//}
		}
        private void DataFamily()
        {
            foreach (var data in BSM.OnDataFamily())
            {
                ViewData[data.Key] = data.Value;
            }
        }
		private void OnDataEducation()
		{
			foreach (var data in BSM.OnDataEducation())
			{
				ViewData[data.Key] = data.Value;
			}
		}
		private void OnDataContract()
		{
			foreach (var data in BSM.OnDataContract())
			{
				ViewData[data.Key] = data.Value;
			}
		}
		private void OnDataDisciplinary()
		{
			foreach (var data in BSM.OnDataDisciplinary())
			{
				ViewData[data.Key] = data.Value;
			}
		}
		private void OnDataCertificate()
		{
			foreach (var data in BSM.OnDataCertificate())
			{
				ViewData[data.Key] = data.Value;
			}
		}
		private void OnDataMedical()
		{
			foreach (var data in BSM.OnDataMedical())
			{
				ViewData[data.Key] = data.Value;
			}
		}
		private void SetSessionNULL()
        {
            Session["Division"] = null;
            Session["GroupDepartment"] = null;
            Session["Department"] = null;
            Session["Position"] = null;
            Session["Section"] = null;
            Session["JobGrade"] = null;
            Session["Level"] = null;
            Session["ObjValue"] = null;
        }
    }
}