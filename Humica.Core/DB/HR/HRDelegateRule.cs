namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDelegateRule")]
    public partial class HRDelegateRule
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(70)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Person_In_Charge { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(200)]
        public string DRPath { get; set; }
    }
}
