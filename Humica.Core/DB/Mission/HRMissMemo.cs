namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMissMemo")]
    public partial class HRMissMemo
    {
        [Key]
        [StringLength(20)]
        public string MissionCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        [StringLength(20)]
        public string MissionType { get; set; }

        [StringLength(150)]
        public string Position { get; set; }

        [StringLength(150)]
        public string Department { get; set; }

        [Column(TypeName = "date")]
        public DateTime PlanDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDay { get; set; }

        public string WorkingPlan { get; set; }

        public int Member { get; set; }

        public decimal? TotalAmount { get; set; }

        [StringLength(30)]
        public string TravelBy { get; set; }

        public bool IsDriver { get; set; }

        [StringLength(200)]
        public string DriverName { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(20)]
        public string CarNo { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }
        public bool? IsClaim { get; set; }
        public int? TotalDate { get; set; } 
    }
}
