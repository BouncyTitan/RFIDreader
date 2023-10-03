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
using System.Web;

namespace RfidApp
{
    class Program
    {
        static SerialPort _serialPort;
        static List<Driver> data;
        const string beginChar = "#";
        const string endChar = "\r";

        private static string SelectComPort()
        {
            string[] ports = SerialPort.GetPortNames();
            Console.WriteLine("Choose com port!");
            for (int i = 0; i < ports.Length; i++)
            {
                Console.WriteLine($"{i}: {ports[i]}");
            }

            while (true)
            {
                string option = Console.ReadLine();
                if(int.TryParse(option,out int intOption))
                {
                    if(intOption>=0&&intOption<ports.Length)
                    {
                        string selectedPort = ports[intOption];
                        Console.WriteLine($"selected port: {selectedPort}");
                        return selectedPort;
                    }
                }
               
                Console.WriteLine("Invalid option");
                
            }
        }

        private static bool IsMessageValid(string message)
        {
            if (message.StartsWith(beginChar)
                && message.EndsWith(endChar)) 
            { 
                return true; 
            }
            
            return false;
        }

        private static string parseMessage(string message)
        {
            return message.Replace(beginChar, "").Trim();
        }

        private static void SendInfo(string uid)
        {
            
            Driver selectedDriver = data.Find(driver => driver.Uid == uid);
            if (selectedDriver == null)
            {
                Console.WriteLine("Not a valid card!");
                return;
            }
            Type driverType = typeof(Driver);
            string message1 = JsonConvert.SerializeObject(selectedDriver.Mirror);
            string message2 = JsonConvert.SerializeObject(selectedDriver.Seat);
            string message3 = JsonConvert.SerializeObject(selectedDriver.SteeringWheel);

            
            
        }

        public static void Main()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = SelectComPort();
            _serialPort.BaudRate = 9600;
            _serialPort.Open();
            Console.WriteLine("Start scanning");
            while (true)
            {
                string jsonFilePath = "data\\drivers.json";
                string jsonString = System.IO.File.ReadAllText(jsonFilePath);
                data = JsonConvert.DeserializeObject<List<Driver>>(jsonString);
                string message = _serialPort.ReadLine();
                if (IsMessageValid(message))
                {
                    SendInfo(parseMessage(message));          
                }                    
            }
                      
        }

    }
}
