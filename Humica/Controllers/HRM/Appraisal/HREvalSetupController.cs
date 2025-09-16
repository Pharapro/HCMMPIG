using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.HR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HREvalSetupController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000009";
        private const string URL_SCREEN = "/HRM/Appraisal/HREvalSetup";
        //private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        //private string ActionName;
        //private string KeyName = "Code";
        HumicaDBContext DH = new HumicaDBContext();
        public HREvalSetupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListEvalType = DH.HREvaluateTypes.ToList();
            BSM.ListRegion = DH.HREvaluateRegions.ToList();
            BSM.ListFactor = DH.HREvaluateFactors.ToList();
            BSM.ListEvaluateRating = DH.HREvaluateRatings.ToList();
            BSM.ListSelfEvaluateSetup = DH.HREvalSelfEvaluates.ToList();
            var d = DateTime.Now;
            var dd = d.TimeOfDay;
            return View(BSM);
        }

        #region Evaluation Types

        public ActionResult GridItemEvalTypes()
        {
            UserConf(ActionBehavior.EDIT);

            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListEvalType = DH.HREvaluateTypes.ToList();
            return PartialView("GridviewEvalType", BSM.ListEvalType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEvalType(HREvaluateType MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DH.HREvaluateTypes.Add(MModel);
                    int row = DH.SaveChanges();
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
            BSM.ListEvalType = DH.HREvaluateTypes.ToList();
            return PartialView("GridviewEvalType", BSM.ListEvalType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEvalType(HREvaluateType MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DHU = new HumicaDBContext();
                    var objUpdate = DHU.HREvaluateTypes.Find(MModel.Code);
                    DH.HREvaluateTypes.Attach(MModel);
                    DH.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DH.SaveChanges();
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
            BSM.ListEvalType = DH.HREvaluateTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewEvalType", BSM.ListEvalType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEvalType(string Code)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Code != null)
            {
                try
                {
                    var obj = DH.HREvaluateTypes.Find(Code);
                    if (obj != null)
                    {
                        DH.HREvaluateTypes.Remove(obj);
                        int row = DH.SaveChanges();
                    }
                    BSM.ListEvalType = DH.HREvaluateTypes.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewEvalType", BSM.ListEvalType);
        }
        #endregion

        #region Evaluation Region

        public ActionResult GridItemEvalRegion()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListRegion = DH.HREvaluateRegions.ToList();
            return PartialView("GridviewEvalRegion", BSM.ListRegion);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEvalRegion(HREvaluateRegion MModel)
        {
            UserSession();
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DH.HREvaluateRegions.Add(MModel);
                    int row = DH.SaveChanges();
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
            BSM.ListRegion = DH.HREvaluateRegions.ToList();
            return PartialView("GridviewEvalRegion", BSM.ListRegion);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEvalRegion(HREvaluateRegion MModel)
        {
            UserSession();
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DH = new HumicaDBContext();
                    var objUpdate = DH.HREvaluateRegions.FirstOrDefault(w => w.Code == MModel.Code);
                    objUpdate.Description = MModel.Description;
                    objUpdate.SecDescription = MModel.SecDescription;
                    objUpdate.InOrder = MModel.InOrder;
                    objUpdate.IsComment = MModel.IsComment;
                    objUpdate.IsQCM = MModel.IsQCM;
                    objUpdate.IsRating = MModel.IsRating;
                    objUpdate.Remark = MModel.Remark;
                    DH.HREvaluateRegions.Attach(objUpdate);
                    DH.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                    int row = DH.SaveChanges();
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
            BSM.ListRegion = DH.HREvaluateRegions.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewEvalRegion", BSM.ListRegion);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEvalRegion(string Code)
        {
            UserSession();
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Code != null)
            {
                try
                {
                    var obj = DH.HREvaluateRegions.FirstOrDefault(w => w.Code == Code);
                    if (obj != null)
                    {
                        DH.HREvaluateRegions.Remove(obj);
                        int row = DH.SaveChanges();
                    }
                    BSM.ListRegion = DH.HREvaluateRegions.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewEvalRegion", BSM.ListRegion);
        }

        #endregion

        #region Evaluation Factor

        public ActionResult GridItemEvalFactor()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListFactor = DH.HREvaluateFactors.ToList();
            return PartialView("GridviewEvalFactor", BSM.ListFactor);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEvalFactor(HREvaluateFactor MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper();
                    DH.HREvaluateFactors.Add(MModel);
                    int row = DH.SaveChanges();
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
            BSM.ListFactor = DH.HREvaluateFactors.ToList();
            DataSelector();
            return PartialView("GridviewEvalFactor", BSM.ListFactor);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEvalFactor(HREvaluateFactor MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DHU = new HumicaDBContext();
                    var objUpdate = DHU.HREvaluateFactors.Find(MModel.Code);
                    DH.HREvaluateFactors.Attach(MModel);
                    DH.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DH.SaveChanges();
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
            DataSelector();
            BSM.ListFactor = DH.HREvaluateFactors.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewEvalFactor", BSM.ListFactor);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEvalFactor(string Code)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (Code != null)
            {
                try
                {
                    var obj = DH.HREvaluateFactors.Find(Code);
                    if (obj != null)
                    {
                        DH.HREvaluateFactors.Remove(obj);
                        int row = DH.SaveChanges();
                    }
                    BSM.ListFactor = DH.HREvaluateFactors.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataSelector();
            return PartialView("GridviewEvalFactor", BSM.ListFactor);
        }

        #endregion

        #region Evaluation Rating

        public ActionResult GridItemEvalRating()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListEvaluateRating = DH.HREvaluateRatings.ToList();
            return PartialView("GridItemEvalRating", BSM.ListEvaluateRating);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEvalRating(HREvaluateRating MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            try
            {
                DH.HREvaluateRatings.Add(MModel);
                int row = DH.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.ListEvaluateRating = DH.HREvaluateRatings.ToList();
            DataSelector();
            return PartialView("GridItemEvalRating", BSM.ListEvaluateRating);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEvalRating(HREvaluateRating MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DHU = new HumicaDBContext();
                    var objUpdate = DHU.HREvaluateRatings.Find(MModel.ID);
                    DH.HREvaluateRatings.Attach(MModel);
                    DH.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DH.SaveChanges();
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
            DataSelector();
            BSM.ListEvaluateRating = DH.HREvaluateRatings.ToList();
            return PartialView("GridItemEvalRating", BSM.ListEvaluateRating);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEvalRating(int ID)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ID != null)
            {
                try
                {
                    var obj = DH.HREvaluateRatings.Find(ID);
                    if (obj != null)
                    {
                        DH.HREvaluateRatings.Remove(obj);
                        int row = DH.SaveChanges();
                    }
                    BSM.ListEvaluateRating = DH.HREvaluateRatings.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataSelector();
            return PartialView("GridItemEvalRating", BSM.ListEvaluateRating);
        }

        #endregion

        #region Self Evaluation

        public ActionResult GridItemEvalSelf()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            BSM.ListSelfEvaluateSetup = DH.HREvalSelfEvaluates.ToList();
            return PartialView("GridItemEvalSelf", BSM.ListSelfEvaluateSetup);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateEvalSelf(HREvalSelfEvaluate MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            try
            {
                DH.HREvalSelfEvaluates.Add(MModel);
                int row = DH.SaveChanges();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.ListSelfEvaluateSetup = DH.HREvalSelfEvaluates.ToList();
            DataSelector();
            return PartialView("GridItemEvalSelf", BSM.ListSelfEvaluateSetup);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditEvalSelf(HREvalSelfEvaluate MModel)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DHU = new HumicaDBContext();
                    var objUpdate = DHU.HREvalSelfEvaluates.Find(MModel.QuestionCode);
                    DH.HREvalSelfEvaluates.Attach(MModel);
                    DH.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DH.SaveChanges();
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
            DataSelector();
            BSM.ListSelfEvaluateSetup = DH.HREvalSelfEvaluates.ToList();
            return PartialView("GridItemEvalSelf", BSM.ListSelfEvaluateSetup);
        }
        // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteEvalSelf(string QuestionCode)
        {
            UserSession();
            HREmpEvaluateObject BSM = new HREmpEvaluateObject();
            if (QuestionCode != null)
            {
                try
                {
                    var obj = DH.HREvalSelfEvaluates.Find(QuestionCode);
                    if (obj != null)
                    {
                        DH.HREvalSelfEvaluates.Remove(obj);
                        int row = DH.SaveChanges();
                    }
                    BSM.ListSelfEvaluateSetup = DH.HREvalSelfEvaluates.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataSelector();
            return PartialView("GridItemEvalSelf", BSM.ListSelfEvaluateSetup);
        }

        #endregion

        private void DataSelector()
        {
            ViewData["REGION_SELECT"] = DH.HREvaluateRegions.ToList().OrderBy(w => w.Description);
            ViewData["EvaluateType_SELECT"] = DH.HREvaluateTypes.ToList();
        }
    }
}
