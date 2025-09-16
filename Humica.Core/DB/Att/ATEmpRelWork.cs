namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATEmpRelWork")]
    public partial class ATEmpRelWork
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(20)]
        public string FromEmpCode { get; set; }

        [StringLength(20)]
        public string ToEmpCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime InDate { get; set; }

        [StringLength(150)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal? WorkingHour { get; set; }
    }
}
