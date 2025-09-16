using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Developments
{
    public class SYDictionaryController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREEN_ID = "SYFRM00001";
        const string URL_SCREEN = "/Administrator/Developments/SYDictionary/";
        private string SUBMENU = SYTranslateType.SUBMENU.ToString();
        private string ACTION = SYTranslateType.ACTION.ToString();
        private SMSystemEntity DB = new SMSystemEntity();
        public const string MENU_L1 = "MODULEL1";
        public const string MENU_L2 = "MODULEL2";
        public SYDictionaryController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConf(ActionBehavior.LIST);

            SYLangObject BSM = new SYLangObject();
            BSM.ListForm = new List<CFForm>();
            BSM.ListTable = new List<CFList>();
            ViewData[SYConstant.PARAM_ID] = "";


            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(SYLangObject BSM)
        {
            UserSession();
            UserConf(ActionBehavior.LIST);

            if (BSM.ScreenID != null)
            {
                ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;

                //Add list
                AddList(BSM);
                //Add form
                AddForm(BSM);
                //add submenu
                AddSubMenu(BSM);
                //add action
                AddAction(BSM);
            }
            else
            {
                BSM.ListForm = new List<CFForm>();
                BSM.ListTable = new List<CFList>();
                BSM.ListSyLang = new List<SYLang>();
                ViewData[SYConstant.PARAM_ID] = "";
            }
            DataList(BSM.ScreenID);
            return View(BSM);
        }

        #endregion
        #region "List Translate"
        public ActionResult GridItems1(string screenId)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            DataList(screenId);
            return PartialView("GridItems1", BSM.ListTable);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFList Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    BSM.DB.CFLists.Add(Frm);
                    DataList();
                    int row = BSM.DB.SaveChanges();

                    updateSessionListLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);

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
            DataList(screenId);
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFList Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;

                    BSM.DB.CFLists.Attach(Frm);
                    BSM.DB.Entry(Frm).Property(w => w.ScreenId).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.FieldName).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Description).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.ViewAs).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Lang).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.InOrder).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Width).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.NavigationUrl).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.KeyValueField).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.IsInvisible).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.NavigationUrl).IsModified = true;
                    int row = BSM.DB.SaveChanges();

                    updateSessionListLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);
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
            DataList(screenId);
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(int ListId, string screenId)
        {
            UserSession();
            SYLangObject BSM = new SYLangObject();
            if (ListId >= 0)
            {
                try
                {
                    BSM.ScreenID = screenId;
                    ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                    var obj = BSM.DB.CFLists.Find(ListId);
                    if (obj != null)
                    {
                        var Frm = obj;

                        BSM.DB.CFLists.Remove(obj);
                        int row = BSM.DB.SaveChanges();

                        updateSessionListLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList(screenId);
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        #endregion
        #region "Form Translate"
        public ActionResult GridItems2(string screenId)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            DataList(screenId);
            return PartialView("GridItems2", BSM.ListForm);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial2(CFForm Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    BSM.DB.CFForms.Add(Frm);
                    DataList(screenId);
                    int row = BSM.DB.SaveChanges();

                    updateSessionFormLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);
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
            DataList(screenId);
            BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListForm);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial2(CFForm Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;

                    BSM.DB.CFForms.Attach(Frm);
                    BSM.DB.Entry(Frm).Property(w => w.ScreenId).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.FieldName).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Width).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.InOrder).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.KeyValueField).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.NavigationUrl).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Description).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.ViewAs).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Lang).IsModified = true;
                    int row = BSM.DB.SaveChanges();
                    updateSessionFormLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);
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
            DataList(screenId);
            BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListForm);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial2(int FormId, string screenId)
        {
            UserSession();
            SYLangObject BSM = new SYLangObject();
            if (FormId >= 0)
            {
                try
                {
                    BSM.ScreenID = screenId;
                    ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                    var obj = BSM.DB.CFForms.Find(FormId);
                    if (obj != null)
                    {
                        var Frm = obj;
                        BSM.DB.CFForms.Remove(obj);
                        int row = BSM.DB.SaveChanges();

                        updateSessionFormLang(Frm.ScreenId, Frm.FieldName, user.Lang, Frm.Description);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList(screenId);
            BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == screenId && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListForm);
        }
        #endregion
        #region "Menu Screen"
        public ActionResult GridItems3(string screenId)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();
            DataList();
            return PartialView("GridItems3", BSM.ListSyLang);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial3(SYLang Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    Frm.ObjectReference = SUBMENU;
                    BSM.DB.SYLangs.Add(Frm);
                    DataList();
                    int row = BSM.DB.SaveChanges();
                    updateSessionMenuLang(Frm.ObjectId, Frm.Lang, Frm.Description);
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
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();
            return PartialView("GridItems3", BSM.ListSyLang);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial3(SYLang Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    Frm.ObjectReference = SUBMENU;
                    BSM.DB.SYLangs.Attach(Frm);
                    BSM.DB.Entry(Frm).Property(w => w.Description).IsModified = true;
                    int row = BSM.DB.SaveChanges();
                    updateSessionMenuLang(Frm.ObjectId, Frm.Lang, Frm.Description);
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
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();
            return PartialView("GridItems3", BSM.ListSyLang);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial3(int ID, string screenId)
        {
            UserSession();
            SYLangObject BSM = new SYLangObject();
            if (ID >= 0)
            {
                try
                {
                    BSM.ScreenID = screenId;
                    ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                    var obj = BSM.DB.SYLangs.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var Frm = obj.First();
                        BSM.DB.SYLangs.Remove(obj.First());
                        int row = DB.SaveChanges();

                        updateSessionMenuLang(Frm.ObjectId, Frm.Lang, Frm.Description);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();
            return PartialView("GridItems3", BSM.ListSyLang);
        }
        #endregion
        #region "Action Screen"
        public ActionResult GridItems4(string screenId)
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();
            DataList();
            return PartialView("GridItems4", BSM.ListSyLang);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial4(SYLang Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    Frm.ObjectReference = ACTION;
                    BSM.DB.SYLangs.Add(Frm);
                    DataList();
                    int row = BSM.DB.SaveChanges();
                    updateSessionActionLang(Frm.ScreenId, Frm.ObjectId, Frm.Lang, Frm.Description);
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
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();
            return PartialView("GridItems4", BSM.ListSyLang);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial4(SYLang Frm, string screenId)
        {
            UserSession();
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    Frm.ObjectReference = ACTION;
                    BSM.DB.SYLangs.Attach(Frm);
                    BSM.DB.Entry(Frm).Property(w => w.Description).IsModified = true;
                    int row = BSM.DB.SaveChanges();

                    updateSessionActionLang(Frm.ScreenId, Frm.ObjectId, Frm.Lang, Frm.Description);
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
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();
            return PartialView("GridItems4", BSM.ListSyLang);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial4(int ID, string screenId)
        {
            UserSession();
            SYLangObject BSM = new SYLangObject();
            if (ID >= 0)
            {
                try
                {
                    BSM.ScreenID = screenId;
                    ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                    var obj = BSM.DB.SYLangs.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var Frm = obj.First();
                        BSM.DB.SYLangs.Remove(obj.First());
                        int row = DB.SaveChanges();
                        updateSessionActionLang(Frm.ScreenId, Frm.ObjectId, Frm.Lang, Frm.Description);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == screenId && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();
            return PartialView("GridItems4", BSM.ListSyLang);
        }
        #endregion
        #region "Private Code"
        private void DataList()
        {

            ViewData["LANG_LIST"] = DB.CFLangs.Where(w => w.Lang == user.Lang).ToList();

        }

        private void DataList(string ScreenID)
        {
            UserSession();
            ViewData["MODULE_LIST"] = new SYDataList("MODULE").ListData;
            ViewData["ITEM_REPORT_TYPE"] = new SYDataList("ITEM_REPORT_TYPE").ListData;
            var DBA = new SMSystemEntity();
            SYScreenDevelop sd = DBA.SYScreenDevelops.Find(ScreenID);
            if (sd != null)
            {
                var fieldList = DBA.SY_Schema.Where(w => w.TableName == sd.ListObject).ToList();
                ViewData["FIELD_LIST"] = fieldList;

                var formList = DBA.SY_Schema.Where(w => w.TableName == sd.FormObject).ToList();
                ViewData["FORM_LIST"] = formList;
            }
            ViewData["LANG_LIST"] = DB.CFLangs.Where(w => w.Lang == user.Lang).ToList();
        }

        public void updateSessionMenuLang(string objId, string lang, string text)
        {
            List<SYMenuItem> listMn = (List<SYMenuItem>)Session[SYConstant.MENU_SUB_HEADER_SESSION];
            int id = Convert.ToInt32(objId);
            if (lang == user.Lang)
            {
                if (listMn.Where(w => w.ID == id).ToList().Count > 0)
                {
                    listMn.Where(w => w.ID == id).First().Text = text;
                }
            }

        }


        public void updateSessionActionLang(string screenid, string objId, string lang, string text)
        {
            List<SYLang> listAC = (List<SYLang>)Session[SYConstant.SYSTEM_LANG];

            if (lang == user.Lang)
            {
                if (listAC == null)
                {
                    var obj = new SYLang();
                    obj.Description = text;
                    obj.Lang = lang;
                    obj.ScreenId = screenid;
                    obj.ObjectId = objId;
                    listAC = new List<SYLang>();
                    listAC.Add(obj);
                }
                else
                {
                    if (listAC.Where(w => w.ScreenId == screenid && w.ObjectId == objId && w.Lang == user.Lang).ToList().Count > 0)
                    {
                        listAC.Where(w => w.ScreenId == screenid && w.ObjectId == objId && w.Lang == user.Lang).First().Description = text;
                    }
                    else
                    {
                        var obj = new SYLang();
                        obj.Description = text;
                        obj.Lang = lang;
                        obj.ScreenId = screenid;
                        obj.ObjectId = objId;
                        listAC.Add(obj);
                    }
                }

            }

        }

        public void updateSessionListLang(string screenid, string FieldName, string lang, string text)
        {
            List<CFList> listAC = (List<CFList>)Session[SYSConstant.LIST_SESSION_STORE_CONF_LIST];
            if (lang == user.Lang)
            {
                if (listAC != null)
                {
                    if (listAC.Where(w => w.ScreenId == screenid && w.FieldName == FieldName && w.Lang == user.Lang).ToList().Count > 0)
                    {
                        listAC.Where(w => w.ScreenId == screenid && w.FieldName == FieldName && w.Lang == user.Lang).First().Description = text;
                    }
                }

            }

        }

        public void updateSessionFormLang(string screenid, string FieldName, string lang, string text)
        {
            List<CFForm> listAC = (List<CFForm>)Session[SYSConstant.LIST_SESSION_STORE_CONF_FORM];
            if (lang == user.Lang)
            {
                if (listAC != null)
                {
                    if (listAC.Where(w => w.ScreenId == screenid && w.FieldName == FieldName && w.Lang == user.Lang).ToList().Count > 0)
                    {
                        listAC.Where(w => w.ScreenId == screenid && w.FieldName == FieldName && w.Lang == user.Lang).First().Description = text;
                    }
                }

            }

        }


        private void AddSubMenu(SYLangObject BSM)
        {


            BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == BSM.ScreenID && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();

            if (BSM.ListSyLang.Count == 0)
            {
                var mn = BSM.DB.SYMenuItems.Where(w => w.ScreenId == BSM.ScreenID).ToList();
                if (mn.Count > 0)
                {
                    try
                    {
                        var DBA = new SMSystemEntity();
                        var obj = new SYLang();
                        obj.Lang = user.Lang;
                        obj.ScreenId = BSM.ScreenID;
                        obj.ObjectReference = SUBMENU;
                        obj.ObjectId = mn.First().ID.ToString();
                        obj.Description = mn.First().Text;
                        DBA.SYLangs.Add(obj);
                        //get Menu

                        DBA.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = SCREEN_ID;
                        log.Action = SYActionBehavior.ADD.ToString();
                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                    }
                    catch (DbUpdateException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = SCREEN_ID;
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/

                    }


                }
                BSM.ListSyLang = BSM.DB.SYLangs.Where(w => w.ScreenId == BSM.ScreenID && w.ObjectReference == SUBMENU && w.Lang == user.Lang).ToList();
            }
        }

        public void AddAction(SYLangObject BSM)
        {

            BSM.ListSyAction = BSM.DB.SYLangs.Where(w => w.ScreenId == BSM.ScreenID && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();

            if (BSM.ListSyAction.Count == 0)
            {
                try
                {
                    BSM.ListAction = BSM.DB.SYActionTemplates.Where(w => w.ScreenID == BSM.ScreenID).ToList();
                    var DBA = new SMSystemEntity();
                    foreach (var read in BSM.ListAction)
                    {

                        var obj = new SYLang();
                        obj.Lang = user.Lang;
                        obj.ScreenId = BSM.ScreenID;
                        obj.ObjectId = read.ActionName;
                        obj.ObjectReference = ACTION;
                        obj.Description = read.ActionName;
                        DBA.SYLangs.Add(obj);
                    }
                    DBA.SaveChanges();
                    BSM.ListSyAction = BSM.DB.SYLangs.Where(w => w.ScreenId == BSM.ScreenID && w.ObjectReference == ACTION && w.Lang == user.Lang).ToList();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();
                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/

                }
            }
        }

        public void AddList(SYLangObject BSM)
        {
            if (user.Lang == "EN")
            {
                BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
                if (BSM.ListTable.Count > 0)
                {
                    return;
                }
            }
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).ToList();

            var ListOrg = new List<CFList>();
            if (!(BSM.FromScreenID == null || BSM.FromScreenID == ""))
            {
                var listReference = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.FromScreenID && w.Lang == user.Lang).ToList();
                foreach (var read in listReference)
                {
                    read.ScreenId = BSM.ScreenID;
                    ListOrg.Add(read);
                }
            }
            else
            {
                ListOrg = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == "EN").ToList();
            }


            //for reference
            var ListReference = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.FromScreenID && w.Lang == user.Lang).ToList();

            if (ListOrg.Count > 0)
            {
                try
                {

                    var DBA = new SMSystemEntity();
                    foreach (var read in ListOrg)
                    {
                        var b = false;
                        foreach (var com in BSM.ListTable)
                        {
                            if (read.FieldName == com.FieldName)
                            {
                                b = true;
                            }
                        }

                        if (b == false)
                        {
                            var obj = read;
                            obj.Lang = user.Lang;
                            obj.ListId = 0;
                            DBA.CFLists.Add(obj);
                        }
                    }


                    DBA.SaveChanges();
                    BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).ToList();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();
                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/

                }
            }
        }

        public void AddForm(SYLangObject BSM)
        {
            if (user.Lang == "EN")
            {
                BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
                if (BSM.ListForm.Count > 0)
                {
                    return;
                }

            }
            BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).ToList();
            var ListOrg = new List<CFForm>();
            if (!(BSM.FromScreenID == null || BSM.FromScreenID == ""))
            {
                var listReference = BSM.DB.CFForms.Where(w => w.ScreenId == BSM.FromScreenID && w.Lang == user.Lang).ToList();
                foreach (var read in listReference)
                {
                    read.ScreenId = BSM.ScreenID;
                    ListOrg.Add(read);
                }
            }
            else
            {
                ListOrg = BSM.DB.CFForms.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == "EN").ToList();
            }
            if (ListOrg.Count > 0)
            {
                try
                {
                    var DBA = new SMSystemEntity();
                    foreach (var read in ListOrg)
                    {
                        var b = false;
                        foreach (var com in BSM.ListForm)
                        {
                            if (read.FieldName == com.FieldName)
                            {
                                b = true;
                            }
                        }
                        if (b == false)
                        {
                            var obj = read;
                            obj.Lang = user.Lang;
                            obj.FormId = 0;
                            DBA.CFForms.Add(obj);
                        }
                    }


                    DBA.SaveChanges();
                    BSM.ListForm = BSM.DB.CFForms.Where(w => w.ScreenId == BSM.ScreenID && w.Lang == user.Lang).OrderBy(w => w.InOrder).ToList();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();
                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = SCREEN_ID;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/

                }
            }
        }
        #endregion

    }
}
