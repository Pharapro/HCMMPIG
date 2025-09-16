namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;

    public partial class HLCheckShiftImports
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }
    }
}
