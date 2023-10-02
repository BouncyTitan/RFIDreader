using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;


internal class Program
{
    static SerialPort _serialPort;
    static List<Driver> data;

    Dictionary<string, List<string>> deviceDataMapping = new Dictionary<string, List<string>>
    {
        { "1", new List<string> { "mirrorX", "mirrorY" } },
        { "3", new List<string> { "seat_height", "seat_angle", "seat_position" } },
        { "4", new List<string> { "steering_wheelX", "steering_wheelY" } },
    };
    private static void Send(IPAddress localIp, IPAddress sendToIp, string messageText, bool acknowledge,
        UdpClient udpClient,
        IPAddress nextIpAddress, int port)
    {
        var message = new Message(localIp.ToString(), sendToIp.ToString(), messageText, acknowledge);
        var messageJson = message.ConvertToJson();

        var messageBytes = Encoding.ASCII.GetBytes(messageJson);
        udpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(nextIpAddress, port));
    }

    private static void InvalidIpError()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid IP address.");
        Console.ResetColor();
    }

    private static void AskForIpInput(int maxDevices)
    {
        Console.Write($"Enter the last index of an IP address to send a message to (1 to {maxDevices}): ");
    }

    private static void Main()
    {
        _serialPort = new SerialPort();
        _serialPort.PortName = "COM8";
        _serialPort.BaudRate = 9600;
        _serialPort.Open();
        // Don't forget to change the version number when you make changes to the code
        const double version = 2.4;

        const int maxDevices = 3;
        const string baseIp = "10.42.0";
        const int port = 25565;

        var localId = baseIp + ".3";

        if (localId == null)
        {
            Console.WriteLine("Error: No network adapters with an IPv4 address found on this system.");
            return;
        }

        var localIp = IPAddress.Parse(localId);

        var lastOctet = int.Parse(localId.Split('.')[3]);

        var nextIpAddress = IPAddress.Parse(lastOctet < maxDevices ? $"{baseIp}.{lastOctet + 1}" : $"{baseIp}.2");

        var previousIpAddress = IPAddress.Parse(lastOctet > 1 ? $"{baseIp}.{lastOctet - 1}" : $"{baseIp}.{maxDevices}");

        Console.WriteLine($"LAN Loop Communication (Version: {version})\n");
        Console.WriteLine($"Your Local IP address: {localIp}");
        Console.WriteLine($"Next IP address in the network: {nextIpAddress}");
        Console.WriteLine($"Previous IP address in the network: {previousIpAddress}\n");

        // Create a UDP client to send and receive messages
        var udpClient = new UdpClient();
        try
        {
            // Bind the client to the local endpoint for receiving messages
            var localEndPoint = new IPEndPoint(localIp, port);
            udpClient.Client.Bind(localEndPoint);

            Console.WriteLine("Waiting for incoming messages...\n");

            Task.Run(() =>
            {
                while (true)
                    try
                    {
                        var remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
                        var receivedBytes = udpClient.Receive(ref remoteEndPoint);
                        var receivedMessage = Encoding.ASCII.GetString(receivedBytes);
                        var message = new Message();
                        message.ConvertFromJson(receivedMessage);

                        if (message.IsMessageForMe(localIp.ToString()))
                        {
                            if (!message.Acknowledge)
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine($"\nMessage from [{message.SenderIp}]: {message.MessageText}\n");
                                Console.ResetColor();

                                Send(localIp, IPAddress.Parse(message.SenderIp),
                                    "[OK!]",
                                    true, udpClient, nextIpAddress,
                                    port);
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"\n{message.MessageText}\n");
                                Console.ResetColor();
                            }

                            AskForIpInput(maxDevices);
                        }
                        else
                        {
                            Send(IPAddress.Parse(message.SenderIp), IPAddress.Parse(message.TargetIp),
                                message.MessageText,
                                message.Acknowledge, udpClient, nextIpAddress,
                                port);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(e);
                        Console.ResetColor();
                        throw;
                    }
            });

            while (true)
            {
                AskForIpInput(maxDevices);

                /*
                var index = Console.ReadLine();
                var indexInt = 0;

                try
                {
                    if (index != null) indexInt = int.Parse(index);
                }
                catch
                {
                    InvalidIpError();
                    continue;
                }

                if (index is null or "" || indexInt is < 1 or > maxDevices)
                {
                    InvalidIpError();
                    continue;
                }

                var sendToIp = IPAddress.Parse($"{baseIp}.{index}");

                Console.Write("Enter the message to send: ");
                var messageText = Console.ReadLine();

                if (messageText is null or "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Message is empty.");
                    Console.ResetColor();
                    continue;
                }
                */

                var index = Console.ReadLine();
                var indexInt = 0;
                var sendToIp = IPAddress.Parse($"{baseIp}.{index}");
                var messageText = Console.ReadLine();

                Send(localIp, sendToIp, messageText, false, udpClient, nextIpAddress, port);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Message sent to {sendToIp}\n");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
    }
    class Driver
    {
        public string id { get; set; }
        public string name { get; set; }
        public int mirrorX { get; set; }
        public int mirrorY { get; set; }
        public int seat_height { get; set; }
        public int seat_angle { get; set; }
        public int seat_position { get; set; }
        public int steering_wheelX { get; set; }
        public int steering_wheelY { get; set; }
    }
}