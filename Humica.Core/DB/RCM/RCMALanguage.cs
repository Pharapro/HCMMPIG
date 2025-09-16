namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMALanguage")]
    public partial class RCMALanguage
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Lang { get; set; }

        [StringLength(10)]
        public string Speaking { get; set; }

        [StringLength(10)]
        public string Listening { get; set; }

        [StringLength(10)]
        public string Reading { get; set; }

        [StringLength(10)]
        public string Writing { get; set; }
    }
}
