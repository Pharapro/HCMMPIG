namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenDeduction")]
    public partial class HISGenDeduction
    {
        [Key]
        public long TranNo { get; set; }

        public long? INYear { get; set; }

        public int? INMonth { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? RatePerDay { get; set; }

        [StringLength(10)]
        public string DedCode { get; set; }

        [StringLength(100)]
        public string DedDesc { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        public bool? TaxAble { get; set; }

        public decimal? DedAm { get; set; }

        public decimal? DedAMPM { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(15)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        [StringLength(50)]
        public string Reckey { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }
    }
}
