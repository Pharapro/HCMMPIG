using DevExpress.Web.Mvc;
using DevExpress.XtraReports.Security;
using Hangfire;
using Hangfire.SqlServer;
using Humica.App_Start;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.License.Model;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Humica
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            string con = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            Hangfire.GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(con, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }
        protected void Application_Start()
        {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            MVCxReportDesigner.StaticInitialize();

            Humica.DashboardConfig.RegisterService(RouteTable.Routes);
            AreaRegistration.RegisterAllAreas();

            //System.Web.Http.GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Hangfire // --------------------------------------------------------------
            HangfireAspNet.Use(GetHangfireServers);

            //new BackgroundService().Init_Service();
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();
            new BackgroundService().Init_Service(scheduler);
            // -------------------------------------------------------------------------
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
            DevExpress.Data.Helpers.ServerModeCore.DefaultForceCaseInsensitiveForAnySource = true;
            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;


            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                string base_url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
                if (Session[SYConstant.USER_LOG_INFORMATION] == null)
                {

                    Response.Redirect(base_url + "/Account/Login", true);
                }
                else
                {
                    bool checkLog = true;
                    if (checkLog)
                    {
                        if (Request.QueryString[SYConstant.SCREEN_ID] != null)
                        {
                            string screenId = Request.QueryString[SYConstant.SCREEN_ID];
                            SMSystemEntity db = new SMSystemEntity();
                            List<SYMenuItem> mi = db.SYMenuItems.Where(w => w.ScreenId == screenId).ToList();
                            if (mi == null || mi.Count == 0)
                            {
                                Response.Redirect(base_url + "/Home/PageNotAuthorize", true);
                            }
                        }
                        else
                        {
                            Response.Redirect(base_url + "/Home/PageNotAuthorize", true);
                        }

                    }
                }

            }

            DevExpress.XtraReports.Web.WebDocumentViewer.Native.WebDocumentViewerBootstrapper.SessionState =
      System.Web.SessionState.SessionStateBehavior.Required;

            //DevExpress.XtraReports.Web.QueryBuilder.Native.QueryBuilderBootstrapper.SessionState =
            //    System.Web.SessionState.SessionStateBehavior.Required;

            DevExpress.XtraReports.Web.ReportDesigner.Native.ReportDesignerBootstrapper.SessionState =
                System.Web.SessionState.SessionStateBehavior.Required;

            //Register Report Storage Extension
            //DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension.RegisterExtensionGlobal(new CustomReportStorageWebExtension());
            // add line for un trust script running for report designer 
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);

            Application["UsersLoggedIn"] = new System.Collections.Generic.List<UserSessionLog>();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                try
                {
                    string base_url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
                    if (Session[SYConstant.USER_LOG_INFORMATION] == null)
                    {
                        if (!Response.IsRequestBeingRedirected)
                        {
                            Response.Redirect(base_url + "/Account/Login", true);
                        }
                    }
                    else
                    {

                        if (Request.CurrentExecutionFilePath.Contains("default.aspx"))
                        {
                            Response.Redirect(base_url,true);
                        }

                    }
                }
                catch
                {

                }
            }
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (Session[SYConstant.USER_LOG_INFORMATION] != null)
            {
                string sessionId = Session.SessionID;
                SYUser obj = (SYUser)Session[SYConstant.USER_LOG_INFORMATION];
                System.Collections.Generic.List<UserSessionLog> d = Application["UsersLoggedIn"]
                   as System.Collections.Generic.List<UserSessionLog>;
                if (d != null)
                {
                    lock (d)
                    {
                        Session.Clear();
                        Session.RemoveAll();
                        d.Remove(d.FirstOrDefault(s => s.SessionId == sessionId));
                        Response.Redirect("~/Account/Login", true);
                    }
                }
            }

        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            if (context.Session == null)
            {
                //Response.Redirect("~/Account/Login", true);
            }
        }

    }
}