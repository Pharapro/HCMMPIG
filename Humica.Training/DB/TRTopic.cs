namespace Humica.Training.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRTopic")]
    public partial class TRTopic
    {
        [Key]
        public int TrainNo { get; set; }

        [StringLength(15)]
        public string Code { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string SecondDescription { get; set; }

        [StringLength(500)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(250)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
