using System.Net;
using System.Text;

namespace LAN_UDP_Com.Classes;

public class Reader
{
    public Reader(Master master)
    {
        Master = master;
    }

    public Master Master { get; set; }

    private void PrintMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public bool StartAsyncRead()
    {
        var noError = true;

        Task.Run(() =>
        {
            while (noError)
                try
                {
                    var remoteEndPoint = new IPEndPoint(Master.LocalIpAddress, Master.Port);
                    var receivedBytes = Master.UdpClient.Receive(ref remoteEndPoint);
                    var receivedMessage = Encoding.ASCII.GetString(receivedBytes);
                    var message = new Message();
                    message.ConvertFromJson(receivedMessage);

                    if (message.IsMessageForMe(Master.LocalIpAddress.ToString()))
                    {
                        if (!message.Acknowledge)
                        {
                            PrintMessage($"\nMessage from [{message.SenderIp}]: {message.MessageText}\n",
                                ConsoleColor.Blue);
                            Master.Writer.SendToNext(IPAddress.Parse(message.SenderIp), $"[OK {DateTime.Now}]", true);
                        }
                        else
                        {
                            PrintMessage($"\nMessage to {message.SenderIp} is {message.MessageText}\n",
                                ConsoleColor.Green);
                        }

                        Master.AskForUserTargetInput();
                    }
                    else
                    {
                        Master.Writer.SendToNext(IPAddress.Parse(message.SenderIp), message.MessageText,
                            message.Acknowledge);
                    }
                }
                catch (Exception exception)
                {
                    Master.PrintError(exception.Message);
                    noError = false;
                }
        });

        return noError;
    }
}