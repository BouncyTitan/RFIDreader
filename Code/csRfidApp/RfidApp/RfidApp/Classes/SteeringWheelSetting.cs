using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RfidApp.Classes
{
    public class SteeringWheelSetting
    {
        [JsonProperty("steeringWheelX")]
        public int SteeringWheelX { get; set; }

        [JsonProperty("steeringWheelY")]
        public int SteeringWheelY { get; set; }
    }
}
