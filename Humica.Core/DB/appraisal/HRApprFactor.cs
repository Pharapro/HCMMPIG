namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApprFactor")]
    public partial class HRApprFactor
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string AppraiselType { get; set; }

        [StringLength(20)]
        public string Region { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }
        [StringLength(500)]
        public string Remark { get; set; }

    }
}
