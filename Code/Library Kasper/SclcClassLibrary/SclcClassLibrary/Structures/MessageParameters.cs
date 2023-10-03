using SclcClassLibrary.Classes;
using SclcClassLibrary.Enums;

namespace SclcClassLibrary.Structures;

public struct MessageParameters
{
    public string Id { get; set; }
    public string Text { get; set; }
    public string Sender { get; set; }
    public string Target { get; set; }
    public DateTime Time { get; set; }
    public MessageType MessageType { get; set; }
    public MessageSendStatus MessageSendStatus { get; set; }
    public int AcknowledgeTryCount { get; set; }

    public MessageParameters(
        string id = "",
        string text = "",
        string sender = "",
        string target = "",
        DateTime? time = null,
        MessageType messageType = MessageType.Normal,
        MessageSendStatus messageSendStatus = MessageSendStatus.AwaitingAcknowledge,
        int acknowledgeTryCount = 0)
    {
        Id = id;
        Text = text;
        Sender = sender;
        Target = target;
        Time = time ?? DateTime.Now;
        MessageType = messageType;
        MessageSendStatus = messageSendStatus;
        AcknowledgeTryCount = acknowledgeTryCount;
    }

    public MessageParameters(Message message)
    {
        Id = message.Id;
        Text = message.Text;
        Sender = message.Sender;
        Target = message.Target;
        Time = message.Time;
        MessageType = message.MessageType;
        MessageSendStatus = message.MessageSendStatus;
        AcknowledgeTryCount = message.AcknowledgeTryCount;
    }
}