using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRFeeSettingController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "PRS0000007";
        private const string URL_SCREEN = "/PR/PRS/PRFeeSetting";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Levels";
        HumicaDBContext DB = new HumicaDBContext();
        public PRFeeSettingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataList();
            PRSFeeSetting BSM = new PRSFeeSetting();
            BSM.ListHeader = DB.PRFeeSettings.ToList();
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);
            DataList();
            PRSFeeSetting BSM = new PRSFeeSetting();
            BSM.ListHeader = DB.PRFeeSettings.ToList().OrderBy(w => w.Levels).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRFeeSetting MModel)
        {
            UserSession();
            DataList();
            PRSFeeSetting BSM = new PRSFeeSetting();
            if (ModelState.IsValid)
            {
                try
                {
                    //  MModel.Code = MModel.Code.ToUpper();
                    DB.PRFeeSettings.Add(MModel);
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
            BSM.ListHeader = DB.PRFeeSettings.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRFeeSetting MModel)
        {
            UserSession();
            DataList();
            PRSFeeSetting BSM = new PRSFeeSetting();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.PRFeeSettings.FirstOrDefault(w => w.Levels == MModel.Levels);
                    DB.PRFeeSettings.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
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
            BSM.ListHeader = DB.PRFeeSettings.OrderBy(w => w.Levels).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string Levels)
        {
            UserSession();
            PRSFeeSetting BSM = new PRSFeeSetting();
            if (Levels != null)
            {
                try
                {
                    var obj = DB.PRFeeSettings.FirstOrDefault(w => w.Levels == Levels);
                    if (obj != null)
                    {
                        DB.PRFeeSettings.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.PRFeeSettings.OrderBy(w => w.Levels).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        private void DataList()
        {
            SYDataList objList = new SYDataList("FEE");
            ViewData["FEE_SELECT"] = objList.ListData;
        }
    }
}
