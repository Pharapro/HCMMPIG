namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMRSkillRequire")]
    public partial class RCMRSkillRequire
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string RequestNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(20)]
        public string SkillCategory { get; set; }

        [StringLength(20)]
        public string SkillType { get; set; }

        [StringLength(300)]
        public string Description { get; set; }
    }
}
