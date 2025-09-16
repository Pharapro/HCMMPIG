namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_View_EmpGenSalary
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Probation { get; set; }

        [StringLength(10)]
        public string PayParam { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime DateTerminate { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Section { get; set; }

        public bool? GEN { get; set; }

        public bool? TAXTYPE { get; set; }

        [StringLength(50)]
        public string CAREERDESC { get; set; }

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

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        [StringLength(10)]
        public string SalaryType { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime ReSalary { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool SalaryFlag { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool IsHold { get; set; }

        public bool? IsPayPartial { get; set; }

        public int? SericeLenghtDay { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }
}
