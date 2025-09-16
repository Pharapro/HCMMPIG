using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Humica.Core.DB
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<HREFBenefit> HREFBenefits { get; set; }
        public virtual DbSet<HREFEmpResign> HREFEmpResigns { get; set; }
        public virtual DbSet<HREFLeave> HREFLeaves { get; set; }
        public virtual DbSet<HREFProbationType> HREFProbationTypes { get; set; }
        public virtual DbSet<HREFReqChangShift> HREFReqChangShifts { get; set; }
        public virtual DbSet<HREFReqProbation> HREFReqProbations { get; set; }
        public virtual DbSet<HREFRequestTransferStaff> HREFRequestTransferStaffs { get; set; }
        public virtual DbSet<HREFTransferAndPromotionType> HREFTransferAndPromotionTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HREFEmpResign>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<HREFEmpResign>()
                .Property(e => e.Status)
                .IsUnicode(false);
        }
    }
}
