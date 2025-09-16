using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Models.SY;
using Humica.Payroll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRGenerateSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private static string Error = "";
        private const string SCREEN_ID = "PRM0000006";
        private const string URL_SCREEN = "/PR/PRM/PRGenerateSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        IClsPRPayroll BSM;
        IUnitOfWork unitOfWork;
        public PRGenerateSalaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsPRPayroll();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.OnIndexLoading();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
                BSM.ListLog = obj.ListLog;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ClsPRPayroll BSM)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            var msg = BSM.OnIndexLoadingFilter();
            if (msg != SYConstant.OK)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #endregion
        public ActionResult Generate()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            var msg = BSM.GenerateSalary();
            if (msg == SYConstant.OK)
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public ActionResult Delete(int Period)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            string msg = BSM.Delete_GenerateAll(Period);
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
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListEmployeeGen);
        }
        public ActionResult GridItemLog()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.LIST);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemLog", BSM.ListLog);
        }
        #region "Import"
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(this.KeyName);
            SYUpLoadTemplate upLoadTemplate = new SYUpLoadTemplate();
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRGenerateSalary", SYSConstant.DEFAULT_UPLOAD_LIST);
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRGenerateSalary", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(unitOfWork.Set<CFUploadPath>().Find("IMP_SALARY"));
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
            //return null;
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRGenerateSalary", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYUpLoadTemplate upLoadTemplate = new SYUpLoadTemplate();
            IEnumerable<MDUploadTemplate> listu = upLoadTemplate.GetData(SCREEN_ID, user.UserName);
            return PartialView(SYListConfuration.ListDefaultUpload, listu.ToList());
        }
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = unitOfWork.Repository<MDUploadTemplate>().Queryable().FirstOrDefault(w => w.ID == id);
            if (obj != null)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                var objStaff = new ClsPRPayroll();
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
                            objStaff.ListImport = new List<ListUploadPayHis>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new ListUploadPayHis();
                                objHeader.Company = dtHeader.Rows[i][0].ToString();
                                objHeader.EmpCode = dtHeader.Rows[i][1].ToString();
                                objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][3].ToString());
                                objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][4].ToString());
                                objHeader.Rate = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][5].ToString());
                                objHeader.BasicSalary = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][6].ToString());
                                objHeader.UnpaidLeaveD = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][7].ToString());
                                objHeader.UnpaidLeave = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][8].ToString());
                                objHeader.Extra_Motivation = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][9].ToString());
                                objHeader.Monthly_Seniority = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][10].ToString());
                                objHeader.Semester_Seniority = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][11].ToString());
                                objHeader.Gasoline_Phone = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][12].ToString());
                                objHeader.OT = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][13].ToString());
                                objHeader.Adjustment = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][14].ToString());
                                objHeader.GrossPay = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][15].ToString());
                                objHeader.Spouse = (int)SYSettings.getNumberValue(dtHeader.Rows[i][16].ToString());
                                objHeader.Child = (int)SYSettings.getNumberValue(dtHeader.Rows[i][17].ToString());
                                objHeader.Tax = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][18].ToString());
                                objHeader.Association = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][19].ToString());
                                objHeader.CompanyPensionFundAmount = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][20].ToString());
                                objHeader.Loan = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][21].ToString());
                                objHeader.NetWage = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][22].ToString());
                                objStaff.ListImport.Add(objHeader);
                            }

                            msg = objStaff.uploaddata();
                            if (msg == SYConstant.OK)
                            {

                                obj.Message = SYConstant.OK;
                                SYMessages mess = SYMessages.getMessageObject("IMPORTED", user.Lang);
                                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                                obj.IsGenerate = true;
                                unitOfWork.Update(obj);
                                unitOfWork.Save();

                            }
                            else
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + objStaff.MessageError;
                                obj.IsGenerate = false;
                                unitOfWork.Update(obj);
                                unitOfWork.Save();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }


                        }
                    }
                    catch (Exception e)
                    {
                        obj.Message = ClsEventLog.Save_EventLogs(SCREEN_ID, user.UserName, "UPLOAD", SYActionBehavior.ADD.ToString(), e, true);
                        unitOfWork.Update(obj);
                        unitOfWork.Save();
                    }
                }

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }

        public ActionResult DownloadTemplate()
        {
            string fileName = Server.MapPath("~/Content/TEMPLATE/SALARY_TEMPLATE.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=SALARY_TEMPLATE.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }
        #endregion
        [HttpPost]
        public string getEmpCode(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (IClsPRPayroll)Session[Index_Sess_Obj + ActionName];
                BSM.EmpID = EmpCode;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }
        protected void DataSelector()
        {
            foreach (var data in BSM.OnDataSelectorLoading())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
