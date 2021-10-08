using System.Collections.Generic;
using System.Net;
using System.Web.Helpers;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TG_Bot
{
    public class CurrencyPairsParser
    {
        private readonly WebClient _webClient;
        private const string Link = "https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5";

        public CurrencyPairsParser()
        {
            _webClient = new WebClient();
        }

        public string GetValuePairs(string message)
        {
            string response = _webClient.DownloadString(Link);
            var data = JsonConvert.DeserializeObject<List<JsonData>>(response);
            foreach (var jsonData in data)
            {
                if (message == jsonData.Ccy)
                {
                    return jsonData.Ccy + "/" + jsonData.Base_Ccy + " - Buy: " + jsonData.Buy + "\n"
                           + jsonData.Ccy + "/" + jsonData.Base_Ccy + " - Sell: " + jsonData.Sale;
                }
            }

            return "Some error :(";
        }
    }

    public class JsonData
    {
        public string Ccy { get; set; }
        public string Base_Ccy { get; set; }
        public float Buy { get; set; }
        public float Sale { get; set; }
    }
}