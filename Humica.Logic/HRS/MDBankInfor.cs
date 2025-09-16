using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public class MDBankInfor
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        //public HRBranch Header { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRBank> ListBank { get; set; }
        public List<HRBankBranch> ListBankBranch { get; set; }
        public List<PRBankFee> ListBankFee { get; set; }
        public List<HRBankAccount> ListBankAccount { get; set; }
        public MDBankInfor()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}