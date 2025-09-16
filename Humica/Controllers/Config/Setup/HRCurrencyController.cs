using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{
    public class HRCurrencyController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "CNF0000002";
        private const string URL_SCREEN = "/Config/Setup/HRCurrency/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();

        public HRCurrencyController()
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
            MDHRCurrency BSM = new MDHRCurrency();
            BSM.ListHeader = DB.HRCurrencies.ToList();
            return View(BSM);
        }

        #endregion

        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);

            MDHRCurrency BSM = new MDHRCurrency();
            BSM.ListHeader = DB.HRCurrencies.ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HRCurrency MModel)
        {
            UserSession();
            MDHRCurrency BSM = new MDHRCurrency();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.CurrencyCode = MModel.CurrencyCode.ToUpper();
                    DB.HRCurrencies.Add(MModel);
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
            BSM.ListHeader = DB.HRCurrencies.ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HRCurrency MModel)
        {
            UserSession();
            MDHRCurrency BSM = new MDHRCurrency();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    DB.HRCurrencies.Attach(MModel);
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
            BSM.ListHeader = DB.HRCurrencies.OrderBy(w => w.CurrencyCode).ToList();
            return PartialView("GridItems", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string Code)
        {
            UserSession();
            MDHRCurrency BSM = new MDHRCurrency();
            if (Code != null)
            {
                try
                {
                    //if (objEmp.Where(w => w.RateType == Code).Any())
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    //}
                    //else
                    //{
                    var obj = DB.HRCurrencies.Find(Code);
                    if (obj != null)
                    {
                        DB.HRCurrencies.Remove(obj);
                        int row = DB.SaveChanges();

                    }
                    BSM.ListHeader = DB.HRCurrencies.OrderBy(w => w.CurrencyCode).ToList();
                    //}
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItems", BSM.ListHeader);
        }
        private void DataSelector()
        {
            //SYDataList objList = new SYDataList("FAAVERAGE_SELECT");
            //ViewData["FAAVERAGE_SELECT"] = objList.ListData;
        }
    }
}
