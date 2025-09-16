namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRChartofAccount")]
    public partial class PRChartofAccount
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(1)]
        public string DC { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string GrpAcc { get; set; }

        [StringLength(10)]
        public string GrpBen { get; set; }

        [StringLength(10)]
        public string BenCode { get; set; }
    }
}
