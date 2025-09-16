using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Developments
{
    public class NMessageController : Humica.EF.Controllers.MasterSaleController
    {
        const string SCREENID = "SYMS00001";
        const string URL_Screen = "/Administrator/Developments/NMessage/";
        SMSystemEntity DBA = new SMSystemEntity();

        public NMessageController()
            : base()
        {
            this.ScreendIDControl = SCREENID;
            this.ScreenUrl = URL_Screen;
        }
        #region "List"
        public ActionResult Index()
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.LISTR);
            SYMessages BSM = new SYMessages();
            BSM.ListItem = DBA.SYMessages.Where(w => w.Lang == user.Lang).OrderBy(w => w.MessageID).ToList();
            return View(BSM);
        }
        public ActionResult GridItems1()
        {
            UserSession();
            DataList();
            UserConf(ActionBehavior.LISTR);
            SYMessages BSM = new SYMessages();
            BSM.ListItem = DBA.SYMessages.Where(w => w.Lang == user.Lang).OrderBy(w => w.MessageID).ToList();
            return PartialView("GridItems1", BSM.ListItem);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(SYMessage MModel)
        {
            UserSession();
            SYMessages BSM = new SYMessages();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Lang = user.Lang;
                    DBA.SYMessages.Add(MModel);

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
            BSM.ListItem = DBA.SYMessages.Where(w => w.Lang == user.Lang).OrderBy(w => w.MessageID).ToList();
            return PartialView("GridItems1", BSM.ListItem);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(SYMessage MModel)
        {
            UserSession();
            SYMessages BSM = new SYMessages();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.Lang = user.Lang;
                    DBA.SYMessages.Attach(MModel);
                    DBA.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
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
            BSM.ListItem = DBA.SYMessages.Where(w => w.Lang == user.Lang).OrderBy(w => w.MessageID).ToList();
            return PartialView("GridItems1", BSM.ListItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(string MessageID)
        {
            UserSession();
            SYMessages BSM = new SYMessages();
            if (MessageID != null)
            {
                try
                {
                    var obj = DBA.SYMessages.Find(MessageID, user.Lang);
                    if (obj != null)
                    {
                        DBA.SYMessages.Remove(obj);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListItem = DBA.SYMessages.Where(w => w.Lang == user.Lang).OrderBy(w => w.MessageID).ToList();
            return PartialView("GridItems1", BSM.ListItem);
        }

        #endregion

        private void DataList()
        {
            UserSession();
            ViewData["MESSAGE_CLASS"] = new SYDataList("MESSAGE_CLASS").ListData;
            ViewData["MESSAGE_TYPE"] = new SYDataList("MESSAGE_TYPE").ListData;
        }

    }
}
