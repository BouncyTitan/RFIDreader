using Newtonsoft.Json;

namespace LAN_UDP_Com.Classes;

public class Message
{
    public Message()
    {
        SenderIp = "";
        TargetIp = "";
        MessageText = "";
        Acknowledge = false;
    }

    public Message(string senderIp, string targetIp, string messageText, bool acknowledge)
    {
        SenderIp = senderIp;
        TargetIp = targetIp;
        MessageText = messageText;
        Acknowledge = acknowledge;
    }

    public string SenderIp { get; set; }
    public string TargetIp { get; set; }
    public string MessageText { get; set; }
    public bool Acknowledge { get; set; }

    public void ConvertFromJson(string json)
    {
        var message = JsonConvert.DeserializeObject<Message>(json);
        if (message == null) return;
        SenderIp = message.SenderIp;
        TargetIp = message.TargetIp;
        MessageText = message.MessageText;
        Acknowledge = message.Acknowledge;
    }

    public string ConvertToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public bool IsMessageForMe(string myIp)
    {
        return TargetIp == myIp;
    }
}