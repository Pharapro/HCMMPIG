using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRBenefitTypeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "PRS0000008";
        private const string URL_SCREEN = "/PR/PRS/PRBenefitType";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public PRBenefitTypeController()
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
            PRSBenefitType BSM = new PRSBenefitType();
            BSM.ListHeader = DB.PRBenefitTypes.ToList();
            BSM.ListInsuType = DB.HRInsuranceTypes.ToList();
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);
            DataList();
            PRSBenefitType BSM = new PRSBenefitType();
            BSM.ListHeader = DB.PRBenefitTypes.ToList().OrderBy(w => w.Code).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRBenefitType MModel)
        {
            UserSession();
            DataList();
            PRSBenefitType BSM = new PRSBenefitType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.PRBenefitTypes.Add(MModel);
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
            BSM.ListHeader = DB.PRBenefitTypes.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRBenefitType MModel)
        {
            UserSession();
            DataList();
            PRSBenefitType BSM = new PRSBenefitType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.PRBenefitTypes.Find(MModel.Code);
                    DB.PRBenefitTypes.Attach(MModel);
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
            BSM.ListHeader = DB.PRBenefitTypes.OrderBy(w => w.Code).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string Code)
        {
            UserSession();
            PRSBenefitType BSM = new PRSBenefitType();
            if (Code != null)
            {
                try
                {
                    var obj = DB.PRBenefitTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.PRBenefitTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.PRBenefitTypes.OrderBy(w => w.Code).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        #region InsuranceTypes

        public ActionResult GridviewInsuranceType()
        {
            UserConf(ActionBehavior.EDIT);

            PRSBenefitType BSM = new PRSBenefitType();
            BSM.ListInsuType = DB.HRInsuranceTypes.ToList();
            return PartialView("GridviewInsuranceType", BSM.ListInsuType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateInsuranceType(HRInsuranceType MModel)
        {
            UserSession();
            PRSBenefitType BSM = new PRSBenefitType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    MModel.LCK = false;
                    DB.HRInsuranceTypes.Add(MModel);
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
            BSM.ListInsuType = DB.HRInsuranceTypes.ToList();
            return PartialView("GridviewInsuranceType", BSM.ListInsuType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditInsuranceType(HRInsuranceType MModel)
        {
            UserSession();
            PRSBenefitType BSM = new PRSBenefitType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRInsuranceTypes.Find(MModel.Code);
                    DB.HRInsuranceTypes.Attach(MModel);
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
            BSM.ListInsuType = DB.HRInsuranceTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewInsuranceType", BSM.ListInsuType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteInsuranceType(string Code)
        {
            UserSession();
            PRSBenefitType BSM = new PRSBenefitType();
            if (Code != null)
            {
                try
                {
                    var obj = DB.HRCertificationTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.HRCertificationTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListInsuType = DB.HRInsuranceTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewInsuranceType", BSM.ListInsuType);
        }

        #endregion
        private void DataList()
        {
            //SYDataList objList = new SYDataList("FEE");
            //ViewData["FEE_SELECT"] = objList.ListData;
        }
    }
}
