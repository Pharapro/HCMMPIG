namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRParameter")]
    public partial class PRParameter
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public bool WDMON { get; set; }

        public bool WDTUE { get; set; }

        public bool WDWED { get; set; }

        public bool WDTHU { get; set; }

        public bool WDFRI { get; set; }

        public bool WDSAT { get; set; }

        public bool WDSUN { get; set; }

        public decimal? WHOUR { get; set; }

        public int? SALWKTYPE { get; set; }

        public double? SALWKVAL { get; set; }

        public int? OTWKTYPE { get; set; }

        public double? OTWKVAL { get; set; }

        public int? ALWTYPE { get; set; }

        public int? DEDTYPE { get; set; }

        public int? ALWVAL { get; set; }

        public int? DEDVAL { get; set; }

        public double? WHPWEEK { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FROMDATE { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TODATE { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangeOn { get; set; }

        public decimal? WDMONDay { get; set; }

        public decimal? WDTUEDay { get; set; }

        public decimal? WDWEDDay { get; set; }

        public decimal? WDTHUDay { get; set; }

        public decimal? WDFRIDay { get; set; }

        public decimal? WDSATDay { get; set; }

        public decimal? WDSUNDay { get; set; }

        public bool? IsPrevoiuseMonth { get; set; }
    }
}
