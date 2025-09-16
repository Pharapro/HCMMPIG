using Humica.EF;
using Humica.EF.MD;
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
    public class TRTrainingProgramItemsController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "TRM000001";
        private const string URL_SCREEN = "/Training/Setup/TRTrainingProgramItems/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string Index_Sess_Msg = SYSConstant.SAVE_COMPLETED + SCREEN_ID;
        private string ActionName = "";
        private string KeyName = "ProgramCode";
        private string status = SYDocumentStatus.OPEN.ToString();

        HumicaDBContextTR DB = new HumicaDBContextTR();
        SMSystemEntity SMS = new SMSystemEntity();
        Humica.Core.DB.HumicaDBContext DBX = new Humica.Core.DB.HumicaDBContext();

        public TRTrainingProgramItemsController() : base()
        {
            ScreendIDControl = SCREEN_ID;
            ScreenUrl = URL_SCREEN;
        }

        #region "List"
        // GET: /NMaterialModel/
        public ActionResult Index()
        {

            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfList(KeyName);
            TrainningObject BSM = new TrainningObject();
            BSM.ListHeader = new List<TrainingProgram>();
            BSM.ListHeader = DB.TrainingProgram.OrderBy(w => w.ID).ToList();
            BSM.ListCourseMaster = DB.TrainingCourseMaster.OrderBy(w => w.ID).ToList();
            BSM.ListTrainingType = DB.TRTrainingTypes.OrderBy(w => w.Code).ToList();
            BSM.ListTRTrainingVenues = DB.TRTrainingVenues.OrderBy(w => w.Code).ToList();
            //BSM.ListCourseType = DB.TRTrainingCourseType.OrderBy(w => w.Code).ToList();
            BSM.ListTRCourseCategory = DB.TRCourseCategories.OrderBy(w => w.Code).ToList();
            BSM.ListCourseType = DB.TRTrainingCourses.OrderBy(w => w.Code).ToList();
            BSM.ListTRTrainingCategory = DB.TRTrainingCategories.OrderBy(w => w.Code).ToList();
            BSM.ListTRTopic = DB.TRTopics.OrderBy(w => w.Code).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        #endregion

        #region "_GridItem"
        public ActionResult _GridItem()
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
            BSM.ListHeader = DB.TrainingProgram.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(TrainingProgram ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TrainingProgram.FirstOrDefault(w => w.ProgramCode == ModelObject.ProgramCode);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (
                         ModelObject.ProgramCode == null || ModelObject.ProgramName == null
                        )
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            DB.TrainingProgram.Add(ModelObject);
                            DB.SaveChanges();
                            //  Session["FILES_NAME"] = null;
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.TrainingProgram.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(TrainingProgram ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TrainingProgram.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        if (
                         ModelObject.ProgramCode == null || ModelObject.ProgramName == null
                        )
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            check.ProgramName = ModelObject.ProgramName;
                            DB.TrainingProgram.Attach(check);
                            DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            int row = DB.SaveChanges();
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

            BSM.ListHeader = DB.TrainingProgram.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem", BSM.ListHeader);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete(int ID)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TrainingProgram.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TrainingProgram.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListHeader = DB.TrainingProgram.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem", BSM.ListHeader);
        }
        #endregion

        #region "_GridItem1"
        public ActionResult _GridItem1()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            DataListSelector();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListCourseMaster = DB.TrainingCourseMaster.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem1", BSM.ListCourseMaster);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create1(TrainingCourseMaster ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TrainingCourseMaster.FirstOrDefault(w => w.Program == ModelObject.Program && w.Code == ModelObject.Code && w.StaffLevel == ModelObject.StaffLevel);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (
                         ModelObject.Program == null || ModelObject.StaffLevel == null || ModelObject.Code == null || ModelObject.Description == null
                        )
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            DB.TrainingCourseMaster.Add(ModelObject);
                            DB.SaveChanges();
                            //  Session["FILES_NAME"] = null;
                        }

                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListCourseMaster = DB.TrainingCourseMaster.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem1", BSM.ListCourseMaster);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit1(TrainingCourseMaster ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {

                try
                {
                    var check = DB.TrainingCourseMaster.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        if (
                         ModelObject.Program == null || ModelObject.Code == null || ModelObject.Description == null
                        )
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            check.Program = ModelObject.Program;
                            check.Code = ModelObject.Code;
                            check.StaffLevel = ModelObject.StaffLevel;
                            check.Description = ModelObject.Description;
                            DB.TrainingCourseMaster.Attach(check);
                            DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
                            int row = DB.SaveChanges();
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

            BSM.ListCourseMaster = DB.TrainingCourseMaster.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem1", BSM.ListCourseMaster);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Delete1(int ID)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID > 0)
            {
                try
                {
                    var obj = DB.TrainingCourseMaster.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {

                        DB.TrainingCourseMaster.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListCourseMaster = DB.TrainingCourseMaster.OrderBy(w => w.ID).ToList();
            return PartialView("_GridItem1", BSM.ListCourseMaster);
        }
        #endregion
        #region "Training Type"
        public ActionResult gvTrainingType()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTrainingType = DB.TRTrainingTypes.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingType", BSM.ListTrainingType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTRType(TRTrainingType ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == ModelObject.Code);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (
                         ModelObject.Code == null || ModelObject.Code == null
                        )
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            DB.TRTrainingTypes.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingType = DB.TRTrainingTypes.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingType", BSM.ListTrainingType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTRType(TRTrainingType ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingTypes.FirstOrDefault(w => w.Code == ModelObject.Code);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.SecondDescription = ModelObject.SecondDescription;
                        DB.TRTrainingTypes.Attach(check);
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

            BSM.ListTrainingType = DB.TRTrainingTypes.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingType", BSM.ListTrainingType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTRType(int ID)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID != null)
            {
                try
                {
                    var obj = DB.TRTrainingTypes.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {
                        DB.TRTrainingTypes.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTrainingType = DB.TRTrainingTypes.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingType", BSM.ListTrainingType);
        }
        #endregion
        #region Training Venue
        public ActionResult gvTrainingVenue()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTRTrainingVenues = DB.TRTrainingVenues.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingVenue", BSM.ListTRTrainingVenues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTRVenue(TRTrainingVenue ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingVenues.FirstOrDefault(w => w.Code == ModelObject.Code);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ModelObject.Code))
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            ModelObject.Code = ModelObject.Code.ToUpper();
                            DB.TRTrainingVenues.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTrainingVenues = DB.TRTrainingVenues.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingVenue", BSM.ListTRTrainingVenues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTRVenue(TRTrainingVenue ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingVenues.FirstOrDefault(w => w.Code == ModelObject.Code);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.Remark = ModelObject.Remark;
                        check.ContactPerson = ModelObject.ContactPerson;
                        check.ContactEmail = ModelObject.ContactEmail;
                        check.ContactPhone = ModelObject.ContactPhone;
                        check.Address = ModelObject.Address;
                        DB.TRTrainingVenues.Attach(check);
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

            BSM.ListTRTrainingVenues = DB.TRTrainingVenues.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingVenue", BSM.ListTRTrainingVenues);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTRVenue(int TrainNo)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (TrainNo != null)
            {
                try
                {
                    var obj = DB.TRTrainingVenues.FirstOrDefault(w => w.TrainNo == TrainNo);
                    if (obj != null)
                    {
                        DB.TRTrainingVenues.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTrainingVenues = DB.TRTrainingVenues.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingVenue", BSM.ListTRTrainingVenues);
        }
        #endregion
        #region training course
        //public ActionResult gvCourseType()
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    UserConfListAndForm();
        //    TrainningObject BSM = new TrainningObject();
        //    if (Session[Index_Sess_Obj + ActionName] != null)
        //    {
        //        BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
        //    }
        //    BSM.ListCourseType = DB.TRTrainingCourseType.OrderBy(w => w.Code).ToList();
        //    return PartialView("gvCourseType", BSM.ListCourse);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreateCourseType(TRTrainingCourseType ModelObject)
        //{
        //    ActionName = "Index";
        //    UserSession();
        //    DataListSelector();
        //    UserConfListAndForm();
        //    TrainningObject BSM = new TrainningObject();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var check = DB.TRTrainingCourseType.FirstOrDefault(w => w.Code == ModelObject.Code);

        //            if (check != null)
        //            {
        //                ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
        //            }
        //            else
        //            {
        //                if (string.IsNullOrEmpty(ModelObject.Code))
        //                {
        //                    ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
        //                }
        //                else
        //                {
        //                    ModelObject.Code = ModelObject.Code.ToUpper();
        //                    ModelObject.CreatedBy = user.UserName;
        //                    ModelObject.CreatedOn = DateTime.Now;
        //                    DB.TRTrainingCourseType.Add(ModelObject);
        //                    DB.SaveChanges();
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    BSM.ListCourseType = DB.TRTrainingCourseType.OrderBy(w => w.Code).ToList();
        //    return PartialView("gvCourseType", BSM.ListCourse);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditCourseType(TRTrainingCourseType ModelObject)
        //{
        //    UserSession();
        //    DataListSelector();
        //    UserConfListAndForm();
        //    TrainningObject BSM = new TrainningObject();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var check = DB.TRTrainingCourseType.FirstOrDefault(w => w.Code == ModelObject.Code);
        //            if (check != null)
        //            {
        //                check.Description = ModelObject.Description;
        //                check.Remark = ModelObject.Remark;
        //                check.SecondDescription = ModelObject.SecondDescription;
        //                check.ChangedBy = user.UserName;
        //                check.ChangedOn = DateTime.Now;
        //                DB.TRTrainingCourseType.Attach(check);
        //                DB.Entry(check).State = System.Data.Entity.EntityState.Modified;
        //                int row = DB.SaveChanges();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE001", user.Lang);
        //    }

        //    BSM.ListCourseType = DB.TRTrainingCourseType.OrderBy(w => w.Code).ToList();
        //    return PartialView("gvCourseType", BSM.ListCourse);
        //}
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeleteCourseType(int TrainNo)
        //{
        //    UserSession();
        //    DataListSelector();
        //    UserConfListAndForm();
        //    TrainningObject BSM = new TrainningObject();
        //    if (TrainNo != null)
        //    {
        //        try
        //        {
        //            var obj = DB.TRTrainingCourseType.FirstOrDefault(w => w.TrainNo == TrainNo);
        //            if (obj != null)
        //            {
        //                DB.TRTrainingCourseType.Remove(obj);
        //                int row = DB.SaveChanges();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    BSM.ListCourseType = DB.TRTrainingCourseType.OrderBy(w => w.Code).ToList();
        //    return PartialView("gvCourseType", BSM.ListCourse);
        //}
        #endregion
        #region Training Category
        public ActionResult gvTrainingCate()
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            //UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTRTrainingCategory = DB.TRTrainingCategories.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingCate", BSM.ListTRTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTRCate(TRTrainingCategory ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(ModelObject.Code))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                    }
                    else
                    {
                        DB.TRTrainingCategories.Add(ModelObject);
                        DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTrainingCategory = DB.TRTrainingCategories.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingCate", BSM.ListTRTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTRCate(TRTrainingCategory ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingCategories.FirstOrDefault(w => w.ID == ModelObject.ID);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.SecondDescription = ModelObject.SecondDescription;
                        check.Remark = ModelObject.Remark;
                        DB.TRTrainingCategories.Attach(check);
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

            BSM.ListTRTrainingCategory = DB.TRTrainingCategories.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingCate", BSM.ListTRTrainingCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTRCate(int ID)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ID != null)
            {
                try
                {
                    var obj = DB.TRTrainingCategories.FirstOrDefault(w => w.ID == ID);
                    if (obj != null)
                    {
                        DB.TRTrainingCategories.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTrainingCategory = DB.TRTrainingCategories.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingCate", BSM.ListTRTrainingCategory);
        }
        #endregion

        #region course category
        public ActionResult _CourseCategory()
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTRCourseCategory = DB.TRCourseCategories.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseCategory", BSM.ListTRCourseCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CourseCateCreate(TRCourseCategory ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrEmpty(ModelObject.Code))
                    {
                        ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                    }
                    else
                    {
                        ModelObject.Code = ModelObject.Code.ToUpper();
                        ModelObject.CreatedBy = user.UserName;
                        ModelObject.CreateOn = DateTime.Now;
                        DB.TRCourseCategories.Add(ModelObject);
                        DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRCourseCategory = DB.TRCourseCategories.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseCategory", BSM.ListTRCourseCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CourseCateEdit(TRCourseCategory ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRCourseCategories.FirstOrDefault(w => w.Code == ModelObject.Code && w.TrainNo == ModelObject.TrainNo);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.Remark = ModelObject.Remark;
                        check.SecondDescription = ModelObject.SecondDescription;
                        check.ChangedBy = user.UserName;
                        check.ChangeOn = DateTime.Now;
                        DB.TRCourseCategories.Attach(check);
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

            BSM.ListTRCourseCategory = DB.TRCourseCategories.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseCategory", BSM.ListTRCourseCategory);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CourseCateDelete(int TrainNo)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (TrainNo != null)
            {
                try
                {
                    var obj = DB.TRCourseCategories.FirstOrDefault(w => w.TrainNo == TrainNo);
                    if (obj != null)
                    {
                        DB.TRCourseCategories.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRCourseCategory = DB.TRCourseCategories.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseCategory", BSM.ListTRCourseCategory);
        }
        #endregion
        #region course type
        public ActionResult _CourseType()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListCourseType = DB.TRTrainingCourses.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseType", BSM.ListCourseType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCourse(TRTrainingCourse ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == ModelObject.Code);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ModelObject.Code))
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            ModelObject.Code = ModelObject.Code.ToUpper();
                            ModelObject.CreatedBy = user.UserName;
                            ModelObject.CreatedOn = DateTime.Now;
                            DB.TRTrainingCourses.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListCourseType = DB.TRTrainingCourses.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseType", BSM.ListCourseType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditCourse(TRTrainingCourse ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == ModelObject.Code);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.Objective = ModelObject.Objective;
                        check.SecondDescription = ModelObject.SecondDescription;
                        check.ChangedBy = user.UserName;
                        check.ChangedOn = DateTime.Now;
                        DB.TRTrainingCourses.Attach(check);
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

            BSM.ListCourseType = DB.TRTrainingCourses.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseType", BSM.ListCourseType);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteCourse(string Code)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Code != null)
            {
                try
                {
                    var obj = DB.TRTrainingCourses.FirstOrDefault(w => w.Code == Code);
                    if (obj != null)
                    {
                        DB.TRTrainingCourses.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListCourseType = DB.TRTrainingCourses.OrderBy(w => w.Code).ToList();
            return PartialView("_CourseType", BSM.ListCourseType);
        }
        #endregion

        private void DataListSelector()
        {
            SYDataList DL = new SYDataList("CLAIM_TEYP_SELECT");
            ViewData["CLAIM_TEYP_SELECT"] = DL.ListData;
            ViewData["STAFF_SELECT"] = DBX.HRStaffProfiles.ToList();
            // ViewData["TRUCK_TYPE"] = DB.CFDPTruckAllows.ToList();
            ViewData["Program_List"] = DB.TrainingProgram.ToList();
            ViewData["TrainingType_SELECT"] = DB.TRTrainingTypes.ToList();
            ViewData["CourseType_SELECT"] = DB.TRTrainingCourses.ToList();
            ViewData["StaffLevel_List"] = SMS.HRLevels.ToList();

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

        #region Topic
        public ActionResult gvTrainingTopic()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (TrainningObject)Session[Index_Sess_Obj + ActionName];
            }
            BSM.ListTRTopic = DB.TRTopics.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingTopic", BSM.ListTRTopic);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateTopic(TRTopic ModelObject)
        {
            ActionName = "Index";
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTopics.FirstOrDefault(w => w.Code == ModelObject.Code);

                    if (check != null)
                    {
                        ViewData["EditError"] = SYMessages.getMessage("ISDUPLICATED", user.Lang);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(ModelObject.Code))
                        {
                            ViewData["EditError"] = SYMessages.getMessage("REQUIRED_FIELD(*)", user.Lang);
                        }
                        else
                        {
                            ModelObject.Code = ModelObject.Code.ToUpper();
                            DB.TRTopics.Add(ModelObject);
                            DB.SaveChanges();
                        }
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTopic = DB.TRTopics.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingTopic", BSM.ListTRTopic);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult EditTopic(TRTopic ModelObject)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (ModelState.IsValid)
            {
                try
                {
                    var check = DB.TRTopics.FirstOrDefault(w => w.Code == ModelObject.Code);
                    if (check != null)
                    {
                        check.Description = ModelObject.Description;
                        check.Remark = ModelObject.Remark;
                        DB.TRTopics.Attach(check);
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

            BSM.ListTRTopic = DB.TRTopics.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingTopic", BSM.ListTRTopic);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteTopic(int TrainNo)
        {
            UserSession();
            DataListSelector();
            UserConfListAndForm();
            TrainningObject BSM = new TrainningObject();
            if (TrainNo != null)
            {
                try
                {
                    var obj = DB.TRTopics.FirstOrDefault(w => w.TrainNo == TrainNo);
                    if (obj != null)
                    {
                        DB.TRTopics.Remove(obj);
                        int row = DB.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            BSM.ListTRTopic = DB.TRTopics.OrderBy(w => w.Code).ToList();
            return PartialView("gvTrainingTopic", BSM.ListTRTopic);
        }
        #endregion
    }
}

