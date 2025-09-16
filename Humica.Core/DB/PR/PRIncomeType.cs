namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRIncomeType")]
    public partial class PRIncomeType
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(200)]
        public string IncomeType { get; set; }
        public int InOrder { get; set; }
    }
}
