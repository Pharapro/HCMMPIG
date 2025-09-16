namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpSVC")]
    public partial class PREmpSVC
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public double? Point { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? InActive { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
