namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("HRMaterialAccount")]
    public partial class HRMaterialAccount
    {
        [Key]
        [StringLength(30)]
        public string MaterialCode { get; set; }

        [StringLength(150)]
        public string MaterialDescription { get; set; }

        [StringLength(150)]
        public string MaterialDescription2 { get; set; }

        [StringLength(30)]
        public string ExpenseAccount { get; set; }

    }
}
