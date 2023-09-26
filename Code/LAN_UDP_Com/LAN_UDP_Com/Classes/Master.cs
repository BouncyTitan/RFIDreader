using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;

namespace LAN_UDP_Com.Classes;

public class Master
{
    public const int StartingDeviceNumber = 1;
    private const string Version = "3.0.3";

    public Master(int maxDevices, string baseIpAddress, int deviceNumber, int port)
    {
        MaxDevices = maxDevices;
        BaseIpAddress = baseIpAddress;
        DeviceNumber = deviceNumber;
        Port = port;
        LocalIpAddress = IPAddress.Parse($"{BaseIpAddress}.{DeviceNumber}");
        var lastOctet = int.Parse(LocalIpAddress.ToString().Split('.').Last());
        NextIpAddress = IPAddress.Parse(lastOctet < maxDevices
            ? $"{BaseIpAddress}.{lastOctet + 1}"
            : $"{BaseIpAddress}.{StartingDeviceNumber}");
        UdpClient = new UdpClient(Port);

        Reader = new Reader(this);
        Writer = new Writer(this);
        PrintStartLog();
    }

    public int MaxDevices { get; set; }
    public string BaseIpAddress { get; set; }
    public int DeviceNumber { get; set; }
    public int Port { get; set; }
    public IPAddress LocalIpAddress { get; set; }
    public IPAddress NextIpAddress { get; set; }
    public UdpClient UdpClient { get; set; }
    public Reader Reader { get; set; }
    public Writer Writer { get; set; }

    private void PrintStartLog()
    {
        Console.WriteLine($"LAN Loop Communication (Version: {Version})\n");
        Console.WriteLine($"Your Local IP address: {LocalIpAddress}");
        Console.WriteLine($"Next IP address in the network: {NextIpAddress}");
    }

    public void PrintError(string exception)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(exception);
        Console.ResetColor();
    }

    public void AskForUserTargetInput()
    {
        Console.Write(
            $"Enter the target ID ({StartingDeviceNumber} to {MaxDevices}): ");
    }
}