using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RfidApp.Classes
{
    public class UserSetting
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public MirrorSetting Mirror { get; set; }
        public SeatSetting Seat { get; set; }
        public SteeringWheelSetting SteeringWheel { get; set; }
    }
}
