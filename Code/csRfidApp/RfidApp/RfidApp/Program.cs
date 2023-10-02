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
                try
                {
                    string selectedPort = ports[int.Parse(Console.ReadLine())];
                    Console.WriteLine($"selected port: {selectedPort}");
                    return selectedPort;
                }
                catch
                {
                    Console.WriteLine("Invalid option");
                }
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

        private static void SendInfo(string id)
        {
            try
            {
                Driver selectedDriver = data.Find(driver => driver.Id == id);
                Type driverType = typeof(Driver);
                Console.WriteLine(selectedDriver.Name);
                Console.WriteLine(selectedDriver.Mirror.Mirror1X);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Not a valid card!");
            }
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
