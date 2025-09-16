namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMApplicant")]
    public partial class RCMApplicant
    {
        [Key]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [StringLength(10)]
        public string VacNo { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(20)]
        public string FirstName { get; set; }

        [StringLength(20)]
        public string LastName { get; set; }

        [StringLength(40)]
        public string AllName { get; set; }

        [StringLength(30)]
        public string OthFirstName { get; set; }

        [StringLength(30)]
        public string OthLastName { get; set; }

        [StringLength(60)]
        public string OthAllName { get; set; }

        [StringLength(10)]
        public string WorkingType { get; set; }

        [StringLength(7)]
        public string Gender { get; set; }

        public decimal? ExpectSalary { get; set; }

        public decimal? LastSalary { get; set; }

        [StringLength(10)]
        public string ShortList { get; set; }

        [StringLength(250)]
        public string POB { get; set; }

        [StringLength(10)]
        public string Country { get; set; }

        [StringLength(10)]
        public string Nationality { get; set; }

        [StringLength(10)]
        public string Height { get; set; }

        [StringLength(10)]
        public string Weight { get; set; }

        [StringLength(20)]
        public string Phone1 { get; set; }

        [StringLength(20)]
        public string Phone2 { get; set; }

        [StringLength(250)]
        public string PermanentAddr { get; set; }

        [StringLength(250)]
        public string CurAddr { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        public DateTime ApplyDate { get; set; }

        [StringLength(20)]
        public string ApplyBranch { get; set; }

        [StringLength(30)]
        public string ApplyPosition { get; set; }

        public DateTime? DOB { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Source { get; set; }

        [StringLength(100)]
        public string ResumeFile { get; set; }

        public int? IntvStep { get; set; }

        public DateTime? TestDate { get; set; }

        [StringLength(300)]
        public string TestNote { get; set; }

        public decimal? TestScore { get; set; }

        [StringLength(10)]
        public string TestStatus { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string BloodType { get; set; }

        public bool RefCHK { get; set; }

        [StringLength(20)]
        public string Title { get; set; }

        [StringLength(20)]
        public string Marital { get; set; }

        public bool IsHired { get; set; }

        [StringLength(10)]
        public string ApplyDept { get; set; }

        [StringLength(100)]
        public string CurStage { get; set; }

        [StringLength(15)]
        public string ShortListingBy { get; set; }

        public DateTime? ShortListingDate { get; set; }

        [StringLength(10)]
        public string Sect { get; set; }

        [StringLength(10)]
        public string PostOffer { get; set; }

        [StringLength(20)]
        public string IntVStatus { get; set; }

        public decimal? ProposedSalary { get; set; }

        public decimal? Salary { get; set; }

        [StringLength(10)]
        public string JobLevel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public DateTime? DocDate { get; set; }

        [StringLength(10)]
        public string StaffType { get; set; }

        public decimal? SalaryAfterProb { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        public bool? IsConfirm { get; set; }
    }
}
