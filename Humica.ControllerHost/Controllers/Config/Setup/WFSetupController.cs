using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setupon.Setup
{
    public class WFSetupController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRR000002";
        private const string URL_SCREEN = "/Config/Setup/WFSetup/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string KeyName = "ID";
        private string GRID_EDITOR_ERROR = "EditError";

        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        private string ActionName = "";

        public WFSetupController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        public ActionResult Index()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            BSM.ScreenID = SCREEN_ID;
            BSM.ListWFApprover = new List<ExCfWFApprover>();
            // BSM.ListWF = DB.ExCfWorkFlows.ToList();
            BSM.ListWF = SMS.CFWorkFlows.ToList();
            BSM.ListWFApprover = DB.ExCfWFApprovers.ToList();
            BSM.ListWFItem = DB.ExCfWorkFlowItems.ToList();
            BSM.ListSalaryApprover = DB.ExCfWFSalaryApprovers.ToList();
            DataList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        #endregion

        #region "List Work FLow"
        public ActionResult GridItems1()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ExWorkFlowPreference)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            BSM.ListWF = SMS.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWF);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartialWF(CFWorkFlow ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new SMSystemEntity();

                DBA.CFWorkFlows.Add(ModelObject);
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWF = SMS.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWF);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartialWF(CFWorkFlow ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new SMSystemEntity();
                DBA.CFWorkFlows.Attach(ModelObject);
                DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWF = SMS.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWF);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartialWF(string WFObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var obj = SMS.CFWorkFlows.Find(WFObject);
                if (obj != null)
                {
                    SMS.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    SMS.SaveChanges();
                }
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = WFObject;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWF = SMS.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWF);
        }



        #endregion
        #region "List WF Approver By Branch"
        public ActionResult GridItems2()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ExWorkFlowPreference)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            BSM.ListWFApprover = DB.ExCfWFApprovers.ToList();
            return PartialView("GridItems2", BSM.ListWFApprover);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartialApprover(ExCfWFApprover ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new HumicaDBContext();

                var objStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == ModelObject.Employee);
                if (objStaff != null)
                {
                    ModelObject.EmployeeName = objStaff.AllName;
                }
                if (string.IsNullOrEmpty(ModelObject.Employee))
                    ViewData["EditError"] = SYMessages.getMessage("IVALIED_EMP");
                if (string.IsNullOrEmpty(ModelObject.WFObject))
                    ViewData["EditError"] = SYMessages.getMessage("IVALIED_WFObject");
                var obj = DB.ExCfWFApprovers.Where(w => w.WFObject == ModelObject.WFObject && w.Employee == ModelObject.Employee
                    && w.Branch == ModelObject.Branch && w.Department == ModelObject.Department).ToList();
                if(obj.Any())
                    ViewData["EditError"] = SYMessages.getMessage("DUPLICATED_DATA");
                DBA.ExCfWFApprovers.Add(ModelObject);
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWFApprover = DB.ExCfWFApprovers.ToList();
            return PartialView("GridItems2", BSM.ListWFApprover);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartialApprover(ExCfWFApprover ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new HumicaDBContext();

                var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == ModelObject.Employee);
                if (objStaff != null)
                {
                    ModelObject.EmployeeName = objStaff.AllName;
                }

                DBA.ExCfWFApprovers.Attach(ModelObject);
                DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWFApprover = DB.ExCfWFApprovers.ToList();
            return PartialView("GridItems2", BSM.ListWFApprover);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartialApprover(int ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var obj = DB.ExCfWFApprovers.Where(w => w.ID == ID).ToList();
                if (obj.Count > 0)
                {
                    DB.Entry(obj.First()).State = System.Data.Entity.EntityState.Deleted;
                    DB.SaveChanges();
                }
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ID.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListWFApprover = DB.ExCfWFApprovers.ToList();
            return PartialView("GridItems2", BSM.ListWFApprover);
        }



        #endregion
        #region "List WF Item"
        public ActionResult GridItems4()
        {
            UserConf(ActionBehavior.EDIT);
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            BSM.ListWFItem = DB.ExCfWorkFlowItems.ToList();
            DataList();
            return PartialView("GridItems4", BSM.ListWFItem);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial4(ExCfWorkFlowItem ModelObject)
        {
            UserSession();
            UserConfListAndForm();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            if (ModelState.IsValid)
            {
                try
                {
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == ModelObject.NumberRank);
                    ModelObject.NumberRankItem = Num.Length.ToString();
                    ModelObject.ScreenID = ModelObject.ScreenID.Trim();
                    ModelObject.DocType = ModelObject.DocType.Trim().ToUpper();
                    ModelObject.IsRequiredApproval = true;
                    DB.ExCfWorkFlowItems.Add(ModelObject);
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListWFItem = DB.ExCfWorkFlowItems.ToList();
            return PartialView("GridItems4", BSM.ListWFItem);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial4(ExCfWorkFlowItem ModelObject)
        {
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            if (ModelState.IsValid)
            {
                try
                {
                    var Num = SMS.BSNumberRanks.FirstOrDefault(w => w.NumberObject == ModelObject.NumberRank);
                    ModelObject.NumberRankItem = Num.Length.ToString();
                    ModelObject.IsRequiredApproval = true;
                    DB.ExCfWorkFlowItems.Attach(ModelObject);
                    DB.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE002", user.Lang);
            }
            DataList();
            BSM.ListWFItem = DB.ExCfWorkFlowItems.ToList();
            return PartialView("GridItems4", BSM.ListWFItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial4(string ScreenID, string DocType)
        {
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var obj = DB.ExCfWorkFlowItems.Find(ScreenID, DocType);
                if (obj != null)
                {
                    DB.ExCfWorkFlowItems.Remove(obj);
                    int row = DB.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            DataList();
            BSM.ListWFItem = DB.ExCfWorkFlowItems.ToList();
            return PartialView("GridItems4", BSM.ListWFItem);
        }

        #endregion
        #region "List Salary Approver"
        public ActionResult GridItems5()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ExWorkFlowPreference)Session[Index_Sess_Obj + ActionName];
            }
            DataList();
            BSM.ListSalaryApprover = DB.ExCfWFSalaryApprovers.ToList();
            return PartialView("GridItems5", BSM.ListSalaryApprover);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartialSApprover(ExCfWFSalaryApprover ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new HumicaDBContext();

                var objStaff = DBV.HR_STAFF_VIEW.FirstOrDefault(w => w.EmpCode == ModelObject.Employee);
                if (objStaff != null)
                {
                    ModelObject.EmployeeName = objStaff.AllName;
                }

                DBA.ExCfWFSalaryApprovers.Add(ModelObject);
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.ScreenId = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListSalaryApprover = DB.ExCfWFSalaryApprovers.ToList();
            return PartialView("GridItems5", BSM.ListSalaryApprover);
        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartialSApprover(ExCfWFSalaryApprover ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var DBA = new HumicaDBContext();

                var objStaff = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == ModelObject.Employee);
                if (objStaff != null)
                {
                    ModelObject.EmployeeName = objStaff.AllName;
                }

                DBA.ExCfWFSalaryApprovers.Attach(ModelObject);
                DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                DBA.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (DbUpdateException e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }
            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ModelObject.Employee;
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListSalaryApprover = DB.ExCfWFSalaryApprovers.ToList();
            return PartialView("GridItems5", BSM.ListSalaryApprover);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartialSApprover(int ID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataList();
            ExWorkFlowPreference BSM = new ExWorkFlowPreference();
            try
            {
                var obj = DB.ExCfWFSalaryApprovers.Where(w => w.ID == ID).ToList();
                if (obj.Count > 0)
                {
                    DB.Entry(obj.First()).State = System.Data.Entity.EntityState.Deleted;
                    DB.SaveChanges();
                }
            }

            catch (Exception e)
            {
                /*------------------SaveLog----------------------------------*/
                SYEventLog log = new SYEventLog();
                log.ScreenId = SCREEN_ID;
                log.UserId = user.UserName;
                log.Action = ID.ToString();
                log.Action = SYActionBehavior.ADD.ToString();

                SYEventLogObject.saveEventLog(log, e, true);
                /*----------------------------------------------------------*/
                ViewData[GRID_EDITOR_ERROR] = SYMessages.getMessage("EE001");
            }

            BSM.ListSalaryApprover = DB.ExCfWFSalaryApprovers.ToList();
            return PartialView("GridItems5", BSM.ListSalaryApprover);
        }



        #endregion
        #region "Private Code"
        private void DataList()
        {
            string INACTIVE = StaffStatus.INACTIVE.ToString();
            ViewData["STAFF_LIST"] = DBV.HR_STAFF_VIEW.ToList();
            ViewData["WF_LIST"] = SMS.CFWorkFlows.ToList();
            ViewData["BRANCH_LIST"] = SMS.HRBranches.ToList();
            ViewData["DEPARTMENT_LIST"] = DB.HRDepartments.ToList();
            //   ViewData["STAFF_GROUP"] = DB.CFDepartments.ToList();
            ViewData["SIGN_OPERATOR"] = new SYDataList("SIGN_OPERATOR").ListData;
            ViewData["CONJUNCTION"] = new SYDataList("CONJUNCTION").ListData;
            ViewData["OBJECT_LIST"] = new TableName().getListRequisition();
            ViewData["NUMBER_LIST"] = SMS.BSNumberRanks.ToList();
            ViewData["TELEGRAM_SELECT"] = DB.TelegramBots.ToList();
            var DBA = new SMSystemEntity();
            //    ViewData["FIELD_LIST"] = DBA.SY_Schema.Where(w => w.TableName == "ExPOHeader").ToList();
        }
        #endregion
    }

    public enum StaffStatus
    {
        ACTIVE, INACTIVE
    }
}