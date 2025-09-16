namespace Humica.Core.DB.SY
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SYCurrency")]
    public partial class SYCurrency
    {
        public int ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        [StringLength(50)]
        public string Key { get; set; }

        [StringLength(50)]
        public string Unit { get; set; }

        public int BID { get; set; }

        public int? Ask { get; set; }

        public double? Average { get; set; }
    }
}
