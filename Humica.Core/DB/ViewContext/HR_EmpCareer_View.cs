namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_EmpCareer_View
    {
        [StringLength(15)]
        public string CompanyCode { get; set; }
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(100)]
        public string CareerDesc { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(100)]
        public string Branch { get; set; }

        public string BU { get; set; }

        [StringLength(100)]
        public string Division { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }
        public string GroupDept { get; set; }
        public string Office { get; set; }
        public string Groups { get; set; }
        public string DEPT { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        public DateTime? DOB { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [StringLength(500)]
        public string Reason { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal OldSalary { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal Increase { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal NewSalary { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(29)]
        public string TXPayType { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }
        [StringLength(20)]
        public string Status { get; set; }
        public string BranchID { get; set; }
    }
}
