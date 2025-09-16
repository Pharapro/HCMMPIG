namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATPayperiod")]
    public partial class ATPayperiod
    {
        [Key]
        public int PeriodID { get; set; }

        [StringLength(100)]
        public string PeriodString { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }
    }
}
