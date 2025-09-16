using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.API
{
    public class ObjectResults<TObject>
    {

        public ICollection<TObject> result { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public bool success { get; set; }
    }
}
