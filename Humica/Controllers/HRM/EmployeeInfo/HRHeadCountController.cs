using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.Att;
using Humica.Logic.HR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HRHeadCountController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000009";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HRHeadCount/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();

        public HRHeadCountController()
        : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region list filter
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            HRHeadCountObject BSM = new HRHeadCountObject();
            BSM.Header = new HRHeadCount();
            BSM.ListDayInMonth = new List<DayInMonth>();
            BSM.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRHeadCountObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(HRHeadCountObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            HRHeadCountObject BSM = new HRHeadCountObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (HRHeadCountObject)Session[Index_Sess_Obj + ActionName];
                if (obj.ToYear.ToString().Length == 4)
                {
                    collection.INYear = obj.ToYear;
                    BSM.ToYear = 0;
                }
            }
            BSM.INYear = collection.INYear;
            BSM.ListHeadCount = new List<HRHeadCount>();
            var chkYear = DB.HRHeadCounts.Where(w => w.INYear == BSM.INYear).ToList();
            if (BSM.ToYear == 0 || chkYear.Count == 0)
            {
                var _ListHeadCount = DB.HRHeadCounts.Where(w => w.INYear == BSM.INYear).ToList();
                var _ListDept = DB.HRDepartments.ToList();
                _ListDept = _ListDept.Where(w => !_ListHeadCount.Where(x => x.Code == w.Code).Any()).ToList();

                if (_ListDept.Count > 0)
                {
                    DB = new HumicaDBContext();
                    _ListHeadCount = new List<HRHeadCount>();
                    foreach (var read in _ListDept)
                    {
                        var Result = new HRHeadCount();
                        Result.Code = read.Code;
                        Result.Description = read.Description;
                        Result.EmpNo = 0;
                        Result.Amount = 0;
                        Result.INYear = BSM.INYear;
                        Result.CreatedBy = user.UserName;
                        Result.CreatedOn = DateTime.Now;
                        _ListHeadCount.Add(Result);
                        DB.HRHeadCounts.Add(Result);
                    }
                    int row = DB.SaveChanges();
                }

                BSM.ListHeadCount = _ListHeadCount.ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            else
            {
                var _ListHeadCount1 = DB.HRHeadCounts.Where(w => w.INYear == BSM.ToYear).ToList();
                BSM.ListHeadCount = _ListHeadCount1.ToList();
                BSM.INYear = BSM.ToYear;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
        }

        #endregion list
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            HRHeadCountObject BSM = new HRHeadCountObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (HRHeadCountObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ItemEdit(HRHeadCount MModel)
        {
            ActionName = "Index";
            UserSession();
            HRHeadCountObject BSM = new HRHeadCountObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (HRHeadCountObject)Session[Index_Sess_Obj + ActionName];
                    }
                    DB = new HumicaDBContext();
                    var ObjMatch = DB.HRHeadCounts.FirstOrDefault(w => w.Code == MModel.Code && w.INYear == BSM.INYear);
                    ObjMatch.EmpNo = MModel.EmpNo;
                    ObjMatch.Amount = MModel.Amount;
                    ObjMatch.ChangedBy = user.UserName;
                    ObjMatch.ChangedOn = DateTime.Now;

                    DB.HRHeadCounts.Attach(ObjMatch);
                    DB.Entry(ObjMatch).Property(x => x.EmpNo).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.Amount).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.ChangedBy).IsModified = true;
                    DB.Entry(ObjMatch).Property(x => x.ChangedOn).IsModified = true;

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
            BSM.ListHeadCount = DB.HRHeadCounts.Where(w => w.INYear == BSM.INYear).ToList();
            return PartialView("GridItems", BSM);
        }
        #endregion 'Grid'
        #region 'Copy'
        public ActionResult Copy(int FromYear, int ToYear, HRHeadCountObject collection)
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            var BSD = (HRHeadCountObject)Session[Index_Sess_Obj + ActionName];
            if (ToYear <= FromYear)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("HC_YEAR", user.Lang);
            }
            else
            {
                var _ListHCount = DB.HRHeadCounts.Where(w => w.INYear == ToYear).ToList();
                var _ListHCountF = DB.HRHeadCounts.Where(w => w.INYear == FromYear).ToList();
                collection.ListHeadCount = new List<HRHeadCount>();

                if (_ListHCount.Count > 0)
                {
                    foreach (var read in _ListHCount.ToList())
                    {
                        DB.HRHeadCounts.Remove(read);
                    }
                    var rw = DB.SaveChanges();
                    DB = new HumicaDBContext();
                    _ListHCount = new List<HRHeadCount>();
                    foreach (var read in _ListHCountF.ToList())
                    {
                        var Result = new HRHeadCount();
                        Result.Code = read.Code;
                        Result.INYear = ToYear;
                        Result.Description = read.Description;
                        Result.EmpNo = read.EmpNo;
                        Result.Amount = read.Amount;
                        Result.CreatedBy = user.UserName;
                        Result.CreatedOn = DateTime.Now;
                        _ListHCount.Add(Result);
                        DB.HRHeadCounts.Add(Result);
                    }
                    var row = DB.SaveChanges();
                }
                else
                {
                    DB = new HumicaDBContext();
                    foreach (var read in _ListHCountF.ToList())
                    {
                        var Result = new HRHeadCount();
                        Result.Code = read.Code;
                        Result.INYear = ToYear;
                        Result.Description = read.Description;
                        Result.EmpNo = read.EmpNo;
                        Result.Amount = read.Amount;
                        Result.CreatedBy = user.UserName;
                        Result.CreatedOn = DateTime.Now;
                        _ListHCount.Add(Result);
                        DB.HRHeadCounts.Add(Result);
                    }
                    var row = DB.SaveChanges();
                }
                BSD.ToYear = ToYear;
                BSD.ListHeadCount = _ListHCount.ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSD;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + BSD);
        }
        #endregion 'Copy'
        #region 'Private Code'
        private void DataSelector()
        {
            var results = DB.HRHeadCounts.GroupBy(n => new { n.INYear, }).Select(g => new { g.Key.INYear }).ToList();
            ViewData["YEAR_SELECT"] = results.ToList();
        }
        #endregion 'Private Code'
    }
}