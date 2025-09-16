using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Models.SY;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers
{

    public class LoadingListController : Humica.EF.Controllers.MasterSaleController
    {

        private HumicaDBContext DB = new HumicaDBContext();

        //
        // GET: /LoadingList/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult PartialEmpCodeSearch(string screenid)
        {
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewEmp);
        }
        public ActionResult EmployeeSearch(string screenid)
        {
            //UserSession();
            //UserConfListAndForm();
            //ViewData[SYConstant.SCREEN_ID] = screenid;
            //return PartialView(SYListFilter.ListFilterViewEmployee);
            ViewData["STAFF_SELECT"] = DB.HRStaffProfiles.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewEmployee);
        }
        public ActionResult ComboBoxCustomFilteringPartial()
        {
            var ListStaff = DB.HRStaffProfiles.ToList();
            return PartialView(ListStaff);
        }
        public ActionResult PartialLocationSearch(string screenid)
        {
            ViewData["LOCATION_SELECT"] = DB.HRLocations.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewLocation);
        }
        public ActionResult PartialCompanySearch(string screenid)
        {
            ViewData["COMPANY_SELECT"] = SYConstant.getCompanyDataAccess();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewCompany);
        }
        public ActionResult PartialBUSearch(string screenid)
        {
            ViewData["BUSINESSUNIT_SELECT"] = DB.HRGroupDepartments.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewBU);
        }
        public ActionResult PartialBranchSearch(string screenid)
        {
            ViewData["BRANCHES_SELECT"] = SYConstant.getBranchDataAccess();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewBranch);
        }
        public ActionResult PartialDivisionSearch(string screenid)
        {
            ViewData["DIVISION_SELECT"] = ClsFilter.LoadDivision();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewDivision);
        }
        public ActionResult PartialDepartmentSearch(string screenid)
        {
            ViewData["DEPARTMENT_SELECT"] = ClsFilter.LoadDepartment();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewDepartment);
        }
        public ActionResult PartialSectionSearch(string screenid)
        {
            ViewData["SECTION_SELECT"] = ClsFilter.LoadSection();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewSection);
        }
        public ActionResult PartialPositionSearch(string screenid)
        {
            ViewData["POSITION_SELECT"] = ClsFilter.LoadPosition();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewPosition);
        } 
        public ActionResult PartialLevelSearch(string screenid)
        {
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewLevel);
        }
        public ActionResult PartialLEVELtSearch(string screenid)
        {
            ViewData["LEVEL_SELECT"] = SYConstant.getLevelDataAccess();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewLevel);
        }
        public ActionResult PartialLeaveTypeSearch(string screenid)
        {
            ViewData["LEAVE_TYPE_SELECT"] = DB.HRLeaveTypes.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewLeaveType);
        }
        public ActionResult PartialCareerCodeSearch(string screenid)
        {
            ViewData["CAREERCODE_SELECT"] = DB.HRCareerHistories.ToList();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewCareerCode);
        }

        public ActionResult MaterialComboBoxPartial()
        {
            return PartialView("MaterialComboBoxPartial");
        }
        public ActionResult DownloadDocument(int id)
        {
            UserSession();
            var obj = DB.MDUploadTemplates.Find(id);
            if (obj != null)
            {
                MemoryStream memoryStream = new MemoryStream(System.IO.File.ReadAllBytes(obj.UpoadPath));
                TextWriter textWriter = new StreamWriter(memoryStream);
                textWriter.Flush(); // added this line
                byte[] bytesInStream = memoryStream.ToArray(); // simpler way of converting to array
                memoryStream.Close();
                Response.Clear();
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + obj.UploadName);
                Response.BinaryWrite(bytesInStream);
                Response.End();

                //string fileName = Server.MapPath("~/Content/TEMPLATE/QUOTA/ADDITIONAL_TEMPLATE_MATERIAL.xlsx");
                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/force-download";
                //Response.AddHeader("content-disposition", "attachment;filename=ADDITIONAL_TEMPLATE_MATERIAL.xlsx");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.WriteFile(fileName);
                //Response.End();             
            }
            return null;
        }
        public ActionResult ViewDocument(int id)
        {
            UserSession();
            UserConfList(ActionBehavior.LISTR, "DocumentNo", "LoadingList");
            var obj = DB.MDUploadTemplates.Find(id);
            ViewData[SYConstant.PARAM_ID] = id;
            if (obj != null)
            {
                List<MDUploadBatchNo> ListBatch = DB.MDUploadBatchNoes.Where(w => w.BathUploadNo == obj.DocumentNo).ToList();
                return View(ListBatch);
            }
            return null;
        }

        public ActionResult PartialList(int id)
        {
            UserSession();
            UserConfListAndForm();
            var obj = DB.MDUploadTemplates.Find(id);
            ViewData[SYConstant.PARAM_ID] = id;
            List<MDUploadBatchNo> ListBatch = DB.MDUploadBatchNoes.Where(w => w.BathUploadNo == obj.DocumentNo).ToList();
            return PartialView("PartialList", ListBatch);

        }

        public string deleteFileUploaded(int id)
        {
            try
            {
                UserSession();
                UserConfListAndForm();
                var obj = DB.MDUploadTemplates.Find(id);
                if (obj != null)
                {
                    DB.MDUploadTemplates.Remove(obj);
                    DB.SaveChanges();

                    //if (System.IO.File.Exists(obj.UpoadPath))
                    //{
                    //    System.IO.File.Delete(obj.UpoadPath);
                    //}

                    return SYConstant.OK;
                }
                return SYConstant.FAIL;
            }
            catch
            {
                return SYConstant.FAIL;
            }

        }

    }
}
