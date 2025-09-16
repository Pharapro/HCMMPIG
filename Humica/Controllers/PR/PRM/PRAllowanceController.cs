using DevExpress.Spreadsheet;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRAllowanceController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000002";
        private const string URL_SCREEN = "/PR/PRM/PRAllowance/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo;EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PRAllowanceController()
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

            PRAllowance BSM = new PRAllowance();
            BSM.Filter = new Humica.Core.FT.FTINYear();
            DateTime DNow = DateTime.Now;
            BSM.Filter.FromDate = new DateTime(DNow.Year, DNow.Month, 1);
            BSM.Filter.ToDate = new DateTime(DNow.Year, DNow.Month, DateTime.DaysInMonth(DNow.Year, DNow.Month));
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = obj.Filter;
            }


            var ListHeaderLoad = DBV.HR_View_EmpAllowance.Where(w => EntityFunctions.TruncateTime(w.FromDate) <= BSM.Filter.ToDate.Date
            && EntityFunctions.TruncateTime(w.ToDate) >= BSM.Filter.FromDate.Date).ToList();

            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            BSM.ListHeaderLoad = ListHeaderLoad.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(PRAllowance BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            var ListHeader = DBV.HR_View_EmpAllowance.Where(w => EntityFunctions.TruncateTime(w.FromDate) <= BSM.Filter.ToDate.Date
            && EntityFunctions.TruncateTime(w.ToDate) >= BSM.Filter.FromDate.Date).ToList();
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();
            ListHeader = ListHeader.Where(x => Staff.Where(w => w.EmpCode == x.EmpCode).Any()).ToList();

            BSM.ListHeaderLoad = ListHeader.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRAllowance BSM = new PRAllowance();
            BSM.ListHeaderLoad = new List<HR_View_EmpAllowance>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderLoad);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PRAllowance BSM = new PRAllowance();
            UserConfListAndForm(this.KeyName);
            BSM.Header = new PREmpAllw();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.ListHeader = new List<PREmpAllw>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PRAllowance collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRAllowance BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
            }
            BSM.ScreenId = SCREEN_ID;
            try
            {
                string msg = BSM.CreateAllowance();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TranNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber + "&EmpCode=" + BSM.Header.EmpCode;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new PREmpAllw();
                    BSM.HeaderStaff = new HR_STAFF_VIEW();
                    BSM.ListHeader = new List<PREmpAllw>();
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
        #region "Edit"
        public ActionResult Edit(string id, string EmpCode)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id.ToString() != "null")
            {
                PRAllowance BSM = new PRAllowance();
                int trn = 0;
                trn = Convert.ToInt32(id);
                BSM.ListHeader = DB.PREmpAllws.Where(w => w.TranNo == trn && w.EmpCode == EmpCode).ToList();

                if (BSM.ListHeader.Count > 0)
                {
                    var obj = BSM.ListHeader.First();
                    BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == obj.EmpCode);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, string EmpCode, PRAllowance collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = EmpCode;
            var BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderStaff = collection.HeaderStaff;
            }
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditAllowance(id, EmpCode);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber + "&EmpCode=" + BSM.HeaderStaff.EmpCode;
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

        #region "Delete"
        public ActionResult Delete(int id, string EmpCode)
        {
            UserSession();
            UserConfForm(SYActionBehavior.DELETE);
            DataSelector();
            if (id != 0)
            {
                PRAllowance GLA = new PRAllowance();
                string msg = GLA.Delete(id, EmpCode);
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
            Humica.Logic.PR.PRAllowance BSM = new Humica.Logic.PR.PRAllowance();
            BSM.ListHeader = new List<PREmpAllw>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM);
        }

        public ActionResult CreateItem(PREmpAllw ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRAllowance BSM = new PRAllowance();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                        if (ModelObject.FromDate.Value.Date > ModelObject.ToDate.Value.Date)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                        }
                        else
                        {
                            var Result = BSM.IsExistEmployeeAllowance(BSM.HeaderStaff.EmpCode, ModelObject.AllwCode, ModelObject.FromDate.Value, ModelObject.ToDate.Value);
                            if (Result == -1)
                            {
                                BSM.ListHeader.Add(ModelObject);
                                Session[Index_Sess_Obj + ActionName] = BSM;
                            }
                            else
                            {
                                Session[Index_Sess_Obj + ActionName] = BSM;
                                ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                            }
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

        public ActionResult CreateItemEdit(PREmpAllw ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRAllowance BSM = new PRAllowance();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                        if (ModelObject.FromDate.Value.Date > ModelObject.ToDate.Value.Date)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                        }
                        else
                        {
                            var Result = BSM.IsExistEmployeeAllowance(BSM.HeaderStaff.EmpCode, ModelObject.AllwCode, ModelObject.FromDate.Value, ModelObject.ToDate.Value);
                            if (Result == -1)
                            {
                                var objCheck = BSM.ListHeader.Where(w => w.AllwCode == ModelObject.AllwCode).ToList();
                                if (objCheck.Count > 0)
                                {
                                    objCheck.First().Amount = ModelObject.Amount;
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

        public ActionResult CreateItemDelete(string AllwCode)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ActionName = "Create";
            PRAllowance BSM = new PRAllowance();
            if (AllwCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                    if (BSM.ListHeader.Count > 0)
                    {
                        var objCheck = BSM.ListHeader.FirstOrDefault(w => w.AllwCode == AllwCode);
                        BSM.ListHeader.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
        }
        #endregion
        #region 'Grid Edit Action'
        public ActionResult GridItemsEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            Humica.Logic.PR.PRAllowance BSM = new Humica.Logic.PR.PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItemsEdit", BSM);
        }

        public ActionResult CreateEditItem(PREmpAllw ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRAllowance BSM = new PRAllowance();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                        if (ModelObject.FromDate.Value.Date > ModelObject.ToDate.Value.Date)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                        }
                        else
                        {
                            var objCheck = BSM.ListHeader.Where(w => w.AllwCode == ModelObject.AllwCode).ToList();
                            if (objCheck.Count == 0)
                            {
                                var Result = BSM.IsExistEmployeeAllowance(BSM.HeaderStaff.EmpCode, ModelObject.AllwCode, ModelObject.FromDate.Value, ModelObject.ToDate.Value);
                                if (Result == -1)
                                {
                                    BSM.ListHeader.Add(ModelObject);

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
                                ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                            }
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

        public ActionResult EditItemEdit(PREmpAllw ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRAllowance BSM = new PRAllowance();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                        if (ModelObject.FromDate.Value.Date > ModelObject.ToDate.Value.Date)
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                        }
                        else
                        {
                            var Reward = DB.PREmpAllws.Where(w => w.EmpCode == BSM.HeaderStaff.EmpCode && w.AllwCode == ModelObject.AllwCode).ToList();
                            Reward = Reward.Where(w => ((ModelObject.FromDate.Value.Date >= w.FromDate.Value.Date
                                 && ModelObject.FromDate.Value.Date <= w.ToDate.Value.Date) || (ModelObject.ToDate.Value.Date >= w.FromDate.Value.Date
                                 && ModelObject.ToDate.Value.Date <= w.ToDate.Value.Date))).ToList();
                            if (Reward.Count() > 0)
                            {
                                if (Reward.Count == 1)
                                {
                                    var objCheck = BSM.ListHeader.Where(w => w.AllwCode == ModelObject.AllwCode).ToList();
                                    if (Reward.FirstOrDefault().TranNo != objCheck.First().TranNo)
                                    {
                                        Session[Index_Sess_Obj + ActionName] = BSM;
                                        ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                                    }
                                    else if (objCheck.Count > 0)
                                    {
                                        objCheck.First().Amount = ModelObject.Amount;
                                        objCheck.First().FromDate = ModelObject.FromDate;
                                        objCheck.First().ToDate = ModelObject.ToDate;
                                        objCheck.First().Remark = ModelObject.Remark;
                                    }
                                }
                                else
                                {
                                    Session[Index_Sess_Obj + ActionName] = BSM;
                                    ViewData["EditError"] = SYMessages.getMessage("INV_DATE");
                                }
                            }
                            else
                            {
                                var objCheck = BSM.ListHeader.Where(w => w.AllwCode == ModelObject.AllwCode).ToList();
                                if (objCheck.Count > 0)
                                {
                                    objCheck.First().Amount = ModelObject.Amount;
                                    objCheck.First().FromDate = ModelObject.FromDate;
                                    objCheck.First().ToDate = ModelObject.ToDate;
                                    objCheck.First().Remark = ModelObject.Remark;
                                }
                                else
                                {
                                    Session[Index_Sess_Obj + ActionName] = BSM;
                                    ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                                }
                            }
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

        public ActionResult EditItemDelete(string AllwCode)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ActionName = "Edit";
            PRAllowance BSM = new PRAllowance();
            if (AllwCode != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
                    if (BSM.ListHeader.Count > 0)
                    {
                        var objCheck = BSM.ListHeader.FirstOrDefault(w => w.AllwCode == AllwCode);
                        BSM.ListHeader.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItemsEdit", BSM);
        }
        #endregion
        #region "Import & Upload"
        public ActionResult GridItemsImport()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRAllowance BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsImport", BSM);
        }
        public ActionResult Import()
        {
            UserSession();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRAllowance", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
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
                BSM.ListHeader = new List<PREmpAllw>();

                if (dtHeader != null)
                {
                    for (int i = 0; i < dtHeader.Rows.Count; i++)
                    {
                        var objHeader = new PREmpAllw();
                        objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                        objHeader.AllwCode = dtHeader.Rows[i][1].ToString();
                        objHeader.FromDate = SYSettings.getDateValue(dtHeader.Rows[i][2].ToString());
                        objHeader.ToDate = SYSettings.getDateValue(dtHeader.Rows[i][3].ToString());
                        objHeader.Amount = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                        objHeader.Remark = dtHeader.Rows[i][5].ToString();
                        BSM.ListHeader.Add(objHeader);
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
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRAllowance", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("Allowance"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "PAYROLL";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);

            PRAllowance BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeader = new List<PREmpAllw>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }
        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "PRAllowance", SYSConstant.DEFAULT_UPLOAD_LIST);

            var BSM = new PRAllowance();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.ListHeader = new List<PREmpAllw>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView(SYListConfuration.ListDefaultUpload, BSM.ListTemplate);
        }
        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            HumicaDBContext DBB = new HumicaDBContext();
            if (obj != null)
            {
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
                        PRAllowance BSM = new PRAllowance();
                        BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);

                        string msg = SYConstant.OK;

                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {
                            BSM.ListHeader = new List<PREmpAllw>();

                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new PREmpAllw();
                                objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
                                objHeader.AllwCode = dtHeader.Rows[i][1].ToString();
                                objHeader.FromDate = SYSettings.getDateValue(dtHeader.Rows[i][2].ToString());
                                objHeader.ToDate = SYSettings.getDateValue(dtHeader.Rows[i][3].ToString());
                                objHeader.Amount = (decimal)SYSettings.getNumberValue(dtHeader.Rows[i][4].ToString());
                                objHeader.Remark = dtHeader.Rows[i][5].ToString();
                                if (objHeader.FromDate.Value.Date > objHeader.ToDate.Value.Date)
                                {
                                    obj.Message = SYMessages.getMessage("INV_DATE") + ' ' + objHeader.EmpCode;
                                    obj.Message += ":" + BSM.MessageError;
                                    obj.IsGenerate = false;
                                    DB.MDUploadTemplates.Attach(obj);
                                    DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                    DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                    DB.SaveChanges();
                                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                                }
                                else
                                {
                                    var Result = BSM.IsExistEmployeeAllowance(objHeader.EmpCode, objHeader.AllwCode, objHeader.FromDate.Value, objHeader.ToDate.Value);
                                    if (Result != -1)
                                    {
                                        obj.Message = SYMessages.getMessage("RECORD_EXIST") + ' ' + objHeader.EmpCode;
                                        obj.Message += ":" + BSM.MessageError;
                                        obj.IsGenerate = false;
                                        DB.MDUploadTemplates.Attach(obj);
                                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                        DB.SaveChanges();
                                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                                    }
                                }
                                BSM.ListHeader.Add(objHeader);
                            }

                            msg = BSM.ImportAllowance();
                            if (msg != SYConstant.OK)
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + BSM.MessageError;
                                obj.IsGenerate = false;
                                DB.MDUploadTemplates.Attach(obj);
                                DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DB.SaveChanges();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }
                            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("GENERATER_COMPLATED", user.Lang);
                            obj.DocumentNo = DocBatch.NextNumberRank;
                            obj.IsGenerate = true;
                            DBB.MDUploadTemplates.Attach(obj);
                            DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                            DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                            DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                            DBB.SaveChanges();
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
            string Allw = "ALLW";
            var ALLWCode = DB.PR_RewardsType.Where(w => w.ReCode == Allw).ToList();

            using (var workbook = new DevExpress.Spreadsheet.Workbook())
            {
                // Ensure sheet names are unique
                workbook.Worksheets[0].Name = "Master";
                List<ExCFUploadMapping> _ListMaster = new List<ExCFUploadMapping>();
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Employee Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Allowance Code" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "From Date\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "To Date\n(date)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Amount\n(number)" });
                _ListMaster.Add(new ExCFUploadMapping { FieldName = "Remark" });
                Worksheet worksheet = workbook.Worksheets[0];
                var sheet2 = workbook.Worksheets.Add("Allowance-Code");

                List<ExCFUploadMapping> _ListMaster1 = new List<ExCFUploadMapping>();
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });
                _ListMaster1.Add(new ExCFUploadMapping { Caption = "Remark", FieldName = "Remark" });

                List<ClsUploadMapping> _ListData = new List<ClsUploadMapping>();
                foreach (var read in ALLWCode)
                {
                    _ListData.Add(new ClsUploadMapping
                    {
                        FieldName = read.Code,
                        FieldName1 = read.Description,
                        FieldName2 = read.OthDesc
                    });
                }
                // Export data to each sheet with header formatting
                ClsConstant.ExportDataToWorksheet(worksheet, _ListMaster);
                ClsConstant.ExportDataToWorksheet(sheet2, _ListMaster1);
                ClsConstant.ExportDataToWorksheetRow(sheet2, _ListData);

                // Save the workbook to a memory stream
                using (var stream = new System.IO.MemoryStream())
                {
                    workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);

                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ALLOWANCE_TEMPLATE.xlsx");
                }
            }
            return null;
        }

        #endregion
        #region 'Code'
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
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

            PRAllowance BSM = new PRAllowance();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRAllowance)Session[Index_Sess_Obj + ActionName];

                BSM.HeaderStaff.EmpCode = EmpCode;

                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        private void DataSelector()
        {
            var Staff = DBV.HR_STAFF_VIEW.ToList();
            var _Branch = SYConstant.getBranchDataAccess();
            Staff = Staff.Where(x => _Branch.Where(w => w.Code == x.BranchID).Any()).ToList();

            ViewData["STAFF_SELECT"] = Staff;// DB.HRStaffProfiles.ToList();
            string ALLType = "ALLW";
            ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w => w.ReCode == ALLType).ToList();
        }
        #endregion
    }
}