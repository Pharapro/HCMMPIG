namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRdressReceive")]
    public partial class HRDressReceive
    {
        [Key]
        [StringLength(50)]
        public string DocNO { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        [StringLength(100)]
        public string EmpName { get; set; }

        [StringLength(50)]
        public string Post { get; set; }

        [StringLength(50)]
        public string Branch { get; set; }

        [StringLength(50)]
        public string DocType { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(20)]
        public string StatusTransfer { get; set; }

        [StringLength(20)]
        public string StatusDeduct { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TransferDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeductDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReceiveDATE { get; set; }

        public int? ReceiveQTY { get; set; }

        [StringLength(255)]
        public string PathFile { get; set; }

        [StringLength(225)]
        public string Remark { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string ChangedBy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ChangedOn { get; set; }

        public int? FlexStatus { get; set; }
    }
}
