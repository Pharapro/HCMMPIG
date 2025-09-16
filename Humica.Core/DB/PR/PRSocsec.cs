namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PRSocsec")]
    public partial class PRSocsec
    {
        public long ID { get; set; }

        public decimal? Grwagef { get; set; }

        public decimal? GrwaGet { get; set; }

        public decimal? AvgGr { get; set; }

        public decimal? ContriBut { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public int? HealthComp { get; set; }

        public int? HealthStaff { get; set; }
    }
}
