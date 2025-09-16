namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HisPendingAppSalaryFP")]
    public partial class HisPendingAppSalaryFP
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [Key]
        [Column(Order = 1)]
        public int PeriodID { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        public bool IsLock { get; set; }

    }
}
