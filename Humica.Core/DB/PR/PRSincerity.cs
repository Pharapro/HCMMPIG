namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSincerity")]
    public partial class PRSincerity
    {
        [Key]
        [StringLength(50)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public decimal? Salary { get; set; }

        public decimal? Rate { get; set; }

        public decimal? TotalBalance { get; set; }

        public int InYear { get; set; }

        public int InMonth { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
    }
}
