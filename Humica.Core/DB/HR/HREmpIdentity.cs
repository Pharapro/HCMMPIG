namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpIdentity")]
    public partial class HREmpIdentity
    {
        [Key]
        public int TranNo { get; set; }
        public int LineItem { get; set; }
        [StringLength(20)]
        public string CompanyCode { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string IdentityTye { get; set; }

        [StringLength(20)]
        public string IDCardNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        public string Attachment { get; set; }
        public string Document { get; set; }
        public bool? IsActive { get; set; }
    }
}
