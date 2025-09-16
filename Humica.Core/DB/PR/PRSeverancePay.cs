namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSeverancePay")]
    public partial class PRSeverancePay
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string EmpCode { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InYear { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int InMonth { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }

        public decimal TotalSalary { get; set; }

        public decimal? TotalAmount { get; set; }

        public int TotalMonth { get; set; }

        public decimal Rate { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }
    }
}
