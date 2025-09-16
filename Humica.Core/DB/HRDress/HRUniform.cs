namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRUniform")]
    public partial class HRUniform
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        public bool IsAutoSelected { get; set; }
    }
}
