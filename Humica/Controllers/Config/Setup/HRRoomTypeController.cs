using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{
    public class HRRoomTypeController : Humica.EF.Controllers.MasterSaleController

    {
        private const string SCREEN_ID = "CNF0000007";
        private const string URL_SCREEN = "/Config/Setup/HRRoomType/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "RoomCode";
        HumicaDBContext DB = new HumicaDBContext();

        public HRRoomTypeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }

        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            MDRoomType BSM = new MDRoomType();
            BSM.ListHeader = new List<HRRoomType>();
            BSM.ListHeader = DB.HRRoomTypes.ToList();
            return View(BSM);
        }
        public ActionResult GridRoomType()
        {
            UserConf(ActionBehavior.EDIT);

            MDRoomType BSM = new MDRoomType();
            BSM.ListHeader = DB.HRRoomTypes.ToList();
            return PartialView("GridRoomType", BSM.ListHeader);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateRoomType(HRRoomType MModel)
        {
            UserSession();
            MDRoomType BSM = new MDRoomType();
            if (ModelState.IsValid)
            {
                try
                {
                    MModel.RoomCode = MModel.RoomCode.ToUpper();
                    DB.HRRoomTypes.Add(MModel);
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
            BSM.ListHeader = DB.HRRoomTypes.ToList();
            return PartialView("GridRoomType", BSM.ListHeader);
        }
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditRoomType(HRRoomType MModel)
        {
            UserSession();
            MDRoomType BSM = new MDRoomType();
            if (ModelState.IsValid)
            {
                try
                {
                    var DBU = new HumicaDBContext();
                    var objUpdate = DBU.HRRoomTypes.Find(MModel.RoomCode);
                    DB.HRRoomTypes.Attach(MModel);
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
            BSM.ListHeader = DB.HRRoomTypes.OrderBy(w => w.RoomCode).ToList();
            return PartialView("GridRoomType", BSM.ListHeader);
        }
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteRoomType(string RoomCode)
        {
            UserSession();
            MDRoomType BSM = new MDRoomType();
            if (RoomCode != "null")
            {
                try
                {
                    var obj = DB.HRRoomTypes.Find(RoomCode);
                    if (obj != null)
                    {
                        DB.HRRoomTypes.Remove(obj);
                        int row = DB.SaveChanges();

                    }
                    BSM.ListHeader = DB.HRRoomTypes.OrderBy(w => w.RoomCode).ToList();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }

            return PartialView("GridRoomType", BSM.ListHeader);
        }
    }
}
