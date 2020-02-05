using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperCodeBase.abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.helpers.api
{
    class PoloniexApiClient : TimedActionObject
    {
        private ApiClient ApiClient { get; set; }

        public PoloniexApiClient()
        {
            ApiClient = new ApiClient();
        }

        /// <summary>
        /// Retrieves all prices in the last 24 hours of given time.
        /// </summary>
        /// <param name="end">The time period to retrieve prices of.</param>
        public void GetPricesBtcUsdc(DateTime end)
        {
            Int32 endUnix = (Int32)(end.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int32 startUnix = (Int32)(end.AddDays(-1).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string jsonString = ApiClient.Get(@"https://poloniex.com/public?command=returnChartData&currencyPair=USDC_BTC&start=" + startUnix + "&end=" + endUnix + "&period=300");
            dynamic j = JsonConvert.DeserializeObject(jsonString);
            List<CandleStick> candleSticks = j.ToObject<List<CandleStick>>();
        }

        /// <summary>
        /// Retrieves the latest bitcoin price in USD.
        /// </summary>
        /// <returns></returns>
        public double GetLatestBtcUsdcPrice()
        {
            int candlestickPeriod = 300; // in seconds

            Int32 endUnix = (Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Int32 startUnix = endUnix - 7200;

            string jsonString = ApiClient.Get(@"https://poloniex.com/public?command=returnChartData&currencyPair=USDC_BTC&start=" + startUnix + "&end=" + endUnix + "&period=" + candlestickPeriod);
            dynamic j = JsonConvert.DeserializeObject(jsonString);
            List<CandleStick> candleSticks = j.ToObject<List<CandleStick>>();

            return candleSticks[candleSticks.Count - 1].close;
        }
    }
}
