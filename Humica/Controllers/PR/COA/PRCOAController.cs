using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRS
{
    public class PRCOAController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000015";
        private const string URL_SCREEN = "/PR/PRM/PRCOA/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public PRCOAController()
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
            PRCOAObject BSM = new PRCOAObject();
            BSM.ListHeader = DB.PRChartofAccounts.ToList();
            BSM.ListJournalType = DB.HRJournalTypes.ToList();
            BSM.ListMaterailMaster = DB.HRMaterialAccounts.ToList();
            BSM.ListCFExVendor = DB.CFExVendors.ToList();
            BSM.ListSetting = DB.SYSHRBuiltinAccs.ToList();
            return View(BSM);
        }

        #region COA
        public ActionResult GridItems()
        {
            UserConf(ActionBehavior.EDIT);

            PRCOAObject BSM = new PRCOAObject();
            BSM.ListHeader = DB.PRChartofAccounts.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PRChartofAccount MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var objUpdate = DB.PRChartofAccounts.Where(w => w.Code == MModel.Code).ToList();
                    if (objUpdate.Count > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST"); ;
                    }
                    else
                    {
                        MModel.Code = MModel.Code.ToUpper();
                        //MModel.CreateBy = User.UserName;
                        MModel.CreateOn = DateTime.Now;
                        DB.PRChartofAccounts.Add(MModel);
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
            BSM.ListHeader = DB.PRChartofAccounts.ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(PRChartofAccount MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    //var objUpdate = DBU.PRChartofAccounts.Find(MModel.Code);
                    var objUpdate = DB.PRChartofAccounts.FirstOrDefault(w => w.Code == MModel.Code);
                    if (objUpdate != null)
                    {
                        objUpdate.Description = MModel.Description;
                        objUpdate.OthDesc = MModel.OthDesc;
                        objUpdate.DC = MModel.DC;
                        //objUpdate.ChangedBy = User.UserName;
                        objUpdate.ChangedOn = DateTime.Now;
                        DB.PRChartofAccounts.Attach(objUpdate);
                        DB.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            BSM.ListHeader = DB.PRChartofAccounts.OrderBy(w => w.Code).ToList();
            return PartialView("Gridview", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string Code)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (Code != null)
            {
                try
                {

                    var obj = DB.PRChartofAccounts.Find(Code);
                    if (obj != null)
                    {
                        DB.PRChartofAccounts.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListHeader = DB.PRChartofAccounts.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("Gridview", BSM.ListHeader);
        }
        #endregion
        #region JournalType
        public ActionResult GridJournalType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            BSM.ListJournalType = DB.HRJournalTypes.ToList();
            return PartialView("GridJournalType", BSM.ListJournalType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateJournalType(HRJournalType MModel)
        {
            UserSession();
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == MModel.NumberRank);
                    MModel.NumberRankItem = Num.Length.ToString();
                    var objUpdate = DB.HRJournalTypes.Where(w => w.JournalType == MModel.JournalType).ToList();
                    if (objUpdate.Count > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST"); ;
                    }
                    else
                    {
                        MModel.JournalType = MModel.JournalType.ToUpper();
                        DB.HRJournalTypes.Add(MModel);
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
            BSM.ListJournalType = DB.HRJournalTypes.ToList();
            return PartialView("GridJournalType", BSM.ListJournalType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditJournalType(HRJournalType MModel)
        {
            UserSession();
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == MModel.NumberRank);
                    MModel.NumberRankItem = Num.Length.ToString();
                    MModel.Description = MModel.Description;
                    DB.HRJournalTypes.Attach(MModel);
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
            BSM.ListJournalType = DB.HRJournalTypes.ToList();
            return PartialView("GridJournalType", BSM.ListJournalType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteJournalType(string JournalType)
        {
            UserSession();
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            if (JournalType != null)
            {
                try
                {

                    var obj = DB.HRJournalTypes.Find(JournalType);
                    if (obj != null)
                    {
                        DB.HRJournalTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListJournalType = DB.HRJournalTypes.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridJournalType", BSM.ListJournalType);
        }
        #endregion
        #region MaterailMaster
        public ActionResult GridMaterailMaster()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            BSM.ListMaterailMaster = DB.HRMaterialAccounts.ToList();
            return PartialView("GridMaterailMaster", BSM.ListMaterailMaster);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMaterial(HRMaterialAccount MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var objUpdate = DB.HRMaterialAccounts.Where(w => w.MaterialCode == MModel.MaterialCode).ToList();
                    if (objUpdate.Count > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST"); ;
                    }
                    else
                    {
                        MModel.MaterialCode = MModel.MaterialCode;
                        MModel.MaterialDescription = MModel.MaterialDescription;
                        MModel.MaterialDescription2 = MModel.MaterialDescription2;
                        MModel.ExpenseAccount = MModel.ExpenseAccount;
                        DB.HRMaterialAccounts.Add(MModel);
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
            BSM.ListMaterailMaster = DB.HRMaterialAccounts.ToList();
            return PartialView("GridMaterailMaster", BSM.ListMaterailMaster);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMaterail(HRMaterialAccount MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DB.HRMaterialAccounts.FirstOrDefault(w => w.MaterialCode == MModel.MaterialCode);
                    if (objUpdate != null)
                    {
                        objUpdate.MaterialDescription = MModel.MaterialDescription;
                        objUpdate.MaterialDescription2 = MModel.MaterialDescription2;
                        objUpdate.ExpenseAccount = MModel.ExpenseAccount;
                        DB.HRMaterialAccounts.Attach(objUpdate);
                        DB.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            BSM.ListMaterailMaster = DB.HRMaterialAccounts.OrderBy(w => w.MaterialCode).ToList();
            return PartialView("GridMaterailMaster", BSM.ListMaterailMaster);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMaterial(string MaterialCode)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (MaterialCode != null)
            {
                try
                {

                    var obj = DB.HRMaterialAccounts.Find(MaterialCode);
                    if (obj != null)
                    {
                        DB.HRMaterialAccounts.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListMaterailMaster = DB.HRMaterialAccounts.OrderBy(w => w.MaterialCode).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridMaterailMaster", BSM.ListMaterailMaster);
        }
        #endregion
        #region EXVendor
        public ActionResult GridVendor()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            BSM.ListCFExVendor = DB.CFExVendors.ToList();
            return PartialView("GridVendor", BSM.ListCFExVendor);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateVendor(CFExVendor MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var objUpdate = DB.CFExVendors.Where(w => w.Code == MModel.Code).ToList();
                    if (objUpdate.Count > 0)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST"); ;
                    }
                    else
                    {
                        MModel.Code = MModel.Code;
                        MModel.Description = MModel.Description;
                        MModel.Branch = MModel.Branch;
                        DB.CFExVendors.Add(MModel);
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
            BSM.ListCFExVendor = DB.CFExVendors.ToList();
            return PartialView("GridVendor", BSM.ListCFExVendor);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditVendor(CFExVendor MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DB.CFExVendors.FirstOrDefault(w => w.Code == MModel.Code);
                    if (objUpdate != null)
                    {
                        objUpdate.Description = MModel.Description;
                        objUpdate.Branch = MModel.Branch;
                        DB.CFExVendors.Attach(objUpdate);
                        DB.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            BSM.ListCFExVendor = DB.CFExVendors.OrderBy(w => w.Code).ToList();
            return PartialView("GridVendor", BSM.ListCFExVendor);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteVendor(string Code)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (Code != null)
            {
                try
                {

                    var obj = DB.CFExVendors.Find(Code);
                    if (obj != null)
                    {
                        DB.CFExVendors.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListCFExVendor = DB.CFExVendors.OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridVendor", BSM.ListCFExVendor);
        }
        #endregion
        #region Setting
        public ActionResult GridSetting()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            PRCOAObject BSM = new PRCOAObject();
            BSM.ListSetting = DB.SYSHRBuiltinAccs.ToList();
            return PartialView("GridviewSetting", BSM.ListSetting);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditSetting(SYSHRBuiltinAcc MModel)
        {
            UserSession();
            PRCOAObject BSM = new PRCOAObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DB.SYSHRBuiltinAccs.FirstOrDefault(w => w.Code == MModel.Code);
                    if (objUpdate != null)
                    {
                        objUpdate.GroupAcc = MModel.GroupAcc;
                        objUpdate.Description = MModel.Description;
                        objUpdate.ObjectName = MModel.ObjectName;
                        objUpdate.FieldName = MModel.FieldName;
                        objUpdate.IsCredit = MModel.IsCredit;
                        DB.SYSHRBuiltinAccs.Attach(objUpdate);
                        DB.Entry(objUpdate).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            BSM.ListSetting = DB.SYSHRBuiltinAccs.OrderBy(w => w.Code).ToList();
            return PartialView("GridviewSetting", BSM.ListSetting);
        }
        #endregion
        private void DataSelector()
        {
            ViewData["NUMBER_LIST"] = SMS.BSNumberRanks.ToList();
            ViewData["EXSPENEACCOUNT"] = DB.PRChartofAccounts.ToList();
            ViewData["SELECT_BRANCH"] = SMS.HRBranches.ToList();
        }
    }
}