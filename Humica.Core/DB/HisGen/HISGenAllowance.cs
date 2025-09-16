namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenAllowance")]
    public partial class HISGenAllowance
    {
        [Key]
        public long TranNo { get; set; }

        public long INYear { get; set; }

        public int INMonth { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? WorkDay { get; set; }

        public decimal? RatePerDay { get; set; }

        [StringLength(10)]
        public string AllwCode { get; set; }

        [StringLength(100)]
        public string AllwDesc { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        public bool? TaxAble { get; set; }

        public decimal? AllwAm { get; set; }

        public decimal? AllwAmPM { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(10)]
        public string PayTo { get; set; }

        public bool? FringTax { get; set; }

        [StringLength(15)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public decimal? ML_PH { get; set; }
    }
}
