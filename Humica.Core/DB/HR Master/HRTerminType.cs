namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRTerminType")]
    public partial class HRTerminType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }
        [StringLength(20)]
        public string CareerCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public bool? IsCalSalary { get; set; }
    }
}
