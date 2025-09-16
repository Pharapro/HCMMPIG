namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATEmpActivity")]
    public partial class ATEmpActivity
    {
        [Key]
        public long TranNo { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public DateTime? ScanDate { get; set; }

        [StringLength(200)]
        public string Longitude { get; set; }

        [StringLength(200)]
        public string latitude { get; set; }

        [StringLength(250)]
        public string Attachment { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        public string MapURL { get; set; }
    }
}
