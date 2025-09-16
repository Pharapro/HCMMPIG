namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRApprRating")]
    public partial class HRApprRating
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string AppraisalType { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public int Rating { get; set; }
    }
}
