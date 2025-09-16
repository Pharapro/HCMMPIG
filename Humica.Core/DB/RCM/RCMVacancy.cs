namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMVacancy")]
    public partial class RCMVacancy
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(30)]
        public string DocRef { get; set; }

        [StringLength(20)]
        public string Position { get; set; }

        [StringLength(15)]
        public string VacancyType { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public DateTime? DocDate { get; set; }

        public DateTime? ClosedDate { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(10)]
        public string Dept { get; set; }

        [StringLength(20)]
        public string WorkingType { get; set; }

        public int? AppApplied { get; set; }

        [StringLength(10)]
        public string Sect { get; set; }

        [StringLength(10)]
        public string JobLevel { get; set; }

        [StringLength(10)]
        public string StaffType { get; set; }

        [StringLength(20)]
        public string ProcessBy { get; set; }

        public DateTime? ProcessDate { get; set; }

        [StringLength(20)]
        public string ClosedBy { get; set; }

        [StringLength(10)]
        public string Level { get; set; }
    }
}
