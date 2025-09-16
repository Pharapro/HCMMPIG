using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;

namespace Humica
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];
        private static string redirectUri = ConfigurationManager.AppSettings["ida:RedirectUri"];
        private static string authority = aadInstance + tenantId + "/v2.0";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,
                    RedirectUri = redirectUri,
                    ResponseType = OpenIdConnectResponseType.IdToken,
                    //TokenValidationParameters = new TokenValidationParameters { ValidateIssuer = true },

                    // Ensuring RequireNonce is explicitly set to true
                    ProtocolValidator = new OpenIdConnectProtocolValidator
                    {
                        RequireNonce = true
                    },
                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        AuthenticationFailed = context =>
                        {
                            context.HandleResponse();
                            //string base_url = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath;
                            //context.Response.Redirect("/Account/Login");
                            //Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        },
                        SecurityTokenValidated = (context) =>
                        {
                            string name = context.AuthenticationTicket.Identity.FindFirst("preferred_username").Value;
                            //context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, name, string.Empty));
                            Web.Core.Helper.LoginAzureAD.Login(name);
                            context.Response.Redirect("/");
                            //
                            return Task.FromResult(0);
                        }
                    }
                });
                 
        }
        
        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}
