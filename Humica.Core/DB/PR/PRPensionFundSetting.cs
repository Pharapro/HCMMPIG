namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRPensionFundSetting")]
    public partial class PRPensionFundSetting
    {
        [Key]
        public int TrainNo { get; set; }

        public int? SeviceLenghtFrom { get; set; }

        public int? SeviceLenghtTo { get; set; }

        public decimal? ComPercentage { get; set; }

        public decimal? StaffPercentage { get; set; }

        [StringLength(150)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(150)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
    }
}
