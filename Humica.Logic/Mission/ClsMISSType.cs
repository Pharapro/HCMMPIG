using Humica.Core.DB;
using Humica.EF.MD;
using Humica.EF.Repo;
using System.Collections.Generic;

namespace Humica.Logic.Mission
{
    public class ClsMISSType
    {


        protected IUnitOfWork unitOfWork;
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<HRMissItem> ListMissItem { get; set; }
        public List<HRMClaimType> ListMClaimType { get; set; }
        public List<HRMTravelby> ListTravelBy { get; set; }
        public List<HRMissOilRating> ListMissOilRating { get; set; }
        public List<HRMItem> ListItem { get; set; }
        public List<HRMType> ListMType { get; set; }
        public void OnLoad()
        {
            unitOfWork = new UnitOfWork<HumicaDBViewContext>(new HumicaDBViewContext());
        }
        public ClsMISSType()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
            OnLoad();
        }
    }
} 