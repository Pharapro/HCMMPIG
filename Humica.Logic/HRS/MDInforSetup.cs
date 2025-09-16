using Humica.Core.DB;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.HRS
{
    public class MDInforSetup
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
        public List<SYHRCompany> ListCompanyGroup { get; set; }
        public List<HRCompany> ListCompany { get; set; }
        public List<HRBranch> ListBranch { get; set; }
        public List<HRDivision> ListDivision { get; set; }
        public List<HRGroupDepartment> ListGroupDepartment { get; set; }
        public List<HRDepartment> ListDepartment { get; set; }
        public List<HRLevel> ListLevel { get; set; }
        public List<HRLine> ListLine { get; set; }
        public List<HRLocation> ListLocation { get; set; }
        public List<HRSection> ListSection { get; set; }
        public List<HREmpType> ListEmpType { get; set; }
        public List<HRFunction> ListFunction { get; set; }
        public List<HRPosition> ListPosition { get; set; }
        public List<HROrgChart> ListOrgChart { get; set; }
        public List<HRJobGrade> ListJobGrade { get; set; }
        public List<HRCategory> ListCategory { get; set; }
        public List<HRProbationType> ListProType { get; set; }
        public List<HRUniform> ListUniform { get; set; }
        public List<HROffice> ListOffice { get; set; }
        public List<HRGroup> ListGroup { get; set; }
        public MDInforSetup()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}