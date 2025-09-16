namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAdvertising")]
    public partial class RCMAdvertising
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string VacNo { get; set; }

        [StringLength(15)]
        public string AdsType { get; set; }

        [StringLength(20)]
        public string Advertising { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        public decimal? TotalCost { get; set; }

        public decimal? TotalBudget { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }
    }
}
