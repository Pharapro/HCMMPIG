namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRIntegratePOItem")]
    public partial class PRIntegratePOItem
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
        public string MaterialCode { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(20)]
        public string CostCenter { get; set; }

        [Required]
        [StringLength(10)]
        public string AccountCode { get; set; }

        [Required]
        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(15)]
        public string Project { get; set; }

        [StringLength(5)]
        public string Unit { get; set; }

        public decimal Quantity { get; set; }

        public decimal? Amount { get; set; }
        [StringLength(20)]
        public string Warehouse { get; set; }
    }
}
