using System.Net.Sockets;
using System.Text;

namespace CSharpChatSystem.Shared;

public static class TcpHelpers
{
    public static async Task WriteMessage(string message, NetworkStream stream)
    {
        var messageLen = message.Length;
        var messageLenBeBytes = IntToBeBytes(messageLen);
        var messageBytes = Encoding.UTF8.GetBytes(message);

        await stream.WriteAsync(messageLenBeBytes);
        await stream.WriteAsync(messageBytes);
    }

    public static async Task<string> ReadMessage(NetworkStream stream)
    {
        var messageByteLen = new byte[4];
        _ = await stream.ReadAsync(messageByteLen);
        var lenOfMessage = BitConverter.ToInt32(messageByteLen);

        var messageBytes = new byte[lenOfMessage];
        _ = await stream.ReadAsync(messageBytes);
        var message = Encoding.UTF8.GetString(messageBytes);

        return message;
    }
    
    private static byte[] IntToBeBytes(int value)
    {
        var bytes = BitConverter.GetBytes(value);
        
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(bytes);
        }
        return bytes;
    }
}