namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREFReqChangShift")]
    public partial class HREFReqChangShift
    {
        public int ID { get; set; }

        [StringLength(40)]
        public string Status { get; set; }

        [StringLength(400)]
        public string Reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(30)]
        public string Increment { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(250)]
        public string Department { get; set; }

        [StringLength(250)]
        public string Position { get; set; }

        public DateTime? StartDate { get; set; }

        [StringLength(250)]
        public string Section { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(100)]
        public string CurrentDuty { get; set; }

        [StringLength(100)]
        public string DutyRequest { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(500)]
        public string AttachFile { get; set; }
    }
}
