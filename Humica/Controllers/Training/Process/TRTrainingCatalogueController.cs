using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Training;
using Humica.Training.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.TrainingN.Process
{
    public class TRTrainingCatalogueController : EF.Controllers.MasterSaleController
    {
        const string SCREENID = "TR00000001";
        private const string URL_SCREEN = "/Training/Process/TRTrainingCatalogue/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREENID;
        private string ActionName;
        private string KeyName = "TrainNo";
        private string PATH_FILE = "12313123123sadfsdfsdfsdf";
        HumicaDBContextTR DBX = new HumicaDBContextTR();

        public TRTrainingCatalogueController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region Index
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            var tblCourseCatalogue = DBX.TRTrainingCatalogueCourses.ToList();
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListTRTrainingCatalogue = new List<TRTrainingCatalogue>();
            BSM.ListModelTRTrainingCatalogue = new List<ModelTRTrainingCatalogue>();
            BSM.ListTRTrainingCatalogue = DBX.TRTrainingCatalogues.ToList();
            BSM.ListCourseType = new List<TRTrainingCourse>();
            BSM.ListCourseType = DBX.TRTrainingCourses.ToList();
            BSM.ListTRTrainingCatalogue.ForEach(x => BSM.ListModelTRTrainingCatalogue.
            Add(new ModelTRTrainingCatalogue
            {
                TrainNo = x.TrainNo,
                InYear = x.InYear ?? DateTime.Now.Year,
                Description = x.Description,
                TotalCourse = tblCourseCatalogue.Where(w => w.TrainingCatalogueID == x.TrainNo).Count()
            }
            ));
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListTRTrainingCatalogue = new List<TRTrainingCatalogue>();
            BSM.ListModelTRTrainingCatalogue = new List<ModelTRTrainingCatalogue>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
            }
            return PartialView("PartialList", BSM.ListModelTRTrainingCatalogue);
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            DataSelector();
            ViewData[SYSConstant.PARAM_ID1] = true;

            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.TRTrainingCatalogue = new TRTrainingCatalogue();
            BSM.ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
            BSM.ListCourseType = new List<TRTrainingCourse>();
            BSM.ListCourseType = DBX.TRTrainingCourses.ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Create(TrainingProcessObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREENID;
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                        BSM.TRTrainingCatalogue = collection.TRTrainingCatalogue;
                    }
                    string msg = BSM.Create();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);

                        mess.DocumentNumber = BSM.TRTrainingCatalogue.TrainNo.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        BSM.TRTrainingCatalogue = new TRTrainingCatalogue();
                        BSM.ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
                        ViewData[SYSConstant.PARAM_ID1] = true;
                        return View(BSM);
                    }
                    else
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        if (!string.IsNullOrEmpty(mess.Description))
                        {
                            mess.Description = string.Format(mess.Description, collection.TRTrainingCatalogue.InYear);
                        }
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return View(BSM);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    return View(BSM);
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                return View(BSM);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            return View(BSM);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? id)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (id != null)
            {
                BSM.TRTrainingCatalogue = new TRTrainingCatalogue();
                BSM.ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
                BSM.ListCourseType = new List<TRTrainingCourse>();
                BSM.TRTrainingCatalogue = DBX.TRTrainingCatalogues.Find(id);
                BSM.ListCourseType = DBX.TRTrainingCourses.ToList();
                if (BSM.TRTrainingCatalogue != null)
                {
                    BSM.ListTRTrainingCatalogueCourse = DBX.TRTrainingCatalogueCourses.Where(w => w.TrainingCatalogueID == id).ToList();
                    BSM.ListTRTrainingCatalogueCourse.ForEach(w => BSM.CatalogueCourseID.Add(w.TrainingCourseID.ToString()));
                    BSM.GetCourseSetup();
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        [HttpPost]
        public ActionResult Edit(int id, TrainingProcessObject collection)
        {
            try
            {
                ActionName = "Create";
                UserSession();
                this.ScreendIDControl = SCREENID;
                DataSelector();
                UserConfForm(SYActionBehavior.EDIT);
                ViewData[SYSConstant.PARAM_ID1] = true;
                TrainingProcessObject BSM = new TrainingProcessObject();
                if (id != null)
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                        BSM.TRTrainingCatalogue = collection.TRTrainingCatalogue;
                    }
                    BSM.ScreenId = SCREENID;
                    string msg = BSM.Edit(id);
                    if (msg == SYConstant.OK)
                    {

                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = id.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + mess.DocumentNumber;
                        ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                        return View(BSM);
                    }
                    else
                    {
                        ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                        return View(BSM);
                    }
                }
                ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
                return View(BSM);
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Details
        public ActionResult Details(int id)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = false;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.TRTrainingCatalogue = new TRTrainingCatalogue();
            BSM.ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
            BSM.TRTrainingCatalogue = DBX.TRTrainingCatalogues.FirstOrDefault(w => w.TrainNo == id);
            BSM.ListModelTRTrainingCatalogue = new List<ModelTRTrainingCatalogue>();
            if (BSM.TRTrainingCatalogue != null)
            {
                BSM.ListTRTrainingCatalogueCourse = DBX.TRTrainingCatalogueCourses.Where(w => w.TrainingCatalogueID == id).ToList();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        public ActionResult Delete(int? TrainNo)
        {
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            DataSelector();

            if (TrainNo != null)
            {
                int TranNo = Convert.ToInt32(TrainNo);
                TrainingProcessObject Del = new TrainingProcessObject();
                string msg = Del.Delete(TranNo);
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

        #region grid traininig course item
        public ActionResult CourseItemGrid()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
            }

            return PartialView("GridItems", BSM);
        }
        public ActionResult DeleteCourseItem(int? TrainingCourseID)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            TrainingProcessObject BSM = new TrainingProcessObject();

            if (TrainingCourseID != 0)
            {
                if (Session[Index_Sess_Obj + ActionName] != null)
                {
                    BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
                    if (BSM.ListTRTrainingCatalogueCourse.Count > 0)
                    {
                        var objCheck = BSM.ListTRTrainingCatalogueCourse.FirstOrDefault(w => w.TrainingCourseID == TrainingCourseID);
                        BSM.ListTRTrainingCatalogueCourse.Remove(objCheck);
                        BSM.CatalogueCourseID.Remove(TrainingCourseID.ToString());
                        BSM.ListCourseType = DBX.TRTrainingCourses.ToList();
                        Session[Index_Sess_Obj + ActionName] = BSM;
                    }
                }
            }
            return PartialView("GridItems", BSM);
        }
        public ActionResult CreateCourseItem(TRTrainingCatalogueCourse ModelObject)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm();

            TrainingProcessObject BSM = new TrainingProcessObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = Session[Index_Sess_Obj + ActionName] as TrainingProcessObject;
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

            return PartialView("GridItems", BSM);
        }
        #endregion
        public ActionResult PartialCourseSearch()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListCourseType = new List<TRTrainingCourse>();
            BSM.ListCourseType = DBX.TRTrainingCourses.ToList();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];

            }
            return PartialView("PartialCourseSearch", BSM);

        }
        public ActionResult GridItemCourse()
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfForm(ActionBehavior.ACC_REV);
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ScreenId = SCREENID;

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            else
            {
                BSM.ListCourseType = new List<TRTrainingCourse>();
            }

            ViewData["TRAINING_COURSE"] = DBX.TRTrainingCourses.ToList();

            BSM.GetCourseSetup();
            return PartialView("GridItemCourse", BSM);

        }
        public ActionResult SelectedCourse(string TrainingCourseID)
        {

            ActionName = "Create";
            UserSession();
            UserConfListAndForm();
            ViewData[SYSConstant.PARAM_ID1] = true;
            TrainingProcessObject BSM = new TrainingProcessObject();
            BSM.ListTRTrainingCatalogueCourse = new List<TRTrainingCatalogueCourse>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
            }
            if (TrainingCourseID != null)
            {
                string[] listCourses = TrainingCourseID.Split(',');
                var list = DBX.TRTrainingCourses.ToList();
                var list1 = BSM.ListTRTrainingCatalogueCourse.ToList();
                var courseTypes = list.Where(w => listCourses.Contains(w.TrainNo.ToString()));
                courseTypes = courseTypes.Where(w => !list1.Any(x => w.TrainNo == x.TrainingCourseID));

                listCourses.ToList().ForEach(x => BSM.CatalogueCourseID.Add(x));
                BSM.CatalogueCourseID = BSM.CatalogueCourseID.Distinct().ToList();
                int i = 0;
                foreach (var item in courseTypes.ToList())
                {
                    ++i;
                    BSM.ListTRTrainingCatalogueCourse.Add(new TRTrainingCatalogueCourse
                    {
                        TrainingCourseID = item.TrainNo,
                        Description = item.Code,
                        Remark = item.Description
                    }); ;
                }
                var result = new
                {
                    MS = SYConstant.OK,
                };
                Session[Index_Sess_Obj + ActionName] = BSM;
                return Json(result, JsonRequestBehavior.DenyGet);
            }

            var rs = new { MS = SYConstant.FAIL };
            return Json(rs, JsonRequestBehavior.DenyGet);
        }
        private void DataSelector()
        {
            ViewData[SYSConstant.PARAM_ID1] = true;
            List<TRTrainingCatalogueCourse> items = new List<TRTrainingCatalogueCourse>();
            TrainingProcessObject BSM = new TrainingProcessObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainingProcessObject)Session[Index_Sess_Obj + ActionName];
                items = BSM.ListTRTrainingCatalogueCourse;
            }
            var tRTrainingCourses = DBX.TRTrainingCourses.Where(w => items.Any(x => x.TrainingCourseID != w.TrainNo));
            ViewData["COURSE_TYPE"] = tRTrainingCourses;
            ViewData["TRAINING_COURSE"] = DBX.TRTrainingCourses.ToList();

        }
    }
}