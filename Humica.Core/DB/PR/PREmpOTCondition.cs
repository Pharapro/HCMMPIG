namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpOTCondition")]
    public partial class PREmpOTCondition
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string AllName { get; set; }

        [Column(TypeName = "date")]
        public DateTime OTDate { get; set; }

        public decimal Hours { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }
    }
}
