using Humica.EF.MD;
using Humica.EF.Models.SY;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers
{
    public class HomeController : Humica.EF.Controllers.HomeController
    {
        public HomeController()
            : base()
        {

        }

        public ActionResult loadingIframe(string id)
        {
            //Validate();

            if (Session[SYConstant.USER_LOG_INFORMATION] == null)
            {
                string base_url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;

                var rs = new { MS = "R", URL = base_url + "/Account/Login", SCREEN = "Login", TITLE = "Login" };
                return Json(rs, JsonRequestBehavior.DenyGet);
                //return Redirect();
            }
            var DB = new SMSystemEntity();

            var listMn = DB.SYMenuItems.Where(w => w.ScreenId == id).ToList();
            if (listMn.Count > 0)
            {
                var objMn = listMn.First();
                var url = SYUrl.getBaseUrl() + objMn.NavigateUrl.Replace("~", "");
                var rs = new { MS = SYConstant.OK, URL = url, SCREEN = objMn.ScreenId, TITLE = objMn.Text };
                Session[SYConstant.SCREEN_REF] = objMn.ScreenId;
                return Json(rs, JsonRequestBehavior.DenyGet);
            }

            return Json(new { MS = Humica.EF.Models.SY.SYConstant.FAIL }, JsonRequestBehavior.DenyGet);
        }

        public ActionResult loadRight(string id)
        {
            var user = SYSession.getSessionUser();
            if (user != null)
            {
                return PartialView("_RightPanelPartial");
            }
            return null;
        }

        public ActionResult changeColor(string code, string StandingUrl)
        {
            var user = SYSession.getSessionUser();
            if (user != null)
            {
                var ownfile = "~/Content/user/custom/" + user.UserName + ".css";
                var defaultFile = "~/Content/user/color.css";
                var css = Server.MapPath(ownfile);
                if (System.IO.File.Exists(css))
                {
                    System.IO.File.Delete(css);
                }
                System.IO.File.Copy(Server.MapPath(defaultFile), css, true);

                if (System.IO.File.Exists(css))
                {
                    string createText = System.IO.File.ReadAllText(css);

                    System.IO.File.WriteAllText(css, createText.Replace(SYSettings.getSetting("DEFAULT_COLOR").SettinValue, "#" + code), System.Text.Encoding.UTF8);

                }
                return Redirect(SYUrl.getBaseUrl());
            }
            return null;
        }
    }
}