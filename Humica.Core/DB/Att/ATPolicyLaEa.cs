namespace Humica.Core.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ATPolicyLaEa")]
    public partial class ATPolicyLaEa
    {
        [Key]
        public int ID { get; set; }

        public string Branch { get; set; }
        public string Department { get; set; }

        public int Late1 { get; set; }

        public int Early1 { get; set; }
        public int Late2 { get; set; }

        public int Early2 { get; set; }
    }
}
