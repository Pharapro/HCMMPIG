namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpFamily")]
    public partial class HREmpFamily
    {
        [Key]
        public long TranNo { get; set; }

        public int? LineItem { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string RelCode { get; set; }

        [StringLength(75)]
        public string RelName { get; set; }

        [StringLength(15)]
        public string Sex { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(20)]
        public string IDCard { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        public bool? TaxDeduc { get; set; }

        public bool? InSchool { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        public string AttachFile { get; set; }
        public string Document { get; set; }

        public bool Child { get; set; }

        public bool Spouse { get; set; }
        public string OtherExceptType { get; set; }

		public DateTime EffectiveDate { get; set; }

		public DateTime? ToDate { get; set; }
	}
}
