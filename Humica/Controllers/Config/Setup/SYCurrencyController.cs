using Humica.Core.DB;
using Humica.Core.DB.SY;
using Humica.EF;
using Humica.Logic.CF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Humica.Controllers.Config.Setup
{

    public class SYCurrencyController : Humica.EF.Controllers.MasterSaleController
    {

        private const string SCREEN_ID = "MDN00012";
        private const string URL_SCREEN = "/Config/Setup/SYCurrency/";
        private string Index_Sess_Obj = SYSConstant.BUSINESS_OBJECT_MODEL + SCREEN_ID;
        private string ActionName;
        private string KeyName = "ID";
        HumicaDBContext DB = new HumicaDBContext();
        public SYCurrencyController()
            : base()
        {
            this.ScreendIDControl = SCREEN_ID;
            this.ScreenUrl = URL_SCREEN;
        }


        #region "List"
        public async Task<ActionResult> Index()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            SYCurrencyObject BSM = new SYCurrencyObject();
            BSM.ListHeader = new List<SYCurrency>();
            try
            {
                string apiUrl = "https://www.nbc.gov.kh/api/exrate.php";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string xmlData = await response.Content.ReadAsStringAsync();

                        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(Newtonsoft.Json.JsonConvert.SerializeXmlNode(new System.Xml.XmlDocument() { InnerXml = xmlData }));
                        JObject exchangeRateObject = (JObject)jsonObject["ExchangeRate"];
                        JArray exArray = (JArray)exchangeRateObject["ex"];
                        List<ExchangeRate> myList = new List<ExchangeRate>();
                        foreach (var item in exArray)
                        {
                            myList.Add(new ExchangeRate((DateTime)item["date"], (string)item["key"], (string)item["unit"], (int)item["bid"], (int)item["ask"], (float)item["average"]));
                        }

                        foreach (var _obj in myList)
                        {
                            var obj = new SYCurrency();
                            obj.Date = _obj.Date;
                            obj.Ask = _obj.Ask;
                            obj.BID = _obj.Bid;
                            obj.Unit = _obj.Unit;
                            obj.Key = _obj.Key;
                            obj.Average = _obj.Average;
                            BSM.ListHeader.Add(obj);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Failed to retrieve data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("An error occurred: " + ex.Message);
            }

            return View(BSM);
        }

        #endregion

        public async Task<ActionResult> Gridview()
        {
            UserSession();
            UserConfListAndForm();
            DataSelector();
            SYCurrencyObject BSM = new SYCurrencyObject();
            BSM.ListHeader = new List<SYCurrency>();
            try
            {
                string apiUrl = "https://www.nbc.gov.kh/api/exrate.php";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string xmlData = await response.Content.ReadAsStringAsync();

                        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(Newtonsoft.Json.JsonConvert.SerializeXmlNode(new System.Xml.XmlDocument() { InnerXml = xmlData }));
                        JObject exchangeRateObject = (JObject)jsonObject["ExchangeRate"];
                        JArray exArray = (JArray)exchangeRateObject["ex"];
                        List<ExchangeRate> myList = new List<ExchangeRate>();
                        foreach (var item in exArray)
                        {
                            myList.Add(new ExchangeRate((DateTime)item["date"], (string)item["key"], (string)item["unit"], (int)item["bid"], (int)item["ask"], (float)item["average"]));
                        }

                        foreach (var _obj in myList)
                        {
                            var obj = new SYCurrency();
                            obj.Date = _obj.Date;
                            obj.Ask = _obj.Ask;
                            obj.BID = _obj.Bid;
                            obj.Unit = _obj.Unit;
                            obj.Key = _obj.Key;
                            obj.Average = _obj.Average;
                            BSM.ListHeader.Add(obj);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Failed to retrieve data. Status code: " + response.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("An error occurred: " + ex.Message);
            }
            return PartialView("Gridview", BSM.ListHeader);
        }

        private void DataSelector()
        {
        }
    }
}