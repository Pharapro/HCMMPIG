namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAssetLocation")]
    public partial class HRAssetLocation
	{
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
		public string Remark { get; set; }
	}
}
