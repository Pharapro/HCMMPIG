using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.HRS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class HRBankInfoController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000003";
        private const string URL_SCREEN = "/HRM/HRS/HRBankInfo";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBContext DB = new HumicaDBContext();
        public HRBankInfoController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: Branch


        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            BSM.ListBank = DB.HRBanks.ToList();
            BSM.ListBankBranch = DB.HRBankBranches.ToList();
            BSM.ListBankFee = DB.PRBankFees.ToList();
            BSM.ListBankAccount = DB.HRBankAccounts.ToList();
            return View(BSM);
        }


        #region Bank
        public ActionResult GridItemBanks()
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            BSM.ListBank = DB.HRBanks.ToList();
            return PartialView("GridItemBanks", BSM.ListBank);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBank(HRBank ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var Code = ModelObject.Code.ToUpper();
                    ModelObject.Code = Code;
                    DB.HRBanks.Add(ModelObject);
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
            BSM.ListBank = DB.HRBanks.ToList();
            return PartialView("GridItemBanks", BSM.ListBank);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBank(HRBank ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRBanks.Attach(ModelObject);
                    DB.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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

            BSM.ListBank = DB.HRBanks.ToList();
            return PartialView("GridItemBanks", BSM.ListBank);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBank(string Code)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.BankName == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRBanks.FirstOrDefault(w => w.Code == Code);
                        if (obj != null)
                        {
                            DB.HRBanks.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListBank = DB.HRBanks.ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemBanks", BSM.ListBank);
        }
        #endregion
        #region BankBranch
        public ActionResult GridItemBankBranchs()
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            BSM.ListBankBranch = DB.HRBankBranches.ToList();
            return PartialView("GridItemBankBranchs", BSM.ListBankBranch);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBankBranch(HRBankBranch ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    var Code = ModelObject.Code.ToUpper();
                    ModelObject.Code = Code;
                    DB.HRBankBranches.Add(ModelObject);
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
            BSM.ListBankBranch = DB.HRBankBranches.ToList();
            return PartialView("GridItemBankBranchs", BSM.ListBankBranch);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBankBranch(HRBankBranch ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRBankBranches.Attach(ModelObject);
                    DB.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
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

            BSM.ListBankBranch = DB.HRBankBranches.ToList();
            return PartialView("GridItemBankBranchs", BSM.ListBankBranch);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBankBranch(string Code)
        {
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (Code != null)
            {
                try
                {
                    var objEmp = DB.HRStaffProfiles.ToList();
                    if (objEmp.Where(w => w.BankBranch == Code).Any())
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
                    }
                    else
                    {
                        var obj = DB.HRBankBranches.FirstOrDefault(w => w.Code == Code);
                        if (obj != null)
                        {
                            DB.HRBankBranches.Remove(obj);
                            int row = DB.SaveChanges();
                        }
                        BSM.ListBankBranch = DB.HRBankBranches.ToList();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemBankBranchs", BSM.ListBankBranch);
        }
        #endregion

        #region "Bank Account"
        public ActionResult GridItemBankAcc()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            BSM.ListBankAccount = DB.HRBankAccounts.ToList();
            return PartialView("GridItemBankAcc", BSM.ListBankAccount);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBankAcc(HRBankAccount MModel)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRBankAccounts.Add(MModel);
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
            BSM.ListBankAccount = DB.HRBankAccounts.ToList();
            return PartialView("GridItemBankAcc", BSM.ListBankAccount);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBankAcc(HRBankAccount MModel)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.HRBankAccounts.Attach(MModel);
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
            BSM.ListBankAccount = DB.HRBankAccounts.ToList();
            return PartialView("GridItemBankAcc", BSM.ListBankAccount);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBankAcc(int ID)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            try
            {
                var obj = DB.HRBankAccounts.FirstOrDefault(w => w.ID == ID);
                if (obj != null)
                {
                    DB.HRBankAccounts.Remove(obj);
                    int row = DB.SaveChanges();
                }
                BSM.ListBankAccount = DB.HRBankAccounts.ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridItemBankAcc", BSM.ListBankAccount);
        }

        #endregion
        #region BankFee

        public ActionResult GridItemBankFee()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            BSM.ListBankFee = DB.PRBankFees.ToList();
            return PartialView("GridItemBankFee", BSM.ListBankFee);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateBankFee(PRBankFee MModel)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.BrankCode = MModel.BrankCode.ToUpper();
                    DB.PRBankFees.Add(MModel);
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
            BSM.ListBankFee = DB.PRBankFees.ToList();
            return PartialView("GridItemBankFee", BSM.ListBankFee);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditBankFee(PRBankFee MModel)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            if (ModelState.IsValid)
            {
                try
                {
                    DB.PRBankFees.Attach(MModel);
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
            BSM.ListBankFee = DB.PRBankFees.OrderBy(w => w.BrankCode).ToList();
            return PartialView("GridItemBankFee", BSM.ListBankFee);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteBankFee(int TranNo)
        {
            UserSession();
            DataSelector();
            MDBankInfor BSM = new MDBankInfor();
            try
            {
                var obj = DB.PRBankFees.Find(TranNo);
                if (obj != null)
                {
                    DB.PRBankFees.Remove(obj);
                    int row = DB.SaveChanges();
                }
                BSM.ListBankFee = DB.PRBankFees.OrderBy(w => w.BrankCode).ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridItemBankFee", BSM.ListBankFee);
        }

        #endregion

        private void DataSelector()
        {
            ViewData["BRANCH_SELECT"] = SMS.HRBranches.ToList();
            ViewData["BANK_SELECT"] = DB.HRBanks.Where(w => w.IsActive == true).ToList().OrderBy(w => w.Description);
            SYDataList objList = new SYDataList("FEE");
            ViewData["FEE_SELECT"] = objList.ListData;
        }
    }
}
