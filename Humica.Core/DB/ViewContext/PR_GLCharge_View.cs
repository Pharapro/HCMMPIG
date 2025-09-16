namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class PR_GLCharge_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string Branch { get; set; }

        [StringLength(20)]
        public string CostCenter { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string PostPeriod { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string SOURCE { get; set; }

        [StringLength(100)]
        public string GLDESCRIPTION { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string GLCode { get; set; }

        public decimal? Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string CurrencyCode { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal DEBITAM { get; set; }

        [Key]
        [Column(Order = 7)]
        public decimal CREDITAM { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string SortKey { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        public string MaterialCode { get; set; }
        public bool? IsCredit { get; set; }
        public bool? IsPO { get; set; }
        [Key]
        [Column(Order = 12)]
        [StringLength(20)]
        public string Warehouse { get; set; }
        [Key]
        [Column(Order = 13)]
        [StringLength(20)]
        public string Project { get; set; }
        public bool? IsDebitNote { get; set; }
    }
}
