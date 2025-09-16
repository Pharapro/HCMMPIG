using Humica.Core.DB;
using Humica.Core.DB.SY;
using Humica.EF.MD;
using System;
using System.Collections.Generic;

namespace Humica.Logic.CF
{
    public class SYCurrencyObject
    {
        public HumicaDBContext DB = new HumicaDBContext();
        public SYUser User { get; set; }
        public SYUserBusiness BS { get; set; }
        public SYCurrency Header { get; set; }
        public string ScreenId { get; set; }
        public string Code { get; set; }
        public string MessageCode { get; set; }
        public bool IsInUse { get; set; }
        public bool IsEditable { get; set; }
        public List<SYCurrency> ListHeader { get; set; }
        public SYCurrencyObject()
        {
            User = SYSession.getSessionUser();
            BS = SYSession.getSessionUserBS();
        }
    }
}

public class ExchangeRate
{
    public DateTime? Date { get; set; }
    public string Key { get; set; }
    public string Unit { get; set; }
    public int Bid { get; set; }
    public int? Ask { get; set; }
    public double? Average { get; set; }
    public ExchangeRate(DateTime date, string key, string unit, int bid, int ask, double average)
    {
        Date = date;
        Key = key;
        Unit = unit;
        Bid = bid;
        Ask = ask;
        Average = average;
    }
}