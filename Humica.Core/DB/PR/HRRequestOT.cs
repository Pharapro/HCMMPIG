namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRRequestOT")]
    public partial class HRRequestOT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string OTRNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string AllName { get; set; }
        [StringLength(150)]
        public string Department { get; set; }

        [StringLength(150)]
        public string Position { get; set; }


        public DateTime OTStartTime { get; set; }

        public DateTime OTEndTime { get; set; }

        public decimal? Hours { get; set; }

        [StringLength(150)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public decimal BreakTime { get; set; }
        [StringLength(20)]
        public string RequestBy { get; set; }
        public DateTime? OTActStart { get; set; }
        public DateTime? OTActEnd { get; set; }
        public decimal? OTActual { get; set; }
        public int? ReferenceNo { get; set; }
        public string OTCategory { get; set; }
    }
}
