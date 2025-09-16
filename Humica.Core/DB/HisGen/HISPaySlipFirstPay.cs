namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.Caching;

    [Table("HISPaySlipFirstPay")]
    public partial class HISPaySlipFirstPay
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranLine { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }
        [Key]
        [Column(Order = 2)]
        public int PeriodID { get; set; }
        [Key]
        [Column(Order = 3)]
        [StringLength(20)]
        public string CompanyCode { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        [StringLength(200)]
        public string EarnDesc { get; set; }

        public decimal? EAmount { get; set; }

        [StringLength(200)]
        public string DeductDesc { get; set; }

        public decimal? DeductAmount { get; set; }

        [StringLength(15)]
        public string DeletedBy { get; set; }

        public DateTime? DeletedOn { get; set; }

        [StringLength(50)]
        public string Reckey { get; set; }
        
    }
}
