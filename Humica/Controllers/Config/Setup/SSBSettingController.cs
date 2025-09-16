using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class SSBSettingController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRM0000002";
        private const string URL_SCREEN = "/Config/Setup/SSBSetting/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "SSBCD";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();

        public SSBSettingController()
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
            MDSSBSetting BSM = new MDSSBSetting();
            BSM.ListHeader = DB.SSBSettings.ToList();
            return View(BSM);
        }

        #endregion

        public ActionResult GridItems()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSSBSetting BSM = new MDSSBSetting();
            BSM.ListHeader = DB.SSBSettings.ToList();
            return PartialView("GridItems", BSM);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(SSBSetting ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSSBSetting BSM = new MDSSBSetting();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SSBSettings.Add(ModelObject);
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
            BSM.ListHeader = DB.SSBSettings.ToList();
            return PartialView("GridItems", BSM);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(SSBSetting ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSSBSetting BSM = new MDSSBSetting();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.SSBSettings.Attach(ModelObject);
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

            BSM.ListHeader = DB.SSBSettings.ToList();
            return PartialView("GridItems", BSM);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string SSBCD)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDSSBSetting BSM = new MDSSBSetting();
            if (SSBCD != null)
            {
                try
                {
                    var obj = DB.SSBSettings.FirstOrDefault(w => w.SSBCD == SSBCD);
                    if (obj != null)
                    {
                        DB.SSBSettings.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.SSBSettings.ToList();
            return PartialView("GridItems", BSM);
        }

        private void DataSelector()
        {
            SYDataList objList = new SYDataList("FAAVERAGE_SELECT");
            ViewData["FAAVERAGE_SELECT"] = objList.ListData;

            ViewData["BRANCH_LIST"] = SMS.HRBranches.ToList();
        }
    }
}
