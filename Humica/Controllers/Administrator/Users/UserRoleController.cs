using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class UserRoleController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYR00001";
        private const string URL_SCREEN = "/Administrator/Users/UserRole/";

        private SMSystemEntity DBA = new SMSystemEntity();

        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;

        private string ActionName = "";
        public UserRoleController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {

            this.ActionName = "Index";
            DataSelector();
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LISTR, "RoleID", "UserRole");
            UserConfFormFitler();
            BSM.Filter = new Humica.EF.Models.FT.FTDocType();
            BSM.ListHeaderRole = DBA.SYRoles.OrderByDescending(w => w.CreatedOn).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(SYPermissionObject BSM)
        {

            this.ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfList(ActionBehavior.LISTR, "RoleID", "UserRole");
            UserConfFormFitler();
            BSM.Filter = new Humica.EF.Models.FT.FTDocType();
            BSM.ListHeaderRole = DBA.SYRoles.Where(w => w.UserType == BSM.HeaderRole.UserType).OrderByDescending(w => w.CreatedOn).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }


        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "RoleID", "UserRole");
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeaderRole);
        }
        #endregion

        #region "Create"
        public ActionResult Create()
        {
            DataSelector();
            this.ActionName = "Create";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConf(ActionBehavior.LIST);
            UserConfFormFitler();
            string part = SLevel.N.ToString();
            BSM.HeaderRole = new SYRole();
            BSM.TemplateType = part;
            BSM.HeaderRole.UserType = part;
            BSM.ListMenu = DBA.SYMenus.Where(w => w.IsActive == true && w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListMenuItem = DBA.SYMenuItems.Where(w => w.IsActive == true && w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListActionTemplate = DBA.SYScreenActionRows.ToList();
            BSM.ListActionName = DBA.SYActionTemplates.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(SYPermissionObject BSM)
        {
            UserSession();
            BSM.SelectPermision = Request.Form["NodeSelect"].ToString();
            string msg = BSM.createRole();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.HeaderRole.RoleID.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
        }

        public ActionResult ChangeRoleTemplate(string userType)
        {
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConf(ActionBehavior.LIST);
            UserConfFormFitler();
            string part = userType;
            BSM.TemplateType = part;
            BSM.HeaderRole = new SYRole();
            BSM.HeaderRole.UserType = part;
            BSM.ListMenu = DBA.SYMenus.Where(w => w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListMenuItem = DBA.SYMenuItems.Where(w => w.Part == part).OrderBy(w => w.InOrder).ToList();
            BSM.ListActionTemplate = DBA.SYScreenActionRows.ToList();
            BSM.ListActionName = DBA.SYActionTemplates.ToList();



            this.ActionName = "Create";
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("TreePermission", BSM);
        }

        public ActionResult TreePermission()
        {
            this.ActionName = "Create";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST, "RoleID", "UserRole");
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionObject)Session[Index_Sess_Obj + ActionName];
                BSM.ListActionName = DBA.SYActionTemplates.ToList();
            }
            return PartialView("TreePermission", BSM);
        }
        #endregion

        #region "Delete"
        public ActionResult Delete(int id)
        {
            DataSelector();
            UserSession();
            SYPermissionObject BSM = new SYPermissionObject();

            BSM.HeaderRole = DBA.SYRoles.Find(id);
            if (BSM.HeaderRole != null)
            {
                string msg = BSM.deleteRole(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("ROL_DEL", user.Lang);
                    mess.DocumentNumber = BSM.HeaderRole.RoleID.ToString();
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
        #endregion

        #region "Edit"
        public ActionResult Edit(int id)
        {
            DataSelector();
            this.ActionName = "Edit";
            SYPermissionObject BSM = new SYPermissionObject();

            BSM.HeaderRole = DBA.SYRoles.Find(id);
            if (BSM.HeaderRole != null)
            {
                UserSession();
                UserConf(ActionBehavior.LIST);
                UserConfFormFitler();
                string part = UserType.N.ToString();
                BSM.TemplateType = part;
                BSM.ListMenu = DBA.SYMenus.Where(w => w.IsActive == true && w.Part == part).OrderBy(w => w.InOrder).ToList();
                BSM.ListMenuItem = DBA.SYMenuItems.Where(w => w.IsActive == true && w.Part == part).OrderBy(w => w.InOrder).ToList();
                BSM.ListActionTemplate = DBA.SYScreenActionRows.ToList();
                BSM.ListActionName = DBA.SYActionTemplates.ToList();
                BSM.ListRoleItem = DBA.SYRoleItems.Where(w => w.RoleId == id).ToList();
                BSM.ListRoleMenu = DBA.SYRoleMenuItems.Where(w => w.RoleId == id).ToList();


                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Edit(SYPermissionObject BSM, int id)
        {
            UserSession();
            BSM.SelectPermision = Request.Form["NodeSelect"].ToString();
            string msg = BSM.editRole(id);
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.HeaderRole.RoleID.ToString();
                Session[SYConstant.MESSAGE_SUBMIT] = mess;

            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + id);

        }
        #endregion

        #region "Private Code"

        private void DataSelector()
        {
            SYDataList DL = new SYDataList("USER_TYPE");
            ViewData["USER_TYPE"] = DL.ListData;
        }

        #endregion

    }
}
