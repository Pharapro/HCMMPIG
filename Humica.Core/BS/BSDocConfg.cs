using Humica.EF.MD;

namespace Humica.Core.BS
{
    public partial class BSDocConfg
    {
        private SMSystemEntity DB = new SMSystemEntity();
        public string POType { get; set; }
        public BSNumberRank Number { get; set; }
        public BSNumberRankItemN NumberItem { get; set; }
        public BSNumberRankNY NumberItemHasIdentity { get; set; }
        public BSNumberRankItem NumberItemHasIdentityYear { get; set; }
        public CFDocType DocConf { get; set; }
        public string Indentification { get; set; }
        public string NextNumberRank { get; set; }
        public int NextNumber { get; set; }

        public string CompanyCode { get; set; }

        //  public CFDelivery DeliveryConf { get; set; }

        public string Separated { get; set; }

        public BSDocConfg(string DocType)
        {

            //    DeliveryConf = DB.CFDeliveries.Find(DocType);
            Number = DB.BSNumberRanks.Find(DocConf.NumberRank);
            NumberItem = DB.BSNumberRankItemNs.Find(DocConf.NumberRank, DocConf.NumberRankItem);

            if (NumberItem != null)
            {
                if (NumberItem.Status == 0)
                {
                    NumberItem.Status = NumberItem.FromNumber;
                }
                else
                {
                    NumberItem.Status = NumberItem.Status + 1;
                }
                DB.Entry(NumberItem).State = System.Data.Entity.EntityState.Modified;
                int row = DB.SaveChanges();
                if (row == 1)
                {
                    this.Separated = NumberItem.Separated;
                    this.NextNumber = NumberItem.Status;
                    NextNumberRank = this.getLenOfPrefix(Number.Length, NumberItem.Status);
                    NextNumberRank = NumberItem.Prefix + NumberItem.Separated + NextNumberRank;
                }
            }


        }

        public BSDocConfg(string DocType, DocConfType Doc, bool IsRunNext = false)
        {
            if (Doc == DocConfType.Normal)
            {
                DocConf = DB.CFDocTypes.Find(DocType);
                Number = DB.BSNumberRanks.Find(DocConf.NumberRank);
                NumberItem = DB.BSNumberRankItemNs.Find(DocConf.NumberRank, DocConf.NumberRankItem);
            }


            if (IsRunNext == true)
            {
                if (NumberItem != null)
                {
                    if (NumberItem.Status == 0)
                    {
                        NumberItem.Status = NumberItem.FromNumber;
                    }
                    else
                    {
                        NumberItem.Status = NumberItem.Status + 1;
                    }
                    DB.Entry(NumberItem).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
                    if (row == 1)
                    {
                        this.Separated = NumberItem.Separated;
                        this.NextNumber = NumberItem.Status;
                        NextNumberRank = this.getLenOfPrefix(Number.Length, NumberItem.Status);
                        NextNumberRank = NumberItem.Prefix + NumberItem.Separated + NextNumberRank;
                    }
                }
            }
        }

        public BSDocConfg(string DocType, string CompanyCode, bool IsRunNext = false)
        {
            DocConf = DB.CFDocTypes.Find(DocType);
            this.CompanyCode = CompanyCode;
            Number = DB.BSNumberRanks.Find(DocConf.NumberRank);
            NumberItemHasIdentity = DB.BSNumberRankNies.Find(DocConf.NumberRank, DocConf.NumberRankItem, CompanyCode);
            if (IsRunNext == true)
            {
                if (NumberItemHasIdentity != null)
                {
                    if (NumberItemHasIdentity.Status == 0)
                    {
                        NumberItemHasIdentity.Status = NumberItemHasIdentity.FromNumber;
                    }
                    else
                    {
                        NumberItemHasIdentity.Status = NumberItemHasIdentity.Status + 1;
                    }
                    DB.Entry(NumberItemHasIdentity).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
                    if (row == 1)
                    {
                        this.Separated = NumberItemHasIdentity.Separated;
                        this.NextNumber = NumberItemHasIdentity.Status;
                        NextNumberRank = this.getLenOfPrefix(Number.Length, NumberItemHasIdentity.Status);
                        NextNumberRank = NumberItemHasIdentity.Prefix + NumberItemHasIdentity.Separated + NextNumberRank;
                    }
                }
            }
        }

        public BSDocConfg(string DocType, string CompanyCode, int Year, bool IsRunNext = false)
        {
            DocConf = DB.CFDocTypes.Find(DocType);
            this.CompanyCode = CompanyCode;
            Number = DB.BSNumberRanks.Find(DocConf.NumberRank);
            NumberItemHasIdentityYear = DB.BSNumberRankItems.Find(DocConf.NumberRank, DocConf.NumberRankItem, CompanyCode, Year);
            if (IsRunNext == true)
            {
                if (NumberItemHasIdentityYear != null)
                {
                    if (NumberItemHasIdentityYear.Status == 0)
                    {
                        NumberItemHasIdentityYear.Status = NumberItemHasIdentityYear.FromNumber;
                    }
                    else
                    {
                        NumberItemHasIdentityYear.Status = NumberItemHasIdentityYear.Status + 1;
                    }
                    DB.Entry(NumberItemHasIdentityYear).State = System.Data.Entity.EntityState.Modified;
                    int row = DB.SaveChanges();
                    if (row == 1)
                    {
                        this.Separated = NumberItemHasIdentityYear.Separated;
                        this.NextNumber = NumberItemHasIdentityYear.Status;
                        NextNumberRank = this.getLenOfPrefix(Number.Length, NumberItemHasIdentityYear.Status);
                        NextNumberRank = NumberItemHasIdentityYear.Prefix + NumberItemHasIdentityYear.Separated + NextNumberRank;
                    }
                }
            }
        }
        public string getLenOfPrefix(int len, int currentNumber)
        {
            string r = "";
            for (int i = currentNumber.ToString().Length; i < len; i++)
            {
                r = r + "0";
            }
            r = r + currentNumber.ToString();
            return r;
        }
    }
    public enum DocConfType
    {
        Normal, Sale, Delivery
    }
}