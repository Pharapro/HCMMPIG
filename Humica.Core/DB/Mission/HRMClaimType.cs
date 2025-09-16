namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRMClaimType")]
    public partial class HRMClaimType
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecDescription { get; set; }

        //[StringLength(250)]
        //public string Remark { get; set; }
    }
}
