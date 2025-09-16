namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRCostCenterGroup")]
    public partial class PRCostCenterGroup
    {
        [Key]
        [StringLength(25)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public decimal? TotalCharge { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
