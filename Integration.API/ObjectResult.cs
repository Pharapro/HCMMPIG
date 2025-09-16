using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.API
{
    public class ObjectResult<TObject>
    {
        public TObject Result { get; set; }
        public bool success { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string message { get; set; }
        public string DocumentNo { get; set; }

    }
}
