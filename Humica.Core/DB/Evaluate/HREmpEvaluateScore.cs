namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEvaluateScore")]
    public partial class HREmpEvaluateScore
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EvaluateID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(10)]
        public string Region { get; set; }
        public string RegionDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Score { get; set; }

        public int RatingID { get; set; }
    }
}
