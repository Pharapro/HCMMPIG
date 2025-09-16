namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRCompanyTree")]
    public partial class HRCompanyTree
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LevelGroup { get; set; }

        [StringLength(50)]
        public string WorkGroup { get; set; }

        [StringLength(50)]
        public string ParentWorkGroupID { get; set; }

        [StringLength(50)]
        public string CompanyMember { get; set; }

        [StringLength(250)]
        public string CompanyMemberDesc { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Level { get; set; }
        public bool? IsAssistant { get; set; }
        [StringLength(50)]
        public string SubParent { get; set; }
    }
}
