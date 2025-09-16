using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Atts;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.AttSetup
{
    public class ATOTPolicyController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ATS0000004";
        private const string URL_SCREEN = "/Attendance/AttSetup/ATOTPolicy";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";

        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SM = new SMSystemEntity();
        public ATOTPolicyController()
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
            DataListPolicy();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            BSM.ListOTPolicy = DB.ATOTPolicies.ToList();
            BSM.ListLaEa = DB.ATLaEaPolicies.ToList();
            BSM.ListOTSetting = DB.ATOTSettings.ToList();
            BSM.ListPolicyLeEa = DB.ATPolicyLeEas.ToList();
            return View(BSM);
        }
        #region OT Setting
        public ActionResult GridItemOTSettings()
        {
            UserConf(ActionBehavior.EDIT);
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            BSM.ListOTSetting = DB.ATOTSettings.OrderBy(w => w.OTTYPE).ToList();
            return PartialView("GridItemOTSettings", BSM.ListOTSetting);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateOTSetting(ATOTSetting MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.StartTime.HasValue && MModel.EndTime.HasValue)
                    {
                        DateTime DateNow = DateTime.Now;
                        MModel.StartTime = DateNow.Date + MModel.StartTime.Value.TimeOfDay;
                        MModel.EndTime = DateNow.Date + MModel.EndTime.Value.TimeOfDay;
                        if (MModel.StartTime >= MModel.EndTime)
                        {
                            if (MModel.OverNight == true)
                            {
                                MModel.EndTime = MModel.EndTime.Value.AddDays(1);
                            }
                            else
                                ViewData["EditError"] = "INVALID TIMER";
                        }
                    }
                    DB.ATOTSettings.Add(MModel);
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
            BSM.ListOTSetting = DB.ATOTSettings.OrderBy(w => w.OTTYPE).ToList();
            return PartialView("GridItemOTSettings", BSM.ListOTSetting);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditOTSetting(ATOTSetting MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.ATOTSettings.Find(MModel.OTTYPE);
                    if (MModel.StartTime.HasValue && MModel.EndTime.HasValue)
                    {
                        DateTime DateNow = DateTime.Now;
                        MModel.StartTime = DateNow.Date + MModel.StartTime.Value.TimeOfDay;
                        MModel.EndTime = DateNow.Date + MModel.EndTime.Value.TimeOfDay;
                        if (MModel.StartTime >= MModel.EndTime)
                        {
                            if (MModel.OverNight == true)
                            {
                                MModel.EndTime = MModel.EndTime.Value.AddDays(1);
                            }
                            else
                                ViewData["EditError"] = "INVALID TIMER";
                        }
                    }
                    DB.ATOTSettings.Attach(MModel);
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
            BSM.ListOTSetting = DB.ATOTSettings.OrderBy(w => w.OTTYPE).ToList();
            return PartialView("GridItemOTSettings", BSM.ListOTSetting);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteOTSetting(string OTTYPE)
        {
            UserSession();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (!string.IsNullOrEmpty(OTTYPE))
            {
                try
                {
                    var obj = DB.ATOTSettings.Find(OTTYPE);
                    if (obj != null)
                    {
                        DB.ATOTSettings.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListOTSetting = DB.ATOTSettings.OrderBy(w => w.OTTYPE).ToList();
            return PartialView("GridItemOTSettings", BSM.ListOTSetting);
        }
        #endregion
        #region OT
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            BSM.ListOTPolicy = DB.ATOTPolicies.ToList().OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItems", BSM.ListOTPolicy);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ATOTPolicy MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.OTFrom > MModel.OTTo)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DOC_INV", user.Lang);
                    }
                    else
                    {
                        DB.ATOTPolicies.Add(MModel);
                        int row = DB.SaveChanges();
                    }
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
            BSM.ListOTPolicy = DB.ATOTPolicies.ToList();
            return PartialView("GridItems", BSM.ListOTPolicy);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(ATOTPolicy MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.OTFrom > MModel.OTTo)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DOC_INV", user.Lang);
                    }
                    else
                    {
                        var DBU = new HumicaDBContext();
                        var objUpdate = DBU.ATOTPolicies.Find(MModel.TranNo);
                        DB.ATOTPolicies.Attach(MModel);
                        DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
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
            BSM.ListOTPolicy = DB.ATOTPolicies.OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItems", BSM.ListOTPolicy);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int TranNo)
        {
            UserSession();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (TranNo != 0)
            {
                try
                {
                    var obj = DB.ATOTPolicies.Find(TranNo);
                    if (obj != null)
                    {
                        DB.ATOTPolicies.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListOTPolicy = DB.ATOTPolicies.OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItems", BSM.ListOTPolicy);
        }
        #endregion
        #region Late_Early_Condition
        public ActionResult GridItemLaEa()
        {
            UserConf(ActionBehavior.EDIT);
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            BSM.ListLaEa = DB.ATLaEaPolicies.ToList().OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItemLaEa", BSM.ListLaEa);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLaEa(ATLaEaPolicy MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.CreateOn = DateTime.Now;
                    MModel.CreatedBy = user.UserName;
                    if (MModel.OTFrom > MModel.OTTo)
                    {
                        ViewData["EditError"] = " Form(minute) can not greater or equal To(mintue)";
                    }
                    else if (IsOverlapAmount(MModel))
                        ViewData["EditError"] = "Overlap Amout.";
                    else
                    {
                        DB.ATLaEaPolicies.Add(MModel);
                        int row = DB.SaveChanges();
                    }
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
            BSM.ListLaEa = DB.ATLaEaPolicies.ToList();
            return PartialView("GridItemLaEa", BSM.ListLaEa);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditLaEa(ATLaEaPolicy MModel)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    //if (MModel.OTFrom > MModel.OTTo)
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DOC_INV", user.Lang);
                    //}
                    if (MModel.OTFrom >= MModel.OTTo)
                        ViewData["EditError"] = "Form (minute) can not greater or equal To (mintue)";
                    else if (IsOverlapAmount(MModel))
                        ViewData["EditError"] = "Overlap Amout.";
                    else
                    {
                        var DBU = new HumicaDBContext();
                        var obj = DBU.ATLaEaPolicies.Find(MModel.TranNo);
                        obj.DedType = MModel.DedType;
                        obj.OTFrom = MModel.OTFrom;
                        obj.OTTo = MModel.OTTo;
                        obj.Rate = MModel.Rate;
                        obj.ChangedBy = user.UserName;
                        obj.ChangedOn = DateTime.Today;
                        obj.RuleType = MModel.RuleType;
                        DB.ATLaEaPolicies.Attach(obj);
                        DB.Entry(obj).Property(w => w.DedType).IsModified = true;
                        DB.Entry(obj).Property(w => w.RuleType).IsModified = true;
                        DB.Entry(obj).Property(w => w.OTFrom).IsModified = true;
                        DB.Entry(obj).Property(w => w.OTTo).IsModified = true;
                        DB.Entry(obj).Property(w => w.Rate).IsModified = true;
                        DB.Entry(obj).Property(w => w.ChangedBy).IsModified = true;
                        DB.Entry(obj).Property(w => w.ChangedOn).IsModified = true;
                        int row = DB.SaveChanges();
                    }
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
            BSM.ListLaEa = DB.ATLaEaPolicies.OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItemLaEa", BSM.ListLaEa);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLaEa(int TranNo)
        {
            UserSession();
            DataList();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (TranNo != 0)
            {
                try
                {
                    var obj = DB.ATLaEaPolicies.Find(TranNo);
                    if (obj != null)
                    {
                        DB.ATLaEaPolicies.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListLaEa = DB.ATLaEaPolicies.OrderBy(w => w.TranNo).ToList();
            return PartialView("GridItemLaEa", BSM.ListLaEa);
        }
        //Add by Tey
        private bool IsOverlapAmount(ATLaEaPolicy ATLateEarly)
        {
            if (ATLateEarly.OTFrom == 0 || ATLateEarly.OTTo == 0) return false;


            var obj = DB.ATLaEaPolicies;
            int count = obj.Where(x => (x.OTFrom <= ATLateEarly.OTTo & x.OTTo >= ATLateEarly.OTFrom) &
            x.TranNo != ATLateEarly.TranNo & x.DedType == ATLateEarly.DedType).Count();
            return (count > 0) ? true : false;
        }
        #endregion

        #region PolicyLeEa
        public ActionResult GridItemPLeEas()
        {
            UserConf(ActionBehavior.EDIT);
            DataListPolicy();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            BSM.ListPolicyLeEa = DB.ATPolicyLeEas.ToList();
            return PartialView("GridItemPLeEas", BSM.ListPolicyLeEa);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePLaEa(ATPolicyLaEa MModel)
        {
            UserSession();
            DataListPolicy();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.ATPolicyLeEas.Add(MModel);
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
            BSM.ListPolicyLeEa = DB.ATPolicyLeEas.ToList();
            return PartialView("GridItemPLeEas", BSM.ListPolicyLeEa);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPLaEa(ATPolicyLaEa MModel)
        {
            UserSession();
            DataListPolicy();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.ATPolicyLeEas.Find(MModel.ID);
                    DB.ATPolicyLeEas.Attach(MModel);
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
            BSM.ListPolicyLeEa = DB.ATPolicyLeEas.ToList();
            return PartialView("GridItemPLeEas", BSM.ListPolicyLeEa);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePLaEa(int ID)
        {
            UserSession();
            ATOTPolicyObject BSM = new ATOTPolicyObject();
            try
            {
                var obj = DB.ATPolicyLeEas.Find(ID);
                if (obj != null)
                {
                    DB.ATPolicyLeEas.Remove(obj);
                    int row = DB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            BSM.ListPolicyLeEa = DB.ATPolicyLeEas.ToList();
            return PartialView("GridItemPLeEas", BSM.ListPolicyLeEa);
        }
        #endregion
        private void DataList()
        {
            SYDataList objList = new SYDataList("LAEA_TYPE");
            ViewData["LAEA_TYPE_SELECT"] = objList.ListData;
            var objList1 = new SYDataList("Request_Late_Early");
            ViewData["REQUEST_SELECT"] = objList1.ListData;
            ViewData["DEDUCTTYPE"] = new SYDataList("DEDUCTTYPE").ListData;
            ViewData["OTTYPE_SELECT"] = DB.PROTRates.ToList();
            ViewData["OTTYPE_SELECT"] = DB.PROTRates.ToList();
        }
        private void DataListPolicy()
        {
            ViewData["Branch_SELECT"] =  SM.HRBranches.ToList();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
        }
    }
}
