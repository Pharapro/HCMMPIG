namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRPORequest")]
    public partial class HRPORequest
    {
        [Key]
        [StringLength(20)]
        public string RequestNumber { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(20)]
        public string DocumentType { get; set; }

        [StringLength(20)]
        public string Requestor { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExtectedDate { get; set; }

        public decimal TotalQty { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }
    }
}
