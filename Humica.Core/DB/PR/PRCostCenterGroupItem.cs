namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRCostCenterGroupItem")]
    public partial class PRCostCenterGroupItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string CodeCCGoup { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CostCenterType { get; set; }

        public decimal? Charge { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
