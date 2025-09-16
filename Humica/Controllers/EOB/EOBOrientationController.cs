using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic.EOB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.EOB
{
    public class EOBOrientationController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000019";
        private const string URL_SCREEN = "/EOB/EOBOrientation";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public EOBOrientationController()
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
            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBOrienChList = DB.EOBOrienChLists.ToList();
            BSM.ListEOBOrienChListItem = DB.EOBOrienChListItems.ToList();
            BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.ToList();
            BSM.ListEOBOrienTypeDur = DB.EOBOrienTypeDurs.ToList();
            BSM.ListEOBOrienDesc = DB.EOBOrienDescs.ToList();

            return View(BSM);
        }
        #region 'Orientation Check List'
        public ActionResult GridItemOrienChList()
        {
            UserConf(ActionBehavior.EDIT);

            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBOrienChList = DB.EOBOrienChLists.ToList();
            return PartialView("GridItemOrienChList", BSM.ListEOBOrienChList);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOrienChList(EOBOrienChList MModel)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBOrienChLists.Add(MModel);
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
            BSM.ListEOBOrienChList = DB.EOBOrienChLists.ToList();
            return PartialView("GridItemOrienChList", BSM.ListEOBOrienChList);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOrienChList(EOBOrienChList MModel)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBOrienChLists.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.InOrder = MModel.InOrder;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBOrienChLists.Attach(ObjMatch);
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
            BSM.ListEOBOrienChList = DB.EOBOrienChLists.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienChList", BSM.ListEOBOrienChList);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOrienChList(string Code)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBOrienChLists.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBOrienChLists.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    BSM.ListEOBOrienChList = DB.EOBOrienChLists.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemOrienChList", BSM.ListEOBOrienChList);
        }
        #endregion 
        #region 'Orientation Check List Item'
        public ActionResult GridItemOrienChListItem()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBOrienChListItem = DB.EOBOrienChListItems.ToList();
            return PartialView("GridItemOrienChListItem", BSM.ListEOBOrienChListItem);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOrienChListItem(EOBOrienChListItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBOrienChListItems.Add(MModel);
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
            BSM.ListEOBOrienChListItem = DB.EOBOrienChListItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienChListItem", BSM.ListEOBOrienChListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOrienChListItem(EOBOrienChListItem MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBOrienChListItems.FirstOrDefault(w => w.Code == MModel.Code && w.LineItem == MModel.LineItem);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.IsActive = MModel.IsActive;

                    DB.EOBOrienChListItems.Attach(ObjMatch);

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
            BSM.ListEOBOrienChListItem = DB.EOBOrienChListItems.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienChListItem", BSM.ListEOBOrienChListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOrienChListItem(string Code, int LineItem)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBOrienChListItems.Find(Code, LineItem);
                    if (obj != null)
                    {
                        DB.EOBOrienChListItems.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListEOBOrienChListItem = DB.EOBOrienChListItems.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemOrienChListItem", BSM.ListEOBOrienChListItem);
        }
        #endregion 
        #region 'Typical Duration Type'
        public ActionResult GridItemTypiDurType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.ToList();
            return PartialView("GridItemTypiDurType", BSM.ListEOBTypiDurType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTypiDurType(EOBTypiDurType MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBTypiDurTypes.Add(MModel);
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
            BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemTypiDurType", BSM.ListEOBTypiDurType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTypiDurType(EOBTypiDurType MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBTypiDurTypes.FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.StartDate = MModel.StartDate;
                    ObjMatch.EndDate = MModel.EndDate;
                    ObjMatch.Duration = MModel.Duration;

                    DB.EOBTypiDurTypes.Attach(ObjMatch);

                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.StartDate).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.EndDate).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Duration).IsModified = true;

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
            BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemTypiDurType", BSM.ListEOBTypiDurType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTypiDurType(string Code)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBTypiDurTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBTypiDurTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemTypiDurType", BSM.ListEOBTypiDurType);
        }
        #endregion 
        #region 'Orientation Typical Duration'
        public ActionResult GridItemOrienTypeDur()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBOrienTypeDur = DB.EOBOrienTypeDurs.ToList();
            return PartialView("GridItemOrienTypeDur", BSM.ListEOBOrienTypeDur);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOrienTypeDur(EOBOrienTypeDur MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBOrienTypeDurs.Add(MModel);
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
            BSM.ListEOBOrienTypeDur = DB.EOBOrienTypeDurs.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienTypeDur", BSM.ListEOBOrienTypeDur);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOrienTypeDur(EOBOrienTypeDur MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBOrienTypeDurs.FirstOrDefault(w => w.Code == MModel.Code && w.OrienteerCode == MModel.OrienteerCode);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.Department = MModel.Department;
                    ObjMatch.Position = MModel.Position;
                    ObjMatch.Duration = MModel.Duration;
                    ObjMatch.Remark = MModel.Remark;

                    DB.EOBOrienTypeDurs.Attach(ObjMatch);

                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Department).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Position).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Duration).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Remark).IsModified = true;

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
            BSM.ListEOBOrienTypeDur = DB.EOBOrienTypeDurs.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienTypeDur", BSM.ListEOBOrienTypeDur);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOrienTypeDur(string Code)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBTypiDurTypes.Find(Code);
                    if (obj != null)
                    {
                        DB.EOBTypiDurTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListEOBTypiDurType = DB.EOBTypiDurTypes.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemOrienTypeDur", BSM.ListEOBTypiDurType);
        }
        #endregion 
        #region 'Orientation Description'
        public ActionResult GridItemOrienDesc()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            MDEOBOrientation BSM = new MDEOBOrientation();
            BSM.ListEOBOrienDesc = DB.EOBOrienDescs.ToList();
            return PartialView("GridItemOrienDesc", BSM.ListEOBOrienDesc);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOrienDesc(EOBOrienDesc MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.EOBOrienDescs.Add(MModel);
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
            BSM.ListEOBOrienDesc = DB.EOBOrienDescs.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienDesc", BSM.ListEOBOrienDesc);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOrienDesc(EOBOrienDesc MModel)
        {
            UserSession();
            DataSelector();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (ModelState.IsValid)
            {
                try
                {
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.EOBOrienDescs.FirstOrDefault(w => w.Code == MModel.Code && w.Department == MModel.Department && w.OrienteerCode == MModel.OrienteerCode);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.OrienteerName = MModel.OrienteerName;
                    ObjMatch.Position = MModel.Position;

                    DB.EOBOrienDescs.Attach(ObjMatch);

                    DB.Entry(ObjMatch).Property(x => x.Description).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.OrienteerName).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Position).IsModified = true;

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
            BSM.ListEOBOrienDesc = DB.EOBOrienDescs.OrderBy(w => w.Code).ToList();
            return PartialView("GridItemOrienDesc", BSM.ListEOBOrienDesc);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOrienDesc(string Code,string Dept, string OrienteerCode)
        {
            UserSession();
            MDEOBOrientation BSM = new MDEOBOrientation();
            if (Code != null)
            {
                try
                {
                    var obj = DB.EOBOrienDescs.Find(Code, Dept, OrienteerCode);
                    if (obj != null)
                    {
                        DB.EOBOrienDescs.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                    }
                    DataSelector();
                    BSM.ListEOBOrienDesc = DB.EOBOrienDescs.OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemOrienDesc", BSM.ListEOBOrienDesc);
        }
        #endregion 
        #region 'Private Code'
        private void DataSelector()
        {
            ViewData["CHKLST_SELECT"] = DB.EOBOrienChLists.Where(w => w.IsActive == true).ToList();
            ViewData["TYPIDURTYPE_SELECT"] = DB.EOBTypiDurTypes.ToList();
            ViewData["STAFF_SELECT"] = DBV.HR_STAFF_VIEW.Where(w => w.Status == "Active").ToList();
            ViewData["POSITION_SELECT"] = DB.HRPositions.ToList();
            ViewData["DEPARTMENT_SELECT"] = DB.HRDepartments.ToList();
        }
        #endregion
    }
}
