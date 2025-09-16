namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRExchRate")]
    public partial class PRExchRate
    {
        [Key]
        public long TranNo { get; set; }

        public decimal NSSFRate { get; set; }

        public decimal Rate { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int PeriodID { get; set; }
        [StringLength(20)]
        public string Currency { get; set; }
    }
}
