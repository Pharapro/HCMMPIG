namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRCurrency")]
    public partial class HRCurrency
    {
        [Key]
        [StringLength(5)]
        public string CurrencyCode { get; set; }

        [StringLength(20)]
        public string CurrencySymbol { get; set; }

        public int DecimalPrecision { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public bool Active { get; set; }
    }
}
