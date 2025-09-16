using DevExpress.Utils.Extensions;
using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic.HRS;
using Humica.Logic.RCM;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.HRS
{
    public class RCMSettingController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "INF0000006";
        private const string URL_SCREEN = "/HRM/RCM/RCMSetting";
        IClsRCMSetting BSM;
        IUnitOfWork unitOfWork;
        public RCMSettingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
            BSM = new ClsRCMSetting();
            unitOfWork = new UnitOfWork<HumicaDBViewContext>();
        }
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            BSM.ListAdvType = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
            BSM.ListAds = unitOfWork.Repository<RCMSAdvertise>().Queryable().ToList();
            BSM.ListLanguage = unitOfWork.Repository<RCMSLang>().Queryable().ToList();
            BSM.ListJD = unitOfWork.Repository<RCMSJobDesc>().Queryable().ToList();
            BSM.ListQuestionnaire = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().ToList();
            BSM.ListSRefQues = unitOfWork.Repository<RCMSRefQuestion>().Queryable().ToList();
            return View(BSM);
        }
        #region 'Advertiser Type'
        public ActionResult GridItemAdvType()
        {
            UserConf(ActionBehavior.EDIT);


            BSM.ListAdvType = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
            return PartialView("GridItemAdvType", BSM.ListAdvType);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateAdvType(RCMAdvType MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.Code == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EECODE_EN", user.Lang);
                    }
                    else
                    {
                        var chkdup = unitOfWork.Repository<RCMAdvType>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
                        if (chkdup != null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                        }
                        else
                        {
                            MModel.Code = MModel.Code.ToUpper();
                            unitOfWork.Add(MModel);
                            unitOfWork.Save();
                        }
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
            BSM.ListAdvType = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
            return PartialView("GridItemAdvType", BSM.ListAdvType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAdvType(RCMAdvType MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMAdvType>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Code = MModel.Code;
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.SecondDescription = MModel.SecondDescription;
                    ObjMatch.Remark = MModel.Remark;

                    unitOfWork.Update(ObjMatch);
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
            BSM.ListAdvType = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
            return PartialView("GridItemAdvType", BSM.ListAdvType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAdvType(string Code)
        {
            UserSession();
            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Repository<RCMAdvType>().Queryable().FirstOrDefault(w => w.Code == Code);
                    if (obj != null)
                    {
                        unitOfWork.Delete(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListAdvType = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemAdvType", BSM.ListAdvType);
        }
        #endregion 
        #region 'Advertiser'
        public ActionResult GridItemAds()
        {
            DataSelector();
            UserConf(ActionBehavior.EDIT);


            BSM.ListAds = unitOfWork.Repository<RCMSAdvertise>().Queryable().ToList();
            return PartialView("GridItemAds", BSM.ListAds);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateAds(RCMSAdvertise MModel)
        {
            DataSelector();
            UserSession();

            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.Code == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EECODE_EN", user.Lang);
                    }
                    else if (MModel.Company == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EECOM_EN", user.Lang);
                    }
                    else if (MModel.AdsType == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EEADS_EN", user.Lang);
                    }
                    MModel.Code = MModel.Code.ToUpper();
                    unitOfWork.Add(MModel);
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
            BSM.ListAds = unitOfWork.Repository<RCMSAdvertise>().Queryable().ToList();
            return PartialView("GridItemAds", BSM.ListAds);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAds(RCMSAdvertise MModel)
        {
            DataSelector();
            UserSession();

            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMSAdvertise>().Queryable().FirstOrDefault(w => w.Code == MModel.Code && w.Company == MModel.Company);
                    ObjMatch.Address = MModel.Address;
                    ObjMatch.Contact = MModel.Contact;
                    ObjMatch.Email = MModel.Email;
                    ObjMatch.Phone1 = MModel.Phone1;
                    ObjMatch.Phone2 = MModel.Phone2;
                    ObjMatch.TokenCode = MModel.TokenCode;
                    ObjMatch.UserName = MModel.UserName;
                    ObjMatch.Password = MModel.Password;
                    ObjMatch.AppID = MModel.AppID;
                    ObjMatch.AppName = MModel.AppName;
                    ObjMatch.AdsType = MModel.AdsType;
                    unitOfWork.Update(ObjMatch);
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
            BSM.ListAds = unitOfWork.Repository<RCMSAdvertise>().Queryable().OrderBy(w => w.Code).ToList();
            return PartialView("GridItemAds", BSM.ListAds);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteAds(string Code, string Company)
        {
            UserSession();
            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Repository<RCMSAdvertise>().Queryable().FirstOrDefault(w => w.Code == Code && w.Company == Company);
                    if (obj != null)
                    {
                        unitOfWork.Delete(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListAds = unitOfWork.Repository<RCMSAdvertise>().Queryable().OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemAds", BSM.ListAds);
        }
        #endregion 'Advertiser'
        #region 'Job Responsibility'
        public ActionResult GridItemJD()
        {

            DataSelector();
            UserConf(ActionBehavior.EDIT);


            BSM.ListJD = unitOfWork.Repository<RCMSJobDesc>().Queryable().ToList();
            return PartialView("GridItemJD", BSM.ListJD);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateJD(RCMSJobDesc MModel)
        {
            DataSelector();
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.Add(MModel);
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
            BSM.ListJD = unitOfWork.Repository<RCMSJobDesc>().Queryable().ToList();
            return PartialView("GridItemJD", BSM.ListJD);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditJD(RCMSJobDesc MModel)
        {
            DataSelector();
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMSJobDesc>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.JobResponsibility = MModel.JobResponsibility;
                    ObjMatch.JobRequirement = MModel.JobRequirement;
                    unitOfWork.Update(ObjMatch);
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
            BSM.ListJD = unitOfWork.Repository<RCMSJobDesc>().Queryable().OrderBy(w => w.Code).ToList();
            return PartialView("GridItemJD", BSM.ListJD);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteJD(string Code)
        {
            DataSelector();
            UserSession();
            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Repository<RCMSJobDesc>().Queryable().FirstOrDefault(w => w.Code == Code);
                    if (obj != null)
                    {
                        unitOfWork.Delete(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListJD = unitOfWork.Repository<RCMSJobDesc>().Queryable().OrderBy(w => w.Code).ToList();

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridItemJD", BSM.ListJD);
        }
        #endregion 'Advertiser'
        #region 'Languages'
        public ActionResult GridItemLanguage()
        {
            UserConf(ActionBehavior.EDIT);


            BSM.ListLanguage = unitOfWork.Repository<RCMSLang>().Queryable().ToList();
            return PartialView("GridItemLanguage", BSM.ListLanguage);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLang(RCMSLang MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.Code == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("EECODE_EN", user.Lang);
                    }
                    else
                    {
                        var chkdup = unitOfWork.Repository<RCMSLang>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
                        if (chkdup != null)
                        {
                            ViewData["EditError"] = SYMessages.getMessage("DUPL_KEY", user.Lang);
                        }
                        else
                        {
                            MModel.Code = MModel.Code.ToUpper();
                            unitOfWork.Add(MModel);
                            unitOfWork.Save();
                        }
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
            BSM.ListLanguage = unitOfWork.Repository<RCMSLang>().Queryable().OrderBy(w => w.ID).ToList();
            return PartialView("GridItemLanguage", BSM.ListLanguage);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditLang(RCMSLang MModel)
        {
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMSLang>().Queryable().FirstOrDefault(w => w.Code == MModel.Code);
                    ObjMatch.Code = MModel.Code;
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.SecondDescription = MModel.SecondDescription;
                    unitOfWork.Update(ObjMatch);
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
            BSM.ListLanguage = unitOfWork.Repository<RCMSLang>().Queryable().OrderBy(w => w.ID).ToList();
            return PartialView("GridItemLanguage", BSM.ListLanguage);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteLang(string Code)
        {
            UserSession();

            if (Code != null)
            {
                try
                {
                    var obj = unitOfWork.Repository<RCMSLang>().Queryable().FirstOrDefault(w => w.Code == Code);
                    if (obj != null)
                    {
                        unitOfWork.Delete(obj);
                        unitOfWork.Save();
                    }
                    BSM.ListLanguage = unitOfWork.Repository<RCMSLang>().Queryable().OrderBy(w => w.ID).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridItemLanguage", BSM.ListLanguage);
        }
        #endregion 'Languages'
        #region 'Questionnaire'
        public ActionResult GridItemQuest()
        {
            DataSelector();
            UserConf(ActionBehavior.EDIT);
            BSM.ListQuestionnaire = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().OrderBy(w => w.Step).OrderByDescending(w => w.TranNo).ToList();
            return PartialView("GridItemQuest", BSM.ListQuestionnaire);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateQuest(RCMSQuestionnaire MModel)
        {
            DataSelector();
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.Position == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("POSITION_EN", user.Lang);
                    }
                    else
                    {
                        unitOfWork.Add(MModel);
                        unitOfWork.Save();
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
            BSM.ListQuestionnaire = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().OrderBy(w => w.Step).OrderByDescending(w => w.TranNo).ToList();
            return PartialView("GridItemQuest", BSM.ListQuestionnaire);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EdiQuest(RCMSQuestionnaire MModel)
        {
            DataSelector();
            UserSession();

            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().FirstOrDefault(w => w.TranNo == MModel.TranNo);
                    ObjMatch.Step = MModel.Step;
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.Position = MModel.Position;
                    unitOfWork.Update(ObjMatch);
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
            BSM.ListQuestionnaire = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().OrderBy(w => w.Step).OrderByDescending(w => w.TranNo).ToList();
            return PartialView("GridItemQuest", BSM.ListQuestionnaire);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteQuest(int TranNo)
        {
            DataSelector();
            try
            {
                var obj = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().FirstOrDefault(w => w.TranNo == TranNo);
                if (obj != null)
                {
                    unitOfWork.Delete(obj);
                    unitOfWork.Save();
                }
                BSM.ListQuestionnaire = unitOfWork.Repository<RCMSQuestionnaire>().Queryable().OrderBy(w => w.Step).OrderByDescending(w => w.TranNo).ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("GridItemQuest", BSM.ListQuestionnaire);
        }
        #endregion 'Languages'
        #region 'Ref Questionnaire'
        public ActionResult GridItemRefQuest()
        {
            DataSelector();
            UserConf(ActionBehavior.EDIT);
            BSM.ListSRefQues = unitOfWork.Repository<RCMSRefQuestion>().Queryable().OrderBy(w => w.step).OrderByDescending(w => w.ID).ToList();
            return PartialView("GridItemRefQuest", BSM.ListSRefQues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateRefQuest(RCMSRefQuestion MModel)
        {
            DataSelector();
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    if (MModel.Description == null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("DOC_NE", user.Lang);
                    }
                    else
                    {
                        unitOfWork.Add(MModel);
                        unitOfWork.Save();
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
            BSM.ListSRefQues = unitOfWork.Repository<RCMSRefQuestion>().Queryable().OrderBy(w => w.step).OrderByDescending(w => w.ID).ToList();
            return PartialView("GridItemRefQuest", BSM.ListSRefQues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EdiRefQuest(RCMSRefQuestion MModel)
        {
            DataSelector();
            UserSession();
            if (ModelState.IsValid)
            {
                try
                {
                    var ObjMatch = unitOfWork.Repository<RCMSRefQuestion>().Queryable().FirstOrDefault(w => w.ID == MModel.ID);
                    ObjMatch.step = MModel.step;
                    ObjMatch.Description = MModel.Description;
                    ObjMatch.SecDescription = MModel.SecDescription;

                    unitOfWork.Update(ObjMatch);
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
            BSM.ListSRefQues = unitOfWork.Repository<RCMSRefQuestion>().Queryable().OrderBy(w => w.step).OrderByDescending(w => w.ID).ToList();
            return PartialView("GridItemRefQuest", BSM.ListSRefQues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteRefQuest(int ID)
        {
            DataSelector();
            try
            {
                var obj = unitOfWork.Repository<RCMSRefQuestion>().Queryable().FirstOrDefault(w => w.ID == ID);
                if (obj != null)
                {
                    unitOfWork.Delete(obj);
                    unitOfWork.Save();
                }
                BSM.ListSRefQues = unitOfWork.Repository<RCMSRefQuestion>().Queryable().OrderBy(w => w.step).OrderByDescending(w => w.ID).ToList();
            }
            catch (Exception e)
            {
                ViewData["EditError"] = e.Message;
            }

            return PartialView("GridItemRefQuest", BSM.ListSRefQues);
        }
        #endregion 'Languages'
        #region 'Private Code'
        private void DataSelector()
        {
            ViewData["ADS_SELECT"] = unitOfWork.Repository<RCMAdvType>().Queryable().ToList();
            ViewData["POST_SELECT"] = unitOfWork.Repository<HRPosition>().Queryable().OrderBy(w => w.Code).ToList();
        }
        #endregion
    }
}
