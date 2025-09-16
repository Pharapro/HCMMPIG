using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HRS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class HRCompanyTreeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000011";
        private const string URL_SCREEN = "/HRM/HRS/HRCompanyTree";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        HumicaDBContext DB = new HumicaDBContext();
        public HRCompanyTreeController()
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
            BSM.ListCompanyGroup = DB.HRCompanyGroups.OrderBy(w => w.Level).ToList();
            BSM.ListCompanyTree = DB.HRCompanyTrees.ToList();
            return View(BSM);
        }
        #region CompanyGroup

        public ActionResult GridviewCompanyGroups()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            BSM.ListCompanyGroup = DB.HRCompanyGroups.OrderBy(w => w.Level).ToList();
            return PartialView("GridviewCompanyGroups", BSM.ListCompanyGroup);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCompanyGroup(HRCompanyGroup MModel)
        {
            UserSession();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    var List = ClsTbableName.LoadDataGroup();
                    var obj = List.FirstOrDefault(x => x.WorkGroup == MModel.WorkGroup);
                    if (obj != null) { MModel.ObjectName = obj.TableName; }
                    DB.HRCompanyGroups.Add(MModel);
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
            BSM.ListCompanyGroup = DB.HRCompanyGroups.ToList();
            return PartialView("GridviewCompanyGroups", BSM.ListCompanyGroup.OrderBy(w => w.Level));
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCompanyGroup(HRCompanyGroup MModel)
        {
            UserSession();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRCompanyGroups.Find(MModel.WorkGroup);
                    var List = ClsTbableName.LoadDataGroup();
                    var obj = List.FirstOrDefault(x => x.WorkGroup == MModel.WorkGroup);
                    if (obj != null) { MModel.ObjectName = obj.TableName; }
                    DB.HRCompanyGroups.Attach(MModel);
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
            BSM.ListCompanyGroup = DB.HRCompanyGroups.ToList();
            return PartialView("GridviewCompanyGroups", BSM.ListCompanyGroup.OrderBy(w => w.Level));
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCompanyGroup(string WorkGroup)
        {
            UserSession();
            MDCompanyTree BSM = new MDCompanyTree();
            if (WorkGroup != null)
            {
                try
                {
                    var obj = DB.HRCompanyGroups.Find(WorkGroup);
                    if (obj != null)
                    {
                        DB.HRCompanyGroups.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                    BSM.ListCompanyGroup = DB.HRCompanyGroups.OrderBy(w => w.WorkGroup).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridviewCompanyGroups", BSM.ListCompanyGroup);
        }

        #endregion

        #region Tree

        public ActionResult GridviewTree()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            BSM.ListCompanyTree = DB.HRCompanyTrees.ToList();
            return PartialView("GridviewTree", BSM.ListCompanyTree);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTree(HRCompanyTree MModel)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    //var ListCompanyTree = DB.HRCompanyTrees.FirstOrDefault(w => w.LevelGroup == MModel.LevelGroup &&
                    //w.WorkGroup == MModel.WorkGroup && w.CompanyMember == MModel.CompanyMember);
                    //if (ListCompanyTree == null)
                    //{
                    MModel.CompanyMemberDesc = MDCompanyTree.GetDescription(MModel.WorkGroup, MModel.CompanyMember);
                    DB.HRCompanyTrees.Add(MModel);
                    int row = DB.SaveChanges();
                    //}
                    //else
                    //{
                    //    ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                    //}
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
            BSM.ListCompanyTree = DB.HRCompanyTrees.ToList();
            return PartialView("GridviewTree", BSM.ListCompanyTree);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTree(HRCompanyTree MModel)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new SMSystemEntity();
                    var obj = DB.HRCompanyTrees.FirstOrDefault(w => w.ID == MModel.ID);
                    obj.IsAssistant = MModel.IsAssistant;
                    obj.Level = MModel.Level;
                    obj.ParentWorkGroupID = MModel.ParentWorkGroupID;
                    obj.SubParent = MModel.SubParent;
                    obj.CompanyMemberDesc = MDCompanyTree.GetDescription(MModel.WorkGroup, MModel.CompanyMember);
                    DB.HRCompanyTrees.Attach(obj);
                    DB.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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
            BSM.ListCompanyTree = DB.HRCompanyTrees.ToList();
            return PartialView("GridviewTree", BSM.ListCompanyTree);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTree(int ID)
        {
            UserSession();
            DataSelector();
            MDCompanyTree BSM = new MDCompanyTree();
            try
            {
                var obj = DB.HRCompanyTrees.FirstOrDefault(w => w.ID == ID);
                if (obj != null)
                {
                    DB.HRCompanyTrees.Remove(obj);
                    int row = DB.SaveChanges();
                }
                BSM.ListCompanyTree = DB.HRCompanyTrees.ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridviewTree", BSM.ListCompanyTree);
        }

        #endregion

        [HttpPost]
        public ActionResult GetData(string Branch, string Department)
        {
            System.Diagnostics.Debug.WriteLine($"Branch: {Branch}, Department: {Department}");

            MDCompanyTree BSM = new MDCompanyTree();
            var filteredData = BSM.LoadDatas();

            var data = new
            {
                MS = "OK",
                DT = filteredData,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectItemElement(string Code)
        {
            Session["CompanyGroupGroup"] = Code;
            Session["ParentWorkGroup"] = Code;
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        public ActionResult SelectItemID(int ID)
        {
            var CompanyTree = DB.HRCompanyTrees.FirstOrDefault(w => w.ID == ID);
            if (CompanyTree != null)
                SelectItemElement(CompanyTree.WorkGroup);
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        public ActionResult SelectParentElement(string Code)
        {
            Session["ParentWorkGroup"] = Code;
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);

        }
        public ActionResult CompanyGroupItem()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "HRCompanyTree", Action = "CompanyGroupItem" };
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", Humica.EF.Models.SY.SYSettings.getLabel("Code"));
                cboProperties.Columns.Add("Description", Humica.EF.Models.SY.SYSettings.getLabel("Description"));
                cboProperties.BindList(Humica.Logic.HRS.MDCompanyTree.GetCompanyGroupItem());
            });
        }
        private void DataSelector()
        {
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData["WorkGroup_SELECT"] = ClsTbableName.LoadDataGroup();
            ViewData["CompanyGroup_SELECT"] = DB.HRCompanyGroups.ToList();
            ViewData["CompanyMember"] = new HumicaDBViewContext().HR_CompanyGroup_View.ToList();
        }

    }
}
