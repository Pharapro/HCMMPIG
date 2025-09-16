namespace Humica.Core.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RCMRefQuestionnaire")]
    public partial class RCMRefQuestionnaire
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ApplicantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(250)]
        public string Question { get; set; }

        [Required]
        [StringLength(250)]
        public string Answer { get; set; }
    }
}
