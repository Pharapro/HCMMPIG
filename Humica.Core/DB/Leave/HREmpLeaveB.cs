namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpLeaveB")]
    public partial class HREmpLeaveB
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        public int? InYear { get; set; }

        public decimal? Forward { get; set; }

        public decimal? DayEntitle { get; set; }

        public decimal? YTD { get; set; }

        public decimal? DayLeave { get; set; }

        public decimal? Balance { get; set; }

        public int? LCK { get; set; }

        public int? InMonth { get; set; }

        public DateTime? ForWardExp { get; set; }

        public decimal ForwardUse { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangeBy { get; set; }

        public DateTime? ChangeOn { get; set; }

        public decimal? ALPayBalance { get; set; }

        public decimal LToDate { get; set; }

        public DateTime? ALPayMonth { get; set; }

        public decimal Current_AL { get; set; }

        public decimal Rest_Edit { get; set; }

        public decimal PH_Edit { get; set; }

        public decimal CurrentEntitle { get; set; }

        public decimal? TakenHour { get; set; }

        public decimal? BalanceHour { get; set; }
        public decimal? SeniorityBalance { get; set; }
        public decimal? Adjustment { get; set; }
    }
}
