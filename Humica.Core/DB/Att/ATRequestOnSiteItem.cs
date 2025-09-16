namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATRequestOnSiteItem")]
    public partial class ATRequestOnSiteItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LineItem { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InDate { get; set; }

        [StringLength(250)]
        public string Location { get; set; }

        [StringLength(150)]
        public string Longitude { get; set; }

        [StringLength(150)]
        public string Latitude { get; set; }
        public string MapURL { get; set; }
    }
}
