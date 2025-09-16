namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class HR_PendingLeave
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long TranNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(140)]
        public string AllName { get; set; }

        [StringLength(10)]
        public string Sex { get; set; }

        [StringLength(100)]
        public string Department { get; set; }

        [StringLength(100)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RequestDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(250)]
        public string Reason { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(10)]
        public string LeaveCode { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Key]
        [Column(Order = 5, TypeName = "date")]
        public DateTime ToDate { get; set; }
        public double? NoDay { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    }
}
