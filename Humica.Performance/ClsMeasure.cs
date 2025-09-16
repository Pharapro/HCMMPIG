using System.Collections.Generic;

namespace Humica.Performance
{
    public class ClsMeasure
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public static List<ClsMeasure> LoadDataMeasure()
        {
            List<ClsMeasure> listData = new List<ClsMeasure>();
            listData.Add(new ClsMeasure() { Code = "#", Description = "#" }); //"Quantity(#)" });
            listData.Add(new ClsMeasure() { Code = "$", Description = "$" }); //"Revenue($)" });
            listData.Add(new ClsMeasure() { Code = "%", Description = "%" }); //"Percentage(%)" });
            return listData;
        }
        public static List<ClsMeasure> LoadDataSymbol()
        {
            List<ClsMeasure> listData = new List<ClsMeasure>();
            listData.Add(new ClsMeasure() { Code = ">", Description = ">" });
            listData.Add(new ClsMeasure() { Code = "<", Description = "<" });
            listData.Add(new ClsMeasure() { Code = "|<1|", Description = "|<1|" });
            return listData;
        }
        public static List<ClsMeasure> LoadDataOption()
        {
            List<ClsMeasure> listData = new List<ClsMeasure>();
            listData.Add(new ClsMeasure() { Code = "SUM", Description = "Sum" });
            listData.Add(new ClsMeasure() { Code = "AVERAGE", Description = "Average" });
            listData.Add(new ClsMeasure() { Code = "Latest", Description = "Latest" });
            return listData;
        }
        public static List<ClsMeasure> LoadDataTrasfer()
        {
            List<ClsMeasure> listData = new List<ClsMeasure>();
            listData.Add(new ClsMeasure() { Code = "STAFF", Description = "Transfer Staff" });
            //listData.Add(new ClsMeasure() { Code = "Handler", Description = "Handler" });
            //listData.Add(new ClsMeasure() { Code = "AVERAGE", Description = "Average" });
            return listData;
        }
    }
    public enum StatusMeasure
    {
        SUM, AVERAGE, Latest
    }
}
