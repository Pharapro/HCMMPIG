namespace Humica.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TREmpSurRating")]
    public partial class TREmpSurRating
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(30)]
        public string SurveyID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RatingID { get; set; }

        [Required]
        [StringLength(20)]
        public string Region { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public int Rating { get; set; }
    }
}
