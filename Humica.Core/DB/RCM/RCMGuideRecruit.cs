namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMGuideRecruit")]
    public partial class RCMGuideRecruit
    {
        [Key]
        [StringLength(10)]
        public string GuideRecruitNo { get; set; }

        [StringLength(10)]
        public string DEPT { get; set; }

        [StringLength(15)]
        public string Status { get; set; }

        public int StaffRequestNo { get; set; }

        public int PositionRequest { get; set; }

        [StringLength(5)]
        public string Gender { get; set; }

        [StringLength(10)]
        public string WorkingType { get; set; }

        public DateTime? RequestedDate { get; set; }

        [StringLength(100)]
        public string Attachment { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
