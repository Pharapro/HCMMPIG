using Humica.EF.MD;
using Humica.Training.DB;
using System.Collections.Generic;

namespace Humica.Training.Setup
{
    public class ClsQualification
    {
        HumicaDBContextTR DBX = new HumicaDBContextTR();
        public SYUser User { get; set; }
        public string ScreenID { get; set; }
        public string Program { get; set; }
        public string Course { get; set; }
        public SYUserBusiness BS { get; set; }
        public List<TRQualificatClass> ListQualiClass { get; set; }
        public List<TRQualificatName> ListQualiName { get; set; }
        public List<TRQualificatType> ListQualiType { get; set; }

        public ClsQualification()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}