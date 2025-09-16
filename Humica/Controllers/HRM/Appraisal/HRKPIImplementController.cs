using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic;
using Humica.Performance;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRKPIImplementController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000016";
        private const string URL_SCREEN = "/HRM/Appraisal/HRKPIImplement/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "AVTCode";
        // private string DOCTYPE = "ASS01";
        //private string DOCTYPE_AVT = "ACT01";
        //private string PATH_FILE = "12313123123sadfsdfsdfs12f";
        //private string PATH_FILE1 = "12313123123sadfsdfsdfs12fxde";
        //private string PATH_FILE2 = "12313123123sadfsdfsdfs12fxdf";
        //private string PATH_FILE3 = "12313123123sadfsdfsdfs12fxdg";
        HumicaDBContext DB = new HumicaDBContext();

        public HRKPIImplementController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;

        }
        #region List
        public async Task<ActionResult> Index()
        {
            UserSession();
            ActionName = "Index";
            UserConfListAndForm(this.KeyName);
            await DataList();
            CLsKPIAssign BSM = new CLsKPIAssign();
            string Open = SYDocumentStatus.OPEN.ToString();
            string COMPLETED = SYDocumentStatus.COMPLETED.ToString();
            //string PENDING = SYDocumentStatus.PENDING.ToString();
            string approval = SYDocumentStatus.APPROVED.ToString();
            BSM.ListTask = await BSM.LoadDataTask();
            BSM.ListTaskCompleted = BSM.ListTask.Where(w => w.Status == COMPLETED).ToList();
            BSM.ListTaskPending = BSM.ListTask.Where(w => w.Status != COMPLETED).ToList();
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialListCompeled()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListCompeled", BSM.ListTaskCompleted);
        }
        public ActionResult PartialListProcess()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListProcess", BSM.ListTaskPending);
        }
        #endregion
        #region Details
        public async Task<ActionResult> Details(string id, string KPI)
        {
            ActionName = "Details";
            UserSession();
            DataSelector();
            UserConfForm(SYActionBehavior.EDIT);
            ViewData[SYSConstant.PARAM_ID] = id;
            ViewData[SYSConstant.PARAM_ID1] = KPI;
            if (id != null)
            {
                CLsKPIAssign BSM = new CLsKPIAssign();
                BSM.AssignItem = await DB.HRKPIAssignItems.FirstOrDefaultAsync(w => w.AssignCode == id && w.ItemCode == KPI);
                if (BSM.AssignItem != null)
                {
                    BSM.AssignHeader = await DB.HRKPIAssignHeaders.FirstOrDefaultAsync(w => w.AssignCode == id);
                    string APPROVED = SYDocumentStatus.APPROVED.ToString();
                    BSM.ListKPITracking = await DB.HRKPITrackings.Where(w => w.AssignCode == id && 
                    w.KPI == BSM.AssignItem.ItemCode && w.Status==APPROVED).ToListAsync();
                    BSM.ListtaskSummary = await BSM.LoadDataTaskSummary(id, KPI, BSM.AssignItem.Target.Value);
                    Session[Index_Sess_Obj + ActionName] = BSM;
                    return View(BSM);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public async Task<ActionResult> Completed(string id, string KPI)
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm(this.KeyName);
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            string resutl = await BSM.CompeledDoc(id, KPI);
            if (resutl == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("DOC_REQ", user.Lang);
                mess.DocumentNumber = id;
                ViewData[SYConstant.MESSAGE_SUBMIT] = mess;
                Session[SYSConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }

            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public ActionResult GridTaskSummary()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridTaskSummary", BSM.ListtaskSummary);
        }
        #endregion
        public ActionResult GridItemsImplement()
        {
            ActionName = "Details";
            UserSession();
            UserConfListAndForm();
            DataSelector();
            CLsKPIAssign BSM = new CLsKPIAssign();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (CLsKPIAssign)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return PartialView("GridItemsImplement", BSM.ListKPITracking);
        }
        private async Task DataList()
        {
            SYDataList DL = new SYDataList("STATUS_APPROVAL");
            ViewData["STATUS_APPROVAL"] = DL.ListData;
            ViewData["LIST_KPICATEGORY"] =await DB.HRKPITypes.Where(w => w.IsActive == true).ToListAsync();
        }
        private void DataSelector()
        {
            var _listBranch = SYConstant.getBranchDataAccess();
            var ListEmp = DB.HRStaffProfiles.ToList();
            ViewData["LIST_STAFF"] = ListEmp.Where(w => _listBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            ViewData["ITEM_TYPE"] = DB.HRMaterials.ToList();
            ViewData["LIST_KPIGROUP"] = DB.HRKPITypes.Where(w => w.IsActive == true).ToList();
            ViewData["LIST_DEPARTMENT"] = ClsFilter.LoadDepartment();
            ViewData["LIST_KPICATEGORY"] = DB.HRKPICategories.Where(w => w.IsActive == true).ToList();
        }
    }
}