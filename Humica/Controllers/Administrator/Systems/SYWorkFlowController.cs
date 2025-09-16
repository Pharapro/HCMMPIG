using Humica.Core.CF;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Administrator.Systems
{
    public class SYWorkFlowController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "SYWF0002";
        private const string URL_SCREEN = "/Administrator/Systems/SYWorkFlow/";
        private SMSystemEntity DBA = new SMSystemEntity();
        public SYWorkFlowController()
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
            //work flow ncx
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            //work flow item

            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            //Process Alert
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            return View(BSM);
        }

        [HttpPost]
        public ActionResult Index(CFSystem BSM)
        {
            UserSession();
            DataList();

            UserConf(ActionBehavior.EDIT);

            if (BSM.CopyFrom != null && BSM.ObjectCopying != null)
            {
                var DBU = new SMSystemEntity();
                var ListWorkFlowItem = DBA.CFWorkFlowItems.Where(w => w.WFObject == BSM.CopyFrom).ToList();
                var listiOr = DBA.CFWorkFlowItems.Where(w => w.WFObject == BSM.ObjectCopying).ToList();

                var ListWorkFlowApprover = DBA.CFWorkFlowApprovers.Where(w => w.WFObject == BSM.CopyFrom).ToList();
                var listaOr = DBA.CFWorkFlowApprovers.Where(w => w.WFObject == BSM.ObjectCopying).ToList();

                var objSource = DBA.CFWorkFlows.Find(BSM.ObjectCopying);
                var objDes = DBA.CFWorkFlows.Find(BSM.CopyFrom);

                if (objSource == null && objDes != null)
                {
                    objDes.WFObject = BSM.ObjectCopying;
                    objDes.Description = "Copy Of " + objDes.Description;

                    DBU.CFWorkFlows.Add(objDes);
                }

                foreach (var item in ListWorkFlowItem)
                {
                    var b = false;

                    foreach (var read in listiOr)
                    {
                        if (read.WFObject == item.WFObject)
                        {
                            b = true;

                            break;
                        }
                    }
                    if (b == false)
                    {
                        item.WFObject = BSM.ObjectCopying;
                        DBU.CFWorkFlowItems.Add(item);
                    }
                }

                foreach (var item in ListWorkFlowApprover)
                {
                    var b = false;

                    foreach (var read in listaOr)
                    {
                        if (read.WFObject == item.WFObject)
                        {
                            b = true;

                            break;
                        }
                    }
                    if (b == false)
                    {
                        item.WFObject = BSM.ObjectCopying;
                        DBU.CFWorkFlowApprovers.Add(item);
                    }
                }
                DBU.SaveChanges();

            }

            //work flow ncx
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            //work flow item
            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            //Process Alert
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            return View(BSM);
        }

        #endregion
        #region "Work Flow"
        public ActionResult GridItems1()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            DataList();
            return PartialView("GridItems1", BSM.ListWorkFlow);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial1(CFWorkFlow ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.CFWorkFlows.Add(ModelObject);
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
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWorkFlow);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial1(CFWorkFlow ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.CFWorkFlows.Attach(ModelObject);
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
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWorkFlow);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial1(string WFObject)
        {
            CFSystem BSM = new CFSystem();
            if (WFObject != null)
            {
                try
                {
                    var obj = DBA.CFWorkFlows.Find(WFObject);
                    if (obj != null)
                    {

                        DBA.CFWorkFlows.Remove(obj);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListWorkFlow = DBA.CFWorkFlows.ToList();
            return PartialView("GridItems1", BSM.ListWorkFlow);
        }
        #endregion
        #region "Work Flow NCX ITEM"
        public ActionResult GridItems2()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            DataList();
            return PartialView("GridItems2", BSM.ListWorkFlowItem);
        }
        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial2(CFWorkFlowItem ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {
                    DBA.CFWorkFlowItems.Add(ModelObject);
                    DataList();
                    int row = DBA.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserName;
                    log.DocurmentAction = ModelObject.ID.ToString();
                    log.Action = SYActionBehavior.EDIT.ToString();

                    SYEventLogObject.saveEventLog(log, e);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
                }
                catch (DbUpdateException e)
                {
                    /*------------------SaveLog----------------------------------*/
                    SYEventLog log = new SYEventLog();
                    log.ScreenId = SCREEN_ID;
                    log.UserId = user.UserName;
                    log.DocurmentAction = ModelObject.ID.ToString();
                    log.Action = SYActionBehavior.EDIT.ToString();

                    SYEventLogObject.saveEventLog(log, e, true);
                    /*----------------------------------------------------------*/
                    ViewData["EditError"] = e.Message;
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
            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            return PartialView("GridItems2", BSM.ListWorkFlowItem);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial2(CFWorkFlowItem ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.CFWorkFlowItems.Attach(ModelObject);
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
            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            return PartialView("GridItems2", BSM.ListWorkFlowItem);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial2(int ID)
        {
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {
                    var obj = DBA.CFWorkFlowItems.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objDel = obj.First();
                        DBA.CFWorkFlowItems.Remove(objDel);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListWorkFlowItem = DBA.CFWorkFlowItems.ToList();
            return PartialView("GridItems2", BSM.ListWorkFlowItem);
        }
        #endregion

        #region "Work Flow APPROVER"
        //public ActionResult GridItems4()
        //{
        //    UserConf(ActionBehavior.EDIT);
        //    CFSystem BSM = new CFSystem();
        //    BSM.ListWorkFlowApprover = DBA.CFWorkFlowApprovers.ToList();
        //    DataList();
        //    return PartialView("GridItems4", BSM.ListWorkFlowApprover);
        //}

        ////create
        //[HttpPost, ValidateInput(false)]
        //public ActionResult CreatePartial4(CFWorkFlowApprover ModelObject)
        //{

        //    CFSystem BSM = new CFSystem();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            DBA.CFWorkFlowApprovers.Add(ModelObject);
        //            DataList();
        //            int row = DBA.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
        //    }
        //    DataList();
        //    BSM.ListWorkFlowApprover = DBA.CFWorkFlowApprovers.ToList();
        //    return PartialView("GridItems4", BSM.ListWorkFlowApprover);
        //}
        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult EditPartial4(CFWorkFlowApprover ModelObject)
        //{
        //    CFSystem BSM = new CFSystem();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            DBA.CFWorkFlowApprovers.Attach(ModelObject);
        //            DBA.Entry(ModelObject).State = System.Data.Entity.EntityState.Modified;
        //            int row = DBA.SaveChanges();
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        ViewData["EditError"] = SYMessages.getMessage("EE004", user.Lang);
        //    }
        //    DataList();
        //    BSM.ListWorkFlowApprover = DBA.CFWorkFlowApprovers.ToList();
        //    return PartialView("GridItems4", BSM.ListWorkFlowApprover);
        //}
        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult DeletePartial4(int ID)
        //{
        //    CFSystem BSM = new CFSystem();
        //    if (ID != 0)
        //    {
        //        try
        //        {
        //            var obj = DBA.CFWorkFlowApprovers.Where(w => w.ID == ID).ToList();
        //            if (obj.Count > 0)
        //            {
        //                var objDel = obj.First();
        //                DBA.CFWorkFlowApprovers.Remove(objDel);
        //                int row = DBA.SaveChanges();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }
        //    DataList();
        //    BSM.ListWorkFlowApprover = DBA.CFWorkFlowApprovers.ToList();
        //    return PartialView("GridItems4", BSM.ListWorkFlowApprover);
        //}
        #endregion
        #region "Process Alert"
        public ActionResult GridItems5()
        {
            UserConf(ActionBehavior.EDIT);
            CFSystem BSM = new CFSystem();
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            DataList();
            return PartialView("GridItems5", BSM.ListProcessAlert);
        }

        //create
        [HttpPost, ValidateInput(false)]
        public ActionResult CreatePartial5(SYProcessAlert ModelObject)
        {

            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.SYProcessAlerts.Add(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE005", user.Lang);
            }
            DataList();
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            return PartialView("GridItems5", BSM.ListProcessAlert);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditPartial5(SYProcessAlert ModelObject)
        {
            CFSystem BSM = new CFSystem();
            if (ModelState.IsValid)
            {
                try
                {

                    DBA.SYProcessAlerts.Attach(ModelObject);
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
                ViewData["EditError"] = SYMessages.getMessage("EE005", user.Lang);
            }
            DataList();
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            return PartialView("GridItems5", BSM.ListProcessAlert);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeletePartial5(int ID)
        {
            CFSystem BSM = new CFSystem();
            if (ID != 0)
            {
                try
                {
                    var obj = DBA.SYProcessAlerts.Where(w => w.ID == ID).ToList();
                    if (obj.Count > 0)
                    {
                        var objAdd = obj.First();
                        DBA.SYProcessAlerts.Remove(objAdd);
                        int row = DBA.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            DataList();
            BSM.ListProcessAlert = DBA.SYProcessAlerts.ToList();
            return PartialView("GridItems5", BSM.ListProcessAlert);
        }
        #endregion
        #region "Private Code"
        private void DataList()
        {
            UserSession();
            ViewData["WORK_FLOW_TYPE"] = DBA.SYDatas.Where(w => w.DropDownType == "WORK_FLOW_TYPE").ToList();
            ViewData["WORK_FLOW_OBJECT"] = DBA.CFWorkFlows.ToList();
            // ViewData["COM_LIST"] = DBA.MDDealers.Where(w => w.IsActive == true).ToList();
            ViewData["EMT_LIST"] = DBA.TPEmailTemplates.ToList();
            ViewData["USER_LIST"] = DBA.SYUsers.Where(w => w.IsApprover == true && w.CompanyOwner == "NCX").ToList();
            ViewData["MODULE_LIST"] = new SYDataList("MODULE").ListData;
            ViewData["WORK_ACTION"] = new SYDataList("STATUS_APPROVAL").ListData;
        }
        #endregion
    }
}
