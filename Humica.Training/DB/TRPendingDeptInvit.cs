namespace Humica.Training.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRPendingDeptInvit")]
    public partial class TRPendingDeptInvit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

       
        public long DocumentNo { get; set; }

        public int InYear { get; set; }

        [StringLength(250)]
        public string CourseName { get; set; }

        [StringLength(250)]
        public string CourseCategory { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Department { get; set; }

        public string Status { get; set; }
    }
}
