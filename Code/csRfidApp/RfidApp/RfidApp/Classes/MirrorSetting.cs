using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RfidApp.Classes
{

    public class MirrorSetting
    {
        [JsonProperty("mirror1X")]
        public int Mirror1X { get; set; }

        [JsonProperty("mirror1Y")]
        public int Mirror1Y { get; set; }

        [JsonProperty("mirror2X")]
        public int Mirror2X { get; set; }

        [JsonProperty("mirror2Y")]
        public int Mirror2Y { get; set; }
    }
}


