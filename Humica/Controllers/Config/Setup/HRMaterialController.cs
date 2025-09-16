using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class HRMaterialController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "HRM0000006";
        private const string URL_SCREEN = "/Config/Setup/HRMaterial/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ItemCode";
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();


        public HRMaterialController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            MDMaterialObject BSM = new MDMaterialObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (MDMaterialObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = DB.HRMaterials.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            MDMaterialObject BSM = new MDMaterialObject();
            BSM.ListHeader = new List<HRMaterial>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDMaterialObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            MDMaterialObject BSD = new MDMaterialObject();
            BSD.ListHeader = new List<HRMaterial>();
            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(MDMaterialObject collection)
        {
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ActionName = "Create";
            MDMaterialObject BSM = new MDMaterialObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (MDMaterialObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Material = collection.Material;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateHRMaterial();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Material.ItemCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region "Edit"
        public ActionResult Edit(string ID)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = ID;
            if (ID == "null") ID = null;
            if (ID != null)
            {
                MDMaterialObject BSM = new MDMaterialObject();
                BSM.Material = new HRMaterial();
                BSM.Material = DB.HRMaterials.FirstOrDefault(w => w.ItemCode == ID);
                var resualt = DB.HRMaterials;
                if (BSM.Material != null)
                {
                    List<HRMaterial> listEmpfa = resualt.Where(x => x.ItemCode == BSM.Material.ItemCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, MDMaterialObject collection)
        {
            ActionName = "Edit";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            MDMaterialObject BSM = new MDMaterialObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (MDMaterialObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Material = collection.Material;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditHRMaterial(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return View(BSM);

        }
        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            if (id == "null") id = null;
            if (id != null)
            {
                MDMaterialObject BSM = new MDMaterialObject();
                BSM.Material = new HRMaterial();
                BSM.Material = DB.HRMaterials.FirstOrDefault(w => w.ItemCode == id);
                var resualt = DB.HRMaterials;
                if (BSM.Material != null)
                {
                    List<HRMaterial> listEmpfa = resualt.Where(x => x.ItemCode == BSM.Material.ItemCode).ToList();
                    BSM.ListHeader = listEmpfa.ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (id == "null") id = null;
            if (id != null)
            {
                MDMaterialObject BSM = new MDMaterialObject();
                string msg = BSM.DeleteHRMaterial(id);
                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Ajax Image'
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
            var objFile = new SYFileImportImage(path);
            objFile.TokenKey = ClsCrypo.GetUniqueKey(15);

            objFile.ObjectTemplate = new MDUploadImage();
            objFile.ObjectTemplate.ScreenId = SCREEN_ID;
            objFile.ObjectTemplate.Module = "MASTER";
            objFile.ObjectTemplate.TokenCode = objFile.TokenKey;
            objFile.ObjectTemplate.UploadBy = user.UserName;

            Session[SYSConstant.IMG_SESSION_KEY_1] = objFile.TokenKey;
            UploadControlExtension.GetUploadedFiles("uc_image", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session["PATH_IMG"] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }
        public string DeleteFile(string FileName)
        {
            //string test = "9586-202-10072";
            // string result = FileName.Substring(FileName.LastIndexOf('/') + 1);

            try
            {
                string[] sp = FileName.Split('/');
                string file = sp[sp.Length - 1];
                string[] spf = file.Split('.');
                string sfile = spf[0];

                var obj = DB.MDUploadImages.FirstOrDefault(w => w.TokenCode == sfile);
                if (obj != null)
                {
                    DB.MDUploadImages.Remove(obj);
                    int row = DB.SaveChanges();
                    if (row > 0)
                    {
                        var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
                        string root = HostingEnvironment.ApplicationPhysicalPath;
                        // obj.UpoadPath = obj.UpoadPath.Replace("~", "").Replace("/", "\\");
                        obj.UpoadPath = obj.UpoadPath.Replace("~/", "").Replace("/", "\\");
                        string fileNameDelete = root + obj.UpoadPath;
                        if (System.IO.File.Exists(fileNameDelete))
                        {
                            System.IO.File.Delete(fileNameDelete);
                        }
                    }
                }
                return SYConstant.OK;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        #endregion

        //public ActionResult GridItems()
        //{
        //    UserConf(ActionBehavior.EDIT);

        //    MDMaterialObject BSM = new MDMaterialObject();
        //    BSM.ListHeader = DB.HRMaterials.ToList();
        //    return PartialView("GridItems", BSM.ListHeader);
        //}

        //edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult Edit(HRMaterial MModel)
        //{
        //    UserSession();
        //    MDMaterialObject BSM = new MDMaterialObject();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            DB.HRMaterials.Attach(MModel);
        //            DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
        //            int row = DB.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
        //    }
        //    BSM.ListHeader = DB.HRMaterials.OrderBy(w => w.ItemCode).ToList();
        //    return PartialView("GridItems", BSM.ListHeader);
        //}
        //delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult Delete(string ItemCode)
        //{
        //    UserSession();
        //    MDMaterialObject BSM = new MDMaterialObject();
        //    if (ItemCode != null)
        //    {
        //        try
        //        {
        //                var obj = DB.HRMaterials.Find(ItemCode);
        //                if (obj != null)
        //                {
        //                    DB.HRMaterials.Remove(obj);
        //                    int row = DB.SaveChanges();

        //                }
        //                BSM.ListHeader = DB.HRMaterials.OrderBy(w => w.ItemCode).ToList();
        //            //}
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }         
        //    return PartialView("GridItems", BSM.ListHeader);
        //}
        private void DataSelector()
        {
            //SYDataList objList = new SYDataList("FAAVERAGE_SELECT");
            //ViewData["FAAVERAGE_SELECT"] = objList.ListData;
        }
    }
}
