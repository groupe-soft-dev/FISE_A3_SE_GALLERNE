using EasySaveV3.AppServer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySaveV3.AppServer.Services
{
    public class SocketServer
    {
        private readonly TcpListener _listener;

        private readonly List<ISocketCommandHandler> _handlers;

        public SocketServer(IEnumerable<ISocketCommandHandler> handlers, int port = 5000)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _handlers = handlers.ToList();
        }

        private string HandleCommand(string command, string? payload)
        {
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(command))
                    return handler.Handle(command, payload);
            }

            return JsonSerializer.Serialize("Commande inconnue");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _listener.Start();
            Console.WriteLine("Serveur en écoute...");

            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync(cancellationToken);
                _ = HandleClientAsync(client);
            }

            _listener.Stop();
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using var stream = client.GetStream();
            byte[] buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            var splitIndex = message.IndexOf('|');
            string command = splitIndex == -1 ? message : message[..splitIndex];
            string? payload = splitIndex == -1 ? null : message[(splitIndex + 1)..];

            string response = HandleCommand(command, payload);
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
