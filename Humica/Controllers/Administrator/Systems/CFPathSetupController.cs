using Humica.Core.CF;
using Humica.Core.DB;
using Humica.EF.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class CFPathSetupController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYCFF001";
        private const string URL_SCREEN = "/Administrator/Systems/CFPathSetup/";
        private HumicaDBContext DB = new HumicaDBContext();
        public CFPathSetupController()
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
            BSM.ListUplaodPath = DB.CFUploadPaths.ToList();
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
            BSM.ListUplaodPath = DB.CFUploadPaths.ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListUplaodPath);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFUploadPath ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.CFUploadPaths.Add(ModelObject);
                    int row = DB.SaveChanges();
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

            BSM.ListUplaodPath = DB.CFUploadPaths.ToList();
            return PartialView("GridItems1", BSM.ListUplaodPath);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFUploadPath ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.CFUploadPaths.Attach(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            BSM.ListUplaodPath = DB.CFUploadPaths.ToList();
            return PartialView("GridItems1", BSM.ListUplaodPath);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(string PathCode)
        {
            UserSession();
            UserConfListAndForm();
            CFSystem BSM = new CFSystem();
            if (PathCode != null)
            {
                try
                {
                    var obj = DB.CFUploadPaths.Where(w => w.PathCode == PathCode).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DB.CFUploadPaths.Remove(objAdd);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListUplaodPath = DB.CFUploadPaths.ToList();
            return PartialView("GridItems1", BSM.ListUplaodPath);
        }
        #endregion

        #region "Private Code"
        private void DataList()
        {
        }
        #endregion
    }
}
