namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class NSSF_Latter
    {
        [StringLength(100)]
        public string CompKHM { get; set; }

        [StringLength(500)]
        public string CompAct { get; set; }

        [StringLength(500)]
        public string DirName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string HDStreet { get; set; }

        [StringLength(500)]
        public string HDCommune { get; set; }

        [StringLength(500)]
        public string HDDistrict { get; set; }

        [StringLength(500)]
        public string HDProvince { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(50)]
        public string FAX { get; set; }

        public decimal? Health { get; set; }

        public decimal? SOSEC { get; set; }

        public decimal? PensionFund { get; set; }

        public int? Count_Sex { get; set; }

        public int? Count_Female { get; set; }

        [StringLength(250)]
        public string NSSFNo { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long INYear { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int INMonth { get; set; }

        [StringLength(10)]
        public string Branch { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        [StringLength(50)]
        public string HDHouse { get; set; }

        [StringLength(200)]
        public string DirPositon { get; set; }
    }
}
