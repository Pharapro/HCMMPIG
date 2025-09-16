namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBSocMedAcc")]
    public partial class EOBSocMedAcc
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(500)]
        public string UrlLink { get; set; }

        public bool IsActive { get; set; }

    }
}
