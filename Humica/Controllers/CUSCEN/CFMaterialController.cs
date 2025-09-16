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
    public class CFMaterialController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "SYNC000002";
        private const string URL_SCREEN = "/Asset/CFMaterial/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AssetCode;status";
        CUSCENDB DB = new CUSCENDB();
        public CFMaterialController()
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
            ClsMDMaterial BSM = new ClsMDMaterial();
            BSM.ListHeader = new List<MDMaterial>();
            BSM.ListMateriaClass = new List<ExMaterialClass>();
            await BSM.LoadData();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsMDMaterial BSM = new ClsMDMaterial();
            BSM.ListHeader = new List<MDMaterial>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMDMaterial)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        public ActionResult _ListMaterial()
        {
            UserSession();
            UserConfListAndForm();
            ClsMDMaterial BSM = new ClsMDMaterial();
            BSM.ListHeader = DB.MDMaterials.ToList();
            return PartialView("_ListMaterial", BSM.ListHeader);
        }
        public ActionResult _ListMaterialClass()
        {
            UserSession();
            UserConfListAndForm();
            ClsMDMaterial BSM = new ClsMDMaterial();
            BSM.ListMateriaClass = DB.ExMaterialClasses.ToList();
            return PartialView("_ListMaterialClass", BSM.ListMateriaClass);
        }

        public async Task<ActionResult> Transfer(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            var BSM = new ClsMDMaterial();
            if (!string.IsNullOrEmpty(id))
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsMDMaterial)Session[Index_Sess_Obj + ActionName];
                }
                var msg = await BSM.TransferMaterial(id);

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
        #region 'Private Code'
        private void DataSelector()
        {
        }
        #endregion
    }
}
