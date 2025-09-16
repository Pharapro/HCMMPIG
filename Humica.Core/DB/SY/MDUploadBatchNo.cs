namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MDUploadBatchNo")]
    public partial class MDUploadBatchNo
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(25)]
        public string BathUploadNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(25)]
        public string DocumentNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }
    }
}
