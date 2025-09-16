namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATPolicy")]
    public partial class ATPolicy
    {
        [Key]
        public int TranNo { get; set; }

        public DateTime? NWFROM { get; set; }

        public DateTime? NWTO { get; set; }

        public int? OTMN { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int? Late { get; set; }

        public int? Early { get; set; }

        public int? IsLate_Early { get; set; }

        public int? MissScan { get; set; }
        public bool? BaseOnScan { get; set; }
        public int? AfterScan { get; set; }

        public bool LHourByRate { get; set; }
        
        public DateTime LFromDate { get; set; }

        public DateTime LToDate { get; set; }
        [StringLength(20)]
        public string ExtraNS { get; set; }
        public decimal MaxLate { get; set; }
        public decimal MaxEarly { get; set; }
    }
}
