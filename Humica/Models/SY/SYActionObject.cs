using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Humica.Models.SY
{
    public class SYActionObject
    {
        //SMSystemEntity DB = new SMSystemEntity();
        Humica.EF.MD.SMSystemEntity DB = new Humica.EF.MD.SMSystemEntity();
        public SYActionObject()
        {
            //if (HttpContext.Current != null && HttpContext.Current.Session != null)
            //{
            //    if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION] != null)
            //      {

            //     }                                    
            //}
        }
        public List<SYUserAction> getListAction(string ScreenId)
        {
            List<SYUserAction> ListStore = new List<SYUserAction>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION] != null)
                {
                    ListStore = (List<SYUserAction>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION];
                }
            }

            List<SYUserAction> actions = ListStore.Where(w => w.ScreenID == ScreenId && w.IsGroup != true).ToList();

            return actions;

        }

        public string getTitle(string ScreenId, string ActionName)
        {
            string title = "";

            var listMenu = DB.SYMenuItems.ToList();

            if (listMenu.Where(w => w.ScreenId == ScreenId).ToList().Count > 0)
            {
                title = listMenu.Where(w => w.ScreenId == ScreenId).First().Text;
            }

            if (ActionName != "Index")
            {
                List<SYActionTemplate> ListStore = new List<SYActionTemplate>();

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE] != null)
                    {
                        ListStore = (List<SYActionTemplate>)HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE];
                    }
                }

                List<SYActionTemplate> actions = ListStore.Where(w => w.ScreenID == ScreenId && w.ActionName == ActionName).ToList();
                if (actions.Count > 0)
                {
                    title = title + " > " + SYSettings.getLabelAction(actions.First().ActionName);
                }
            }

            return title;


        }
        public string getTitleScreen(string ScreenId, string ActionName)
        {
            string title = "";

            var listMenu = (List<SYMenuItem>)HttpContext.Current.Session[SYConstant.MENU_SUB_HEADER_SESSION];

            if (listMenu.Where(w => w.ScreenId == ScreenId).ToList().Count > 0)
            {
                title = listMenu.Where(w => w.ScreenId == ScreenId).First().Text;
            }

            if (ActionName != "Index")
            {
                List<SYActionTemplate> ListStore = new List<SYActionTemplate>();

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE] != null)
                    {
                        ListStore = (List<SYActionTemplate>)HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE];
                    }
                }

                List<SYActionTemplate> actions = ListStore.Where(w => w.ScreenID == ScreenId && w.ActionName == ActionName).ToList();
                if (actions.Count > 0)
                {
                    title = title;
                }
            }

            return title;


        }

        public string getTitle(string ScreenId)
        {
            List<SYMenuItem> ListStore = new List<SYMenuItem>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.MENU_SUB_HEADER_SESSION] != null)
                {
                    ListStore = (List<SYMenuItem>)HttpContext.Current.Session[SYConstant.MENU_SUB_HEADER_SESSION];
                }
            }

            List<SYMenuItem> actions = ListStore.Where(w => w.ScreenId == ScreenId).ToList();
            if (actions.Count > 0)
            {
                string text = actions.First().Text;

                return text;
            }
            return "";

        }

        public static string getTitleScreen(string ScreenId)
        {
            List<SYMenuItem> ListStore = new List<SYMenuItem>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.MENU_SUB_HEADER_SESSION] != null)
                {
                    ListStore = (List<SYMenuItem>)HttpContext.Current.Session[SYConstant.MENU_SUB_HEADER_SESSION];
                }
            }

            List<SYMenuItem> actions = ListStore.Where(w => w.ScreenId == ScreenId).ToList();
            if (actions.Count > 0)
            {
                string text = actions.First().Text;

                return text;
            }
            return "";
        }
        public List<SYActionTemplate> getListActionTemplate(string ScreenId, string ActionName)
        {
            List<SYActionTemplate> ListStore = new List<SYActionTemplate>();
            List<SYRoleItem> ListRoleItem = new List<SYRoleItem>();

            if (ActionName == "DocumentViewerPartial")
            {
                var obj = new SYActionTemplate();
                obj.ScreenID = ScreenId;
                obj.ActionTemplateID = "NONE";
                obj.Description = "";
                obj.ActionName = ActionName;
                var objList = new List<SYActionTemplate>();
                objList.Add(obj);
                return objList;

            }

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYActionTemplate>)HttpContext.Current.Session[SYConstant.ACTION_ACTION_SESSION_TEMPLATE];

                    List<SYRoleItem> ListRole = (List<SYRoleItem>)HttpContext.Current.Session[SYSConstant.LIST_AUTTH_ROLE];
                    ListRoleItem = ListRole.Where(w => w.ScreenId == ScreenId && w.ActionName == ActionName).ToList();
                }
            }

            List<SYActionTemplate> actions = ListStore.Where(w => w.ScreenID == ScreenId && w.ActionName == ActionName).ToList();

            //List<SYActionTemplate> auth = new List<SYActionTemplate>();
            //foreach(var read in actions)
            //{
            //    if(ListRoleItem.Where(w=>w.ActionName==read.ActionName && w.ScreenId==read.ScreenID && w.ActionTemplateID==read.ActionTemplateID).ToList().Count>0)
            //    {
            //        auth.Add(read);
            //    }
            //}
            //return auth;
            return actions;
        }
        public List<SYUserAction> getListGroupAction(string ScreenId)
        {
            List<SYUserAction> ListStore = new List<SYUserAction>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION] != null)
                {
                    ListStore = (List<SYUserAction>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION];
                }
            }

            List<SYUserAction> actions = ListStore.Where(w => w.ScreenID == ScreenId && w.IsGroup == true).ToList();

            return actions;

        }

        public List<SYScreenActionRow> getDefaultObjectTemplate()
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];
                }
            }
            List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == "_DEFAULT").ToList();

            return actions;
        }


        public string getText(string TemplateID, string actionName)
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];
                }
            }

            //if (HttpContext.Current.Request.QueryString["rpt"] != null)
            //{
            //    string rpt = HttpContext.Current.Request.QueryString["rpt"].ToString().ToUpper();
            //    rpt="Is"+rpt;
            //    List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == "LIST_ACTION_IMPORT_REQUEST_DP").ToList();

            //    foreach (var read in actions)
            //    {
            //        if (read.IsAction.ToUpper() == rpt.ToUpper())
            //        {
            //            return read.Text;
            //        }
            //    }
            //}           



            return null;
        }


        public List<SYScreenActionRow> getObjectTemplateByTemplateIdWithoutGroup(string templateID, string screenId)
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();

            List<SYRoleItem> ListRoleItem = new List<SYRoleItem>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];

                    List<SYRoleItem> ListRole = (List<SYRoleItem>)HttpContext.Current.Session[SYSConstant.LIST_AUTTH_ROLE];

                    ListRoleItem = ListRole.Where(w => w.ScreenId == screenId).ToList();

                }
            }

            List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == templateID
                && (w.GroupAction == "" || w.GroupAction == null)
                ).OrderBy(w => w.InOrder).ToList();

            List<SYScreenActionRow> auth = new List<SYScreenActionRow>();
            foreach (var read in actions)
            {

                if (
                    read.IsAction == "IsBack" || read.IsAction == "IsSave"
                   )
                {
                    auth.Add(read);
                }
                else
                {
                    foreach (var com in ListRoleItem)
                    {
                        if (read.ActionInCode == com.ActionName)
                        {
                            if (HttpContext.Current.Session[SYConstant.SYSTEM_LANG] != null)
                            {
                                List<SYLang> listLang = (List<SYLang>)HttpContext.Current.Session[SYConstant.SYSTEM_LANG];
                                if (listLang.Where(w => w.ScreenId == com.ScreenId && w.ObjectId == com.ActionName).ToList().Count > 0)
                                {
                                    read.Text = listLang.Where(w => w.ScreenId == com.ScreenId && w.ObjectId == com.ActionName).First().Description;
                                }
                            }


                            auth.Add(read);
                        }
                    }
                }

            }


            return auth;

        }

        public List<SYScreenActionRow> getObjectTemplateByTemplateIdWithGroup(string templateID, string screenId)
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();
            List<SYRoleItem> ListRoleItem = new List<SYRoleItem>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];

                    List<SYRoleItem> ListRole = (List<SYRoleItem>)HttpContext.Current.Session[SYSConstant.LIST_AUTTH_ROLE];

                    ListRoleItem = ListRole.Where(w => w.ScreenId == screenId).ToList();
                }
            }

            List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == templateID
                && w.GroupAction != "" && w.GroupAction != null
                ).OrderBy(w => w.InOrder).ToList();

            List<SYScreenActionRow> auth = new List<SYScreenActionRow>();
            foreach (var read in actions)
            {

                if (
                    read.IsAction == "IsBack" || read.IsAction == "IsRun"
                   )
                {
                    auth.Add(read);
                }
                else
                {
                    foreach (var com in ListRoleItem)
                    {
                        if (read.ActionInCode == com.ActionName)
                        {

                            if (HttpContext.Current.Session[SYConstant.SYSTEM_LANG] != null)
                            {
                                List<SYLang> listLang = (List<SYLang>)HttpContext.Current.Session[SYConstant.SYSTEM_LANG];
                                if (listLang.Where(w => w.ScreenId == com.ScreenId && w.ObjectId == com.ActionName).ToList().Count > 0)
                                {
                                    read.Text = listLang.Where(w => w.ScreenId == com.ScreenId && w.ObjectId == com.ActionName).First().Description;
                                }
                            }

                            auth.Add(read);
                        }
                    }
                }

            }
            return auth;


        }


        public SYScreenActionRow getObjectTemplateUnGroup(SYUserAction UserActions, string ActionName)
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];
                }
            }
            foreach (var read in ListStore)
            {
                if (read.TemplateID.Equals(UserActions.TemplateID) && read.IsAction.Equals(UserActions.ActionID) && read.ActionName.Equals(ActionName))
                {
                    if (read.GroupAction == "" || read.GroupAction == null)
                    {
                        return read;
                    }
                }
            }
            //List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == UserActions.TemplateID
            //    && w.IsAction == UserActions.ActionID
            //    && w.ActionName == ActionName
            //    && (w.GroupAction == "" || w.GroupAction == null)
            //    ).ToList();
            //if (actions.Count > 0)
            //{
            //    return actions.First();
            //}
            return null;
        }

        public List<SYScreenActionRow> getObjectTemplate(string TemplateID, string ActionName)
        {
            List<SYScreenActionRow> ListStore = new List<SYScreenActionRow>();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE] != null)
                {
                    ListStore = (List<SYScreenActionRow>)HttpContext.Current.Session[SYConstant.ACTION_PERMISSION_SESSION_TEMPLATE];
                }
            }
            List<SYScreenActionRow> actions = ListStore.Where(w => w.TemplateID == TemplateID &&
                w.ActionName == ActionName
                && w.GroupAction != "" && w.GroupAction != null).ToList();
            return actions;
        }

        public bool IsExistAction(List<SYScreenActionRow> Templates, List<SYUserAction> Actions, string key)
        {
            foreach (var read in Templates.GroupBy(w => w.GroupAction))
            {
                if (read.Key == key)
                {
                    foreach (var action in Actions)
                    {
                        if (Templates.Where(w => w.IsAction == action.ActionID).ToList().Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsActionAuthorized(string ActionName, string ScreenId)
        {
            var ListRoleItem = (List<SYRoleItem>)HttpContext.Current.Session[SYSConstant.LIST_AUTTH_ROLE];
            if (ListRoleItem.Where(w => w.ScreenId == ScreenId && w.ActionName == ActionName).ToList().Count == 0)
            {
                return false;
            }
            return true;
        }

        public string getIconObject(string iConBtn)
        {
            switch (iConBtn)
            {
                case "ActionsFill16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsFill16x16;//IsPost
                case "ActionsUp216x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsUp216x16;//IsRelease
                case "ActionsReset16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsReset16x16;//IsReverse
                case "ActionsAdd16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsAdd16x16;//IsAdd
                case "EditEdit16x16":
                    return DevExpress.Web.ASPxThemes.IconID.EditEdit16x16;//IsEdit
                case "ExportExporttoxlsx16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ExportExporttoxlsx16x16;//IsExport
                case "ActionsShow16x16office2013":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsShow16x16office2013;//IsView
                case "ContentCheckbox16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ContentCheckbox16x16;//IsApprove
                case "ActionsCancel16x16gray":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsCancel16x16gray;//IsCancel
                case "NavigationBackward16x16":
                    return DevExpress.Web.ASPxThemes.IconID.NavigationBackward16x16;//IsBack
                case "SaveSave16x16":
                    return DevExpress.Web.ASPxThemes.IconID.SaveSave16x16;//IsSave
                case "ActionsCancel16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsCancel16x16;//IsDelete                
                case "ActionsPrint16x16devav":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsPrint16x16devav;//IsPrint    
                case "ActionsMerge16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ActionsMerge16x16;//IsGenerate                
                case "ArrowsFirst16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ArrowsFirst16x16;//IsFirst       
                case "ArrowsPrev16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ArrowsPrev16x16;//IsPrevious       
                case "ArrowsNext16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ArrowsNext16x16;//IsNext       
                case "ArrowsLast16x16":
                    return DevExpress.Web.ASPxThemes.IconID.ArrowsLast16x16;//IsLast
                case "PeopleAssignto16x16":
                    return DevExpress.Web.ASPxThemes.IconID.PeopleAssignto16x16;//IsRequest
                case "SetupProperties16x16":
                    return DevExpress.Web.ASPxThemes.IconID.SetupProperties16x16;//IsSetup
                case "FormatListbullets16x16":
                    return DevExpress.Web.ASPxThemes.IconID.FormatListbullets16x16;//IsList
                case "	DashboardsLocknavigation16x16":
                    return DevExpress.Web.ASPxThemes.IconID.DashboardsLocknavigation16x16;//IsLock
            }

            return "";
        }


        public string getIconObjectSecondSpace(string iConBtn)
        {
            switch (iConBtn)
            {
                case "ActionsFill16x16":
                    return "e040";//IsPost
                case "ActionsUp216x16":
                    return "e123";//IsRelease
                case "ActionsReset16x16":
                    return "0053";//IsReverse
                case "ActionsAdd16x16":
                    return "e03e";//IsAdd
                case "EditEdit16x16":
                    return "0025";//IsEdit
                case "ExportExporttoxlsx16x16":
                    return "e0c7";//IsExport
                case "ActionsShow16x16office2013":
                    return "e025";//IsView
                case "ContentCheckbox16x16":
                    return "e03c";//IsApprove
                case "ActionsCancel16x16gray":
                    return "e131";//IsCancel
                case "NavigationBackward16x16":
                    return "e05d";//IsBack
                case "SaveSave16x16":
                    return "e040";//IsSave
                case "ActionsCancel16x16":
                    return "e001";//IsDelete                
                case "ActionsPrint16x16devav":
                    return "e14c";//IsPrint    
                case "ActionsMerge16x16":
                    return "e170";//IsGenerate                
                case "ArrowsFirst16x16":
                    return "e1c5";//IsFirst       
                case "ArrowsPrev16x16":
                    return "e0f4";//IsPrevious       
                case "ArrowsNext16x16":
                    return "e043";//IsNext       
                case "ArrowsLast16x16":
                    return "e04f";//IsLast
                case "PeopleAssignto16x16":
                    return "0041";//IsRequest
                case "DashboardsLocknavigation16x16":
                    return "006c";//IsLock
            }

            return "";
        }
        public string getIconObjectIconURL(string iConBtn)
        {
            switch (iConBtn)
            {
                case "ActionsAdd16x16":
                    return "/Content/Images/add.svg";
                case "ActionsCancel16x16":
                    return "/Content/Images/delete.svg";
                case "ActionsCancel16x16gray":
                    return "/Content/Images/cancel.svg";
                case "ActionsFill16x16":
                    return "/Content/Images/post.svg";
                case "ActionsMerge16x16":
                    return "/Content/Images/generate.svg";
                case "ActionsPrint16x16devav":
                    return "/Content/Images/print.svg";
                case "ActionsReset16x16":
                    return "/Content/Images/reverse.svg";
                case "ActionsShow16x16office2013":
                    return "/Content/Images/view.svg";
                case "ActionsUp216x16":
                    return "/Content/Images/release.svg";
                case "ArrowsFirst16x16":
                    return "/Content/Images/first.svg";
                case "ArrowsLast16x16":
                    return "/Content/Images/last.svg";
                case "ArrowsNext16x16":
                    return "/Content/Images/next.svg";
                case "ArrowsPrev16x16":
                    return "/Content/Images/previous.svg";
                case "ContentCheckbox16x16":
                    return "/Content/Images/approve.svg";
                case "CopyCopy16x16":
                    return "/Content/Images/Copy.svg";
                case "DashboardsLocknavigation16x16":
                    return "/Content/Images/lock.svg";
                case "EditEdit16x16":
                    return "/Content/Images/edit.svg";
                case "ExportExporttoxlsx16x16":
                    return "/Content/Images/Import.svg";
                case "export":
                    return "/Content/Images/export.svg";
                case "FormatListbullets16x16":
                    return "/Content/Images/list.svg";
                case "NavigationBackward16x16":
                    return "/Content/Images/back.svg";
                case "PeopleAssignto16x16":
                    return "/Content/Images/request.svg";
                case "SaveSave16x16":
                    return "/Content/Images/save.svg";
                case "Contect":
                    return "/Content/Images/Contect.svg";
                case "Change":
                    return "/Content/Images/Change.svg";
                case "announcement":
                    return "/Content/Images/announcement.svg";
                case "check":
                    return "/Content/Images/check.svg";
                case "Interview":
                    return "/Content/Images/Interview.svg";
                case "calendar":
                    return "/Content/Images/calendar.svg";
                default:
                    return "";
            }
        }


        public string getIconObjectThirdSpace(string iConBtn)
        {
            //switch (iConBtn)
            //{
            //    case "ActionsFill16x16":
            //        return "entypo-upload";//IsPost
            //    case "ActionsUp216x16":
            //        return "entypo-check";//IsRelease
            //    case "ActionsReset16x16":
            //        return "entypo-back-in-time";//IsReverse
            //    case "ActionsAdd16x16":
            //        return "entypo-plus-squared";//IsAdd
            //    case "EditEdit16x16":
            //        return "entypo-pencil";//IsEdit
            //    case "ExportExporttoxlsx16x16":
            //        return "entypo-export";//IsExport
            //    case "ActionsShow16x16office2013":
            //        return "entypo-eye";//IsView
            //    case "ContentCheckbox16x16":
            //        return "entypo-right-dir";//IsApprove
            //    case "ActionsCancel16x16gray":
            //        return "entypo-cancel";//IsCancel
            //    case "NavigationBackward16x16":
            //        return "entypo-back";//IsBack
            //    case "SaveSave16x16":
            //        return "entypo-upload";//IsSave
            //    case "ActionsCancel16x16":
            //        return "entypo-minus-circled";//IsDelete                
            //    case "ActionsPrint16x16devav":
            //        return "entypo-print";//IsPrint    
            //    case "ActionsMerge16x16":
            //        return "entypo-clock";//IsGenerate                
            //    case "ArrowsFirst16x16":
            //        return "e1c5";//IsFirst       
            //    case "ArrowsPrev16x16":
            //        return "e0f4";//IsPrevious       
            //    case "ArrowsNext16x16":
            //        return "e043";//IsNext       
            //    case "ArrowsLast16x16":
            //        return "e04f";//IsLast
            //    case "PeopleAssignto16x16":
            //        return "0041";//IsRequest
            //    case "DashboardsLocknavigation16x16":
            //        return "entypo-lock";//IsLock
            //}

            return iConBtn;
        }

        public DevExpress.Web.ButtonRenderMode getButtonMode(string btnMode)
        {
            if (btnMode == "L")
            {
                return DevExpress.Web.ButtonRenderMode.Link;
            }
            return DevExpress.Web.ButtonRenderMode.Button;
        }

        public static string GetError(System.Web.Mvc.ModelStateDictionary ModelState)
        {
            var errors = ModelState.Where(n => n.Value.Errors.Count > 0).ToList();
            foreach (var read in errors)
            {
                return read.Key.Split('.')[1] + ": " + read.Value.Errors[0].ErrorMessage;
            }
            return null;
        }
    }
}