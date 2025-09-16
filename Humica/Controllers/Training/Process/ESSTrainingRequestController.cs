using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Controllers.Reporting.Training;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.Core.SY;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Process
{
    public class ESSTrainingRequestController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TRA0000001";
        private const string URL_SCREEN = "/Training/Process/ESSTrainingRequest/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        private string _DOCTYPE_ = "_DOCTYPE_";
        private string _LOCATION_ = "_LOCATION_";
        private string _DEPT_ = "_DEPT_";
        public ClsEmail EmailObject { get; set; }
        public SYUser User { get; set; }
        HumicaDBContextTR DB = new HumicaDBContextTR();
        Core.DB.HumicaDBContext DBX = new Core.DB.HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public ESSTrainingRequestController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            BSM.Filter = new FTDGeneralPeriod();
            UserSession();
            UserConfList(ActionBehavior.LISTR, KeyName, "ESSTrainingRequest");
            Session[_DOCTYPE_] = "";
            Session[_LOCATION_] = "";
            Session[_DEPT_] = "";

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = BSM.Filter;
            }
            BSM.ListRequestHeader = new List<TrainingRequestHeader>();
            BSM.WaitingListRequestHeader = new List<TrainingRequestHeader>();
            BSM.ListSchedule = new List<ESSTrainingSchedule>();
            string pending = SYDocumentStatus.PENDING.ToString();
            string received = SYDocumentStatus.RECEIVED.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string Approved = SYDocumentStatus.APPROVED.ToString();

            var app = DBX.ExDocApprovals.AsEnumerable().Where(w => w.DocumentType == "TR" && w.Approver == user.UserName && w.Status == open).ToList();
            var obj = DB.TrainingRequestHeader.Where(w => w.RequestCode == user.UserName && w.Status != null).ToList().OrderByDescending(w => w.TrainNo);
            if (obj.Any()) BSM.ListRequestHeader = obj.ToList();            
            foreach (var item in app)
            {
                if(item.ApproveLevel > 1 && item.Status == open)
                {
                    var OtherApp = DBX.ExDocApprovals.AsEnumerable().Where(w => w.DocumentNo == item.DocumentNo && w.DocumentType == "TR" && w.Status != Approved && w.ApproveLevel < item.ApproveLevel).ToList();
                    if (OtherApp.Any()) continue;
                    var WaitingListRequestHeader = DB.TrainingRequestHeader.Where(w => w.TrainNo == item.DocumentNo && w.DocType == item.DocumentType).ToList().OrderByDescending(w => w.TrainNo);
                    foreach (var request in WaitingListRequestHeader)
                    {
                        BSM.WaitingListRequestHeader.Add(request);
                    }
                }
                else if (item.ApproveLevel == 1 && item.Status == open)
                {
                    var WaitingListRequestHeader = DB.TrainingRequestHeader.Where(w => w.TrainNo == item.DocumentNo && w.DocType == item.DocumentType && w.Status == pending).ToList().OrderByDescending(w => w.TrainNo);
                    foreach (var request in WaitingListRequestHeader)
                    {
                        BSM.WaitingListRequestHeader.Add(request);
                    }
                }
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(TrainningObject BSM)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSTrainingRequest");

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            //DataListSelector();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSTrainingRequest");
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.WaitingListRequestHeader);
        }
        public ActionResult PartialList1()
        {
            ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            //DataListSelector();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSTrainingRequest");
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList1", BSM.ListRequestHeader);
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSD = new TrainningObject();
            BSD.ListApproval = new List<ExDocApproval>();
            BSD.RequestHeader = new TrainingRequestHeader();
            BSD.RequestItem = new TrainingRequestItem();
            BSD.ListSchedule = new List<ESSTrainingSchedule>();
            BSD.HeaderStaff = new HR_STAFF_VIEW();
            BSD.ESSTrainingSchedules = new ESSTrainingSchedule();
            BSD.RequestHeader.RequestDate = DateTime.Now;
            var emp = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (emp != null)
            {
                BSD.HeaderStaff.EmpCode = emp.EmpCode;
                BSD.HeaderStaff.Department = emp.Department;
                BSD.HeaderStaff.Position = emp.Position;
            }

            else if (emp == null)
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(TrainningObject collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            var BSD = new TrainningObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSD = Session[Index_Sess_Obj + ActionName] as TrainningObject;
                BSD.RequestHeader = collection.RequestHeader;
            }
            if (Session[PATH_FILE] != null)
            {
                BSD.RequestHeader.AttachFile = Session[PATH_FILE].ToString();
            }
            var User = DBX.HRStaffProfiles.Where(w => w.EmpCode == user.UserName);
            BSD.ScreenId = SCREENID;
            string msg = BSD.CreateTraineeItems();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSD.RequestHeader.ID.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + BSD.RequestHeader.ID.ToString();

                Session[Index_Sess_Obj + ActionName] = BSD;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            else
            {
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSD;
                return View(BSD);
            }
        }
        #endregion
        #region Edit
        public ActionResult Edit(int id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataListSelector();
            ViewData[SYSConstant.PARAM_ID] = id;
            TrainningObject BSD = new TrainningObject();
            BSD.ListApproval = new List<ExDocApproval>();
            BSD.ListSchedule = new List<ESSTrainingSchedule>();
            BSD.RequestHeader = new TrainingRequestHeader();
            BSD.RequestItem = new TrainingRequestItem();
            BSD.HeaderStaff = new HR_STAFF_VIEW();
            BSD.ESSTrainingSchedules = new ESSTrainingSchedule();

            BSD.RequestHeader = DB.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);
            var Requester = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSD.RequestHeader.RequestCode);
            BSD.HeaderStaff.EmpCode = BSD.RequestHeader.RequestCode;
            BSD.HeaderStaff.AllName = BSD.RequestHeader.RequesterName;
            BSD.HeaderStaff.Department = Requester.Department;
            BSD.HeaderStaff.Position = Requester.Position;

            if (id != null)
            {
                BSD.RequestHeader = DB.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);

                if (BSD.RequestHeader != null)
                {
                    var RequestItem = DB.TrainingRequestItem.Where(w => w.ID == BSD.RequestHeader.ID).ToList();
                    BSD.ListRequestItem = RequestItem.Where(w => w.ID == BSD.RequestHeader.ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSD;
                    string Approve = SYDocumentStatus.APPROVED.ToString();
                    var ListSchedule = DB.ESSTrainingSchedule.Where(w => w.TrainNo == BSD.RequestHeader.TrainNo).ToList();
                    BSD.ListSchedule = ListSchedule;
                    Session[Index_Sess_Obj + ActionName] = BSD;
                    if (BSD.RequestHeader.Status != Approve)
                    {
                        BSD.ListApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == BSD.RequestHeader.TrainNo && w.DocumentType == BSD.RequestHeader.DocType).ToList();
                        SelectParam("TR", BSD.RequestHeader.Branch,BSD.RequestHeader.Department);
                        Session[Index_Sess_Obj + ActionName] = BSD;
                        return View(BSD);
                    }
                    else
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV");
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                    }
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSD;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, TrainningObject collection)
        {
            try
            {
                ActionName = "Create";
                UserSession();
                UserConfListAndForm(this.KeyName);
                DataListSelector();
                ViewData[SYSConstant.PARAM_ID1] = false;
                TrainningObject BSD = new TrainningObject();
                if (id != null)
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSD = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                        BSD.RequestHeader = collection.RequestHeader;
                    }
                    if (Session[PATH_FILE] != null)
                    {
                        BSD.RequestHeader.AttachFile = Session[PATH_FILE].ToString();
                    }
                    BSD.ScreenId = SCREENID;
                    string msg = BSD.EditTraineeItems(id);
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = id.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        return View(BSD);
                    }
                    else
                    {
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return View(BSD);
                    }
                }
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return View(BSD);
            }
            catch
            {
                return View();
            }
        }
        #endregion 
        #region Detials
        public ActionResult Details(int id)
        {
            ActionName = "Details";
            this.UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "ESSTrainingRequest");
            TrainningObject BSD = new TrainningObject();
            BSD.ListApproval = new List<ExDocApproval>();
            BSD.ListSchedule = new List<ESSTrainingSchedule>();
            BSD.RequestHeader = new TrainingRequestHeader();
            BSD.RequestItem = new TrainingRequestItem();
            BSD.HeaderStaff = new HR_STAFF_VIEW();
            BSD.ESSTrainingSchedules = new ESSTrainingSchedule();

            string OPEN = SYDocumentStatus.OPEN.ToString();
            BSD.RequestHeader = DB.TrainingRequestHeader.SingleOrDefault(w => w.ID == id);
            BSD.RequestHeader = DB.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);
            var TrainType = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == BSD.RequestHeader.TrainingType);
            BSD.RequestHeader.TrainingType = TrainType.Description;
            var TrainCat = DB.TRCourseCategories.FirstOrDefault(w => w.Code == BSD.RequestHeader.TrainingCategory);
            BSD.RequestHeader.TrainingCategory = TrainCat.SecondDescription;
            var Requester = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSD.RequestHeader.RequestCode);
            BSD.HeaderStaff.EmpCode = BSD.RequestHeader.RequestCode;
            BSD.HeaderStaff.AllName = BSD.RequestHeader.RequesterName;
            BSD.HeaderStaff.Department = Requester.Department;
            BSD.HeaderStaff.Position = Requester.Position;
            BSD.ListApproval = DBX.ExDocApprovals.Where(w => w.DocumentNo == BSD.RequestHeader.TrainNo && w.DocumentType == BSD.RequestHeader.DocType).ToList();
            var ListSchedule = DB.ESSTrainingSchedule.Where(w => w.TrainNo == BSD.RequestHeader.TrainNo).ToList();
            BSD.ListSchedule = ListSchedule;
            Session[Index_Sess_Obj + ActionName] = BSD;
            if (BSD.RequestHeader != null)
            {
                BSD.ListRequestItem = DB.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode && w.ID == BSD.RequestHeader.ID).ToList();
                ViewData[SYConstant.PARAM_ID] = id;
                var Mont = DateTime.Now.Month;
                Session[Index_Sess_Obj + ActionName] = BSD;
                return View(BSD);
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItemDetails()
        {
            ActionName = "Details";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            //BSM.ListItems = new List<SPPOItem>();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemDetails", BSM.ListRequestItem);
        }
        public ActionResult GridItemSchedule()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            BSM.ScreenId = SCREENID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            DataListSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItemSchedule";
            return PartialView("GridItemSchedule",BSM.ListSchedule);
        }
        public ActionResult EditSchedule(ESSTrainingSchedule MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var DBU = new HumicaDBContextTR();
                    DateTime FromDate = MModel.TrainDate;
                    DateTime ToDate = MModel.TrainDate;
                    var TrainNo = MModel.TrainNo;
                    var No = MModel.No;
                    var listSchedule = BSM.ListSchedule.Where(w => w.No == MModel.No).ToList();
                    if (listSchedule.Count > 0)
                    {
                        var objUpdate = listSchedule.First();
                        if (MModel.FromTime != DateTime.MinValue && MModel.ToTime != DateTime.MinValue)
                        {
                            var totals = MModel.ToTime.Subtract(MModel.FromTime).TotalHours;
                            objUpdate.Hour = (decimal)totals;
                            if (totals >= 5 && totals >= 9)
                            {
                                objUpdate.Hour = (decimal)totals - 1;
                            }
                            else if(totals >= 5 && totals <= 9)
                            {
                                objUpdate.Hour = (decimal)totals - 1;
                            }
                            else objUpdate.Hour = (decimal)totals;
                            objUpdate.FromTime = MModel.FromTime;
                            objUpdate.ToTime = MModel.ToTime;
                        }

                        Session[Index_Sess_Obj + ActionName] = BSM;
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
            DataListSelector();
            return PartialView("GridItemSchedule", BSM.ListSchedule);
        }
        public ActionResult DeleteSchedule(ESSTrainingSchedule MModel)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            DateTime FromDate = MModel.TrainDate;
            DateTime ToDate = MModel.TrainDate;
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                    }

                    BSM.ListSchedule.Where(w => w.No == MModel.No).ToList();
                    if (BSM.ListSchedule.Count > 0)
                    {
                        var objDel = BSM.ListSchedule.Where(w => w.No == MModel.No).First();
                        BSM.ListSchedule.Remove(objDel);
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
            DataListSelector();
            return PartialView("GridItemSchedule", BSM.ListSchedule);
        }
        #endregion

        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = DBX.CFUploadPaths.Find("TRFORM_UPLOAD");
            SYFileImport sfi = new SYFileImport(path);
            sfi.ObjectTemplate = new Core.DB.MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREENID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            sfi.ObjectTemplate.Module = "STAFF";
            sfi.ObjectTemplate.IsGenerate = false;

            UploadedFile[] files = UploadControlExtension.GetUploadedFiles("UploadControl",
                sfi.ValidationSettings,
                sfi.uc_FileUploadCompleteFile);
            Session[PATH_FILE] = sfi.ObjectTemplate.UpoadPath;

            return null;
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataListSelector();
            TrainningObject BSM = new TrainningObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainningObject;
                if (BSM.ListRequestItem != null)
                {
                    BSM.ListRequestItem = BSM.ListRequestItem.OrderBy(w => w.EmpCode).ToList();
                }
            }

            return PartialView("GridItems", BSM);
        }
        [HttpPost]
        public ActionResult DeleteTrainee(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataListSelector();
            TrainningObject BSM = new TrainningObject();

            if (LineItem != 0)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainningObject;
                    if (BSM.ListRequestItem.Count > 0)
                    {
                        var objCheck = BSM.ListRequestItem.FirstOrDefault(w => w.LineItem == LineItem);
                        BSM.ListRequestItem.Remove(objCheck);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
        }
        [HttpPost]
        public ActionResult gvemployeeselector()
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainningObject BSM = new TrainningObject();
            BSM.ScreenId = SCREENID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            else
            {
                BSM.ListStaffs = new List<Core.DB.HRStaffProfile>();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";

            BSM.GetAllStaffs();
            return PartialView("gvemployeeselector", BSM);

        }
        public ActionResult PartialEmployeeSearch()
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListStaffs = new List<Core.DB.HRStaffProfile>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            BSM.GetAllStaffs();
            return PartialView("PartialEmployeeSearch", BSM);

        }
        public ActionResult SelectedTrainee(string EmpCode)
        {

            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            if (BSM.ListRequestItem == null)
            {
                BSM.ListRequestItem = new List<TrainingRequestItem>();
            }
            if (EmpCode != null)
            {
                string[] empCodes = EmpCode.Split(',');
                var staff = DBX.HRStaffProfiles.ToList();
                List<Core.DB.HRDepartment> ListDepartment = DBX.HRDepartments.ToList();
                List<Core.DB.HRPosition> ListPosition = DBX.HRPositions.ToList();
                TrainingRequestHeader bsm = new TrainingRequestHeader();
                var list = staff.Where(w => empCodes.Contains(w.EmpCode));
                int i = 0;
                i = BSM.ListRequestItem.Count();
                foreach (var item in list)
                {
                    if (!BSM.ListRequestItem.Where(w => w.EmpCode == item.EmpCode).Any())
                    {
                        i++;
                        string Position = "";
                        if (!string.IsNullOrEmpty(item.JobCode))
                        {
                            var Post = ListPosition.FirstOrDefault(w => w.Code == item.JobCode);
                            if (Post != null) Position = Post.Description;
                        }
                        BSM.ListRequestItem.Add(new TrainingRequestItem
                        {
                            LineItem = i,
                            EmpCode = item.EmpCode,
                            EmpName = item.AllName,
                            Position = Position
                        });
                    }
                }
                BSM.ListRequestItem.OrderBy(x => x.EmpCode);
                var result = new
                {
                    MS = SYConstant.OK,
                    TotalCapacity = BSM.ListRequestItem.Count()
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        #region RequestToRelease
        public ActionResult ReleaseDoc(string TrainNo)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataListSelector();
            if (TrainNo != null)
            {
                TrainningObject BSD = new TrainningObject();

                BSD.ScreenId = SCREENID;
                string Pending = SYDocumentStatus.PENDING.ToString();
                string URL = SYUrl.getBaseUrl() + "/Training/Process/ESSRequestInvitation/";
                string msg = BSD.Request(TrainNo,URL);
                if (msg == SYConstant.OK)
                {
                    var ListTrainee = DB.TrainingRequestItem.Where(w => w.TrainNo == TrainNo && w.Status == Pending).ToList();
                    if (ListTrainee.Count == 0)
                    {
                        #region template
                        ESS_FRM_TRAINING_REQUEST sa = new ESS_FRM_TRAINING_REQUEST();
                        string receiver = string.Empty;
                        int approvelevel = 0;
                        receiver = DBX.ExDocApprovals.Where(w => w.DocumentNo == TrainNo).OrderBy(w => w.ApproveLevel).FirstOrDefault().Approver;
                        var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREENID && w.IsActive == true).ToList();
                        if (objRpt.Count > 0)
                        {
                            sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                        }
                        var _Appro = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);
                        sa.Parameters["TrainNo"].Value = TrainNo;
                        sa.Parameters["APROLEVEL"].Value = approvelevel;

                        sa.Parameters["TrainNo"].Visible = false;
                        sa.Parameters["APROLEVEL"].Visible = false;
                        Session[this.Index_Sess_Obj + this.ActionName] = sa;

                        string fileName = Server.MapPath("~/Content/UPLOAD/" + "TRAINING REQUEST FORM.pdf");
                        string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                        if (!Directory.Exists(UploadDirectory))
                        {
                            Directory.CreateDirectory(UploadDirectory);
                        }
                        sa.ExportToPdf(fileName);

                        BSD.SendEmail(TrainNo,fileName, receiver);
                        #endregion
                    }

                    SYMessages mess = SYMessages.getMessageObject("RELEASE_M", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_NE", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approve
        public ActionResult Approve(string TrainNo, string Comment)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataListSelector();
            if (TrainNo != null)
            {
                TrainningObject BSD = new TrainningObject();
                BSD.ScreenId = SCREENID;
                string fileName = Server.MapPath("~/Content/UPLOAD/" + "TRAINING REQUEST FORM.pdf");
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                string msg = BSD.ESSApproved(TrainNo, fileName, Comment);

                if (msg == SYConstant.OK)
                {
                    ESS_FRM_TRAINING_REQUEST sa = new ESS_FRM_TRAINING_REQUEST();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREENID && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    var _Appro = DBV.ExDocApprovals.FirstOrDefault(w => w.DocumentNo == TrainNo && w.Approver == user.UserName);

                    sa.Parameters["TrainNo"].Value = TrainNo;
                    sa.Parameters["APROLEVEL"].Value = _Appro.ApproveLevel;
                    sa.Parameters["TrainNo"].Visible = false;
                    sa.Parameters["APROLEVEL"].Visible = false;
                    Session[this.Index_Sess_Obj + this.ActionName] = sa;

                    
                    if (!Directory.Exists(UploadDirectory))
                    {
                        Directory.CreateDirectory(UploadDirectory);
                    }
                    sa.ExportToPdf(fileName);
                
                
                    TrainingRequestHeader objmatch = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);

                    string Open = SYDocumentStatus.OPEN.ToString();
                    var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocType
                                        && w.DocumentNo == objmatch.TrainNo && w.Status == Open).OrderBy(w => w.ApproveLevel).ToList();

                    var b = false;
                    int approverLevel = 0;
                    approverLevel = _Appro.ApproveLevel;

                    #region Email
                    if (listApproval.Where(w => w.DocumentNo == objmatch.TrainNo && w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0)
                    {
                        ExDocApproval NextApp = listApproval.Where(w => w.ApproveLevel > approverLevel).OrderBy(w=>w.ApproveLevel).First();
                        var AlertTO_ = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == NextApp.Approver);
                        var EmailConf_ = SMS.CFEmailAccounts.FirstOrDefault();
                        var ReqTraining = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);
                        string URL = SYUrl.getBaseUrl() + "/Training/Process/ESSTrainingRequest/Details/" + ReqTraining.ID;

                        if (EmailConf_ != null && AlertTO_ != null)
                        {
                            string str = "<div style=\"margin-top: 30px;\"> <h1 style=\"background-color:#14A3C7;color:white; width: 600px;text-align:center;margin-bottom: 30px;\">TRAINING REQUEST FORM: " + "HCA" + "/" + ReqTraining.TrainNo + " </h1 > " + " </div>";
                            str += "<b>" + "Dear " + AlertTO_.Title + " " + AlertTO_.AllName + "</b>";
                            if (NextApp.ApproveLevel == 2) str += "<br /><br />I have Reviewed and Recommended on the training request ";
                            else str += "<br /><br />I have verified on the training request ";
                            str += "<br /><br /><b> Training Subject: " + ReqTraining.TrainingCourse + "</b>";
                            str += "<br /><br /><b> Training: Start Date: " + ReqTraining.FromDate.ToString("dd-MM-yyyy") + " " + "End Date: " + ReqTraining.ToDate.ToString("dd-MM-yyyy") + "</b>";
                            str += "<br /><br /> Please kindly click on the link for<b> approval:</b> " + string.Format("<a href='{0}'>Link</a>", URL);
                            CFEmailAccount emailAccount = EmailConf_;
                            string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                            string body = str;
                            string filePath = fileName;
                            string fileNames = Path.GetFileName(filePath);
                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMailNew(emailAccount, "", AlertTO_.Email,
                                subject, body, filePath, fileName);
                        }
                        var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                        mess.Description = mess.Description + ". " + BSD.MessageError;
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    }
                    else
                    {
                        var ReqTraining = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);
                        var AlertTO = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestCode);
                        var PIC = DBX.ExCfWFApprovers.FirstOrDefault(w => w.WFObject == "PIC");
                        var ListTrainee = DB.TrainingRequestItem.Where(w => w.TrainNo == TrainNo && w.EmpCode != ReqTraining.RequestCode).ToList();
                        string listTrainee = "";
                        var receiver = "";
                        
                        foreach (var item in ListTrainee)
                        {
                            var Trainee = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == item.EmpCode);
                            listTrainee += Trainee.Email + ";";
                        }
                        if (PIC != null)
                        {
                            var StaffPIC = DBX.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == PIC.Employee);
                            receiver = AlertTO.Email + ";" + StaffPIC.Email + ";" + listTrainee;
                        }
                        else receiver = AlertTO.Email + ";" + listTrainee;

                        string URL = SYUrl.getBaseUrl() + "/Training/Process/ESSTrainingRequest/Details/" + ReqTraining.ID;
                        var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                        if (EmailConf != null && AlertTO != null)
                        {
                            string str = "<div style=\"margin-top: 30px;\"> <h1 style=\"background-color:#14A3C7;color:white; width: 600px;text-align:center;margin-bottom: 30px;\">YOUR TRAINING REQUEST IS APPROVED BY THE FINAL APPROVER </h1> </div>";
                            str += "<b>" + "Dear " + AlertTO.Title + " " + AlertTO.AllName + "</b>";
                            str += "<br /><br />Your training request is approved by the CEO.";
                            str += "<br /><br /><b> Training Subject: " + ReqTraining.TrainingCourse + "</b>";
                            str += "<br /><br /><b> Training: Start Date: " + ReqTraining.FromDate.ToString("dd-MM-yyyy") + " " + "End Date: " + ReqTraining.ToDate.ToString("dd-MM-yyyy") + "</b>";
                            str += "<br /><br /> Please kindly click on the link for<b> review:</b> " + string.Format("<a href='{0}'>Link</a>", URL);
                            CFEmailAccount emailAccount = EmailConf;
                            string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                            string body = str;
                            string filePath = fileName;
                            string fileNames = Path.GetFileName(filePath);
                            EmailObject = new ClsEmail();
                            int rs = EmailObject.SendMailNew(emailAccount, "", receiver,
                                subject, body, filePath, fileName);
                        }
                        var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                        mess.Description = mess.Description + ". " + BSD.MessageError;
                        Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    }
                    #endregion
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Reject
        public ActionResult Reject(string TrainNo, string Comment)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataListSelector();
            if (TrainNo != null)
            {
                TrainningObject BSD = new TrainningObject();
                BSD.ScreenId = SCREENID;
                string fileName = Server.MapPath("~/Content/UPLOAD/" + "TRAINING REQUEST FORM.pdf");
                string UploadDirectory = Server.MapPath("~/Content/UPLOAD");
                string msg = BSD.ESSRejected(TrainNo, fileName, Comment);

                if (msg == SYConstant.OK)
                {
                    ESS_FRM_TRAINING_REQUEST sa = new ESS_FRM_TRAINING_REQUEST();
                    var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREENID && w.IsActive == true).ToList();
                    if (objRpt.Count > 0)
                    {
                        sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                    }
                    var _Appro = DBV.ExDocApprovals.FirstOrDefault(w => w.DocumentNo == TrainNo && w.Approver == user.UserName);

                    sa.Parameters["TrainNo"].Value = TrainNo;
                    sa.Parameters["APROLEVEL"].Value = _Appro.ApproveLevel;
                    sa.Parameters["TrainNo"].Visible = false;
                    sa.Parameters["APROLEVEL"].Visible = false;
                    Session[this.Index_Sess_Obj + this.ActionName] = sa;


                    if (!Directory.Exists(UploadDirectory))
                    {
                        Directory.CreateDirectory(UploadDirectory);
                    }
                    sa.ExportToPdf(fileName);

                    TrainingRequestHeader objmatch = DB.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == TrainNo);

                    string Open = SYDocumentStatus.OPEN.ToString();
                    var listApproval = DBX.ExDocApprovals.Where(w => w.DocumentType == objmatch.DocType
                                        && w.DocumentNo == objmatch.TrainNo).OrderBy(w => w.ApproveLevel).ToList();
                    var listUser = listApproval.FirstOrDefault(w => w.Approver == user.UserName);

                    var b = false;
                    int approverLevel = 0;
                    approverLevel = listUser.ApproveLevel;

                    var Status = SYDocumentStatus.APPROVED.ToString();
                    var Appro_ = objmatch.RequestCode;
                    // Approver
                    if ((listApproval.Where(w => w.ApproveLevel > approverLevel && w.Status == SYDocumentStatus.OPEN.ToString()).Count() > 0))
                    {
                        Status = SYDocumentStatus.PENDING.ToString();
                        Appro_ = listApproval.Where(w => w.ApproveLevel == approverLevel).Select(w => w.Approver)?.FirstOrDefault();

                    }

                    #region Email
                    
                    var AlertTO = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == objmatch.RequestCode);
                    var EmailConf = SMS.CFEmailAccounts.FirstOrDefault();
                    if (EmailConf != null && AlertTO != null)
                    {
                        CFEmailAccount emailAccount = EmailConf;
                        string subject = string.Format("Training Request Form {0:dd-MMM-yyyy}", DateTime.Today);
                        //string body = str;
                        string filePath = fileName;
                        string fileNames = Path.GetFileName(filePath);
                        EmailObject = new ClsEmail();
                        int rs = EmailObject.SendMailNew(emailAccount, "", AlertTO.Email,
                            subject, "", filePath, fileName);
                    }
                    var mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    mess.Description = mess.Description + ". " + BSD.MessageError;
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    
                    #endregion
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion
        #region Approver Detail
        public ActionResult SelectParam(string docType, string location, string department)
        {
            UserSession();
            Session[_DOCTYPE_] = docType;
            Session[_LOCATION_] = location;
            var rs = new { MS = SYConstant.OK };
            //Auto Approval
            ActionName = "Create";
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var Dept = DBX.HRDepartments.FirstOrDefault(w => w.Description == department);
                if (Dept != null)
                {
                    department = Dept.Code;
                }
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                BSM.SetAutoApproval(docType, location, department, SCREENID);
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            Session[_DEPT_] = department;
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        public ActionResult GridApproval()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            BSM.ListApproval = new List<ExDocApproval>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            DataListSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateApproval(ExDocApproval ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                    }

                    var msg = BSM.isValidApproval(ModelObject, EnumActionGridLine.Add);
                    if (msg == SYConstant.OK)
                    {

                        if (BSM.ListApproval.Count == 0)
                        {
                            ModelObject.ID = 1;
                        }
                        else
                        {
                            ModelObject.ID = BSM.ListApproval.Max(w => w.ID) + 1;
                        }
                        //  ModelObject.DocumentType = Session[_DOCTYPE_].ToString();
                        ModelObject.DocumentNo = "N/A";
                        BSM.ListApproval.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
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
            DataListSelector();

            return PartialView("GridApproval", BSM.ListApproval);
        }
        public ActionResult DeleteApproval(string Approver)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
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
            DataListSelector();

            return PartialView("GridApproval", BSM.ListApproval);
        }
        //Edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditApproval(ExDocApproval approval)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                var listcheck = BSM.ListApproval.Where(w => w.Approver == approval.Approver).ToList();
                if (listcheck.ToList().Count > 0)
                {
                    var objUpdate = listcheck.First();
                    objUpdate.ApproveLevel = approval.ApproveLevel;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            DataListSelector();
            return PartialView("GridApproval", BSM.ListApproval);
        }
        #endregion
        public ActionResult Get_Day(DateTime FromDate, DateTime ToDate)
        {
            ActionName = "Create";
            TrainningObject BSM = new TrainningObject();
            BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            BSM.ListSchedule = new List<ESSTrainingSchedule>();
            DateTime DateNow = DateTime.Now;
            DateTime _FromDate = FromDate.Date;
            DateTime _ToDate = ToDate.Date;
            decimal total = (_ToDate-_FromDate).Days + 1;
            int Line = 0;
            decimal weekday = 0;
            decimal weekend = 0;
            for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
            {
                var Schedule = new ESSTrainingSchedule();
                var Duration = new TrainingRequestHeader();
                Line += 1;

                Schedule.TrainDate = day;
                Schedule.No = Line;
                Schedule.FromTime = new DateTime(day.Year, day.Month, day.Day, 8, 0, 0);
                Schedule.ToTime = new DateTime(day.Year, day.Month, day.Day, 17, 0, 0);
                var totals = Schedule.ToTime.Subtract(Schedule.FromTime).TotalHours;
                if (totals > 8 && totals >= 9)
                    Schedule.Hour = (decimal)totals - 1;
                else Schedule.Hour = (decimal)totals;
                var Total = Schedule.Hour;
                if (Total >= 8)
                {
                    weekday += 1;
                    BSM.ListSchedule.Add(Schedule);
                }
            }
            var result = new
            {
                MS = SYConstant.OK,
                Day = weekday
            };
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Json(result, JsonRequestBehavior.DenyGet);
        }
        public ActionResult Update_Hour(string Action)
        {
            ActionName = Action;
            TrainningObject BSM = new TrainningObject();
            BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            DateTime DateNow = DateTime.Now;
            decimal weekday = 0;
            var list = BSM.ListSchedule.ToList();
            foreach(var item in list)
            {
                if (item.Hour >= 8)
                {
                    weekday += 1;
                }
                if (item.Hour < 8)
                {
                    weekday += item.Hour / 8;
                }
            }
            var result = new
            {
                MS = SYConstant.OK,
                Day = weekday
            };
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Json(result, JsonRequestBehavior.DenyGet);
        }

        #region "Private Code"
        [HttpPost]
        public ActionResult ShowDataEmp(string ID, string EmpCode)
        {

            ActionName = "Details";
            var EmpStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                Session["EmpCode"] = EmpCode;
                var result = new
                {
                    MS = SYConstant.OK,
                    DEPT = EmpStaff.Department,
                    Post =EmpStaff.Position
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataListSelector()
        {
            var objWf = new TrainningObject();
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
            var department = "";
            if (Session[_DEPT_] != null)
            {
                department = Session[_DEPT_].ToString();
            }
            ViewData["WF_LIST"] = objWf.getApproverListByDocType(docType, location,department, SCREENID);
            SYDataList DL = new SYDataList("YEAR_SELECT");
            ViewData["YEAR_SELECT"] = DL.ListData;

            DL = new SYDataList("STATUS_APPROVAL");
            ViewData["STATUS_APPROVAL"] = DL.ListData;

            //ViewData["DEALER_LIST"] = MDDMaterialObject.DealerList().ToList();
            DL = new SYDataList("QT_RELEASE_TYPE");
            ViewData["QT_RELEASE_TYPE"] = DL.ListData;
            DL = new SYDataList("CALENDAR_TYPE");
            ViewData["CALENDAR_TYPE"] = DL.ListData;
            DL = new SYDataList("MONTH_SELECT");
            ViewData["MONTH_SELECT"] = DL.ListData;
            DL = new SYDataList("STATUS_APPROVAL");
            ViewData["STATUS_APPROVAL"] = DL.ListData;
            DL = new SYDataList("REPORT_TYPE");
            ViewData["REPORT_TYPE"] = DL.ListData;
            DL = new SYDataList("KI");
            ViewData["YEAR_KI_SELECT"] = DL.ListData;
            DL = new SYDataList("KI_QUATER");
            ViewData["QUATER_KI_SELECT"] = DL.ListData;

            ViewData["REQUEST_SELECT"] = new SYDataList("REQUEST_SELECT").ListData;
            DL = new SYDataList("STAFF_POSITION");
            ViewData["STAFF_POSITION"] = DL.ListData;
            var listReason = DBX.CFReasonCodes.Where(w => w.Indicator == "H").ToList();
            foreach (var read in listReason)
            {
                read.Description1 = read.Description2 + "-" + read.Description1;
            }
            ViewData["REASON_CANCEL"] = listReason;
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            //ViewData["Course_List"] = DB.TrainingCourse.ToList();
            //ViewData["Program_List"] = DB.TrainingProgram.ToList();
            //ViewData["StaffLevel_List"] = DBX.HRLevels.ToList();
            //ViewData["Target_List"] = DB.TRTrainingRequirements.Where(w => w.Category == "T").ToList();
            //ViewData["Venue_List"] = DB.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            //ViewData["Dayterm_List"] = DB.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            //ViewData["Group_List"] = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            //ViewData["Requirement_List"] = DB.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            ViewData["Staff_List"] = DBX.HRStaffProfiles.ToList();

            ViewData["COURCE_SELECT"] = DB.TRTrainingCourses.ToList();
            ViewData["TRAINING_TYPE"] = DB.TRTrainingTypes.ToList();
            ViewData["DEPT_SELECT"] = DBX.HRDepartments.ToList();
            ViewData["TRAINING_CATAGORY_SELECT"] = DB.TRTrainingCategories.ToList();
            ViewData["COURSE_CATAGORY_SELECT"] = DB.TRCourseCategories.ToList();

        }
        #endregion

    }
}