using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.HRS;
using Humica.Models.Mission;
using Humica.Logic.Mission;
using Humica.Models.HR;
using Humica.Models.Mission;
using System;
using System.Linq;
using System.Web.Mvc;
using Humica.EF.Repo;
using System.Collections.Generic;
using System.Data.Entity;

namespace Humica.Controllers.HRM.Mission
{
    public class MISSTypeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "MISS000001";
        private const string URL_SCREEN = "/HRM/Mission/MISSType/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "Code";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        ClsMISSType BSM;
        IUnitOfWork unitOfWork;
        public MISSTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsMISSType();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            var userName = user.UserName;
            BSM.ListMType = unitOfWork.Set<HRMType>().ToList();
            BSM.ListItem = unitOfWork.Set<HRMItem>().ToList();
            BSM.ListMissOilRating = unitOfWork.Set<HRMissOilRating>().ToList();
            BSM.ListTravelBy = unitOfWork.Set<HRMTravelby>().ToList();
            BSM.ListMClaimType = unitOfWork.Set<HRMClaimType>().ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #region MissType

        public ActionResult GridMMissType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();

            BSM.ListMType = unitOfWork.Set<HRMType>().ToList();
            return PartialView("GridMMissType", BSM.ListMType);
        }
        public ActionResult CreateMissType(HRMType MModel)
        {
            ActionName = "Create";
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    string Code = MModel.Code.ToUpper().Trim();
                    var listDivi = unitOfWork.Set<HRMType>().Find(Code);
                    if (listDivi == null)
                    {
                        MModel.Code = Code;
                        unitOfWork.Set<HRMType>().Add(MModel);
                        unitOfWork.Save();
                    }
                    else
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
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
            BSM.ListMType = unitOfWork.Set<HRMType>().ToList();
            return PartialView("GridMMissType", BSM.ListMType);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMissType(HRMType MModel)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            if (ModelState.IsValid)
            {
                try
                {

                    unitOfWork.Update(MModel);
                    unitOfWork.Save();
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
            BSM.ListMType = unitOfWork.Set<HRMType>().OrderBy(w => w.Code).ToList();
            return PartialView("GridMMissType", BSM.ListMType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMissType(string Code)
        {
            ActionName = "Create";
            UserSession();
            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Set<HRMType>().Find(Code);
                    if (obj != null)
                    {
                        unitOfWork.Set<HRMType>().Remove(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListMType = unitOfWork.Set<HRMType>().OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridMMissType", BSM.ListMType);
        }

        #endregion

        #region MissItem
        public ActionResult GridMissItem()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            
            BSM.ListItem = unitOfWork.Set<HRMItem>().ToList();
            return PartialView("GridMissItem", BSM.ListItem);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(HRMItem MModel)
        {
            UserSession();
            //  DataSelector();
            
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper().Trim();
                    unitOfWork.Set<HRMItem>().Add(MModel);
                    unitOfWork.Save();
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
            BSM.ListItem = unitOfWork.Set<HRMItem>().ToList();
            return PartialView("GridMissItem", BSM.ListItem);

        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(HRMItem MModel)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Update(MModel);
                    unitOfWork.Save();
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
            BSM.ListItem = unitOfWork.Set<HRMItem>().ToList();
            return PartialView("GridMissItem", BSM.ListItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(string Code)
        {

            UserSession();
            
            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Set<HRMItem>().Find(Code);
                    if (obj != null)
                    {
                        unitOfWork.Set<HRMItem>().Remove(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListItem = unitOfWork.Set<HRMItem>().OrderBy(w => w.Code).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridMissItem", BSM.ListItem);
        }
        #endregion

        #region HRMOilRating
        public ActionResult GridHRMOilRating()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            
            BSM.ListMissOilRating = unitOfWork.Set<HRMissOilRating>().ToList();
            return PartialView("GridHRMOilRating", BSM.ListMissOilRating);
        }

        [HttpPost, ValidateInput(false)]
    
        public ActionResult CreateHRMOilRating(HRMissOilRating MModel)
        {
            UserSession();
              DataSelector();
            
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.ID = MModel.ID;
                    unitOfWork.Set<HRMissOilRating>().Add(MModel);
                    unitOfWork.Save();
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
            BSM.ListMissOilRating = unitOfWork.Set<HRMissOilRating>().ToList();
            return PartialView("GridHRMOilRating", BSM.ListMissOilRating);

        }

        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditHRMOilRating(HRMissOilRating MModel)
        {
            UserSession();

            
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Update(MModel);
                    unitOfWork.Save();
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
            BSM.ListMissOilRating = unitOfWork.Set<HRMissOilRating>().OrderBy(w => w.ID).ToList();
            DataSelector();
            return PartialView("GridHRMOilRating", BSM.ListMissOilRating);
        }
       // delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteHRMOilRating(int ID)
        {
     
            UserSession();
            
            if (ID != null)
            {
                try
                {
                    var obj = unitOfWork.Set<HRMissOilRating>().Find(ID);
                    if (obj != null)
                    {
                        unitOfWork.Set<HRMissOilRating>().Remove(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListMissOilRating = unitOfWork.Set<HRMissOilRating>().OrderBy(w => w.ID).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridHRMOilRating", BSM.ListMissOilRating);
        }

        #endregion
        #region HRMClaimType

        public ActionResult GridHRMClaimType()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            
            BSM.ListMClaimType = unitOfWork.Set<HRMClaimType>().ToList();
            return PartialView("GridHRMClaimType", BSM.ListMClaimType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateClaimType(HRMClaimType MModel)
        {
            UserSession();
            //  DataSelector();
            
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper().Trim();
                    unitOfWork.Set<HRMClaimType>().Add(MModel);
                    unitOfWork.Save();
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
            BSM.ListMClaimType = unitOfWork.Set<HRMClaimType>().ToList();
            return PartialView("GridHRMClaimType", BSM.ListMClaimType);

        }

        //edit 
        [HttpPost, ValidateInput(false)]
        public ActionResult EditClaimType(HRMClaimType MModel)
        {
            UserSession();

            
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Update(MModel);
                    unitOfWork.Save();
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
            BSM.ListMClaimType = unitOfWork.Set<HRMClaimType>().OrderBy(w => w.Code).ToList();
            // DataSelector();
            return PartialView("GridHRMClaimType", BSM.ListMClaimType);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteClaimType(String Code)

        {

            UserSession();
            
            try
            {
               
                var obj = unitOfWork.Set<HRMClaimType>().Where(w => w.Code == Code).FirstOrDefault();
                if (obj != null)
                {
                    unitOfWork.Set<HRMClaimType>().Remove(obj);
                    unitOfWork.Save();
                }
                BSM.ListMClaimType = unitOfWork.Set<HRMClaimType>().OrderBy(w => w.Code).ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridHRMClaimType", BSM.ListMClaimType);
        }

        #endregion
        #region HRMTravelBy
        public ActionResult GridHRMTravelBy()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
            
            BSM.ListTravelBy = unitOfWork.Set<HRMTravelby>().ToList();
            return PartialView("GridHRMTravelBy", BSM.ListTravelBy);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTravel(HRMTravelby MModel)
        {
            UserSession();
            DataSelector();
            
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Code = MModel.Code.ToUpper().Trim();
                    unitOfWork.Set<HRMTravelby>().Add(MModel);
                    unitOfWork.Save();
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
            BSM.ListTravelBy = unitOfWork.Set<HRMTravelby>().ToList();
            return PartialView("GridHRMTravelBy", BSM.ListTravelBy);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTravel(HRMTravelby MModel)
        {
            UserSession();

            
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Update(MModel);
                    unitOfWork.Save();
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
            BSM.ListTravelBy = unitOfWork.Set<HRMTravelby>().OrderBy(w => w.Code).ToList();
            DataSelector();
            return PartialView("GridHRMTravelBy", BSM.ListTravelBy);
        }

        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTravel(string Code)
        {
            UserSession();
            
            try
            {
              
                var obj = unitOfWork.Set<HRMTravelby>().Where(w => w.Code == Code).FirstOrDefault();
                if (obj != null)
                {
                    unitOfWork.Set<HRMTravelby>().Remove(obj);
                    unitOfWork.Save();
                }
                BSM.ListTravelBy = unitOfWork.Set<HRMTravelby>().OrderBy(w => w.Code).ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }
            return PartialView("GridHRMTravelBy", BSM.ListTravelBy);
        }

        #endregion
        private void DataSelector()
        {
            SYDataList objList = new SYDataList("CAR_TYPE");
            ViewData["CAR_TYPE"] = objList.ListData;
        }
    }
}