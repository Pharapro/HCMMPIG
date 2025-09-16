namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATRequestOnSite")]
    public partial class ATRequestOnSite
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ID { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

		[StringLength(250)]
		public string EmployeeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

		[StringLength(20)]
		public string Unit { get; set; }

		[StringLength(250)]
		public string Location { get; set; }

		[StringLength(150)]
		public string Longitude { get; set; }

		[StringLength(150)]
		public string Latitude { get; set; }

		[Column(TypeName = "nvarchar(MAX)")]
		public string Reason { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime? StartTime { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime? EndTime { get; set; }

		[StringLength(50)]
		public string Status { get; set; }

		[Column(TypeName = "nvarchar(MAX)")]
		public string Attachment { get; set; }

		[Column(TypeName = "nvarchar(MAX)")]
		public string Comment { get; set; }

		[StringLength(50)]
		public string CreatedBy { get; set; }

		[Column(TypeName = "datetime")]
		public DateTime? CreatedOn { get; set; }

		[StringLength(50)]
		public string ChangedBy { get; set; }

		[Column(TypeName = "date")]
		public DateTime? ChangedOn { get; set; }

		[Column(TypeName = "bit")]
		public bool? IsReAlert { get; set; }

		[Column(TypeName = "date")]
		public DateTime? AlertDate { get; set; }
	}
}
