namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissItem")]
    public partial class HRMissItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string Type { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecDescription { get; set; }
    }
}
