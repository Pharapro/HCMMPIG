namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRExceptType")]
    public partial class PRExceptType
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string ExceptType { get; set; }

        public decimal Amount { get; set; }
        [StringLength(50)]
        public string ValueBased { get; set; }
        public bool? IsActive { get; set; }
    }
}
