namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISGenSalaryD")]
    public partial class HISGenSalaryD
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        public long TranNo { get; set; }

        public long INYear { get; set; }

        public int INMonth { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(10)]
        public string CareerCode { get; set; }

        public decimal? WorkDay { get; set; }

        public decimal? WorkHour { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string EmpType { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Location { get; set; }

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
        public string Sect { get; set; }
        [StringLength(10)]
        public string Groups { get; set; }

        [StringLength(10)]
        public string POST { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(10)]
        public string JobGrade { get; set; }

        [StringLength(10)]
        public string PersGrade { get; set; }

        [StringLength(10)]
        public string Homefunction { get; set; }

        [StringLength(10)]
        public string Functions { get; set; }

        [StringLength(10)]
        public string SubFunction { get; set; }

        public DateTime? PayFrom { get; set; }

        public DateTime? PayTo { get; set; }

        public decimal? ActWorkDay { get; set; }

        public decimal? BasicSalary { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string RECKEY { get; set; }

        public decimal? ActWorkHours { get; set; }
        public int PeriodID { get; set; }
    }
}
