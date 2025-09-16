namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRDressTransfer")]
    public partial class HRDressTransfer
    {
        [Key]
        [StringLength(50)]
        public string DocNo { get; set; }

        [StringLength(10)]
        public string FEmpCode { get; set; }

        [StringLength(10)]
        public string TEmpCode { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        //[StringLength(10)]
        //public string Dept { get; set; }

        //[StringLength(10)]
        //public string Post { get; set; }

        public DateTime TransferDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DocRef { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(255)]
        public string PathFile { get; set; }
    }
}
