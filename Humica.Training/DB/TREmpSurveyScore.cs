namespace Humica.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TREmpSurveyScore")]
    public partial class TREmpSurveyScore
    {
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string SurveyID { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string SecDescription { get; set; }

        [StringLength(10)]
        public string Region { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Score { get; set; }
    }
}
