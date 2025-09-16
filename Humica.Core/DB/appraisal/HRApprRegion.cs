namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApprRegion")]
    public partial class HRApprRegion
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string AppraiselType { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Code { get; set; }

        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? InOrder { get; set; }

        public bool? IsKPI { get; set; }

        public decimal? Rating { get; set; }
    }
}
