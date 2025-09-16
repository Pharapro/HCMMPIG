namespace Humica.Core
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SyBackgroundService")]
    public partial class SyBackgroundService
    {
        public int ID { get; set; }

        public bool? IsBirthday { get; set; }

        public DateTime? BDScheduleTime { get; set; }

        public int? BDNextRunTime { get; set; }

        public bool? IsAttendance { get; set; }

        [StringLength(50)]
        public string AttendanceType { get; set; }

        public DateTime? AttScheduleTime { get; set; }

        public decimal? AttNextRunTime { get; set; }

        public DateTime? AttLastRunTime { get; set; }

        public bool? IsAttWeekly { get; set; }

        [StringLength(100)]
        public string WeeklyChatID { get; set; }

        public DateTime? WeeklyScheduleTime { get; set; }

        [StringLength(100)]
        public string ScheduleType { get; set; }

        public bool? IsAttMonthly { get; set; }

        [StringLength(100)]
        public string MonthlyChatID { get; set; }

        public DateTime? MonthlyScheduleTime { get; set; }

        public DateTime? NextMonthlySchedule { get; set; }
    }
}
