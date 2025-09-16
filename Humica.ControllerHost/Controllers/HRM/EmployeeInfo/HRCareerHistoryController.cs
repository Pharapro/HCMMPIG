using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Employee;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Humica.Employee.ClsEmployee;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRCareerHistoryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000002";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRCareerHistory/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILE2 = "12313123123sadfsdfsdfs12fxdf";
        private string KeyName = "TranNo";
        IClsEmployee BSM;
        IUnitOfWork unitOfWork;
        public HRCareerHistoryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEmployee();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
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
            BSM.OnLoandindEmpCarrer();
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
            BSM.OnLoandindEmpCarrer();
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
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpCareer.OrderBy(x => x.FromDate).ToList());
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            BSM.OnCreatingEmpCarrer();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsEmployee collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.HeaderCareer.AttachFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                }
                BSM.NewSalary = collection.NewSalary;
                BSM.Increase = collection.Increase;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.HeaderCareer = collection.HeaderCareer;

                string msg = BSM.CreateCareerStaff();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderCareer.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
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

        #region "Edit"
        public ActionResult Edit(int ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            if (ID > 0)
            {
                BSM.OnDetailCarrerLoading(ID, "EDIT"); 
                if (BSM.HeaderCareer != null)
                {
                    var IsSalary = BSM.IsHideSalary(BSM.HeaderCareer.LevelCode);
                    ViewData[ClsConstant.IS_SALARY] = IsSalary;
                    if (IsSalary == true)
                    {
                        BSM.ListCareerBankList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == BSM.HeaderCareer.EmpCode && w.Reference == BSM.HeaderCareer.TranNo).ToList();
                    }
                    else
                    {
                        BSM.ListCareerBankList = new List<HREmpCareerBankList>();
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("CAREER_NE", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(string ID, ClsEmployee collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            Int64 TranNo = 0;
            if (ID != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    if (Session[PATH_FILE2] != null)
                    {
                        collection.HeaderCareer.AttachFile = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        collection.HeaderCareer.AttachFile = BSM.HeaderCareer.AttachFile;
                    }

                    TranNo = BSM.HeaderCareer.TranNo;
                    if (collection.HeaderCareer.CareerCode == null)
                        collection.HeaderCareer.CareerCode = BSM.HeaderCareer.CareerCode;
                    BSM.HeaderCareer = collection.HeaderCareer;
                    BSM.HeaderCareer.TranNo = TranNo;
                    BSM.HeaderCareer.EmpCode = collection.HeaderStaff.EmpCode;
                    BSM.NewSalary = collection.NewSalary;
                    BSM.Increase = collection.Increase;
                }

                string msg = "";
                msg = BSM.EditCareerStaff(ID);

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = ID;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
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

        #region "Detail"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector(true);
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (id.ToString() != "null")
            {
                int TranNo = Convert.ToInt32(id);
                BSM.OnDetailCarrerLoading(TranNo);
                if (BSM.HeaderCareer != null)
                {
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("CAREER_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            if (id != null)
            {
                string msg = BSM.Delete_EmpCareerHistory(id);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("CAREER_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion

        #region Download Document
        public ActionResult DownloadENG(string id, string SignEmpCode)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                }
                string fileName = "";

                int TranNo = Convert.ToInt32(id);
                var empCareer = unitOfWork.Repository<HREmpCareer>().FirstOrDefault(w => w.TranNo == TranNo);
                if (empCareer != null)
                {
                    var CareerC = unitOfWork.Repository< HRCareerHistory>().FirstOrDefault(w => w.Code == empCareer.CareerCode);
                    if (CareerC != null)
                    {
                        if (CareerC.TemplatePath != "" && CareerC.TemplatePath != null)
                        {
                            fileName = CareerC.TemplatePath;
                        }
                        else
                        {
                            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TEMPLATE", user.Lang);

                            //ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TEMPLATE", user.Lang);
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empCareer.EmpCode);
                    var POS = unitOfWork.Repository<HRPosition>().FirstOrDefault(w => w.Code == empCareer.JobCode);
                    var NATION = unitOfWork.Repository<HRNation>().FirstOrDefault(w => w.Code == STAFFP.Nation);


                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + empCareer.EmpCode + CareerC.Description + ".docx");

                    Para.ListObjectDictionary = new List<object>();
                    if (POS != null)
                    {
                        Para.ListObjectDictionary.Add(POS);
                    }
                    Para.ListObjectDictionary.Add(empCareer);
                    Para.ListObjectDictionary.Add(STAFFP);

                    var SIGNEmp = unitOfWork.Repository <HRStaffProfile>().FirstOrDefault(w => w.EmpCode == SignEmpCode);
                    Para.HDHeader = new ClsHeadDepartment();
                    if (SIGNEmp != null)
                    {
                        Para.HDHeader.HDName = SIGNEmp.AllName;
                        Para.HDHeader.HDNameKH = SIGNEmp.OthAllName;
                        var HDPOS = unitOfWork.Repository <HRPosition>().FirstOrDefault(w => w.Code == SIGNEmp.JobCode);
                        if (HDPOS != null)
                        {
                            Para.HDHeader.HDPosition = HDPOS.Description;
                            Para.HDHeader.HDPositionKH = HDPOS.SecDescription;
                        }
                    }

                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpCareer");
                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + empCareer.EmpCode + CareerC.Description + ".docx");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.WriteFile(FileSource);
                        Response.End();

                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                    //return null;
                    //return RedirectToAction("Index");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        public ActionResult DownloadKH(string id, string SignEmpCode)
        {
            ActionName = "Index";
            UserSession();
            //UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                    //BSM.Filter.InMonth = Temp;
                }
                string fileName = "";// Server.MapPath("~/Content/UPLOAD/Contract.docx");

                int TranNo = Convert.ToInt32(id);
                var empCareer = unitOfWork.Repository <HREmpCareer>().FirstOrDefault(w => w.TranNo == TranNo);
                if (empCareer != null)
                {
                    var CareerC = unitOfWork.Repository < HRCareerHistory>().FirstOrDefault(w => w.Code == empCareer.CareerCode);
                    if (CareerC != null)
                    {
                        if (CareerC.TemplatePathKH != "" && CareerC.TemplatePathKH != null)
                        {
                            fileName = CareerC.TemplatePathKH;
                        }
                        else
                        {
                            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TEMPLATE", user.Lang);
                            //ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TEMPLATE", user.Lang);
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                        }
                    }
                    SYExecuteFindAndReplace Para = new SYExecuteFindAndReplace();
                    string TfileName = Server.MapPath(fileName);
                    var STAFFP = unitOfWork.Repository < HRStaffProfile>().FirstOrDefault(w => w.EmpCode == empCareer.EmpCode);
                    //var STAFF = DB.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == CareerC.EmpCode);
                    var POS = unitOfWork.Repository < HRPosition>().FirstOrDefault(w => w.Code == empCareer.JobCode);
                    //var EffDate = DB.HREmpCareerEffectDates.FirstOrDefault(w => w.EmpCode == empCareer.EmpCode);
                    //var NATION = DB.HRNations.FirstOrDefault(w => w.Code == STAFFP.Nation);
                    if (POS.SecDescription == null)
                    {
                        POS.SecDescription = POS.Description;
                    }

                    var SIGNEmp = unitOfWork.Repository < HRStaffProfile>().FirstOrDefault(w => w.EmpCode == SignEmpCode);
                    Para.HDHeader = new ClsHeadDepartment();
                    if (SIGNEmp != null)
                    {
                        Para.HDHeader.HDName = SIGNEmp.AllName;
                        Para.HDHeader.HDNameKH = SIGNEmp.OthAllName;
                        var HDPOS = unitOfWork.Repository < HRPosition>().FirstOrDefault(w => w.Code == SIGNEmp.JobCode);
                        if (HDPOS != null)
                        {
                            Para.HDHeader.HDPosition = HDPOS.Description;
                            Para.HDHeader.HDPositionKH = HDPOS.SecDescription;
                        }
                    }

                    string FileSource = Server.MapPath("~/Content/TEMPLATE/" + empCareer.EmpCode + CareerC.Description + "KH.docx");

                    Para.ListObjectDictionary = new List<object>();
                    // var contract = DB.HREmpContracts.FirstOrDefault(w => w.TranNo == TranNo);
                    //Para.ListObjectDictionary.Add(contract);
                    //Para.ListObjectDictionary.Add(STAFF);
                    Para.ListObjectDictionary.Add(POS);
                    //Para.ListObjectDictionary.Add(Company);
                    Para.ListObjectDictionary.Add(empCareer);
                    // Para.ListObjectDictionary.Add(EffDate);
                    Para.ListObjectDictionary.Add(STAFFP);
                    //if (Province != null)
                    //{
                    //    Para.ListObjectDictionary.Add(Province);
                    //}
                    //if (NID != null)
                    //{
                    //    Para.ListObjectDictionary.Add(NID);
                    //}
                    //if (NATION != null)
                    //{
                    //    Para.ListObjectDictionary.Add(NATION);
                    //}   
                    // Para.ListObjectDictionary.Add(StartDate);
                    var msg = Para.ExecuteFindAndReplaceDOC(TfileName, FileSource, "EmpCareer");
                    if (msg == SYConstant.OK)
                    {

                        Response.Clear();
                        Response.Buffer = true;
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + empCareer.EmpCode + CareerC.Description + "KH.docx");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.WriteFile(FileSource);
                        Response.End();

                    }
                    else
                    {
                        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    }
                    //return null;
                    //return RedirectToAction("Index");
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        public ActionResult UploadControlCallbackActionJobDis()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Repository <CFUploadPath>().Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("Uploadjobdisc", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE2] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #region "Import"
        public ActionResult GridItemHis()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            IClsEmployee objStaff = new ClsEmployee();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                if (objStaff.ListEmp != null)
                {
                    objStaff.ListEmp.Clear();
                }
            }
            if (objStaff.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in objStaff.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
            }
            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView("GridItemHis", objStaff);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRCareerHistory", SYSConstant.DEFAULT_UPLOAD_LIST);

            BSM = new ClsEmployee();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListEmp != null)
                {
                    BSM.ListEmp.Clear();
                }
            }

            BSM.ListTemplate = unitOfWork.Repository < MDUploadTemplate>().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in BSM.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {

            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRCareerHistory", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "STAFF_CAREER"));
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "CareerHistory", SYSConstant.DEFAULT_UPLOAD_LIST);
            IEnumerable<MDUploadTemplate> listu = unitOfWork.Repository < MDUploadTemplate>().Where(w => w.ScreenId == SCREEN_ID).OrderByDescending(w => w.UploadDate);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }

        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = unitOfWork.Repository <MDUploadTemplate>().Find(id);
            if (obj != null)
            {
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
                    var BSM = new ClsEmployee();

                    BSM.ScreenId = ScreendIDControl;
                    BSM.ListCareer = new List<HREmpCareer>();
                    try
                    {

                        DateTime create = DateTime.Now;
                        if (dt.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                var objHeader = new HREmpCareer();
                                objHeader.EmpCode = dt.Rows[i][0].ToString();
                                objHeader.CareerCode = dt.Rows[i][2].ToString();
                                objHeader.Increase = (decimal)SYSettings.getNumberValue(dt.Rows[i][5].ToString());
                                objHeader.EffectDate = SYSettings.getDateValue(dt.Rows[i][4].ToString());
                                objHeader.resigntype = dt.Rows[i][3].ToString();
                                objHeader.LOCT = dt.Rows[i][6].ToString();
                                objHeader.Branch = dt.Rows[i][7].ToString();
                                objHeader.Division = dt.Rows[i][8].ToString();
                                objHeader.GroupDept = dt.Rows[i][9].ToString();
                                objHeader.DEPT = dt.Rows[i][10].ToString();
                                objHeader.Office = dt.Rows[i][11].ToString();
                                objHeader.SECT = dt.Rows[i][12].ToString();
                                objHeader.Groups = dt.Rows[i][13].ToString();
                                objHeader.JobCode = dt.Rows[i][14].ToString();
                                objHeader.LevelCode = dt.Rows[i][15].ToString();
                                objHeader.CATE = dt.Rows[i][16].ToString();
                                objHeader.Remark = dt.Rows[i][17].ToString();
                                objHeader.NewEmpCode = dt.Rows[i][18].ToString();
                                BSM.ListCareer.Add(objHeader);
                            }

                            var msg = BSM.uploadCareer();
                            if (msg != SYConstant.OK)
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + BSM.MessageError;
                                obj.IsGenerate = false;
                                unitOfWork.Update(obj);
                                unitOfWork.Save();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }

                        }
                        obj.Message = SYConstant.OK;
                        foreach(var read in BSM.ListCareer)
                        if (!string.IsNullOrEmpty(read.NewEmpCode))
                        {
                            var ChkCode = unitOfWork.Repository<HRStaffProfile>().FirstOrDefault(w => w.EmpCode == read.EmpCode);
                            var ChkNCode = unitOfWork.Repository < HRStaffProfile>().FirstOrDefault(w => w.EmpCode == read.NewEmpCode);
                            if (ChkCode != null && ChkNCode == null)
                            {
                                var DBC = new HumicaDBContext();
                                string Cmd = "exec dbo.Change_EmpCode '" + read.NewEmpCode + "','" + read.EmpCode + "','" + user.UserName + "'";
                                DBC.Database.ExecuteSqlCommand(Cmd);
                            }
                        }
                        //obj.DocumentNo = DocBatch.NextNumberRank;
                        obj.IsGenerate = true;
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                    }
                    catch (DbUpdateException e)
                    {
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.ScreenId = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/

                    }
                    catch (Exception e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.DocurmentAction = "UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
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
            //DevExpress.Spreadsheet.
            var RelCode = unitOfWork.Repository<HRCareerHistory>().Queryable().ToList();
            var SepCode = unitOfWork.Repository < HRTerminType>().Queryable().ToList();
            using (var workbook = new DevExpress.Spreadsheet.Workbook())
            {
                workbook.Worksheets[0].Name = "Master";
                List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>();
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee ID" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Name" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Career Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Separate Type" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "EffectDate\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Increase\n(number)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Location" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Branch" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Division" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Business Unit" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName ="Department" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName ="Office" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName ="Section" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName ="Team" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName ="Position" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Level" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Category" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Remark" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "New Employee ID" });
                Worksheet worksheet = workbook.Worksheets[0];
                var sheet2 = workbook.Worksheets.Add("Career-Code");
                List<ExCFUploadMapping> _ListMaster1 = new List<ExCFUploadMapping>();
                _ListMaster1.Add(new ExCFUploadMapping { FieldName = "Code" });
                _ListMaster1.Add(new ExCFUploadMapping { FieldName= "Description" });
                List<ClsUploadMapping> _ListData = new List<ClsUploadMapping>();
                foreach (var read in RelCode)
                {
                    _ListData.Add(new ClsUploadMapping
                    {
                        FieldName = read.Code,
                        FieldName1 = read.Description,
                        FieldName2 = read.SecDescription
                    });
                }
                var sheet3 = workbook.Worksheets.Add("Separate-Type");
                List<ExCFUploadMapping> _ListMaster2 = new List<ExCFUploadMapping>();
                _ListMaster2.Add(new ExCFUploadMapping { FieldName = "Code" });
                _ListMaster2.Add(new ExCFUploadMapping { FieldName = "Description" });
                List<ClsUploadMapping> _ListData2 = new List<ClsUploadMapping>();
                foreach (var read in SepCode)
                {
                    _ListData2.Add(new ClsUploadMapping
                    {
                        FieldName = read.Code,
                        FieldName1 = read.Description,
                        FieldName2 = read.OthDesc
                    });
                }
                ClsConstant.ExportDataToWorksheet(worksheet, _ListMaster);
                ClsConstant.ExportDataToWorksheet(sheet2, _ListMaster1);
                ClsConstant.ExportDataToWorksheetRow(sheet2, _ListData);
                //
                ClsConstant.ExportDataToWorksheet(sheet3, _ListMaster2);
                ClsConstant.ExportDataToWorksheetRow(sheet3, _ListData2);
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CAREER_TEMPLATE.xlsx");
                }
            }
            return null;
        }

        #endregion
        #region Grid Salary Info
        public ActionResult GridItemsHisBanklist()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemsHisBanklist";
            return PartialView("GridItemsHisBanklist", BSM.ListHisBanKList);
        }
        public ActionResult Banklist()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            bool isSalaryVisible = BSM.IsHideSalary(BSM.HeaderCareer.LevelCode);

            //* don't delete this code
            //if (!string.IsNullOrEmpty(BSM.IsSalary))
            //    isSalaryVisible= BSM.IsHideSalary(BSM.IsSalary);
            ViewData[ClsConstant.IS_SALARY] = isSalaryVisible;
            //

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("Banklist", BSM.ListCareerBankList);
        }
        public ActionResult BanklistDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("BanklistDetial", BSM.ListCareerBankList);
        }
        public ActionResult CreateBank(HREmpCareerBankList ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            try
            {
                if (!string.IsNullOrEmpty(ModelObject.Company) && ModelObject.OldSalary >= 0)
                {
                    int id = 0;
                    if (BSM.ListCareerBankList.Count > 0) id = BSM.ListCareerBankList.Max(w => w.ID);
                    if (BSM.ListCareerBankList.Where(w => w.Company == ModelObject.Company && w.FromDate == ModelObject.FromDate).Count() == 0)
                    {

                        var EmpCareerBankList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().FirstOrDefault(x => x.EmpCode == BSM.HeaderCareer.EmpCode && x.Company == ModelObject.Company);

                        var BankAcc = unitOfWork.Repository<HREmpBankAcc>().Queryable().FirstOrDefault(x => x.EmpCode == BSM.HeaderCareer.EmpCode && x.Company == ModelObject.Company);

                        if (EmpCareerBankList == null && BankAcc != null)
                        {
                            ModelObject.OldSalary = BankAcc.Salary;
                        }
                        else if (EmpCareerBankList != null)
                        {
                            ModelObject.OldSalary = EmpCareerBankList.NewSalary;
                        }
                        ModelObject.ID = id + 1;
                        ModelObject.NewSalary = ModelObject.OldSalary + ModelObject.Increase;

                        ModelObject.Todate = new DateTime(1900, 1, 1);

                        BSM.ListCareerBankList.Add(ModelObject);

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
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataSelector();
            return PartialView("Banklist", BSM.ListCareerBankList);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBank(HREmpCareerBankList ObjType)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
                var listCheck = BSM.ListCareerBankList.Where(w => w.Company == ObjType.Company).ToList();
                if (listCheck.ToList().Count > 0)
                {
                    var objUpdate = listCheck.First();
                    objUpdate.Company = ObjType.Company;
                    objUpdate.OldSalary = ObjType.OldSalary;
                    objUpdate.Increase = ObjType.Increase;
                    objUpdate.NewSalary = ObjType.OldSalary + ObjType.Increase;
                    objUpdate.FromDate = ObjType.FromDate;
                    objUpdate.Todate = new DateTime(5000, 1, 1);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            DataSelector();
            return PartialView("Banklist", BSM.ListCareerBankList);
        }
        public ActionResult Deletebank(int ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
                }
                var error = 0;
                bool isSalaryVisible = BSM.IsHideSalary(BSM.HeaderCareer.LevelCode);
          
                var checkList = BSM.ListCareerBankList.Where(w => w.ID == ID).ToList();
                if (checkList.Count == 0)
                {
                    ViewData["EditError"] = SYMessages.getMessage("NO_ITEM_DELETE");
                    error = 1;
                }

                if (error == 0 && isSalaryVisible)
                {
                    BSM.ListCareerBankList.Remove(checkList.First());
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
                else if (!isSalaryVisible) ViewData["EditError"] = SYMessages.getMessage("USER_CANNOT_DELETE");
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            DataSelector();

            return PartialView("Banklist", BSM.ListCareerBankList);
        }
        #endregion
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListCareerHis);
        }
        public ActionResult ShowData(string ID, string Tran)
        {
            ActionName = "Details";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                int TranNo = Convert.ToInt32(Tran);
                var Carceer = BSM.ListCareer.FirstOrDefault(x => x.TranNo == TranNo);
                BSM.HeaderCareer = Carceer;
                var IsSalary = BSM.IsHideSalary(BSM.HeaderCareer.LevelCode);
                if (IsSalary == true)
                {
                    ViewData[ClsConstant.IS_SALARY] = false;
                    BSM.NewSalary = BSM.HeaderCareer.NewSalary.ToString();
                    BSM.OldSalary = BSM.HeaderCareer.OldSalary.ToString();
                    BSM.Increase = BSM.HeaderCareer.Increase.ToString();
                }
                else
                {
                    ViewData[ClsConstant.IS_SALARY] = true;
                    BSM.NewSalary = "#####";
                    BSM.OldSalary = "#####";
                    BSM.Increase = "#####";
                }
                var result = new
                {
                    MS = SYConstant.OK,
                    CompanyCode = BSM.HeaderCareer.CompanyCode,
                    EmpType = BSM.HeaderCareer.EmpType,
                    Branch = BSM.HeaderCareer.Branch,
                    Division = BSM.HeaderCareer.Division,
                    DEPT = BSM.HeaderCareer.DEPT,
                    SECT = BSM.HeaderCareer.SECT,
                    JobCode = BSM.HeaderCareer.JobCode,
                    LevelCode = BSM.HeaderCareer.LevelCode,
                    CareerCode = BSM.HeaderCareer.CareerCode,
                    EffectDate = BSM.HeaderCareer.EffectDate,
                    OldSalary = BSM.OldSalary,
                    Increase = BSM.Increase,
                    NewSalary = BSM.NewSalary,
                    ToDate = BSM.HeaderCareer.ToDate,
                    Cate = BSM.HeaderCareer.CATE,
                    Remark = BSM.HeaderCareer.Remark
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Refreshvalue(string id, string Increase)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                decimal Inc = Convert.ToDecimal(Increase);
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderCareer.Increase = Inc;
                if (BSM.Increase != "#####")
                    BSM.HeaderCareer.NewSalary = BSM.HeaderCareer.OldSalary + Inc;
                else
                    BSM.HeaderCareer.NewSalary = 0;
                var result = new
                {
                    MS = SYConstant.OK,
                    NewSalary = BSM.HeaderCareer.NewSalary
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            this.ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            //UserSession();
            //DataSelector();
            //ActionName = "Details";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
                var listEmpCareer = BSM.GetEmployee(EmpCode);
            int tranNo = Convert.ToInt32(listEmpCareer.Max(w => w.TranNo));
            if (BSM.HeaderStaff != null)
            {
                var EmpStaff = BSM.HeaderStaff;
                var EmpCareer = listEmpCareer.FirstOrDefault(w => w.TranNo == tranNo);
                if (EmpCareer != null)
                {
                    var salary = "";
                    var IsSalary = BSM.IsHideSalary(EmpCareer.LevelCode);
                    if (IsSalary == true)
                    {
                        salary = EmpCareer.NewSalary.ToString();
                    }
                    else
                    {
                        ViewData[ClsConstant.IS_SALARY] = true;
                        salary = "#####";
                    }
                    //JobType(EmpCareer.Branch, "Branch");
                    var result = new
                    {
                        MS = SYConstant.OK,
                        AllName = EmpStaff.AllName,
                        EmpType = EmpStaff.EmployeeType,
                        Division = EmpStaff.Division,
                        DEPT = EmpStaff.Department,
                        SECT = EmpStaff.Section,
                        LevelCode = EmpStaff.Level,
                        Position = EmpStaff.Position,
                        StartDate = EmpStaff.StartDate,

                        CCompany = EmpCareer.CompanyCode,
                        CEmpType = EmpCareer.EmpType,
                        CBranch = EmpCareer.Branch,
                        CLocation = EmpCareer.LOCT,
                        CDivi = EmpCareer.Division,
                        CGDept = EmpCareer.GroupDept,
                        COffice = EmpCareer.Office,
                        CGroups = EmpCareer.Groups,
                        CDept = EmpCareer.DEPT,
                        CLine = EmpCareer.LINE,
                        CSect = EmpCareer.SECT,
                        CPost = EmpCareer.JobCode,
                        CJopGrade = EmpCareer.JobGrade,
                        CLevel = EmpCareer.LevelCode,
                        CEff = DateTime.Now,
                        StaffType = EmpCareer.StaffType,
                        CSalary = salary,
                        Cate = EmpCareer.CATE
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    //GetData(EmpCode, "Create");
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var result = new
                    {
                        MS = SYConstant.OK,
                        AllName = EmpStaff.AllName,
                        EmpType = EmpStaff.EmployeeType,
                        Division = EmpStaff.Division,
                        DEPT = EmpStaff.Department,
                        SECT = EmpStaff.Section,
                        LevelCode = EmpStaff.Level,
                        Position = EmpStaff.Position,
                        StartDate = EmpStaff.StartDate
                    };
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    //GetData(EmpCode, "Create");
                    return Json(result, JsonRequestBehavior.DenyGet);
                }

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowDataBankList(string ID, string EmpCode, string Code)
        {
            UserSession();
            DataSelector();
            ActionName = "Details";
            HREmpCareerBankList EmpStaff = unitOfWork.Repository<HREmpCareerBankList>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode && w.Company == Code);

            List<HREmpCareerBankList> listEmpCareer = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(x => x.EmpCode == EmpCode).ToList();
            int tranNo = Convert.ToInt32(listEmpCareer.Max(w => w.Reference));
            if (EmpStaff != null)
            {
                var EmpCareer = listEmpCareer.FirstOrDefault(w => w.Reference == tranNo);
                if (EmpCareer != null)
                {

                    //JobType(EmpCareer.Company, "Branch");
                    var result = new
                    {
                        MS = SYConstant.OK,
                        OldSalary = EmpCareer.NewSalary
                    };
                    GetDataBank(EmpCode, "Create", Code);
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
                else
                {
                    var result = new
                    {
                        MS = SYConstant.OK,

                    };
                    GetData(EmpCode, "Create");
                    return Json(result, JsonRequestBehavior.DenyGet);
                }
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }

        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            BSM.ListCareerHis = new List<HR_Career>();
            BSM.ListCareerBankList = new List<HREmpCareerBankList>();
            BSM.ListHisBanKList = new List<HR_HisBanKList>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var resualt = unitOfWork.Repository<HREmpCareer>().Queryable();
                List<HREmpCareer> listEmpCareer = resualt.Where(x => x.EmpCode == EmpCode).ToList();
                int tranNo = Convert.ToInt32(listEmpCareer.Max(w => w.TranNo));
                BSM.IsValidChecking(tranNo);
                var IsSalary = BSM.IsHideSalary(BSM.HeaderCareer.LevelCode);
                //if (!string.IsNullOrEmpty(BSM.IsSalary))
                //    IsSalary = BSM.IsHideSalary(BSM.IsSalary);

                //Salary Info
                var OldCareer = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.Reference == tranNo && w.EmpCode == EmpCode).ToList();

                foreach (var read in OldCareer)
                {
                    var obj = new HREmpCareerBankList();
                    obj = read;
                    if (IsSalary)
                    {
                        obj.OldSalary = read.NewSalary;
                        obj.Increase = 0;
                        obj.NewSalary = read.NewSalary;
                    }
                    else
                    {
                        obj.OldSalary = 0;
                        obj.Increase = 0;
                        obj.NewSalary = 0;
                    }

                    BSM.ListCareerBankList.Add(obj);
                }
                //Salary Info History
                List<HREmpCareerBankList> bankList = unitOfWork.Repository<HREmpCareerBankList>().Queryable().Where(w => w.EmpCode == EmpCode).ToList();
                foreach (var read in bankList.OrderByDescending(w => w.FromDate).ToList())
                {
                    var obj = new HR_HisBanKList();

                    obj.FromDate = read.FromDate.Value;
                    obj.Todate = read.Todate.Value;
                    obj.Branch = read.Company;
                    if (IsSalary == true)
                    {
                        obj.NewSalary = read.NewSalary.ToString();
                        obj.OldSalary = read.OldSalary.ToString();
                        obj.Increase = read.Increase.ToString();
                    }
                    else
                    {
                        obj.NewSalary = "#####";
                        obj.OldSalary = "#####";
                        obj.Increase = "#####";
                    }
                    BSM.ListHisBanKList.Add(obj);
                }
                foreach (var read in listEmpCareer.OrderByDescending(w => w.FromDate).ToList())
                {
                    var obj = new HR_Career();
                    obj.FromDate = read.FromDate.Value;
                    obj.ToDate = read.ToDate.Value;
                    obj.Department = read.DEPT;
                    obj.Position = read.JobCode;
                    obj.Career = read.CareerCode;
                    obj.Level = read.LevelCode;
                    if (IsSalary == true)
                    {
                        obj.NewSalary = read.NewSalary.ToString();
                        obj.OldSalary = read.OldSalary.ToString();
                        obj.Increase = read.Increase.ToString();
                    }
                    else
                    {
                        obj.NewSalary = "#####";
                        obj.OldSalary = "#####";
                        obj.Increase = "#####";
                    }
                    obj.CreatedBy = read.CreateBy;
                    obj.ChangedBy = read.ChangedBy;
                    BSM.ListCareerHis.Add(obj);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_Employee");
            }
        }
        public JsonResult GetDataBank(string EmpCode, string Action, string Code)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];

                return Json(new
                {
                    Success = true,
                    Data = BSM.ListCareerBankList
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Success = false,
                    Message = SYMessages.getMessage("PLEASE_SEARCH_Employee")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult FilterLevel(string Level)
        {
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            var IsSalary = BSM.IsHideSalary(Level);
            if (IsSalary == true)
            {
                BSM.IsSalary = Level;
            }
            else BSM.IsSalary = null;
            Session[Index_Sess_Obj + ActionName] = BSM;
            var result = new
            {
                MS = SYConstant.OK,
            };
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public ActionResult SelectSeperateType(string Code)
        {
            ClsFilterJob FilterJob = new ClsFilterJob();
            ViewData["SEPARATE_SELECT"] = FilterJob.LoadSeperateType(Code).ToList();
            var data = new
            {
                MS = SYConstant.OK,
                DATA = ViewData["SEPARATE_SELECT"]
            };
            return Json(data, JsonRequestBehavior.DenyGet);
        }

        private void DataSelector(params object[] keys)
        {
            bool IsDetail = false;
            if (keys != null && keys.Length > 0)
            {
                IsDetail = (bool)keys[0];
            }
            foreach (var data in BSM.OnDataJobLoading(IsDetail))
            {
                ViewData[data.Key] = data.Value;
            }
            foreach (var data in BSM.OnDataListCarrer(IsDetail))
            {
                ViewData[data.Key] = data.Value;
            }
            //ViewData["STAFF_SELECT"] = ListStaffs;
        }
        private void DataList()
        {
            foreach (var data in BSM.OnDataListLoading())
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

        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Repository<CFUploadPath>().Find("IMG_UPLOAD");
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
    }
}
