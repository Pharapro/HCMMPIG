using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.API
{
    public class FilterRequest
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string filter { get; set; }
        public string Para { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string companyCode { get; set; }
        public string branch { get; set; }
        public string warehouseCode { get; set; }
        public string DocNumInvoice { get; set; }
    }
}
