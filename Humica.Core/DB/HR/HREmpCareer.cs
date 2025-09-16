namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpCareer")]
    public partial class HREmpCareer
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [StringLength(10)]
        public string CareerCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }
        public string NewEmpCode { get; set; }

        [StringLength(10)]
        public string EmpType { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }
        [StringLength(10)]
        public string Office { get; set; }

        [StringLength(10)]
        public string LINE { get; set; }

        [StringLength(10)]
        public string CATE { get; set; }
        [StringLength(10)]
        public string Groups { get; set; }
        [StringLength(10)]
        public string SECT { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(10)]
        public string JobGrade { get; set; }

        [StringLength(10)]
        public string PersGrade { get; set; }

        [StringLength(10)]
        public string HomeFunction { get; set; }

        [StringLength(10)]
        public string Functions { get; set; }

        [StringLength(10)]
        public string SubFunction { get; set; }

        [StringLength(10)]
        public string JobCode { get; set; }

        [StringLength(100)]
        public string JobDesc { get; set; }

        [StringLength(500)]
        public string JobSpec { get; set; }

        public decimal OldSalary { get; set; }

        public decimal Increase { get; set; }

        public decimal NewSalary { get; set; }

        [StringLength(10)]
        public string SupCode { get; set; }

        [StringLength(140)]
        public string SuperVisor { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EffectDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public int? Active { get; set; }

        [StringLength(140)]
        public string Appby { get; set; }

        [StringLength(50)]
        public string AppDate { get; set; }

        [StringLength(140)]
        public string VeriFyBy { get; set; }

        [StringLength(20)]
        public string VeriFYDate { get; set; }

        public int? APPL { get; set; }

        [StringLength(140)]
        public string APPLBY { get; set; }

        public int? LCK { get; set; }

        public int? LCKEDIT { get; set; }

        [StringLength(10)]
        public string resigntype { get; set; }

        [StringLength(50)]
        public string EstartSAL { get; set; }

        [StringLength(50)]
        public string EIncrease { get; set; }

        [StringLength(50)]
        public string ESalary { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProBation { get; set; }

        [StringLength(50)]
        public string RECKEY { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastDate { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string SalaryType { get; set; }

        public bool? ALRemaining { get; set; }

        [StringLength(30)]
        public string StaffType { get; set; }
        [StringLength(10)]
        public string GroupDept { get; set; }

        public string AttachFile { get; set; }
    }
}
