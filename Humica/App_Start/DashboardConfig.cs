using DevExpress.DashboardWeb;
using DevExpress.DashboardWeb.Mvc;
using System.Web.Routing;

namespace Humica
{
    public static class DashboardConfig
    {
        public static void RegisterService(RouteCollection routes)
        {
            routes.MapDashboardRoute("dashboardControl", "DashboardRender");
        }

        private static void DataLoading(object sender, DataLoadingWebEventArgs e)
        {
            if (e.DataSourceName == "DataSource")
            {

            }
        }
    }
}