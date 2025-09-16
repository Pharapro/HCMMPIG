namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_ListEmployee
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(70)]
        public string FirstName { get; set; }

        [StringLength(70)]
        public string LastName { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(5)]
        public string TITLE { get; set; }

        [StringLength(10)]
        public string SEX { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal Salary { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Probation { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string CareerCode { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal OldSalary { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal Increase { get; set; }

        [Key]
        [Column(Order = 5)]
        public decimal NewSalary { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(100)]
        public string BranchDesc { get; set; }

        [StringLength(100)]
        public string DevisionDesc { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(100)]
        public string Departion { get; set; }

        [StringLength(10)]
        public string MV_LEVEL { get; set; }

        [StringLength(10)]
        public string TERMINATESTATUS { get; set; }

        [StringLength(100)]
        public string TEAMDESC { get; set; }

        [StringLength(100)]
        public string OldPosition { get; set; }

        [StringLength(100)]
        public string OldLevel { get; set; }

        [StringLength(100)]
        public string CareerDesc { get; set; }

        [StringLength(10)]
        public string WORKID { get; set; }

        [StringLength(100)]
        public string BankName { get; set; }

        [StringLength(50)]
        public string BankAcc { get; set; }

        [StringLength(100)]
        public string Phone1 { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string SECT { get; set; }

        [StringLength(10)]
        public string JobCode { get; set; }
    }
}
