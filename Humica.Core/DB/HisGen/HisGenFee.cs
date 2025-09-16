namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisGenFee")]
    public partial class HisGenFee
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        [StringLength(20)]
        public string PayType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string Levels { get; set; }

        public decimal FeeFrom { get; set; }

        public decimal FeeTo { get; set; }

        public decimal? Rate { get; set; }

        public decimal Amount { get; set; }

        [StringLength(10)]
        public string Type { get; set; }

        public decimal DedAmount { get; set; }
    }
}
