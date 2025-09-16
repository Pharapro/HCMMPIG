using Humica.Core.CF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Humica.Controllers.Config.Setup
{

    public class SYEmailTemplateController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYEMT001";
        private const string URL_SCREEN = "/Administrator/Systems/SYEmailTemplate/";
        private SMSystemEntity DBA = new SMSystemEntity();
        public SYEmailTemplateController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            DataList();

            UserConf(ActionBehavior.EDIT);

            CFSystem BSM = new CFSystem();
            //Email Account
            BSM.ListEmailAccount = DBA.CFEmailAccounts.ToList();
            //Email Template
            BSM.ListEmailTemplate = DBA.TPEmailTemplates.ToList();
            //User Template
            BSM.ListEmailParameter = DBA.SYEmailParameters.ToList();

            return View(BSM);
        }
        #endregion
        #region "Email Account"
        public ActionResult GridItems1()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListEmailAccount = DBA.CFEmailAccounts.ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListEmailAccount);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFEmailAccount ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.CFEmailAccounts.Add(ModelObject);
                    DataList();
                    int row = DBA.SaveChanges();
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
            DataList();
            BSM.ListEmailAccount = DBA.CFEmailAccounts.ToList();
            return PartialView("GridItems1", BSM.ListEmailAccount);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFEmailAccount ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.CFEmailAccounts.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
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
            DataList();
            BSM.ListEmailAccount = DBA.CFEmailAccounts.ToList();
            return PartialView("GridItems1", BSM.ListEmailAccount);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(string SMTPObject)
        {
            CFSystem BSM = new CFSystem();
            if (SMTPObject != null)
            {
                try
                {
                    var obj = DBA.CFEmailAccounts.Find(SMTPObject);
                    if (obj != null)
                    {

                        DBA.CFEmailAccounts.Remove(obj);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListEmailAccount = DBA.CFEmailAccounts.ToList();
            return PartialView("GridItems1", BSM.ListEmailAccount);
        }
        #endregion
        #region "Email Template"
        public ActionResult GridItems2()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListEmailTemplate = DBA.TPEmailTemplates.ToList();
            DataList();
            return PartialView("GridItems2", BSM.ListEmailTemplate);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial2(TPEmailTemplate ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.TPEmailTemplates.Add(ModelObject);
                    DataList();
                    int row = DBA.SaveChanges();
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
            BSM.ListEmailTemplate = DBA.TPEmailTemplates.ToList();
            return PartialView("GridItems2", BSM.ListEmailTemplate);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial2(TPEmailTemplate ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.TPEmailTemplates.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
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
            BSM.ListEmailTemplate = DBA.TPEmailTemplates.ToList();
            return PartialView("GridItems2", BSM.ListEmailTemplate);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial2(string EMTemplateObject)
        {
            CFSystem BSM = new CFSystem();
            if (EMTemplateObject != null)
            {
                try
                {
                    var obj = DBA.TPEmailTemplates.Find(EMTemplateObject);
                    if (obj != null)
                    {

                        DBA.TPEmailTemplates.Remove(obj);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListEmailTemplate = DBA.TPEmailTemplates.ToList();
            return PartialView("GridItems2", BSM.ListEmailTemplate);
        }
        #endregion
        #region "Email User"
        public ActionResult GridItems3()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListEmailParameter = DBA.SYEmailParameters.ToList();
            DataList();
            return PartialView("GridItems3", BSM.ListEmailParameter);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial3(SYEmailParameter ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.SYEmailParameters.Add(ModelObject);
                    DataList();
                    int row = DBA.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE003", user.Lang);
            }
            DataList();
            BSM.ListEmailParameter = DBA.SYEmailParameters.ToList();
            return PartialView("GridItems3", BSM.ListEmailParameter);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial3(SYEmailParameter ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    DBA.SYEmailParameters.Attach(ModelObject);
                    DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
                    int row = DBA.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
            {
                ViewData["EditError"] = SYMessages.getMessage("EE003", user.Lang);
            }
            DataList();
            BSM.ListEmailParameter = DBA.SYEmailParameters.ToList();
            return PartialView("GridItems3", BSM.ListEmailParameter);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial3(int ID)
        {
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {
                    var obj = DBA.SYEmailParameters.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {

                        DBA.SYEmailParameters.Remove(obj.First());
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListEmailParameter = DBA.SYEmailParameters.ToList();
            return PartialView("GridItems3", BSM.ListEmailParameter);
        }
        #endregion
        #region "Private Code"
        private void DataList()
        {
            UserSession();
            ViewData["SMTP_LIST"] = DBA.CFEmailAccounts.ToList();
            ViewData["USER_LIST"] = DBA.SYUsers.Where(w => w.IsActive == true).ToList();
            ViewData["TEMPLATE_ID"] = DBA.TPEmailTemplates.ToList();
        }
        #endregion
    }
}
