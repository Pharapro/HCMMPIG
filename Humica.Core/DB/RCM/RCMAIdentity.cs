namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMAIdentity")]
    public partial class RCMAIdentity
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string IdentityType { get; set; }

        [StringLength(20)]
        public string IDCardNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? IssueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }
    }
}
