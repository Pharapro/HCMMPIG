namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRReqPaymentItem")]
    public partial class HRReqPaymentItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string RPNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(150)]
        public string SupportingDoc { get; set; }

        public decimal? AmountReq { get; set; }

        public decimal? AmountRev { get; set; }

        [StringLength(100)]
        public string DocumentReference { get; set; }
    }
}
