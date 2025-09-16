namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HREmpCareerBankList")]
    public partial class HREmpCareerBankList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string Company { get; set; }

        public decimal? NewSalary { get; set; }

        public decimal? OldSalary { get; set; }

        public decimal? Increase { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Todate { get; set; }

        public long? Reference { get; set; }
        
    }
}
