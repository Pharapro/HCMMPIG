using DevExpress.Web.Mvc;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Users
{
    public class ActionTempalateController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYUP00002";
        private const string URL_SCREEN = "/Administrator/Users/ActionTempalate/";

        private SMSystemEntity DBA = new SMSystemEntity();

        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;

        private string ActionName = "";
        public ActionTempalateController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            //DataSelector();
            this.ActionName = "Index";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST, "TemplateID", "ActionTempalate");
            UserConfFormFitler();
            BSM.ListActionTemplate = DBA.SYScreenActionRows.OrderBy(w => w.InOrder).ToList();
            BSM.generateListAction();
            if (BSM.ListActionTemplate.Count == 0)
            {
                BSM.ListAction = new List<SYACTemplate>();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "TemplateID", "ActionTempalate");
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListAction);
        }
        #endregion
        #region "Create"
        public ActionResult Create()
        {
            this.ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.SAVEGRID);
            SYPermissionObject BSM = new SYPermissionObject();
            BSM.ListActionTemplate = new List<SYScreenActionRow>();
            if (Request.QueryString["id"] != null)
            {
                BSM.TemplateID = Request.QueryString["id"].ToString();
                BSM.ListActionTemplate = DBA.SYScreenActionRows.Where(w => w.TemplateID == BSM.TemplateID).OrderBy(w => w.InOrder).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[SYConstant.PARAM_ID] = BSM.TemplateID;
            return View(BSM);
        }

        [HttpPost]
        public string Create(SYPermissionObject BSM)
        {
            BSM.ListActionTemplate = new List<SYScreenActionRow>();
            if (Request.QueryString["id"] != null)
            {
                BSM.TemplateID = Request.QueryString["id"].ToString();
                BSM.ListActionTemplate = DBA.SYScreenActionRows.Where(w => w.TemplateID == BSM.TemplateID).OrderBy(w => w.InOrder).ToList();
                return SYConstant.OK;
            }
            else
            {
                if (ModelState.IsValid)
                {
                    this.ActionName = "Create";
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    return SYConstant.OK;
                }
                return SYConstant.FAIL;
            }


        }

        public ActionResult GridItems()
        {
            this.ActionName = "Create";
            SYPermissionObject BSM = new SYPermissionObject();
            UserSession();
            UserConfForm(ActionBehavior.SAVEGRID);
            UserConfFormFitler();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (SYPermissionObject)Session[Index_Sess_Obj + ActionName];
            }
            //if (Request.QueryString["id"] != null)
            //{
            //    BSM.TemplateID = Request.QueryString["id"].ToString();
            //    BSM.ListActionTemplate = DBA.SYScreenActionRows.Where(w => w.TemplateID == BSM.TemplateID).OrderBy(w => w.InOrder).ToList();
            //}
            ViewData[SYConstant.PARAM_ID] = BSM.TemplateID;
            return PartialView("GridItems", BSM);
        }
        public ActionResult BatchEditingUpdateModelAdd(MVCxGridViewBatchUpdateValues<SYScreenActionRow, int> updateValues, string id)
        {
            //sess user profile
            this.ActionName = "Create";
            UserSession();
            SYPermissionObject BSM = new SYPermissionObject();

            try
            {
                if (id == null)
                {
                    BSM = (SYPermissionObject)Session[Index_Sess_Obj + this.ActionName];
                    if (BSM != null)
                    {
                        id = BSM.TemplateID;
                    }
                }

                if (id != null)
                {
                    SMSystemEntity DBU = new SMSystemEntity();

                    BSM.ScreenId = SCREEN_ID;

                    foreach (var read in updateValues.Insert)
                    {
                        read.TemplateID = id;
                        DBU.SYScreenActionRows.Add(read);
                    }
                    foreach (var read in updateValues.Update)
                    {
                        read.TemplateID = id;
                        DBU.SYScreenActionRows.Attach(read);
                        DBU.Entry(read).State = System.Data.Entity.EntityState.Modified;
                    }
                    foreach (var read in updateValues.DeleteKeys)
                    {
                        List<SYScreenActionRow> objDel = DBA.SYScreenActionRows.Where(w => w.ID == read).ToList();
                        if (objDel.Count > 0)
                        {
                            DBU.Entry(objDel.First()).State = System.Data.Entity.EntityState.Deleted;
                        }

                    }

                    int row = DBU.SaveChanges();

                    string msg = "";
                    if (row > 0)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.TemplateID;
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                        Session[Index_Sess_Obj] = null;
                    }
                    else
                    {
                        SYMessages mess = SYMessages.getMessageObject(msg, user.Lang);
                        Session[Index_Sess_Obj + this.ActionName] = null;
                        DataSelector();
                        UserConfForm(ActionBehavior.SAVEGRID);
                        if (BSM.ListActionTemplate.Count > 1)
                        {
                            for (var i = 0; i < BSM.ListActionTemplate.Count - 1; i++)
                            {
                                updateValues.SetErrorText(BSM.ListActionTemplate[i], "");
                            }
                        }
                        updateValues.SetErrorText(BSM.ListActionTemplate.Last(), mess.Description);


                        return PartialView("GridItems", BSM);
                    }


                }

            }
            catch
            {

            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create?id=" + id);

        }
        #endregion
        #region "Private Code"
        private void DataSelector()
        {
            SYDataList DL = new SYDataList("LINK_MODE");
            ViewData["LINK_MODE"] = DL.ListData;

            DL = new SYDataList("ICON_BUTTON");
            ViewData["ICON_BUTTON"] = DL.ListData;

            DL = new SYDataList("ISACTION_LIST");
            ViewData["ISACTION_LIST"] = DL.ListData;

            DL = new SYDataList("ACTION_CLICK");
            ViewData["ACTION_CLICK"] = DL.ListData;

        }



        #endregion
    }
}
