using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class SYSocialMediaController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "CNF0000008";
        private const string URL_SCREEN = "/Config/Setup/SYSocialMedia/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        public SYSocialMediaController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }


        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            //MDSYSocialMedia BSM = new MDSYSocialMedia();
            MDSYSocialMedia BSM = new MDSYSocialMedia();
            BSM.ListHeader = DB.SYSocialMedias.ToList();
            return View(BSM);
        }

        #endregion

        public ActionResult Gridview()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSYSocialMedia BSM = new MDSYSocialMedia();
            BSM.ListHeader = DB.SYSocialMedias.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SYSocialMedia ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSYSocialMedia BSM = new MDSYSocialMedia();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SYSocialMedias.Add(ModelObject);
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
            BSM.ListHeader = DB.SYSocialMedias.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(SYSocialMedia ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSYSocialMedia BSM = new MDSYSocialMedia();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SYSocialMedias.Attach(ModelObject);
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

            BSM.ListHeader = DB.SYSocialMedias.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string ID)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSYSocialMedia BSM = new MDSYSocialMedia();
            if (ID != null)
            {
                try
                {
                    int TokenID = Convert.ToInt32(ID);
                    var obj = DB.SYSocialMedias.FirstOrDefault(w => w.ID == TokenID);
                    if (obj != null)
                    {
                        DB.SYSocialMedias.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHeader = DB.SYSocialMedias.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("Gridview", BSM.ListHeader);
        }

        private void DataSelector()
        {
        }
    }
}