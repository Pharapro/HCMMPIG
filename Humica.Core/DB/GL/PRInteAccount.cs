namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRInteAccount")]
    public partial class PRInteAccount
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string IntegrateNo { get; set; }

        [StringLength(30)]
        public string DocumentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        public decimal DebitAmount { get; set; }

        public decimal CreditAmount { get; set; }

        [StringLength(30)]
        public string CurrencyCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PaymentDate { get; set; }

        public int InYear { get; set; }

        public int InMonth { get; set; }

        [StringLength(30)]
        public string Status { get; set; }

        [StringLength(30)]
        public string BatchNo { get; set; }

        [StringLength(30)]
        public string CancelRef { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
