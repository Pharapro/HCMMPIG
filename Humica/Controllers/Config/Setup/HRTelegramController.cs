using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.CF;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class HRTelegramController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "CNF0000003";
        private const string URL_SCREEN = "/Config/Setup/HRTelegram/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        public HRTelegramController()
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
            MDHRTelegram BSM = new MDHRTelegram();
            BSM.ListHeader = DB.TelegramBots.ToList();
            return View(BSM);
        }

        #endregion

        public ActionResult Gridview()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDHRTelegram BSM = new MDHRTelegram();
            BSM.ListHeader = DB.TelegramBots.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TelegramBot ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDHRTelegram BSM = new MDHRTelegram();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.TelegramBots.Add(ModelObject);
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
            BSM.ListHeader = DB.TelegramBots.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TelegramBot ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDHRTelegram BSM = new MDHRTelegram();
            if (ModelObject.ChatID != null)
            {
                try
                {
                    var DBM = new HumicaDBContext();
                    //DB.TelegramBots.Attach(ModelObject);
                    //DB.Entry(ModelObject).Property(w => w.Description).IsModified = true;
                    DBM.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBM.SaveChanges();
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

            BSM.ListHeader = DB.TelegramBots.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string ID)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDHRTelegram BSM = new MDHRTelegram();
            if (ID != null)
            {
                try
                {
                    int TokenID = Convert.ToInt32(ID);
                    var obj = DB.TelegramBots.FirstOrDefault(w => w.ID == TokenID);
                    if (obj != null)
                    {
                        DB.TelegramBots.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHeader = DB.TelegramBots.ToList();
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