namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPODetail")]
    public partial class HRPODetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string PONumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string ItemCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineNbr { get; set; }

        [StringLength(255)]
        public string Descr { get; set; }

        [StringLength(5)]
        public string Unit { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }

        public decimal SubTotal { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }

        [StringLength(100)]
        public string DocumentReference { get; set; }
    }
}
