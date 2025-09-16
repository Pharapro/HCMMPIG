namespace Humica.Core
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SyServiceURL")]
    public partial class SyServiceURL
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string URLType { get; set; }

        public string URL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
