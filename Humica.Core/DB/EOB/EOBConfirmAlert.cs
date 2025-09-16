namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBConfirmAlert")]
    public partial class EOBConfirmAlert
    {
        [StringLength(20)]
        public string ID { get; set; }

        [StringLength(40)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Title { get; set; }

        [StringLength(50)]
        public string SenderName { get; set; }

        public DateTime DateOfSending { get; set; }

        [StringLength(10)]
        public string SendingSelected { get; set; }

        [StringLength(40)]
        public string Remark { get; set; }

        public DateTime? JoinDate { get; set; }

        public bool? Confirmed { get; set; }

        [StringLength(100)]
        public string AttachForm { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }

        [StringLength(20)]
        public string Status { get; set; }
    }
}
