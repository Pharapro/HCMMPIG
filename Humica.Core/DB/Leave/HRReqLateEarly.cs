namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRReqLateEarly")]
    public partial class HRReqLateEarly
    {
        [Key]
        [StringLength(15)]
        public string ReqLaEaNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(10)]
        public string RequestType { get; set; }

        [Column(TypeName = "date")]
        public DateTime LeaveDate { get; set; }

        public int? Qty { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [StringLength(20)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
        public string Remark { get; set; }

    }
}
