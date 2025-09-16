namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_RP_View
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string RPNumber { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(140)]
        public string Requestor { get; set; }

        [StringLength(20)]
        public string BeneName { get; set; }

        [StringLength(20)]
        public string BeneAcc { get; set; }

        [StringLength(20)]
        public string BeneBank { get; set; }

        [StringLength(20)]
        public string Currency { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool Advance { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool SettlementAdvance { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool PayymenyStaff { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool PaymentVendor { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal TotalAmountReq { get; set; }

        [Key]
        [Column(Order = 7)]
        public decimal TotalAmountRev { get; set; }

        public decimal? AdvanceAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime? AdvanceDate { get; set; }

        public decimal? DueToEmployee { get; set; }

        public decimal? DueToCompany { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeliveryDate { get; set; }

        public string DescDetails { get; set; }
    }
}
