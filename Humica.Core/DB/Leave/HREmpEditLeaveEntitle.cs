namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpEditLeaveEntitle")]
    public partial class HREmpEditLeaveEntitle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string EmpName { get; set; }

        [StringLength(220)]
        public string Position { get; set; }

        public System.DateTime? DocumentDate { get; set; }

        [StringLength(100)]
        public string LeaveType { get; set; }

        public decimal? Balance { get; set; }
        public int? InYear { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangeOn { get; set; }
    }
}
