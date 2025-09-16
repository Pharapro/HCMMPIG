namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMRefCheckPerson")]
    public partial class RCMRefCheckPerson
    {
        [Key]
        [StringLength(20)]
        public string ApplicantID { get; set; }

        [StringLength(70)]
        public string Name { get; set; }

        public DateTime? RefChkDate { get; set; }

        [StringLength(70)]
        public string NameOfRef { get; set; }

        [StringLength(70)]
        public string OccupationOfRef { get; set; }

        [StringLength(70)]
        public string CompanyOfRef { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        [StringLength(250)]
        public string ReasonForLeaving { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }

        [StringLength(100)]
        public string Relationship { get; set; }
        
        [StringLength(100)]
        public string CompanyCan { get; set; }
        
        [StringLength(100)]
        public string PositionCan { get; set; }

        public string Description { get; set; }
    }
}
