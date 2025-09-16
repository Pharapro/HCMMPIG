namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMaterial")]
    public partial class HRMaterial
    {
        [Key]
        [StringLength(20)]
        public string ItemCode { get; set; }

        [StringLength(200)]
        public string ItemDescription { get; set; }

        [StringLength(200)]
        public string ItemDescription2 { get; set; }

        public decimal? Quantity { get; set; }

        public string Images { get; set; }
    }
}
