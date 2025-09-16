namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PREmpWorking")]
    public partial class PREmpWorking
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TranNo { get; set; }

        public int? PayPeriodId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        public int? InYear { get; set; }

        public int? InMonth { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public decimal ActWorkDay { get; set; }

        public decimal? Hours { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(15)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(15)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
