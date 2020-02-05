using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase.helpers.api
{
    class CandleStick
    {
        public long date { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public double close { get; set; }
        public double volume { get; set; }
        public double quoteVolume { get; set; }
        public double weightedAverage { get; set; }

        public override string ToString()
        {
            return date + "|" + close;
        }
    }
}
