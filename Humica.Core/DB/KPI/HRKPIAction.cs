namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIAction")]
    public partial class HRKPIAction
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }
    }
}
