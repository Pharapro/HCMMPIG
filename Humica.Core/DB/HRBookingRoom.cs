namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRBookingRoom")]
    public partial class HRBookingRoom
    {
        [Key]
        [StringLength(25)]
        public string BookingNo { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(200)]
        public string EmpName { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        public decimal TotalHour { get; set; }

        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(15)]
        public string CreateedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(15)]
        public string ChangedBy { get; set; }

        public DateTime? ChangedOn { get; set; }
        public string Comment { get; set; }
    }
}
