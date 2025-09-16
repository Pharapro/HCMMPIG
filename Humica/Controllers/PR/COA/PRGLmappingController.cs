using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.PR;
using Humica.Models.SY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.PR.PRM
{
    public class PRGLmappingController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "PRM0000016";
        private const string URL_SCREEN = "/PR/PRM/PRGLmapping/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "GLMCode";
        HumicaDBContext DB = new HumicaDBContext();
        HumicaDBViewContext DBV = new HumicaDBViewContext();

        public PRGLmappingController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            ActionName = "Index";
            DataSelector();
            UserSession();
            UserConfListAndForm(this.KeyName);

            PRGLmappingObject BSM = new PRGLmappingObject();

            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }

            BSM.ListCostCenter = DB.PRCostCenters.ToList();
            BSM.ListHeader = DB.PRGLmappings.ToList();
            BSM.ListGLMapping = DBV.PR_TEMP_SYGL_VIEW.OrderBy(w => w.SortKey).ToList();

            Session[Index_Sess_Obj + ActionName] = BSM;

            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.ListCostCenter = new List<PRCostCenter>();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListCostCenter);
        }


        public ActionResult Create()
        {
            ActionName = "Create";
            DataSelector();
            UserSession();
            PRGLmappingObject BSM = new PRGLmappingObject();
            UserConfListAndForm(this.KeyName);
            BSM.HeaderCost = new PRCostCenter();
            BSM.ListHeader = new List<PRGLmapping>();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public ActionResult Create(PRGLmappingObject collection)
        {
            ActionName = "Create";
            UserSession();
            DataSelector();
            UserConfListAndForm(this.KeyName);
            PRGLmappingObject BSM = new PRGLmappingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderCost = collection.HeaderCost;
            }

            if (ModelState.IsValid)
            {
                BSM.ScreenId = SCREEN_ID;
                try
                {
                    string msg = BSM.CreateGLMapping();
                    if (msg == SYConstant.OK)
                    {
                        SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                        mess.DocumentNumber = BSM.HeaderCost.Project.ToString();
                        mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit/" + mess.DocumentNumber;

                        Session[Index_Sess_Obj + ActionName] = BSM;
                        Session[SYConstant.MESSAGE_SUBMIT] = mess;
                        return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Create");
                    }
                    else
                    {
                        Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        return View(BSM);
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            else
            {
                Session["EditError"] = SYMessages.getMessage("EE001", user.Lang);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }

        public ActionResult Edit(int id)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            PRGLmappingObject BSM = new PRGLmappingObject();
            BSM.HeaderCost = DB.PRCostCenters.FirstOrDefault(w => w.ID == id);
            if (BSM.HeaderCost != null)
            {
                BSM.LoadDataMapping(id);
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        [HttpPost]
        public ActionResult Edit(int id, PRGLmappingObject collection)
        {
            ActionName = "Edit";
            UserSession();
            DataSelector();
            this.ScreendIDControl = SCREEN_ID;
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;

            var BSM = new PRGLmappingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
                BSM.HeaderCost = collection.HeaderCost;
            }
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = BSM.EditGLMapping(id);
                if (msg == SYConstant.OK)
                {
                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Edit?id=" + mess.DocumentNumber;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
                else
                {
                    Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            //EE001= Message load Error data not complate
            Session[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("EE001", user.Lang);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);

        }

        #region Grid Edit Action
        public ActionResult GridItemsEdit()
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRGLmappingObject BSM = new PRGLmappingObject();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];
            }

            return PartialView("GridItemsEdit", BSM);
        }

        public ActionResult EditItemEdit(PRGLmapping ModelObject)
        {
            ActionName = "Edit";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            PRGLmappingObject BSM = new PRGLmappingObject();
            //BSM.ListAllocationDestination = new List<GLAllocationDestination>();
            if (ModelState.IsValid)
            {
                try
                {
                    if (Session[Index_Sess_Obj + ActionName] != null)
                    {
                        BSM = (PRGLmappingObject)Session[Index_Sess_Obj + ActionName];

                        var objCheck = BSM.ListHeader.Where(w => w.BenCode == ModelObject.BenCode).ToList();
                        if (objCheck.Count > 0)
                        {
                            objCheck.First().GLCode = ModelObject.GLCode;
                            objCheck.First().MaterialCode = ModelObject.MaterialCode;
                            objCheck.First().Remark = ModelObject.Remark;
                        }
                        else
                        {
                            ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST");
                        }

                    }
                    else
                    {
                        Session[Index_Sess_Obj + ActionName] = BSM;
                        ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
            return PartialView("GridItemsEdit", BSM);
        }

        #endregion

        //[HttpPost, ValidateInput(false)]
        //public ActionResult Create(PRGLmapping MModel)
        //{
        //    UserSession();
        //    DataSelector();
        //    PRGLmappingObject BSM = new PRGLmappingObject();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            var objUpdate = DB.PRGLmappings.Where(w=>w.BenCode == MModel.BenCode).ToList();
        //            if(objUpdate.Count > 0)
        //            {
        //                ViewData["EditError"] = SYMessages.getMessage("RECORD_EXIST"); ;
        //            }
        //            else
        //            {
        //                var objCheck = DB.PR_TEMP_SYGL_VIEW.Where(w => w.AccCode == MModel.BenCode).First();

        //                MModel.BenCode = MModel.BenCode.ToUpper();
        //                MModel.BenGrp = objCheck.GroupAcc.ToUpper(); 
        //                MModel.CreateBy = User.UserName;
        //                MModel.CreateOn = DateTime.Now;
        //                DB.PRGLmappings.Add(MModel);
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
        //    BSM.ListHeader = DB.PRGLmappings.ToList();
        //    return PartialView("Gridview", BSM.ListHeader);
        //}
        ////edit
        //[HttpPost, ValidateInput(false)]
        //public ActionResult Edit(PRGLmapping MModel)
        //{
        //    UserSession();
        //    DataSelector();
        //    PRGLmappingObject BSM = new PRGLmappingObject();
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var DBU = new DBBusinessProcess();
        //            //var objUpdate = DBU.PRChartofAccounts.Find(MModel.Code);
        //            var objUpdate = DB.PRGLmappings.Where(w => w.BenCode == MModel.BenCode).ToList();
        //            if (objUpdate.Count > 0)
        //            {
        //                MModel.ChangedBy = User.UserName;
        //                MModel.ChangedOn = DateTime.Now;
        //                DP.PRGLmappings.Attach(MModel);
        //                DP.Entry(MModel).State = System.Data.Entity.EntityState.Modified;
        //                int row = DP.SaveChanges();
        //            }
        //            else
        //            {
        //                ViewData["EditError"] = SYMessages.getMessage("NO_CATCHING");
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
        //    BSM.ListHeader = DP.PRGLmappings.OrderBy(w => w.BenCode).ToList();
        //    return PartialView("Gridview", BSM.ListHeader);
        //}
        ////delete
        //[HttpPost, ValidateInput(false)]
        //public ActionResult Delete(string BenCode)
        //{
        //    UserSession();
        //    DataSelector();
        //    PRGLmappingObject BSM = new PRGLmappingObject();
        //    if (BenCode != null)
        //    {
        //        try
        //        {

        //            var obj = DP.PRGLmappings.Where(w=>w.BenCode== BenCode).First();
        //            if (obj != null)
        //            {
        //                DP.PRGLmappings.Remove(obj);
        //                int row = DP.SaveChanges();
        //            }
        //            BSM.ListHeader = DP.PRGLmappings.OrderBy(w => w.BenCode).ToList();

        //            //var objEmp = DB.HREmpCareers.ToList();
        //            //if (objEmp.Where(w => w.Branch == Code).Any())
        //            //{
        //            //    ViewData["EditError"] = SYMessages.getMessage("DATA_USE", user.Lang);
        //            //}
        //            //else
        //            //{
        //            //    var obj = DP.HRBranches.Find(Code);
        //            //    if (obj != null)
        //            //    {
        //            //        DP.HRBranches.Remove(obj);
        //            //        int row = DP.SaveChanges();
        //            //    }
        //            //    BSM.ListHeader = DP.PRChartofAccounts.OrderBy(w => w.Code).ToList();
        //            //}
        //        }
        //        catch (Exception e)
        //        {
        //            ViewData["EditError"] = e.Message;
        //        }
        //    }

        //    return PartialView("Gridview", BSM.ListHeader);
        //}

        private void DataSelector()
        {
            //ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            //string ALLType = "ALLW";
            //ViewData["ALLOWANCE_SELECT"] = DB.PR_RewardsType.Where(w=>w.ReCode== ALLType).ToList();
            ViewData["COA_SELECT"] = DB.PRChartofAccounts.ToList();
            ViewData["Material_SELECT"] = DB.HRMaterialAccounts.ToList();
            ViewData["TEMP_SYGL_SELECT"] = DBV.PR_TEMP_SYGL_VIEW.OrderBy(w => w.SortKey).ToList();
        }
    }
}