namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAWorkHistory")]
    public partial class RCMAWorkHistory
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [StringLength(50)]
        public string Company { get; set; }

        [StringLength(50)]
        public string Position { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(70)]
        public string SupervisorName { get; set; }

        [StringLength(20)]
        public string SupervisorPhone { get; set; }

        [StringLength(100)]
        public string LeaveReason { get; set; }

        [StringLength(250)]
        public string Duties { get; set; }

        public decimal? StartSalary { get; set; }

        public decimal? EndSalary { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }
    }
}
