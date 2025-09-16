namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAssetClass")]
    public partial class HRAssetClass
    {
        [Key]
        [StringLength(20)]
        public string AssetClassCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [StringLength(20)]
        public string AssetTypeCode { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }
        [StringLength(30)]
        public string NumberRank { get; set; }
		public bool IsVehicle { get; set; }
	}
}
