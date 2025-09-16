namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRInteAccountItem")]
    public partial class PRInteAccountItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string IntegrateNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [Column(TypeName = "date")]
        public DateTime PaymentDate { get; set; }

        [StringLength(20)]
        public string CostCenter { get; set; }

        [StringLength(10)]
        public string AccountCode { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        public decimal CreditAmount { get; set; }

        public decimal DebitAmount { get; set; }

        [StringLength(15)]
        public string Project { get; set; }
    }
}
