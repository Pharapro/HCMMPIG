using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HRS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class HRMedicalTypeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000007";
        private const string URL_SCREEN = "/HRM/HRS/HRMedicalType";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public HRMedicalTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            MDMedicalType BSM = new MDMedicalType();
            BSM.ListHopital = DB.HRHospitals.ToList();
            BSM.ListMedicalType = DB.HRMedicalTypes.ToList();
            BSM.ListPHChkResult = DB.HRPHCKHResults.ToList();
            return View(BSM);
        }

        #region 'Hospital'

        public ActionResult GridItemHospital()
        {
            UserConf(ActionBehavior.EDIT);

            MDMedicalType BSM = new MDMedicalType();
            BSM.ListHopital = DB.HRHospitals.ToList();
            return PartialView("GridviewHospital", BSM.ListHopital);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateHost(HRHospital MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRHospitals.Add(MModel);
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
            BSM.ListHopital = DB.HRHospitals.ToList();
            return PartialView("GridviewHospital", BSM.ListHopital);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditHost(HRHospital MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRHospitals.Find(MModel.Code);
                    DB.HRHospitals.Attach(MModel);
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
            BSM.ListHopital = DB.HRHospitals.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewHospital", BSM.ListHopital);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteHost(string Code)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (Code != null)
            {
                try
                {
                    var obj = DB.HRHospitals.Find(Code);
                    if (obj != null)
                    {
                        DB.HRHospitals.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHopital = DB.HRHospitals.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridviewHospital", BSM.ListHopital);
        }

        #endregion

        #region 'Medical Type'

        public ActionResult GridItemMedicalType()
        {
            UserConf(ActionBehavior.EDIT);

            MDMedicalType BSM = new MDMedicalType();
            BSM.ListMedicalType = DB.HRMedicalTypes.ToList();
            return PartialView("GridviewMedicalType", BSM.ListMedicalType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMedicalType(HRMedicalType MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRMedicalTypes.Add(MModel);
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
            BSM.ListMedicalType = DB.HRMedicalTypes.ToList();
            return PartialView("GridviewMedicalType", BSM.ListMedicalType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMedicalType(HRMedicalType MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRMedicalTypes.Find(MModel.Code);
                    DB.HRMedicalTypes.Attach(MModel);
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
            BSM.ListMedicalType = DB.HRMedicalTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewMedicalType", BSM.ListMedicalType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMedicalType(string Code)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (Code != null)
            {
                try
                {
                    var obj = DB.HRMedicalTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.HRMedicalTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListMedicalType = DB.HRMedicalTypes.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridviewMedicalType", BSM.ListMedicalType);
        }

        #endregion

        #region 'Chk Result'

        public ActionResult GridItemChkResult()
        {
            UserConf(ActionBehavior.EDIT);

            MDMedicalType BSM = new MDMedicalType();
            BSM.ListPHChkResult = DB.HRPHCKHResults.ToList();
            return PartialView("GridviewChkResult", BSM.ListPHChkResult);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateChkResult(HRPHCKHResult MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DB.HRPHCKHResults.Add(MModel);
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
            BSM.ListPHChkResult = DB.HRPHCKHResults.ToList();
            return PartialView("GridviewChkResult", BSM.ListPHChkResult);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditChkResult(HRPHCKHResult MModel)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRPHCKHResults.Find(MModel.Code);
                    DB.HRPHCKHResults.Attach(MModel);
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
            BSM.ListPHChkResult = DB.HRPHCKHResults.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewChkResult", BSM.ListPHChkResult);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteChkResult(string Code)
        {
            UserSession();
            MDMedicalType BSM = new MDMedicalType();
            if (Code != null)
            {
                try
                {
                    var obj = DB.HRPHCKHResults.Find(Code);
                    if (obj != null)
                    {
                        DB.HRPHCKHResults.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListPHChkResult = DB.HRPHCKHResults.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridviewChkResult", BSM.ListPHChkResult);
        }

        #endregion

    }
}
