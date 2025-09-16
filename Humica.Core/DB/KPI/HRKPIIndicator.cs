namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIIndicator")]
    public partial class HRKPIIndicator
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(200)]
        public string SecDescription { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(20)]
        public string Department { get; set; }
        [StringLength(20)]
        public string Category { get; set; }
    }
}