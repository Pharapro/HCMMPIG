namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRBankFee")]
    public partial class PRBankFee
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(50)]
        public string BrankCode { get; set; }

        public decimal FeeFrom { get; set; }

        public decimal FeeTo { get; set; }

        public decimal Rate { get; set; }

        [StringLength(10)]
        public string Type { get; set; }
    }
}
