namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissionClaim")]
    public partial class HRMissionClaim
    {
        [Key]
        [StringLength(20)]
        public string ClaimCode { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(15)]
        public string ClaimType { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmployeeName { get; set; }

        [StringLength(150)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime ClaimDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }
        public decimal TotalAmount { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        public string WorkingPlan { get; set; }

        [StringLength(20)]
        public string MissionCode { get; set; }

        [StringLength(15)]
        public string AssignFinance { get; set; }

        [StringLength(15)]
        public string SummitTo { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        [StringLength(15)]
        public string ReStatus { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
