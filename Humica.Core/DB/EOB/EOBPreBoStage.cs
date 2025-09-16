namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBPreBoStage")]
    public partial class EOBPreBoStage
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(50)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

    }
}
