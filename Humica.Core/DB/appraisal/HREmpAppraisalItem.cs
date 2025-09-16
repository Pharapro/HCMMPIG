namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpAppraisalItem")]
    public partial class HREmpAppraisalItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string ApprID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(10)]
        public string Region { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
        public int Weighting { get; set; }

        public decimal? Score { get; set; }

        public decimal? ScoreAppraiser { get; set; }

        public decimal? ScoreAppraiser2 { get; set; }

        public int RatingID { get; set; }
        public string Comment { get; set; }
        public string RegionDescription { get; set; }
    }
}
