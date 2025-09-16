namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressReturn")]
    public partial class HRDressReturn
    {
        [Key]
        public int TranNo { get; set; }

        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string EmpName { get; set; }

        public DateTime ReturnDate { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(50)]
        public string DocRef { get; set; }

        public int? QTY { get; set; }

        public DateTime? AcceptDate { get; set; }

        [StringLength(20)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(255)]
        public string PathFile { get; set; }

        [StringLength(20)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
