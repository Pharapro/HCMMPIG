namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRLeaveImport")]
    public partial class HRLeaveImport
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LeaveDate { get; set; }

        [StringLength(15)]
        public string LeaveCode { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(10)]
        public string Status { get; set; }
    }
}
