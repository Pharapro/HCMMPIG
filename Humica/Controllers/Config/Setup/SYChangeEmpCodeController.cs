using Humica.Core.DB;
using Humica.EF;
using Humica.EF.Models.SY;
using Humica.Logic.MD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class SYChangeEmpCodeController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "SYHR000001";
        private const string URL_SCREEN = "/Config/Setup/SYChangeEmpCode/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "EmpCodeFrom";
        HumicaDBContext DB = new HumicaDBContext();
        public SYChangeEmpCodeController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }
        public ActionResult Index()
        {
            ActionName = "Index";
            UserSession();
            UserConfListAndForm(this.KeyName);
            MDChangeEmpCode BSM = new MDChangeEmpCode();
            if (Session[Index_Sess_Obj + ActionName] != null)
            {
                BSM = (MDChangeEmpCode)Session[Index_Sess_Obj + ActionName];
            }
            Session[Index_Sess_Obj + ActionName] = BSM;
            return View(BSM);
        }
        public ActionResult Change(string EmpCodeFrom, string EmpCodeTo)
        {
            ActionName = "Index";
            UserSession();
            UserConfForm(SYActionBehavior.EDIT);
            try
            {
                var ChkCode = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCodeFrom);
                var ChkNCode = DB.HRStaffProfiles.FirstOrDefault(w => w.EmpCode == EmpCodeTo);
                if (ChkCode != null && ChkNCode == null)
                {
                    var DBC = new HumicaDBContext();
                    string Cmd = "exec dbo.Change_EmpCode '" + EmpCodeTo + "','" + EmpCodeFrom + "','" + user.UserName + "'";
                    DBC.Database.ExecuteSqlCommand(Cmd);

                    SYMessages mess = SYMessages.getMessageObject("MS001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
                else
                {
                    SYMessages mess = SYMessages.getMessageObject("EE001", user.Lang);
                    Session[SYConstant.MESSAGE_SUBMIT] = mess;
                    return Redirect(SYUrl.getBaseUrl() + URL_SCREEN);
                }
            }
            catch (Exception ex)
            {

            }
            return Redirect(SYUrl.getBaseUrl());
        }

    }

}