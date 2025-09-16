namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_PR_VIEW
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string RequestNumber { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(50)]
        public string DocumentType { get; set; }

        [StringLength(140)]
        public string Requestor { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExtectedDate { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal TotalQty { get; set; }

        public string DescDetails { get; set; }
    }
}
