namespace Humica.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRSurveyRegion")]
    public partial class TRSurveyRegion
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        public string Description { get; set; }

        [StringLength(500)]
        public string SecDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public bool? IsRating { get; set; }

        public int? InOrder { get; set; }

        public bool? IsComment { get; set; }
    }
}
