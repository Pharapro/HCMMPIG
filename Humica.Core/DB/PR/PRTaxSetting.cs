namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRTaxSetting")]
    public partial class PRTaxSetting
    {
        [Key]
        public int TranNo { get; set; }

        public decimal? TaxFrom { get; set; }

        public decimal? TaxTo { get; set; }

        public decimal? TaxPercent { get; set; }

        public decimal? Amdeduct { get; set; }

        public int? SalaryBasic { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreatOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
