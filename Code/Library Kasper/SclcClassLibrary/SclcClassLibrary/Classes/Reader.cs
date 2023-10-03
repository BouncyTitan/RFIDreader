using System.Net;
using System.Text;
using SclcClassLibrary.Enums;

namespace SclcClassLibrary.Classes;

public abstract class Reader
{
    // Returns a message object if a message is received and is for the current computer, else returns null
    public static Message? HandleIncomingMessage(Config config)
    {
        try
        {
            // Receive the message
            var remoteEndPoint = new IPEndPoint(config.LocalIpAddress, config.Port);
            var receivedBytes = config.UdpClient.Receive(ref remoteEndPoint);
            var receivedMessage = Encoding.ASCII.GetString(receivedBytes);
            var message = Message.DeserializeFromJson(receivedMessage);

            // If message is null return null
            if (message == null) return null;

            // If message is for me
            if (message.IsMessageForMe(config.LocalIpAddress.ToString()))
            {
                // If message is an acknowledge message else send an acknowledge message
                if (message.MessageType != MessageType.Acknowledge)
                {
                    Writer.SendAcknowledge(config, message);
                }

                // Set the time of receiving the message
                message.Time = DateTime.Now;
                return message;
            }

            // If message is not for me send it to the next computer in the network
            Writer.SendToNextDevice(config, message);

            return null;
        }
        catch
        {
            // TODO: Handle exception
            return null;
        }
    }
}