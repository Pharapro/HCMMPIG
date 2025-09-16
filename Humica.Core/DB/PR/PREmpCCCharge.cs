namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpCCCharge")]
    public partial class PREmpCCCharge
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        public string CostCenter { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public decimal? Charge { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
