namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CFExVendor")]
    public partial class CFExVendor
    {
        [Key]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(20)]
        public string Branch { get; set; }

    }
}
