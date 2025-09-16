namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HISApproveSalaryFD")]
    public partial class HISApproveSalaryFD
    {
        [Key]
        [StringLength(20)]
        public string ASNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime PayInMonth { get; set; }

        [Required]
        [StringLength(30)]
        public string DocumentType { get; set; }

        [StringLength(30)]
        public string Requestor { get; set; }

        public string CompanyCode { get; set; }

        public int PeriodID { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public bool IsPostGL { get; set; }
    }
}
