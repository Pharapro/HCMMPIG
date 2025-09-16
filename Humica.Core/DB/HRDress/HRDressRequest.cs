namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressRequest")]
    public partial class HRDressRequest
    {
        [Key]
        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [StringLength(100)]
        public string VendorName { get; set; }

        public DateTime RequestDate { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        public int? StaffQTY { get; set; }

        [StringLength(255)]
        public string PathFile { get; set; }
    }
}
