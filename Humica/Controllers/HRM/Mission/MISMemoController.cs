using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.HR;
using Humica.Models.Mission;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Humica.EF.MD;
using Humica.EF.Repo;
using Humica.Models.Report.Mission;

namespace Humica.Controllers.HRM.Mission
{
    public class MISMemoController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "MIS0000002";
        private const string URL_SCREEN = "/HRM/Mission/MISMemo/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "MissionCode;Status";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string PATH_FILE1 = "12313123123sadfsdfsdfsdf1";

        IClsHRMissMemo BSM;
        IUnitOfWork unitOfWork;
        public MISMemoController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsHRMissMemo();
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            BSM.ListHeader = new List<HRMissionPlan>();
            BSM.ListPlan = new List<ClsListPlanPending>();
            var obj = unitOfWork.Set<HRMissionPlan>().AsQueryable().Where(w => w.IsMemo == true).ToList();
            if (obj.Any()) BSM.ListHeader = obj;
            var lisPlan = unitOfWork.Repository<HRMissionPlan>().Queryable()
                           .Where(w => w.IsMemo != true && w.Status == SYDocumentStatus.APPROVED.ToString()).ToList();
            if (lisPlan != null && lisPlan.Any()) 
            {
                BSM.ListPlan = lisPlan.Select(plan => new ClsListPlanPending
                {
                    MissionCode = plan.MissionCode,
                    EmpCode = plan.EmpCode,
                    PlannerName = plan.PlannerName,
                    Position = plan.Position,
                    MissionType = plan.MissionType,
                    TravelBy = plan.TravelBy,
                    Status = plan.Status
                }).ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            BSM.ListHeader = new List<HRMissionPlan>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfList(KeyName);
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPlan);
        }
        #endregion
        #region Edit
        public ActionResult Edit(string ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.Header = new HRMissionPlan();
            BSM.ListMember = new List<HRMissionPlanMem>();
            BSM.ListVehicle = new List<HRResgierVehicle>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (ID == "null") ID = null;
            if (!string.IsNullOrEmpty(ID))
            {
                var Header = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == ID 
                            && w.Status == SYDocumentStatus.APPROVED.ToString());
                if (Header != null)
                {
                    BSM.Header = Header;
                    var ListMember = unitOfWork.Set<HRMissionPlanMem>().AsQueryable().Where(w => w.MissionCode == ID).ToList();
                    if (ListMember.Any())
                    {
                        BSM.ListMember = ListMember;
                    }
                    var ListVehicle = unitOfWork.Set<HRResgierVehicle>().AsQueryable().Where(w => w.MissionCode == ID).ToList();
                    if (ListVehicle.Any())
                    {
                        BSM.ListVehicle = ListVehicle;
                    }
                    var ListApprovel = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == ID).ToList();
                    if (ListApprovel.Any())
                    {
                        BSM.ListApproval = ListApprovel;
                    }
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, ClsHRMissMemo collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();
            if (id == "null") id = null;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            if (!string.IsNullOrEmpty(id))
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.UpdateMemo(id);
                if (msg != SYConstant.OK)
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return View(BSM);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                Session[Index_Sess_Obj + this.ActionName] = BSM;

            }
            return View(BSM);

        }
        #endregion
        #region Details
        public ActionResult Details(string ID)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[Humica.EF.SYSConstant.PARAM_ID] = ID;
            BSM.Header = new HRMissionPlan();
            BSM.ListVehicle = new List<HRResgierVehicle>();
            BSM.ListMember = new List<HRMissionPlanMem>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (ID == "null") ID = null;
            if (!string.IsNullOrEmpty(ID))
            {
                var Header = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == ID && w.IsMemo == true && w.Status == SYDocumentStatus.APPROVED.ToString());
                if (Header != null)
                {
                    BSM.Header = Header;
                    var ListMember = unitOfWork.Set<HRMissionPlanMem>().AsQueryable().Where(w => w.MissionCode == ID).ToList();
                    if (ListMember.Any())
                    {
                        BSM.ListMember = ListMember;
                    }
                    var ListVehicle = unitOfWork.Set<HRResgierVehicle>().AsQueryable().Where(w => w.MissionCode == ID).ToList();
                    if (ListVehicle.Any())
                    {
                        BSM.ListVehicle = ListVehicle;
                    }
                    var ListApproval = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == ID).ToList();
                    if (ListApproval.Any())
                    {
                        BSM.ListApproval = ListApproval;
                    }

                }
            }
                return View(BSM);
        }
        #endregion
        #region Delete
        //public ActionResult Delete(string ID)
        //{
        //    UserSession();
        //    if (ID == "null") ID = null;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string msg = BSM.deleteMemo(id);
        //        if (msg == SYConstant.OK)
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DELETE_M", user.Lang);
        //            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //        }
        //        else
        //        {
        //            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
        //            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //        }
        //    }
        //    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
        //    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        //}
        #endregion
        #region Print
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
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                var SD = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == id && w.IsMemo == true);
                if (SD != null)
                {
                    try
                    {
                        var sa = new RPTMissMemo();
                        var reportHelper = new clsReportHelper();
                        string path = reportHelper.Get_Path(SCREEN_ID);
                        if (!string.IsNullOrEmpty(path))
                        {
                            sa.LoadLayoutFromXml(path);
                        }
                        ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                        sa.Parameters["MissionCode"].Value = SD.MissionCode;
                        sa.Parameters["MissionCode"].Visible = false;
                        Session[Index_Sess_Obj + ActionName] = sa;

                        return PartialView("PrintForm", sa);
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
            }
            return null;
        }
        public ActionResult DocumentViewerExportTo(string id)
        {
            ActionName = "Print";
            if (id == "null") id = null;
            if (!string.IsNullOrEmpty(id))
            {
                var SD = unitOfWork.Set<HRMissionPlan>().AsQueryable().FirstOrDefault(w => w.MissionCode == id && w.IsMemo == true);
                ViewData[Humica.EF.SYSConstant.PARAM_ID] = id;
                if (SD != null)
                {
                    RPTMissMemo reportModel = new RPTMissMemo();

                    reportModel = (RPTMissMemo)Session[Index_Sess_Obj + ActionName];
                    return ReportViewerExtension.ExportTo(reportModel);
                }
            }
            return null;
        }
        #endregion
        #region GridMember
        public ActionResult GridMember()
        {
            ActionName = "Create";
            UserSession();
            UserConfList(KeyName);
            BSM.ListMember = new List<HRMissionPlanMem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridMember", BSM.ListMember);
        }
        [HttpPost]
        #endregion
        #region GridHRResgierVehicle
        public ActionResult GridHRResgierVehicle()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfList(KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridHRResgierVehicle", BSM.ListVehicle);
        }
        public ActionResult GridHRResgierVehicleDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridHRResgierVehicleDetails", BSM.ListVehicle);
        }
        public ActionResult CreateResgierVehicle(HRResgierVehicle ModelObject)
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }

            if (ModelObject.AssetCode != null)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
                    }

                    if (BSM.ListVehicle == null)
                        BSM.ListVehicle = new List<HRResgierVehicle>();

                    if (!BSM.ListVehicle.Any(w => w.AssetCode == ModelObject.AssetCode))
                    {
                        BSM.ListVehicle.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPLICATED_ITEM");
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
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridHRResgierVehicle", BSM.ListVehicle);
        }
        public ActionResult EditResgierVehicle(HRResgierVehicle ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListVehicle.Where(w => w.AssetClass == ModelObject.AssetClass).ToList();

                if (objCheck.Count > 0)
                {
                    var Asset = objCheck.First();
                    Asset.AssetClass = ModelObject.AssetClass;
                    objCheck.First().AssetCode = ModelObject.AssetCode;
                    //objCheck.First().AssetCode = ModelObject.AssetCode;
                    objCheck.First().Year = ModelObject.Year;
                    objCheck.First().Model = ModelObject.Model;
                    objCheck.First().VehicleType = ModelObject.VehicleType;
                    objCheck.First().PlateNumber = ModelObject.PlateNumber;
                    objCheck.First().Color = ModelObject.Color;
                    objCheck.First().Description = ModelObject.Description;
                  
                   

                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridHRResgierVehicle", BSM.ListVehicle);
        }
        public ActionResult DeleteResgierVehicle(string ID)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListVehicle.Where(w => w.AssetClass == ID).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListVehicle.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridHRResgierVehicle", BSM.ListVehicle);
        }
        #endregion
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            BSM.ListApproval = new List<ExDocApproval>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
                if (BSM.ListApproval == null)
                {
                    BSM.ListApproval = new List<ExDocApproval>();
                }
            }
            return PartialView("GridApproval", BSM);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridApprovalDetail";
            return PartialView("GridApprovalDetail", BSM);
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
                    BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
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
        //public void LoadData(string userName)
        //{
        //    OnLoad();
        //    //string pending = SYDocumentStatus.PENDING.ToString();
        //    //string open = SYDocumentStatus.OPEN.ToString();
        //    string approved = SYDocumentStatus.APPROVED.ToString();
        //    string Cancel = SYDocumentStatus.CANCELLED.ToString();
        //    var listobj = unitOfWork.Set<HRMissionPlan>().AsQueryable()
        //        .Where(w => w.Status == approved)
        //        .ToList();
        //    var listApp = unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
        //        .Where(w => listobj.AsEnumerable().Where(x => w.DocumentNo == x.MissionCode
        //            && w.DocumentType == x.MissionType && w.Approver == userName).Any())
        //        .ToList();
        //    foreach (var item in listApp)
        //    {
        //        if (item.ApproveLevel == 1 && item.Status == approved)
        //        {
        //            var EmpStaff = listobj.Where(w => w.MissionCode == item.DocumentNo).ToList();
        //            if (!EmpStaff.Any()) continue;
        //            AddPendingToList(item, EmpStaff);
        //        }
        //        else if (item.ApproveLevel > 1 && item.Status == approved)
        //        {
        //            var level = unitOfWork.Set<ExDocApproval>().AsQueryable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == item.DocumentType && w.ApproveLevel < item.ApproveLevel && w.Status != approved).ToList().OrderByDescending(w => w.ApproveLevel).FirstOrDefault();
        //            if (level != null) continue;
        //            var EmpStaff = listobj.Where(w => w.MissionCode == item.DocumentNo).ToList();
        //            if (!EmpStaff.Any()) continue;
        //            AddPendingToList(item, EmpStaff);
        //        }
        //    }
        //    var ListLeaveCreater = unitOfWork.Set<HRMissionPlan>().AsQueryable().AsEnumerable()
        //            .Where(x => unitOfWork.Set<ExDocApproval>().AsQueryable().AsEnumerable()
        //                .Any(w => x.MissionCode == w.DocumentNo
        //                           && x.MissionType == w.DocumentType && w.Approver == userName && x.Status != approved && x.Status != Cancel)
        //                || x.CreatedBy == userName)
        //            .ToList();
        //    ListHeader = ListLeaveCreater;
        //}

        public ActionResult ShowDataEmp(string ID, string EmpCode, string docType)
        {
            ActionName = "Details";
            var EmpStaff = unitOfWork.Repository<HRResgierVehicle>().Queryable().FirstOrDefault(w => w.AssetClass == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    PlateNum = EmpStaff.PlateNumber
                };
                SelectParam(docType, EmpStaff.PlateNumber, EmpStaff.PlateNumber);
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult SelectParam(string docType, string location, string department)
        {
            UserSession();
            var rs = new { MS = SYConstant.OK };
            ActionName = "Create";
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsHRMissMemo)Session[Index_Sess_Obj + ActionName];
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
     
        private void DataSelector()
        {
            ViewData["CARNO_SELECT"] = unitOfWork.Set<HRResgierVehicle>().AsQueryable().ToList();
            ViewData["CarType_SELECT"] = unitOfWork.Set<HRMissOilRating>().AsQueryable().ToList();
            
        }
    }
}
