using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserRoleAppController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYR00002";
        private const string URL_SCREEN = "/Administrator/Users/UserRoleApp/";

        private SMSystemEntity DBA = new SMSystemEntity();
        private string KeyName = "RoleID";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        public UserRoleAppController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {

            this.ActionName = "Index";
            SYPermissionAppObject BSM = new SYPermissionAppObject();
            UserSession();
            UserConfListAndForm(this.KeyName);
            //UserConfFormFitler();
            //BSM.Filter = new Humica.EF.Models.FT.FTDocType();
            BSM.ListRoleApp = DBA.SYRoleAPPs.OrderByDescending(w => w.CreatedOn).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(SYPermissionObject BSM)
        {

            this.ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.Filter = new Humica.EF.Models.FT.FTDocType();
            //BSM.ListHeaderRole = DBA.SYRoles.Where(w => w.UserType == BSM.HeaderRole.UserType).OrderByDescending(w => w.CreatedOn).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }


        public ActionResult PartialList()
        {
            SYPermissionAppObject BSM = new SYPermissionAppObject();
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionAppObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListRoleApp);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            this.ActionName = "Create";
            SYPermissionAppObject BSM = new SYPermissionAppObject();
            UserSession();
            UserConf(ActionBehavior.LIST);
            UserConfListAndForm(this.KeyName);
            string part = SLevel.N.ToString();
            BSM.HeaderRoleApp = new SYRoleAPP();
            BSM.TemplateType = part;
            BSM.ListActionName = new List<SYActionTemplateAPP>();
            BSM.SelectedItem = new List<string>();
            var tblSYActionTemplateAPP = DBA.SYActionTemplateAPPs;
            BSM.ListActionName = tblSYActionTemplateAPP.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(SYPermissionAppObject BSM)
        {
            UserSession();
            UserConfListAndForm(this.KeyName);
            var selectedItems = Session["object"] as List<string>;
            if (selectedItems == null)
            {
                selectedItems = new List<string>();
            }
            List<SYActionTemplateAPP> items = new List<SYActionTemplateAPP>();
            BSM.ListRoleItemApp = new List<SYRoleItemAPP>();
            BSM.ActionTemplateSelected = new List<SYActionTemplateAPP>();
            items = DBA.SYActionTemplateAPPs.Where(x => selectedItems.Any(y => y == x.ScreenID))?.ToList();
            BSM.ActionTemplateSelected = items;
            string msg = BSM.createRole();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.HeaderRoleApp.RoleID.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
        }
        public ActionResult CheckedAllItems(bool IsChecked, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            Session["object"] = null;
            bool isChecked = IsChecked;
            SYPermissionAppObject BSM = new SYPermissionAppObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as SYPermissionAppObject;
                BSM.SelectedItem = new List<string>();
                BSM.ListActionName.ForEach(x => BSM.SelectedItem.Add(x.ScreenID));

                if (!IsChecked)
                {
                    BSM.SelectedItem.Clear();
                }
                Session["object"] = BSM.SelectedItem;

                var result = new
                {
                    MS = SYConstant.OK,
                    isChecked = IsChecked

                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult CheckedValue(string Value, bool IsChecked, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            SYPermissionAppObject BSM = new SYPermissionAppObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as SYPermissionAppObject;

                BSM.SelectedItem.Add(Value);
                if (!IsChecked)
                {
                    BSM.SelectedItem.RemoveAll(x => x == Value);
                }
                Session["object"] = BSM.SelectedItem;

                var result = new
                {
                    MS = SYConstant.OK
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region "Edit"
        public ActionResult Edit(int id)
        {
            this.ActionName = "Edit";
            SYPermissionAppObject BSM = new SYPermissionAppObject();
            BSM.ListActionName = new List<SYActionTemplateAPP>();
            BSM.HeaderRoleApp = new SYRoleAPP();
            BSM.ListRoleItemApp = new List<SYRoleItemAPP>();
            BSM.SelectedItem = new List<string>();
            var selectedItems = Session["object"] as List<string>;
            if (id != null)
            {
                UserSession();
                UserConf(ActionBehavior.LIST);
                UserConfFormFitler();
                BSM.HeaderRoleApp = DBA.SYRoleAPPs.Where(x => x.RoleID == id).FirstOrDefault();
                var tblSYActionTemplateAPP = DBA.SYActionTemplateAPPs;
                BSM.ListActionName = tblSYActionTemplateAPP.ToList();
                //// create object as SYRoleItemAPP
                var tblSYRoleItemApp = DBA.SYRoleItemAPPs;
                //// retriev data from table object (sYRoleItemApp) then, assign into other obj
                BSM.ListRoleItemApp = tblSYRoleItemApp.Where(x => x.RoleId == id).ToList();
                BSM.ListRoleItemApp.ForEach(x => BSM.SelectedItem.Add(x.ScreenId));
                Session["object"] = BSM.SelectedItem;
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Edit(SYPermissionAppObject BSM, int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            UserConfListAndForm(this.KeyName);
            var selectedItems = Session["object"] as List<string>;
            #region linq list in list
            //var itemsRemoved = Session["remove"] as List<string>;
            //itemsRemoved.RemoveAt(0);
            //if(selectedItems != null)
            //{
            //    selectedItems= selectedItems.Distinct().ToList();
            //    if(itemsRemoved != null)
            //    {
            //        selectedItems = selectedItems.Where(x=>!itemsRemoved.Any(w=>w==x)).ToList(); 
            //    }
            //}
            #endregion
            var tblSYActionTemplate = DBA.SYActionTemplateAPPs;
            var items = tblSYActionTemplate.Where(x => selectedItems.Any(y => y == x.ScreenID))?.ToList();
            var tblSYRoleItemApp = DBA.SYRoleItemAPPs;
            BSM.ListRoleItemApp = new List<SYRoleItemAPP>();
            BSM.ActionTemplateSelected = new List<SYActionTemplateAPP>();
            BSM.ActionTemplateSelected = items;
            BSM.ListRoleItemApp = tblSYRoleItemApp.Where(x => x.RoleId == id).ToList();
            string msg = BSM.editRole(id);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id.ToString();
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);

        }

        #endregion

        //#region "Delete"
        public ActionResult Delete(int id)
        {
            UserSession();
            SYPermissionAppObject BSM = new SYPermissionAppObject();

            BSM.HeaderRoleApp = DBA.SYRoleAPPs.Find(id);
            if (BSM.HeaderRoleApp != null)
            {
                string msg = BSM.deleteRole(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("ROL_DEL", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                    mess.Description = mess.Description + BSM.ErrorMessage;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        //#endregion
    }
}
