namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMEmpEvaluateScore")]
    public partial class RCMEmpEvaluateScore
    {
        public int ID { get; set; }

        public string Applicant { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(10)]
        public string Region { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Score { get; set; }

        public int? InVStep { get; set; }
    }
}
