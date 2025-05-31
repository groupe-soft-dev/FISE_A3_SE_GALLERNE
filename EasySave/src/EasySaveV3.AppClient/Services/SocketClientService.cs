using System.Text.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using EasySaveV3.AppClient.Notifiers;

namespace EasySaveV3.AppClient.Services
{
    public class SocketClientService
    {
        public async Task<string> SendCommandAsync(string command, object? payload)
        {
            MessageBoxNotifier notif = new MessageBoxNotifier();
            using TcpClient client = new();
            await client.ConnectAsync("127.0.0.1", 5000);

            using NetworkStream stream = client.GetStream();

            // Construction du message (avec ou sans payload)
            string message;
            if (payload != null)
            {
                string jsonPayload = JsonSerializer.Serialize(payload);
                message = $"{command}|{jsonPayload}";
            }
            else
            {
                message = command;
            }
            notif.ShowWarning($"{message}");
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);

            // Lire la réponse
            byte[] buffer = new byte[4096];
            int bytesRead;
            var responseBuilder = new StringBuilder();

            do
            {
                bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                responseBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            while (stream.DataAvailable);

            return responseBuilder.ToString();
        }

    }
}
