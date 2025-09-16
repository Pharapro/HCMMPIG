namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRMissionPlan")]
    public partial class HRMissionPlan
    {
        [Key]
        [StringLength(20)]
        public string MissionCode { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string PlannerName { get; set; }

        [StringLength(20)]
        public string MissionType { get; set; }

        [StringLength(150)]
        public string Position { get; set; }
        [StringLength(200)]
        public string Department { get; set; }

        [Column(TypeName = "date")]
        public DateTime PlanDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public string WorkingPlan { get; set; }

        public int Member { get; set; }

        [StringLength(30)]
        public string TravelBy { get; set; }

        public int Province { get; set; }

        public bool? Credit { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? Distance { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(20)]
        public string ReStatus { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }
        public string PlateNumber { get; set; }
        public string MemoDescription { get; set; }
        public bool? IsMemo { get; set; }

    }
}
