namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EOBEmpHealthCheckUp")]
    public partial class EOBEmpHealthCheckUp
    {
        [Key]
        [StringLength(10)]
        public string EmpCode { get; set; }

        [StringLength(10)]
        public string HealthCheckUpType { get; set; }

        [StringLength(20)]
        public string HospitalName { get; set; }

        [StringLength(250)]
        public string HospitalAddr { get; set; }

        [StringLength(60)]
        public string DrName { get; set; }

        [StringLength(20)]
        public string PhoneNo { get; set; }

        public DateTime? ChkUpDate { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string AttachmentRef { get; set; }

        public DateTime DocDate { get; set; }

        public bool? Checked { get; set; }

        [StringLength(10)]
        public string AckBy { get; set; }

        [StringLength(10)]
        public string CheckedBy { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(30)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
    }
}
