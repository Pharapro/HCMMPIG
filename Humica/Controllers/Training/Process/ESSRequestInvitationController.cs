using DevExpress.Web.Mvc;
using DevExpress.Web;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Humica.Core.DB;
using Humica.Models.Report.Training;
using System.IO;
using Humica.Controllers.Reporting.Training;

namespace Humica.Controllers.Training.Process
{
    public class ESSRequestInvitationController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TRA0000004";
        const string DOC_TYPE = "NSV03";
        private const string URL_SCREEN = "/Training/Process/ESSRequestInvitation/";
        private string status = SYDocumentStatus.PENDING.ToString();
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName = "";
        private string KeyName = "ID";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Humica.Core.DB.HumicaDBContext DB = new Humica.Core.DB.HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public ESSRequestInvitationController()
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
            DataListSelector();
            UserConfList(ActionBehavior.LISTR, KeyName, "ESSRequestInvitation");

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
                BSM.Filter = BSM.Filter;
            }
            BSM.ListRequestHeader = new List<TrainingRequestHeader>();
            string pending = SYDocumentStatus.PENDING.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            string approved = SYDocumentStatus.APPROVED.ToString();
            BSM.ListRequestItem = DBX.TrainingRequestItem.Where(w => w.Status != pending && w.EmpCode == user.UserName).OrderByDescending(w => w.TrainNo).ToList();
            var ListHeader = DBX.TrainingRequestHeader.ToList();
            var trainingItems = DBX.TrainingRequestItem.AsEnumerable();
            BSM.ListWaitingRequestItem = trainingItems.Where(w => ListHeader.Any(x => x.TrainNo == w.TrainNo && x.Status == pending && w.Status == pending && w.EmpCode == user.UserName)).ToList();
            
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(TrainningObject BSM)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            //BSM.Filter.Status = SYDocumentStatus.PENDING.ToString();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSRequestInvitation");

            string pending = SYDocumentStatus.PENDING.ToString();
            string received = SYDocumentStatus.RECEIVED.ToString();
            string open = SYDocumentStatus.OPEN.ToString();
            foreach (var r in BSM.ListRequestItem)
            {
                DBX.TrainingRequestItem.Attach(r);
            }
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            DataListSelector();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSRequestInvitation");
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListWaitingRequestItem);
        }
        public ActionResult PartialList1()
        {
            ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            DataListSelector();
            UserConfList(ActionBehavior.LISTR, "ID", "ESSRequestInvitation");
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList1", BSM.ListRequestItem);
        }
        #endregion
        #region Detials
        public ActionResult Details(int id)
        {
            ActionName = "Details";
            this.UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "ID", "ESSRequestInvitation");
            DataListSelector();
            TrainningObject BSD = new TrainningObject();
            string OPEN = SYDocumentStatus.OPEN.ToString();
            BSD.ListApproval = new List<ExDocApproval>();
            BSD.ListSchedule = new List<ESSTrainingSchedule>();
            BSD.RequestHeader = new TrainingRequestHeader();
            BSD.RequestItem = new TrainingRequestItem();
            BSD.HeaderStaff = new HR_STAFF_VIEW();
            BSD.ESSTrainingSchedules = new ESSTrainingSchedule();

            BSD.RequestHeader = DBX.TrainingRequestHeader.SingleOrDefault(w => w.ID == id);
            BSD.RequestHeader = DBX.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);
            var Requester = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == BSD.RequestHeader.RequestCode);
            BSD.HeaderStaff.EmpCode = BSD.RequestHeader.RequestCode;
            BSD.HeaderStaff.AllName = BSD.RequestHeader.RequesterName;
            BSD.HeaderStaff.Department = Requester.Department;
            BSD.HeaderStaff.Position = Requester.Position;
            BSD.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == BSD.RequestHeader.TrainNo && w.DocumentType == BSD.RequestHeader.DocType).ToList();
            var ListSchedule = DBX.ESSTrainingSchedule.Where(w => w.TrainNo == BSD.RequestHeader.TrainNo).ToList();
            BSD.ListSchedule = ListSchedule;
            Session[Index_Sess_Obj + ActionName] = BSD;
            if (BSD.RequestHeader != null)
            {
                BSD.ListRequestItem = DBX.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode && w.ID == id).ToList();
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
            return PartialView("GridItemSchedule", BSM.ListSchedule);
        }
        public ActionResult EditItems(TrainingRequestItem obj)
        {
            ActionName = "Details";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            var objCheck = BSM.ListRequestItem.Where(w => w.LineItem == obj.LineItem).ToList();

            if (objCheck.Count > 0)
            {
                //jCheck.First().Comment = obj.Comment;

            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemDetails", BSM.ListRequestItem);
        }
        #endregion

        public ActionResult UploadControlCallbackActionImage(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            var path = DB.CFUploadPaths.Find("IMG_UPLOAD");
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
                    BSM.ListRequestItem = BSM.ListRequestItem.OrderBy(w=>w.EmpCode).ToList();
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
                        //BSM.ListRequestHeader.Remove(objCheck);
                        //BSM.ListRequestHeader.OrderBy(x => x.re);
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

            BSM.GetAllStaff();
            return PartialView("gvemployeeselector", BSM);

        }
        public ActionResult PartialEmployeeSearch()
        {
            ActionName = "Create";
            UserSession();
            DataListSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
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
            if(BSM.ListRequestItem == null)
            {
                BSM.ListRequestItem = new List<TrainingRequestItem>();
            }
            if (EmpCode != null)
            {
                string[] empCodes = EmpCode.Split(',');
                var staff = DB.HRStaffProfiles.ToList();
                List<Core.DB.HRDepartment> ListDepartment = DB.HRDepartments.ToList();
                List<Core.DB.HRPosition> ListPosition = DB.HRPositions.ToList();
                TrainingRequestHeader bsm = new TrainingRequestHeader(); 
                var list = staff.Where(w => empCodes.Contains(w.EmpCode));
                int i = 0;
                i= BSM.ListRequestItem.Count();
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

        public ActionResult Accept(int id)
        {
            ActionName = "Details";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataListSelector();
            if (id != null)
            {
                TrainningObject BSM = new TrainningObject();
                BSM.RequestItem = new TrainingRequestItem();

                BSM.ScreenId = SCREENID;
                string Pending = SYDocumentStatus.PENDING.ToString();
                string msg = BSM.ESSAccept(id);

                if (msg == SYConstant.OK)
                {
                    var Header = DBX.TrainingRequestHeader.FirstOrDefault(w => w.ID == id);
                    var ListTrainee = DBX.TrainingRequestItem.Where(w => w.TrainNo == Header.TrainNo && w.Status == Pending).ToList();
                    if (ListTrainee.Count == 0)
                    {
                        #region template
                        ESS_FRM_TRAINING_REQUEST sa = new ESS_FRM_TRAINING_REQUEST();
                        string receiver = string.Empty;
                        int approvelevel = 0;
                        receiver = DB.ExDocApprovals.Where(w => w.DocumentNo == Header.TrainNo).OrderBy(w => w.ApproveLevel).FirstOrDefault().Approver;
                        //receiver = ListApp.Employee;
                        var objRpt = DB.CFReportObjects.Where(w => w.ScreenID == SCREENID && w.IsActive == true).ToList();
                        if (objRpt.Count > 0)
                        {
                            sa.LoadLayoutFromXml(ClsConstant.DEFAULT_REPORT_PATH + objRpt.First().ReportObject);
                        }
                        var _Appro = DBX.TrainingRequestHeader.FirstOrDefault(w => w.TrainNo == Header.TrainNo);
                        sa.Parameters["TrainNo"].Value = Header.TrainNo;
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
                        BSM.SendEmail(Header.TrainNo,fileName, receiver);
                        #endregion
                    }
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id.ToString();
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_ACCEPT", user.Lang);
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
        #region "Reject"
        public ActionResult Reject(int id)
        {
            UserSession();
            if (id != null)
            {
                TrainningObject BSM = new TrainningObject();

                BSM.ScreenId = SCREENID;
                string msg = BSM.ESSReject(id);

                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id.ToString();
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_RJ", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
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
        #region "Private Code"
        private void DataSelectorForFilter()
        {

        }
        private void DataListSelector()
        {
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
            var listReason = DB.CFReasonCodes.Where(w => w.Indicator == "H").ToList();
            foreach (var read in listReason)
            {
                read.Description1 = read.Description2 + "-" + read.Description1;
            }
            ViewData["REASON_CANCEL"] = listReason;
            //ViewData["Course_List"] = DBX.TrainingCourse.ToList();
            ViewData["Program_List"] = DBX.TrainingProgram.ToList();
            ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();
            ViewData["Target_List"] = DBX.TRTrainingRequirements.Where(w => w.Category == "T").ToList();
            ViewData["Venue_List"] = DBX.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            ViewData["Dayterm_List"] = DBX.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            ViewData["Group_List"] = DBX.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            ViewData["Requirement_List"] = DBX.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            ViewData["Staff_List"] = DB.HRStaffProfiles.ToList();

            ViewData["COURCE_SELECT"] = DBX.TRTrainingCourses.ToList();
            ViewData["TRAINING_TYPE"] = DBX.TRTrainingTypes.ToList();
            ViewData["DEPT_SELECT"] = DB.HRDepartments.ToList();
            ViewData["TRAINING_CATAGORY_SELECT"] = DBX.TRTrainingCategories.ToList();

        }
        #endregion



    }
}