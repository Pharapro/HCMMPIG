namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HRMItem")]
    public partial class HRMItem
    {
        [Key]
        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        public string SecDescription { get; set; }

        [StringLength(250)]
        public string Breakfast { get; set; }

        [StringLength(250)]
        public string Lunch { get; set; }

        [StringLength(250)]
        public string Dinner { get; set; }

        [StringLength(250)]
        public string Accommodation { get; set; }
    }
}
