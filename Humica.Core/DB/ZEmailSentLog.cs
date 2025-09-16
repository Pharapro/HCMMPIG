using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Humica.Core.DB
{
    [Table("ZEmailSentLog")]
    public partial class ZEmailSentLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(255)]
        public string SMTPHostName { get; set; }

        [StringLength(255)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Password { get; set; }

        public int? SMTPPort { get; set; }

        public bool IsEnableSSL { get; set; }

        [StringLength(255)]
        public string FromAddress { get; set; }

        [StringLength(255)]
        public string ToAddress { get; set; }

        [StringLength(255)]
        public string CcAddress { get; set; }

        [StringLength(255)]
        public string Subject { get; set; }

        [StringLength(int.MaxValue)]
        public string Body { get; set; }

        [StringLength(int.MaxValue)]
        public string AttachFile { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public int TryAttemp { get; set; }

        [StringLength(10)]
        public string CompanyCode { get; set; }

        [StringLength(50)]
        public string RefNumber { get; set; }

        [StringLength(10)]
        public string ScreenID { get; set; }

        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(10)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(20)]
        public string StmpObject { get; set; }

        public bool? Status { get; set; }

        [StringLength(20)]
        public string StateName { get; set; }

        public string Message { get; set; }
    }
}
