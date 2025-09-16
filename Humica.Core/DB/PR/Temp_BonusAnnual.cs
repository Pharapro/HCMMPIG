namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Temp_BonusAnnual
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long InYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(15)]
        public string BonusCode { get; set; }

        [StringLength(100)]
        public string BonusDesc { get; set; }

        public decimal? Salary { get; set; }

        public decimal? Unpaid_leave { get; set; }

        public decimal? Warning { get; set; }

        public decimal? Adding { get; set; }

        public decimal? TotalBonus { get; set; }

        public decimal? Bonus { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public decimal? S_Month { get; set; }

        public decimal? O_Salary { get; set; }

        public decimal? O_Month { get; set; }

        public decimal? PR_Salary { get; set; }

        public decimal? PR_Month { get; set; }

        public int? LOCK { get; set; }

        [StringLength(50)]
        public string REMARK { get; set; }
    }
}
