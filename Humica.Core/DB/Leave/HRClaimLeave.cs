namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRClaimLeave")]
    public partial class HRClaimLeave
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [Column(TypeName = "date")]
        public DateTime WorkingDate { get; set; }

        [StringLength(30)]
        public string WorkingType { get; set; }

        public decimal WorkingHour { get; set; }

        [StringLength(30)]
        public string ClaimLeave { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        public DateTime? Expired { get; set; }
        public bool? IsExpired { get; set; }
        public string Status { get; set; }
        public bool? IsUsed { get; set; }
        [StringLength(50)]
        public string DocumentRef { get; set; }
    }
}
