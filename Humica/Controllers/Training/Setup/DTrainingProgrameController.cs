using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.SetUp
{
    public class DTrainingProgrameController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRD000001";
        private const string URL_SCREEN = "/Training/Process/DTrainingPrograme/";
        const string DOC_TYPE = "SPC01";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string Index_Sess_Msg = SYSConstant.SAVE_COMPLETED + SCREEN_ID;
        private string ActionName = "";
        private string PARAM_INDEX = "ProgramCode;Status";
        private string TR_REF = "NTR01";
        private string PATH_FILE = "dkslkds";
        private string ClaimQTY = "dkdsfds";
        private string DOCTYPE = "RQC01";
        private string status = SYDocumentStatus.OPEN.ToString();
        HumicaDBContextTR DB = new HumicaDBContextTR();
        HumicaDBViewContext DBV = new HumicaDBViewContext();
        HumicaDBContext DBX = new HumicaDBContext();
        public DTrainingProgrameController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region LIST
        public ActionResult Index()
        {
            UserSession();
            DataSelectorForFilter();
            this.ActionName = "Index";
            TrainningObject BSM = new TrainningObject();
            UserSession();
            UserConfListAndForm();
            UserConfFormFitler();
            string released = SYDocumentStatus.RELEASED.ToString();

            BSM.ListRequestHeader = DB.TrainingRequestHeader.ToList();
            BSM.ListInvitation = DB.TRTrainingInvitations.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            //Session["ISFORCE"] = null;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            this.ActionName = "Index";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            //UserConfList(ActionBehavior.LIST_ADD, "RequestCode", "DTrainingPrograme");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialList", BSM.ListRequestHeader);
        }
        public ActionResult GridCourse()
        {
            this.ActionName = "Index";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            //UserConfList(ActionBehavior.LIST_ADD, "ID", "DTrainingPrograme");
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridCourse", BSM.ListInvitation);
        }
        #endregion


        #region Create
        public ActionResult Create(string CourseCode, string Program)
        {
            ActionName = "Create";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            TrainningObject BSD = new TrainningObject();
            BSD.RequestHeader = new TrainingRequestHeader();
            BSD.ListRequestItem = new List<TrainingRequestItem>();
            BSD.ListStaff = new List<Humica.Core.DB.HR_STAFF_VIEW>();
            BSD.RequestHeader.RequestDate = DateTime.Now;
            BSD.RequestHeader.Status = SYDocumentStatus.OPEN.ToString();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(TrainningObject Collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelectorForFilter();
            TrainningObject BSD = new TrainningObject();
            BSD = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            BSD.ListRequestHeader = Collection.ListRequestHeader;
            BSD.RequestHeader = Collection.RequestHeader;
            // BSD.DocType = DOCTYPE;

            if (ModelState.IsValid)
            {
                string msg = "";// BSD.SAVERequest();
                if (msg != "OK")
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSD;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSD.RequestHeader.RequestCode;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + ActionName] = BSD;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");

            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSD);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            ActionName = "Create";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            TrainningObject BSD = new TrainningObject();
            ViewData[SYConstant.PARAM_ID] = id;
            var Mont = DateTime.Now.Month;
            //BSD.RequestHeader = DBX.TrainingCourse.SingleOrDefault(w => w.Coursecode == id && w.Branch == "SV");
            string OPEN = SYDocumentStatus.OPEN.ToString();
            BSD.RequestHeader = DB.TrainingRequestHeader.SingleOrDefault(w => w.RequestCode == id && w.Status == OPEN);
            if (BSD.RequestHeader != null)
            {
                BSD.ListRequestItem = DB.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode).ToList();
                ViewData[SYConstant.PARAM_ID] = id;
                Session[Index_Sess_Obj + ActionName] = BSD;
                return View(BSD);
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            Session[Index_Sess_Obj + ActionName] = BSD;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(string id, TrainningObject Collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelectorForFilter();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            //BSD.ListHeader = DBX.TrainingCourse.Where(w => w.Coursecode == id && w.Branch == "SV").ToList();
            ViewData[SYConstant.PARAM_ID] = id;
            BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            BSM.RequestHeader = Collection.RequestHeader;
            if (ModelState.IsValid)
            {
                string msg = BSM.UpdateRequest(id);
                if (msg != "OK")
                {
                    SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    UserConfForm(ActionBehavior.SAVEGRID);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details/id=" + id);
                }
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = id;
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region Detials
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            this.UserSession();
            UserConfList(ActionBehavior.LIST_ADD, "RequestCode", "DTrainingPrograme");
            DataSelectorForFilter();
            TrainningObject BSD = new TrainningObject();
            string OPEN = SYDocumentStatus.OPEN.ToString();
            BSD.RequestHeader = DB.TrainingRequestHeader.SingleOrDefault(w => w.RequestCode == id);
            if (BSD.RequestHeader != null)
            {
                BSD.ListRequestItem = DB.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode).ToList();
                ViewData[SYConstant.PARAM_ID] = id;
                ViewData[SYConstant.DOCUMENT_STATUS] = OPEN;
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
            DataSelectorForFilter();
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
        #endregion


        #region Release
        public ActionResult RequestTrain(string id)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSD = new TrainningObject();
            if (id != null)
            {
                string open = SYDocumentStatus.OPEN.ToString();
                BSD.RequestHeader = DB.TrainingRequestHeader.FirstOrDefault(w => w.RequestCode == id && w.Status == open);
                if (BSD.RequestHeader != null)
                {
                    BSD.ListRequestItem = DB.TrainingRequestItem.Where(w => w.RequestCode == BSD.RequestHeader.RequestCode).ToList();
                    foreach (var r in BSD.ListRequestItem)
                    {
                        r.Status = SYDocumentStatus.PENDING.ToString();
                        DB.TrainingRequestItem.Attach(r);
                        DB.Entry(r).Property(w => w.Status).IsModified = true;
                    }
                    BSD.RequestHeader.Status = SYDocumentStatus.PENDING.ToString();
                    DB.TrainingRequestHeader.Attach(BSD.RequestHeader);
                    DB.Entry(BSD.RequestHeader).Property(w => w.Status).IsModified = true;
                    DB.SaveChanges();
                }
                string sms = SYSConstant.OK;
                if (sms == "OK")
                {
                    SYMessages mess_err = SYMessages.getMessageObject("REQUESTED", user.Lang);
                    Session[Index_Sess_Obj + this.ActionName] = BSD;
                    UserConfForm(ActionBehavior.RELEASE);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
            }
            else
            {
                Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #endregion

        #region GRIDITEM
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            //BSM.ListItems = new List<SPPOItem>();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM.ListStaff);
        }
        public ActionResult CreateItems(TrainingRequestItem ModelObject)
        {
            ActionName = "Create";
            UserSession();
            DataSelectorForFilter();
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
                    var Item = BSM.ListRequestItem.ToList();
                    var line = Item.Count + 1;
                    var check = BSM.ListRequestItem.Where(w => w.EmpCode == ModelObject.EmpCode).ToList();
                    if (check.Count == 0)
                    {
                        ModelObject.LineItem = line;
                        BSM.ListRequestItem.Add(ModelObject);
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
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
            //DataSelector();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        public ActionResult EditItems(TrainingRequestItem obj)
        {
            ActionName = "Create";
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
                objCheck.First().EmpCode = obj.EmpCode;

            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        public ActionResult DeleteItems(int LineItem)
        {
            ActionName = "Create";
            UserSession();
            DataSelectorForFilter();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];

            }

            var objCheck = BSM.ListRequestItem.Where(w => w.LineItem == LineItem).ToList();
            if (objCheck.Count > 0)
            {
                BSM.ListRequestItem.Remove(objCheck.First());
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        #endregion
        public ActionResult selectProgram(string Program)
        {
            UserSession();
            TrainningObject BSM = new TrainningObject();

            if (Session[Index_Sess_Obj + this.ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + this.ActionName];
            }
            if (Program == null || Program == "")
            {
                var rs1 = new { MS = SYConstant.FAIL };
                return Json(rs1, JsonRequestBehavior.DenyGet);
            }
            var lstCourse = DB.TrainingCourseMaster.Where(w => w.Program == Program).ToList();

            if (lstCourse.Count > 0)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    DATA = lstCourse.ToList(),
                };
                return Json(result, JsonRequestBehavior.DenyGet);

            }

            Session[Index_Sess_Obj + ActionName] = BSM;
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        //public ActionResult Selectstaff(string Actionname, string StaffID)
        //{
        //    ActionName = Actionname;
        //    UserSession();
        //    TrainningObject BSM = new TrainningObject();

        //    if (Session[Index_Sess_Obj + this.ActionName] != null)
        //    {
        //        BSM = (TrainningObject)Session[Index_Sess_Obj + this.ActionName];
        //    }
        //    var check1 = DB.HRStaffProfiles.Where(x => x.EmpCode == StaffID).ToList();
        //    var checkSLevel = DB.HRStaffProfiles.FirstOrDefault(x => x.EmpCode == StaffID);
        //    var check = DBV.HR_STAFF_VIEW.Where(w => w.EmpCode == StaffID).ToList();


        //    Session[Index_Sess_Obj + ActionName] = BSM;


        //    if (check.Count == 0)
        //    {
        //        var data1 = new { MS = "FAIL" };
        //        return Json(data1, (JsonRequestBehavior)1);
        //    }
        ////    else
        ////    {
        ////        var dat = DateTime.Now;
        ////        double? wp = 0;
        ////        if (check.First().StartDate != null)
        ////        {
        ////            wp = dat.Subtract((DateTime)check.First().StartDate).Days / (365.25 / 12);
        ////        }
        ////        var stafflevel = DBX.hrlev.Where(w => w.Code == checkSLevel.LevelCode).ToList();
        ////        string stl = "";
        ////        if (stafflevel.Count > 0)
        ////        {
        ////            stl = stafflevel.First().Description;
        ////        }
        ////        var data = new
        ////        {
        ////            MS = "OK",
        ////            NameEN = check.First().AllName,
        ////            NameKH = check.First().OthAllName,
        ////            Gender = check.First().Sex,
        ////            Position = check.First().Position,
        ////            Phone = check.First().Phone1,
        ////            NatID = check1.First().Nation,
        ////            HireDate = check.First().StartDate,
        ////            WorkExperience = wp,
        ////            StaffLevel = stl,
        ////        };
        ////        return Json(data, (JsonRequestBehavior)1);
        ////    }

        //}

        private void DataSelectorForFilter()
        {
            SYDataList DL = new SYDataList("CLAIM_TEYP_SELECT");
            ViewData["CLAIM_TEYP_SELECT"] = DL.ListData;
            //ViewData["STAFF_SELECT"] = DB.Staffs.Where(w => w.CompanyCode == bs.CompanyCode).ToList();
            // ViewData["TRUCK_TYPE"] = DB.CFDPTruckAllows.ToList();
            //var DL = new SYDataList("TRCODE_SELECT");
            ViewData["TRCODE_SELECT"] = DL.ListData;
            DL = new SYDataList("STATUS_APPROVAL");
            ViewData["STATUS_APPROVAL"] = DL.ListData;
            //var listReason = DB.CFReasonCodes.Where(w => w.Indicator == "H").ToList();
            //foreach (var read in listReason)
            //{
            //    read.Description1 = read.Description2 + "-" + read.Description1;
            //}
            //ViewData["REASON_CANCEL"] = listReason;
            ViewData["Course_List"] = DB.TRTrainingInvitations.ToList();
            ViewData["Program_List"] = DB.TrainingProgram.ToList();
            //ViewData["StaffLevel_List"] = DB.HRLevels.ToList();
            var ListReqData = DB.TRTrainingRequirements.ToList();
            ViewData["Target_List"] = ListReqData.Where(w => w.Category == "T").ToList();
            ViewData["Venue_List"] = ListReqData.Where(w => w.Category == "V").ToList();
            ViewData["Dayterm_List"] = ListReqData.Where(w => w.Category == "D").ToList();
            ViewData["Group_List"] = ListReqData.Where(w => w.Category == "G").ToList();
            ViewData["Requirement_List"] = ListReqData.Where(w => w.Category == "R").ToList();
            ViewData["Staff_List"] = DBV.HR_STAFF_VIEW.ToList();
        }

    }
}

