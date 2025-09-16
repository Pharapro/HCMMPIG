namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRMissProvince")]
    public partial class HRMissProvince
    {
        [Key]
        public int LineItem { get; set; }

        [StringLength(20)]
        public string MissionCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime FromDay { get; set; }

        [Column(TypeName = "date")]
        public DateTime ToDay { get; set; }

        public string Location { get; set; }

        public string Maps { get; set; }

        public int? Route { get; set; }
    }
}
