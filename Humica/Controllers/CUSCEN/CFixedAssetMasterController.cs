using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Integration.EF;
using Humica.Integration.EF.Models;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Asset
{
    public class CFixedAssetMasterController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "SYNC000001";
        private const string URL_SCREEN = "/Asset/CFixedAssetMaster/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AssetCode;status";
        CUSCENDB DB = new CUSCENDB();
        public CFixedAssetMasterController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public async Task<ActionResult> Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ClsAssetManagement BSM = new ClsAssetManagement();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ClsAssetManagement)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListHeader = new List<MDAssetManagement>();
            //BSM.ListAssetType = new List<MDFixAssetType>();
            BSM.ListAssetClass = new List<MDFixAssetClass>();
            await BSM.LoadData();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsAssetManagement BSM = new ClsAssetManagement();
            BSM.ListHeader = new List<MDAssetManagement>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAssetManagement)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        public ActionResult _ListFixAssetType()
        {
            UserSession();
            UserConfListAndForm();
            ClsAssetManagement BSM = new ClsAssetManagement();
            BSM.ListAssetType = DB.MDFixAssetTypes.ToList();
            return PartialView("_ListFixAssetType", BSM.ListAssetType);
        }
        public ActionResult _ListFixAssetClass()
        {
            UserSession();
            UserConfListAndForm();
            ClsAssetManagement BSM = new ClsAssetManagement();
            BSM.ListAssetClass = DB.MDFixAssetClasses.ToList();
            return PartialView("_ListFixAssetClass", BSM.ListAssetClass);
        }
        public ActionResult _ListFixAssetMaster()
        {
            UserSession();
            UserConfListAndForm();
            ClsAssetManagement BSM = new ClsAssetManagement();
            BSM.ListHeader = DB.MDAssetManagements.ToList();
            return PartialView("_ListFixAssetMaster", BSM.ListHeader);
        }

        public async Task<ActionResult> Transfer(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ClsAssetManagement();
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsAssetManagement)Session[Index_Sess_Obj + ActionName];
                }
                var msg = await BSM.TransferAsset(id);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("TANSFER_COMPLATED", user.Lang);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        //#region "Create"
        //public ActionResult Create()
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    DataSelector();
        //    UserConfListAndForm(this.KeyName);
        //    AssetRegisterObject BSM = new AssetRegisterObject();
        //    BSM.Header = new HRAssetRegister();
        //    BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
        //    Session[Index_Sess_Obj + ActionName] = BSM;
        //    return View(BSM);
        //}
        //[HttpPost]
        //public ActionResult Create(AssetRegisterObject collection)
        //{
        //    UserSession();
        //    UserConfListAndForm(this.KeyName);
        //    DataSelector();
        //    ActionName = "Create";
        //    AssetRegisterObject BSM = new AssetRegisterObject();
        //    if (ModelState.IsValid)
        //    {
        //        if (Session[Index_Sess_Obj + ActionName] != null)
        //        {
        //            BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
        //        }
        //        BSM.Header = collection.Header;
        //        BSM.ScreenId = SCREEN_ID;
        //        string msg = BSM.CreateFixAsset();
        //        if (msg == SYConstant.OK)
        //        {
        //            SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
        //            mess.DocumentNumber = BSM.Header.AssetCode.ToString();
        //            mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
        //            ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
        //            BSM = new AssetRegisterObject();
        //            BSM.Header = new HRAssetRegister();
        //            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
        //            Session[Index_Sess_Obj + ActionName] = BSM;
        //            return View(BSM);
        //        }
        //        else
        //        {
        //            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //            Session[Index_Sess_Obj + ActionName] = BSM;
        //            return View(BSM);
        //        }
        //    }
        //    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

        //}
        //#endregion

        //#region "Edit"
        //public ActionResult Edit(string ID)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    ViewData[SYSConstant.PARAM_ID] = ID;
        //    if (ID == "null") ID = null;
        //    if (ID != null)
        //    {
        //        AssetRegisterObject BSM = new AssetRegisterObject();
        //        BSM.Header = new HRAssetRegister();
        //        BSM.Header = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == ID);
        //        if (BSM.Header != null)
        //        {
        //            Session[Index_Sess_Obj + ActionName] = BSM;
        //            return View(BSM);
        //        }
        //    }
        //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}
        //[HttpPost]
        //public ActionResult Edit(string id, AssetRegisterObject collection)
        //{
        //    ActionName = "Create";
        //    UserSession();
        //    this.ScreendIDControl = SCREEN_ID;
        //    DataSelector();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    ViewData[SYSConstant.PARAM_ID] = id;
        //    AssetRegisterObject BSM = new AssetRegisterObject();
        //    if (id != null)
        //    {
        //        if (Session[Index_Sess_Obj + ActionName] != null)
        //        {
        //            BSM = (AssetRegisterObject)Session[Index_Sess_Obj + ActionName];
        //            BSM.Header = collection.Header;
        //        }
        //        BSM.ScreenId = SCREEN_ID;
        //        string msg = BSM.EditFixAsset(id);
        //        if (msg == SYConstant.OK)
        //        {
        //            SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
        //            mess.DocumentNumber = id;
        //            mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
        //            ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
        //            return View(BSM);
        //        }
        //        else
        //        {
        //            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //            return View(BSM);
        //        }
        //    }
        //    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
        //    return View(BSM);

        //}
        //#endregion
        //#region "Delete"
        //public ActionResult Delete(string id)
        //{
        //    UserSession();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    DataSelector();
        //    if (id == "null") id = null;
        //    if (id != null)
        //    {
        //        AssetRegisterObject Del = new AssetRegisterObject();
        //        string msg = Del.DeleteFixAsset(id);
        //        if (msg == SYConstant.OK)
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_RM", user.Lang);
        //        }
        //        else
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //        }
        //    }
        //    else
        //    {
        //        Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
        //    }
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        //}
        //#endregion

        //#region "Details"
        //public ActionResult Details(string id)
        //{
        //    UserSession();
        //    DataSelector();
        //    UserConfForm(SYActionBehavior.EDIT);
        //    ViewData[SYSConstant.PARAM_ID] = id;
        //    if (id == "null") id = null;
        //    if (id != null)
        //    {
        //        AssetRegisterObject BSM = new AssetRegisterObject();
        //        BSM.Header = new HRAssetRegister();
        //        BSM.Header = DB.HRAssetRegisters.FirstOrDefault(w => w.AssetCode == id);
        //        if (BSM.Header != null)
        //        {
        //            Session[Index_Sess_Obj + ActionName] = BSM;
        //            return View(BSM);
        //        }
        //    }
        //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}
        //#endregion

        #region 'Private Code'
        private void DataSelector()
        {
            ViewData["ASSETTYPE_SELECT"] = DB.MDFixAssetTypes.ToList();
            ViewData["ASSECLASS_SELECT"] = DB.MDFixAssetClasses.ToList();
        }
        #endregion
    }
}
