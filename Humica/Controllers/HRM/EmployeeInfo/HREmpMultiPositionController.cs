using Humica.Core;
using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Logic.HR;
using Humica.Logic.HRS;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.EmployeeInfo
{
    public class HREmpMultiPositionController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRE0000017";
        private const string URL_SCREEN = "/HRM/EmployeeInfo/HREmpMultiPosition/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        HumicaDBContext DB = new HumicaDBContext();
        public HREmpMultiPositionController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        // GET: Tree
        public ActionResult Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
            BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
            return View(BSM);
        }

        #region Multi Position
        public ActionResult GridviewMultiPosition()
        {
            UserConf(ActionBehavior.EDIT);
            DataSelector();
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
			BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
			return PartialView("GridviewMultiPosition", BSM.ListHREmpMultiPosition);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateMultiPosition(HREmpMultiPosition MModel)
        {
            UserSession();
            DataSelector();
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
            if (ModelState.IsValid)
            {
                try
                {
					var existingPosition = DB.HREmpMultiPositions.FirstOrDefault(p => p.PositionCode == MModel.PositionCode && p.EmpCode==MModel.EmpCode);
					if (existingPosition != null)
					{
						ViewData["EditError"] = "Position already exists for that staff";
						BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
						return PartialView("GridviewMultiPosition", BSM.ListHREmpMultiPosition);
					}
					ClsFilterStaff Staff = new ClsFilterStaff();
                    MModel.PositionDescription = Staff.Get_Positon(MModel.PositionCode);
                    DB.HREmpMultiPositions.Add(MModel);
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
			BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
			return PartialView("GridviewMultiPosition", BSM.ListHREmpMultiPosition);
		}
        //edit
        [HttpPost, ValidateInput(false)]
        public ActionResult EditMultiPosition(HREmpMultiPosition MModel)
        {
            UserSession();
            DataSelector();
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
            if (ModelState.IsValid)
            {
                try
                {
					var DBU = new HumicaDBContext();
                    ClsFilterStaff Staff = new ClsFilterStaff();
                    MModel.PositionDescription = Staff.Get_Positon(MModel.PositionCode);
                    DB.HREmpMultiPositions.Attach(MModel);
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
			BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
			return PartialView("GridviewMultiPosition", BSM.ListHREmpMultiPosition);
		}
        //delete
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteMultiPosition(String EmpCode,string PositionCode)
        {
            UserSession();
            DataSelector();
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
            if (EmpCode != null && PositionCode != null)
            {
                try
                {
                    var obj = DB.HREmpMultiPositions.FirstOrDefault(s=>s.EmpCode==EmpCode && s.PositionCode== PositionCode);
                    if (obj != null)
                    {
                        DB.HREmpMultiPositions.Remove(obj);
                        int row = DB.SaveChanges();
                    }
					BSM.ListHREmpMultiPosition = DB.HREmpMultiPositions.ToList();
				}
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
			return PartialView("GridviewMultiPosition", BSM.ListHREmpMultiPosition);
		}
		#endregion
		public ActionResult getPostStructureID(string id, string Action)
		{
			ActionName = Action;
			HREmpMultiPositionObject BSM = new HREmpMultiPositionObject();
		    BSM = (HREmpMultiPositionObject)Session[Index_Sess_Obj + ActionName];
		    var obj = DB.HRPositionStructures.FirstOrDefault(w => w.ID.ToString() == id);
			if (obj != null)
			{
				var result = new
				{
					MS = SYConstant.OK,
					PostStructureID = obj.ReportingID,
					PostID = obj.ID,
				};
				return Json(result, JsonRequestBehavior.DenyGet);
			}
			var rs = new { MS = SYConstant.OK };
			return Json(rs, JsonRequestBehavior.DenyGet);
		}
		private void DataSelector()
        {
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
			ViewData["Staff_SELECT"] = DB.HRStaffProfiles.Where(s=>s.Status== "A").ToList();
			ViewData["PositionStructure_SELECT"] = DB.HRPositionStructures.ToList();
		}
    }
}