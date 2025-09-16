namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PROTRate")]
    public partial class PROTRate
    {
        [Key]
        [StringLength(10)]
        public string OTCode { get; set; }

        [StringLength(100)]
        public string OTType { get; set; }

        [StringLength(100)]
        public string OTHDESC { get; set; }

        [StringLength(10)]
        public string Measure { get; set; }

        [StringLength(10)]
        public string Foperand { get; set; }

        [StringLength(10)]
        public string Foperator { get; set; }

        [StringLength(10)]
        public string Soperand { get; set; }

        [StringLength(10)]
        public string Soperator { get; set; }

        public decimal? Toperand { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
