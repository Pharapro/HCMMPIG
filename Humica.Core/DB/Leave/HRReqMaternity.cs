namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRReqMaternity")]
    public partial class HRReqMaternity
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public int LateEarly { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateOn { get; set; }

        [StringLength(20)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
