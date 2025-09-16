using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserPermissionTemplateController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYUP00001";
        private const string URL_SCREEN = "/Administrator/Users/UserPermissionTemplate/";

        private SMSystemEntity DBA = new SMSystemEntity();

        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;

        private string ActionName = "";

        public UserPermissionTemplateController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            DataSelector();
            this.ActionName = "Index";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST, "UserName", "UserPermissionTemplate");
            UserConfFormFitler();
            string part = SLevel.N.ToString();
            BSM.TemplateType = part;
            BSM.ListMenu = DBA.SYMenus.Where(w => w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListMenuItem = DBA.SYMenuItems.Where(w => w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListActionTemplate = DBA.SYScreenActionRows.ToList();
            BSM.ListActionName = DBA.SYActionTemplates.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(SYPermissionObject BSM)
        {
            DataSelector();
            this.ActionName = "Index";
            UserSession();
            UserConfList(ActionBehavior.LIST, "UserName", "UserPermissionTemplate");
            UserConfFormFitler();
            string part = BSM.TemplateType;
            BSM.TemplateType = part;
            BSM.ListMenu = DBA.SYMenus.Where(w => w.Part == part).ToList();
            BSM.ListMenuItem = DBA.SYMenuItems.Where(w => w.Part == part).ToList();
            BSM.ListActionTemplate = DBA.SYScreenActionRows.ToList();
            BSM.ListActionName = DBA.SYActionTemplates.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult TreePermission()
        {
            this.ActionName = "Index";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST, "UserName", "UserPermissionTemplate");
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListActionName = DBA.SYActionTemplates.ToList();
            }
            return PartialView("TreePermission", BSM);
        }
        #endregion

        #region "InsertTemplate"
        [HttpPost]
        public string SaveTemplateAction(string ScreenId, string ActionName, string TemplateID)
        {
            try
            {
                SYActionTemplate obj = new SYActionTemplate();
                string[] sp = ScreenId.Split(':');
                if (sp.Length == 2)
                {
                    obj.ScreenID = sp[0];
                    obj.ActionName = ActionName;
                    obj.ActionTemplateID = TemplateID;
                    obj.Description = sp[1];
                    DBA.SYActionTemplates.Add(obj);
                    int row = DBA.SaveChanges();
                    return SYConstant.OK;
                }
            }
            catch
            {
                return SYConstant.FAIL;
            }
            return SYConstant.FAIL;
        }
        #endregion


        #region "Delete Template"
        [HttpPost]
        public string DeleteTemplateAction(string selectKeys)
        {
            try
            {
                if (Request.QueryString["selectKeys"] != null)
                {
                    selectKeys = Request.QueryString["selectKeys"].ToString();
                }
                SYActionTemplate obj = new SYActionTemplate();
                string[] sp = selectKeys.Split(':');
                if (sp.Length == 2)
                {
                    obj = DBA.SYActionTemplates.Find(sp[0], sp[1]);
                    DBA.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    int row = DBA.SaveChanges();
                    return SYConstant.OK;

                }
                return SYConstant.FAIL;
            }
            catch
            {
                return SYConstant.FAIL;
            }

        }
        #endregion

        #region "Private Code"
        public void DataSelector()
        {
            List<SYScreenActionRow> ListAction = DBA.SYScreenActionRows.ToList();

            List<SYDocumentNo> ListDoc = new List<SYDocumentNo>();
            foreach (var read in ListAction.GroupBy(w => w.TemplateID))
            {
                SYDocumentNo obj = new SYDocumentNo();
                obj.DocumentNo = read.Key;
                ListDoc.Add(obj);
            }

            ViewData["ACTION_LIST"] = ListDoc;
            SYDataList DL = new SYDataList("USER_TYPE");
            ViewData["USER_TYPE"] = DL.ListData;
        }
        #endregion

    }

}
