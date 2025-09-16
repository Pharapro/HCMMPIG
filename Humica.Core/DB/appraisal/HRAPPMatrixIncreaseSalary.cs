namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAPPMatrixIncreaseSalary")]
    public partial class HRAPPMatrixIncreaseSalary
    {
        public long ID { get; set; }
        [StringLength(50)]
        public string ScreenID { get; set; }
        [StringLength(50)]
        public string DocumentRef { get; set; }

        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        public int InYear { get; set; }

        [StringLength(10)]
        public string JobLevel { get; set; }

        public decimal JobLevelMidPoint { get; set; }

        public decimal Salary { get; set; }

        public int Rating { get; set; }

        public decimal CompaRatio { get; set; }

        public decimal SalaryIncPers { get; set; }

        public decimal SalaryIncAmount { get; set; }

        public decimal? Adding { get; set; }

        public decimal NewSalary { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
