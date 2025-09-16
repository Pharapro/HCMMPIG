namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATEmpSchedule")]
    public partial class ATEmpSchedule
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(20)]
        public string CardNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime TranDate { get; set; }

        public int? Flag1 { get; set; }

        public DateTime? IN1 { get; set; }

        public DateTime? OUT1 { get; set; }

        public DateTime? Start1 { get; set; }

        public DateTime? End1 { get; set; }

        public int? Late1 { get; set; }

        [StringLength(50)]
        public string LateDesc1 { get; set; }

        public double? LateVal1 { get; set; }

        [StringLength(50)]
        public string LateFlag1 { get; set; }

        public double? LateAm1 { get; set; }

        public int? Early1 { get; set; }

        [StringLength(50)]
        public string DepDesc1 { get; set; }

        public double? DepVal1 { get; set; }

        [StringLength(50)]
        public string DepFlag1 { get; set; }

        public double? DepAm1 { get; set; }

        public int? Flag2 { get; set; }

        public DateTime? IN2 { get; set; }

        public DateTime? OUT2 { get; set; }

        public DateTime? Start2 { get; set; }

        public DateTime? End2 { get; set; }

        public int? Late2 { get; set; }

        [StringLength(50)]
        public string LateDesc2 { get; set; }

        public double? LateVal2 { get; set; }

        [StringLength(50)]
        public string LateFlag2 { get; set; }

        public double? LateAm2 { get; set; }

        public int? Early2 { get; set; }

        [StringLength(50)]
        public string DepDesc2 { get; set; }

        public double? DepVal2 { get; set; }

        [StringLength(50)]
        public string DEPFLAG2 { get; set; }

        public double? DEPAM2 { get; set; }

        public decimal WHOUR { get; set; }

        public decimal WOT { get; set; }

        public decimal NWH { get; set; }

        [StringLength(10)]
        public string OTTYPE { get; set; }

        public long? LeaveNo { get; set; }

        [StringLength(10)]
        public string SHIFT { get; set; }

        public int? STATUS { get; set; }

        public int? DAYFLAG { get; set; }

        [StringLength(500)]
        public string REMARK { get; set; }

        [StringLength(15)]
        public string USERLCK { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        public DateTime? WIN1 { get; set; }

        public DateTime? WOUT1 { get; set; }

        public DateTime? WIN2 { get; set; }

        public DateTime? WOUT2 { get; set; }

        public int? WIN_LATE1 { get; set; }

        public int? WOUT_EALY1 { get; set; }

        public int? WIN_LATE2 { get; set; }

        public int? WOUT_EALY2 { get; set; }

        [StringLength(50)]
        public string LeaveDesc { get; set; }

        [StringLength(10)]
        public string GMSTATUS { get; set; }

        [StringLength(500)]
        public string Remark2 { get; set; }

        [StringLength(15)]
        public string USERDEL { get; set; }

        public DateTime? DATEDEL { get; set; }

        [StringLength(50)]
        public string RECKEY { get; set; }

        public int BreakFast { get; set; }

        public int Lunch { get; set; }

        public int Dinner { get; set; }

        public int NightMeal { get; set; }

        public bool? GEN { get; set; }

        public int Meal { get; set; }

        public decimal OTApproval { get; set; }
        public decimal OTRequest { get; set; }
        public decimal WOTMin { get; set; }
        public DateTime? OTStart { get; set; }
        public DateTime? OTEnd { get; set; }
        public string OTReason { get; set; }
        public string WokingHour { get; set; }
        public decimal? ActWHour { get; set; }
        public int? OTFood { get; set; }
        public DateTime? NSWStart { get; set; }
        public DateTime? NSWEnd { get; set; }
    }
}