using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Setup
{
    public class TRTrainerTypeController : EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "TRM000008";
        private const string URL_SCREEN = "/Training/Setup/TRTrainerType/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "TrainNo";

        Core.DB.HumicaDBContext DB = new Core.DB.HumicaDBContext();
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        Core.DB.HumicaDBViewContext DBV = new Core.DB.HumicaDBViewContext();
        public TRTrainerTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region 'List'
        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            BSM.ListHeader = new List<TRTrainerType>();
            BSM.ListTrainerInfor = new List<TRTrainerInfo>();
            var trainertype = DBX.TRTrainerType;
            var trainerInfo = DBX.TRTrainerInfo.ToList();
            TRTrainerType ty = new TRTrainerType(trainerInfo);
            BSM.ListHeader = trainertype.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListHeader);
        }
        #endregion

        #region 'CREATE'
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();

            TRTrainerTypeObject BSD = new TRTrainerTypeObject();

            BSD.Header = new TRTrainerType();
            BSD.ListHeader = new List<TRTrainerType>();
            BSD.ListTrainerInfor = new List<TRTrainerInfo>();

            UserConfListAndForm();
            Session[Index_Sess_Obj + ActionName] = BSD;
            return View(BSD);
        }
        [HttpPost]
        public ActionResult Create(TRTrainerTypeObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            if (ModelState.IsValid)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
                    collection.ListTrainerInfor = BSM.ListTrainerInfor;
                    BSM.Header = collection.Header;
                }
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.CreateTrainerType();
                if (msg == SYConstant.OK)
                {
                    BSM.ScreenId = SCREEN_ID;
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = BSM.Header.TrainNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?TrainNo=" + mess.DocumentNumber;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    BSM.Header = new TRTrainerType();
                    BSM.ListTrainerInfor = new List<TRTrainerInfo>();
                    Session[Index_Sess_Obj + this.ActionName] = BSM;
                    return View(BSM);
                }

                Session[Index_Sess_Obj + this.ActionName] = BSM;
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                return View(BSM);
            }

            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region 'EDIT'
        public ActionResult Edit(string TrainNo)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = TrainNo;
            if (TrainNo != null)
            {
                TRTrainerTypeObject BSM = new TRTrainerTypeObject();
                int ID = Convert.ToInt32(TrainNo);
                BSM.Header = DBX.TRTrainerType.FirstOrDefault(w => w.TrainNo == ID);
                if (BSM.Header != null)
                {
                    BSM.InputType = BSM.Header.Code;
                    BSM.ListTrainerInfor = DBX.TRTrainerInfo.Where(w => w.TrainerTypeID == TrainNo).ToList();
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);

            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int TrainNo, TRTrainerTypeObject collection)
        {
            ActionName = "Create";
            UserSession();
            this.ScreendIDControl = SCREEN_ID;
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = TrainNo;
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            if (TrainNo != null)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
                }
                BSM.Header = collection.Header;
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditTrainerInfo(TrainNo);
                if (msg == SYConstant.OK)
                {
                    DBX = new HumicaDBContextTR();
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = TrainNo.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?TrainNo=" + mess.DocumentNumber;
                    ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }
        #endregion

        #region DETAILS
        public ActionResult Details(int TrainNo)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = TrainNo;

            if (TrainNo != null)
            {
                TRTrainerTypeObject BSM = new TRTrainerTypeObject();
                BSM.Header = DBX.TRTrainerType.FirstOrDefault(w => w.TrainNo == TrainNo);
                if (BSM.Header != null)
                {
                    string ID = TrainNo.ToString();
                    BSM.InputType = BSM.Header.Code;
                    BSM.ListTrainerInfor = DBX.TRTrainerInfo.Where(w => w.TrainerTypeID == ID).ToList();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridItemsDetails()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsDetails", BSM);
        }
        #endregion

        #region 'DELETE'
        public ActionResult Delete(int TrainNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();
            if (TrainNo != null)
            {
                TRTrainerTypeObject Del = new TRTrainerTypeObject();
                string msg = Del.DeleteTrainerInfo(TrainNo);
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

        #region 'GRID ITEM CREATE'
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateItems(TRTrainerInfo ModelObject, TRTrainerTypeObject Coll)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            try
            {
                if (ModelObject != null && !string.IsNullOrEmpty(ModelObject.TrainerCode))
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
                    }
                    BSM.ListTrainerInfor.Add(ModelObject);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItems", BSM);
        }
        public ActionResult EditItems(TRTrainerInfo obj)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
                var objCheck = BSM.ListTrainerInfor.Where(w => w.TrainNo == obj.TrainNo).ToList();
                if (objCheck.Count > 0)
                {
                    objCheck.First().TrainerCode = obj.TrainerCode;
                    objCheck.First().Position = obj.Position;
                    objCheck.First().Department = obj.Department;
                    objCheck.First().Email = obj.Email;
                    objCheck.First().StartDate = obj.StartDate;
                    objCheck.First().TrainerName = obj.TrainerName;
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult DeleteItems(int TrainNo)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];

                var objCheck = BSM.ListTrainerInfor.Where(w => w.TrainNo == TrainNo).ToList();
                if (objCheck.Count > 0)
                {
                    BSM.ListTrainerInfor.Remove(objCheck.First());
                }
                else
                {
                    ViewData["EditError"] = SYMessages.getMessage("HOUSE_NE");
                }
                Session[Index_Sess_Obj + ActionName] = BSM;
            }
            return PartialView("GridItems", BSM);
        }
        #endregion

        public ActionResult SelectTypeInput(string Actionname, string Code)
        {
            ActionName = Actionname;
            UserSession();
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();
            var listBranch = SYConstant.getBranchDataAccess();
            ViewData[SYSConstant.PARAM_ID] = Code.Trim();
            BSM.InputType = Code;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + this.ActionName];
                BSM.InputType = Code;
            }
            Session[Index_Sess_Obj + Actionname] = BSM;
            var data = new
            {
                MS = SYSConstant.OK,
            };
            return Json(data, (JsonRequestBehavior)1);
        }
        public ActionResult ShowData(string Code, string Action)
        {
            ActionName = Action;
            TRTrainerTypeObject BSM = new TRTrainerTypeObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TRTrainerTypeObject)Session[Index_Sess_Obj + ActionName];
            }
            var EmpInfor = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == Code);

            if (EmpInfor != null)
            {
                var result = new
                {
                    MS = SYConstant.OK,
                    Post = EmpInfor.Position,
                    Dept = EmpInfor.Department,
                    ConNum = EmpInfor.Phone1,
                    Email = EmpInfor.Email,
                    Name = EmpInfor.AllName,
                    //StartDate = EmpInfor.StartDate,
                };
                return Json(result, JsonRequestBehavior.DenyGet);
            }
            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            SYDataList DL = new SYDataList("TRAINER_TYPE");
            ViewData["LIST_TRAINERTYPE"] = DL.ListData;
            ViewData["EMPCODE_SELECT"] = DB.HRStaffProfiles.Where(w => w.Status == "A").ToList();
            ViewData["POST_SELECT"] = DB.HRPositions.ToList();
            ViewData["DEPT_SELECT"] = DB.HRDepartments.ToList();
        }
    }
}