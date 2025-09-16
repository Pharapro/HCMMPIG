using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.DS;
using Humica.Logic.MD;
using Humica.Models.Report.HRM;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Dress
{
    public class HRDressDeductController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "DS0000001";
        private const string URL_SCREEN = "/HRM/Dress/HRDressDeduct/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TranNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string _DOCTYPE_ = "_DOCTYPE2_";
        private string _LOCATION_ = "_LOCATION_";
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public HRDressDeductController()
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
            DataList();
            UserConfListAndForm(this.KeyName);

            DressDeductObject BSM = new DressDeductObject();
            BSM.FInYear = new Humica.Core.FT.FTINYear();
            BSM.FInYear.INYear = DateTime.Now.Year;
            BSM.ListHeader = new List<HRDressDeduct>();
            BSM.ListItem = new List<HRDressDeductItem>();

            BSM.ListHeader = DB.HRDressDeducts.ToList();
            BSM.FInYear.Status = SYDocumentStatus.APPROVED.ToString();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DressDeductObject BSM = new DressDeductObject();
            BSM.ListHeader = new List<HRDressDeduct>();
            BSM.ListItem = new List<HRDressDeductItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region "Create"

        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfListAndForm();
            DressDeductObject BSM = new DressDeductObject();
            BSM.ListItem = new List<HRDressDeductItem>();
            BSM.User = SYSession.getSessionUser();
            BSM.ListReceive = new List<HRDressReceiveItem>();
            BSM.Header = new HRDressDeduct();
            BSM.HeaderReceive = new HRDressReceive();
            BSM.Header.DeductDate = DateTime.Now;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(DressDeductObject collection)
        {

            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            var BSM = new DressDeductObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (ModelState.IsValid)
            {
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateDR();
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.DocNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return View(BSM);
                }
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }

        #endregion
        #region "Edit"
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressDeductObject BSM = new DressDeductObject();
            string re = id;
            if (id != null)
            {

                BSM.Header = new HRDressDeduct();
                BSM.HeaderStaff = new HR_STAFF_VIEW();
                BSM.HeaderST_P = new HRStaffProfile();
                BSM.ListHeader = new List<HRDressDeduct>();
                BSM.ListItem = new List<HRDressDeductItem>();
                BSM.Header.DocNo = id;
                BSM.Header = DB.HRDressDeducts.FirstOrDefault(x => x.DocNo == id);
                var obj = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == BSM.Header.EmpCode);
                BSM.Header.EmpCode = obj.EmpCode;
                BSM.HeaderStaff.AllName = obj.AllName;
                BSM.ListItem = DB.HRDressDeductItems.Where(w => w.DocNo == id).ToList();
                BSM.ListDelete = new List<DeleteItem>();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        [HttpPost]
        public ActionResult Edit(string id, DressDeductObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataList();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            DressDeductObject BSM = new DressDeductObject();
            if (id != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                if (Session[PATH_FILE] != null)
                {
                    collection.Header.PathFile = Session[PATH_FILE].ToString();
                    Session[PATH_FILE] = null;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditDR(id);
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
        #region "Delete"
        public ActionResult Delete(string id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();

            if (id != null)
            {
                DressDeductObject Del = new DressDeductObject();

                string msg = Del.DeleteDR(id);
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
        #region ActionGrid
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressDeductObject BSM = new DressDeductObject();
            BSM.ListItem = new List<HRDressDeductItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            return PartialView("GridItems", BSM.ListItem);
        }

        public ActionResult EditItem(HRDressDeductItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressDeductObject BSM = new DressDeductObject();

            BSM.ListItem = new List<HRDressDeductItem>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var Listitem = BSM.ListItem.Where(w => w.ID == ModelObject.ID).ToList();

                    if (Listitem.Count > 0)
                    {
                        var objUpdate = Listitem.First();
                        decimal Amount = 0;
                        foreach (var item in BSM.ListItem)
                        {
                            Amount += ModelObject.Amount;
                        }
                        BSM.Header = new HRDressDeduct();
                        BSM.Header.DedAm = Amount;
                        var msg = BSM.isValidItem(ModelObject, EnumActionGridLine.Add);
                        if (msg == SYConstant.OK)
                        {
                            objUpdate.Qty = ModelObject.Qty;
                            objUpdate.Amount = ModelObject.Amount;
                            //if (objUpdate.Amount > 0)
                            //{
                            //    BSM.Header.DedAm = objUpdate.Amount;
                            //}
                            objUpdate.Description = ModelObject.Description;
                            Session[Index_Sess_Obj + ActionName] = BSM;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage(msg);
                        }

                    }
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
            return PartialView("GridItems", BSM.ListItem);
        }
        public ActionResult DeleteItem(int ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DressDeductObject BSM = new DressDeductObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];
                    }
                    BSM.ListDelete = new List<DeleteItem>();
                    var _ListDelete = new DeleteItem();
                    BSM.ListItem.Where(w => w.ID == ID).ToList();
                    if (BSM.ListItem.Count > 0)
                    {
                        var objDel = BSM.ListItem.Where(w => w.ID == ID).First();
                        string id = objDel.ReceivedItemID.ToString();
                        _ListDelete.id = objDel.ID;
                        _ListDelete.DocNO = objDel.DocNo;
                        _ListDelete.description = objDel.Description;
                        _ListDelete.Qty = objDel.Qty;
                        _ListDelete.ReceiveID = id;
                        BSM.ListDelete.Add(_ListDelete);
                        BSM.ListItem.Remove(objDel);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("APPROVER_NE");
                    }
                    Session[Index_Sess_Obj + ActionName] = BSM;

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

            return PartialView("GridItems", BSM.ListItem);
        }

        #endregion ActionGrid
        #region "Details"
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataList();
            ViewData[SYSConstant.PARAM_ID] = ID;
            ViewData[ClsConstant.IS_READ_ONLY] = true;
            if (ID != null)
            {
                DressDeductObject BSM = new DressDeductObject();
                BSM.Header = DB.HRDressDeducts.FirstOrDefault(w => w.DocNo == ID);
                BSM.HeaderStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSM.Header.EmpCode);
                var resualt = DB.HRDressDeducts;
                List<HRDressDeduct> listEmpFa = resualt.Where(x => x.EmpCode == BSM.Header.EmpCode).ToList();
                BSM.ListHeader = listEmpFa.OrderByDescending(x => x.DocNo).ToList();
                if (BSM.Header != null)
                {
                    BSM.ListItem = DB.HRDressDeductItems.Where(w => w.DocNo == ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region "Print"
        public ActionResult Print(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string id)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            var SD = DB.HRDressDeducts.FirstOrDefault(w => w.DocNo == id);
            if (SD != null)
            {
                try
                {
                    ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                    string Company = SMS.SYHRCompanies.FirstOrDefault().CompENG;
                    FRM_DressDeduct reportModel = new FRM_DressDeduct();

                    reportModel.Parameters["DocNo"].Value = SD.DocNo;
                    reportModel.Parameters["DocNo"].Visible = false;

                    Session[Index_Sess_Obj + ActionName] = reportModel;

                    return PartialView("PrintForm", reportModel);
                }
                catch (Exception e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserID.ToString();
                    log.DocurmentAction = id;
                    log.Action = SYActionBehavior.ADD.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";
            var SD = DB.HRDressDeducts.FirstOrDefault(w => w.DocNo == id);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
            if (SD != null)
            {
                FRM_DressDeduct reportModel = new FRM_DressDeduct();
                reportModel.Parameters["DocNo"].Value = SD.DocNo;
                reportModel.Parameters["DocNo"].Visible = false;
                return ReportViewerExtension.ExportTo(reportModel);
            }
            return null;
        }
        #endregion
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {
            ActionName = "Create";
            if (EmpCode != null)
            {
                GetData(EmpCode, ActionName);
                var rss = new { MS = SYConstant.OK };
                return Json(rss, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public string GetData(string EmpCode, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();
            DataList();
            DressDeductObject BSM = new DressDeductObject();
            BSM.ListItem = new List<HRDressDeductItem>();
            var _ListReceive = DB.HRDressReceiveItems.Where(w => w.EmpCode == EmpCode).ToList();
            int id = 0;
            foreach (var list in _ListReceive)
            {
                var _list = new HRDressDeductItem();
                id += 1;
                _list.DocNo = list.DocNO;
                _list.ReceivedItemID = list.ID;
                _list.ItemName = list.Description;
                _list.Qty = list.QTY;
                _list.ID = id;
                if (list.QTY != 0)
                {
                    BSM.ListItem.Add(_list);
                }

            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            return SYConstant.OK;

        }
        public ActionResult ShowAmount(string Action)
        {
            if (Session[Index_Sess_Obj + Action] != null)
            {
                var BSM = new DressDeductObject();
                BSM = (DressDeductObject)Session[Index_Sess_Obj + ActionName];

                var rs = new
                {
                    MS = SYConstant.OK,
                    DeduAmount = BSM.Header.DedAm,
                };
                return Json(rs, (JsonRequestBehavior)1);
            }
            var data1 = new { MS = "FAIL" };
            return Json(data1, (JsonRequestBehavior)1);
        }

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
            UploadControlExtension.GetUploadedFiles("UploadControl", objFile.ValidationSettings, objFile.uc_FileUploadComplete);
            Session[PATH_FILE] = objFile.ObjectTemplate.UpoadPath;
            return null;
        }

        private void DataList()
        {
            DressReturnObject BSM = new DressReturnObject();
            ViewData["DOCUMENT_TYPE"] = DB.ExCfWorkFlowItems.Where(w => w.ScreenID == SCREEN_ID).ToList();
            ViewData["STAFF_LOCATION"] = SYConstant.getBranchDataAccess();
            var ListRE = DB.HRDressReceives.Where(w => w.FlexStatus == 0).ToList();
            var Epm = (from ListEmp in ListRE
                       group ListEmp by new { ListEmp.EmpCode, ListEmp.EmpName }
                      into myGroup
                       where myGroup.Count() > 0
                       select new
                       {
                           myGroup.Key.EmpCode,
                           myGroup.Key.EmpName,
                       }).ToList();
            var ListREitem = DB.HRDressReceiveItems.ToList();
            ViewData["STAFF_SELECT"] = Epm;
            ViewData["EMP_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["ITEM_SELECT"] = ListREitem.Where(b => ListRE.Any(a => a.DocNO == b.DocNO));
            var objWf = new ExWorkFlowPreference();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            var location = "";
            if (Session[_LOCATION_] != null)
            {
                location = Session[_LOCATION_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location, SCREEN_ID);

            SYDataList objList = new SYDataList("LEAVE_TIME");
            objList = new SYDataList("STATUS_LEAVE_APPROVAL");
            ViewData["STATUS"] = objList.ListData;

        }
    }
}