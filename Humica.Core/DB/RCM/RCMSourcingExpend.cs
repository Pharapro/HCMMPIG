namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RCMSourcingExpend")]
    public partial class RCMSourcingExpend
    {
        public int ID { get; set; }

        public string ExpendType { get; set; }

        [StringLength(10)]
        public string VacancyNumber { get; set; }

        public decimal Amount { get; set; }

        public string DocumentReference { get; set; }

        [Column(TypeName = "date")]
        public DateTime DocumentDate { get; set; }

        [StringLength(250)]
        public string Remark { get; set; }

        [StringLength(250)]
        public string AttachFile { get; set; }

        [StringLength(10)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(10)]
        public string ChangedBy { get; set; }

        public DateTime? ChangeDate { get; set; }
    }
}
