using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCodeBase
{
    class Data
    {
        public String Name { get; set; }
        public int Value { get; set; }

        public Data()
        {

        }

        public Data(String name, int value)
        {
            this.Name = name;
            this.Value = value;
        }

        public Data(DataRow row)
        {
            this.Name = Convert.ToString(row[0]);
            this.Value = Convert.ToInt32(row[1]);
        }
    }
}
