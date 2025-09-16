using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRLateEarlyController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000023";
        private const string URL_SCREEN = "/PR/PRM/PRLateEarly/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRLateEarlyController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }

            var ListEmpLateEarly = DBV.HR_EmpLateEarly_View.ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            BSM.ListEmpLateEarly = ListEmpLateEarly.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRDeduction BSM = new PRDeduction();
            BSM.ListEmpLateEarly = new List<HR_EmpLateEarly_View>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListEmpLateEarly);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PRDeduction BSM = new PRDeduction();
            UserConfListAndForm(this.KeyName);
            BSM.HeaderLE = new PREmpLateDeduct();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.ListHeaderLE = new List<PREmpLateDeduct>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PRDeduction collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderStaff = collection.HeaderStaff;
            }
            BSM.ScreenId = SCREEN_ID;
            try
            {
                string msg = BSM.CreateLateEarly();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.HeaderLE.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.HeaderLE = new PREmpLateDeduct();
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    BSM.ListHeaderLE = new List<PREmpLateDeduct>();
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
                return View(BSM);
            }
        }
        #endregion
        #region Edit
        public ActionResult Edit(string id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                PRDeduction BSM = new PRDeduction();
                int trn = 0;
                trn = Convert.ToInt32(id);
                BSM.ListHeaderLE = DB.PREmpLateDeducts.Where(w => w.TranNo == trn).ToList();

                if (BSM.ListHeaderLE.Count > 0)
                {
                    var obj = BSM.ListHeaderLE.First();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == obj.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PRDeduction collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderStaff = collection.HeaderStaff;
            }
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditLateEarly(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            //EE001= Message load Error data not complate
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);

        }
        #endregion
        #region Delete
        public ActionResult Delete(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (id != 0)
            {
                PRDeduction GLA = new PRDeduction();
                string msg = GLA.DeleteLateEarly(id);
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
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion

        #region Grid Create Action
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            Humica.Logic.PR.PRDeduction BSM = new Humica.Logic.PR.PRDeduction();
            BSM.ListHeaderLE = new List<PREmpLateDeduct>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM);
        }

        public ActionResult CreateItem(PREmpLateDeduct ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRDeduction BSM = new PRDeduction();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                        var Result = BSM.IsExistLateEarlyDeduction(BSM.HeaderStaff.EmpCode, ModelObject.DedCode, ModelObject.FromDate.Value, ModelObject.ToDate.Value);
                        if (Result == -1)
                        {
                            BSM.ListHeaderLE.Add(ModelObject);

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
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            ////DataSelector();

            return PartialView("GridItems", BSM);
        }

        public ActionResult CreateItemEdit(PREmpLateDeduct ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRDeduction BSM = new PRDeduction();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                        var Result = BSM.IsExistLateEarlyDeduction(BSM.HeaderStaff.EmpCode, ModelObject.DedCode, ModelObject.FromDate.Value, ModelObject.ToDate.Value);
                        if (Result == -1)
                        {
                            var objCheck = BSM.ListHeaderLE.Where(w => w.DedCode == ModelObject.DedCode).ToList();
                            if (objCheck.Count > 0)
                            {
                                objCheck.First().Day = ModelObject.Day;
                                objCheck.First().Minute = ModelObject.Minute;
                                objCheck.First().FromDate = ModelObject.FromDate;
                                objCheck.First().ToDate = ModelObject.ToDate;
                                objCheck.First().Remark = ModelObject.Remark;
                            }
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
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            ////DataSelector();

            return PartialView("GridItems", BSM);
        }

        public ActionResult CreateItemDelete(string DedCode)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ActionName = "Create";
            PRDeduction BSM = new PRDeduction();
            if (DedCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                    if (BSM.ListHeaderLE.Count > 0)
                    {
                        var objCheck = BSM.ListHeaderLE.FirstOrDefault(w => w.DedCode == DedCode);
                        BSM.ListHeaderLE.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
        }
        #endregion

        #region Grid Edit Action
        public ActionResult GridItemsEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            Humica.Logic.PR.PRDeduction BSM = new Humica.Logic.PR.PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItemsEdit", BSM);
        }

        public ActionResult CreateEditItem(PREmpLateDeduct ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRDeduction BSM = new PRDeduction();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];

                        var objCheck = BSM.ListHeaderLE.Where(w => w.DedCode == ModelObject.DedCode).ToList();
                        if (objCheck.Count == 0)
                        {
                            BSM.ListHeaderLE.Add(ModelObject);
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("HOUSE_DUPLICATED");
                        }

                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            ////DataSelector();

            return PartialView("GridItemsEdit", BSM);
        }

        public ActionResult EditItemEdit(PREmpLateDeduct ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRDeduction BSM = new PRDeduction();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];

                        var objCheck = BSM.ListHeaderLE.Where(w => w.DedCode == ModelObject.DedCode).ToList();
                        if (objCheck.Count > 0)
                        {
                            objCheck.First().Day = ModelObject.Day;
                            objCheck.First().Minute = ModelObject.Minute;
                            objCheck.First().FromDate = ModelObject.FromDate;
                            objCheck.First().ToDate = ModelObject.ToDate;
                            objCheck.First().Remark = ModelObject.Remark;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                        }
                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            ////DataSelector();

            return PartialView("GridItemsEdit", BSM);
        }

        public ActionResult EditItemDelete(string DedCode)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ActionName = "Edit";
            PRDeduction BSM = new PRDeduction();
            if (DedCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                    if (BSM.ListHeaderLE.Count > 0)
                    {
                        var objCheck = BSM.ListHeaderLE.FirstOrDefault(w => w.DedCode == DedCode);
                        BSM.ListHeaderLE.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItemsEdit", BSM);
        }
        #endregion
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW.ToList();
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.DivisionDesc,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                GetData(EmpCode, "Create");
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);

        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            PRDeduction BSM = new PRDeduction();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];

                BSM.HeaderStaff.EmpCode = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_EMPLOYEE");
            }
        }

        #region "Import"
        public ActionResult GridItemsInOut()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListHeaderLE != null)
                {
                    BSM.ListHeaderLE.Clear();
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
                BSM.ListHeaderLE = new List<PREmpLateDeduct>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new PREmpLateDeduct();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.DedCode = dtHeader.Rows[i][1].ToString();
                        objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                        objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][3].ToString());
                        objHeader.Minute = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                        objHeader.Remark = dtHeader.Rows[i][5].ToString();
                        BSM.ListHeaderLE.Add(objHeader);
                    }
                }


            }
            //BSM.ListDepreMethod = DB.ExFADepreciationMethods.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsInOut", BSM);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRDeduction", SYSConstant.DEFAULT_UPLOAD_LIST);

            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            if (BSM.ListTemplate.Count > 0)
            {
                SYExcel excel = new SYExcel();
                foreach (var read in BSM.ListTemplate.ToList())
                {
                    excel.FileName = read.UpoadPath;
                }
                DataTable dtHeader = excel.GenerateExcelData();
                BSM.ListHeaderLE = new List<PREmpLateDeduct>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new PREmpLateDeduct();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.DedCode = dtHeader.Rows[i][1].ToString();
                        objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                        objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][3].ToString());
                        objHeader.Minute = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                        objHeader.Remark = dtHeader.Rows[i][5].ToString();
                        BSM.ListHeaderLE.Add(objHeader);
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRDeduction", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("INOUT"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "PAYROLL";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);


            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }


            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeaderLE = new List<PREmpLateDeduct>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
            //return null;
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRDeduction", SYSConstant.DEFAULT_UPLOAD_LIST);

            PRDeduction BSM = new PRDeduction();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRDeduction)Session[Index_Sess_Obj + ActionName];
            }


            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeaderLE = new List<PREmpLateDeduct>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView(SYListConfuration.ListDefaultUpload, BSM.ListTemplate);
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            if (obj != null)
            {
                var DBB = new HumicaDBContext();
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                PRDeduction BSM = new PRDeduction();
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
                            BSM.ListHeaderLE = new List<PREmpLateDeduct>();
                            //objStaff.ListItem = new List<HRStaffProfile>();
                            //objStaff.ListPlanItem = new List<HLContractPlanItem>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new PREmpLateDeduct();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.DedCode = dtHeader.Rows[i][1].ToString();
                                objHeader.FromDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][2].ToString());
                                objHeader.ToDate = SYSettings.getDateTimeValue(dtHeader.Rows[i][3].ToString());
                                objHeader.Minute = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                                objHeader.Remark = dtHeader.Rows[i][5].ToString();
                                BSM.ListHeaderLE.Add(objHeader);
                            }

                            msg = "";// BSM.ImportAllowance();
                            if (msg == SYConstant.OK)
                            {
                                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);

                                if (obj != null)
                                {
                                    DB.MDUploadTemplates.Remove(obj);
                                    DB.SaveChanges();

                                    if (System.IO.File.Exists(obj.UpoadPath))
                                    {
                                        System.IO.File.Delete(obj.UpoadPath);
                                    }
                                }

                            }
                            else
                            {
                                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessage(msg, user.Lang) + "(" + BSM.MessageError + ")";
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
            string fileName = Server.MapPath("~/Content/TEMPLATE/ALLOWANCE_TEMPLATE.xlsx");
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=ALLOWANCE_TEMPLATE.xlsx");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.WriteFile(fileName);
            Response.End();
            return null;
        }

        #endregion
        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;
            string DEDType = "DED";
            ViewData["DED_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == DEDType).ToList();
        }
    }
}