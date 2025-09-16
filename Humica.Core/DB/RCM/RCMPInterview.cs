namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMPInterview")]
    public partial class RCMPInterview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranNo { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IntVStep { get; set; }

        [StringLength(70)]
        public string CandidateName { get; set; }

        [StringLength(20)]
        public string VacNo { get; set; }

        [StringLength(20)]
        public string ApplyPost { get; set; }

        public DateTime? ApplyDate { get; set; }

        public DateTime? IntVDate { get; set; }

        public string Strength { get; set; }

        public string Weakness { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        public string IntCmt { get; set; }

        public decimal? ProposedSalary { get; set; }

        [StringLength(20)]
        public string PositionOffer { get; set; }

        [StringLength(200)]
        public string AttachFile { get; set; }

        public DateTime? AppointmentDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(20)]
        public string AlertToInterviewer { get; set; }

        [StringLength(10)]
        public string DocType { get; set; }

        public DateTime? DocDate { get; set; }

        public decimal? SalaryAfterProb { get; set; }

        [StringLength(20)]
        public string ReStatus { get; set; }

        public int TotalScore { get; set; }

        public string Result { get; set; }
    }
}
