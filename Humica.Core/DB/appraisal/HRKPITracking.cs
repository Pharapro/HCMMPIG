namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPITracking")]
    public partial class HRKPITracking
    {
        [Key]
        public int TranNo { get; set; }
        [StringLength(20)]
        public string AssignCode { get; set; }
        [StringLength(50)]
        public string Measure { get; set; }
        [StringLength(50)]
        public string KPIType { get; set; }
        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }
        [StringLength(250)]
        public string Department { get; set; }
        [StringLength(250)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(50)]
        public string KPI { get; set; }

        //[StringLength(150)]
        public string KPIDescription { get; set; }
        public string ActionPlan { get; set; }
        [StringLength(50)]
        public string DirectedByCode { get; set; }
        public decimal Actual { get; set; }
        public decimal? Target { get; set; }
        [StringLength(20)]
        public string Status { get; set; }

        public string Remark { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ChangedOn { get; set; }
    }
}
