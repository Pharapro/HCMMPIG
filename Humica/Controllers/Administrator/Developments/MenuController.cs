using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Developments
{
    public class MenuController : Humica.EF.Controllers.MasterSaleController
    {
        SMSystemEntity DBA = new SMSystemEntity();
        HumicaDBContext DBX = new HumicaDBContext();
        const string SCREENID = "SYM000002";
        const string URL_Screen = "/Administrator/Developments/Menu/";
        private string PATH_FILE = "12313123123sadfsdfsdsfsdf212";
        public MenuController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_Screen;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConf(ActionBehavior.LISTR);
            SYMenuObject BSM = new SYMenuObject();
            BSM.ListMenu = DBA.SYMenus.ToList();
            BSM.ListMenuItem = DBA.SYMenuItems.ToList();
            return View(BSM);
        }

        #endregion
        #region "Menu"
        public ActionResult GridItems1()
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.LISTR);
            SYMenuObject BSM = new SYMenuObject();
            BSM.ListMenu = DBA.SYMenus.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenu", BSM.ListMenu);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMenu1(SYMenu MModel)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.ImageUrl = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    MModel.CreatedBy = user.UserName;
                    MModel.CreatedOn = DateTime.Now;
                    DBA.SYMenus.Add(MModel);

                    int row = DBA.SaveChanges();
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
            BSM.ListMenu = DBA.SYMenus.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenu", BSM.ListMenu);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMenu1(SYMenu MModel)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.ImageUrl = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        var obj = DBA.SYMenuItems.Find(MModel.ID);
                        if (obj != null)
                        {
                            MModel.ImageUrl = obj.ImageUrl;
                            DBA = new SMSystemEntity();
                        }
                    }
                    MModel.ChangedBy = user.UserName;
                    MModel.ChangedOn = DateTime.Now;
                    DBA.SYMenus.Attach(MModel);
                    DBA.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
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
            BSM.ListMenu = DBA.SYMenus.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenu", BSM.ListMenu);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMenu1(int ID)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DBA.SYMenus.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        DBA.SYMenus.Remove(obj.First());
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListMenu = DBA.SYMenus.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenu", BSM.ListMenu);
        }
        #endregion
        #region "Menu Item"
        public ActionResult GridMenuItems1()
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.LISTR);
            SYMenuObject BSM = new SYMenuObject();
            BSM.ListMenuItem = DBA.SYMenuItems.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenuItem", BSM.ListMenuItem);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMenuItem1(SYMenuItem MModel)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.ImageUrl = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    MModel.Parent = MModel.MenuId.ToString();
                    MModel.CreatedBy = user.UserName;

                    DBA.SYMenuItems.Add(MModel);

                    int row = DBA.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataList();
            BSM.ListMenuItem = DBA.SYMenuItems.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenuItem", BSM.ListMenuItem);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMenuItem1(SYMenuItem MModel)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[PATH_FILE] != null)
                    {
                        MModel.ImageUrl = Session[PATH_FILE].ToString();
                        Session[PATH_FILE] = null;
                    }
                    else
                    {
                        var obj = DBA.SYMenuItems.Find(MModel.ID);
                        if (obj != null)
                        {
                            MModel.ImageUrl = obj.ImageUrl;
                            DBA = new SMSystemEntity();
                        }
                    }


                    MModel.Parent = MModel.MenuId.ToString();
                    DBA.SYMenuItems.Attach(MModel);
                    DBA.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREENID;
                    log.UserId = user.UserName;
                    log.ScreenId = MModel.ScreenId;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            DataList();
            BSM.ListMenuItem = DBA.SYMenuItems.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenuItem", BSM.ListMenuItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMenuItem1(int ID)
        {
            UserSession();
            SYMenuObject BSM = new SYMenuObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DBA.SYMenuItems.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        DBA.SYMenuItems.Remove(obj.First());
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListMenuItem = DBA.SYMenuItems.OrderBy(w => w.ID).ToList();
            return PartialView("_GridMenuItem", BSM.ListMenuItem);
        }
        #endregion
        #region "Upload"
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DBX.CFUploadPaths.Find("IMG_ICON");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREENID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }

        public ActionResult UploadControlCallbackActionImage1()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DBX.CFUploadPaths.Find("IMG_ICON");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREENID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image1", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        #endregion
        #region "private code"
        private void DataList()
        {
            ViewData["LIST_MENU"] = DBA.SYMenus.Where(w => w.Segment == 1).ToList();
            ViewData["LIST_MENU_2"] = DBA.SYMenus.Where(w => w.Segment == 3).ToList();
            ViewData["LIST_MENU_ITEM"] = DBA.SYMenuItems.ToList();
        }
        #endregion

    }
}
