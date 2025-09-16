namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressDeduct")]
    public partial class HRDressDeduct
    {
        [Key]
        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(50)]
        public string AllName { get; set; }

        [StringLength(50)]
        public string Branch { get; set; }

        public DateTime DeductDate { get; set; }

        public decimal DedAm { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(255)]
        public string PathFile { get; set; }
    }
}
