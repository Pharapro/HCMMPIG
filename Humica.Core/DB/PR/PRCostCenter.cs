namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRCostCenter")]
    public partial class PRCostCenter
    {
        [Key]
        public long ID { get; set; }
        [StringLength(20)]
        public string Branch { get; set; }
        [StringLength(20)]
        public string Warehouse { get; set; }
        [StringLength(20)]
        public string Project { get; set; }

        [StringLength(40)]
        public string Description { get; set; }
    }
}
