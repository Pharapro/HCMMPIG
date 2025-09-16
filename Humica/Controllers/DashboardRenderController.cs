using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Sql;
using Humica.EF.Controllers;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using Humica.Models.SY;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers
{
    public class DashboardRenderController : MasterDashboardController//DashboardController
    {
        public DashboardRenderController()
            : base()
        {
        }
        public ActionResult Design(string user)
        {
            UserSession();
            var pathApp = "~/App_Data/Dashboards/" + this.user.UserName + "/";
            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(pathApp);
            string path = Server.MapPath("~") + "/App_Data/Dashboards/" + this.user.UserName + "/";
            if (!Directory.Exists(path))
            {
                pathApp = "~/App_Data/Dashboards/Normal/";
                dashboardFileStorage = new DashboardFileStorage(pathApp);
            }
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);


            var myConnectionName = "DashboardConnectionString";

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            // Registers an SQL data source.
            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("Humica Data Source", myConnectionName);

            SelectQuery query = SelectQueryFluentBuilder
                .AddTable("HR_STAFF_VIEW")
                .SelectAllColumns()
                .Build("Staff Profile");
            sqlDataSource.Queries.Add(query);
            //---------------------

            dataSourceStorage.RegisterDataSource("CUSCENData", sqlDataSource.SaveToXml());

            //DashboardObjectDataSource objDataSourceStaff = new DashboardObjectDataSource("NewStaff");
            //dataSourceStorage.RegisterDataSource("objNewStaff", objDataSourceStaff.SaveToXml());
            DashboardObjectDataSource objDataLeave = new DashboardObjectDataSource("Leave");
            dataSourceStorage.RegisterDataSource("objLeave", objDataLeave.SaveToXml());
            DashboardObjectDataSource objDataNotificat = new DashboardObjectDataSource("Notificat");
            dataSourceStorage.RegisterDataSource("objNotificat", objDataNotificat.SaveToXml());

            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
            DashboardConfigurator.Default.ConfigureDataReloadingTimeout += ASPxDashboard1_ConfigureDataReloadingTimeout;
            DashboardConfigurator.Default.DataLoading += DataLoading;
            DashboardConfigurator.Default.CustomParameters += CustomParameters;
            return View();
        }

        public ActionResult Designer(string user)
        {
            UserSession();
            var pathApp = "~/App_Data/Dashboards/" + this.user.UserName + "/";
            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(pathApp);
            string path = Server.MapPath("~") + "/App_Data/Dashboards/" + this.user.UserName + "/";
            if (!Directory.Exists(path))
            {
                pathApp = "~/App_Data/Dashboards/Normal/";
                dashboardFileStorage = new DashboardFileStorage(pathApp);
            }
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);

            // Uncomment this string to allow end users to create new data sources based on predefined connection strings.
            //DashboardConfigurator.Default.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());

            var myConnectionName = "DashboardConnectionString";

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            //var DBR = new DMSPRO.Models.MD.SMMasterData();
            // Registers an SQL data source.
            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("Humica Data Source", "DashboardConnectionString");
            //SelectQuery query = new SelectQuery();
            //foreach (var read in source)
            //{
            //    query = SelectQueryFluentBuilder
            //    .AddTable(read.DataSource)
            //    .SelectAllColumns()
            //    .Build(read.DataName);
            //    sqlDataSource.Queries.Add(query);
            //}
            SelectQuery query = SelectQueryFluentBuilder
                .AddTable("HR_STAFF_VIEW")
                .SelectAllColumns()
                .Build("Staff Profile");
            sqlDataSource.Queries.Add(query);
            //---------------------

            dataSourceStorage.RegisterDataSource("CUSCENData", sqlDataSource.SaveToXml());

            //DashboardObjectDataSource objDataSourceStaff = new DashboardObjectDataSource("NewStaff");
            //dataSourceStorage.RegisterDataSource("objNewStaff", objDataSourceStaff.SaveToXml());
            DashboardObjectDataSource objDataLeave = new DashboardObjectDataSource("Leave");
            dataSourceStorage.RegisterDataSource("objLeave", objDataLeave.SaveToXml());
            DashboardObjectDataSource objDataNotificat = new DashboardObjectDataSource("Notificat");
            dataSourceStorage.RegisterDataSource("objNotificat", objDataNotificat.SaveToXml());

            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
            DashboardConfigurator.Default.ConfigureDataReloadingTimeout += ASPxDashboard1_ConfigureDataReloadingTimeout;
            DashboardConfigurator.Default.DataLoading += DataLoading;
            DashboardConfigurator.Default.CustomParameters += CustomParameters;
            return View();
        }
        private static void ASPxDashboard1_ConfigureDataReloadingTimeout(object sender, ConfigureDataReloadingTimeoutWebEventArgs e)
        {
            e.DataReloadingTimeout = TimeSpan.FromSeconds(5);
        }
        private static void DataLoading(object sender, DataLoadingWebEventArgs e)
        {
            var listBranch = SYConstant.getBranchDataAccess();
            //if (e.DataSourceName == "NewStaff")
            //{
            //    e.Data = MDDashboard.NewStaff(listBranch);
            //}
            //MDDashboard mDDashboard = new MDDashboard();
            //if (e.DataSourceName == "Leave")
            //{
            //    string URL = @Humica.EF.Models.SY.SYUrl.getBaseUrl() + "/MyTeamLeaveRequest/Details/";
            //    e.Data = mDDashboard.PandingLeave(URL);
            //}
            //if (e.DataSourceName == "Notificat")
            //{
            //    e.Data = mDDashboard.getAlertNotifications();
            //}
        }

        public ActionResult Render(string token)
        {
            UserSession();
            var userType = Humica.EF.MD.SYSession.getSessionUser();
            var pathApp = "~/App_Data/Dashboards/" + this.user.UserName + "/";
            //if (userType.IsRemember == true)
            //{
            var path = "~/App_Data/Dashboards/public/";
            //        if (!string.IsNullOrEmpty(userType.JDType))
            //        {
            //            path = "~/App_Data/Dashboards/" + userType.JDType + "/";
            //        }
            ClsConstant clsConstant = new ClsConstant();
            clsConstant.MoveFilesDashboard(path, pathApp);
            //}

            DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(pathApp);
            DashboardConfigurator.Default.SetDashboardStorage(dashboardFileStorage);

            // Uncomment this string to allow end users to create new data sources based on predefined connection strings.
            //DashboardConfigurator.Default.SetConnectionStringsProvider(new DevExpress.DataAccess.Web.ConfigFileConnectionStringsProvider());

            var myConnectionName = "DashboardConnectionString";

            DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
            //    var DBR = new DMSPRO.Models.MD.SMMasterData();
            // Registers an SQL data source.
            DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("Humica Data Source", myConnectionName);

            //    var source = DB.CfDashboardDataSources.Where(w => w.IsCustom == true).ToList();
            //    SelectQuery query = new SelectQuery();

            //    foreach (var read in source)
            //    {
            //        query = SelectQueryFluentBuilder
            //           .AddTable(read.DataSource)
            //           .SelectAllColumns()
            //           .Build(read.DataName);
            //        sqlDataSource.Queries.Add(query);
            //    }
            //    //---------------------

            dataSourceStorage.RegisterDataSource("CUSCENData", sqlDataSource.SaveToXml());


            DashboardConfigurator.Default.SetDataSourceStorage(dataSourceStorage);
            DashboardConfigurator.Default.DataLoading += DataLoading;
            DashboardConfigurator.Default.CustomParameters += CustomParameters;

            return View();
        }

        private void CustomParameters(object sender, CustomParametersWebEventArgs e)
        {
            //var companycode = e.Parameters.FirstOrDefault(w => w.Name == "CompanyCode");
            var username = e.Parameters.FirstOrDefault(w => w.Name == "UserName");
            //var departmentcode = e.Parameters.FirstOrDefault(w => w.Name == "DepartmentCode");
            var branchcode = e.Parameters.FirstOrDefault(w => w.Name == "BranchCode");
            var LevelCode = e.Parameters.FirstOrDefault(w => w.Name == "LevelCode");
            var InDate = e.Parameters.FirstOrDefault(w => w.Name == "InDate");
            if (username != null)
            {
                username.Value = base.user.UserName;
            }
            if (branchcode != null)
            {
                var LsBranch = SYConstant.getBranchDataAccess();

                branchcode.Value = (from Branch in LsBranch
                                    group Branch by Branch.Code into grouped
                                    select grouped.Key).ToList();
            }
            if (LevelCode != null)
            {
                var LsLevel = SYConstant.getLevelDataAccess();

                LevelCode.Value = (from Branch in LsLevel
                                   group Branch by Branch.Code into grouped
                                   select grouped.Key).ToList();
            }
            if (InDate != null)
            {
                InDate.Value = DateTime.Now.Date;
            }
        }
    }
}