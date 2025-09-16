namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MDUploadTemplate")]
    public partial class MDUploadTemplate
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string UploadName { get; set; }

        public DateTime? UploadDate { get; set; }

        [StringLength(15)]
        public string UploadBy { get; set; }

        public string UploadByName { get; set; }

        [StringLength(10)]
        public string ScreenId { get; set; }

        [StringLength(20)]
        public string Module { get; set; }

        public string UpoadPath { get; set; }

        public long? ContentLength { get; set; }

        [StringLength(150)]
        public string ContentType { get; set; }

        public bool? IsGenerate { get; set; }

        public string Message { get; set; }

        [StringLength(30)]
        public string DocumentNo { get; set; }
    }
}
