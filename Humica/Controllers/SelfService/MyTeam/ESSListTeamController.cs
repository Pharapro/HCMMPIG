using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.SelfService.MyTeam
{

    public class ESSListTeamController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000021";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSListTeam/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";

        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();

        public ESSListTeamController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            var HOD_ = DB.HRStaffProfiles.Where(w => w.EmpCode == BSM.User.UserName).FirstOrDefault();
            if (HOD_ != null)
            {
                string Status = SYDocumentStatus.A.ToString();
                DateTime date = DateTime.Now.AddMonths(-1);
                var ListStaffView = await DBV.HR_STAFF_VIEW.Where(w => (w.StatusCode == "A" || (w.DateTerminate > date && w.StatusCode == "I"))
                && (w.HODCode == HOD_.EmpCode || w.FirstLine == HOD_.EmpCode || w.SecondLine == HOD_.EmpCode || w.FirstLine2 == HOD_.EmpCode || w.SecondLine2 == HOD_.EmpCode)).ToListAsync();
                BSM.ListStaffView = ListStaffView.ToList();
            }


            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListStaffView = new List<HR_STAFF_VIEW>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListStaffView);
        }
        #endregion
        #region "Details"
        public ActionResult Details(string Empcode)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            ViewData[ClsConstant.IS_READ_ONLY1] = true;
            ViewData[ClsConstant.IS_SALARY] = true;
            ViewData[SYConstant.PARAM_ID] = Empcode;
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ListEmpIdentity = new List<HREmpIdentity>();
            BSM.ListEmpDisciplinary = new List<HREmpDisciplinary>();
            if (Empcode == "null") Empcode = null;
            if (!string.IsNullOrEmpty(Empcode))
            {
                BSM.Header = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == Empcode);
                FitlertDataProvince(BSM.Header.Province, BSM.Header.District, BSM.Header.Commune);
                if (BSM.Header.SalaryFlag != true)
                    BSM.Header.ReSalary = DateTime.Now;
                BSM.ListEmpIdentity = DB.HREmpIdentities.Where(w => w.EmpCode == Empcode).ToList();
                BSM.ListEmpDisciplinary = DB.HREmpDisciplinaries.Where(w => w.EmpCode == Empcode).ToList();

                BSM.Salary = BSM.Header.Salary.ToString();

                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("STAFF_NE", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion
        #region "Ajax Image"
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
            Session["PATH_IMG"] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        public string DeleteFile(string FileName)
        {
            //string test = "9586-202-10072";
            // string result = FileName.Substring(FileName.LastIndexOf('/') + 1);

            try
            {
                string[] sp = FileName.Split('/');
                string file = sp[sp.Length - 1];
                string[] spf = file.Split('.');
                string sfile = spf[0];

                var obj = DB.MDUploadImages.FirstOrDefault(w => w.TokenCode == sfile);
                if (obj != null)
                {
                    DB.MDUploadImages.Remove(obj);
                    int row = DB.SaveChanges();
                    if (row > 0)
                    {
                        var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                        string root = HostingEnvironment.ApplicationPhysicalPath;
                        // obj.UpoadPath = obj.UpoadPath.Replace("~", "").Replace("/", "\\");
                        obj.UpoadPath = obj.UpoadPath.Replace("~/", "").Replace("/", "\\");
                        string fileNameDelete = root + obj.UpoadPath;
                        if (System.IO.File.Exists(fileNameDelete))
                        {
                            System.IO.File.Delete(fileNameDelete);
                        }
                    }
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

            if (Session[SYSConstant.IMG_SESSION_KEY_2] != null)
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

            Session[SYSConstant.IMG_SESSION_KEY_2] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_signature", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session["PATH_IMG"] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion
        public ActionResult GridItemDiscip()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            DataSelector();
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemDiscip";
            return PartialView("GridItemDiscip", BSM.ListEmpDisciplinary);
        }
        #region "Identify"
        public ActionResult _IdentityDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRStaffProfileObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("_IdentityDetail", BSM.ListEmpIdentity);
        }
        #endregion
        [HttpPost]
        public ActionResult ShowData(DateTime StartDate, string ProType)
        {
            var Pro = DB.HRProbationTypes.Find(ProType);
            var humanSetting = DB.HumanResourceSettings.FirstOrDefault();
            int Months = 0;
            int day = 0;
            if (Pro != null)
            {
                Months = Pro.InMonth;
                day = Pro.Day;
            }
            DateTime Probation = StartDate.AddMonths(Months).AddDays(day);
            DateTime LeaveCof = StartDate;
            DateTime openShirtDate = StartDate.AddDays(humanSetting != null ? (humanSetting.OpenShirtDate ?? 0) : 0);
            var result = new
            {
                MS = SYConstant.OK,
                Probation = Probation,
                LeaveCof = LeaveCof,
                openShirtDate = openShirtDate

            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        #region "Ajax"

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
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "District" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.District());
            });
        }

        public ActionResult Commune()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "Commune" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.Commune());
            });

        }
        public ActionResult Village()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "Village" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Desc1";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Desc1", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("Desc2", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.Village());
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
            HRStaffProfileObject BSM = new HRStaffProfileObject();
            var IsSalary = BSM.IsHideSalary(Level);
            var result = new
            {
                MS = SYConstant.OK,
                IsSalary = IsSalary,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public ActionResult JobType(string code, string addType)
        {
            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(code))
            {
                var _listCom = DB.HRCompanyGroups.Where(w => w.ParentWorkGroupID == addType).ToList();
                string Res = "";
                if (_listCom.Count() > 0)
                {
                    var obj = _listCom.FirstOrDefault();
                    Res = obj.WorkGroup;
                    if (obj.WorkGroup == "Division")
                        Session["Division"] = code;
                    else if (obj.WorkGroup == "GroupDepartment")
                        Session["GroupDepartment"] = code;
                    else if (obj.WorkGroup == "Department")
                        Session["Department"] = code;
                    else if (obj.WorkGroup == "Position")
                        Session["Position"] = code;
                    else if (obj.WorkGroup == "Section")
                        Session["Section"] = code;
                    else if (obj.WorkGroup == "Level")
                        Session["Level"] = code;
                    else if (obj.WorkGroup == "JobGrade")
                        Session["JobGrade"] = code;
                }
                var result = new
                {
                    MS = Res,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GetDivision()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetDivision" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.GetDivision());
            });
        }
        public ActionResult GetGroupDepartment()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetGroupDepartment" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.GetGroupDepartment());
            });
        }
        public ActionResult GetDepartment()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetDepartment" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(Humica.Logic.HR.HRStaffProfileObject.GetDepartment());
            });
        }
        public ActionResult GetPosition()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetPosition" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(HRStaffProfileObject.GetPosition());
            });
        }
        public ActionResult GetSection()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetSection" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(HRStaffProfileObject.GetSection());
            });
        }
        public ActionResult GetLevel()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetLevel" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(HRStaffProfileObject.GetLevel());
            });
        }
        public ActionResult GetJobGrade()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "StaffProfile", Action = "GetJobGrade" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{1}";
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 70);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.Columns.Add("SecDescription", SYSettings.getLabel("Second Description"), 250);
                cboProperties.BindList(HRStaffProfileObject.GetJobGrade());
            });
        }
        public void FitlertDataSession(string Branch, string Division, string GDept, string Dept,
          string Section, string Position, string Level, string Grade)
        {
            JobType(Branch, "Branch");
            JobType(Division, "Division");
            JobType(GDept, "GroupDepartment");
            JobType(Dept, "Department");
            JobType(Section, "Section");
            JobType(Position, "Position");
            JobType(Level, "Level");
            JobType(Grade, "JobGrade");
        }
        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("SEX");
            ViewData["GENDER_SELECT"] = objList.ListData;
            objList = new SYDataList("INITIAL");
            ViewData["INITIAL_SELECT"] = objList.ListData;
            objList = new SYDataList("MARITAL");
            ViewData["MARITAL_SELECT"] = objList.ListData;

            ViewData["COUNTRY_SELECT"] = DB.HRCountries.ToList().OrderBy(w => w.Description);
            ViewData["NATION_SELECT"] = DB.HRNations.ToList().OrderBy(w => w.Description);
            ViewData["PROVICES_SELECT"] = DB.HRProvices.ToList().OrderBy(w => w.Description);
            var _listBranch = SYConstant.getBranchDataAccess();
            ViewData["BRANCHES_SELECT"] = _listBranch.ToList();
            ViewData["BANK_SELECT"] = DB.HRBanks.Where(w => w.IsActive == true).ToList().OrderBy(w => w.Description);
            ViewData["BANKBRANCH_SELECT"] = DB.HRBankBranches.ToList().OrderBy(w => w.Description);
            ViewData["EMPTYPE_SELECT"] = DB.HREmpTypes.ToList().OrderBy(w => w.Description);
            var ObjTax = new SYDataList("TAXPAID");
            ViewData["TAXPAID_SELECT"] = ObjTax.ListData;
            ViewData["FUNCTION_SELECT"] = DB.HRFunctions.ToList().OrderBy(w => w.Description);
            ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList().OrderBy(w => w.Description);
            ViewData["LINE_SELECT"] = DB.HRLines.ToList().OrderBy(w => w.Description);
            ViewData["PERAMETER_SELECT"] = DB.PRParameters.ToList().OrderBy(w => w.Description);
            ViewData["POSITIONFAMILY_SELECT"] = DB.HRPositionFamilies.Where(w => w.IsActive == true).ToList().OrderBy(w => w.Description);
            var IdentityTye = new SYDataList("IdentityTye");
            ViewData["IdentityTye_SELECT"] = IdentityTye.ListData;
            ViewData["ROSTER_SELECT"] = DB.ATBatches.ToList();
            ViewData["STAFFTYPE_SELECT"] = DB.HRWorkingTypes.ToList();
            ViewData["BLOODTYPE_SELECT"] = DB.HRBloodTypes.ToList();
            ViewData["TELEGRAM_SELECT"] = DB.TelegramBots.ToList();
            ViewData["CATEGORY_SELECT"] = DB.HRCategories.ToList();
            ViewData["PROBATIONTYPE_SELECT"] = DB.HRProbationTypes.ToList();
            ViewData["CareerHistories_SELECT"] = DB.HRCareerHistories.ToList();
            var Province = DB.OBDescs.SqlQuery("SELECT distinct Province as Code,ProvinceDesc1 as Desc1 ,ProvinceDesc2 as Desc2 FROM CFPostalCode");
            ViewData["PROVINCE"] = Province.ToList();
            ViewData["DISCIPLINAY_LIST"] = DB.HRDisciplinActions.ToList();
            ViewData["COSTCENT_LIST"] = DB.PRCostCenterGroups.ToList();
            var SalaryType = new SYDataList("SALARYTYPE_SELECT");
            ViewData["SALARYTYPE_SELECT"] = SalaryType.ListData;
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            var NSSF = new SYDataList("NSSF_CONTRIBUTION");
            ViewData["NSSF_CONTRIBUTION"] = NSSF.ListData;
            ViewData["SetEntitle_SELECT"] = DB.HRSetEntitleHs.ToList();
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
