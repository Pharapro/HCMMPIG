namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRKPIType")]
    public partial class HRKPIType
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public int? Period { get; set; }

        public bool? IsActive { get; set; }

        public decimal? KPIAverage { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangeBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangeOn { get; set; }
    }
}
