namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPORequestItem")]
    public partial class HRPORequestItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string RequestNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(20)]
        public string ItemCode { get; set; }

        [StringLength(200)]
        public string ItemDescription { get; set; }

        [StringLength(5)]
        public string Unit { get; set; }

        [StringLength(200)]
        public string Brand { get; set; }

        public decimal Quantity { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }
    }
}
