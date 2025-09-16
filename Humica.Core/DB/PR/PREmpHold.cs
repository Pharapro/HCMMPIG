namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpHold")]
    public partial class PREmpHold
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime InMonth { get; set; }

        public decimal? Salary { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Reason { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PayBack { get; set; }
    }
}
