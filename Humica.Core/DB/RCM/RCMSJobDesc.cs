namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RCMSJobDesc")]
    public partial class RCMSJobDesc
    {
        [Key]
        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public string JobResponsibility { get; set; }

        public string JobRequirement { get; set; }

        [StringLength (100)]
        public string Position { get; set; }
    }
}
