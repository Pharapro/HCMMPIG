namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRBiExchangeRate")]
    public partial class PRBiExchangeRate
    {
        [Key]
        public long TranNo { get; set; }

        public int? InYear { get; set; }

        public int? InMonth { get; set; }

        public decimal NSSFRate { get; set; }

        public decimal Rate { get; set; }

        public int? LCK { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int Period { get; set; }

        [StringLength(100)]
        public string ExchDes { get; set; }
    }
}
