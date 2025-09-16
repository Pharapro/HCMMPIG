using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Humica.Core.DB
{
    [Table("ExCFUploadMapping")]
    public partial class ExCFUploadMapping
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string ScreenID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Version { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string TableName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(70)]
        public string FieldName { get; set; }

        public int FiledIndex { get; set; }

        [StringLength(70)]
        public string AliasName { get; set; }


        [StringLength(150)]
        public string Caption { get; set; }

        public bool? IsHeader { get; set; }

        public bool? IsGroup { get; set; }

        public bool? IsPrimary { get; set; }
    }
}
