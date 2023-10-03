namespace SclcClassLibrary.Classes;

public class ConfigJson
{
    public bool IsTestModeEnabled { get; set; } = false;
    public bool IsSpamModeEnabled { get; set; } = false;
    public int RetrySendMaxTryCount { get; set; } = 50;
    public int RetrySendTimerIntervalInMs { get; set; } = 300;
    public int MessageCacheSize { get; set; } = 500;
    public int MaxDeviceNumber { get; set; } = 4;
    public string BaseIpAddress { get; set; } = "10.42.0";
    public int LocalDeviceNumber { get; set; } = 1;
    public int StartingDeviceNumber { get; set; } = 1;
    public int Port { get; set; } = 25565;
}