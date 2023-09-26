using System.Net;
using System.Text;

namespace LAN_UDP_Com.Classes;

public class Writer
{
    public Writer(Master master)
    {
        Master = master;
    }

    public Master Master { get; set; }

    public void SendToNext(IPAddress sendToIp, string messageText, bool acknowledge)
    {
        var message = new Message(Master.LocalIpAddress.ToString(), sendToIp.ToString(), messageText, acknowledge);
        var messageJson = message.ConvertToJson();

        var messageBytes = Encoding.ASCII.GetBytes(messageJson);
        Master.UdpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(Master.NextIpAddress, Master.Port));
    }

    public bool StartWrite()
    {
        var noError = true;
        while (noError)
            try
            {
                string? targetId;

                try
                {
                    Master.AskForUserTargetInput(); // Prompt the user to enter the target ID
                    targetId = Console.ReadLine();
                    var targetIdInt = Convert.ToInt32(targetId);

                    // Check if targetIdInt is within the valid range
                    if (targetIdInt < Master.StartingDeviceNumber || targetIdInt > Master.MaxDevices)
                    {
                        Master.PrintError("Target ID was out of range");
                        continue;
                    }
                }
                catch (Exception exception)
                {
                    Master.PrintError(exception.Message);
                    continue;
                }

                Console.Write("Enter the message to send: ");
                var messageText = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(messageText))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Message is empty.");
                    Console.ResetColor();
                    continue;
                }

                var sendToIp = IPAddress.Parse($"{Master.BaseIpAddress}.{targetId}");

                SendToNext(sendToIp, messageText, false);
            }
            catch (Exception exception)
            {
                Master.PrintError(exception.Message);
                noError = false;
            }

        return noError;
    }
}