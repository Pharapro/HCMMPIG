using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace HR.Controllers.PR.PRM
{

    public class PREmpCostCenterController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "PRM0000025";
        private const string URL_SCREEN = "/PR/PRM/PREmpCostCenter/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "EmpCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        public PREmpCostCenterController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.Header = new PREmpCCCharge();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.ListCCCharge = new List<PREmpCCCharge>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #region 'Grid'
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            BSM.ListCCCharge = new List<PREmpCCCharge>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateItemEdit(PREmpCCCharge ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
                        var objCheck = BSM.ListCCCharge.Where(w => w.CostCenter == ModelObject.CostCenter).ToList();

                        if (objCheck.Count > 0)
                        {

                            var EmpCode = objCheck.First().EmpCode;
                            var CheckDB = DB.PREmpCCCharges.Where(w => w.EmpCode == EmpCode && w.CostCenter == ModelObject.CostCenter).ToList();
                            if (CheckDB.Count > 0)
                            {
                                foreach (var read in CheckDB)
                                {
                                    DB.PREmpCCCharges.Remove(read);
                                    DB.SaveChanges();
                                }
                            }
                            if (ModelObject.Charge > 0)
                            {
                                ModelObject.EmpCode = EmpCode;
                                ModelObject.Description = ModelObject.Description;
                                ModelObject.CreatedBy = user.UserName;
                                ModelObject.CreatedOn = DateTime.Now;
                                DB.PREmpCCCharges.Add(ModelObject);
                            }
                            objCheck.First().Charge = ModelObject.Charge;
                            objCheck.First().Remark = ModelObject.Remark;
                            objCheck.First().Description = ModelObject.Description;
                            DB.SaveChanges();

                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            Session[Index_Sess_Obj + ActionName] = BSM;
                            ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                        }
                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
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

            return PartialView("GridItems", BSM);
        }
        #endregion 'Grid'

        [HttpPost]
        public ActionResult SelectedChanged(String EmpCode, string Action)
        {
            ActionName = Action;
            PRCostCenterObject BSM = new PRCostCenterObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRCostCenterObject)Session[Index_Sess_Obj + ActionName];
            }
            var chkEmp = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            var list = DB.PREmpCCCharges.Where(w => w.EmpCode == EmpCode).ToList();
            if (list.Count > 0)
            {
                BSM.ListCCCharge = list;
                var Charg = DB.PREmpCCCharges.Where(w => w.EmpCode == EmpCode).Select(x => x.CostCenter).ToList();
                var Cost = DB.PRCostCenters.Where(w => !Charg.Contains(w.Project)).ToList();
                if (Cost.Count > 0)
                {
                    foreach (var read in Cost)
                    {
                        var Result = new PREmpCCCharge();
                        Result.EmpCode = EmpCode;
                        Result.CostCenter = read.Project;
                        Result.Description = read.Description;
                        Result.Charge = 0;
                        BSM.ListCCCharge.Add(Result);
                    }
                }
            }
            else
            {
                BSM.ListCCCharge = new List<PREmpCCCharge>();
                var LT = DB.PRCostCenters.ToList();
                foreach (var item in LT)
                {
                    var Result = new PREmpCCCharge();
                    Result.EmpCode = EmpCode;
                    Result.CostCenter = item.Project;
                    Result.Description = item.Description;
                    Result.Charge = 0;
                    BSM.ListCCCharge.Add(Result);
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            if (BSM.ListCCCharge.Count > 0)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    EmpName = chkEmp.AllName,
                    EmpType = chkEmp.EmployeeType,
                    Div = chkEmp.Division,
                    Dept = chkEmp.Department,
                    SEC = chkEmp.Section,
                    Level = chkEmp.Level,
                    POST = chkEmp.Position,
                    SDATE = chkEmp.StartDate
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            else
            {
                var rs = new { MS = SYConstant.FAIL };
                return Json(rs, JsonRequestBehavior.DenyGet);
            }
        }
        #region private code
        private void DataSelector()
        {
            ViewData["EMPCODE_SELECT"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["COSTCENTER_SELECT"] = DB.PRCostCenters.ToList();
        }

        #endregion
    }
}
