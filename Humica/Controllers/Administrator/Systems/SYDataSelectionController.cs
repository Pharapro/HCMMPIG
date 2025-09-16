using Humica.Core.CF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class SYDataSelectionController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYDT0001";
        private const string URL_SCREEN = "/Administrator/Systems/SYDataSelection/";
        private SMSystemEntity DBA = new SMSystemEntity();
        public SYDataSelectionController()
           : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            UserSession();


            UserConf(ActionBehavior.EDIT);

            CFSystem BSM = new CFSystem();
            BSM.ListDataSelection = DBA.SYDatas.ToList();
            BSM.ListSYSetting = DBA.SYSettings.ToList();
            DataList();
            return View(BSM);
        }
        #endregion
        #region "Data Selection"
        public ActionResult GridItems1()
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            BSM.ListDataSelection = DBA.SYDatas.OrderBy(w => w.ID).ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListDataSelection);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(SYData ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.SelectValue == null)
                    {
                        ModelObject.SelectValue = "";
                    }
                    DBA.SYDatas.Add(ModelObject);
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

            BSM.ListDataSelection = DBA.SYDatas.OrderBy(w => w.ID).ToList();
            return PartialView("GridItems1", BSM.ListDataSelection);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(SYData ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.SelectValue == null)
                    {
                        ModelObject.SelectValue = "";
                    }
                    DBA.SYDatas.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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
            BSM.ListDataSelection = DBA.SYDatas.OrderBy(w => w.ID).ToList();
            return PartialView("GridItems1", BSM.ListDataSelection);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(int ID)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {
                    var obj = DBA.SYDatas.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DBA.SYDatas.Remove(objAdd);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListDataSelection = DBA.SYDatas.OrderBy(w => w.ID).ToList();
            return PartialView("GridItems1", BSM.ListDataSelection);
        }
        #endregion

        #region "SySetting"
        public ActionResult GridItemsSetting()
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            BSM.ListSYSetting = DBA.SYSettings.ToList();
            return PartialView("GridItemsSetting", BSM.ListSYSetting);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartialSetting(SYSetting ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.SettinValue == null)
                    {
                        ModelObject.SettinValue = "";
                    }
                    DBA.SYSettings.Add(ModelObject);
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

            BSM.ListSYSetting = DBA.SYSettings.ToList();
            return PartialView("GridItemsSetting", BSM.ListSYSetting);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartialSetting(SYSetting ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.SettinValue == null)
                    {
                        ModelObject.SettinValue = "";
                    }
                    DBA.SYSettings.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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
            BSM.ListSYSetting = DBA.SYSettings.ToList();
            return PartialView("GridItemsSetting", BSM.ListSYSetting);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartialSetting(string SettingName)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (!string.IsNullOrEmpty(SettingName))
            {
                try
                {
                    var obj = DBA.SYSettings.Where(w => w.SettingName == SettingName).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DBA.SYSettings.Remove(objAdd);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListSYSetting = DBA.SYSettings.ToList();
            return PartialView("GridItemsSetting", BSM.ListSYSetting);
        }
        #endregion

        #region "Private Code"
        private void DataList()
        {
            UserSession();
        }
        #endregion
    }
}
