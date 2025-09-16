namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissType")]
    public partial class HRMissType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
