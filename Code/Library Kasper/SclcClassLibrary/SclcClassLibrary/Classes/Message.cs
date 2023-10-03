using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using SclcClassLibrary.Enums;
using SclcClassLibrary.Structures;

namespace SclcClassLibrary.Classes;

public class Message : INotifyPropertyChanged
{
    private int _acknowledgeTryCount;
    private MessageSendStatus _messageSendStatus;

    #region Constructors

    // Create a message with default parameters
    public Message()
    {
        var messageParameters = new MessageParameters();

        Id = string.IsNullOrEmpty(messageParameters.Id) ? Guid.NewGuid().ToString() : messageParameters.Id;
        Text = messageParameters.Text;
        Sender = messageParameters.Sender;
        Target = messageParameters.Target;
        Time = messageParameters.Time;
        MessageType = messageParameters.MessageType;
        MessageSendStatus = messageParameters.MessageSendStatus;
        AcknowledgeTryCount = messageParameters.AcknowledgeTryCount;
    }

    // Create a message with the given parameters
    public Message(MessageParameters messageParameters)
    {
        Id = string.IsNullOrEmpty(messageParameters.Id) ? Guid.NewGuid().ToString() : messageParameters.Id;
        Text = messageParameters.Text;
        Sender = messageParameters.Sender;
        Target = messageParameters.Target;
        Time = messageParameters.Time;
        MessageType = messageParameters.MessageType;
        MessageSendStatus = messageParameters.MessageSendStatus;
        AcknowledgeTryCount = messageParameters.AcknowledgeTryCount;
    }

    #endregion

    #region Properties

    public string Id { get; set; }
    public string Text { get; set; }
    public string Sender { get; set; }
    public string Target { get; set; }
    public DateTime Time { get; set; }

    public MessageType MessageType { get; set; }

    public MessageSendStatus MessageSendStatus
    {
        get => _messageSendStatus;
        set
        {
            if (value == _messageSendStatus) return;
            _messageSendStatus = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MessageSendStatusString));
            OnPropertyChanged(nameof(MessageSendStatusColor));
            OnPropertyChanged(nameof(IsProgressBarVisible));
        }
    }

    public string MessageSendStatusString
    {
        get
        {
            return MessageSendStatus switch
            {
                MessageSendStatus.AwaitingAcknowledge => "Awaiting Acknowledge",
                MessageSendStatus.Acknowledged => "Acknowledged",
                MessageSendStatus.FailedToAcknowledge => "Failed To Acknowledge",
                _ => "Unknown Status"
            };
        }
    }

    public string MessageSendStatusColor
    {
        get
        {
            return MessageSendStatus switch
            {
                MessageSendStatus.AwaitingAcknowledge => "#FFFFD700",
                MessageSendStatus.Acknowledged => "#FF3CB371",
                MessageSendStatus.FailedToAcknowledge => "#FFFF4500",
                _ => "#FFFFFF"
            };
        }
    }

    public int AcknowledgeTryCount
    {
        get => _acknowledgeTryCount;
        set
        {
            if (value == _acknowledgeTryCount) return;
            _acknowledgeTryCount = value;
            OnPropertyChanged();
        }
    }

    public bool IsProgressBarVisible => MessageSendStatus == MessageSendStatus.AwaitingAcknowledge;

    #endregion

    #region Methods

    // Acknowledge the message
    public void AcknowledgeMessage()
    {
        MessageSendStatus = MessageSendStatus.Acknowledged;
    }

    public void FailedToSendMessage()
    {
        MessageSendStatus = MessageSendStatus.FailedToAcknowledge;
    }

    // Serialize the message to JSON
    public string SerializeToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public static Message? DeserializeFromJson(string json)
    {
        return JsonSerializer.Deserialize<Message>(json);
    }

    // Check if the message is for me
    public bool IsMessageForMe(string localIpAddress)
    {
        return Target == localIpAddress;
    }

    #endregion

    #region Overrides

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override string ToString()
    {
        return $"Id: {Id}, " +
               $"Text: {Text}, " +
               $"Sender: {Sender}, " +
               $"Target: {Target}, " +
               $"Time: {Time}, " +
               $"MessageType: {MessageType}, " +
               $"MessageSendStatus: {MessageSendStatus}, " +
               $"AcknowledgeTryCount: {AcknowledgeTryCount}";
    }

    #endregion
}