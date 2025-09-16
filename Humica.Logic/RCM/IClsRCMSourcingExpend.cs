using Humica.Core.DB;
using Humica.EF;
using Humica.EF.MD;
using System.Collections.Generic;

namespace Humica.Logic.RCM
{
    public interface IClsRCMSourcingExpend : IClsApplication
    {
        SYUser User { get; set; }
        string MessageError { get; set; }
        SYUserBusiness BS { get; set; }
        string SaleOrderNo { get; set; }
        string ScreenId { get; set; }
        string DocType { get; set; }
        RCMSourcingExpend Header { get; set; }
        List<RCMSourcingExpend> ListHeader { get; set; }
        CFDocType DocTypeObject { get; set; }
        decimal VATRate { get; set; }
        string PLANT { get; set; }
        string Token { get; set; }
        string PenaltyNo { get; set; }
        bool IsSave { get; set; }

        string CreateEX();
        string EditEX(int ID);
        string DeleteEX(int ID);
    }
}