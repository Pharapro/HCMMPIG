using DevExpress.Web;
using DevExpress.Web.Mvc;
using Humica.Core;
using Humica.Core.DB;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using Humica.Logic;
using Humica.Models.SY;
using Humica.Performance;
using System.Web.Mvc;

namespace Humica.Controllers
{

    public class LoadingListFilterController : LoadingListController
    {

        private IUnitOfWork unitOfWork;
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>(new HumicaDBContext());
        }
        public LoadingListFilterController()
            : base()
        {
            OnLoad();
        }
        public ActionResult KPIList(string Department)
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "LoadingListFilter", Action = "KPIList" };
                cboProperties.ClientInstanceName = "SelectKPI_LIST";
                cboProperties.TextField = "Description";
                cboProperties.ValueField = "Code";

                cboProperties.TextFormatString = ClsConstant.TEXT_COMBOBOX_STRING;
                cboProperties.DisplayFormatString = ClsConstant.TEXT_COMBOBOX_STRING_SINGLE;
                cboProperties.Columns.Add("Code", Humica.EF.Models.SY.SYSettings.getLabel("Code"), 90);
                cboProperties.Columns.Add("Description", Humica.EF.Models.SY.SYSettings.getLabel("Description"), 300);
                cboProperties.ClientSideEvents.SelectedIndexChanged = "cboItemChange";

                cboProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;

                cboProperties.BindList(CLsKPIAssign.GetKPIList());
            });

        }
        public ActionResult Employee()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "LoadingListFilter", Action = "Employee" };
                cboProperties.ClientInstanceName = "cboEmployee";
                cboProperties.TextField = "AllName";
                cboProperties.ValueField = "EmpCode";

                cboProperties.TextFormatString = ClsConstant.TEXT_COMBOBOX_STRING;
                cboProperties.DisplayFormatString = ClsConstant.TEXT_COMBOBOX_FILTER;
                cboProperties.Columns.Add("EmpCode", Humica.EF.Models.SY.SYSettings.getLabel("EmpCode"), 90);
                cboProperties.Columns.Add("AllName", Humica.EF.Models.SY.SYSettings.getLabel("EmployeeName"), 300);
                cboProperties.Columns.Add("OthAllName", Humica.EF.Models.SY.SYSettings.getLabel("EmployeeName 2"), 300);
                cboProperties.ClientSideEvents.SelectedIndexChanged = "cboEmployeeChange";

                cboProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                ClsFilterStaff clsFilterStaff = new ClsFilterStaff();
                cboProperties.BindList(clsFilterStaff.OnLoadStaff());

            });
        }
        public ActionResult KPIList_BU()
        {
            UserSession();
            return GridViewExtension.GetComboBoxCallbackResult(cboProperties =>
            {
                cboProperties.CallbackRouteValues = new { Controller = "LoadingListFilter", Action = "KPIList_BU" };
                cboProperties.ClientInstanceName = "SelectKPI_LIST";
                cboProperties.TextField = "Description";
                cboProperties.ValueField = "Code";

                cboProperties.TextFormatString = ClsConstant.TEXT_COMBOBOX_STRING;
                cboProperties.DisplayFormatString = ClsConstant.TEXT_COMBOBOX_STRING;
                cboProperties.Columns.Add("Code", Humica.EF.Models.SY.SYSettings.getLabel("Code"), 90);
                cboProperties.Columns.Add("Description", Humica.EF.Models.SY.SYSettings.getLabel("Description"), 300);
                cboProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                ClsKPIConfig clsKPIConfig = new ClsKPIConfig();
                cboProperties.BindList(clsKPIConfig.GetKPIList_BU());
            });
        }
        public ActionResult PartialEmployeeByHODSearch(string screenid)
        {
            UserSession();
            UserConfListAndForm();
            ClsFilter clsFilter = new ClsFilter();
            ViewData["STAFF_SELECT_HOD"] = clsFilter.OnLoadStaffByHOD();
            ViewData[SYConstant.SCREEN_ID] = screenid;
            return PartialView(SYListFilter.ListFilterViewEmpByHOD);
        }
    }
}
