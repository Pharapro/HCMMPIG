namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_VIEW_EmpLeave
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Level { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string LeaveCode { get; set; }

        [StringLength(100)]
        public string LeaveType { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime ToDate { get; set; }

        public double? NoDay { get; set; }

        public double? NoPH { get; set; }

        public double? NoRest { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(250)]
        public string Status { get; set; }
        public string Waiting { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }
        [StringLength(20)]
        public string Units { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangedOn { get; set; }
        public bool? IsReqCancel { get; set; }
        public string ResonToCancel { get; set; }
    }
}
