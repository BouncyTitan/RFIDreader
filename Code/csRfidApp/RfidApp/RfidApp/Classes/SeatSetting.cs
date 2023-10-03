using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace RfidApp.Classes
{
    public class SeatSetting
    {
        [JsonProperty("seatHeight")]
        public int SeatHeight { get; set; }

        [JsonProperty("seatAngle")]
        public int SeatAngle { get; set; }

        [JsonProperty("seatPosition")]
        public int SeatPosition { get; set; }
    }
}
