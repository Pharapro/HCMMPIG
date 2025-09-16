namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATDevSetting")]
    public partial class ATDevSetting
    {
        [Key]
        [StringLength(10)]
        public string DeviceID { get; set; }

        public int? MachNo { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int? Status { get; set; }

        public int? Trantype { get; set; }

        [StringLength(50)]
        public string IPAddress { get; set; }

        public int Port { get; set; }

        [StringLength(5)]
        public string DevType { get; set; }

        public int? CPassword { get; set; }

        [StringLength(30)]
        public string CreateBy { get; set; }

        public DateTime? CreateOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
