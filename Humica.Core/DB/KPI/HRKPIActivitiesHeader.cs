namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIActivitiesHeader")]
    public partial class HRKPIActivitiesHeader
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string AVTCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string AssignCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string KPIGroupCode { get; set; }

        [StringLength(10)]
        public string KPICategory { get; set; }

        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DocumentDate { get; set; }

        [StringLength(50)]
        public string Department { get; set; }

        [StringLength(50)]
        public string VerifyBy { get; set; }

        [StringLength(50)]
        public string ApprovedBy { get; set; }

        [StringLength(250)]
        public string Signature { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ApprovalDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateChecking { get; set; }

        public decimal? TotalAcheivement { get; set; }

        public decimal? KPIAverage { get; set; }

        public decimal? TotalScore { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(50)]
        public string ReasonCode { get; set; }

        [StringLength(150)]
        public string Comment { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangeBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangeOn { get; set; }

        [StringLength(50)]
        public string ReviewBy { get; set; }

        [StringLength(50)]
        public string AcknowledgeBy { get; set; }

        [StringLength(150)]
        public string SectionDESC { get; set; }

        [StringLength(20)]
        public string KPICode { get; set; }
    }
}
