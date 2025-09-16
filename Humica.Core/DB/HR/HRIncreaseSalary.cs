namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRIncreaseSalary")]
    public partial class HRIncreaseSalary
    {
        [Key]
        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [StringLength(10)]
        public string Dept { get; set; }

        [StringLength(10)]
        public string Post { get; set; }

        [StringLength(20)]
        public string Requestor { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        public decimal? Salary { get; set; }

        public DateTime? EffecDate { get; set; }

        public decimal Increase { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime DocumentDate { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
