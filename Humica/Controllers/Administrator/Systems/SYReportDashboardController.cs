using Humica.Core.CF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class SYReportDashboardController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYRD0003";
        private const string URL_SCREEN = "/Administrator/Systems/SYReportDashboard/";
        private SMSystemEntity DB = new SMSystemEntity();
        public SYReportDashboardController()
           : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            DataList();

            UserConf(ActionBehavior.EDIT);

            CFSystem BSM = new CFSystem();
            //Report
            // BSM.ListReport = DB.CFReports.ToList();
            //Report Item
            //BSM.ListReportItem = DB.CFReportItems.ToList();
            //Dashboard Master
            BSM.ListDashboardMaster = DB.SYDashboardMasters.ToList();
            //Dashboard
            BSM.ListDashboard = DB.SYDashboards.ToList();
            //Report Template
            //BSM.ListReportModelTemplate = DB.CFModelReports.Where(w => w.CompanyCode == bs.CompanyCode).OrderBy(w=>w.MaterialOrder).ToList();
            return View(BSM);
        }


        #endregion
        #region "Dashboard Master"
        public ActionResult GridItems3()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListDashboardMaster = DB.SYDashboardMasters.ToList();
            DataList();
            return PartialView("GridItems3", BSM.ListDashboardMaster);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial3(SYDashboardMaster ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DB.SYDashboardMasters.Add(ModelObject);
                    DataList();
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE003", user.Lang);
            }
            DataList();
            BSM.ListDashboardMaster = DB.SYDashboardMasters.ToList();
            return PartialView("GridItems3", BSM.ListDashboardMaster);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial3(SYDashboardMaster ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DB.SYDashboardMasters.Attach(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE003", user.Lang);
            }
            DataList();
            BSM.ListDashboardMaster = DB.SYDashboardMasters.ToList();
            return PartialView("GridItems3", BSM.ListDashboardMaster);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial3(string DashBoardName)
        {
            CFSystem BSM = new CFSystem();
            if (DashBoardName != null)
            {
                try
                {
                    var obj = DB.SYDashboardMasters.Find(DashBoardName);
                    if (obj != null)
                    {

                        DB.SYDashboardMasters.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListDashboardMaster = DB.SYDashboardMasters.ToList();
            return PartialView("GridItems3", BSM.ListDashboardMaster);
        }
        #endregion
        #region "Dashboard"
        public ActionResult GridItems4()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListDashboard = DB.SYDashboards.ToList();
            DataList();
            return PartialView("GridItems4", BSM.ListDashboard);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial4(SYDashboard ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.Datasource == null)
                        ModelObject.Datasource = "";
                    ModelObject.AccountType = "N";
                    // ModelObject.CompanyCode = bs.CompanyCode;
                    DB.SYDashboards.Add(ModelObject);
                    DataList();
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
            }
            DataList();
            BSM.ListDashboard = DB.SYDashboards.ToList();//.Where(w => w.CompanyCode == bs.CompanyCode).ToList();
            return PartialView("GridItems4", BSM.ListDashboard);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial4(SYDashboard ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelObject.Datasource == null)
                        ModelObject.Datasource = "";
                    //ModelObject.CompanyCode = bs.CompanyCode;
                    ModelObject.AccountType = "N";
                    DB.SYDashboards.Attach(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
            }
            DataList();
            BSM.ListDashboard = DB.SYDashboards.ToList();//.Where(w=>w.CompanyCode==bs.CompanyCode).ToList();
            return PartialView("GridItems4", BSM.ListDashboard);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial4(int ID)
        {
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {
                    var obj = DB.SYDashboards.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DB.SYDashboards.Remove(objAdd);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListDashboard = DB.SYDashboards.ToList();//.Where(w => w.CompanyCode == bs.CompanyCode).ToList();
            return PartialView("GridItems4", BSM.ListDashboard);
        }
        #endregion
        #region "Material List"
        //public ActionResult GridItems5()
        //{
        //    UserSession();
        //    UserConfListAndForm();
        //    UserConf(ActionBehavior.EDIT);
        //    CFSystem BSM = new CFSystem();
        //    BSM.ListReportModelTemplate = DB.CFModelReports.Where(w => w.CompanyCode == bs.CompanyCode).OrderBy(w=>w.MaterialOrder).ToList();
        //    DataList();
        //    return PartialView("GridItems5", BSM.ListReportModelTemplate);
        //}

        //create
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreatePartial5(CFModelReport ModelObject)
        //{
        //    UserSession();
        //    UserConfListAndForm();
        //    CFSystem BSM = new CFSystem();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            ModelObject.CompanyCode = bs.CompanyCode;
        //            var objMat = DB.MDMaterials.Find(ModelObject.MaterialCode,bs.CompanyCode);
        //            ModelObject.MaterialDescription = objMat.MaterialDecription1;
        //            ModelObject.ModelCode = objMat.ModelCode;
        //            var objModel = DB.MDMaterialModels.Find(objMat.ModelCode, objMat.Year);
        //            ModelObject.ModelDescription = objModel.ModelDescription1;
        //            ModelObject.ColorCode = objMat.Color;
        //            DB.CFModelReports.Add(ModelObject);
        //            DataList();
        //            int row = DB.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
        //    }
        //    DataList();
        //    BSM.ListReportModelTemplate = DB.CFModelReports.Where(w => w.CompanyCode == bs.CompanyCode).OrderBy(w=>w.MaterialOrder).ToList();
        //    return PartialView("GridItems5", BSM.ListReportModelTemplate);
        //}
        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditPartial5(CFModelReport ModelObject)
        //{
        //    UserSession();
        //    UserConfListAndForm();
        //    CFSystem BSM = new CFSystem();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var objUPdate = DB.CFModelReports.Find(ModelObject.ID);
        //            objUPdate.CompanyCode = bs.ObjectCode;
        //            var objMat = DB.MDMaterials.Find(ModelObject.MaterialCode, bs.CompanyCode);
        //            objUPdate.MaterialDescription = objMat.MaterialDecription1;
        //            objUPdate.ModelCode = objMat.ModelCode;

        //            objUPdate.MaterialOrder = ModelObject.MaterialOrder;
        //            objUPdate.ModelOrder = ModelObject.ModelOrder;

        //            var objModel = DB.MDMaterialModels.Find(objMat.ModelCode, objMat.Year);
        //            objUPdate.ModelDescription = objModel.ModelDescription1;
        //            objUPdate.ColorCode = objMat.Color;
        //            DB.CFModelReports.Attach(objUPdate);
        //            DB.Entry(objUPdate).State = System.Data.Entity.EntityState.Modified;
        //            int row = DB.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
        //    }
        //    DataList();
        //    BSM.ListReportModelTemplate = DB.CFModelReports.Where(w => w.CompanyCode == bs.CompanyCode).OrderBy(w=>w.MaterialOrder).ToList();
        //    return PartialView("GridItems5", BSM.ListReportModelTemplate);
        //}
        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeletePartial5(int ID)
        //{
        //    UserSession();
        //    UserConfListAndForm();
        //    CFSystem BSM = new CFSystem();
        //    if (ID != 0)
        //    {
        //        try
        //        {
        //            var obj = DB.CFModelReports.Find(ID);
        //            if (obj!=null)
        //            {
        //                DB.CFModelReports.Remove(obj);
        //                int row = DB.SaveChanges();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    DataList();
        //    BSM.ListReportModelTemplate = DB.CFModelReports.Where(w => w.CompanyCode == bs.CompanyCode).OrderBy(w=>w.MaterialOrder).ToList();
        //    return PartialView("GridItems5", BSM.ListReportModelTemplate);
        //}
        #endregion
        #region "Private Code"
        private void DataList()
        {
            UserSession();
            ViewData["DASH_LIST"] = DB.SYDashboardMasters.ToList();//.Where(w=>w.Indicator=="N").ToList();
            var listUser = DB.SYUsers.ToList();//.Where(w => w.CompanyOwner == bs.CompanyCode).ToList();
            var usr = new SYUser();
            usr.UserName = "*";
            listUser.Add(usr);
            ViewData["USER_LIST"] = listUser;
            ViewData["MODULE_LIST"] = new SYDataList("MODULE").ListData;
            ViewData["GRAPH_TYPE"] = DB.SYGraphTypes.ToList();
            // ViewData["MATERIAL_LIST"] = DB.MDMaterials.Where(w => w.Plant == bs.ObjectCode).ToList();
            // ViewData["MODEL_LIST"] = DB.MDMaterialModels.ToList();
        }
        #endregion

    }
}
