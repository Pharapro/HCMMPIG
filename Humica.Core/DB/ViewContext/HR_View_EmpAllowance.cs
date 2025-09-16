namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_View_EmpAllowance
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string AllwCode { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string AllwDescription { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }
    }
}
