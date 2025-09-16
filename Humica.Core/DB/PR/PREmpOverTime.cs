namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpOverTime")]
    public partial class PREmpOverTime
    {
        public int? Line { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(500)]
        public string EmpName { get; set; }

        public DateTime? PayMonth { get; set; }

        public DateTime? OTDate { get; set; }

        public decimal OTHour { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string OTType { get; set; }

        [StringLength(150)]
        public string OTDescription { get; set; }

        public int? LCK { get; set; }

        public int? Status { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int? TranType { get; set; }

        public string Reason { get; set; }

        public DateTime? OTFromTime { get; set; }

        public DateTime? OTToTime { get; set; }

        public decimal? BreakTime { get; set; }

    }
}
