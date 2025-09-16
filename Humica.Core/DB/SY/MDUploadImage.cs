namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MDUploadImage")]
    public partial class MDUploadImage
    {
        [Key]
        [StringLength(20)]
        public string TokenCode { get; set; }

        [StringLength(250)]
        public string UploadName { get; set; }

        public DateTime UploadDate { get; set; }

        [StringLength(10)]
        public string UploadBy { get; set; }

        [StringLength(10)]
        public string ScreenId { get; set; }

        [StringLength(20)]
        public string Module { get; set; }

        public string UpoadPath { get; set; }

        public long ContentLength { get; set; }

        [StringLength(150)]
        public string ContentType { get; set; }

        [StringLength(1)]
        public string Remove { get; set; }
    }
}
