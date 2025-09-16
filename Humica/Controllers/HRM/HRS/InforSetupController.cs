using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HRS;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class InforSetupController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000001";
        private const string URL_SCREEN = "/HRM/HRS/InforSetup";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILEBU = "12313123123sadfsdfsdfsdf";
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public InforSetupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: Branch
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListCompanyGroup = SMS.SYHRCompanies.ToList();
            BSM.ListCompany = SMS.HRCompanies.ToList();
            BSM.ListBranch = SMS.HRBranches.ToList();
            BSM.ListDivision = DB.HRDivisions.ToList();
            BSM.ListGroupDepartment = DB.HRGroupDepartments.ToList();
            BSM.ListDepartment = DB.HRDepartments.ToList();
            BSM.ListLevel = SMS.HRLevels.ToList();
            BSM.ListLine = DB.HRLines.ToList();
            BSM.ListCategory = DB.HRCategories.ToList();
            BSM.ListLocation = DB.HRLocations.ToList();
            BSM.ListSection = DB.HRSections.ToList();
            BSM.ListEmpType = DB.HREmpTypes.ToList();
            BSM.ListFunction = DB.HRFunctions.ToList();
            BSM.ListPosition = DB.HRPositions.ToList();
            //BSM.ListOrgChart = DB.HROrgCharts.ToList();
            BSM.ListJobGrade = DB.HRJobGrades.ToList();
            BSM.ListProType = DB.HRProbationTypes.ToList();
            BSM.ListUniform = DB.HRUniforms.ToList();
            BSM.ListOffice = DB.HROffices.ToList();
            BSM.ListGroup = DB.HRGroups.ToList();
            return View(BSM);
        }
        #region Company Group

        public ActionResult GridItemCompanyGroup()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListCompanyGroup = SMS.SYHRCompanies.ToList();
            return PartialView("GridviewCompanyGroup", BSM.ListCompanyGroup);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCompanyGroup(SYHRCompany MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {

                    MModel.CompanyCode = MModel.CompanyCode.ToUpper();
                    SMS.SYHRCompanies.Add(MModel);
                    int row = SMS.SaveChanges();
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
            BSM.ListCompanyGroup = SMS.SYHRCompanies.ToList();
            return PartialView("GridviewCompanyGroup", BSM.ListCompanyGroup);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCompanyGroup(SYHRCompany MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new SMSystemEntity();
                    var objUpdate = DBU.SYHRCompanies.Find(MModel.CompanyCode);
                    SMS.SYHRCompanies.Attach(MModel);
                    SMS.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = SMS.SaveChanges();
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
            BSM.ListCompanyGroup = SMS.SYHRCompanies.OrderBy(w => w.CompanyCode).ToList();
            return PartialView("GridviewCompanyGroup", BSM.ListCompanyGroup);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCompanyGroup(string CompanyCode)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (CompanyCode != null)
            {
                try
                {
                    //var objEmp = DP.HRBranches.ToList();
                    //if (objEmp.Where(w => w.Branch == CompanyCode).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                    var obj = SMS.SYHRCompanies.Find(CompanyCode);
                    if (obj != null)
                    {
                        SMS.SYHRCompanies.Remove(obj);
                        int row = SMS.SaveChanges();
                    }
                    BSM.ListCompanyGroup = SMS.SYHRCompanies.OrderBy(w => w.CompanyCode).ToList();
                    //  }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewCompanyGroup", BSM.ListCompanyGroup);
        }


        #endregion

        #region Company

        public ActionResult GridItemCompany()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListCompany = SMS.HRCompanies.ToList();
            return PartialView("GridviewCompany", BSM.ListCompany);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCompany(HRCompany MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Company.ToUpper().Trim();
                    var listCompany = SMS.HRCompanies.FirstOrDefault(w => w.Company == Code);
                    if (listCompany == null)
                    {
                        if (Session[PATH_FILE] != null)
                        {
                            MModel.Images = Session[PATH_FILE].ToString();
                            Session[PATH_FILE] = null;
                        }
                        MModel.Company = Code;
                        SMS.HRCompanies.Add(MModel);
                        int row = SMS.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListCompany = SMS.HRCompanies.ToList();
            return PartialView("GridviewCompany", BSM.ListCompany);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCompany(HRCompany MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    SMS = new SMSystemEntity();
                    var objUpdate = SMS.HRCompanies.FirstOrDefault(w => w.Company == MModel.Company);
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.Images = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        MModel.Images = objUpdate.Images;
                    }
                    objUpdate.Images = MModel.Images;
                    objUpdate.Description = MModel.Description;
                    objUpdate.SecDescription = MModel.SecDescription;
                    objUpdate.Address = MModel.Address;
                    objUpdate.Remark = MModel.Remark;
                    objUpdate.Email = MModel.Email;
                    objUpdate.NSSFNo = MModel.NSSFNo;
                    objUpdate.Phone = MModel.Phone;
                    objUpdate.VatinNumber = MModel.VatinNumber;
                    SMS.HRCompanies.Attach(objUpdate);
                    SMS.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                    int row = SMS.SaveChanges();
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
            BSM.ListCompany = SMS.HRCompanies.OrderBy(w => w.Company).ToList();
            return PartialView("GridviewCompany", BSM.ListCompany);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCompany(string Company)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (Company != null)
            {
                try
                {
                    //var objEmp = DB.HREmpCareers.ToList();
                    //if (objEmp.Where(w => w.Branch == Code).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                    var obj = SMS.HRCompanies.FirstOrDefault(w => w.Company == Company);
                    if (obj != null)
                    {
                        SMS.HRCompanies.Remove(obj);
                        int row = SMS.SaveChanges();
                    }
                    BSM.ListCompany = SMS.HRCompanies.OrderBy(w => w.Company).ToList();
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewCompany", BSM.ListCompany);
        }
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);

            //Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            objFile.ObjectTemplate.UpoadPath = objFile.ObjectTemplate.UpoadPath.Replace("~", "").Replace("/", "/");
            string fileNameUpload = SYUrl.getBaseUrl() + objFile.ObjectTemplate.UpoadPath;
            Session[PATH_FILE] = fileNameUpload;
            return null;
        }

        #endregion

        #region Branchs

        public ActionResult GridItemBranchs()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListBranch = SMS.HRBranches.ToList();
            return PartialView("GridviewBranch", BSM.ListBranch);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBranch(HRBranch MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listBranch = SMS.HRBranches.FirstOrDefault(w => w.Code == Code && w.CompanyCode == MModel.CompanyCode);
                    if (listBranch == null)
                    {
                        MModel.Code = Code;
                        SMS.HRBranches.Add(MModel);
                        int row = SMS.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListBranch = SMS.HRBranches.ToList();
            return PartialView("GridviewBranch", BSM.ListBranch);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBranch(HRBranch MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new SMSystemEntity();
                    var objUpdate = DBU.HRBranches.FirstOrDefault(w => w.Code == MModel.Code && w.CompanyCode == MModel.CompanyCode);
                    SMS.HRBranches.Attach(MModel);
                    SMS.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = SMS.SaveChanges();
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
            BSM.ListBranch = SMS.HRBranches.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewBranch", BSM.ListBranch);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBranch(string CompanyCode, string Code)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.Where(w => w.CompanyCode == CompanyCode && w.Branch == Code).ToList();
                    if (objEmp.Count() > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE");
                    }
                    else
                    {
                        var obj = SMS.HRBranches.FirstOrDefault(w => w.CompanyCode == CompanyCode && w.Code == Code);
                        if (obj != null)
                        {
                            SMS.HRBranches.Remove(obj);
                            int row = SMS.SaveChanges();
                        }
                        BSM.ListBranch = SMS.HRBranches.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewBranch", BSM.ListBranch);
        }

        #endregion

        #region Division

        public ActionResult GridItemDivisions()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListDivision = DB.HRDivisions.ToList();
            return PartialView("GridviewDivision", BSM.ListDivision);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateDivision(HRDivision MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDivi = DB.HRDivisions.Find(Code);
                    if (listDivi == null)
                    {
                        MModel.Code = Code;
                        DB.HRDivisions.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListDivision = DB.HRDivisions.ToList();
            return PartialView("GridviewDivision", BSM.ListDivision);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditDivision(HRDivision MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRDivisions.Find(MModel.Code);
                    DB.HRDivisions.Attach(MModel);
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
            BSM.ListDivision = DB.HRDivisions.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewDivision", BSM.ListDivision);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDivision(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.Division == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRDivisions.Find(Code);
                        if (obj != null)
                        {
                            DB.HRDivisions.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListDivision = DB.HRDivisions.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewDivision", BSM.ListDivision);
        }

        #endregion

        #region Group Department

        public ActionResult GridItemGroupDepartments()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListGroupDepartment = DB.HRGroupDepartments.ToList();
            return PartialView("GridviewGroupDepartment", BSM.ListGroupDepartment);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateGroupDepartment(HRGroupDepartment MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDept = DB.HRGroupDepartments.Find(Code);
                    if (listDept == null)
                    {
                        if (Session[PATH_FILEBU] != null)
                        {
                            MModel.Image = Session[PATH_FILEBU].ToString();
                            Session[PATH_FILEBU] = null;
                        }
                        MModel.Code = Code;
                        DB.HRGroupDepartments.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListGroupDepartment = DB.HRGroupDepartments.ToList();
            return PartialView("GridviewGroupDepartment", BSM.ListGroupDepartment);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditGroupDepartment(HRGroupDepartment MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    if (Session[PATH_FILEBU] != null)
                    {
                        MModel.Image = Session[PATH_FILEBU].ToString();
                        Session[PATH_FILEBU] = null;
                    }
                    var objUpdate = DBU.HRGroupDepartments.Find(MModel.Code);
                    DB.HRGroupDepartments.Attach(MModel);
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
            BSM.ListGroupDepartment = DB.HRGroupDepartments.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewGroupDepartment", BSM.ListGroupDepartment);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteGroupDepartment(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.GroupDept == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRGroupDepartments.Find(Code);
                        if (obj != null)
                        {
                            DB.HRGroupDepartments.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListGroupDepartment = DB.HRGroupDepartments.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewGroupDepartment", BSM.ListGroupDepartment);
        }
        public ActionResult UploadControlCallbackActionImageBU()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_imageBU", objFile.ValidationSettings, objFile.uc_FileUploadComplete);

            objFile.ObjectTemplate.UpoadPath = objFile.ObjectTemplate.UpoadPath.Replace("~", "").Replace("/", "/");
            string fileNameUpload = SYUrl.getBaseUrl() + objFile.ObjectTemplate.UpoadPath;
            Session[PATH_FILEBU] = fileNameUpload;
            return null;
        }

        #endregion

        #region Department

        public ActionResult GridItemDepartments()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListDepartment = DB.HRDepartments.ToList();
            return PartialView("GridviewDepartment", BSM.ListDepartment);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateDepartment(HRDepartment MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDept = DB.HRDepartments.Find(Code);
                    if (listDept == null)
                    {
                        MModel.Code = Code;
                        DB.HRDepartments.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListDepartment = DB.HRDepartments.ToList();
            return PartialView("GridviewDepartment", BSM.ListDepartment);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditDepartment(HRDepartment MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRDepartments.Find(MModel.Code);
                    DB.HRDepartments.Attach(MModel);
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
            BSM.ListDepartment = DB.HRDepartments.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewDepartment", BSM.ListDepartment);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteDepartment(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.DEPT == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRDepartments.Find(Code);
                        if (obj != null)
                        {
                            DB.HRDepartments.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListDepartment = DB.HRDepartments.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewDepartment", BSM.ListDepartment);
        }

        #endregion

        #region Office

        public ActionResult GridviewOffice()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListOffice = DB.HROffices.ToList();
            return PartialView("GridviewOffice", BSM.ListOffice);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOffice(HROffice MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDept = DB.HROffices.Find(Code);
                    if (listDept == null)
                    {
                        MModel.Code = Code;
                        DB.HROffices.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListOffice = DB.HROffices.ToList();
            return PartialView("GridviewOffice", BSM.ListOffice);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOffice(HROffice MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HROffices.Find(MModel.Code);
                    DB.HROffices.Attach(MModel);
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
            BSM.ListOffice = DB.HROffices.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewOffice", BSM.ListOffice);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOffice(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    //var objEmp = DB.HREmpCareers.ToList();
                    //if (objEmp.Where(w => w.DEPT == Code).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                        var obj = DB.HROffices.Find(Code);
                        if (obj != null)
                        {
                            DB.HROffices.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListOffice = DB.HROffices.OrderBy(w => w.Code).ToList();
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewOffice", BSM.ListOffice);
        }

        #endregion

        #region Location

        public ActionResult GridItemLocations()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListLocation = DB.HRLocations.ToList();
            return PartialView("GridviewLocation", BSM.ListLocation);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLocation(HRLocation MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listloca = DB.HRLocations.Find(Code);
                    if (listloca == null)
                    {
                        MModel.Code = Code;
                        DB.HRLocations.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListLocation = DB.HRLocations.ToList();
            return PartialView("GridviewLocation", BSM.ListLocation);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditLocation(HRLocation MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRLocations.Find(MModel.Code);
                    DB.HRLocations.Attach(MModel);
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
            BSM.ListLocation = DB.HRLocations.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewLocation", BSM.ListLocation);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLocation(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.LOCT == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRLocations.Find(Code);
                        if (obj != null)
                        {
                            DB.HRLocations.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListLocation = DB.HRLocations.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewLocation", BSM.ListLocation);
        }

        #endregion

        #region Line

        public ActionResult GridItemLines()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListLine = DB.HRLines.ToList();
            return PartialView("GridviewLine", BSM.ListLine);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLine(HRLine MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRLines.Add(MModel);
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
            BSM.ListLine = DB.HRLines.ToList();
            return PartialView("GridviewLine", BSM.ListLine);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditLine(HRLine MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRLines.Find(MModel.Code);
                    DB.HRLines.Attach(MModel);
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
            BSM.ListLine = DB.HRLines.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewLine", BSM.ListLine);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLine(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.LINE == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRLines.Find(Code);
                        if (obj != null)
                        {
                            DB.HRLines.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListLine = DB.HRLines.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewLine", BSM.ListLine);
        }

        #endregion

        #region Category
        public ActionResult GridItemCategory()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListCategory = DB.HRCategories.ToList();
            return PartialView("GridviewCategory", BSM.ListCategory);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCategory(HRCategory MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listCat = DB.HRCategories.Find(Code);
                    if (listCat == null)
                    {
                        MModel.Code = Code;
                        DB.HRCategories.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListCategory = DB.HRCategories.ToList();
            return PartialView("GridviewCategory", BSM.ListCategory);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCategory(HRCategory MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    DB.HRCategories.Attach(MModel);
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
            BSM.ListCategory = DB.HRCategories.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewCategory", BSM.ListCategory);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCategory(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.CATE == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRCategories.Find(Code);
                        if (obj != null)
                        {
                            DB.HRCategories.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListCategory = DB.HRCategories.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewCategory", BSM.ListCategory);
        }

        #endregion

        #region JobGrade
        public ActionResult GridItemJobGrade()
        {
            UserConf(ActionBehavior.EDIT);
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListJobGrade = DB.HRJobGrades.ToList();
            return PartialView("GridviewJobGrade", BSM.ListJobGrade);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateJobGrade(HRJobGrade MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper().Trim();
                    MModel.LCK = 0;
                    DB.HRJobGrades.Add(MModel);
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
            BSM.ListJobGrade = DB.HRJobGrades.ToList();
            return PartialView("GridviewJobGrade", BSM.ListJobGrade);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditJobGrade(HRJobGrade MModel)
        {
            UserSession();

            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRJobGrades.Attach(MModel);
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
            BSM.ListJobGrade = DB.HRJobGrades.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewJobGrade", BSM.ListJobGrade);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteJobGrade(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    var obj = DB.HRJobGrades.Where(w => w.Code == Code).FirstOrDefault();
                    if (obj != null)
                    {
                        DB.HRJobGrades.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListJobGrade = DB.HRJobGrades.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewJobGrade", BSM.ListJobGrade);
        }

        #endregion

        #region Level

        public ActionResult GridItemLevels()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListLevel = SMS.HRLevels.ToList();
            return PartialView("GridviewLevel", BSM.ListLevel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLevel(HRLevel MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listLevel = SMS.HRLevels.Find(Code);
                    if (listLevel == null)
                    {
                        MModel.Code = Code;
                        SMS.HRLevels.Add(MModel);
                        int row = SMS.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListLevel = SMS.HRLevels.ToList();
            return PartialView("GridviewLevel", BSM.ListLevel);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditLevel(HRLevel MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            var DBU = new SMSystemEntity();
            if (ModelState.IsValid)
            {
                try
                {

                    // var objUpdate = DBU.HRLevels.Find(MModel.Code);
                    DBU.HRLevels.Attach(MModel);
                    DBU.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBU.SaveChanges();
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
            BSM.ListLevel = DBU.HRLevels.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewLevel", BSM.ListLevel);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLevel(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.LevelCode == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = SMS.HRLevels.Find(Code);
                        if (obj != null)
                        {
                            SMS.HRLevels.Remove(obj);
                            int row = SMS.SaveChanges();
                        }
                        BSM.ListLevel = SMS.HRLevels.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewLevel", BSM.ListLevel);
        }

        #endregion

        #region Function

        public ActionResult GridItemFunction()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListFunction = DB.HRFunctions.ToList();
            return PartialView("GridviewFunction", BSM.ListFunction);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateFunction(HRFunction MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listPost = DB.HRFunctions.Find(Code);
                    if (listPost == null)
                    {
                        MModel.Code = Code;
                        DB.HRFunctions.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListFunction = DB.HRFunctions.ToList();
            return PartialView("GridviewFunction", BSM.ListFunction);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditFunction(HRFunction MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    DB.HRFunctions.Attach(MModel);
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
            BSM.ListFunction = DB.HRFunctions.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewFunction", BSM.ListFunction);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteFunction(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.Functions == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRFunctions.Find(Code);
                        if (obj != null)
                        {
                            DB.HRFunctions.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListFunction = DB.HRFunctions.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewFunction", BSM.ListFunction);
        }

        #endregion
        #region Position

        public ActionResult GridItemPositions()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListPosition = DB.HRPositions.ToList();
            return PartialView("GridviewPosition", BSM.ListPosition);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePosition(HRPosition MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listPost = DB.HRPositions.Find(Code);
                    if (listPost == null)
                    {
                        MModel.Code = Code;
                        DB.HRPositions.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListPosition = DB.HRPositions.ToList();
            return PartialView("GridviewPosition", BSM.ListPosition);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPosition(HRPosition MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    DB.HRPositions.Attach(MModel);
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
            BSM.ListPosition = DB.HRPositions.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewPosition", BSM.ListPosition);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePosition(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.JobCode == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRPositions.Find(Code);
                        if (obj != null)
                        {
                            DB.HRPositions.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListPosition = DB.HRPositions.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewPosition", BSM.ListPosition);
        }

        #endregion

        #region Position Family

        //public ActionResult GridItemPositionFamilys()
        //{
        //    UserConf(ActionBehavior.EDIT);
        //    DataSelector();
        //    MDInforSetup BSM = new MDInforSetup();
        //    BSM.ListOrgChart = DB.HROrgCharts.ToList();
        //    return PartialView("GridviewPositionFamily", BSM.ListOrgChart);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreatePositionFamily(HROrgChart MModel)
        //{
        //    UserSession();
        //    DataSelector();
        //    MDInforSetup BSM = new MDInforSetup();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            string Code = MModel.Code.ToUpper().Trim();
        //            var listPost = DB.HROrgCharts.Find(MModel.Branch, Code);
        //            if (listPost == null)
        //            {
        //                MModel.Code = Code;
        //                DB.HROrgCharts.Add(MModel);
        //                int row = DB.SaveChanges();
        //            }
        //            else
        //            {
        //                ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
        //    }
        //    BSM.ListOrgChart = DB.HROrgCharts.ToList();
        //    return PartialView("GridviewPositionFamily", BSM.ListOrgChart);
        //}
        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditPositionFamily(HROrgChart MModel)
        //{
        //    UserSession();
        //    DataSelector();
        //    MDInforSetup BSM = new MDInforSetup();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var DBU = new HumicaDBContext();
        //            var objUpdate = DBU.HROrgCharts.Find(MModel.Branch, MModel.Code);
        //            DB.HROrgCharts.Attach(MModel);
        //            DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
        //            int row = DB.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
        //    }
        //    BSM.ListOrgChart = DB.HROrgCharts.OrderBy(w => w.Code).ToList();
        //    return PartialView("GridviewPositionFamily", BSM.ListOrgChart);
        //}
        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeletePositionFamily(string Branch, string Code)
        //{
        //    UserSession();
        //    DataSelector();
        //    MDInforSetup BSM = new MDInforSetup();
        //    if (Code != null)
        //    {
        //        try
        //        {
        //            //var objEmp = DP.HRStaffProfiles.ToList();
        //            //if (objEmp.Where(w => w.PostFamily == Code).Any())
        //            //{
        //            //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
        //            //}
        //            //else
        //            //{
        //            var obj = DB.HROrgCharts.Where(w => w.Branch == Branch && w.Code == Code).FirstOrDefault();
        //            if (obj != null)
        //            {
        //                DB.HROrgCharts.Remove(obj);
        //                int row = DB.SaveChanges();
        //            }
        //            BSM.ListOrgChart = DB.HROrgCharts.OrderBy(w => w.Code).ToList();
        //            //}
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }

        //    return PartialView("GridviewPositionFamily", BSM.ListOrgChart);
        //}

        #endregion

        #region Section
        public ActionResult GridItemSections()
        {
            UserSession();
            DataSelector();
            UserConfListAndForm();
            //DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListSection = DB.HRSections.ToList();
            return PartialView("GridItemSections", BSM.ListSection);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateSection(HRSection ModelObject)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm();
            //DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = ModelObject.Code.ToUpper().Trim();
                    var listSec = DB.HRSections.Find(Code);
                    if (listSec == null)
                    {
                        ModelObject.Code = Code;
                        DB.HRSections.Add(ModelObject);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListSection = DB.HRSections.ToList();
            return PartialView("GridItemSections", BSM.ListSection);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSection(HRSection ModelObject)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm();
            //DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRSections.Attach(ModelObject);
                    DB.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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

            BSM.ListSection = DB.HRSections.ToList();
            return PartialView("GridItemSections", BSM.ListSection);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSection(string Code)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.SECT == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRSections.FirstOrDefault(w => w.Code == Code);
                        if (obj != null)
                        {
                            DB.HRSections.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListSection = DB.HRSections.ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemSections", BSM.ListSection);
        }
        #endregion

        #region EmpType
        public ActionResult GridItemEmpTypes()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListEmpType = DB.HREmpTypes.ToList();
            return PartialView("GridItemEmpTypes", BSM.ListEmpType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEmpType(HREmpType MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listPType = DB.HREmpTypes.Find(Code);
                    if (listPType == null)
                    {
                        MModel.Code = Code;
                        DB.HREmpTypes.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListEmpType = DB.HREmpTypes.ToList();
            return PartialView("GridItemEmpTypes", BSM.ListEmpType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEmpType(HREmpType MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HREmpTypes.Find(MModel.Code);
                    DB.HREmpTypes.Attach(MModel);
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
            BSM.ListEmpType = DB.HREmpTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemEmpTypes", BSM.ListEmpType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEmpType(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HREmpCareers.ToList();
                    if (objEmp.Where(w => w.EmpType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HREmpTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HREmpTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListEmpType = DB.HREmpTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemEmpTypes", BSM.ListEmpType);
        }
        #endregion

        #region Group

        public ActionResult GridviewGroup()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            BSM.ListGroup = DB.HRGroups.ToList();
            return PartialView("GridviewGroup", BSM.ListGroup);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateGroup(HRGroup MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDept = DB.HRGroups.Find(Code);
                    if (listDept == null)
                    {
                        MModel.Code = Code;
                        DB.HRGroups.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListGroup = DB.HRGroups.ToList();
            return PartialView("GridviewGroup", BSM.ListGroup);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditGroup(HRGroup MModel)
        {
            UserSession();
            DataSelector();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRGroups.Find(MModel.Code);
                    DB.HRGroups.Attach(MModel);
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
            BSM.ListGroup = DB.HRGroups.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewGroup", BSM.ListGroup);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteGroup(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    //var objEmp = DB.HREmpCareers.ToList();
                    //if (objEmp.Where(w => w.DEPT == Code).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                    var obj = DB.HRGroups.Find(Code);
                    if (obj != null)
                    {
                        DB.HRGroups.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListGroup = DB.HRGroups.OrderBy(w => w.Code).ToList();
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewGroup", BSM.ListGroup);
        }

        #endregion
        #region Probation Typ
        public ActionResult GridviewProType()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListProType = DB.HRProbationTypes.ToList();
            return PartialView("GridviewProType", BSM.ListProType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateProType(HRProbationType MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listPType = DB.HRProbationTypes.Find(Code);
                    if (listPType == null)
                    {
                        MModel.Code = Code;
                        DB.HRProbationTypes.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListProType = DB.HRProbationTypes.ToList();
            return PartialView("GridviewProType", BSM.ListProType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditProType(HRProbationType MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRProbationTypes.Find(MModel.Code);
                    DB.HRProbationTypes.Attach(MModel);
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
            BSM.ListProType = DB.HRProbationTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewProType", BSM.ListProType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteProType(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.ProbationType == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRProbationTypes.Find(Code);
                        if (obj != null)
                        {
                            DB.HRProbationTypes.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListProType = DB.HRProbationTypes.OrderBy(w => w.Code).ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewProType", BSM.ListProType);
        }
        #endregion

        #region Uniform
        public ActionResult GridviewUniform()
        {
            UserConf(ActionBehavior.EDIT);

            MDInforSetup BSM = new MDInforSetup();
            BSM.ListUniform = DB.HRUniforms.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewUniform", BSM.ListUniform);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateUniform(HRUniform MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listniform = DB.HRUniforms.Find(Code);
                    if (listniform == null)
                    {
                        MModel.Code = Code;
                        DB.HRUniforms.Add(MModel);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    }
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
            BSM.ListUniform = DB.HRUniforms.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewUniform", BSM.ListUniform);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditUniform(HRUniform MModel)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRUniforms.Find(MModel.Code);
                    DB.HRUniforms.Attach(MModel);
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
            BSM.ListUniform = DB.HRUniforms.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewUniform", BSM.ListUniform);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteUniform(string Code)
        {
            UserSession();
            MDInforSetup BSM = new MDInforSetup();
            if (Code != null)
            {
                try
                {
                    //var objEmp = DP.HRStaffProfiles.ToList();
                    //if (objEmp.Where(w => w.ProbationType == Code).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                    var obj = DB.HRUniforms.Find(Code);
                    if (obj != null)
                    {
                        DB.HRUniforms.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListUniform = DB.HRUniforms.OrderBy(w => w.Code).ToList();
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewUniform", BSM.ListUniform);
        }
        #endregion


        private void DataSelector()
        {
            var _listBranch = SYConstant.getBranchDataAccess();
            var Level = SMS.HRLevels.ToList();
            Level = Level.ToList();
            ViewData["GroupCompany_SELECT"] = SMS.SYHRCompanies.ToList();
            ViewData["Company_SELECT"] = SMS.HRCompanies.ToList();
            ViewData["BRANCHES_SELECT"] = _listBranch;// DB.HRBranches.ToList().OrderBy(w => w.Description);
            //ViewData["LEVEL_SELECT"] = Level;
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
        }
    }
}
