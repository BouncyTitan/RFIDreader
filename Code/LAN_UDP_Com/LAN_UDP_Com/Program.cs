using LAN_UDP_Com.Classes;
using System.Net.Sockets;
using System.Net;

namespace LAN_UDP_Com;

public class Program
{
    private static void Main()
    {
        Master master = new Master(2, "10.42.0", 1, 25565);

        try
        {
            master.Reader.StartAsyncRead();
            master.Writer.StartWrite();
        }
        catch (Exception e)
        {
            master.PrintError(e.Message);
            throw;
        }
    }
}