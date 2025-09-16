namespace Humica.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRTrainingSurvey")]
    public partial class TRTrainingSurvey
    {
        [Key]
        [StringLength(20)]
        public string SurveyID { get; set; }

        [Required]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(30)]
        public string EmpName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SurveyDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SurFromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SurToDate { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateOfHire { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(10)]
        public string Result { get; set; }

        public int TotalScore { get; set; }

        [Required]
        [StringLength(15)]
        public string AssignedTo { get; set; }

        [StringLength(200)]
        public string AssignedPosition { get; set; }

        public string Strengths { get; set; }

        public string Weakness { get; set; }

        public string TrainingProgram { get; set; }

        public string Comments { get; set; }

        public string ActionPlan { get; set; }

        [Required]
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChengedOn { get; set; }
    }
}
