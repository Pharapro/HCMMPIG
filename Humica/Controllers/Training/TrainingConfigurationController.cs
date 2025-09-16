using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Training.Setup
{
    public class TrainingConfigurationController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000006";
        private const string URL_SCREEN = "/Training/Setup/TrainingConfiguration/";
        const string DOC_TYPE = "SPC01";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string Index_Sess_Msg = SYSConstant.SAVE_COMPLETED + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ProgramCode";
        private string PARAM_INDEX = "ProgramCode;Branch;Status";
        private string TR_REF = "NTR01";
        private string PATH_FILE = "dkslkds";
        private string ClaimQTY = "dkdsfds";
        private string status = SYDocumentStatus.OPEN.ToString();

        HumicaDBContextTR DB = new HumicaDBContextTR();
        private SMSystemEntity SMS = new SMSystemEntity();
        Humica.Core.DB.HumicaDBContext DBX = new Humica.Core.DB.HumicaDBContext();
        public TrainingConfigurationController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        #region "List"
        // GET: /NMaterialModel/
        public ActionResult Index()
        {

            ActionName = "Index";
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            var ListReqData = DB.TRTrainingRequirements.ToList();
            BSM.ListTrainingRequirement = ListReqData.Where(w => w.Category == "R").ToList();
            BSM.ListTrainingGroup = ListReqData.Where(w => w.Category == "G").ToList();
            BSM.ListTrainingRoom = ListReqData.Where(w => w.Category == "V").ToList();
            BSM.ListTrainingDays = ListReqData.Where(w => w.Category == "D").ToList();
            BSM.ListTrainingCategory = ListReqData.Where(w => w.Category == "C").ToList();
            BSM.ListTrainingSubCategory = ListReqData.Where(w => w.Category == "S").ToList();
            BSM.ListExamType = ListReqData.Where(w => w.Category == "P").ToList();
            BSM.ListLevel = SMS.HRLevels.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult _GridItem7()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListExamType = DB.TRTrainingRequirements.Where(w => w.Category == "P").ToList();
            return PartialView("_GridItem7", BSM.ListExamType);
        }
        #endregion

        #region "_requirement"
        public ActionResult _GridItem1()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingRequirement = DB.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            return PartialView("_GridItem1", BSM.ListTrainingRequirement);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.Description == ModelObject.Description);

                    ModelObject.Category = "R";
                    ModelObject.Type = "R";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingRequirement = DB.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            return PartialView("_GridItem1", BSM.ListTrainingRequirement);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.Value = ModelObject.Value;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingRequirement = DB.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            return PartialView("_GridItem1", BSM.ListTrainingRequirement);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingRequirement = DB.TRTrainingRequirements.Where(w => w.Category == "R").ToList();
            return PartialView("_GridItem1", BSM.ListTrainingRequirement);
        }
        #endregion

        #region "_traing group"
        public ActionResult _GridItem2()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            return PartialView("_GridItem2", BSM.ListTrainingGroup);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateG(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.Category = "G";
                    ModelObject.Type = "G";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            return PartialView("_GridItem2", BSM.ListTrainingGroup);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditG(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            return PartialView("_GridItem2", BSM.ListTrainingGroup);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteG(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingGroup = DB.TRTrainingRequirements.Where(w => w.Category == "G").ToList();
            return PartialView("_GridItem2", BSM.ListTrainingGroup);
        }
        #endregion

        #region "_traing room"
        public ActionResult _GridItem3()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingRoom = DB.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            return PartialView("_GridItem3", BSM.ListTrainingRoom);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateV(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.Category = "V";
                    ModelObject.Type = "V";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingRoom = DB.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            return PartialView("_GridItem3", BSM.ListTrainingRoom);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditV(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingRoom = DB.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            return PartialView("_GridItem3", BSM.ListTrainingRoom);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteV(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingRoom = DB.TRTrainingRequirements.Where(w => w.Category == "V").ToList();
            return PartialView("_GridItem3", BSM.ListTrainingRoom);
        }
        #endregion

        #region "_traing days"
        public ActionResult _GridItem4()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingDays = DB.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            return PartialView("_GridItem4", BSM.ListTrainingDays);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateD(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.Category = "D";
                    ModelObject.Type = "D";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingDays = DB.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            return PartialView("_GridItem4", BSM.ListTrainingDays);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditD(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingDays = DB.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            return PartialView("_GridItem4", BSM.ListTrainingDays);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteD(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingDays = DB.TRTrainingRequirements.Where(w => w.Category == "D").ToList();
            return PartialView("_GridItem4", BSM.ListTrainingDays);
        }
        #endregion

        #region "_traing category"
        public ActionResult _GridItem5()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingCategory = DB.TRTrainingRequirements.Where(w => w.Category == "C").ToList();
            return PartialView("_GridItem5", BSM.ListTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateC(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.Category = "C";
                    ModelObject.Type = "C";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingCategory = DB.TRTrainingRequirements.Where(w => w.Category == "C").ToList();
            return PartialView("_GridItem5", BSM.ListTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditC(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingCategory = DB.TRTrainingRequirements.Where(w => w.Category == "C").ToList();
            return PartialView("_GridItem5", BSM.ListTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteC(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingCategory = DB.TRTrainingRequirements.Where(w => w.Category == "C").ToList();
            return PartialView("_GridItem5", BSM.ListTrainingCategory);
        }
        #endregion

        #region "_traing subcategory"
        public ActionResult _GridItem6()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            //DataSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingSubCategory = DB.TRTrainingRequirements.Where(w => w.Category == "S").ToList();
            return PartialView("_GridItem6", BSM.ListTrainingSubCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateS(TRTrainingRequirement ModelObject)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    ModelObject.Category = "S";
                    ModelObject.Type = "S";
                    ModelObject.Value = 1;
                    DB.TRTrainingRequirements.Add(ModelObject);
                    DB.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingSubCategory = DB.TRTrainingRequirements.Where(w => w.Category == "S").ToList();
            return PartialView("_GridItem6", BSM.ListTrainingSubCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditS(TRTrainingRequirement ModelObject)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        DB.TRTrainingRequirements.Attach(check);
                        DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                        int row = DB.SaveChanges();

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

            BSM.ListTrainingSubCategory = DB.TRTrainingRequirements.Where(w => w.Category == "S").ToList();
            return PartialView("_GridItem6", BSM.ListTrainingSubCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteS(int ID)
        {
            UserSession();
            //DataSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TRTrainingRequirements.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TRTrainingRequirements.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingSubCategory = DB.TRTrainingRequirements.Where(w => w.Category == "S").ToList();
            return PartialView("_GridItem6", BSM.ListTrainingSubCategory);
        }
        #endregion


        private void DataListSelector()
        {
            SYDataList DL = new SYDataList("CLAIM_TEYP_SELECT");
            ViewData["CLAIM_TEYP_SELECT"] = DL.ListData;
            //ViewData["STAFF_SELECT"] = DB.Staffs.Where(w => w.CompanyCode == bs.CompanyCode).ToList();
            // ViewData["TRUCK_TYPE"] = DB.CFDPTruckAllows.ToList();
        }
        private void DataList()
        {
            UserSession();
            UserConfListAndForm();
        }

        private void DataSelectorForFilter()
        {
            var DL = new SYDataList("TRCODE_SELECT");
            ViewData["TRCODE_SELECT"] = DL.ListData;
            DL = new SYDataList("STATUS_APPROVAL");
            ViewData["STATUS_APPROVAL"] = DL.ListData;
            var listReason = DBX.CFReasonCodes.Where(w => w.Indicator == "H").ToList();
            foreach (var read in listReason)
            {
                read.Description1 = read.Description2 + "-" + read.Description1;
            }
            ViewData["REASON_CANCEL"] = listReason;
        }

        //public ActionResult ExportTo(string id)
        //{
        //    UserSession();
        //    if (id == "SHIP")
        //    {
        //        SYSGridSettingExport gSetting = new SYSGridSettingExport();
        //        gSetting.GridName = "SimulatedList";
        //        gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
        //        gSetting.ListConfigure = SYSettings.getListConf(SCREEN_ID, user.Lang);
        //        GridViewExportFormat format = GridViewExportFormat.Xlsx;
        //        if (SYExportFile.ExportFormatsInfo.ContainsKey(format))
        //        {
        //            ActionName = "GenerateShip";
        //            LGShipmentObject BSM = new LGShipmentObject();

        //            if (Session[Index_Sess_Obj + ActionName] != null)
        //            {
        //                BSM = (LGShipmentObject)Session[Index_Sess_Obj + ActionName];


        //                if (BSM.ListItemToBeShip != null)
        //                {
        //                    var obj = BSM.ListItemToBeShip.First();

        //                    var dix = ClsInformation.GetDictionaryInfo(obj);
        //                    return SYExportFile.ExportFormatsInfo[format](SYExportFile.CreateExportGridViewSettings(gSetting, dix),
        //            BSM.ListItemToBeShip);
        //                }

        //            }

        //        }
        //    }
        //    else
        //    {
        //        SYSGridSettingExport gSetting = new SYSGridSettingExport();
        //        gSetting.GridName = "ShipmentList";

        //        gSetting.DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
        //        gSetting.ListConfigure = SYSettings.getListConf(SCREEN_ID, user.Lang);
        //        GridViewExportFormat format = GridViewExportFormat.Xlsx;
        //        if (SYExportFile.ExportFormatsInfo.ContainsKey(format))
        //        {
        //            this.ActionName = "Index";
        //            LGShipmentObject BSM = new LGShipmentObject();

        //            if (Session[Index_Sess_Obj + ActionName] != null)
        //            {
        //                BSM = (LGShipmentObject)Session[Index_Sess_Obj + ActionName];
        //            }
        //            return SYExportFile.ExportFormatsInfo[format](SYExportFile.CreateExportGridViewSettings(gSetting),
        //               BSM.ListHeader);
        //        }
        //    }

        //    return RedirectToAction("Index");
        //}


    }
}

