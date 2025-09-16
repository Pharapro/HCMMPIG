namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMERecruitD")]
    public partial class RCMERecruitD
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string JobID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RequestNo { get; set; }

        [StringLength(10)]
        public string Position { get; set; }

        public int? NoOfRecruit { get; set; }
    }
}
