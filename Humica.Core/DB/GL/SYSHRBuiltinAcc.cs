namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SYSHRBuiltinAcc")]
    public partial class SYSHRBuiltinAcc
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(50)]
        public string GroupAcc { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(200)]
        public string ObjectName { get; set; }

        [StringLength(200)]
        public string FieldName { get; set; }
        public bool? IsCredit { get; set; }
        public bool? IsPO { get; set; }
    }
}
