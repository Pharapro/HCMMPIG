namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRInsuranceType")]
    public partial class HRInsuranceType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public bool? LCK { get; set; }
    }
}
