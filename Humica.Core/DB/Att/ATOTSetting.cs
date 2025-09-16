namespace Humica.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATOTSetting")]
    public partial class ATOTSetting
    {
        [Key]
        [StringLength(20)]
        public string OTTYPE { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool? IsDayOFF { get; set; }

        public bool? IsPH { get; set; }

        public bool? OverNight { get; set; }
        public bool? IsSunday { get; set; }
    }
}
