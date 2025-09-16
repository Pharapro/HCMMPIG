namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpLeaveD")]
    public partial class HREmpLeaveD
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long LeaveTranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        public DateTime LeaveDate { get; set; }

        public DateTime? CutMonth { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(10)]
        public string Shift { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public decimal? LHour { get; set; }

        [StringLength(50)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
