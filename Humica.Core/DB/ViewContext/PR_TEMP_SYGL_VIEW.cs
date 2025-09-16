namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_TEMP_SYGL_VIEW
    {
        [StringLength(50)]
        public string GroupAcc { get; set; }
        [StringLength(200)]
        public string BenefitGroup { get; set; }
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string AccCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string SortKey { get; set; }
    }
}
