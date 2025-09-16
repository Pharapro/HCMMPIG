namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HREmpResign")]
    public partial class HREmpResign
    {
        public int ID { get; set; }

        [StringLength(15)]
        public string EmpCode { get; set; }

        public DateTime? DocDate { get; set; }

        public DateTime? RequestedDate { get; set; }

        [StringLength(250)]
        public string Reason { get; set; }

        [StringLength(250)]
        public string ReasonCEO { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(30)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
