namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATLateEarlyDeduct")]
    public partial class ATLateEarlyDeduct
    {
        [Key]
        public int ID { get; set; }
        public string EmpCode { get; set; }
        public string DeductType { get; set; }
        public DateTime DocumentDate { get; set; }
        public string LeaveCode { get; set; }
        public int IsMissScan { get; set; }
        public int IsLate_Early { get; set; }
        public decimal BeforeAmount { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public string Remark1 { get; set; }
        public bool IsTransfer { get; set; }
    }
}
