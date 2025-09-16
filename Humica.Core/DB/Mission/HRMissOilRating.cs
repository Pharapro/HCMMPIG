namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRMissOilRating")]
    public partial class HRMissOilRating
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string CarType { get; set; }

        [StringLength(250)]
        public string Model { get; set; }

        [StringLength(250)]
        public string Horsepower { get; set; }

        public decimal? Rate { get; set; }

        public bool? IsCompany { get; set; }
    }
}
