namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISSVCMonthly")]
    public partial class HISSVCMonthly
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        public decimal? GetPoint { get; set; }

        public decimal? Day { get; set; }

        public decimal? NoDay { get; set; }

        public decimal? ProRate { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Deduct { get; set; }

        public decimal? Adding { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public int? LCK { get; set; }

        [StringLength(15)]
        public string CretaeBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
