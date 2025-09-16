using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using System.Configuration;

namespace Humica.Logic.MD
{
    public class ClsRunScript
    {
        private HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string ActionName { get; set; }
        public string Script { get; set; }
        public SYHisScript Header { get; set; }

        public ClsRunScript()
        {
            this.User = SYSession.getSessionUser();
            this.BS = SYSession.getSessionUserBS();
        }
        string NorthwindConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            }
        }
        public System.Web.UI.WebControls.SqlDataSource DataSource
        {
            get
            {
                return new System.Web.UI.WebControls.SqlDataSource(NorthwindConnectionString, Script);
            }
        }
        public string RunScript(string _Script)
        {
            System.Web.UI.WebControls.SqlDataSource sql = new System.Web.UI.WebControls.SqlDataSource();
            sql.ConnectionString = NorthwindConnectionString;
            int raw = 0;
            if (Script.Substring(0, 6).ToUpper() == "INSERT")
            {
                sql.InsertCommand = _Script;
                raw = sql.Insert();
            }
            else if (Script.Substring(0, 6).ToUpper() == "UPDATE")
            {
                sql.UpdateCommand = _Script;
                raw = sql.Update();
            }
            else if (Script.Substring(0, 6).ToUpper() == "DELETE")
            {
                sql.DeleteCommand = _Script;
                raw = sql.Delete();
            }
            return SYConstant.OK;
        }
    }
}