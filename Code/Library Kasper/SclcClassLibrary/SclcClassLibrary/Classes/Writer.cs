using System.Net;
using System.Text;
using SclcClassLibrary.Enums;
using SclcClassLibrary.Structures;

namespace SclcClassLibrary.Classes;

public abstract class Writer
{
    public static void SendAcknowledge(Config config, Message message)
    {
        var messageParameters = new MessageParameters(
            text: message.Id,
            sender: config.LocalIpAddress.ToString(),
            target: message.Sender,
            messageType: MessageType.Acknowledge
        );

        var acknowledgeMessage = new Message(messageParameters);

        SendToNextDevice(config, acknowledgeMessage);
    }

    // Send a message to the next computer in the network
    public static void SendToNextDevice(Config config, Message message)
    {
        // Create the JSON message 
        try
        {
            var messageJson = message.SerializeToJson();

            // Send the message to the next computer in the network
            var messageBytes = Encoding.ASCII.GetBytes(messageJson);
            config.UdpClient.Send(messageBytes, messageBytes.Length, new IPEndPoint(config.NextIpAddress, config.Port));
        }
        catch
        {
            // TODO: Handle exception
        }
    }
}