namespace Humica.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRPayPeriodItem")]
    public partial class PRPayPeriodItem
    {
        public int FiscalYear { get; set; }

        [Key]
        public int PeriodID { get; set; }

        [StringLength(7)]
        public string PeriodString { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ATStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ATEndDate { get; set; }

        public bool IsActive { get; set; }
    }
}
