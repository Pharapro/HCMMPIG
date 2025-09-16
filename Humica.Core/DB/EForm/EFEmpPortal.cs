namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EFEmpPortal")]
    public partial class EFEmpPortal
    {
        [Key]
        [StringLength(30)]
        public string PortalNo { get; set; }
        [StringLength(10)]
        public string PortalType { get; set; }
        [StringLength(300)]
        public string PortalName { get; set; }

        [StringLength(30)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmployeeName { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [StringLength(200)]
        public string Position { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime ExpectedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime Deadline { get; set; }
        [StringLength(10)]
        public string Status { get; set; }
        [StringLength(200)]
        public string SurveyName { get; set; }
        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        public bool? IsRead { get; set; }
    }
}
