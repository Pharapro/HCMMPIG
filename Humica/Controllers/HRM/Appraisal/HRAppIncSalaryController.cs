using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Models.SY;
using Humica.Performance;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.HRM.Appraisal
{
    public class HRAppIncSalaryController : Humica.EF.Controllers.MasterSaleController
    {
        private const string SCREEN_ID = "HRA0000017";
        private const string URL_SCREEN = "/HRM/Appraisal/HRAppIncSalary/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        HumicaDBContext DB = new HumicaDBContext();
        private string KeyName = "EmpCode";
        public HRAppIncSalaryController()
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
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            BSM.Filter = new Core.FT.FTFilterEmployee();
            BSM.ListIncSalary = await DB.HRAPPIncSalaries.ToListAsync();
            BSM.ListPendindIncrease = await BSM.LoadDataIncrease(BSM.Filter, SYConstant.getBranchDataAccess());
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult PartialList()
        {
            ActionName = "Index";
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            UserSession();
            UserConfListAndForm(this.KeyName);
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView(SYListConfuration.ListDefaultView, BSM.ListIncSalary);
        }
        public ActionResult PartialListPending()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("PartialListPending", BSM.ListPendindIncrease);
        }
        #endregion
        #region Create MultiRef
        public async Task<ActionResult> CreateMultiRef(string id)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ViewData[SYSConstant.PARAM_ID1] = false;
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            await BSM.GetDataIncrease(id);
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        [HttpPost]
        public async Task<ActionResult> CreateMultiRef(ClsAPPIncreaseSalary collection)
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            await DataSelector();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            BSM.ScreenId = SCREEN_ID;
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = Session[Index_Sess_Obj + ActionName] as ClsAPPIncreaseSalary;
                BSM.HeaderIncSalary = collection.HeaderIncSalary;
            }
            string msg = await BSM.CreateIncSalary();
            if (msg == SYConstant.OK)
            {
                SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                mess.DocumentNumber = BSM.HeaderIncSalary.ID.ToString();
                mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details/" + mess.DocumentNumber;

                Session[Index_Sess_Obj + ActionName] = BSM;
                Session[SYConstant.MESSAGE_SUBMIT] = mess;
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            ViewData[SYConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
            return View(BSM);
        }
        public ActionResult GridItems()
        {
            ActionName = "Create";
            UserSession();
            UserConfListAndForm(this.KeyName);
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (ClsAPPIncreaseSalary)Session[Index_Sess_Obj + ActionName];
            }
            return PartialView("GridItems", BSM.ListIncSalaryItem);
        }
        #endregion
        #region Details
        public async Task<ActionResult> Details(int id)
        {
            ActionName = "Create";
            UserSession();
            UserConfForm(SYActionBehavior.VIEW);
            ViewData[SYConstant.PARAM_ID] = id;
            await DataSelector();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();

            BSM.HeaderIncSalary = await DB.HRAPPIncSalaries.FirstOrDefaultAsync(w => w.ID == id);
            if (BSM.HeaderIncSalary != null)
            {
                BSM.ListIncSalaryItem = await DB.HRAPPIncSalaryItems.Where(x => x.ID == id).ToListAsync();
                Session[Index_Sess_Obj + ActionName] = BSM;
                return View(BSM);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_INV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        #endregion

        public async Task<ActionResult> RequestForApproval(int id)
        {
            UserSession();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg =await BSM.requestToApprove(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        public async Task<ActionResult> Approve(int id)
        {
            UserSession();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (id != null)
            {

                BSM.ScreenId = SCREEN_ID;
                string msg =await BSM.approveTheDoc(id);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_APPROVED", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }
        public async Task<ActionResult> ReleaseDoc(int id,DateTime EffectiveDate,string Career)
        {
            UserSession();
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (id != null)
            {
                BSM.ScreenId = SCREEN_ID;
                string msg = await BSM.ReleaseTheDoc(id, EffectiveDate, Career);
                if (msg == SYConstant.OK)
                {
                    var mess = SYMessages.getMessageObject("DOC_RQ", user.Lang);
                    mess.DocumentNumber = id.ToString();
                    mess.Description = mess.Description + BSM.MessageError;
                    mess.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    Session[Index_Sess_Obj + ActionName] = null;
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    var mess = SYMessages.getMessageObject(msg, user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("DOC_EV", user.Lang);
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
        }

        #region "Cancel"
        public async Task<ActionResult> Cancel(int id)
        {
            this.UserSession();
            ViewData[SYSConstant.PARAM_ID] = id;
            ClsAPPIncreaseSalary BSM = new ClsAPPIncreaseSalary();
            if (id.ToString() != "null")
            {
                BSM.ScreenId = SCREEN_ID;
                string msg =await BSM.CancelTheDoc(id);

                if (msg == SYConstant.OK)
                {
                    SYMessages messageObject = SYMessages.getMessageObject(msg, user.Lang);
                    messageObject.DocumentNumber = id.ToString();
                    messageObject.NavigationUrl = SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id;
                    SYMessages mess = SYMessages.getMessageObject("DOC_CANCEL", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                }
                else
                {
                    Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject(msg, user.Lang);
                }
                return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);
            }
            else
            {
                Session[SYSConstant.MESSAGE_SUBMIT] = SYMessages.getMessageObject("INV_DOC", user.Lang);
            }
            return Redirect(SYUrl.getBaseUrl() + URL_SCREEN + "Details?id=" + id);

        }
        #endregion

        private async Task DataList()
        {
            //SYDataList DL = new SYDataList("STATUS_APPROVAL");
            //ViewData["STATUS_APPROVAL"] = DL.ListData;
            //ViewData["LIST_KPICATEGORY"] =await DB.HRKPITypes.Where(w => w.IsActive == true).ToListAsync();
            ViewData["CAREER_TYPE"] = await DB.HRCareerHistories.ToListAsync();
        }
        private async Task DataSelector()
        {
            var _listBranch = SYConstant.getBranchDataAccess();
            var ListEmp =await DB.HRStaffProfiles.ToListAsync();
            ViewData["STAFF_VIEW"] = ListEmp.Where(w => _listBranch.Where(x => x.Code == w.Branch).Any()).ToList();
            ViewData["CAREER_TYPE"] =await DB.HRCareerHistories.ToListAsync();
            //ViewData["LIST_KPIGROUP"] = DB.HRKPITypes.Where(w => w.IsActive == true).ToList();
            //ViewData["LIST_DEPARTMENT"] = DB.HRDepartments.ToList();
            //ViewData["LIST_KPICATEGORY"] = DB.HRKPICategories.Where(w => w.IsActive == true).ToList();
        }
    }
}