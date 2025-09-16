using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Config.Setup
{
    public class UploadSetupController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRR000017";
        private const string URL_SCREEN = "/Configuration/Preference/UploadSetup/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string KeyName = "ID";
        private string GRID_EDITOR_ERROR = "EditError";
        private string ActionName = "";
        SMSystemEntity DB = new SMSystemEntity();

        public UploadSetupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Create";
            UserSession();
            UserConf(ActionBehavior.LIST);
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            //BSM.ListFileTemplate = new List<ExCfFileTemplate>();
            //BSM.ListMapping = new List<ExCFUploadMapping>();
            BSM.ListScreenDevelop = new List<SYScreenDevelop>();
            //BSM.ListValidation = new List<CfValidationField>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            DataList("", "");
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ExWorkFlowPreference BSM)
        {
            ActionName = "Create";
            UserSession();
            UserConf(ActionBehavior.LIST);
            //BSM.ListFileTemplate = new List<ExCfFileTemplate>();
            //BSM.ListMapping = new List<ExCFUploadMapping>();
            BSM.ListScreenDevelop = new List<SYScreenDevelop>();
            if (BSM.ScreenID != null)
            {
                BSM.ListScreenDevelop = DB.SYScreenDevelops.Where(w => w.ScreenId == BSM.ScreenID).ToList();
                //BSM.ListFileTemplate = DBX.ExCfFileTemplates.Where(w => w.ScreenId == BSM.ScreenID).OrderBy(w=>w.Version).ToList();
                //BSM.ListMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == BSM.ScreenID).OrderBy(w => w.Version).ThenBy(w => w.FiledIndex).ToList();

                //BSM.ListValidation = DB.CfValidationFields.Where(w => w.ScreenId == BSM.ScreenID).ToList();

                if (BSM.Table == null)
                {
                    BSM.Table = "";
                }

                ViewData[SYSConstant.PARAM_ID1] = BSM.ScreenID;
                ViewData[SYSConstant.PARAM_ID2] = BSM.Table;
                DataList(BSM.ScreenID, BSM.Table);
            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        #endregion

        //#region "List Field Mapping"
        //public ActionResult GridItems1(string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    DataList(screen, table);
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();


        //    BSM.ListMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == screen).OrderBy(w => w.Version).ThenBy(w => w.FiledIndex).ToList();
        //    return PartialView("GridItems1", BSM.ListMapping);
        //}

        ////create
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreatePartialField(ExCFUploadMapping ModelObject,string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    DataList(screen, table);
        //    ViewData[SYConstant.PARAM_ID] = screen;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new DBGenerateX();
        //        ModelObject.ScreenID = screen;
        //        ModelObject.TableName = table;
        //        DBA.ExCFUploadMappings.Add(ModelObject);
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == screen).OrderBy(w => w.Version).ThenBy(w => w.FiledIndex).ToList();
        //    return PartialView("GridItems1", BSM.ListMapping);
        //}

        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditPartialField(ExCFUploadMapping ModelObject,string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    DataList(screen, table);
        //    ViewData[SYConstant.PARAM_ID] = screen;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new DBGenerateX();
        //        ModelObject.ScreenID = screen;
        //        ModelObject.TableName = table;
        //        DBA.ExCFUploadMappings.Attach(ModelObject);
        //        DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.FieldName;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == screen).OrderBy(w => w.Version).ThenBy(w => w.FiledIndex).ToList();
        //    return PartialView("GridItems1", BSM.ListMapping);
        //}

        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeletePartialField(int id,string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    DataList(screen, table);            
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var obj = DBX.ExCFUploadMappings.Where(w => w.id == id).ToList();
        //        if (obj.Count>0)
        //        {
        //            DBX.Entry(obj.First()).State = System.Data.Entity.EntityState.Deleted;
        //            DBX.SaveChanges();
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = id.ToString();
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == screen).OrderBy(w=>w.Version).ThenBy(w => w.FiledIndex).ToList();
        //    return PartialView("GridItems1", BSM.ListMapping);
        //}



        //#endregion

        //#region "List File Upload"
        //public ActionResult GridItems2(string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();


        //    BSM.ListFileTemplate = DBX.ExCfFileTemplates.Where(w => w.ScreenId == screen).OrderBy(w => w.Version).ToList();
        //    return PartialView("GridItems2", BSM.ListFileTemplate);
        //}

        ////create
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreatePartialFile(ExCfFileTemplate ModelObject, string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen,table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new DBGenerateX();
        //        ModelObject.ScreenId = screen;
        //        DBA.ExCfFileTemplates.Add(ModelObject);
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListFileTemplate = DBX.ExCfFileTemplates.Where(w => w.ScreenId == screen).OrderBy(w => w.Version).ToList();
        //    return PartialView("GridItems2", BSM.ListFileTemplate);
        //}

        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditPartialFile(ExCfFileTemplate ModelObject, string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen, table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;

        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new DBGenerateX();
        //        ModelObject.ScreenId = screen;
        //        DBA.ExCfFileTemplates.Attach(ModelObject);
        //        DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListFileTemplate = DBX.ExCfFileTemplates.Where(w => w.ScreenId == screen).OrderBy(w => w.Version).ToList();
        //    return PartialView("GridItems2", BSM.ListFileTemplate);
        //}

        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeletePartialFile(int id, string screen,string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen, table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var obj = DBX.ExCfFileTemplates.Where(w => w.id == id).ToList();
        //        if (obj.Count > 0)
        //        {
        //            DBX.Entry(obj.First()).State = System.Data.Entity.EntityState.Deleted;
        //            DBX.SaveChanges();
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = id.ToString();
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListFileTemplate = DBX.ExCfFileTemplates.Where(w => w.ScreenId == screen).OrderBy(w=>w.Version).ToList();
        //    return PartialView("GridItems2", BSM.ListFileTemplate);
        //}



        //#endregion

        #region "List Screen Developer"

        //public ActionResult ReIndex(string screenid)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    var listMapping = DBX.ExCFUploadMappings.Where(w => w.ScreenID == screenid).OrderBy(w => w.Version).ToList();
        //    int index = 0;            
        //    foreach(var version in listMapping.GroupBy(w=>w.Version))
        //    {
        //        var DBS = new DBGenerateX();
        //        foreach (var read in version.OrderBy(w=>w.FiledIndex))
        //        {                    
        //            read.FiledIndex = index;
        //            index++;
        //            DBS.ExCFUploadMappings.Attach(read);
        //            DBS.Entry(read).Property(w => w.FiledIndex).IsModified = true;
        //        }
        //        DBS.SaveChanges();
        //    }

        //    var rs = new { MS = SYConstant.OK };
        //    return Json(rs, JsonRequestBehavior.DenyGet);
        //}

        public ActionResult GridItems3(string screen, string table)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList(screen, table);
            ViewData[SYSConstant.PARAM_ID1] = screen;
            ViewData[SYSConstant.PARAM_ID2] = table;
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();

            DataList(screen, "");
            BSM.ListScreenDevelop = DB.SYScreenDevelops.Where(w => w.ScreenId == screen).ToList();
            return PartialView("GridItems3", BSM.ListScreenDevelop);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartialDev(SYScreenDevelop ModelObject, string screen, string table)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList(screen, table);
            ViewData[SYSConstant.PARAM_ID1] = screen;
            ViewData[SYSConstant.PARAM_ID2] = table;
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new SMSystemEntity();
                ModelObject.ScreenId = screen;
                DBA.SYScreenDevelops.Add(ModelObject);
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListScreenDevelop = DB.SYScreenDevelops.Where(w => w.ScreenId == screen).ToList();
            return PartialView("GridItems3", BSM.ListScreenDevelop);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartialDev(SYScreenDevelop ModelObject, string screen, string table)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList(screen, table);
            ViewData[SYSConstant.PARAM_ID1] = screen;
            ViewData[SYSConstant.PARAM_ID2] = table;
            ViewData[SYConstant.PARAM_ID] = screen;
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new SMSystemEntity();
                DBA.SYScreenDevelops.Attach(ModelObject);
                DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.ScreenId;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListScreenDevelop = DB.SYScreenDevelops.Where(w => w.ScreenId == screen).ToList();
            return PartialView("GridItems3", BSM.ListScreenDevelop);
        }

        #endregion

        //#region "List Validation Field"
        //public ActionResult ValidationField(string screen, string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    DataList(screen, table);

        //    BSM.ListValidation = DB.CfValidationFields.Where(w => w.ScreenId == screen).ToList();
        //    return PartialView("_ValidationField", BSM.ListValidation);
        //}

        ////create
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreateValidationField(CfValidationField ModelObject, string screen, string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen, table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new SMMasterData();
        //        ModelObject.ScreenId = screen;
        //        ModelObject.ObjectName = table;
        //        DBA.CfValidationFields.Add(ModelObject);
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.ScreenId = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListValidation = DB.CfValidationFields.Where(w => w.ScreenId == screen).ToList();
        //    return PartialView("_ValidationField", BSM.ListValidation);
        //}

        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditValidationField(CfValidationField ModelObject, string screen, string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen, table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;

        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var DBA = new SMMasterData();
        //        ModelObject.ScreenId = screen;
        //        ModelObject.ObjectName = table;
        //        DBA.CfValidationFields.Attach(ModelObject);
        //        DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
        //        DBA.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }
        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = ModelObject.ScreenId;
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListValidation = DB.CfValidationFields.Where(w => w.ScreenId == screen).ToList();
        //    return PartialView("_ValidationField", BSM.ListValidation);
        //}

        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeleteValidationField(int id, string screen, string table)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    UserConfListAndForm();
        //    DataList(screen, table);
        //    ViewData[SYSConstant.PARAM_ID1] = screen;
        //    ViewData[SYSConstant.PARAM_ID2] = table;
        //    ExWorkFlowPreference BSM = new ExWorkFlowPreference();
        //    try
        //    {
        //        var obj = DB.CfValidationFields.Where(w => w.id == id).ToList();
        //        if (obj.Count > 0)
        //        {
        //            DB.Entry(obj.First()).State = System.Data.Entity.EntityState.Deleted;
        //            DB.SaveChanges();
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        /*------------------SaveLog----------------------------------*/
        //        SYEventLog log = new SYEventLog();
        //        log.ScreenId = SCREEN_ID;
        //        log.UserId = user.UserName;
        //        log.Action = id.ToString();
        //        log.Action = SYActionBehavior.ADD.ToString();

        //        SYEventLogObject.saveEventLog(log, e, true);
        //        /*----------------------------------------------------------*/
        //        ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
        //    }

        //    BSM.ListValidation = DB.CfValidationFields.Where(w => w.ScreenId == screen).ToList();
        //    return PartialView("_ValidationField", BSM.ListValidation);
        //}



        //#endregion

        //public ActionResult DownloadTemplate(string screen,int verion)
        //{
        //    UserSession();

        //    var objversion = DBX.ExCfFileTemplates.Find(screen, verion);
        //    if(objversion!=null)
        //    {
        //        SYSGridSettingExport gSetting = new SYSGridSettingExport();
        //        gSetting.GridName = "DownloadTemplate" + screen;  

        //        gSetting.CallBackControllerName = "";
        //        gSetting.CallBackActionName = "";
        //        gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
        //        gSetting.ListConfigure = SYSettings.getListConf(SCREEN_ID, user.Lang);
        //        GridViewExportFormat format = GridViewExportFormat.Xlsx;
        //        if (SYExportFile.ExportFormatsInfo.ContainsKey(format))
        //        {
        //            List<ExCfFileTemplate> DList = new List<ExCfFileTemplate>();

        //            return SYExportFile.ExportFormatsInfo[format](ClsConstant.CreateExportGridViewSettings(gSetting, screen, verion,objversion.Description),
        //                 DList);
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}

        #region "Private Code"


        private void DataList(string screen, string objName)
        {

            var DBA = new SMSystemEntity();

            ViewData["FIELD_LIST"] = DBA.SY_Schema.Where(w => w.TableName == objName).ToList();
            //ViewData["VERSION_LIST"] = DBX.ExCfFileTemplates.Where(w=>w.ScreenId==screen).ToList();
            ViewData["OBJECT_LIST"] = DB.SY_Table.ToList();
            ViewData["TABLE_LIST"] = DB.SY_Table.Where(w => w.TABLE_TYPE != "VIEW").ToList();
            ViewData["MESSAGE_RETURN"] = DBA.SYMessages.Where(w => w.Lang == user.Lang).ToList();
        }


        #endregion
    }


}
