namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRCompanyGroup")]
    public partial class HRCompanyGroup
    {
        [Key]
        [StringLength(50)]
        public string WorkGroup { get; set; }

        [StringLength(250)]
        public string WorkGroupDescription { get; set; }

        [StringLength(50)]
        public string ParentWorkGroupID { get; set; }

        public int Level { get; set; }
        public string ObjectName { get; set; }
    }
}
