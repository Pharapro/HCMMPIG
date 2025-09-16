using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Employee;
using Humica.Logic.HR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRImportSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000011";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRImportSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCode";
        IClsEmployee BSM;
        IUnitOfWork unitOfWork;
        public HRImportSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsEmployee();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ActionResult Index()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListStaffProfile);
        }



        public ActionResult GridItems()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListStaffProfile != null)
                {
                    BSM.ListStaffProfile.Clear();
                }
            }
            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in BSM.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
                DataTable dtHeader = excel.GenerateExcelData();
                BSM.ListStaffProfile = new List<HRStaffProfile>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRStaffProfile();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.AllName = dtHeader.Rows[i][1].ToString();
                        objHeader.Salary = (decimal)(int?)SYSettings.getNumberValue(dtHeader.Rows[i][2].ToString());
                        BSM.ListStaffProfile.Add(objHeader);
                    }
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        #region "Import"
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRImportSalary", SYSConstant.DEFAULT_UPLOAD_LIST);

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListStaffProfile != null)
                {
                    BSM.ListStaffProfile.Clear();
                }

            }
            BSM.ListTemplate = unitOfWork.Repository<MDUploadTemplate>().Queryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in BSM.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }

                DataTable dtHeader = excel.GenerateExcelData();

                BSM.ListStaffProfile = new List<HRStaffProfile>();
                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new HRStaffProfile();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.AllName = dtHeader.Rows[i][1].ToString();
                        objHeader.Salary = SYSettings.getNumberValue(dtHeader.Rows[i][2].ToString());

                        BSM.ListStaffProfile.Add(objHeader);

                    }
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "HRImportSalary", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(unitOfWork.Repository<CFUploadPath>().Queryable().FirstOrDefault(w => w.PathCode == "IMP_SALARY"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "HR";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);


            var objStaff = new ClsEmployee();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            objStaff.ListTemplate = unitOfWork.Repository<MDUploadTemplate>().Queryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListStaffProfile = new List<HRStaffProfile>();

            Session[Index_Sess_Obj + ActionName] = objStaff;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ImportSalary", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new ClsEmployee();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ClsEmployee)Session[Index_Sess_Obj + ActionName];
            }
            objStaff.ListTemplate = unitOfWork.Repository<MDUploadTemplate>().Queryable().Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            objStaff.ListStaffProfile = new List<HRStaffProfile>();


            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = unitOfWork.Repository<MDUploadTemplate>().Queryable().FirstOrDefault(w => w.ID == id);
            if (obj != null)
            {
                var DBB = new HumicaDBContext();
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
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
                        if (dtHeader.Rows.Count > 0) // Header
                        {
                            BSM.ListSaparate = new List<HREmpBankAcc>();
                            if (dtHeader != null)
                            {
                                for (int i = 0; i < dtHeader.Rows.Count; i++)
                                {
                                    var objHeader = new HREmpBankAcc();
                                    objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                    objHeader.Company = dtHeader.Rows[i][2].ToString();
                                    objHeader.Salary = SYSettings.getNumberValue(dtHeader.Rows[i][3].ToString());
                                    objHeader.BankName = dtHeader.Rows[i][4].ToString();
                                    objHeader.AccountNo = dtHeader.Rows[i][5].ToString();
                                    objHeader.AccountName = dtHeader.Rows[i][6].ToString();
                                    int IsTax = Convert.ToInt32(dtHeader.Rows[i][7].ToString());
                                    objHeader.IsTax = Convert.ToBoolean(IsTax);
                                    int IsNSSF = Convert.ToInt32(dtHeader.Rows[i][8].ToString());
                                    objHeader.IsNSSF = Convert.ToBoolean(IsNSSF);
                                    BSM.ListSaparate.Add(objHeader);

                                }
                            }
                        }
                        msg = BSM.uploadSalary();
                        if (msg != SYConstant.OK)
                        {
                            obj.Message = SYMessages.getMessage(msg);
                            obj.Message += ":" + BSM.MessageError;
                            obj.IsGenerate = false;
                            unitOfWork.Update(obj);
                            unitOfWork.Save();
                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                        }
                        obj.Message = SYConstant.OK;
                        obj.IsGenerate = true;
                        DBB.MDUploadTemplates.Attach(obj);
                        DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                        DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DBB.SaveChanges();

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

            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
        }

        public ActionResult DownloadTemplate()
        {
            string fileName = Server.MapPath("~/Content/TEMPLATE/TemplateSalary.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=TemplateSalary.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion

        private void DataSelector()
        {
            //ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            //ViewData["APPRTYPE_SELECT"] = DH.HRApprTypes.ToList();
            //ViewData["HREmpEduType_LIST"] = DH.HREduTypes.ToList();
        }
    }
}