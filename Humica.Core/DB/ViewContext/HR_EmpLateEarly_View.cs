namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_EmpLateEarly_View
    {
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

        [StringLength(10)]
        public string DedCode { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string DedDescription { get; set; }

        public decimal? Day { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal Minute { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        public int? Status { get; set; }
    }
}
