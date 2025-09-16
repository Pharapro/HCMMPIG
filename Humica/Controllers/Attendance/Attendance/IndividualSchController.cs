using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.Att;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.Attendance
{

    public class IndividualSchController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATM0000002";
        private const string URL_SCREEN = "/Attendance/Attendance/IndividualSch/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "Day";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public IndividualSchController()
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
            BSM.Filter = new Humica.Core.FT.FTFilterEmployee();
            BSM.ListDayInMonth = new List<DayInMonth>();
            BSM.Filter.INYear = DateTime.Now.Year;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                var obj = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            // BSM.ListDayInMonth = BSM.listDay();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATEmpSchObject BSM)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            BSM.listDay();
            BSM.listEmployeeRoster();
            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }

        #endregion
        public ActionResult EditSch(DayInMonth MModel)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            ATEmpSchObject BSM = new ATEmpSchObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
                    }
                    var _list = new List<ListMonth>();
                    _list.Add(new ListMonth { ID = 1, Months = MModel.Jan });
                    _list.Add(new ListMonth { ID = 2, Months = MModel.Feb });
                    _list.Add(new ListMonth { ID = 3, Months = MModel.Mar });
                    _list.Add(new ListMonth { ID = 4, Months = MModel.Apr });
                    _list.Add(new ListMonth { ID = 5, Months = MModel.May });
                    _list.Add(new ListMonth { ID = 6, Months = MModel.Jun });
                    _list.Add(new ListMonth { ID = 7, Months = MModel.Jul });
                    _list.Add(new ListMonth { ID = 8, Months = MModel.Aug });
                    _list.Add(new ListMonth { ID = 9, Months = MModel.Sep });
                    _list.Add(new ListMonth { ID = 10, Months = MModel.Oct });
                    _list.Add(new ListMonth { ID = 11, Months = MModel.Nov });
                    _list.Add(new ListMonth { ID = 12, Months = MModel.Dec });
                    BSM.HeaderDayInMonth = MModel;
                    var msg = BSM.CreateShift(_list);
                    if (msg == SYSConstant.OK)
                    {
                        var list = BSM.ListDayInMonth.Where(x => x.Day == MModel.Day).ToList();
                        if (list.Count > 0)
                        {
                            var ObjUpdate = list.First();
                            ObjUpdate.Jan = MModel.Jan;
                            ObjUpdate.Feb = MModel.Feb;
                            ObjUpdate.Mar = MModel.Mar;
                            ObjUpdate.Apr = MModel.Apr;
                            ObjUpdate.May = MModel.May;
                            ObjUpdate.Jun = MModel.Jun;
                            ObjUpdate.Jul = MModel.Jul;
                            ObjUpdate.Aug = MModel.Aug;
                            ObjUpdate.Sep = MModel.Sep;
                            ObjUpdate.Oct = MModel.Oct;
                            ObjUpdate.Nov = MModel.Nov;
                            ObjUpdate.Dec = MModel.Dec;
                        }
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage(msg);
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
            DataSelector();
            return PartialView("GridItems", BSM.ListDayInMonth);
        }
        public ActionResult GridItems()
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(ActionBehavior.ACC_REV);
            ATEmpSchObject BSM = new ATEmpSchObject();
            BSM.ScreenId = SCREEN_ID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ATEmpSchObject)Session[Index_Sess_Obj + ActionName];
            }
            DataSelector();
            Session[SYConstant.CURRENT_URL] = URL_SCREEN + "GridItems";
            return PartialView("GridItems", BSM.ListDayInMonth);
        }
        public ActionResult ShowData(string ID, string EmpCode)
        {
            ActionName = "Details";
            var Stafff = DBV.HR_STAFF_VIEW;
            HR_STAFF_VIEW EmpStaff = Stafff.FirstOrDefault(w => w.EmpCode == EmpCode);
            if (EmpStaff != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    AllName = EmpStaff.AllName,
                    EmpType = EmpStaff.EmpType,
                    Division = EmpStaff.Division,
                    DEPT = EmpStaff.Department,
                    SECT = EmpStaff.Section,
                    LevelCode = EmpStaff.LevelCode,
                    Position = EmpStaff.Position,
                    StartDate = EmpStaff.StartDate
                };

                return Json(result, JsonRequestBehavior.DenyGet);

            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector(params object[] keys)
        {
            var BSM = new ATEmpSchObject();
            foreach (var data in BSM.OnDataLoadingOther())
            {
                ViewData[data.Key] = data.Value;
            }
        }
    }
}
