using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HRS;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.HRM.HRS
{
    public class HRPositionStructureController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "INF0000016";
        private const string URL_SCREEN = "/HRM/HRS/HRPositionStructure";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        HumicaDBContext DB = new HumicaDBContext();
        public HRPositionStructureController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: Tree
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            BSM.ListPositionStructure = DB.HRPositionStructures.ToList();
            return View(BSM);
        }

        #region Position Structure

        public ActionResult GridviewPositionStructure()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            BSM.ListPositionStructure = DB.HRPositionStructures.ToList();
            return PartialView("GridviewPositionStructure", BSM.ListPositionStructure);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePositionStructure(HRPositionStructure MModel)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    ClsFilterStaff Staff = new ClsFilterStaff();
                    MModel.Description = Staff.Get_Positon(MModel.Code);
                    DB.HRPositionStructures.Add(MModel);
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
            BSM.ListPositionStructure = DB.HRPositionStructures.ToList();
            return PartialView("GridviewPositionStructure", BSM.ListPositionStructure);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPositionStructure(HRPositionStructure MModel)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRPositionStructures.Find(MModel.ID);
                    ClsFilterStaff Staff = new ClsFilterStaff();
                    MModel.Description = Staff.Get_Positon(MModel.Code);
                    DB.HRPositionStructures.Attach(MModel);
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
            BSM.ListPositionStructure = DB.HRPositionStructures.ToList();
            return PartialView("GridviewPositionStructure", BSM.ListPositionStructure);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePositionStructure(int ID)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ID != null)
            {
                try
                {
                    var obj = DB.HRPositionStructures.Find(ID);
                    if (obj != null)
                    {
                        DB.HRPositionStructures.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListPositionStructure = DB.HRPositionStructures.ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewPositionStructure", BSM.ListPositionStructure);
        }

        #endregion

        [HttpPost]
        public ActionResult GetData(string Branch, string Department)
        {
            System.Diagnostics.Debug.WriteLine($"Branch: {Branch}, Department: {Department}");

            MDCompanyTree BSM = new MDCompanyTree();
            var filteredData = BSM.LoadDataPosition();

            var data = new
            {
                MS = "OK",
                DT = filteredData,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ParentItem()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "HRPositionStructure", Action = "ParentItem" };
                cboProperties.Width = Unit.Percentage(100);
                cboProperties.ValueField = "ID";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0}:{2}";
                cboProperties.Columns.Add("ID", SYSettings.getLabel("ID"), 80);
                cboProperties.Columns.Add("Code", SYSettings.getLabel("Code"), 150);
                cboProperties.Columns.Add("Description", SYSettings.getLabel("Description"), 250);
                cboProperties.BindList(MDCompanyTree.GetParentPosition());
            });
        }
        public ActionResult SelectItemID(string Code)
        {
            var CompanyTree = DB.HRPositionStructures.FirstOrDefault();
            if (CompanyTree != null)
                SelectItemElement(Code);
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        public ActionResult SelectItemElement(string Code)
        {
            Session["ParentPosition"] = Code;
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        private void DataSelector()
        {
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
        }
    }
}