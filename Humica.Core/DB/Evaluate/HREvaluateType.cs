namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREvaluateType")]
    public partial class HREvaluateType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public bool IsActive { get; set; }
    }
}
