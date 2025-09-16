using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBCheckListController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000010";
        private const string URL_SCREEN = "/EOB/EOBCheckList";
        HumicaDBContext DB = new HumicaDBContext();
        public EOBCheckListController()
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
            MDEOBCheckList BSM = new MDEOBCheckList();
            BSM.ListOnBoardChkLst = DB.EOBSChkLsts.ToList();
            BSM.ListOnBoardChkLstItem = DB.EOBSChkLstItems.ToList();

            return View(BSM);
        }
        #region 'Check List'
        public ActionResult GridItemChkLst()
        {
            UserConf(ActionBehavior.EDIT);

            MDEOBCheckList BSM = new MDEOBCheckList();
            BSM.ListOnBoardChkLst = DB.EOBSChkLsts.ToList();
            return PartialView("GridItemChkLst", BSM.ListOnBoardChkLst);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateChkLst(EOBSChkLst MModel)
        {
            UserSession();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBSChkLsts.Add(MModel);
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
            BSM.ListOnBoardChkLst = DB.EOBSChkLsts.ToList();
            return PartialView("GridItemChkLst", BSM.ListOnBoardChkLst);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditChkLst(EOBSChkLst MModel)
        {
            UserSession();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBSChkLsts.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.InOrder = MModel.InOrder;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBSChkLsts.Attach(ObjMatch);
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
            BSM.ListOnBoardChkLst = DB.EOBSChkLsts.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemChkLst", BSM.ListOnBoardChkLst);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteChkLst(string Code)
        {
            UserSession();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBSChkLsts.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBSChkLsts.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    BSM.ListOnBoardChkLst = DB.EOBSChkLsts.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemChkLst", BSM.ListOnBoardChkLst);
        }
        #endregion 
        #region 'Check List'
        public ActionResult GridItemChkLstItem()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBCheckList BSM = new MDEOBCheckList();
            BSM.ListOnBoardChkLstItem = DB.EOBSChkLstItems.ToList();
            return PartialView("GridItemChkLstItem", BSM.ListOnBoardChkLstItem);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateChkLstItem(EOBSChkLstItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBSChkLstItems.Add(MModel);
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
            BSM.ListOnBoardChkLstItem = DB.EOBSChkLstItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemChkLstItem", BSM.ListOnBoardChkLstItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditChkLstItem(EOBSChkLstItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBSChkLstItems.FirstOrDefault(w => w.Code == MModel.Code && w.LineItem == MModel.LineItem);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBSChkLstItems.Attach(ObjMatch);

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
            BSM.ListOnBoardChkLstItem = DB.EOBSChkLstItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemChkLstItem", BSM.ListOnBoardChkLstItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteChkLstItem(string Code, int LineItem)
        {
            UserSession();
            MDEOBCheckList BSM = new MDEOBCheckList();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBSChkLstItems.Find(Code, LineItem);
                    if (obj != null)
                    {
                        DB.EOBSChkLstItems.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListOnBoardChkLstItem = DB.EOBSChkLstItems.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemChkLstItem", BSM.ListOnBoardChkLstItem);
        }
        #endregion 
        #region 'Private Code'
        private void DataSelector()
        {
            ViewData["CHKLST_SELECT"] = DB.EOBSChkLsts.Where(w => w.IsActive == true).ToList();
        }
        #endregion 'Private Code' 
    }
}
