using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.Logic.Atts;
using Humica.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Attendance.AttSetup
{
    public class ATMealInOutSetUpController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "ATS0000006";
        private const string URL_SCREEN = "/Attendance/AttSetup/ATMealInOutSetUp/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "TranNo";
        private string PARAM_INDEX = "TranNo;";
        HumicaDBContext DB = new HumicaDBContext();
        SMSystemEntity SMS = new SMSystemEntity();
        public ATMealInOutSetUpController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region 'Index'
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelect();
            DataList();
            ATMealInOutSetupObject BSM = new ATMealInOutSetupObject();
            BSM.Header = new ATMealSetup();
            BSM.ListItem = DB.ATMealSetupItems.OrderBy(w => w.LevelCode).ToList();

            var obj = DB.ATMealSetups.FirstOrDefault();
            if (obj != null) BSM.Header = obj;

            Session[Index_Sess_Obj + ActionName] = BSM;
            ViewData[ClsConstant.PARAM_INDEX] = PARAM_INDEX;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Index(ATMealInOutSetupObject collection)
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataSelect();
            DataList();
            var BSM = (ATMealInOutSetupObject)Session[Index_Sess_Obj + ActionName];

            collection.ScreenId = SCREEN_ID;
            BSM.Header = collection.Header;
            string msg = BSM.updateMealSetup();

            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return View(BSM);
            }
            else
            {
                SYMessages mess_err = SYMessages.getMessageObject(msg, user.Lang);
                Session[Index_Sess_Obj + this.ActionName] = BSM;
                UserConfForm(ActionBehavior.SAVEGRID);
                Session[SYConstant.MESSAGE_SUBMIT] = mess_err;
                return View(BSM);
            }
            Session[Index_Sess_Obj + this.ActionName] = BSM;
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion
        #region 'Grid'
        public ActionResult GridItems()
        {
            DataList();
            UserConf(ActionBehavior.EDIT);

            ATMealInOutSetupObject BSM = new ATMealInOutSetupObject();
            BSM.ListItem = DB.ATMealSetupItems.OrderBy(w => w.LevelCode).ToList();
            return PartialView("Gridview", BSM.ListItem);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(ATMealSetupItem MModel)
        {
            DataList();
            UserSession();
            ATMealInOutSetupObject BSM = new ATMealInOutSetupObject();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.CreatedBy = user.UserName;
                    MModel.CreatedOn = DateTime.Now;
                    DB.ATMealSetupItems.Add(MModel);
                    int row = DB.SaveChanges();
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
            BSM.ListItem = DB.ATMealSetupItems.OrderBy(w => w.LevelCode).ToList();
            return PartialView("Gridview", BSM.ListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(ATMealSetupItem MModel)
        {
            DataList();
            UserSession();
            ATMealInOutSetupObject BSM = new ATMealInOutSetupObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.ATMealSetupItems.Find(MModel.LevelCode);
                    MModel.CreatedBy = objUpdate.CreatedBy;
                    MModel.CreatedOn = objUpdate.CreatedOn;
                    MModel.ChangedBy = user.UserName;
                    MModel.ChangedOn = DateTime.Now;
                    DB.ATMealSetupItems.Attach(MModel);
                    DB.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
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
            BSM.ListItem = DB.ATMealSetupItems.OrderBy(w => w.LevelCode).ToList();
            return PartialView("Gridview", BSM.ListItem);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string LevelCode)
        {
            UserSession();
            ATMealInOutSetupObject BSM = new ATMealInOutSetupObject();
            if (LevelCode != "")
            {
                try
                {
                    var obj = DB.ATMealSetupItems.Find(LevelCode);
                    if (obj != null)
                    {
                        DB.ATMealSetupItems.Remove(obj);
                        int row = DB.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListItem = DB.ATMealSetupItems.OrderBy(w => w.LevelCode).ToList();
            return PartialView("Gridview", BSM.ListItem);
        }
        #endregion
        #region 'Code'
        private void DataList()
        {
            //SYDataList objList = new SYDataList("MEAL_TYPE");
            //ViewData["MEALTYPE_SELECT"] = objList.ListData;
            //objList = new SYDataList("DEVICE_TYPE");
            //ViewData["TYPE_SELECT"] = objList.ListData;
            ViewData["Level_SELECT"] = SMS.HRLevels.ToList();
        }
        private void DataSelect()
        {
            var ListReward = DB.PR_RewardsType.Where(w => w.ReCode == "ALLW").ToList();
            ViewData["Reward_SELECT"] = ListReward;
        }
        #endregion
    }
}
