namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGLBenCharge")]
    public partial class HISGLBenCharge
    {
        [StringLength(30)]
        public string CompanyCode { get; set; }

        [StringLength(30)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [StringLength(30)]
        public string PostPeriod { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string GRPBen { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string BenCode { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string GLCode { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(20)]
        public string CurrencyCode { get; set; }

        [StringLength(20)]
        public string CostCenter { get; set; }
        [StringLength(20)]
        public string Warehouse { get; set; }
        [StringLength(20)]
        public string Project { get; set; }
        public bool IsGenerate { get; set; }
        public bool? IsCredit { get; set; }
        [StringLength(30)]
        public string MaterialCode { get; set; }
        public bool? IsPO { get; set; }
        public bool? IsCreditNote { get; set; }
        public bool? IsDebitNote { get; set; }
        public int PeriodID { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
    }
}
