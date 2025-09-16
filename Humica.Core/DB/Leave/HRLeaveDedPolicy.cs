namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRLeaveDedPolicy")]
    public partial class HRLeaveDedPolicy
    {
        public int ID { get; set; }

        [StringLength(10)]
        public string LeaveCode { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public int RateDeduct { get; set; }

        [StringLength(50)]
        public string WorkingType { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }
    }
}
