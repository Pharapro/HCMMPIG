using System;

namespace Humica.Models.Report.Payroll
{
    public partial class RPTBankABAReport : DevExpress.XtraReports.UI.XtraReport
    {
        public RPTBankABAReport()
        {
            InitializeComponent();
        }
        static string qtyLetter;
        public static string CONVERT_TO_LETTER_ENG(string NUMBER_VALUE)
        {
            string[] amount = null;
            string STR1 = "";
            string STR2 = "";
            qtyLetter = "";
            amount = NUMBER_VALUE.Split('.');
            NUMBER_TO_LETTER_ENG(Convert.ToInt32(amount[0]), Convert.ToInt32(amount[0].Length));
            STR1 = qtyLetter.Trim();
            if (amount.GetUpperBound(0) >= 1)
            {
                qtyLetter = "";
                string tmp = Convert.ToInt32(amount[1]).ToString();
                NUMBER_TO_LETTER_ENG(Convert.ToInt32(tmp), Convert.ToInt32(tmp.Length));
                STR2 = qtyLetter.Trim();
                if (!string.IsNullOrEmpty(STR2))
                    STR2 = " Usd " + STR2 + " Cents Only";
                else STR1 += " Usd Only";
            }
            else STR1 += " Usd Only";

            return STR1 + STR2;
        }
        public static void NUMBER_TO_LETTER_ENG(int numDegit, int numLen)
        {

            switch (numLen)
            {
                case 1:
                    switch (numDegit)
                    {
                        case 1: qtyLetter += "One"; break;
                        case 2: qtyLetter += "Two"; break;
                        case 3: qtyLetter += "Three"; break;
                        case 4: qtyLetter += "Four"; break;
                        case 5: qtyLetter += "Five"; break;
                        case 6: qtyLetter += "Six"; break;
                        case 7: qtyLetter += "Seven"; break;
                        case 8: qtyLetter += "Eight"; break;
                        case 9: qtyLetter += "Nine"; break;
                    }
                    break;
                case 2:
                    string STR1 = null;
                    if (Convert.ToDouble(numDegit) < 20)
                    {
                        switch (Convert.ToInt32(numDegit))
                        {
                            case 10: qtyLetter = qtyLetter + "Ten "; break;
                            case 11: qtyLetter = qtyLetter + "Eleven "; break;
                            case 12: qtyLetter = qtyLetter + "Twelve "; break;
                            case 13: qtyLetter = qtyLetter + "Thirt "; break;
                            case 14: qtyLetter = qtyLetter + "Fourteen "; break;
                            case 15: qtyLetter = qtyLetter + "Fifteen "; break;
                            case 16: qtyLetter = qtyLetter + "Sixteen "; break;
                            case 17: qtyLetter = qtyLetter + "Seventeen "; break;
                            case 18: qtyLetter = qtyLetter + "Eighteen "; break;
                            case 19: qtyLetter = qtyLetter + "Nineteen "; break;
                        }
                    }
                    else
                    {
                        STR1 = Convert.ToString(numDegit);
                        switch (Convert.ToInt32(STR1.Substring(0, 1)))
                        {
                            case 1: qtyLetter = qtyLetter + "Ten "; break;
                            case 2: qtyLetter = qtyLetter + "Twenty "; break;
                            case 3: qtyLetter = qtyLetter + "Thirty "; break;
                            case 4: qtyLetter = qtyLetter + "Forty "; break;
                            case 5: qtyLetter = qtyLetter + "Fifty "; break;
                            case 6: qtyLetter = qtyLetter + "Sixty "; break;
                            case 7: qtyLetter = qtyLetter + "Seventy "; break;
                            case 8: qtyLetter = qtyLetter + "Eighty "; break;
                            case 9: qtyLetter = qtyLetter + "Ninety "; break;
                        }
                        NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(1, 1)), 1);
                    }

                    break;
                case 3:

                    STR1 = Convert.ToString(numDegit);
                    switch (Convert.ToInt32(STR1.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "One"; break;
                        case 2: qtyLetter += "Two"; break;
                        case 3: qtyLetter += "Three"; break;
                        case 4: qtyLetter += "Four"; break;
                        case 5: qtyLetter += "Five"; break;
                        case 6: qtyLetter += "Six"; break;
                        case 7: qtyLetter += "Seven"; break;
                        case 8: qtyLetter += "Eight"; break;
                        case 9: qtyLetter += "Nine"; break;
                    }
                    qtyLetter += " Hundred ";
                    string STR2 = Convert.ToInt32(STR1.Substring(1, 2)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(1, 2)), Convert.ToInt32(STR2.Length));
                    break;
                case 4:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    switch (Convert.ToInt32(STR1.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "One"; break;
                        case 2: qtyLetter += "Two"; break;
                        case 3: qtyLetter += "Three"; break;
                        case 4: qtyLetter += "Four"; break;
                        case 5: qtyLetter += "Five"; break;
                        case 6: qtyLetter += "Six"; break;
                        case 7: qtyLetter += "Seven"; break;
                        case 8: qtyLetter += "Eight"; break;
                        case 9: qtyLetter += "Nine"; break;
                    }
                    qtyLetter += " Thousand ";
                    STR2 = Convert.ToInt32(STR1.Substring(1, 3)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(1, 3)), Convert.ToInt32(STR2.Length));
                    break;
                case 5:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(0, 2)), 2);

                    qtyLetter += " Thousand ";
                    //MsgBox(Len(str1))
                    STR2 = Convert.ToInt32(STR1.Substring(2, 3)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(2, 3)), Convert.ToInt32(STR2.Length));
                    break;
                case 6:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(0, 3)), 3);

                    qtyLetter += " Thousand ";
                    STR2 = Convert.ToInt32(STR1.Substring(3, 3)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(3, 3)), Convert.ToInt32(STR2.Length));
                    break;
                case 7:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    //Return_Letter(Val(STR1.Substring(0, 4)), 4)
                    switch (Convert.ToInt32(STR1.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "One"; break;
                        case 2: qtyLetter += "Two"; break;
                        case 3: qtyLetter += "Three"; break;
                        case 4: qtyLetter += "Four"; break;
                        case 5: qtyLetter += "Five"; break;
                        case 6: qtyLetter += "Six"; break;
                        case 7: qtyLetter += "Seven"; break;
                        case 8: qtyLetter += "Eight"; break;
                        case 9: qtyLetter += "Nine"; break;
                    }
                    qtyLetter += " Million ";
                    STR2 = Convert.ToInt32(STR1.Substring(1, 6)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(1, 6)), Convert.ToInt32(STR2.Length));
                    break;
                case 8:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(0, 2)), 2);

                    qtyLetter += " Million ";
                    STR2 = Convert.ToInt32(STR1.Substring(2, 6)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(2, 6)), Convert.ToInt32(STR2.Length));
                    break;
                case 9:
                    STR1 = null;
                    STR1 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(0, 3)), 3);

                    qtyLetter += " Million ";
                    STR2 = Convert.ToInt32(STR1.Substring(3, 6)).ToString();
                    NUMBER_TO_LETTER_ENG(Convert.ToInt32(STR1.Substring(3, 6)), Convert.ToInt32(STR2.Length));
                    break;
            }

        }
        private void lblNetText_BeforePrint(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string Total = "";
            //if (GetCurrentColumnValue("Total") == null)
            //{
            //    Total = "";
            //}
            //else
            //{
            //    Total = CONVERT_TO_LETTER_ENG(GetCurrentColumnValue("Total").ToString());
            //}
            //lblNetText.Text = Total;
        }
    }
}
