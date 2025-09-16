using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRCostCenterTypeController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRS0000009";
        private const string URL_SCREEN = "/PR/PRS/PRCostCenterType";
        HumicaDBContext DB = new HumicaDBContext();

        public PRCostCenterTypeController()
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
            PRCostCenterObject BSM = new PRCostCenterObject();
            BSM.ListCosCenterType = DB.PRCostCenters.ToList();
            return View(BSM);
        }

        #endregion

        #region "CostCenter"
        public ActionResult GridItems()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            BSM.ListCosCenterType = DB.PRCostCenters.ToList();
            return PartialView("GridItems", BSM.ListCosCenterType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRCostCenter ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.PRCostCenters.Add(ModelObject);
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
            BSM.ListCosCenterType = DB.PRCostCenters.ToList();
            return PartialView("GridItems", BSM.ListCosCenterType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRCostCenter ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var objUpdate = DB.PRCostCenters.FirstOrDefault(w => w.ID == ModelObject.ID);
                    objUpdate.Description = ModelObject.Description;
                    DB.PRCostCenters.Attach(objUpdate);
                    DB.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
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

            BSM.ListCosCenterType = DB.PRCostCenters.ToList();
            return PartialView("GridItems", BSM.ListCosCenterType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int ID)
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            try
            {
                var obj = DB.PRCostCenters.FirstOrDefault(w => w.ID == ID);
                if (obj != null)
                {
                    DB.PRCostCenters.Remove(obj);
                    int row = DB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.ListCosCenterType = DB.PRCostCenters.ToList();
            return PartialView("GridItems", BSM.ListCosCenterType);
        }
        #endregion       

        private void DataSelector()
        {

        }
    }
}
