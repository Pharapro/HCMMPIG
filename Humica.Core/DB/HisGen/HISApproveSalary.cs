namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HISApproveSalary")]
    public partial class HISApproveSalary
    {
        [Key]
        [StringLength(20)]
        public string ASNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime PayInMonth { get; set; }

        [StringLength(30)]
        public string DocumentType { get; set; }

        [StringLength(30)]
        public string Requestor { get; set; }
        public int InYear { get; set; }

        public int InMonth { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public bool IsPostGL { get; set; }
        public bool? IsPayPartial { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }
        public int? PeriodID { get; set; }
    }
}
