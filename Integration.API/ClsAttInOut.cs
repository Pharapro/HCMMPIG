using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.API
{
    public class ClsBioTime
    {
        public int count { get; set; }
        public List<ClsAttInOut> Data { get; set; }
    }
    public class ClsAttInOut
    {
        public string emp_code { get; set; }
        public int emp { get; set; }
        public DateTime upload_time { get; set; }
    }
}
