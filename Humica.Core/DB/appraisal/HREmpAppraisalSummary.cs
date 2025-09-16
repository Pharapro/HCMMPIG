namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpAppraisalSummary")]
    public partial class HREmpAppraisalSummary
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string AppraisalNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AppraisalType { get; set; }
        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TaskID { get; set; }

        [StringLength(250)]
        public string EvaluationCriteria { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Score { get; set; }
        public decimal? SubTotal { get; set; }
        public int? InOrder { get; set; }
        public bool? IsKPI { get; set; }
    }
}
