using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RfidApp.Classes
{
    public class Driver
    {     
        [JsonProperty("1")]
        public MirrorSetting Mirror { get; set; }

        [JsonProperty("2")]
        public SeatSetting Seat { get; set; }

        [JsonProperty("3")]
        public SteeringWheelSetting SteeringWheel { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
