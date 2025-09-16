using System.Web.Mvc;
using System.Web.Routing;

namespace Humica
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            // routes.MapMvcAttributeRoutes();


            routes.MapRoute(
                name: "Administrator12", // Route name
                url: "Administrator/Users/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Administrator.Users" }
                );


            routes.MapRoute(
                name: "Administrator11", // Route name
                url: "Administrator/Systems/{controller}/{action}/{id}", // URL with parameters..................
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Administrator.Systems" }
                );
            routes.MapRoute(
                name: "OnSite", // Route name
                url: "SelfService/OnSite/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.SelfService.OnSite" }
            );
            routes.MapRoute(
                name: "Administrator10", // Route name
                url: "Administrator/Developments/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Administrator.Developments" }
                );

            routes.MapRoute(
              name: "HLOP2", // Route name
              url: "House/Setting/{controller}/{action}/{id}", // URL with parameters
              defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
              namespaces: new[] { "Humica.Controllers.House.Setting" }
              );

            routes.MapRoute(
                 name: "Humica", // Route name
                 url: "Config/Setup/{controller}/{action}/{id}", // URL with parameters
                 defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                 namespaces: new[] { "Humica.Controllers.Config.Setup" }
             );
            routes.MapRoute(
                name: "PR", // Route name
                url: "PR/PRS/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.PR.PRS" }
            );
            routes.MapRoute(
               name: "PR1", // Route name
               url: "PR/PRM/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.PR.PRM" }
            );
            routes.MapRoute(
                name: "Asset", // Route name
                url: "Asset/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Asset" }
            );
            routes.MapRoute(
                name: "HRE", // Route name
                url: "HRM/EmployeeInfo/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.HumanResource.EmployeeInfo" }
            );
            routes.MapRoute(
               name: "LM", // Route name
               url: "HRM/LMS/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.LMS" }
            );
            routes.MapRoute(
               name: "RM1", // Route name
               url: "HRM/HRS/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.HRS" }
            );
            routes.MapRoute(
               name: "Leave", // Route name
               url: "HRM/Leave/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.Leave" }
            );
            routes.MapRoute(
               name: "Mission", // Route name
               url: "HRM/Mission/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.Mission" }
            );
            routes.MapRoute(
               name: "Appraisal", // Route name
               url: "HRM/Appraisal/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.Appraisal" }
            );
            routes.MapRoute(
              name: "Attendance", // Route name
              url: "Attendance/AttSetup/{controller}/{action}/{id}", // URL with parameters
              defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
              namespaces: new[] { "Humica.Controllers.Attendance.AttSetup" }
            );
            routes.MapRoute(
                name: "Attendance1", // Route name
                url: "Attendance/Attendance/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Attendance.Attendance" }
            );
            routes.MapRoute(
                name: "TRProcess", // Route name
                url: "Training/Process/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Training" }
             );
            routes.MapRoute(
               name: "TRSetup", // Route name
               url: "Training/Setup/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.Training.Setup" }
            );
            routes.MapRoute(
                name: "RCM", // Route name
                url: "HRM/RCM/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.HRM.RCM" }
             );
            routes.MapRoute(
                name: "SelfService", // Route name
                url: "SelfService/LeaveBalance/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.SelfService.LeaveBalance" }
            );
            routes.MapRoute(
                name: "MyTeam", // Route name
                url: "SelfService/MyTeam/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.SelfService.MyTeam" }
             );
            routes.MapRoute(
                name: "Evaluation", // Route name
                url: "SelfService/Performance/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.SelfService.Performance" }
             );
            routes.MapRoute(
                name: "EOB", // Route name
                url: "EOB/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.EOB" }
             );
            routes.MapRoute(
                name: "Approval", // Route name
                url: "Approval/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Approval" }
             );
            routes.MapRoute(
                name: "Reporting", // Route name
                url: "Reporting/{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                namespaces: new[] { "Humica.Controllers.Reporting" }
            );
            routes.MapRoute(
               name: "POD", // Route name
               url: "POD/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.POD" }
            );
            routes.MapRoute(
               name: "Dress", // Route name
               url: "HRM/Dress/{controller}/{action}/{id}", // URL with parameters
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
               namespaces: new[] { "Humica.Controllers.HRM.Dress" }
            );
            routes.MapRoute(
              name: "Inquiry", // Route name
              url: "Inquiry/{controller}/{action}/{id}", // URL with parameters
              defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
              namespaces: new[] { "Humica.Controllers.Inquiry" }
           );
            routes.MapRoute(
             name: "Services", // Route name
             url: "Services/{controller}/{action}/{id}", // URL with parameters
             defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
             namespaces: new[] { "Humica.Controllers.Services" }
          );
            routes.MapRoute(
            name: "EForm", // Route name
            url: "HRM/EForm/{controller}/{action}/{id}", // URL with parameters
            defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
            namespaces: new[] { "Humica.Controllers.HRM.EForm" }
         );
            routes.MapRoute(
                name: "Default", // Route name
                url: "{controller}/{action}/{id}", // URL with parameters
                defaults: new { controller = "Home", action = "InitFist", id = UrlParameter.Optional },
                namespaces: new[] { "Humica.Controllers" }
            );
        }
    }
}