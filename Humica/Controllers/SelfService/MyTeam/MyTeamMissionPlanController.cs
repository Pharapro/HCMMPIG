using DevExpress.Web.Mvc;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.Report.Mission;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.SelfService.MyTeam
{
    public class MyTeamMissionPlanController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "ESS0000031";
        private const string URL_SCREEN = "/SelfService/MyTeam/MyTeamMissionPlan/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "MissionCode;Status";
        private string _DOCTYPE_ = "_DOCTYPE_";
        private string _Branch_ = "_Branch_";
        private string _Dept_ = "_Dept_";

        IClsMyteamPlan BSM;
        IUnitOfWork unitOfWork;
        public MyTeamMissionPlanController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsMyteamPlan();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }

        #region List
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            var userName = user.UserName;
            BSM.ListPending = new List<ClsMyteamPlanPending>();
            BSM.ListHeader = new List<HRMissionPlan>();
            BSM.LoadData(userName);
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
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPending);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.ADD);
            BSM.Header = new HRMissionPlan();
            BSM.ListMember = new List<HRMissionPlanMem>();
            BSM.ListProvince = new List<HRMissProvince>();
            BSM.ListApproval = new List<ExDocApproval>();
            BSM.ListHeader = new List<HRMissionPlan>();
            BSM.HeaderStaff = new HR_STAFF_VIEW();
            BSM.Header.EmpCode = user.UserName;
            BSM.Header.PlanDate = DateTime.Now;
            BSM.Header.FromDate = DateTime.Now;
            BSM.Header.ToDate = DateTime.Now;
            BSM.Header.Status = SYDocumentStatus.OPEN.ToString();
            BSM.Header.Member = 0;
            BSM.Header.TotalAmount = 0;
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(ClsMyteamPlan collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.ADD);
            if (collection.Header != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                    BSM.Header = collection.Header;
                }
                BSM.Header = collection.Header;
                BSM.HeaderStaff = collection.HeaderStaff;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateMissPlan();

                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.MissionCode;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?DocNo=" + mess.DocumentNumber;

                    Session[Index_Sess_Obj + ActionName] = BSM;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                BSM.ListMember = new List<HRMissionPlanMem>();
                BSM.ListProvince = new List<HRMissProvince>();
                BSM.ListApproval = new List<ExDocApproval>();
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion 
        #region Edit
        public ActionResult Edit(string DocNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (DocNo == "null") DocNo = null;
            BSM.Header = new HRMissionPlan();
            BSM.ListMember = new List<HRMissionPlanMem>();
            BSM.ListProvince = new List<HRMissProvince>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(DocNo))
            {
                var Header = unitOfWork.Repository<HRMissionPlan>().Queryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (Header != null)
                {
                    var Staff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (Staff != null) BSM.HeaderStaff = Staff;
                    BSM.Header = Header;
                    var ListMember = unitOfWork.Repository<HRMissionPlanMem>().Queryable().Where(w => w.MissionCode == DocNo).ToList();
                    if (ListMember.Any())
                        BSM.ListMember = ListMember;
                    var ListProvince = unitOfWork.Repository<HRMissProvince>().Queryable().Where(w => w.MissionCode == DocNo).ToList();
                    if (ListProvince.Any())
                        BSM.ListProvince = ListProvince;
                    var approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == Header.MissionCode && w.DocumentType == Header.MissionType).ToList();
                    if (approve.Any()) BSM.ListApproval = approve;
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE");
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string DocNo, ClsMyteamPlan collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                BSM.Header = collection.Header;
            }
            BSM.ScreenId = SCREEN_ID;
            if (BSM.Header != null)
            {
                string msg = BSM.UpdateMPlan(DocNo);
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
        #endregion 
        #region Details
        public ActionResult Details(string DocNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.VIEW);
            if (DocNo == "null") DocNo = null;
            ViewData[SYConstant.PARAM_ID] = DocNo;
            BSM.ListMember = new List<HRMissionPlanMem>();
            BSM.ListProvince = new List<HRMissProvince>();
            BSM.ListApproval = new List<ExDocApproval>();
            if (!string.IsNullOrEmpty(DocNo))
            {
                var Header = unitOfWork.Repository<HRMissionPlan>().Queryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (Header != null)
                {
                    var Staff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == Header.EmpCode);
                    if (Staff != null) BSM.HeaderStaff = Staff;
                    BSM.Header = Header;
                    var ListMember = unitOfWork.Repository<HRMissionPlanMem>().Queryable().Where(w => w.MissionCode == DocNo).ToList();
                    if (ListMember.Any())
                        BSM.ListMember = ListMember;
                    var ListProvince = unitOfWork.Repository<HRMissProvince>().Queryable().Where(w => w.MissionCode == DocNo).ToList();
                    if (ListProvince.Any())
                        BSM.ListProvince = ListProvince;
                    var approve = unitOfWork.Repository<ExDocApproval>().Queryable().Where(w => w.DocumentNo == Header.MissionCode && w.DocumentType == Header.MissionType).ToList();
                    if (approve.Any()) BSM.ListApproval = approve;
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion
        #region Delete
        public ActionResult Delete(string DocNo)
        {
            UserSession();
            if (DocNo == "null") DocNo = null;
            if (DocNo != null)
            {
                string msg = BSM.deleteMPlan(DocNo);

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
        #region GridMember
        public ActionResult GridMember()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridMember", BSM.ListMember);
        }
        public ActionResult GridMemberDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridMemberDetails", BSM.ListMember);
        }
        public ActionResult CreatePMember(HRMissionPlanMem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            if (ModelObject.EmpCode != null)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                    }
                    if (!BSM.ListMember.Where(w => w.EmpCode == ModelObject.EmpCode).Any())
                    {
                        BSM.ListMember.Add(ModelObject);
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
            return PartialView("GridMember", BSM.ListMember);
        }
        public ActionResult EditPMember(HRMissionPlanMem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListMember.Where(w => w.EmpCode == ModelObject.EmpCode).ToList();

                if (objCheck.Count > 0)
                {
                    var member = objCheck.First();
                    member.EmpCode = ModelObject.EmpCode;
                    objCheck.First().EmpName = ModelObject.EmpName;
                    objCheck.First().Position = ModelObject.Position;
                    objCheck.First().Department = ModelObject.Department;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridMember", BSM.ListMember);
        }
        public ActionResult DeletePMember(string EmpCode)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();


            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListMember.Where(w => w.EmpCode == EmpCode).ToList();

                if (objCheck.Count > 0)
                {
                    BSM.ListMember.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridMember", BSM.ListMember);
        }
        #endregion
        #region Province
        public ActionResult _Province()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("_Province", BSM.ListProvince);
        }
        public ActionResult _ProvinceDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_ProvinceDetail", BSM.ListProvince);
        }
        public ActionResult CreatePProvince(HRMissProvince ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            try
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                }

                if (BSM.ListProvince.Count == 0)
                {
                    ModelObject.LineItem = 1;
                    if (ModelObject.FromDay < DateTime.Now.Date)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }
                    else if (ModelObject.FromDay > ModelObject.ToDay)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }
                    else
                    {
                        BSM.ListProvince.Add(ModelObject);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
                else
                {
                    ModelObject.LineItem = BSM.ListProvince.Max(w => w.LineItem) + 1;
                    if (ModelObject.FromDay < DateTime.Now.Date)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }
                    else if (ModelObject.FromDay > ModelObject.ToDay)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }
                    else if (BSM.ListProvince.Any(p => p.Route == ModelObject.Route))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RT", user.Lang);
                    }
                    else if (BSM.ListProvince.Any(p =>
                        ModelObject.FromDay < p.ToDay &&
                        ModelObject.ToDay > p.FromDay))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQ_ALR", user.Lang);
                    }
                    else
                    {
                        BSM.ListProvince.Add(ModelObject);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }

            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("_Province", BSM.ListProvince);
        }
        public ActionResult EditPProvince(HRMissProvince ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListProvince.Where(w => w.LineItem == ModelObject.LineItem).ToList();

                if (objCheck.Count > 0)
                {
                    if (ModelObject.FromDay < DateTime.Now.Date)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }

                    if (ModelObject.FromDay > ModelObject.ToDay)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("IN_DATE_RANK", user.Lang);
                    }
                    else if (BSM.ListProvince.Any(p =>
                        p.LineItem != ModelObject.LineItem &&
                        p.Route == ModelObject.Route))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("RT", user.Lang);
                    }
                    else if (BSM.ListProvince.Any(p =>
                        p.LineItem != ModelObject.LineItem &&
                        ModelObject.FromDay < p.ToDay &&
                        ModelObject.ToDay > p.FromDay))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQ_ALR", user.Lang);
                    }
                    else
                    {

                        var item = objCheck.First();
                        item.Location = ModelObject.Location;
                        item.FromDay = ModelObject.FromDay;
                        item.ToDay = ModelObject.ToDay;
                        item.Maps = ModelObject.Maps;
                        item.Route = ModelObject.Route;
                    }
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }

                Session[Index_Sess_Obj + ActionName] = BSM;
            }

            return PartialView("_Province", BSM.ListProvince);

        }
        public ActionResult DeletePProvince(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                var objCheck = BSM.ListProvince.Where(w => w.LineItem == LineItem).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListProvince.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("_Province", BSM.ListProvince);
        }
        #endregion
        #region GridApprove
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataApprover();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
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
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
            }
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridApprovalDetail";
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
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
                        BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
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
                        BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
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
        #region RequestForApproval
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
        #endregion
        #region Approve
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
        #endregion
        #region Reject
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
                    BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
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
        #endregion
        #region Cancel
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
        #endregion
        #region Print
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
                var SD = unitOfWork.Repository<HRMissionPlan>().Queryable().FirstOrDefault(w => w.MissionCode == DocNo);
                if (SD != null)
                {
                    try
                    {

                        var sa = new RptPlan();
                        var reportHelper = new clsReportHelper();
                        string path = reportHelper.Get_Path(SCREEN_ID);
                        if (!string.IsNullOrEmpty(path))
                        {
                            sa.LoadLayoutFromXml(path);
                        }
                        ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                        RptPlan reportModel = new RptPlan();

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
                var SD = unitOfWork.Repository<HRMissionPlan>().Queryable().FirstOrDefault(w => w.MissionCode == DocNo);
                ViewData[Humica.EF.SYSConstant.PARAM_ID] = DocNo;
                if (SD != null)
                {
                    RptPlan reportModel = new RptPlan();

                    reportModel = (RptPlan)Session[Index_Sess_Obj + ActionName];
                    return ReportViewerExtension.ExportTo(reportModel);
                }
            }
            return null;
        }
        #endregion
        public ActionResult RefreshTotal(string action)
        {
            ActionName = action;

            BSM.ListMember = new List<HRMissionPlanMem>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRMissionPlan();
                }
                BSM.Header.Member = BSM.ListMember.Count();
               
                var result = new
                {
                    MS = SYConstant.OK,
                    TotalAmount = BSM.Header.Member,
                   

                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult RefreshTotalProvince(string action)
        {
            ActionName = action;

            BSM.ListProvince = new List<HRMissProvince>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
                if (BSM.Header == null)
                {
                    BSM.Header = new HRMissionPlan();
                }
                BSM.Header.Province = BSM.ListProvince.Count();
                var result = new
                {
                    MS = SYConstant.OK,
                    Province = BSM.Header.Province,

                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult ShowDataEmp(string ID, string EmpCode, string docType)
        {
            ActionName = "Details";
            var EmpStaff = unitOfWork.Repository<HR_STAFF_VIEW>().Queryable().FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Branch = EmpStaff.BranchID,
                    dept = EmpStaff.DEPT,
                    Post = EmpStaff.Position,
                    Name = EmpStaff.AllName
                };
                SelectParam(docType, EmpStaff.BranchID, EmpStaff.DEPT);
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
            Session[_DOCTYPE_] = docType;
            Session[_Branch_] = location;
            Session[_Dept_] = department;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsMyteamPlan)Session[Index_Sess_Obj + ActionName];
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
        private void DataSelector()
        {
            ViewData["MISSION_TYPE_SELECT"] = unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().Where(w => w.ScreenID == SCREEN_ID).ToList();
            unitOfWork.Repository<ExCfWorkFlowItem>().Queryable().Where(w => w.ScreenID == SCREEN_ID).ToList();

           // SYDataList objListMiss_Type = new SYDataList("TRAVEL_BY");
            ViewData["TRAVEL_BY_SELECT"] = unitOfWork.Repository<HRMTravelby>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["BRANCHES_SELECT"] = unitOfWork.Repository<HRBranch>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_SELECT"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData["PROVICES_SELECT"] = unitOfWork.Repository<HRProvice>().Queryable().ToList().OrderBy(w => w.Description);
            ViewData["DEPARTMENT_LIST"] = unitOfWork.Repository<HRDepartment>().Queryable().ToList();
            ViewData["STAFF_SELECT"] = unitOfWork.Repository<HR_STAFF_VIEW>()
                .Queryable().Where(w => w.StatusCode == "A").Select(w => new
                {
                    EmpCode = w.EmpCode,
                    AllName = w.AllName,
                    Department = w.Department,
                    Position = w.Position
                }).ToList();
        }
    }
}
