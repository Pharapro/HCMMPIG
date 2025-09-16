using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBPreboardingController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000018";
        private const string URL_SCREEN = "/EOB/EOBPreboarding";
        HumicaDBContext DB = new HumicaDBContext();
        public EOBPreboardingController()
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
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            BSM.ListPreBoardingStep = DB.EOBPreBoSteps.ToList();
            BSM.ListPreBoardingItem = DB.EOBPreBoItems.ToList();
            BSM.ListPreBoardingStage = DB.EOBPreBoStages.ToList();
            BSM.ListSocialMediaAcc = DB.EOBSocMedAccs.ToList();

            return View(BSM);
        }
        #region 'Pre Boarding Step'
        public ActionResult GridItemPreBoStep()
        {
            UserConf(ActionBehavior.EDIT);

            MDEOBPreboarding BSM = new MDEOBPreboarding();
            BSM.ListPreBoardingStep = DB.EOBPreBoSteps.ToList();
            return PartialView("GridItemPreBoStep", BSM.ListPreBoardingStep);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePreBoStep(EOBPreBoStep MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBPreBoSteps.Add(MModel);
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
            BSM.ListPreBoardingStep = DB.EOBPreBoSteps.ToList();
            return PartialView("GridItemPreBoStep", BSM.ListPreBoardingStep);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPreBoStep(EOBPreBoStep MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBPreBoSteps.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.InOrder = MModel.InOrder;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBPreBoSteps.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.InOrder).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.IsActive).IsModified = true;

                    DB.SaveChanges();
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
            BSM.ListPreBoardingStep = DB.EOBPreBoSteps.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemPreBoStep", BSM.ListPreBoardingStep);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePreBoStep(string Code)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBPreBoSteps.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBPreBoSteps.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    BSM.ListPreBoardingStep = DB.EOBPreBoSteps.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemPreBoStep", BSM.ListPreBoardingStep);
        }
        #endregion 
        #region 'Pre Boarding Item'
        public ActionResult GridItemPreBoItem()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBPreboarding BSM = new MDEOBPreboarding();
            BSM.ListPreBoardingItem = DB.EOBPreBoItems.ToList();
            return PartialView("GridItemPreBoItem", BSM.ListPreBoardingItem);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePreBoItem(EOBPreBoItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBPreBoItems.Add(MModel);
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
            BSM.ListPreBoardingItem = DB.EOBPreBoItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemPreBoItem", BSM.ListPreBoardingItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPreBoItem(EOBPreBoItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBPreBoItems.FirstOrDefault(w => w.Code == MModel.Code && w.LineItem == MModel.LineItem);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBPreBoItems.Attach(ObjMatch);

                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.IsActive).IsModified = true;

                    DB.SaveChanges();
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
            BSM.ListPreBoardingItem = DB.EOBPreBoItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemPreBoItem", BSM.ListPreBoardingItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePreBoItem(string Code, int LineItem)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBPreBoItems.Find(Code, LineItem);
                    if (obj != null)
                    {
                        DB.EOBPreBoItems.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListPreBoardingItem = DB.EOBPreBoItems.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemPreBoItem", BSM.ListPreBoardingItem);
        }
        #endregion 
        #region 'Pre Boarding Stage'
        public ActionResult GridItemPreBoStage()
        {
            UserConf(ActionBehavior.EDIT);

            MDEOBPreboarding BSM = new MDEOBPreboarding();
            BSM.ListPreBoardingStage = DB.EOBPreBoStages.ToList();
            return PartialView("GridItemPreBoStage", BSM.ListPreBoardingStage);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePreBoStage(EOBPreBoStage MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBPreBoStages.Add(MModel);
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
            BSM.ListPreBoardingStage = DB.EOBPreBoStages.ToList();
            return PartialView("GridItemPreBoStage", BSM.ListPreBoardingStage);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPreBoStage(EOBPreBoStage MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBPreBoStages.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Type = MModel.Type;
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBPreBoStages.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Type).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.IsActive).IsModified = true;

                    DB.SaveChanges();
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
            BSM.ListPreBoardingStage = DB.EOBPreBoStages.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemPreBoStage", BSM.ListPreBoardingStage);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePreBoStage(string Code)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBPreBoStages.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBPreBoStages.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    BSM.ListPreBoardingStage = DB.EOBPreBoStages.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemPreBoStage", BSM.ListPreBoardingStage);
        }
        #endregion 
        #region 'Social Media Account'
        public ActionResult GridItemSocMedAcc()
        {
            UserConf(ActionBehavior.EDIT);

            MDEOBPreboarding BSM = new MDEOBPreboarding();
            BSM.ListSocialMediaAcc = DB.EOBSocMedAccs.ToList();
            return PartialView("GridItemSocMedAcc", BSM.ListSocialMediaAcc);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateSocMedAcc(EOBSocMedAcc MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBSocMedAccs.Add(MModel);
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
            BSM.ListSocialMediaAcc = DB.EOBSocMedAccs.ToList();
            return PartialView("GridItemSocMedAcc", BSM.ListSocialMediaAcc);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSocMedAcc(EOBSocMedAcc MModel)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBSocMedAccs.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Type = MModel.Type;
                    ObjMatch.UrlLink = MModel.UrlLink;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBSocMedAccs.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(x => x.UrlLink).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Type).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.IsActive).IsModified = true;

                    DB.SaveChanges();
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
            BSM.ListSocialMediaAcc = DB.EOBSocMedAccs.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemSocMedAcc", BSM.ListSocialMediaAcc);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteSocMedAcc(string Code)
        {
            UserSession();
            MDEOBPreboarding BSM = new MDEOBPreboarding();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBSocMedAccs.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBSocMedAccs.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    BSM.ListSocialMediaAcc = DB.EOBSocMedAccs.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemSocMedAcc", BSM.ListSocialMediaAcc);
        }
        #endregion 
        #region 'Private Code'
        private void DataSelector()
        {
            ViewData["PREBOASTEP_SELECT"] = DB.EOBPreBoSteps.Where(w => w.IsActive == true).ToList();
        }
        #endregion 'Private Code' 
    }
}
