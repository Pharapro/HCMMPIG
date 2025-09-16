using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Developments
{
    public class SYListLabelController : Humica.EF.Controllers.MasterSaleController
    {
        SMSystemEntity DB = new SMSystemEntity();
        const string SCREEN_ID = "SYFRM00003";
        const string URL_SCREEN = "/Administrator/Developments/SYListLabel/";
        public SYListLabelController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            UserConf(ActionBehavior.LIST);


            SYLangObject BSM = new SYLangObject();
            BSM.ListTable = new List<CFList>();
            ViewData[SYConstant.PARAM_ID] = "";
            //BSM.ListIProvince = DB.ADProvinces.ToList();
            //BSM.ListDistrict = DB.ADDistricts.ToList();
            //BSM.ListCommune = DB.ADCommunes.ToList();
            //BSM.ListVillage = DB.ADVillages.ToList();

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(SYLangObject BSM)
        {
            UserSession();
            UserConf(ActionBehavior.LIST);

            if (BSM.ScreenID != null)
            {
                ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == BSM.ScreenID).ToList();
            }
            else
            {
                BSM.ListTable = new List<CFList>();
                ViewData[SYConstant.PARAM_ID] = "";
            }

            return View(BSM);
        }

        #endregion
        #region "List Translate"
        public ActionResult GridItems1(string screenId)
        {
            UserConf(ActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId).ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListTable);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFList Frm, string screenId)
        {
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;
                    BSM.DB.CFLists.Add(Frm);
                    DataList();
                    int row = BSM.DB.SaveChanges();
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
            DataList();
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFList Frm, string screenId)
        {
            ViewData[SYConstant.PARAM_ID] = screenId;
            SYLangObject BSM = new SYLangObject();
            if (ModelState.IsValid)
            {
                try
                {
                    Frm.ScreenId = screenId;

                    BSM.DB.CFLists.Attach(Frm);
                    BSM.DB.Entry(Frm).Property(w => w.ScreenId).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.FieldName).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Description).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.ViewAs).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Lang).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.InOrder).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.Width).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.NavigationUrl).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.KeyValueField).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.IsInvisible).IsModified = true;
                    BSM.DB.Entry(Frm).Property(w => w.NavigationUrl).IsModified = true;
                    int row = BSM.DB.SaveChanges();
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
            DataList();
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(int FormId, string screenId)
        {
            SYLangObject BSM = new SYLangObject();
            if (FormId >= 0)
            {
                try
                {
                    BSM.ScreenID = screenId;
                    ViewData[SYConstant.PARAM_ID] = BSM.ScreenID;
                    var obj = BSM.DB.CFLists.Find(FormId);
                    if (obj != null)
                    {
                        BSM.DB.CFLists.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListTable = BSM.DB.CFLists.Where(w => w.ScreenId == screenId).OrderBy(w => w.InOrder).ToList();
            return PartialView("GridItems1", BSM.ListTable);
        }
        #endregion

        #region "Private Code"
        private void DataList()
        {

            ViewData["LANG_LIST"] = DB.CFLangs.ToList();

        }
        #endregion
    }
}
