using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public class MDMedicalType
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRMedicalType> ListMedicalType { get; set; }
        public List<HRHospital> ListHopital { get; set; }
        public List<HRPHCKHResult> ListPHChkResult { get; set; }
        public MDMedicalType()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}