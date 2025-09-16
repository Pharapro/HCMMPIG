using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Training.DB;
using Humica.Training.Setup;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.SetUp
{
    public class TRQualificationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRS000001";
        private const string URL_SCREEN = "/Training/Setup/TRQualification/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public TRQualificationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            ClsQualification BSM = new ClsQualification();
            BSM.ListQualiClass = DBX.TRQualificatClass.ToList();
            BSM.ListQualiName = DBX.TRQualificatName.ToList();
            BSM.ListQualiType = DBX.TRQualificatType.ToList();
            return View(BSM);
        }

        #region Class
        public ActionResult GridItemClass()
        {
            UserConf(ActionBehavior.EDIT);

            ClsQualification BSM = new ClsQualification();
            BSM.ListQualiClass = DBX.TRQualificatClass.ToList();
            return PartialView("GridItemClass", BSM.ListQualiClass);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateClass(TRQualificatClass MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DBX.TRQualificatClass.Add(MModel);
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiClass = DBX.TRQualificatClass.ToList();
            return PartialView("GridItemClass", BSM.ListQualiClass);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditClass(TRQualificatClass MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContextTR();
                    var objUpdate = DBU.TRQualificatClass.Find(MModel.Code);
                    DBX.TRQualificatClass.Attach(MModel);
                    DBX.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiClass = DBX.TRQualificatClass.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemClass", BSM.ListQualiClass);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteClass(string Code)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (Code != null)
            {
                try
                {
                    var obj = DBX.TRQualificatClass.Find(Code);
                    if (obj != null)
                    {
                        DBX.TRQualificatClass.Remove(obj);
                        int row = DBX.SaveChanges();

                    }
                    BSM.ListQualiClass = DBX.TRQualificatClass.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemClass", BSM.ListQualiClass);
        }
        #endregion
        #region Name
        public ActionResult GridItemName()
        {
            UserConf(ActionBehavior.EDIT);

            ClsQualification BSM = new ClsQualification();
            BSM.ListQualiName = DBX.TRQualificatName.ToList();
            return PartialView("GridItemName", BSM.ListQualiName);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateName(TRQualificatName MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DBX.TRQualificatName.Add(MModel);
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiName = DBX.TRQualificatName.ToList();
            return PartialView("GridItemName", BSM.ListQualiName);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditName(TRQualificatName MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContextTR();
                    var objUpdate = DBU.TRQualificatName.Find(MModel.Code);
                    DBX.TRQualificatName.Attach(MModel);
                    DBX.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiName = DBX.TRQualificatName.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemName", BSM.ListQualiName);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteName(string Code)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (Code != null)
            {
                try
                {
                    var obj = DBX.TRQualificatName.Find(Code);
                    if (obj != null)
                    {
                        DBX.TRQualificatName.Remove(obj);
                        int row = DBX.SaveChanges();

                    }
                    BSM.ListQualiName = DBX.TRQualificatName.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemName", BSM.ListQualiName);
        }
        #endregion
        #region Type
        public ActionResult GridItemType()
        {
            UserConf(ActionBehavior.EDIT);

            ClsQualification BSM = new ClsQualification();
            BSM.ListQualiType = DBX.TRQualificatType.ToList();
            return PartialView("GridItemType", BSM.ListQualiType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateType(TRQualificatType MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DBX.TRQualificatType.Add(MModel);
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiType = DBX.TRQualificatType.ToList();
            return PartialView("GridItemType", BSM.ListQualiType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditType(TRQualificatType MModel)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContextTR();
                    var objUpdate = DBU.TRQualificatType.Find(MModel.Code);
                    DBX.TRQualificatType.Attach(MModel);
                    DBX.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DBX.SaveChanges();
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
            BSM.ListQualiType = DBX.TRQualificatType.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemType", BSM.ListQualiType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteType(string Code)
        {
            UserSession();
            ClsQualification BSM = new ClsQualification();
            if (Code != null)
            {
                try
                {
                    var obj = DBX.TRQualificatType.Find(Code);
                    if (obj != null)
                    {
                        DBX.TRQualificatType.Remove(obj);
                        int row = DBX.SaveChanges();

                    }
                    BSM.ListQualiType = DBX.TRQualificatType.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemType", BSM.ListQualiType);
        }
        #endregion
    }

}