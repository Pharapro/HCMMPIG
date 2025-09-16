namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRLeaveType")]
    public partial class HRLeaveType
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string OthDesc { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        public bool? CUT { get; set; }

        public int? CUTTYPE { get; set; }

        public int? WorkFlag { get; set; }

        public int? WorkDay { get; set; }

        [StringLength(20)]
        public string Foperand { get; set; }

        [StringLength(1)]
        public string Operator { get; set; }

        public decimal? Soperand { get; set; }

        public bool? LCK { get; set; }

        public bool? SVC { get; set; }

        public int? NOTSHOW { get; set; }

        public bool? Probation { get; set; }

        public bool? TIP { get; set; }

        public int? BeforeDay { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public bool? IncPub { get; set; }

        public bool IsParent { get; set; }

        [StringLength(20)]
        public string Parent { get; set; }

        public decimal? Amount { get; set; }

        public bool IsCurrent { get; set; }

        public bool Allowbackward { get; set; }

        public bool ReqDocument { get; set; }

        public int NumDay { get; set; }

        public decimal Beforebackward { get; set; }
        public Nullable<bool> InRest { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        public bool IsOverEntitle { get; set; }
        [StringLength(20)]
        public string BalanceType { get; set; }
        public int MinTaken { get; set; }
        public bool? NotAllowRequest { get; set; }
    }
}
