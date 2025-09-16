namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRLeaveHourPolicy")]
    public partial class HRLeaveHourPolicy
    {
        [Key]
        public int TranNo { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public decimal Rate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
