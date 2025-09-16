using DevExpress.Pdf;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using Humica.Core.DB;
using Humica.Core.Helper;
using Humica.EF;
using Humica.EF.MD;
using Humica.EF.Models.SY;
using Humica.EF.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Humica.Models.SY
{
    public class SYExecuteFindAndReplace
    {
        IUnitOfWork unitOfWork;
        public SYExecuteFindAndReplace()
        {
            unitOfWork = new UnitOfWork<HumicaDBContext>();
        }
        public List<object> ListObjectDictionary { get; set; }
        public string DateFormat { get; set; }
        static string qtyLetter;
        public ClsHeadDepartment HDHeader { get; set; }
        public string ExecuteFindAndReplaceDOC(string FileName, string FileSource, string ParContraon)
        {
            try
            {
                DateFormat = SYSettings.getSetting(SYSConstant.DATE_FORMAT).SettinValue;
                RichEditDocumentServer server = new RichEditDocumentServer();
                server.LoadDocument(FileName);
                Document document = server.Document;
                var txtSpeechText = document.Text;
                DateTime DateNow = DateTime.Now;
                string[] textsp = txtSpeechText.Split(new char[] { ' ', '\r', '\a', '\t', '\n' });
                if (textsp.LongLength > 0)
                {
                    foreach (string param in textsp)
                    {
                        if (param.Trim() == "") continue;
                        if (param.Substring(0, 1) == "@")
                        {
                            string strReplace = "";
                            var objParam = unitOfWork.Repository<SYEmailParameter>().FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == ParContraon);
                            if (objParam != null)
                            {
                                if (ListObjectDictionary.Count > 0)
                                {
                                    strReplace = GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                                    var spilt = param.Split('_');
                                    if (spilt.Count() == 2)
                                    {
                                        if (spilt[1] == "NUM")
                                        {
                                            var Amount = ConvertNumtoKH(strReplace);
                                            strReplace = Amount;
                                        }
                                    }
                                    else if (spilt.Count() >= 3)
                                    {
                                        if (spilt[1] == "NUM")
                                        {
                                            var Amount = Convert.ToDecimal(strReplace);
                                            if (spilt[2] == "EN")
                                                strReplace = CONVERT_TO_LETTER_ENG(Amount.ToString());
                                            else
                                                strReplace = CONVERT_TO_LETTER_KH(Amount.ToString());
                                        }
                                        else if (spilt[1] == "DATE")
                                        {
                                            if (spilt[2] == "KH")
                                                strReplace = ConvertDateKH(Convert.ToDateTime(strReplace));
                                        }
                                    }
                                }
                            }
                            else if (param == "@DateNow")
                            {
                                strReplace = DateNow.ToString(DateFormat);
                            }
                            else if (param == "@DateNowKH")
                            {
                                strReplace = DateNow.ToString(DateFormat);
                                strReplace = ConvertDateKH(Convert.ToDateTime(strReplace));
                            }
                            if (HDHeader != null)
                            {
                                if (param == "@HDName") strReplace = HDHeader.HDName;
                                else if (param == "@HDNameKH") strReplace = HDHeader.HDNameKH;
                                else if (param == "@HDPosition") strReplace = HDHeader.HDPosition;
                                else if (param == "@HDPositionKH") strReplace = HDHeader.HDPositionKH;
                            }
                            DocumentRange[] ranges = server.Document.FindAll(param, SearchOptions.None, server.Document.Range);
                            for (int i = 0; i < ranges.Length; i++)
                            {
                                if (strReplace == "null")
                                    strReplace = String.Empty;
                                server.Document.Replace(ranges[i], strReplace);
                                CharacterProperties cp = server.Document.BeginUpdateCharacters(ranges[i]);
                                // cp.BackColor = System.Drawing.Color.FromArgb(180, 201, 233);
                                server.Document.EndUpdateCharacters(cp);
                            };
                        }
                    }
                }
                //Shape
                ReadOnlyShapeCollection shapes = server.Document.Shapes.Get(server.Document.Range);
                int j;
                for (j = 0; j < shapes.Count; j++)
                {
                    Shape currentTextBox = shapes[j];
                    if (currentTextBox.TextBox != null)
                    {
                        string strReplace = "";
                        string tbText = shapes[j].TextBox.Document.GetText(shapes[j].TextBox.Document.Range);
                        var objParam = unitOfWork.Repository< SYEmailParameter>().FirstOrDefault(w => w.Parameter == tbText.Trim()
                         && w.TemplateID == ParContraon);
                        if (objParam != null)
                        {
                            if (ListObjectDictionary.Count > 0)
                            {
                                strReplace = GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                            }
                            shapes[j].TextBox.Document.Replace(shapes[j].TextBox.Document.Range, strReplace);
                        }

                    }
                }
                server.SaveDocument(FileSource, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                //  Process.Start(FileSource);
                return SYConstant.OK;
            }
            catch (Exception ex)
            {
                return "EE001";
            }
        }
        public string ExecuteFindAndReplacePDF(string FileName, string FileSource, string ParContraon)
        {
            try
            {
                RichEditDocumentServer server = new RichEditDocumentServer();
                server.LoadDocument(FileName);
                Document document = server.Document;
                var txtSpeechText = document.Text;
                DateTime DateNow = DateTime.Now;
                string[] textsp = txtSpeechText.Split(new char[] { ' ', '\r', '\a', '\t', '\n' });
                if (textsp.LongLength > 0)
                {
                    foreach (string param in textsp)
                    {
                        if (param.Trim() == "") continue;
                        if (param.Substring(0, 1) == "@")
                        {
                            string strReplace = "";
                            var objParam = unitOfWork.Repository<SYEmailParameter>().FirstOrDefault(w => w.Parameter == param
                        && w.TemplateID == ParContraon);
                            if (objParam != null)
                            {
                                if (ListObjectDictionary.Count > 0)
                                {
                                    strReplace = GetFieldValues(objParam.ObjectName, ListObjectDictionary, objParam.FieldName);
                                }
                            }
                            else if (param == "@DateNow")
                            {
                                strReplace = DateNow.ToString("dd.MM.yyyy");
                            }
                            DocumentRange[] ranges = server.Document.FindAll(param, SearchOptions.None, server.Document.Range);
                            for (int i = 0; i < ranges.Length; i++)
                            {
                                if (strReplace == "null")
                                    strReplace = String.Empty;
                                server.Document.Replace(ranges[i], strReplace);
                                CharacterProperties cp = server.Document.BeginUpdateCharacters(ranges[i]);
                                // cp.BackColor = System.Drawing.Color.FromArgb(180, 201, 233);
                                server.Document.EndUpdateCharacters(cp);
                            };
                        }
                    }
                }
                server.SaveDocument(FileSource, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);


                //  Process.Start(FileSource);
                return SYConstant.OK;
            }
            catch (Exception ex)
            {
                return "EE001";
            }
        }
        public static string ExtractRegexNSSF(string FileName, string FileSource, NSSF_Latter _NSSF, DateTime ValueDate)
        {
            try
            {
                using (PdfDocumentProcessor documentProcessor = new PdfDocumentProcessor())
                {
                    documentProcessor.LoadDocument(FileName);
                    PdfFormData formData = documentProcessor.GetFormData();
                    //1017873ថ
                    formData["comb_1"].Value = _NSSF.NSSFNo;
                    formData["Text1"].Value = _NSSF.FromDate.Value.ToString("dd");
                    formData["Text2"].Value = _NSSF.FromDate.Value.ToString("MM");
                    formData["Text3"].Value = _NSSF.FromDate.Value.ToString("yyyy");
                    formData["Text4"].Value = _NSSF.ToDate.Value.ToString("dd");
                    formData["Text5"].Value = _NSSF.ToDate.Value.ToString("MM");
                    formData["Text6"].Value = _NSSF.ToDate.Value.ToString("yyyy");
                    formData["Text7"].Value = _NSSF.CompKHM;
                    formData["Text8"].Value = _NSSF.CompAct;
                    formData["Text9"].Value = _NSSF.HDHouse;
                    formData["Text10"].Value = _NSSF.HDStreet;
                    formData["Text11"].Value = _NSSF.HDCommune;
                    formData["Text12"].Value = _NSSF.HDDistrict;
                    formData["Text13"].Value = _NSSF.HDProvince;
                    formData["Text14"].Value = "";
                    formData["Text15"].Value = _NSSF.Phone;
                    formData["Text16"].Value = _NSSF.FAX;
                    formData["Text17"].Value = _NSSF.Email;
                    formData["Text18"].Value = "";
                    formData["Text19"].Value = "";
                    formData["Text20"].Value = "";
                    formData["Text21"].Value = "";
                    formData["Text22"].Value = "";
                    formData["Text23"].Value = "";
                    formData["Text24"].Value = "";
                    formData["Text25"].Value = _NSSF.Count_Sex.ToString();
                    formData["Text26"].Value = _NSSF.Count_Female.ToString();
                    formData["Text27"].Value = _NSSF.DirName;
                    formData["Text28"].Value = _NSSF.DirPositon;
                    formData["Text29"].Value = ValueDate.ToString("dd");
                    formData["Text30"].Value = ValueDate.ToString("MM");
                    formData["Text31"].Value = ValueDate.ToString("yyyy");
                    formData["Text32"].Value = "";
                    formData["Text33"].Value = "";
                    formData["Text34"].Value = "";
                    formData["Text35"].Value = "";
                    formData["Text36"].Value = "";
                    formData["Text37"].Value = "";
                    formData["Text38"].Value = _NSSF.SOSEC.ToString();
                    formData["Text39"].Value = _NSSF.Health.ToString();
                    formData["Text40"].Value = _NSSF.PensionFund.ToString();
                    formData["Text41"].Value = ClsRounding.Rounding_UP_KH(_NSSF.SOSEC + _NSSF.Health + _NSSF.PensionFund).ToString();
                    formData["Check Box2"].Value = "Off";
                    formData["Check Box2"].Value = "Off";
                    formData["Check Box3"].Value = "Off";
                    formData["Check Box4"].Value = "Off";
                    formData["Check Box5"].Value = "Off";
                    formData["Check Box6"].Value = "Off";
                    documentProcessor.SaveDocument(FileSource);
                }

                //  Process.Start(FileSource);
                return SYConstant.OK;
            }
            catch (Exception ex)
            {
                return "EE001";
            }
        }
        public string GetFieldValues(string ObjectName, List<object> obj, string field)
        {
            foreach (var obj1 in obj)
            {
                if (obj1.GetType().Name != ObjectName.Trim()) continue;
                Dictionary<string, string> dix = new Dictionary<string, string>();
                PropertyInfo[] infos = obj1.GetType().GetProperties();
                foreach (PropertyInfo info in infos)
                {
                    if (field == info.Name)
                    {
                        var PropertyType = info.PropertyType.Name;
                        if (info.PropertyType.IsGenericType &&
                           info.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            var propertyType = info.PropertyType.GetGenericArguments()[0];
                            if (ObjectName == "ATShift" && propertyType.Name == "DateTime")
                            {
                                var GetValues = info.GetValue(obj1);
                                return Convert.ToDateTime(GetValues).ToString("h:mm");
                            }
                            if (propertyType.Name == "DateTime")
                            {
                                var GetValues = info.GetValue(obj1);
                                return Convert.ToDateTime(GetValues).ToString(DateFormat);
                            }
                            else
                                return info.GetValue(obj1).ToString();
                        }
                        else if (ObjectName == "ATShift" && PropertyType == "DateTime")
                        {
                            var GetValues = info.GetValue(obj1);
                            return Convert.ToDateTime(GetValues).ToString("h:mm");
                        }
                        else if (PropertyType == "DateTime")
                        {
                            var GetValues = info.GetValue(obj1);
                            return Convert.ToDateTime(GetValues).ToString(DateFormat);
                        }
                        else if (info.PropertyType == typeof(decimal))
                        {
                            var GetValues = info.GetValue(obj1);
                            return string.Format("{0:#,#0.00}", GetValues); // Convert.ToString(GetValues.ToString("N2"));
                        }
                        else
                            if (info.GetValue(obj1) == null)
                        {
                            return null;
                        }
                        else
                            return info.GetValue(obj1, null).ToString();
                    }
                }
            }
            return null;
        }
        public string CONVERT_TO_LETTER_ENG(string NUMBER_VALUE)
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
                    STR2 = " USD " + STR2 + " Cents Only";
                else STR1 += " USD Only";
            }
            else STR1 += " USD Only";

            return STR1 + STR2;
        }
        public void NUMBER_TO_LETTER_ENG(int numDegit, int numLen)
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

        public string CONVERT_TO_LETTER_KH(string NUMBER_VALUE)
        {
            string amount = "";
            qtyLetter = "";
            string[] strArray = NUMBER_VALUE.Split('.');
            NUMBER_TO_LETTER_KH(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[0].Length));
            string str2 = qtyLetter.Trim();
            if (strArray.GetUpperBound(0) >= 1)
            {
                qtyLetter = "";
                string str3 = Convert.ToInt32(strArray[1]).ToString();
                NUMBER_TO_LETTER_KH(Convert.ToInt32(str3), Convert.ToInt32(str3.Length));
                amount = qtyLetter.Trim();
                if (!string.IsNullOrEmpty(amount))
                {
                    if (!string.IsNullOrEmpty(amount))
                        amount = " ដុល្លារ អាមេរិក " + amount + " សេនគត់";
                    else
                        str2 += " ដុល្លារ អាមេរិកតែគត់";
                }
            }
            else
                str2 += " ដុល្លារ អាមេរិកគត់";
            return str2 + amount;
        }
        public void NUMBER_TO_LETTER_KH(int numDegit, int numLen)
        {
            string str1;
            switch (numLen)
            {
                case 1:
                    switch (numDegit)
                    {
                        case 1: qtyLetter += "មួយ"; return;
                        case 2: qtyLetter += "ពីរ"; return;
                        case 3: qtyLetter += "បី"; return;
                        case 4: qtyLetter += "បួន"; return;
                        case 5: qtyLetter += "ប្រាំ"; return;
                        case 6: qtyLetter += "ប្រាំមួយ"; return;
                        case 7: qtyLetter += "ប្រាំពីរ"; return;
                        case 8: qtyLetter += "ប្រាំបី"; return;
                        case 9: qtyLetter += "ប្រាំបួន"; return;
                        default: return;
                    }
                case 2:
                    str1 = null;
                    if (Convert.ToDouble(numDegit) < 20.0)
                    {
                        switch (Convert.ToInt32(numDegit))
                        {
                            case 10: qtyLetter += "ដប់ "; return;
                            case 11: qtyLetter += "ដប់មួយ "; return;
                            case 12: qtyLetter += "ដប់ពីរ "; return;
                            case 13: qtyLetter += "ដប់បី "; return;
                            case 14: qtyLetter += "ដប់បួន "; return;
                            case 15: qtyLetter += "ដប់ប្រាំ "; return;
                            case 16: qtyLetter += "ដប់ប្រាំមួយ "; return;
                            case 17: qtyLetter += "ដប់ប្រាំពីរ "; return;
                            case 18: qtyLetter += "ដប់ប្រាំបី "; return;
                            case 19: qtyLetter += "ដប់ប្រាំបួន "; return;
                            default: return;
                        }
                    }
                    else
                    {
                        string str2 = Convert.ToString(numDegit);
                        switch (Convert.ToInt32(str2.Substring(0, 1)))
                        {
                            case 1: qtyLetter += "ដប់ "; break;
                            case 2: qtyLetter += "ម្ភៃ "; break;
                            case 3: qtyLetter += "សាមសិប "; break;
                            case 4: qtyLetter += "សែសិប "; break;
                            case 5: qtyLetter += "ហាសិប "; break;
                            case 6: qtyLetter += "ហុកសិប "; break;
                            case 7: qtyLetter += "ចិតសិប "; break;
                            case 8: qtyLetter += "ប៉ែតសិប "; break;
                            case 9: qtyLetter += "កៅសិប "; break;
                        }
                        NUMBER_TO_LETTER_KH(Convert.ToInt32(str2.Substring(1, 1)), 1);
                        break;
                    }
                case 3:
                    string str3 = Convert.ToString(numDegit);
                    switch (Convert.ToInt32(str3.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "មួយ"; break;
                        case 2: qtyLetter += "ពីរ"; break;
                        case 3: qtyLetter += "បី"; break;
                        case 4: qtyLetter += "បួន"; break;
                        case 5: qtyLetter += "ប្រាំ"; break;
                        case 6: qtyLetter += "ប្រាំមួយ"; break;
                        case 7: qtyLetter += "ប្រាំពីរ"; break;
                        case 8: qtyLetter += "ប្រាំបី"; break;
                        case 9: qtyLetter += "ប្រាំបួន"; break;
                    }
                    qtyLetter += " រយ ";
                    string str4 = Convert.ToInt32(str3.Substring(1, 2)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str3.Substring(1, 2)), Convert.ToInt32(str4.Length));
                    break;
                case 4:
                    str1 = (string)null;
                    string str5 = Convert.ToString(numDegit);
                    switch (Convert.ToInt32(str5.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "មួយ"; break;
                        case 2: qtyLetter += "ពីរ"; break;
                        case 3: qtyLetter += "បី"; break;
                        case 4: qtyLetter += "បួន"; break;
                        case 5: qtyLetter += "ប្រាំ"; break;
                        case 6: qtyLetter += "ប្រាំមួយ"; break;
                        case 7: qtyLetter += "ប្រាំពីរ"; break;
                        case 8: qtyLetter += "ប្រាំបី"; break;
                        case 9: qtyLetter += "ប្រាំបួន"; break;
                    }
                    qtyLetter += " ពាន់ ";
                    string str6 = Convert.ToInt32(str5.Substring(1, 3)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str5.Substring(1, 3)), Convert.ToInt32(str6.Length));
                    break;
                case 5:
                    str1 = null;
                    string str7 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str7.Substring(0, 2)), 2);
                    qtyLetter += " ពាន់ ";
                    string str8 = Convert.ToInt32(str7.Substring(2, 3)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str7.Substring(2, 3)), Convert.ToInt32(str8.Length));
                    break;
                case 6:
                    str1 = null;
                    string str9 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str9.Substring(0, 3)), 3);
                    qtyLetter += " ពាន់ ";
                    string str10 = Convert.ToInt32(str9.Substring(3, 3)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str9.Substring(3, 3)), Convert.ToInt32(str10.Length));
                    break;
                case 7:
                    str1 = null;
                    string str11 = Convert.ToString(numDegit);
                    switch (Convert.ToInt32(str11.Substring(0, 1)))
                    {
                        case 1: qtyLetter += "មួយ"; break;
                        case 2: qtyLetter += "ពីរ"; break;
                        case 3: qtyLetter += "បី"; break;
                        case 4: qtyLetter += "បួន"; break;
                        case 5: qtyLetter += "ប្រាំ"; break;
                        case 6: qtyLetter += "ប្រាំមួយ"; break;
                        case 7: qtyLetter += "ប្រាំពីរ"; break;
                        case 8: qtyLetter += "ប្រាំបី"; break;
                        case 9: qtyLetter += "ប្រាំបួន"; break;
                    }
                    qtyLetter += " លាន ";
                    string str12 = Convert.ToInt32(str11.Substring(1, 6)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str11.Substring(1, 6)), Convert.ToInt32(str12.Length));
                    break;
                case 8:
                    str1 = null;
                    string str13 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str13.Substring(0, 2)), 2);
                    qtyLetter += " លាន ";
                    string str14 = Convert.ToInt32(str13.Substring(2, 6)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str13.Substring(2, 6)), Convert.ToInt32(str14.Length));
                    break;
                case 9:
                    str1 = null;
                    string str15 = Convert.ToString(numDegit);
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str15.Substring(0, 3)), 3);
                    qtyLetter += " លាន ";
                    string str16 = Convert.ToInt32(str15.Substring(3, 6)).ToString();
                    NUMBER_TO_LETTER_KH(Convert.ToInt32(str15.Substring(3, 6)), Convert.ToInt32(str16.Length));
                    break;
            }
        }
        public string ConvertDateKH(DateTime DayKH)
        {
            string str = "";
            string Result = "";
            string DayK = ConvertNumtoKH(DayKH.Day.ToString());
            if (DayKH.Month == 1) str += "ខែ មករា ឆ្នាំ ";
            else if (DayKH.Month == 2) str += "ខែ កុម្ភៈ ឆ្នាំ ";
            else if (DayKH.Month == 3) str += "ខែ មីនា ឆ្នាំ ";
            else if (DayKH.Month == 4) str += "ខែ មេសា ឆ្នាំ ";
            else if (DayKH.Month == 5) str += "ខែ ឧសភា ឆ្នាំ ";
            else if (DayKH.Month == 6) str += "ខែ មិថុនា ឆ្នាំ ";
            else if (DayKH.Month == 7) str += "ខែ កក្កដា ឆ្នាំ ";
            else if (DayKH.Month == 8) str += "ខែ សីហា ឆ្នាំ ";
            else if (DayKH.Month == 9) str += "ខែ កញ្ញា ឆ្នាំ ";
            else if (DayKH.Month == 10) str += "ខែ តុលា ឆ្នាំ ";
            else if (DayKH.Month == 11) str += "ខែ វិច្ឆិកា ឆ្នាំ ";
            else if (DayKH.Month == 12) str += "ខែ ធ្នូ ឆ្នាំ ";

            Result = "ថ្ងៃទី " + DayK + " " + str + ' ' + ConvertNumtoKH(DayKH.Year.ToString());
            return Result;
        }
        public string ConvertNumtoKH(string Num)
        {
            int length = Num.ToString().Length;
            string str1 = "";
            for (int startIndex = 0; startIndex < length; ++startIndex)
            {
                string str2 = Num.Substring(startIndex, 1);
                if (str2 == "1") str1 += "១";
                else if (str2 == "2") str1 += "២";
                else if (str2 == "3") str1 += "៣";
                else if (str2 == "4") str1 += "៤";
                else if (str2 == "5") str1 += "៥";
                else if (str2 == "6") str1 += "៦";
                else if (str2 == "7") str1 += "៧";
                else if (str2 == "8") str1 += "៨";
                else if (str2 == "9") str1 += "៩";
                else if (str2 == "0") str1 += "០";
                else if (str2 == ".") str1 += ".";
                else if (str2 == "(") str1 += "(";
                else if (str2 == ")") str1 += ")";
            }
            return str1;
        }
    }
    public class ClsHeadDepartment
    {
        public string HDName { get; set; }
        public string HDNameKH { get; set; }
        public string HDPosition { get; set; }
        public string HDPositionKH { get; set; }
        public string Sex { get; set; }
        public DateTime DateTerminate { get; set; }
    }
}