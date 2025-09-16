using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.HRS;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class EmpInforSetupController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000002";
        private const string URL_SCREEN = "/HRM/HRS/EmpInforSetup";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILE_KH = "KH12313123123sadfsdfsdfsdf";
        private string PATH_FILE_C = "C12313123123sadfsdfsdfsdf";
        private string PATH_FILE_CKH = "CKH12313123123sadfsdfsdfsdf";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public EmpInforSetupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListCountry = DB.HRCountries.ToList();
            BSM.ListNation = DB.HRNations.ToList();
            BSM.ListProvine = DB.HRProvices.ToList();
            BSM.ListRelationshipType = DB.HRRelationshipTypes.ToList();
            BSM.ListEduType = DB.HREduTypes.ToList();
            BSM.ListCertificationType = DB.HRCertificationTypes.ToList();
			BSM.ListIdentityType = DB.HRIdentityTypes.ToList();
			BSM.ListContractType = DB.HRContractTypes.ToList();
            BSM.ListCareerHistory = DB.HRCareerHistories.ToList();
            BSM.ListWorkingType = DB.HRWorkingTypes.ToList();
            BSM.ListBloodType = DB.HRBloodTypes.ToList();
            BSM.ListDisciplinAction = DB.HRDisciplinActions.ToList();
            BSM.ListDisciplinType = DB.HRDisciplinTypes.ToList();
            BSM.ListSeparateType = DB.HRTerminTypes.ToList();
            BSM.ListInsuranceCompany = DB.HRInsuranceCompanies.ToList();
            BSM.ListAnnounementType = DB.HRAnnounceTypes.ToList();
            return View(BSM);
        }

        #region Country
        public ActionResult GridItemCountrys()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListCountry = DB.HRCountries.ToList();
            return PartialView("GridItemCountrys", BSM.ListCountry);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCountry(HRCountry MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRCountries.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListCountry = DB.HRCountries.ToList();
            return PartialView("GridItemCountrys", BSM.ListCountry);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCountry(HRCountry MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRCountries.Find(MModel.Code);
                    DB.HRCountries.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListCountry = DB.HRCountries.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemCountrys", BSM.ListCountry);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCountry(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.Country == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRCountries.Find(Code);
                        if (obj != null)
                        {
                            DB.HRCountries.Remove(obj);
                            int row = DB.SaveChanges();

                        }
                        BSM.ListCountry = DB.HRCountries.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemCountrys", BSM.ListCountry);
        }
        #endregion

        #region Nation
        public ActionResult GridItemNations()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListNation = DB.HRNations.ToList();
            return PartialView("GridItemNations", BSM.ListNation);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateNation(HRNation MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRNations.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListNation = DB.HRNations.ToList();
            return PartialView("GridItemNations", BSM.ListNation);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditNation(HRNation MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRNations.Find(MModel.Code);
                    DB.HRNations.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListNation = DB.HRNations.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemNations", BSM.ListNation);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteNation(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.Nation == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRNations.Find(Code);
                        if (obj != null)
                        {
                            DB.HRNations.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListNation = DB.HRNations.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemNations", BSM.ListNation);
        }
        #endregion

        #region Provine
        public ActionResult GridItemProvines()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListProvine = DB.HRProvices.ToList();
            return PartialView("GridItemProvines", BSM.ListProvine);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateProvine(HRProvice MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRProvices.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListProvine = DB.HRProvices.ToList();
            return PartialView("GridItemProvines", BSM.ListProvine);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditProvine(HRProvice MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRProvices.Find(MModel.Code);
                    DB.HRProvices.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListProvine = DB.HRProvices.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemProvines", BSM.ListProvine);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteProvine(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {

                    var obj = DB.HRProvices.Find(Code);
                    if (obj != null)
                    {
                        DB.HRProvices.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListProvine = DB.HRProvices.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemProvines", BSM.ListProvine);
        }
        #endregion

        #region Edu Type

        public ActionResult GridItemEduTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListEduType = DB.HREduTypes.ToList();
            return PartialView("GridviewEduType", BSM.ListEduType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEduType(HREduType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HREduTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListEduType = DB.HREduTypes.ToList();
            return PartialView("GridviewEduType", BSM.ListEduType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEduType(HREduType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRDivisions.Find(MModel.Code);
                    DB.HREduTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListEduType = DB.HREduTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewEduType", BSM.ListEduType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEduType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpEducs.ToList();
                    if (objEmp.Where(w => w.EduType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HREduTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HREduTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListEduType = DB.HREduTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewEduType", BSM.ListEduType);
        }

        #endregion

        #region Certification Type

        public ActionResult GridItemCertificationTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListCertificationType = DB.HRCertificationTypes.ToList();
            return PartialView("GridviewCertificationType", BSM.ListCertificationType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCertificationType(HRCertificationType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRCertificationTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListCertificationType = DB.HRCertificationTypes.ToList();
            return PartialView("GridviewCertificationType", BSM.ListCertificationType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCertificationType(HRCertificationType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRCertificationTypes.Find(MModel.Code);
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        MModel.TemplatePath = objUpdate.TemplatePath;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    else
                    {
                        MModel.TemplatePathKH = objUpdate.TemplatePathKH;
                    }
                    DB.HRCertificationTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListCertificationType = DB.HRCertificationTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewCertificationType", BSM.ListCertificationType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCertificationType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var obj = DB.HRCertificationTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.HRCertificationTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListCertificationType = DB.HRCertificationTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewCertificationType", BSM.ListCertificationType);
        }

		#endregion

		#region Identity Type
		public ActionResult GridItemIdentityType()
		{
			UserConf(ActionBehavior.EDIT);

			MDEmpInfor BSM = new MDEmpInfor();
			BSM.ListIdentityType = DB.HRIdentityTypes.ToList();
			return PartialView("GridItemIdentityType", BSM.ListIdentityType);
		}
		[HttpPost, ValidateInput(false)]
		public ActionResult CreateIdentityType(HRIdentityType MModel)
		{
			UserSession();
			MDEmpInfor BSM = new MDEmpInfor();
			if (ModelState.IsValid)
			{
				try
				{
					MModel.Code = MModel.Code.ToUpper();
					DB.HRIdentityTypes.Add(MModel);
					int row = DB.SaveChanges();
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
			BSM.ListIdentityType = DB.HRIdentityTypes.ToList();
			return PartialView("GridItemIdentityType", BSM.ListIdentityType);
		}
		//edit
		[HttpPost, ValidateInput(false)]
		public ActionResult EditIdentityType(HRIdentityType MModel)
		{
			UserSession();
			MDEmpInfor BSM = new MDEmpInfor();
			if (ModelState.IsValid)
			{
				try
				{
					var DBU = new HumicaDBContext();
					var objUpdate = DBU.HRIdentityTypes.Find(MModel.Code);
					DB.HRIdentityTypes.Attach(MModel);
					DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
					int row = DB.SaveChanges();
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
			BSM.ListIdentityType = DB.HRIdentityTypes.OrderBy(w => w.Code).ToList();
			return PartialView("GridItemIdentityType", BSM.ListIdentityType);
		}
		//delete
		[HttpPost, ValidateInput(false)]
		public ActionResult DeleteIdentityType(string Code)
		{
			//UserSession();
			//MDEmpInfor BSM = new MDEmpInfor();
			//if (Code != null)
			//{
			//	try
			//	{

			//		var obj = DB.HRIdentityTypes.Find(Code);
			//		if (obj != null)
			//		{
			//			DB.HRIdentityTypes.Remove(obj);
			//			int row = DB.SaveChanges();
			//		}

			//	}
			//	catch (Exception e)
			//	{
			//		ViewData["EditError"] = e.Message;
			//	}
			//}
			UserSession();
			MDEmpInfor BSM = new MDEmpInfor();
			if (Code != null)
			{
				try
				{
					var objEmp = DB.HREmpIdentities.ToList();
					if (objEmp.Where(w => w.IdentityTye == Code).Any())
					{
						ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
					}
					else
					{
						var obj = DB.HRIdentityTypes.Find(Code);
						if (obj != null)
						{
							DB.HRIdentityTypes.Remove(obj);
							int row = DB.SaveChanges();
						}
						BSM.ListIdentityType = DB.HRIdentityTypes.OrderBy(w => w.Code).ToList();
					}
				}
				catch (Exception e)
				{
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("GridItemIdentityType", BSM.ListIdentityType);
		}
		#endregion

		#region Contract Type

		public ActionResult GridItemContractTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListContractType = DB.HRContractTypes.ToList();
            return PartialView("GridviewContractType", BSM.ListContractType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateContractType(HRContractType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplateNameKhm = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRContractTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListContractType = DB.HRContractTypes.ToList();
            return PartialView("GridviewContractType", BSM.ListContractType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditContractType(HRContractType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRContractTypes.Find(MModel.Code);
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        MModel.TemplatePath = objUpdate.TemplatePath;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplateNameKhm = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    else
                    {
                        MModel.TemplateNameKhm = objUpdate.TemplateNameKhm;
                    }
                    DB.HRContractTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListContractType = DB.HRContractTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewContractType", BSM.ListContractType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteContractType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {

                    var obj = DB.HRContractTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.HRContractTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListContractType = DB.HRContractTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewContractType", BSM.ListContractType);
        }

        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            var path = DBV.CFUploadPaths.Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }
        public ActionResult UploadControlCallbackActionKH(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = DBV.CFUploadPaths.Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPBKH",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE_KH] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }

        #endregion

        #region Relationship Type

        public ActionResult GridItemRelationshipTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListRelationshipType = DB.HRRelationshipTypes.ToList();
            return PartialView("GridviewRelationshipType", BSM.ListRelationshipType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateRelationshipType(HRRelationshipType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRRelationshipTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListRelationshipType = DB.HRRelationshipTypes.ToList();
            return PartialView("GridviewRelationshipType", BSM.ListRelationshipType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditRelationshipType(HRRelationshipType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRRelationshipTypes.Find(MModel.Code);
                    DB.HRRelationshipTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListRelationshipType = DB.HRRelationshipTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewRelationshipType", BSM.ListRelationshipType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteRelationshipType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpFamilies.ToList();
                    if (objEmp.Where(w => w.RelCode == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRRelationshipTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRRelationshipTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListRelationshipType = DB.HRRelationshipTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewRelationshipType", BSM.ListRelationshipType);
        }

        #endregion

        #region Career Code
        public ActionResult GridItemCareerHistorys()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListCareerHistory = DB.HRCareerHistories.ToList();
            return PartialView("GridviewCareerHistorys", BSM.ListCareerHistory);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCareerHistory(HRCareerHistory MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    DB.HRCareerHistories.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListCareerHistory = DB.HRCareerHistories.ToList();
            return PartialView("GridviewCareerHistorys", BSM.ListCareerHistory);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCareerHistory(HRCareerHistory MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRCareerHistories.Find(MModel.Code);
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                        MModel.TemplatePath = objUpdate.TemplatePath;
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    else
                        MModel.TemplatePathKH = objUpdate.TemplatePathKH;

                    DB.HRCareerHistories.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListCareerHistory = DB.HRCareerHistories.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewCareerHistorys", BSM.ListCareerHistory);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCareerHistory(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.CareerCode == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRCareerHistories.Find(Code);
                        if (obj.LCK == 0)
                        {
                            DB.HRCareerHistories.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListCareerHistory = DB.HRCareerHistories.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewCareerHistorys", BSM.ListCareerHistory);
        }
        #endregion

        #region Working Type

        public ActionResult GridItemWorkingTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListWorkingType = DB.HRWorkingTypes.ToList();
            return PartialView("GridviewWorkingType", BSM.ListWorkingType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateWorkingType(HRWorkingType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRWorkingTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListWorkingType = DB.HRWorkingTypes.ToList();
            return PartialView("GridviewWorkingType", BSM.ListWorkingType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditWorkingType(HRWorkingType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    //var objUpdate = DBU.HRDivisions.Find(MModel.Code);
                    DB.HRWorkingTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListWorkingType = DB.HRWorkingTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewWorkingType", BSM.ListWorkingType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteWorkingType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.StaffType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRWorkingTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRWorkingTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListWorkingType = DB.HRWorkingTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewWorkingType", BSM.ListWorkingType);
        }

        #endregion

        #region Blood Type

        public ActionResult GridItemBloodTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListBloodType = DB.HRBloodTypes.ToList();
            return PartialView("GridviewBloodType", BSM.ListBloodType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBloodType(HRBloodType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRBloodTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListBloodType = DB.HRBloodTypes.ToList();
            return PartialView("GridviewBloodType", BSM.ListBloodType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBloodType(HRBloodType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    //var objUpdate = DBU.HRDivisions.Find(MModel.Code);
                    DB.HRBloodTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListBloodType = DB.HRBloodTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewBloodType", BSM.ListBloodType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBloodType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.BloodType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRBloodTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRBloodTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListBloodType = DB.HRBloodTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewBloodType", BSM.ListBloodType);
        }

        #endregion

        #region DisciplinAction
        public ActionResult GridviewDisciplinType()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListDisciplinType = DB.HRDisciplinTypes.ToList();
            return PartialView("GridviewDisciplinType", BSM.ListDisciplinType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateDisciplinType(HRDisciplinType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRDisciplinTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListDisciplinType = DB.HRDisciplinTypes.ToList();
            return PartialView("GridviewDisciplinType", BSM.ListDisciplinType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditDisciplinType(HRDisciplinType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRDisciplinTypes.Find(MModel.Code);
                    DB.HRDisciplinTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListDisciplinType = DB.HRDisciplinTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewDisciplinType", BSM.ListDisciplinType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDisciplinType(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpDisciplinaries.ToList();
                    if (objEmp.Where(w => w.DiscType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRDisciplinTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRDisciplinTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListDisciplinType = DB.HRDisciplinTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewDisciplinType", BSM.ListDisciplinType);
        }
        #endregion

        #region DisciplinAction
        public ActionResult GridviewDisciplinAction()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListDisciplinAction = DB.HRDisciplinActions.ToList();
            return PartialView("GridviewDisciplinAction", BSM.ListDisciplinAction);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateDisciplinAction(HRDisciplinAction MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRDisciplinActions.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListDisciplinAction = DB.HRDisciplinActions.ToList();
            return PartialView("GridviewDisciplinAction", BSM.ListDisciplinAction);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditDisciplinAction(HRDisciplinAction MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRDisciplinActions.Find(MModel.Code);
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.TemplatePath = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        MModel.TemplatePath = objUpdate.TemplatePath;
                    }
                    if (Session[PATH_FILE_KH] != null)
                    {
                        MModel.TemplatePathKH = Session[PATH_FILE_KH].ToString();
                        Session[PATH_FILE_KH] = null;
                    }
                    else
                    {
                        MModel.TemplatePathKH = objUpdate.TemplatePathKH;
                    }
                    DB.HRDisciplinActions.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListDisciplinAction = DB.HRDisciplinActions.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewDisciplinAction", BSM.ListDisciplinAction);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDisciplinAction(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpDisciplinaries.ToList();
                    if (objEmp.Where(w => w.DiscAction == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRDisciplinActions.Find(Code);
                        if (obj != null)
                        {
                            DB.HRDisciplinActions.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListDisciplinAction = DB.HRDisciplinActions.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewDisciplinAction", BSM.ListDisciplinAction);
        }
        #endregion

        #region Separate Type
        public ActionResult GridviewSeparateType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListSeparateType = DB.HRTerminTypes.ToList();
            return PartialView("GridviewSeparateType", BSM.ListSeparateType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateSeparateType(HRTerminType MModel)
        {
            UserSession();
            DataSelector();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRTerminTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListSeparateType = DB.HRTerminTypes.ToList();
            return PartialView("GridviewSeparateType", BSM.ListSeparateType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSeparateType(HRTerminType MModel)
        {
            UserSession();
            DataSelector();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRTerminTypes.Find(MModel.Code);
                    DB.HRTerminTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListSeparateType = DB.HRTerminTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewSeparateType", BSM.ListSeparateType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSeparateType(string Code)
        {
            UserSession();
            DataSelector();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.resigntype == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRTerminTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRTerminTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListSeparateType = DB.HRTerminTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewSeparateType", BSM.ListSeparateType);
        }
        #endregion
        #region Announcement
        public ActionResult GridAnnouncement()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListAnnounementType = DB.HRAnnounceTypes.ToList();
            return PartialView("GridAnnouncement", BSM.ListAnnounementType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateAnnounce(HRAnnounceType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRAnnounceTypes.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListAnnounementType = DB.HRAnnounceTypes.ToList();
            return PartialView("GridAnnouncement", BSM.ListAnnounementType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAnnoune(HRAnnounceType MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRAnnounceTypes.Find(MModel.Code);
                    DB.HRAnnounceTypes.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListAnnounementType = DB.HRAnnounceTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridAnnouncement", BSM.ListAnnounementType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAnnounce(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.Country == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRAnnounceTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRAnnounceTypes.Remove(obj);
                            int row = DB.SaveChanges();

                        }
                        BSM.ListAnnounementType = DB.HRAnnounceTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridAnnouncement", BSM.ListAnnounementType);
        }
        #endregion
        #region Insurance Company
        public ActionResult GridviewInsuranceCompany()
        {
            UserConf(ActionBehavior.EDIT);

            MDEmpInfor BSM = new MDEmpInfor();
            BSM.ListInsuranceCompany = DB.HRInsuranceCompanies.ToList();
            return PartialView("GridviewInsuranceCompany", BSM.ListInsuranceCompany);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateInsuranceCompany(HRInsuranceCompany MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    MModel.CompanyName = MModel.CompanyName;
                    MModel.CreatedBy = user.UserName;
                    MModel.CreatedOn = DateTime.Now;
                    DB.HRInsuranceCompanies.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListInsuranceCompany = DB.HRInsuranceCompanies.ToList();
            return PartialView("GridviewInsuranceCompany", BSM.ListInsuranceCompany);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditInsuranceCompany(HRInsuranceCompany MModel)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRInsuranceCompanies.Find(MModel.Code);
                    DB.HRInsuranceCompanies.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListInsuranceCompany = DB.HRInsuranceCompanies.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewInsuranceCompany", BSM.ListInsuranceCompany);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteInsuranceCompany(string Code)
        {
            UserSession();
            MDEmpInfor BSM = new MDEmpInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRInsuranceCompanies.ToList();
                    if (objEmp.Where(w => w.Code == Code && w.IsActive == true).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRInsuranceCompanies.Find(Code);
                        if (obj != null)
                        {
                            DB.HRInsuranceCompanies.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListInsuranceCompany = DB.HRInsuranceCompanies.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewInsuranceCompany", BSM.ListInsuranceCompany);
        }
        #endregion

        private void DataSelector(params object[] keys)
        {
            MDEmpInfor BSM = new MDEmpInfor();
            foreach (var data in BSM.OnDataSelector())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}