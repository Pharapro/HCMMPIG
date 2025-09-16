using Humica.EF.MD;
using System.Collections.Generic;
using Humica.Core;
using Humica.Core.DB;

namespace Humica.Logic.Att
{
    public class EmployeeOnside
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public ATEmpActivity Header { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public int id { get; set; }
        public List<ATEmpActivity> ListHeader { get; set; }

        public EmployeeOnside()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}