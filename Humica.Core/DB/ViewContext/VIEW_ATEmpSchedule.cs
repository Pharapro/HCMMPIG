namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class VIEW_ATEmpSchedule
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        public bool? GEN { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string LevelCode { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        [StringLength(10)]
        public string SHIFT { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        public DateTime? Start1 { get; set; }

        public DateTime? End1 { get; set; }

        public DateTime? Start2 { get; set; }

        public DateTime? End2 { get; set; }

        [StringLength(10)]
        public string DivisionCode { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(10)]
        public string Division { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal WHOUR { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal WOT { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal NWH { get; set; }

        [StringLength(10)]
        public string GMSTATUS { get; set; }

        public int? Late1 { get; set; }

        public int? Late2 { get; set; }

        public int? Early1 { get; set; }

        public int? Early2 { get; set; }

        [StringLength(10)]
        public string TerminateStatus { get; set; }

        [StringLength(500)]
        public string Remark2 { get; set; }

        public DateTime? WIN1 { get; set; }

        public DateTime? WOUT1 { get; set; }

        public int? WIN_LATE1 { get; set; }

        public int? WOUT_EALY1 { get; set; }

        public DateTime? WIN2 { get; set; }

        public DateTime? WOUT2 { get; set; }

        public int? WIN_LATE2 { get; set; }

        public int? WOUT_EALY2 { get; set; }

        [StringLength(50)]
        public string LEAVEDESC { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "date")]
        public DateTime TranDate { get; set; }

        [StringLength(10)]
        public string LOCT { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        public DateTime? DateTerminate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal OTApproval { get; set; }

        public int? MEAL { get; set; }
        public DateTime? OTStart { get; set; }
        public DateTime? OTEnd { get; set; }
        public decimal? ActWHour { get; set; }
        public int? MissScan { get; set; }
        public DateTime? NSWStart { get; set; }
        public DateTime? NSWEnd { get; set; }
    }
}
