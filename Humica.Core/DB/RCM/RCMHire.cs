namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMHire")]
    public partial class RCMHire
    {
        [Key]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        public DateTime ApplyDate { get; set; }

        [StringLength(50)]
        public string ApplicantName { get; set; }

        [StringLength(10)]
        public string ApplyPosition { get; set; }

        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string EmployeeType { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string Department { get; set; }

        [StringLength(10)]
        public string Category { get; set; }

        [StringLength(10)]
        public string Level { get; set; }

        [StringLength(10)]
        public string Position { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? ProbationEndDate { get; set; }

        public decimal? Salary { get; set; }

        [StringLength(20)]
        public string PayParameter { get; set; }

        [StringLength(10)]
        public string Line { get; set; }

        public bool? IsResident { get; set; }

        public bool? Supervisory { get; set; }

        [StringLength(20)]
        public string FirstLine { get; set; }

        [StringLength(20)]
        public string SecondLine { get; set; }

        [StringLength(20)]
        public string ProbationType { get; set; }

        [StringLength(20)]
        public string WorkingType { get; set; }

        [StringLength(20)]
        public string SalaryType { get; set; }

        public DateTime? LeaveConf { get; set; }

        [StringLength(10)]
        public string JobGrade { get; set; }

        [StringLength(10)]
        public string ROSTER { get; set; }

        public bool? Announcement { get; set; }

        [StringLength(10)]
        public string Section { get; set; }

        [StringLength(10)]
        public string TXPayType { get; set; }

        public bool? IsAtten { get; set; }

        [StringLength(30)]
        public string StaffType { get; set; }

        public string Images { get; set; }

        [StringLength(10)]
        public string HODCode { get; set; }

        public bool? IsBiSalary { get; set; }
        public string CompanyCode { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal BenefitAmount { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
        public string AttachFile { get; set; } 
    }
}
