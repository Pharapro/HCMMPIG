using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.MD;
using Humica.Logic.Mission;
using Humica.Models.HR;
using Humica.Models.Report.Mission;
using Humica.Models.SY;
using System; 
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Mission
{
    public class MISClaimController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "MIS0000004";
        private const string URL_SCREEN = "/HRM/Mission/MISClaim/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "MissionCode;Status";
        private string _DOCTYPE_ = "_DOCTYPE_";
        private string _LOCATION_ = "_LOCATION_";
        private string _Branch_ = "_Branch_";
        private string _Dept_ = "_Dept_";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILE1 = "12313123123sadfsdfsdfsdf1";     
        IClsHRMissClaim BSM;
        IUnitOfWork unitOfWork;
        public MISClaimController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsHRMissClaim();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region 'List' 
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfList(KeyName);
            BSM.ListHeader = new List<HRMissionClaim>();
            var ListClaim = unitOfWork.Set<HRMissionClaim>().ToList();
            if (ListClaim.Any())
            {
                BSM.ListHeader = ListClaim.ToList(); 
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List< HRMissionClaim>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion
        #region 'Create'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            BSM.Header = new HRMissionClaim();
            BSM.ListItem = new List<HRMissionClaimItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ListHeader = new List<HRMissionClaim>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header.EmpCode = user.UserName;
            BSM.Header.ClaimDate = DateTime.Now;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.TotalAmount = 0;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsHRMissClaim collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            if (collection.Header != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateMissClaim();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.MissionCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                BSM.ListItem = new List<HRMissionClaimItem>();
                BSM.ListApproval = new List<ExDocApproval>();
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(collection);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        public ActionResult Edit(string DocNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (DocNo == "null") DocNo = null;
            BSM.Header = new HRMissionClaim();
            BSM.ListItem = new List<HRMissionClaimItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(DocNo))
            {
                var Header = unitOfWork.Repository<HRMissionClaim>().Queryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                if (Header != null)
                {
                    var Staff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (Staff != null) BSM.HeaderStaff = Staff;
                    BSM.Header = Header;
                    var ListItem = unitOfWork.Repository<HRMissionClaimItem>().Queryable().Where(w => w.ClaimCode == DocNo).ToList();
                    if (ListItem.Any())
                        BSM.ListItem = ListItem;
                    var approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == Header.MissionCode && w.DocumentType == Header.ClaimType).ToList();
                    if (approve.Any()) BSM.ListApproval = approve;
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string DocNo, ClsHRMissClaim collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            BSM.ScreenId = SCREEN_ID;
            if (BSM.Header != null)
            {
                string msg = BSM.UpdateMClaim(DocNo);
                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = DocNo;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        public ActionResult Details(string DocNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.VIEW);
            if (DocNo == "null") DocNo = null;
            ViewData[SYConstant.PARAM_ID] = DocNo;
            BSM.ListItem = new List<HRMissionClaimItem>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(DocNo))
            {
                var Header = unitOfWork.Repository<HRMissionClaim>().Queryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                if (Header != null)
                {
                    var Staff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (Staff != null) BSM.HeaderStaff = Staff;
                    BSM.Header = Header;
                    var ListItem = unitOfWork.Repository<HRMissionClaimItem>().Queryable().Where(w => w.ClaimCode == DocNo).ToList();
                    if (ListItem.Any())
                        BSM.ListItem = ListItem;
                    var approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == Header.MissionCode && w.DocumentType == Header.ClaimType).ToList();
                    if (approve.Any()) BSM.ListApproval = approve;
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult Delete(string DocNo)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                string msg = BSM.deleteMClaim(DocNo);

                if (msg == SYConstant.OK)
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region 'Grid Item'
        public ActionResult GridItem()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRMissClaim BSM = new ClsHRMissClaim();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItem", BSM.ListItem);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRMissClaim BSM = new ClsHRMissClaim();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemDetails ", BSM.ListItem);
        }
        public ActionResult CreatePItem(HRMissionClaimItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ClsHRMissClaim BSM = new ClsHRMissClaim();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                    }
                    if (BSM.ListItem.Count == 0)
                    {
                        ModelObject.LineItem = 1;
                    }
                    else
                    {
                        ModelObject.LineItem = BSM.ListItem.Max(w => w.LineItem) + 1;
                    }
                    if (Session[PATH_FILE1] != null)
                    {
                        ModelObject.Attachment = Session[PATH_FILE1].ToString();
                        Session[PATH_FILE] = null;
                    }
                    BSM.ListItem.Add(ModelObject);
                    Session[Index_Sess_Obj + ActionName] = BSM;

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItem", BSM.ListItem);
        }
        public ActionResult EditPItem(HRMissionClaimItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsHRMissClaim BSM = new ClsHRMissClaim();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.LineItem == ModelObject.LineItem).ToList();
                if (Session[PATH_FILE1] != null)
                {
                    ModelObject.Attachment = Session[PATH_FILE1].ToString();
                    Session[PATH_FILE] = null;
                }
                else
                {
                    ModelObject.Attachment = objCheck.FirstOrDefault().Attachment;
                }
                if (objCheck.Count > 0)
                {
                    objCheck.First().MissionDate = ModelObject.MissionDate;
                    objCheck.First().Description = ModelObject.Description;
                    objCheck.First().Description = ModelObject.Description;
                    objCheck.First().QtyInvoice = ModelObject.QtyInvoice;
                    objCheck.First().Invoice = ModelObject.Invoice;
                    objCheck.First().NonInvoice = ModelObject.NonInvoice;
                    objCheck.First().NumOfPer = ModelObject.NumOfPer;
                    objCheck.First().Duration = ModelObject.Duration;
                    objCheck.First().Amount = ModelObject.Amount;
                    objCheck.First().Attachment = ModelObject.Attachment;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM.ListItem);
        }
        public ActionResult DeletePItem(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            ClsHRMissClaim BSM = new ClsHRMissClaim();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListItem.Where(w => w.LineItem == LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListItem.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItem", BSM.ListItem);
        }

        #region GridApproval
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            //BSM.ListApproval = new List<ExDocApproval>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            return PartialView("GridApproval", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridApprovalDetail";
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(ModelObject.Approver))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                    }
                    if (BSM.ListApproval.Count == 0)
                    {
                        ModelObject.ID = 1;
                    }
                    else
                    {
                        ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                    }
                    ModelObject.DocumentNo = "N/A";
                    BSM.ListApproval.Add(ModelObject);

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
            DataApprover();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (!string.IsNullOrEmpty(Approver))
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListApproval.Where(w => w.Approver == Approver).ToList();
                    if (BSM.ListApproval.Count > 0)
                    {
                        var objDel = BSM.ListApproval.Where(w => w.Approver == Approver).First();
                        BSM.ListApproval.Remove(objDel);
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

            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion

        public ActionResult Print(string DocNo)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
            UserMVCReportView();
            return View("ReportView");
        }
        public ActionResult DocumentViewerPartial(string DocNo)
        {
            UserSession();
            UserConf(ActionBehavior.VIEWONLY);
            ActionName = "Print";
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                var SD = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                if (SD != null)
                {
                    try
                    {
                        var sa = new RptMissClaim();
                        var reportHelper = new clsReportHelper();
                        string path = reportHelper.Get_Path(SCREEN_ID);
                        if (!string.IsNullOrEmpty(path))
                        {
                            sa.LoadLayoutFromXml(path);
                        }
                        ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                        sa.Parameters["ClaimCode"].Value = SD.ClaimCode;
                        sa.Parameters["ClaimCode"].Visible = false;
                        Session[Index_Sess_Obj + ActionName] = sa;

                        return PartialView("PrintForm", sa);
                    }
                    catch (Exception e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = DocNo;
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/
                    }
                }
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string DocNo)
        {
            ActionName = "Print";
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                var SD = unitOfWork.Set<HRMissionClaim>().AsQueryable().FirstOrDefault(w => w.ClaimCode == DocNo);
                ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                if (SD != null)
                {
                    RptMissClaim reportModel = new RptMissClaim();

                    reportModel = (RptMissClaim)Session[Index_Sess_Obj + ActionName];
                    return ReportViewerExtension.ExportTo(reportModel);
                }
            }
            return null;
        } 
        public ActionResult RequestForApproval(string DocNo)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.requestToApprove(DocNo);

                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = DocNo;
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }  
        public ActionResult Approve(string DocNo)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.approveTheDoc(DocNo);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = DocNo;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Reject(string DocNo)
        {
            this.UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {

                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                }

                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.Reject(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = DocNo;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        public ActionResult Cancel(string DocNo)
        {
            this.UserSession();
            if (DocNo == "null") DocNo = null;
            if (!string.IsNullOrEmpty(DocNo))
            {
                string msg = BSM.CancelDoc(DocNo);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = DocNo;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult Refreshvalue(string action)
        {
            ActionName = action;

            BSM.ListItem = new List<HRMissionClaimItem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRMissionClaim();
                }

                BSM.Header.TotalAmount = 0;
                foreach (var item in BSM.ListItem)
                {
                    BSM.Header.TotalAmount += Convert.ToDecimal(item.Amount);
                }

                var result = new
                {
                    MS = SYConstant.OK,
                    TotalAmount = BSM.Header.TotalAmount,
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        //public ActionResult Refreshvalue(string action)
        //{
        //    ActionName = action;
        //    


        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];


        //        if (BSM.Header == null)
        //        {
        //            BSM.Header = new HRMissionPlan();
        //        }
        //        BSM.Header.TotalAmount_USD_ = 0;
        //        BSM.Header.TotalAmount_KH_ = 0;

        //        if (BSM.ListItem != null)
        //        {
        //            foreach (var item in BSM.ListItem)
        //            {

        //                BSM.Header.TotalAmount_USD_ += Convert.ToDecimal(item.AmountCost_USD);

        //                BSM.Header.TotalAmount_KH_ += Convert.ToDecimal(item.AmountCost_KH);
        //            }
        //        }
        //        var result = new
        //        {
        //            MS = SYConstant.OK,
        //            AmountCost_USD = BSM.Header.TotalAmount_USD_,
        //            AmountCost_KH = BSM.Header.TotalAmount_KH_
        //        };

        //        return Json(result, JsonRequestBehavior.DenyGet);
        //    }

        //    var rs = new { MS = SYConstant.FAIL };
        //    return Json(rs, JsonRequestBehavior.DenyGet);
        //}
        //public ActionResult ShowDataEmp(string ID, string EmpCode, string docType)
        //{
        //    ActionName = "Details";
        //    var EmpStaff = unitOfWork.Repository<HRMissionPlan>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
        //    if (EmpStaff != null)
        //    {
        //        var result = new
        //        {
        //            MS = SYConstant.OK,
        //            Branch = EmpStaff.Branch,
        //           // dept = EmpStaffs.Department,
        //            Post = EmpStaff.Position,
        //            Name = EmpStaff.PlannerName
        //        };
        //        SelectParam(docType, EmpStaff.Branch, EmpStaffs.Department);
        //        return Json(result, JsonRequestBehavior.DenyGet);

        //    }
        //    var rs = new { MS = SYConstant.FAIL };  
        //    return Json(rs, JsonRequestBehavior.DenyGet);
        //}
        public ActionResult SelectParam(string docType, string location, string department)
        {
            UserSession();
            var rs = new { MS = SYConstant.OK };
            ActionName = "Create";
            Session[_DOCTYPE_] = docType;
            Session[_Branch_] = location;
            Session[_Dept_] = department;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(docType, location, department, SCREEN_ID);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataApprover()
        {
            var Branch = "";
            if (Session[_Branch_] != null)
                Branch = Session[_Branch_].ToString();
            var docType = "";
            if (Session[_DOCTYPE_] != null)
                docType = Session[_DOCTYPE_].ToString();
            var Dept = "";
            if (Session[_Dept_] != null)
                Dept = Session[_Dept_].ToString();
            var EWFI = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().FirstOrDefault(w => w.ScreenID == SCREEN_ID && w.DocType == docType);
            if (EWFI != null)
            {
                var CFWI = unitOfWork.Repository<CFWorkFlow>().Queryable().FirstOrDefault(w => w.WFObject == EWFI.ApprovalFlow);
                if (CFWI != null && CFWI.ByDepartment == true)
                    ViewData["WF_LIST"] = unitOfWork.Repository<ExCFWFDepartmentApprover>().Queryable().Where(w => w.WFObject == CFWI.WFObject && w.IsSelected != true && w.Department == Dept).ToList();
                else ViewData["WF_LIST"] = unitOfWork.Repository<ExCfWFApprover>().Queryable().Where(w => w.WFObject == CFWI.WFObject && w.IsSelected != true && w.Branch == Branch).ToList();
            }
        }
        [HttpPost]
        public ActionResult UploadControlCallbackActionImage()
        {
            UserSession();

            if (Session[SYSConstant.IMG_SESSION_KEY_1] != null)
            {
                //DeleteFile(Session[SYSConstant.IMG_SESSION_KEY_1].ToString());
            }

            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
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
        public ActionResult UploadControlCallbackActionClaim(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = unitOfWork.Set<CFUploadPath>().Find("IMG_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadClaim",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE1] = sfi.ObjectTemplate.UpoadPath; ;
            return null;
        }
        public ActionResult SelectMItemElement(string Actionname, string Code)
        {
            ActionName = Actionname;
            Session["Type"] = Code;
            UserSession();
            ClsHRMissClaim BSM = new ClsHRMissClaim();
            var listBranch = SYConstant.getBranchDataAccess();
            ViewData[SYSConstant.PARAM_ID] = Code;
            if (Session[Index_Sess_Obj + this.ActionName] != null)
            {
                BSM = (ClsHRMissClaim)Session[Index_Sess_Obj + this.ActionName];
            }
            var data = new
            {
                MS = SYSConstant.OK
            };
            return Json(data, (JsonRequestBehavior)1);

        }
        public ActionResult MissSetupItem()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "MISPlan", Action = "MissSetupItem" };
                cboProperties.ValueField = "Code";
                cboProperties.TextField = "Description";
                cboProperties.TextFormatString = "{0}:{1}";
                cboProperties.Columns.Add("Code", Humica.EF.Models.SY.SYSettings.getLabel("Code"));
                cboProperties.Columns.Add("Description", Humica.EF.Models.SY.SYSettings.getLabel("Description"));
                //cboProperties.BindList(Humica.Logic.Mission.ClsHRMissClaim.GetAllMissItem());
            });
        }
        private void DataSelector()
        {
            ViewData["MISSION_TYPE_SELECT"] = unitOfWork.Set<ExCfWorkFlowItem>().Where(w => w.ScreenID == SCREEN_ID).ToList();
            SYDataList objListMiss_Type = new SYDataList("TRAVEL_BY");
            ViewData["TRAVEL_BY_SELECT"] = objListMiss_Type.ListData;
            var objWf = new ExWorkFlowPreference();
            var location = "";

            if (Session[_LOCATION_] != null)
            {
                location = Session[_LOCATION_].ToString();
            }
            var docType = "";
            if (Session[_DOCTYPE_] != null)
            {
                docType = Session[_DOCTYPE_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location, SCREEN_ID);

            //objList = new SYDataList("MARITAL");
            //ViewData["MARITAL_SELECT"] = objList.ListData;
            //objList = new SYDataList("LANG_SKILLS");
            //ViewData["LANGSKILLS_SELECT"] = objList.ListData;
            //ViewData["BRANCHES_SELECT"] = DH.HRBranches.ToList().OrderBy(w => w.Description);
            //ViewData["COUNTRY_SELECT"] = DH.HRCountries.ToList().OrderBy(w => w.Description);
            //ViewData["NATION_SELECT"] = DH.HRNations.ToList().OrderBy(w => w.Description);
            //ViewData["RelationshipType_LIST"] = DH.HRRelationshipTypes.ToList();
            //ViewData["HREmpEduType_LIST"] = DH.HREduTypes.ToList();
            //var Processing = SYDocumentStatus.PROCESSING.ToString();
            //ViewData["VACANCY_SELECT"] = DB.RCMVacancies.Where(m => m.Status == Processing).ToList();
            //ViewData["LANG_SELECT"] = DH.RCLangs.ToList().OrderBy(w => w.Lang);
            //ViewData["POSITION_SELECT"] = DH.HRPositions.ToList().OrderBy(w => w.Description);
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            //ViewData["CHANNELRECEIVED_SELECT"] = DH.RCAdvertisers.ToList().OrderBy(w => w.Company);
            //ViewData["MissType_SELECT"] = DB.HRMissTypes.ToList().OrderBy(w => w.Description);
            //ViewData["PROVICES_SELECT"] = DB.HRProvices.ToList().OrderBy(w => w.Description);
           // ViewData["STAFF_SELECT"] = unitOfWork.Set<HR_STAFF_VIEW>().ToList();
            ViewData["DEPARTMENT_LIST"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList();
            ViewData["ClaimType_SELECT"] = unitOfWork.Set<HRMClaimType>().AsQueryable().ToList();
            ViewData["PROVICES_SELECT"] = unitOfWork.Set<HRProvice>().AsQueryable().ToList();
            ViewData["Missplan_SELECT"] = unitOfWork.Repository<HRMissionPlan>()
               .Queryable().Where(w => w.Status == "APPROVED").Select(w => new
               {
                   EmpCode = w.EmpCode,
                   AllName = w.PlannerName,
                   Department = w.Department,
                   Position = w.Position,
                   MissionCode = w.MissionCode,
                   MissionType = w.MissionType,
                   TravelBy = w.TravelBy
               }).ToList();
        }
        private void LoadSession(string docType, string location)
        {
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
        }

    }
    #endregion
}
