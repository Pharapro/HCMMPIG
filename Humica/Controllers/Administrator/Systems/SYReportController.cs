using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.CF;
using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class SYReportController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYRPT003";
        private const string URL_SCREEN = "/Administrator/Systems/SYReport/";
        private SMSystemEntity DBA = new SMSystemEntity();
        private HumicaDBContext DB = new HumicaDBContext();
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        public SYReportController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            DataList(BSM.ScreenID);
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(CFSystem BSM)
        {
            UserSession();
            DataList(BSM.ScreenID);
            UserConf(ActionBehavior.EDIT);
            Session[SYConstant.PARAM_ID] = BSM.ScreenID;
            ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;

            //Report
            BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == BSM.ScreenID).OrderBy(w => w.InOrder).ToList();
            //Report Item
            BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == BSM.ScreenID).OrderBy(w => w.InOrder).ToList();
            //Report form
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == BSM.ScreenID).ToList();
            var DBU = new HumicaDBContext();
            if (BSM.ListReportObject.Count == 0)
            {
                //var obj = new CFReportObject();
                //obj.ScreenID = BSM.ScreenID;
                //obj.ReportObject = "DEFAULT";
                //DBU.CFReportObjects.Add(obj);
                //DBU.SaveChanges();
                BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == BSM.ScreenID).ToList();
            }
            if (BSM.ListReport.Count == 0)
            {
                var DBUS = new SMSystemEntity();
                if (BSM.ScreenIDReference != null)
                {



                    var ListReference = DBA.CFReports.Where(w => w.ScreenId == BSM.ScreenIDReference).ToList();
                    if (ListReference.Count > 0)
                    {
                        foreach (var read in ListReference)
                        {
                            read.ScreenId = BSM.ScreenID;
                            DBUS.CFReports.Add(read);
                        }
                        var ListItemReference = DBA.CFReportItems.Where(w => w.ScreenId == BSM.ScreenIDReference).ToList();
                        foreach (var read in ListItemReference)
                        {
                            read.ScreenId = BSM.ScreenID;
                            DBUS.CFReportItems.Add(read);
                        }
                        var objDev = DBUS.SYScreenDevelops.Find(BSM.ScreenIDReference);
                        if (objDev != null)
                        {
                            var obj = new SYScreenDevelop();
                            obj.DashboardObject = objDev.DashboardObject;
                            obj.Description = objDev.Description;
                            obj.FormObject = objDev.FormObject;
                            obj.ItemObject = objDev.ItemObject;
                            obj.ListObject = objDev.ListObject;
                            obj.RptObject = objDev.RptObject;
                            obj.ScreenId = BSM.ScreenID;
                            DBUS.SYScreenDevelops.Add(obj);
                        }

                        DBUS.SaveChanges();
                        //Report
                        BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == BSM.ScreenID).OrderBy(w => w.InOrder).ToList();
                        //Report Item
                        BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == BSM.ScreenID).OrderBy(w => w.InOrder).ToList();
                    }
                }
            }

            return View(BSM);
        }

        #endregion
        #region "Report"
        public ActionResult GridItems1(string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            DataList(ScreenID);
            return PartialView("GridItems1", BSM.ListReport);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFReport ModelObject, string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.ScreenId = ScreenID;
                    DBA.CFReports.Add(ModelObject);
                    DataList(ScreenID);
                    int row = DBA.SaveChanges();
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
            DataList(ScreenID);
            BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListReport);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFReport ModelObject, string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.ScreenId = ScreenID;
                    DBA.CFReports.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
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
            DataList(ScreenID);
            BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListReport);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(int ID, string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {

                    var obj = DBA.CFReports.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DBA.CFReports.Remove(objAdd);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList(ScreenID);
            BSM.ListReport = DBA.CFReports.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListReport);
        }
        #endregion
        #region "Report Item"
        public ActionResult GridItems2(string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            DataList(ScreenID);
            return PartialView("GridItems2", BSM.ListReportItem);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial2(string ScreenID, CFReportItem ModelObject)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.ScreenId = ScreenID;
                    DBA.CFReportItems.Add(ModelObject);
                    DataList(ScreenID);
                    int row = DBA.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList(ScreenID);
            BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListReportItem);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial2(string ScreenID, CFReportItem ModelObject)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.ScreenId = ScreenID;
                    DBA.CFReportItems.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList(ScreenID);
            BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListReportItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial2(string ScreenID, int ID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ID != 0)
            {
                try
                {
                    var obj = DBA.CFReportItems.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DBA.CFReportItems.Remove(objAdd);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList(ScreenID);
            BSM.ListReportItem = DBA.CFReportItems.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems2", BSM.ListReportItem);
        }
        #endregion
        #region "Report Form"
        public ActionResult GridItems3(string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            DataList(ScreenID);
            return PartialView("GridItems3", BSM.ListReportObject);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial3(string ScreenID, CFReportObject ModelObject)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    string Path = "~" + SYSettings.getSetting("REPORT_PATH").SettinValue + ModelObject.ReportObject;
                    ModelObject.Company = "";
                    if (ModelObject.DocType == null || ModelObject.DocType == "") ModelObject.DocType = "";
                    ModelObject.PathStore = Path.Replace("//", "/");
                    ModelObject.ScreenID = ScreenID;
                    DB.CFReportObjects.Add(ModelObject);
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList(ScreenID);
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            return PartialView("GridItems3", BSM.ListReportObject);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial3(string ScreenID, CFReportObject ModelObject)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ModelState.IsValid)
            {
                try
                {
                    var Obj = DB.CFReportObjects.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (Obj != null)
                    {
                        string Path = "~" + SYSettings.getSetting("REPORT_PATH").SettinValue + ModelObject.ReportObject;
                        Obj.PathStore = Path.Replace("//", "/");
                        Obj.DocType = ModelObject.DocType;
                        if (Obj.DocType == null || Obj.DocType == "") Obj.DocType = "";
                        Obj.ScreenID = ScreenID;
                        Obj.ReportObject = ModelObject.ReportObject;
                        Obj.ObjectName = ModelObject.ObjectName;
                        Obj.Company = "";
                        DB.CFReportObjects.Attach(Obj);
                        DB.Entry(Obj).State = System.Data.Entity.EntityState.Modified;
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
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList(ScreenID);
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            return PartialView("GridItems3", BSM.ListReportObject);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial3(string ScreenID, int ID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (ID != 0)
            {
                try
                {
                    var obj = DB.CFReportObjects.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DB.CFReportObjects.Remove(objAdd);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList(ScreenID);
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            return PartialView("GridItems3", BSM.ListReportObject);
        }
        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = new CFUploadPath();
            var _scrin = Session[SYConstant.PARAM_ID];
            path.PathDirectory = ClsConstant.DEFAULT_REPORT_PATH + _scrin + "/";// "RPTHR00013/";

            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;
            return null;
        }

        public ActionResult Download(int ID)
        {
            UserSession();
            var report = DB.CFReportObjects.Where(w => w.ID == ID).ToList();
            if (report.Count > 0)
            {
                var _ReportStore = report.FirstOrDefault();
                string name = _ReportStore.PathStore;
                string FileSource = Server.MapPath(_ReportStore.PathStore);

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + _ReportStore.ObjectName + ".repx");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.WriteFile(name);
                Response.End();
            }

            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Report File"
        public ActionResult GridReportFile(string ScreenID)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            DataList(ScreenID);
            return PartialView("GridReportFile", BSM.GetFilesReport());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteReportFile(string ScreenID, string FileName)
        {
            ScreenID = Session[SYConstant.PARAM_ID].ToString();
            CFSystem BSM = new CFSystem();
            ViewData[SYConstant.PARAM_ID] = Session[SYConstant.PARAM_ID].ToString();
            if (FileName != null)
            {
                try
                {
                    var path = Server.MapPath("~") + SYSettings.getSetting("REPORT_PATH").SettinValue + "/" + FileName;
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListReportObject = DB.CFReportObjects.Where(w => w.ScreenID == ScreenID).ToList();
            DataList(ScreenID);
            return PartialView("GridReportFile", BSM.GetFilesReport());
        }
        #endregion
        #region "Private Code"
        private void DataList(string ScreenID)
        {
            UserSession();
            ViewData["MODULE_LIST"] = new SYDataList("MODULE").ListData;
            ViewData["ITEM_REPORT_TYPE"] = new SYDataList("ITEM_REPORT_TYPE").ListData;
            ViewData["REPORT_ID_LIST"] = new SYDataList("REPORT_TYPE").ListData;
            SYScreenDevelop sd = DBA.SYScreenDevelops.Find(ScreenID);
            if (sd != null)
            {
                var fieldList = DBA.SY_Schema.Where(w => w.TableName == sd.RptObject).ToList();
                ViewData["FIELD_LIST"] = fieldList;
            }
            ViewData["REPORT_ID_LIST"] = DBA.CFReports.Where(w => w.ScreenId == ScreenID).OrderBy(w => w.InOrder).ToList();

        }

        #endregion
    }
}