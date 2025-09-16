namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSVCValue")]
    public partial class PRSVCValue
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TranDate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int LCK { get; set; }

        public decimal? SVCAM { get; set; }

        public decimal? TIPAM { get; set; }

        [StringLength(15)]
        public string SVCACC { get; set; }

        [StringLength(15)]
        public string TIPACC { get; set; }

        public decimal? SVCOAM { get; set; }

        public decimal? SVCPAM { get; set; }

        [StringLength(20)]
        public string SVCPACC { get; set; }

        public decimal? SVCPERCEN { get; set; }

        public decimal? PFoundFORWARD { get; set; }

        public decimal? PCARRYFORWARD { get; set; }

        public int? PAYABLEACC { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
