using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using RfidApp.Classes;


namespace RfidApp
{
    class Program
    {
        static SerialPort _serialPort;
        static List<Driver> data;
        public static void Main()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = "COM8";
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
            while (true)
            {
                string jsonFilePath = "C:\\Users\\jipla\\github\\RFIDreader\\Code\\data\\drivers.json";
                string jsonString = System.IO.File.ReadAllText(jsonFilePath);
                data = JsonConvert.DeserializeObject<List<Driver>>(jsonString);

                string receivedId = _serialPort.ReadLine().Trim();
                try
                {
                    Driver selectedDriver = data.Find(driver => driver.Id == receivedId);
                    Type driverType = typeof(Driver);
                    Console.WriteLine(selectedDriver.Name);
                    Console.WriteLine(selectedDriver.Mirror.Mirror1X);
                    /*
                    foreach (var propertyInfo in driverType.GetProperties())
                    {
                        var value = propertyInfo.GetValue(selectedDriver, null);
                        Console.WriteLine($"{propertyInfo.Name}: {value}");
                    }
                    */

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Not a valid card!");
                }
            }
                      
        }

    }
}
