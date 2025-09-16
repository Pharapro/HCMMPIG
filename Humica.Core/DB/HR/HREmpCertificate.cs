namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpCertificate")]
    public partial class HREmpCertificate
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(150)]
        public string AllName { get; set; }

        [StringLength(20)]
        public string CertType { get; set; }

        [StringLength(200)]
        public string CertDesc { get; set; }

        public DateTime IssueDate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public string AttachFile { get; set; }
    }
}
