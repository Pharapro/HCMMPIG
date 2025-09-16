namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissionClaimItem")]
    public partial class HRMissionClaimItem
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ClaimCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Column(TypeName = "date")]
        public DateTime MissionDate { get; set; }
        [StringLength(100)]
        public string Province { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string WorkingPlan { get; set; }

        [StringLength(50)]
        public string Invoice { get; set; }

        [StringLength(100)]
        public string NonInvoice { get; set; }

        public string Attachment { get; set; }

        public int NumOfPer { get; set; }

        public int QtyInvoice { get; set; }

        public decimal Duration { get; set; }

        public decimal Amount { get; set; }

        [StringLength(10)]
        public string Remark { get; set; }
    }
}
