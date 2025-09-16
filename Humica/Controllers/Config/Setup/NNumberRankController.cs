using Humica.Core.CF;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setupon.Setup
{
    public class NNumberRankController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "MDN00006";
        const string URL_SCREEN = "/Config/Setup/NNumberRank/";
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        private string ActionName = "";
        public NNumberRankController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            DataList();

            UserConf(ActionBehavior.EDIT);

            CFNumberRankObject BSM = new CFNumberRankObject();
            BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
            BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
            BSM.ListNumberSofComYear = SMS.BSNumberRankItems.ToList();
            //EmpCode
            BSM.ListNumEmpCode = DB.HREmpCodes.ToList();
            return View(BSM);
        }
        #endregion
        #region "Number Rank Heaer"
        public ActionResult GridItems1()
        {
            UserConf(ActionBehavior.EDIT);
            CFNumberRankObject BSM = new CFNumberRankObject();
            BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListNumberRankHeader);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(BSNumberRank ModelObject)
        {

            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {

                    SMS.BSNumberRanks.Add(ModelObject);
                    DataList();
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
            DataList();
            BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
            return PartialView("GridItems1", BSM.ListNumberRankHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(BSNumberRank ModelObject)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {

                    SMS.BSNumberRanks.Attach(ModelObject);
                    SMS.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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
            DataList();
            BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
            return PartialView("GridItems1", BSM.ListNumberRankHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(string NumberObject)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (NumberObject != null)
            {
                try
                {
                    var obj = SMS.BSNumberRanks.Find(NumberObject);
                    if (obj != null)
                    {

                        SMS.BSNumberRanks.Remove(obj);
                        int row = SMS.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
            return PartialView("GridItems1", BSM.ListNumberRankHeader);
        }
        #endregion
        #region "Number Rank Item"
        public ActionResult GridItems2()
        {
            UserConf(ActionBehavior.EDIT);
            CFNumberRankObject BSM = new CFNumberRankObject();
            BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
            DataList();
            return PartialView("GridItems2", BSM.ListNumberRankHofCom);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial2(BSNumberRankItemN ModelObject)
        {

            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var err = false;
                    if (ModelObject.FromNumber > ModelObject.ToNumber)
                    {
                        err = true;

                    }
                    UserSession();
                    if (ModelObject.Status > 0)
                    {
                        if (ModelObject.Status < ModelObject.FromNumber || ModelObject.Status > ModelObject.ToNumber)
                        {
                            err = true;
                        }
                    }

                    if (err == true)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("INV_NUMRANK", user.Lang);
                    }
                    else
                    {

                        SMS.BSNumberRankItemNs.Add(ModelObject);
                        DataList();
                        int row = SMS.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
            return PartialView("GridItems2", BSM.ListNumberRankHofCom);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial2(BSNumberRankItemN ModelObject)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {

                    SMS.BSNumberRankItemNs.Attach(ModelObject);
                    SMS.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = SMS.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
            return PartialView("GridItems2", BSM.ListNumberRankHofCom);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial2(int ID)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ID != 0)
            {
                try
                {
                    var obj = SMS.BSNumberRankItemNs.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objDel = obj.First();
                        SMS.BSNumberRankItemNs.Remove(objDel);
                        int row = SMS.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
            return PartialView("GridItems2", BSM.ListNumberRankHofCom);
        }
        #endregion
        #region "Number EmpCode"
        public ActionResult GridItems3()
        {
            UserConf(ActionBehavior.EDIT);
            CFNumberRankObject BSM = new CFNumberRankObject();
            BSM.ListNumEmpCode = DB.HREmpCodes.ToList();
            DataList();
            return PartialView("GridItems3", BSM.ListNumEmpCode);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial3(HREmpCode ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (!string.IsNullOrEmpty(ModelObject.NumberRank))
            {
                try
                {
                    var _Company = SMS.HRCompanies.FirstOrDefault(w => w.Company == ModelObject.Company);
                    ModelObject.Description = _Company.Description;
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == ModelObject.NumberRank);
                    ModelObject.NumberRankItem = Num.Length.ToString();
                    DB.HREmpCodes.Add(ModelObject);
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListNumEmpCode = DB.HREmpCodes.ToList();
            return PartialView("GridItems3", BSM.ListNumEmpCode);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial3(HREmpCode ModelObject)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (!string.IsNullOrEmpty(ModelObject.NumberRank))
            {
                try
                {
                    var _Company = SMS.HRCompanies.FirstOrDefault(w => w.Company == ModelObject.Company);
                    ModelObject.Description = _Company.Description;
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == ModelObject.NumberRank);
                    ModelObject.NumberRankItem = Num.Length.ToString();
                    DB.HREmpCodes.Attach(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListNumEmpCode = DB.HREmpCodes.ToList();
            return PartialView("GridItems3", BSM.ListNumEmpCode);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial3(int ID)
        {
            CFNumberRankObject BSM = new CFNumberRankObject();
            try
            {
                var obj = DB.HREmpCodes.Where(w => w.ID == ID).ToList();
                if (obj.Count > 0)
                {
                    var objDel = obj.First();
                    DB.HREmpCodes.Remove(objDel);
                    int row = DB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataList();
            BSM.ListNumEmpCode = DB.HREmpCodes.ToList();
            return PartialView("GridItems3", BSM.ListNumEmpCode);
        }
        #endregion

        #region "Number Rank By Compny & Year"
        public ActionResult GridItems5()
        {
            UserSession();
            UserConfListAndForm();
            UserConf(ActionBehavior.EDIT);
            CFNumberRankObject BSM = new CFNumberRankObject();
            BSM.ListNumberSofComYear = SMS.BSNumberRankItems.ToList();
            DataList();
            return PartialView("GridItems5", BSM.ListNumberSofComYear);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial5(BSNumberRankItem ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {
                    bool flag = false;
                    if (ModelObject.FromNumber > ModelObject.ToNumber)
                        flag = true;
                    UserSession();
                    if (ModelObject.Status > 0 && (ModelObject.Status < ModelObject.FromNumber || ModelObject.Status > ModelObject.ToNumber))
                        flag = true;
                    if (flag)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("INV_NUMRANK", user.Lang);
                    }
                    else
                    {
                        SMS.BSNumberRankItems.Add(ModelObject);
                        DataList();
                        SMS.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (DbUpdateException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
            }
            else
                ViewData["EditError"] = SYMessages.getMessage("EE005", user.Lang);
            DataList();
            BSM.ListNumberSofComYear = SMS.BSNumberRankItems.ToList();
            return PartialView("GridItems5", BSM.ListNumberSofComYear);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial5(BSNumberRankItem ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ModelState.IsValid)
            {
                try
                {
                    SMS.BSNumberRankItems.Attach(ModelObject);
                    SMS.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    SMS.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (DbUpdateException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
            }
            else
                ViewData["EditError"] = SYMessages.getMessage("EE005", user.Lang);
            DataList();
            BSM.ListNumberSofComYear = SMS.BSNumberRankItems.ToList();
            return PartialView("GridItems5", BSM.ListNumberSofComYear);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial5(int ID)
        {
            UserSession();
            UserConfListAndForm();
            CFNumberRankObject BSM = new CFNumberRankObject();
            if (ID != 0)
            {
                try
                {
                    List<BSNumberRankItem> list = SMS.BSNumberRankItems.Where(w => w.ID == ID).ToList();
                    if (list.Count > 0)
                    {
                        SMS.BSNumberRankItems.Remove(list.First());
                        SMS.SaveChanges();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (DbUpdateException ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
                catch (Exception ex)
                {
                    SYEventLogObject.saveEventLog(new SYEventLog()
                    {
                        ScreenId = SCREEN_ID,
                        UserId = user.UserName,
                        Action = SYActionBehavior.ADD.ToString()
                    }, ex, true);
                    ViewData["EditError"] = SYMessages.getMessage("EE001");
                }
            }
            DataList();
            BSM.ListNumberSofComYear = SMS.BSNumberRankItems.ToList();
            return PartialView("GridItems5", BSM.ListNumberSofComYear);
        }
        #endregion
        #region "Private Code"
        private void DataList()
        {
            var SMSB = new SMSystemEntity();
            UserSession();
            var listNumber = SMSB.BSNumberRanks.ToList();
            //foreach(var read in listNumber)
            //{
            //    read.Description = read.NumberObject + " : " + read.Description;
            //}
            ViewData["NUMBER_LIST"] = listNumber;
            //    var comList = SMSB.CFPlants.ToList();           
            ViewData["WORK_FLOW"] = SMS.CFWorkFlows.ToList();
            ViewData["TELEGRAM_SELECT"] = DB.TelegramBots.ToList();
            ViewData["COMPANY_SELECT"] = SMS.HRCompanies.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["GROUPDEPARTMENT_SELECT"] = DB.HRGroupDepartments.ToList().OrderBy(w => w.Description);
            ViewData["COM_LIST"] = SMS.HRCompanies.ToList();
        }
        #endregion
        #region "ExportTo"
        public ActionResult ExportTo(string id)
        {
            UserSession();
            if (id != null)
            {
                object obj = null;
                CFNumberRankObject BSM = new CFNumberRankObject();
                SYSGridSettingExport gSetting = new SYSGridSettingExport();
                gSetting.GridName = "ListConf";
                gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;

                if (id == "0")
                {
                    BSM.ListNumberRankHeader = SMS.BSNumberRanks.ToList();
                    if (BSM.ListNumberRankHeader.Count > 0)
                    {
                        obj = BSM.ListNumberRankHeader.First();
                    }
                    GridViewExportFormat format = GridViewExportFormat.Xlsx;
                    var dix = ClsInformation.GetDictionaryInfo(obj);
                    return SYExportFile.ExportFormatsInfo[format](SYExportFile.CreateExportGridViewSettings(gSetting, dix)
                        , BSM.ListNumberRankHeader);
                }

                if (id == "1")
                {
                    //Item
                    BSM.ListNumberRankHofCom = SMS.BSNumberRankItemNs.ToList();
                    if (BSM.ListNumberRankHofCom.Count > 0)
                    {
                        obj = BSM.ListNumberRankHofCom.First();
                    }
                    GridViewExportFormat format = GridViewExportFormat.Xlsx;
                    var dix = ClsInformation.GetDictionaryInfo(obj);
                    return SYExportFile.ExportFormatsInfo[format](SYExportFile.CreateExportGridViewSettings(gSetting, dix)
                        , BSM.ListNumberRankHofCom);
                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
    }
}
