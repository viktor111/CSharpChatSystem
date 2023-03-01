using System.Net.Sockets;
using CSharpChatSystem.Shared;

namespace CSharpChatSystem.Client;

using static CSharpChatSystem.Shared.Config;

public class Client
{
    public async Task RunClient()
    {
        var client = new TcpClient();
        await client.ConnectAsync(IP_ADDR, PORT);
        var stream = client.GetStream();

        Console.WriteLine("Enter your name:");
        var name = Console.ReadLine();
        await TcpHelpers.WriteMessage(name, stream);

        var welcomeMessage = await TcpHelpers.ReadMessage(stream);
        Console.WriteLine(welcomeMessage);

       // _ = Task.Run(() => ReadMessages(stream));

        while (true)
        {
            var message = Console.ReadLine();
            await TcpHelpers.WriteMessage(message, stream);
        }
    }
    
    private async Task ReadMessages(NetworkStream stream)
    {
        while (true)
        {
            var message = await TcpHelpers.ReadMessage(stream);
            if(string.IsNullOrEmpty(message)) continue;
            Console.WriteLine(message);
        }
    }
}