namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HumanResourceSetting")]
    public partial class HumanResourceSetting
    {
        [Key]
        public int TranNo { get; set; }

        public int? MaleAge { get; set; }

        public int? FemaleAge { get; set; }

        public int? OpenShirtDate { get; set; }
    }
}
