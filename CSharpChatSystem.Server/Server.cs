using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using CSharpChatSystem.Shared;

using static CSharpChatSystem.Shared.Config;

namespace CSharpChatSystem.Server;

public class Server
{
    private ConcurrentDictionary<string, TcpClient> _clients = new();

    public async Task RunServer()
    {
        var listener = new TcpListener(IP_ADDR, PORT);
        listener.Start();
        
        Logger.LogInformation("Server started waiting for connections");
        
        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = Task.Run(async () => await HandleConnection(client));
        }
    }

    private async Task HandleConnection(TcpClient tcpClient)
    {
        Logger.LogInformation("Client connected");
        var stream = tcpClient.GetStream();

        var name = (await TcpHelpers.ReadMessage(stream)).Trim();
        if (_clients.ContainsKey(name))
        {
            Logger.LogInformation($"Name {name} already taken");
            await TcpHelpers.WriteMessage($"Name {name} already taken", stream);
            tcpClient.Close();
            return;
        }
        _clients.TryAdd(name, tcpClient);
        
        await TcpHelpers.WriteMessage($"Welcome {name}", stream);
        Logger.LogInformation($"{name} joined the chat");
        
        await BroadcastMessage($"{name} joined the chat");
        Logger.LogInformation("Broadcast message joined the chat to all clients");
        
        while (true)
        {
            var message = await TcpHelpers.ReadMessage(stream);
            if (message == "/exit")
            {
                Logger.LogInformation($"Client {name} disconnecting...");
                _clients.TryRemove(name, out _);
                tcpClient.Close();
                break;
            }
            await BroadcastMessage($"{name}: {message}");
        }
    }
    
    private async Task BroadcastMessage(string message)
    {
        foreach (var client in _clients)
        {
            await TcpHelpers.WriteMessage(message, client.Value.GetStream());
        }
    }

}