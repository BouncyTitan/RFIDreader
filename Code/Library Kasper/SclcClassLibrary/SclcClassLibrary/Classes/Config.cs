using System.Net;
using System.Net.Sockets;

namespace SclcClassLibrary.Classes;

public class Config
{
    #region Constructor

    public Config(ConfigJson configJson)
    {
        IsTestModeEnabled = configJson.IsTestModeEnabled;
        IsSpamModeEnabled = configJson.IsSpamModeEnabled;
        RetrySendMaxTryCount = Clamp(configJson.RetrySendMaxTryCount, 1, int.MaxValue);
        RetrySendTimerIntervalInMs = Clamp(configJson.RetrySendTimerIntervalInMs, 1, int.MaxValue);
        MessageCacheSize = Clamp(configJson.MessageCacheSize, 1, int.MaxValue);
        MaxDeviceNumber = Clamp(configJson.MaxDeviceNumber, 1, int.MaxValue);
        BaseIpAddress = configJson.BaseIpAddress;
        LocalDeviceNumber = Clamp(configJson.LocalDeviceNumber, 1, MaxDeviceNumber);
        StartingDeviceNumber = Clamp(configJson.StartingDeviceNumber, 1, int.MaxValue);
        Port = Clamp(configJson.Port, 0, int.MaxValue);
        LocalIpAddress = IPAddress.None;
        NextIpAddress = IPAddress.None;
        UdpClient = new UdpClient(Port);
    }

    #endregion

    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0) return min;

        return value.CompareTo(max) > 0 ? max : value;
    }

    #region Properties

    public bool IsTestModeEnabled { get; set; }
    public bool IsSpamModeEnabled { get; set; }
    public int RetrySendMaxTryCount { get; set; }
    public int RetrySendTimerIntervalInMs { get; set; }
    public int MessageCacheSize { get; set; }
    public int MaxDeviceNumber { get; set; }
    public string BaseIpAddress { get; set; }
    public int LocalDeviceNumber { get; set; }
    public int StartingDeviceNumber { get; set; }
    public int Port { get; set; }
    public IPAddress LocalIpAddress { get; set; }
    public IPAddress NextIpAddress { get; set; }
    public UdpClient UdpClient { get; set; }

    #endregion
}