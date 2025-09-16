namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATEmpMissscan")]
    public partial class ATEmpMissscan
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string DocumentNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(20)]
        public string RequestType { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime MissscanDate { get; set; }

        public DateTime Time { get; set; }

        public string Reason { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
