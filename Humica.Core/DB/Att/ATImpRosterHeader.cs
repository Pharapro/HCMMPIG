namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATImpRosterHeader")]
    public partial class ATImpRosterHeader
    {
        [Key]
        [StringLength(30)]
        public string DocumentNo { get; set; }

        [StringLength(250)]
        public string UploadName { get; set; }

        public DateTime? UploadDate { get; set; }

        [StringLength(10)]
        public string UploadBy { get; set; }

        public string UploadByName { get; set; }

        public string UpoadPath { get; set; }

        [Column(TypeName = "date")]
        public DateTime InMonth { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
