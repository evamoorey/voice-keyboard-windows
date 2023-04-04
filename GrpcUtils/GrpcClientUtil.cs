using System.Net.Sockets;

namespace VoiceKeyboard.GrpcUtils;

public static class GrpcClientUtil
{
    public static readonly string ServerHost = "localhost";
    public static readonly int ServerPort = 50033;

    public static bool PingServer(string hostUri, int portNumber)
    {
        try
        {
            using var _ = new TcpClient(hostUri, portNumber);
            return true;
        }
        catch (SocketException)
        {
            return false;
        }
    }
}