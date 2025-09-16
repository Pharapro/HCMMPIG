using DevExpress.Spreadsheet;
using DevExpress.Web.Mvc;
using Humica.Core.BS;
using Humica.Core.DB;
using Humica.Core.FT;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.Att;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.Attendance.Attendance
{
    public class ESSUploadRosterController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ESS0000012";
        private const string URL_SCREEN = "/SelfService/MyTeam/ESSUploadRoster/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "DocumentNo";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public ESSUploadRosterController()
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
            UserConfListAndForm(this.KeyName);
            DataSelector();

            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ListImpRoster = new List<ATImpRosterHeader>();
            var ListImpRoster = DB.ATImpRosterHeaders.ToList();
            var Emp = DB.HRStaffProfiles.Where(w => w.FirstLine == user.UserName).ToList();
            if (Emp != null)
            {
                // Assuming Emp has a property called UserName that you want to match with UploadBy
                BSM.ListImpRoster = ListImpRoster
                    .Where(w => w.UploadBy == user.UserName || Emp.Any(e => e.EmpCode == w.UploadBy))
                    .OrderByDescending(w => w.DocumentNo)
                    .ToList();
            }
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListImpRoster);
        }

        #endregion
        #region "Details"
        public ActionResult Details(string id)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.FMonthly = new FTMonthlySum();
            BSM.LIstEmplSch = new List<ListEmpSch>();
            BSM.RosterHeader = new ATImpRosterHeader();
            if (id.ToString() != "null")
            {
                BSM.RosterHeader = DB.ATImpRosterHeaders.FirstOrDefault(w => w.DocumentNo == id);
                if (BSM.RosterHeader != null)
                {
                    BSM.ListApproval = DB.ExDocApprovals.Where(w => w.DocumentNo == id && w.DocumentType == "ROSTER").ToList();
                    BSM.LIstEmplSch = BSM.LoadEmpImport(id);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);

        }
        #endregion
        #region "Import"

        public ActionResult GridItems()
        {
            ActionName = "Import";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.LIstEmplSch = new List<ListEmpSch>();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }

        public ActionResult Import()
        {
            UserSession();
            DataSelector();
            ActionName = "Import";
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ESSUploadRoster", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objATEmpSch = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objATEmpSch = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            objATEmpSch.Filter = new FTFilterEmployee();
            objATEmpSch.Period = DateTime.Now;

            objATEmpSch.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();

            objATEmpSch.LIstEmplSch = new List<ListEmpSch>();
            #region Close
            //if (objATEmpSch.ListTemplate.Count > 0)
            //{
            //    SYExcel excel = new SYExcel();
            //    if (objATEmpSch.ListTemplate.Where(w => w.IsGenerate == false).Any())
            //    {
            //        excel.FileName = objATEmpSch.ListTemplate.Where(w => w.IsGenerate == false).First().UpoadPath;
            //    }
            //    else
            //        excel.FileName = objATEmpSch.ListTemplate.First().UpoadPath;
            //    //foreach (var read in objATEmpSch.ListTemplate.ToList())
            //    //{
            //    //    excel.FileName = read.UpoadPath;
            //    //}
            //    DataTable dtHeader = excel.GenerateExcelData();

            //    var shift = DBV.HLCheckShiftImports.ToList();
            //    DateTime LastDate = new DateTime(objATEmpSch.Period.Year, objATEmpSch.Period.Month,
            //    DateTime.DaysInMonth(objATEmpSch.Period.Year, objATEmpSch.Period.Month));
            //    if (dtHeader != null)
            //    {
            //        for (int i = 0; i < dtHeader.Rows.Count; i++)
            //        {
            //            var objHeader = new ListEmpSch();

            //            objHeader.EmpCode = dtHeader.Rows[i][0].ToString();
            //            objHeader.AllName = dtHeader.Rows[i][1].ToString();
            //            objHeader.Position = dtHeader.Rows[i][2].ToString();
            //            objHeader.D_1 = dtHeader.Rows[i][3].ToString();
            //            objHeader.D_2 = dtHeader.Rows[i][4].ToString();
            //            objHeader.D_3 = dtHeader.Rows[i][5].ToString();
            //            objHeader.D_4 = dtHeader.Rows[i][6].ToString();
            //            objHeader.D_5 = dtHeader.Rows[i][7].ToString();
            //            objHeader.D_6 = dtHeader.Rows[i][8].ToString();
            //            objHeader.D_7 = dtHeader.Rows[i][9].ToString();
            //            objHeader.D_8 = dtHeader.Rows[i][10].ToString();
            //            objHeader.D_9 = dtHeader.Rows[i][11].ToString();
            //            objHeader.D_10 = dtHeader.Rows[i][12].ToString();
            //            objHeader.D_11 = dtHeader.Rows[i][13].ToString();
            //            objHeader.D_12 = dtHeader.Rows[i][14].ToString();
            //            objHeader.D_13 = dtHeader.Rows[i][15].ToString();
            //            objHeader.D_14 = dtHeader.Rows[i][16].ToString();
            //            objHeader.D_15 = dtHeader.Rows[i][17].ToString();
            //            objHeader.D_16 = dtHeader.Rows[i][18].ToString();
            //            objHeader.D_17 = dtHeader.Rows[i][19].ToString();
            //            objHeader.D_18 = dtHeader.Rows[i][20].ToString();
            //            objHeader.D_19 = dtHeader.Rows[i][21].ToString();
            //            objHeader.D_20 = dtHeader.Rows[i][22].ToString();
            //            objHeader.D_21 = dtHeader.Rows[i][23].ToString();
            //            objHeader.D_22 = dtHeader.Rows[i][24].ToString();
            //            objHeader.D_23 = dtHeader.Rows[i][25].ToString();
            //            objHeader.D_24 = dtHeader.Rows[i][26].ToString();
            //            objHeader.D_25 = dtHeader.Rows[i][27].ToString();
            //            objHeader.D_26 = dtHeader.Rows[i][28].ToString();
            //            objHeader.D_27 = dtHeader.Rows[i][29].ToString();
            //            objHeader.D_28 = dtHeader.Rows[i][30].ToString();
            //            if (LastDate.Day >= 29)
            //                objHeader.D_29 = dtHeader.Rows[i][31].ToString();
            //            if (LastDate.Day >= 30)
            //                if (dtHeader.Columns.Count > 32)
            //                    objHeader.D_30 = dtHeader.Rows[i][32].ToString();
            //            if (LastDate.Day == 31)
            //                if (dtHeader.Columns.Count > 33)
            //                    objHeader.D_31 = dtHeader.Rows[i][33].ToString();

            //            #region Check Code if not exsit

            //            if (objHeader.D_1 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_1.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_1 = "Error";
            //                }
            //            }

            //            if (objHeader.D_2 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_2.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_2 = "Error";
            //                }
            //            }

            //            if (objHeader.D_3 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_3.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_3 = "Error";
            //                }
            //            }

            //            if (objHeader.D_4 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_4.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_4 = "Error";
            //                }
            //            }

            //            if (objHeader.D_5 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_5.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_5 = "Error";
            //                }
            //            }

            //            if (objHeader.D_6 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_6.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_6 = "Error";
            //                }
            //            }

            //            if (objHeader.D_7 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_7.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_7 = "Error";
            //                }
            //            }

            //            if (objHeader.D_8 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_8.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_8 = "Error";
            //                }
            //            }

            //            if (objHeader.D_9 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_9.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_9 = "Error";
            //                }
            //            }

            //            if (objHeader.D_10 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_10.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_10 = "Error";
            //                }
            //            }

            //            if (objHeader.D_11 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_11.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_11 = "Error";
            //                }
            //            }

            //            if (objHeader.D_12 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_12.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_12 = "Error";
            //                }
            //            }

            //            if (objHeader.D_13 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_13.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_13 = "Error";
            //                }
            //            }

            //            if (objHeader.D_14 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_14.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_14 = "Error";
            //                }
            //            }

            //            if (objHeader.D_15 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_15.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_15 = "Error";
            //                }
            //            }

            //            if (objHeader.D_16 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_16.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_16 = "Error";
            //                }
            //            }

            //            if (objHeader.D_17 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_17.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_17 = "Error";
            //                }
            //            }

            //            if (objHeader.D_18 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_18.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_18 = "Error";
            //                }
            //            }

            //            if (objHeader.D_19 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_19.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_19 = "Error";
            //                }
            //            }

            //            if (objHeader.D_20 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_20.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_20 = "Error";
            //                }
            //            }

            //            if (objHeader.D_21 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_21.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_21 = "Error";
            //                }
            //            }

            //            if (objHeader.D_22 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_22.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_22 = "Error";
            //                }
            //            }

            //            if (objHeader.D_23 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_23.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_23 = "Error";
            //                }
            //            }

            //            if (objHeader.D_24 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_24.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_24 = "Error";
            //                }
            //            }

            //            if (objHeader.D_25 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_25.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_25 = "Error";
            //                }
            //            }
            //            if (objHeader.D_26 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_26.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_26 = "Error";
            //                }
            //            }

            //            if (objHeader.D_27 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_27.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_27 = "Error";
            //                }
            //            }

            //            if (objHeader.D_28 != "")
            //            {
            //                var getShift = shift.Where(w => w.Code == objHeader.D_28.ToString()).ToList();
            //                if (getShift.Count == 0)
            //                {
            //                    objHeader.Er_D_28 = "Error";
            //                }
            //            }

            //            if (objHeader.D_29 != "")
            //            {
            //                if (objHeader.D_29 != null)
            //                {
            //                    var getShift = shift.Where(w => w.Code == objHeader.D_29.ToString()).ToList();
            //                    if (getShift.Count == 0)
            //                    {
            //                        objHeader.Er_D_29 = "Error";
            //                    }
            //                }
            //            }

            //            if (objHeader.D_30 != "")
            //            {
            //                if (objHeader.D_30 != null)
            //                {
            //                    var getShift = shift.Where(w => w.Code == objHeader.D_30.ToString()).ToList();
            //                    if (getShift.Count == 0)
            //                    {
            //                        objHeader.Er_D_30 = "Error";
            //                    }
            //                }
            //            }

            //            if (objHeader.D_31 != "")
            //            {
            //                if (objHeader.D_31 != null)
            //                {
            //                    var getShift = shift.Where(w => w.Code == objHeader.D_31.ToString()).ToList();
            //                    if (getShift.Count == 0)
            //                    {
            //                        objHeader.Er_D_31 = "Error";
            //                    }
            //                }
            //            }

            //            #endregion
            //            objATEmpSch.LIstEmplSch.Add(objHeader);
            //        }
            //    }


            //}
            #endregion
            Session[Index_Sess_Obj + ActionName] = objATEmpSch;
            return View(objATEmpSch);
        }

        [HttpPost]
        public ActionResult UploadControlCallbackAction(HttpPostedFileBase file_Uploader)
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ESSUploadRoster", SYSConstant.DEFAULT_UPLOAD_LIST);
            SYFileImport sfi = new SYFileImport(DB.CFUploadPaths.Find("SCHEDULE"));
            sfi.ObjectTemplate = new MDUploadTemplate();
            sfi.ObjectTemplate.UploadDate = DateTime.Now;
            sfi.ObjectTemplate.ScreenId = SCREEN_ID;
            sfi.ObjectTemplate.UploadBy = user.UserName;
            var staff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (staff != null)
                sfi.ObjectTemplate.UploadByName = staff.AllName;
            sfi.ObjectTemplate.Module = "ATTENDANCE";
            sfi.ObjectTemplate.IsGenerate = false;

            DevExpress.Web.UploadedFile[] files = UploadControlExtension.GetUploadedFiles("FileUploadOPB",
                sfi.ValidationSettings,
                sfi.uc_FileUploadComplete);

            var BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            BSM.LIstEmplSch = new List<ListEmpSch>();

            Session[Index_Sess_Obj + ActionName] = BSM;
            return Redirect(SYUrl.getBaseUrl() + ScreenUrl + "Import");
        }

        public ActionResult UploadList()
        {
            UserSession();
            ActionName = "Import";
            this.ScreendIDControl = SYSConstant.DEFAULT_UPLOAD_LIST;
            UserConfListAndForm(ActionBehavior.IMPORT, "UploadName", "ESSUploadRoster", SYSConstant.DEFAULT_UPLOAD_LIST);

            var objStaff = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objStaff = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }

            objStaff.ListTemplate = DB.MDUploadTemplates.Where(w => w.ScreenId == SCREEN_ID && w.UploadBy == user.UserName).OrderByDescending(w => w.UploadDate).ToList();
            Session[Index_Sess_Obj + ActionName] = objStaff;
            return PartialView(SYListConfuration.ListDefaultUpload, objStaff.ListTemplate);
        }

        [HttpGet]
        public ActionResult GenerateUpload(int id)
        {
            UserSession();
            ActionName = "Import";
            MDUploadTemplate obj = DB.MDUploadTemplates.Find(id);
            HumicaDBContext DBB = new HumicaDBContext();
            if (obj != null)
            {
                SYExcel excel = new SYExcel();
                excel.FileName = obj.UpoadPath;
                DataTable dtHeader = excel.GenerateExcelData();
                if (obj.IsGenerate == true)
                {
                    SYMessages mess = SYMessages.getMessageObject("FILE_RG", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }
                if (dtHeader != null)
                {
                    ATEmpSchObject BSM = new ATEmpSchObject();
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                    }
                    BSM.ScreenId = ScreendIDControl;
                    BSM.LIstEmplSch = new List<ListEmpSch>();
                    BSDocConfg DocBatch = new BSDocConfg("BATCH_UPLOAD", DocConfType.Normal, true);
                    try
                    {

                        DateTime create = DateTime.Now;
                        if (dtHeader.Rows.Count > 0)
                        {

                            var shift = DBV.HLCheckShiftImports.ToList();
                            shift = shift.ToList();
                            DateTime InDate = BSM.Period;
                            DateTime FromDate = new DateTime(InDate.Year, InDate.Month, 1);
                            DateTime ToDate = new DateTime(InDate.Year, InDate.Month, DateTime.DaysInMonth(InDate.Year, InDate.Month));
                            var ListTemp_Roster = new List<Temp_Roster>();
                            for (int i = 0; i < dtHeader.Rows.Count; i++)
                            {
                                var objHeader = new ListEmpSch();
                                for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
                                {
                                    int Count = day.Day;
                                    var _Temp_Roster = new Temp_Roster();
                                    _Temp_Roster.EmpCode = dtHeader.Rows[i][0].ToString();
                                    _Temp_Roster.Shift = dtHeader.Rows[i][2 + Count].ToString().Trim().ToUpper();
                                    _Temp_Roster.InDate = day;
                                    if (_Temp_Roster.Shift.Trim() != "")
                                    {
                                        var getShift = shift.Where(w => w.Code == _Temp_Roster.Shift).ToList();
                                        if (getShift.Count == 0)
                                        {
                                            obj.Message = "";
                                            obj.Message += "ID: " + _Temp_Roster.EmpCode + " :day " + _Temp_Roster.InDate.Day.ToString() + ": Shift:" + _Temp_Roster.Shift;
                                            obj.IsGenerate = false;
                                            DB.MDUploadTemplates.Attach(obj);
                                            DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                            DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                            DB.SaveChanges();
                                            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                                            //Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                                            //return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                                        }
                                    }
                                    ListTemp_Roster.Add(_Temp_Roster);
                                }
                            }
                            var msg = BSM.ImportdRosterTem(ListTemp_Roster, DocBatch.NextNumberRank, obj);
                            if (msg != SYConstant.OK)
                            {
                                obj.Message = SYMessages.getMessage(msg);
                                obj.Message += ":" + BSM.MessageError;
                                obj.IsGenerate = false;
                                DB.MDUploadTemplates.Attach(obj);
                                DB.Entry(obj).Property(w => w.Message).IsModified = true;
                                DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                                DB.SaveChanges();
                                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
                            }
                        }
                        obj.Message = SYConstant.OK;
                        obj.DocumentNo = DocBatch.NextNumberRank;
                        obj.IsGenerate = true;
                        DBB.MDUploadTemplates.Attach(obj);
                        DBB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DBB.Entry(obj).Property(w => w.DocumentNo).IsModified = true;
                        DBB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DBB.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserID.ToString();
                        log.DocurmentAction = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e);
                        /*----------------------------------------------------------*/
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();
                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.ScreenId = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/

                    }
                    catch (Exception e)
                    {
                        obj.Message = e.Message;
                        obj.IsGenerate = false;
                        DB.MDUploadTemplates.Attach(obj);
                        DB.Entry(obj).Property(w => w.Message).IsModified = true;
                        DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                        DB.SaveChanges();

                        /*------------------SaveLog----------------------------------*/
                        SYEventLog log = new SYEventLog();
                        log.ScreenId = SCREEN_ID;
                        log.UserId = user.UserName.ToString();
                        log.DocurmentAction = "OT_UPLOAD";
                        log.Action = SYActionBehavior.ADD.ToString();

                        SYEventLogObject.saveEventLog(log, e, true);
                        /*----------------------------------------------------------*/
                    }
                }
                else
                {
                    obj.Message = SYMessages.getMessage("EX_DT");
                    obj.IsGenerate = false;
                    DB.MDUploadTemplates.Attach(obj);
                    DB.Entry(obj).Property(w => w.Message).IsModified = true;
                    DB.Entry(obj).Property(w => w.IsGenerate).IsModified = true;
                    DB.SaveChanges();

                }
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "/Import");
        }

        public ActionResult DownloadTemplate()
        {
            UserSession();
            ActionName = "Import";
            var objATEmpSch = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                objATEmpSch = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                if (objATEmpSch.LIstEmplSch != null)
                {
                    objATEmpSch.LIstEmplSch.Clear();
                }
            }
            DateTime FromDate = new DateTime(objATEmpSch.Period.Year, objATEmpSch.Period.Month, 1);
            DateTime ToDate = new DateTime(objATEmpSch.Period.Year, objATEmpSch.Period.Month,
                DateTime.DaysInMonth(objATEmpSch.Period.Year, objATEmpSch.Period.Month));

            List<ExCFUploadMapping> _List = new List<ExCFUploadMapping>();
            _List.Add(new ExCFUploadMapping { Caption = "Employee Code", FieldName = "Employee Code" });
            _List.Add(new ExCFUploadMapping { Caption = "Employee Name", FieldName = "Employee Name" });
            _List.Add(new ExCFUploadMapping { Caption = "Position", FieldName = "Position" });
            for (var day = FromDate.Date; day.Date <= ToDate.Date; day = day.AddDays(1))
            {
                string Name = day.Day.ToString() + "\r\n" + string.Format("{0:ddd}", day);
                _List.Add(new ExCFUploadMapping { Caption = Name, FieldName = Name });
            }
            var HOD_ = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == user.UserName);
            if (HOD_ != null)
            {
                var dtHeader = DB.HRStaffProfiles.Where(w => (w.Status == "A" || (w.DateTerminate > FromDate && w.Status == "I"))
                                && (w.HODCode == HOD_.EmpCode || w.FirstLine == HOD_.EmpCode || w.SecondLine == HOD_.EmpCode || w.EmpCode == HOD_.EmpCode)).ToList();
                var Pos = DB.HRPositions.ToList();
                //var ListBranch = SYConstant.getBranchDataAccess();
                //dtHeader = dtHeader.Where(w => ListBranch.Where(x => x.Code == w.Branch).Any()).ToList();
                //var ListDepartment = ClsConstant.getDepartmentDataAccess();
                //dtHeader = dtHeader.Where(w => ListDepartment.Where(x => x.DeptCode == w.LOCT).Any()).ToList();
                dtHeader = dtHeader.Where(w => ((w.StartDate.Value.Date <= ToDate.Date && w.Status == "A") ||   (w.DateTerminate > FromDate && w.Status == "I")) && w.IsExceptScan != true).ToList();
                var ShiftData = DBV.HLCheckShiftImports.ToList();

                if (dtHeader.Count == 0)
                {
                    SYMessages mess = SYMessages.getMessageObject("NOEMP", user.Lang);
                    Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Import");
                }

                using (var workbook = new DevExpress.Spreadsheet.Workbook())
                {
                    // Ensure sheet names are unique
                    workbook.Worksheets[0].Name = "Master";
                    Worksheet worksheet = workbook.Worksheets[0];
                    var sheet2 = workbook.Worksheets.Add("Other_Information");

                    List<ExCFUploadMapping> _ListShift = new List<ExCFUploadMapping>();
                    _ListShift.Add(new ExCFUploadMapping { Caption = "Code", FieldName = "Code" });
                    _ListShift.Add(new ExCFUploadMapping { Caption = "Description", FieldName = "Description" });
                    _ListShift.Add(new ExCFUploadMapping { Caption = "Remark", FieldName = "Remark" });

                    List<ClsUploadMapping> _ListMaster = new List<ClsUploadMapping>();
                    foreach (var read in dtHeader)
                    {
                        string position = "";
                        if (read.JobCode != null)
                        {
                            if (Pos.Where(w => w.Code == read.JobCode).Any())
                                position = Pos.Where(w => w.Code == read.JobCode).First().Description;
                        }
                        _ListMaster.Add(new ClsUploadMapping
                        {
                            FieldName = read.EmpCode,
                            FieldName1 = read.AllName,
                            FieldName2 = position
                        });
                    }
                    List<ClsUploadMapping> _ListMaster1 = new List<ClsUploadMapping>();
                    foreach (var read in ShiftData)
                    {
                        _ListMaster1.Add(new ClsUploadMapping
                        {
                            FieldName = read.Code,
                            FieldName1 = read.Description,
                            FieldName2 = read.Remark
                        });
                    }
                    // Get data for each sheet
                    // Export data to each sheet with header formatting
                    ClsConstant.ExportDataToWorksheet(worksheet, _List);
                    ClsConstant.ExportDataToWorksheet(sheet2, _ListShift);
                    ClsConstant.ExportDataToWorksheetRow(worksheet, _ListMaster);
                    ClsConstant.ExportDataToWorksheetRow(sheet2, _ListMaster1);

                    // Save the workbook to a memory stream
                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveDocument(stream, DevExpress.Spreadsheet.DocumentFormat.Xlsx);
                        stream.Seek(0, System.IO.SeekOrigin.Begin);
                        DateTime DateNow = DateTime.Now;
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ROUSTER_TEMPLATE_" + DateNow.ToString("yyyy_MM_dd_hhmmss") + ".xlsx");
                    }
                }
            }
            return null;
        }

        #endregion
        #region "Approve"
        public ActionResult Approve(string id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (id.ToString() != "null")
            {
                string msg = BSM.ApporvalImport(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.Description = messageObject.Description + BSM.MessageError;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
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
        #region "Cancel"
        public ActionResult Cancel(string id)
        {
            this.UserSession();
            DataSelector();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID] = id;
            ATEmpSchObject BSD = new ATEmpSchObject();
            if (id.ToString() != "null")
            {
                string sms = BSD.CancelImport(id);
                if (sms == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(sms, user.Lang);
                    messageObject.DocumentNumber = id;
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(sms, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }

            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
        }
        #endregion
        [HttpPost]
        public string getPeriod(DateTime Date, string Action)
        {
            this.ActionName = Action;
            UserSession();
            UserConfListAndForm();

            ATEmpSchObject BSM = new ATEmpSchObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                DateTime date = new DateTime(Date.Year, Date.Month, 1);
                BSM.Period = date;
                Session[Index_Sess_Obj + ActionName] = BSM;
                return SYConstant.OK;
            }
            else
            {
                return SYMessages.getMessage("PLEASE_SEARCH_ALLOWANCE");
            }
        }
        public ActionResult GridApprovalDetail()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridApprovalDetail", BSM.ListApproval);
        }
        private void DataSelector()
        {
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();// DB.HRBranches.ToList();
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            var locations = ClsConstant.getDepartmentDataAccess();
            var ListLoct = DB.HRLocations.ToList();
            ListLoct = ListLoct.Where(w => locations.Where(x => x.DeptCode == w.Code).Any()).ToList();
            ViewData["LOCATION_SELECT"] = ListLoct.ToList();// DB.HRLocations.ToList();
        }
    }
}
