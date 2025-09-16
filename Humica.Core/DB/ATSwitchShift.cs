namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATSwitchShift")]
    public partial class ATSwitchShift
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string FromEmpCode { get; set; }
        [StringLength(200)]
        public string FromEmployeeName { get; set; }

        [StringLength(20)]
        public string ToEmpCode { get; set; }
        [StringLength(200)]
        public string ToEmployeeName { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDate { get; set; }

        [StringLength(20)]
        public string FromShiftCode { get; set; }

        [StringLength(20)]
        public string ToShiftCode { get; set; }

        [Column(TypeName = "text")]
        public string Reason { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
