using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRSProvidentFundController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "PRS0000006";
        private const string URL_SCREEN = "/PR/PRS/PRSProvidentFund";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        HumicaDBContext DB = new HumicaDBContext();

        public PRSProvidentFundController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            PRSProvidentFund BSM = new PRSProvidentFund();
            BSM.ListHeader = DB.PRProvidentFunds.ToList();
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);

            PRSProvidentFund BSM = new PRSProvidentFund();
            BSM.ListHeader = DB.PRProvidentFunds.ToList().OrderBy(w => w.YEAR).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRProvidentFund MModel)
        {
            UserSession();
            PRSProvidentFund BSM = new PRSProvidentFund();
            if (ModelState.IsValid)
            {
                try
                {
                    //  MModel.Code = MModel.Code.ToUpper();
                    DB.PRProvidentFunds.Add(MModel);
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
            BSM.ListHeader = DB.PRProvidentFunds.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRProvidentFund MModel)
        {
            UserSession();
            PRSProvidentFund BSM = new PRSProvidentFund();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.PRProvidentFunds.Attach(MModel);
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
            BSM.ListHeader = DB.PRProvidentFunds.OrderBy(w => w.YEAR).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int TranNo)
        {
            UserSession();
            PRSProvidentFund BSM = new PRSProvidentFund();
            if (TranNo != null)
            {
                try
                {
                    var obj = DB.PRProvidentFunds.Find(TranNo);
                    if (obj != null)
                    {
                        DB.PRProvidentFunds.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.PRProvidentFunds.OrderBy(w => w.YEAR).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
    }
}
