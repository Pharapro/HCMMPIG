namespace Humica.Core.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRAssetType")]
    public partial class HRAssetType
    {
        [Key]
        [StringLength(20)]
        public string AssetTypeCode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
