using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace SyncUI
{
    public class NamedPipeClient
    {
        private readonly string _serverName;
        private NamedPipeClientStream _pipeClient;

        public NamedPipeClient(string serverName)
        {
            _serverName = serverName;
            _pipeClient = new NamedPipeClientStream(".", _serverName, PipeDirection.InOut);
        }

        public async Task ConnectAsync()
        {
            await _pipeClient.ConnectAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await _pipeClient.WriteAsync(buffer, 0, buffer.Length);
        }

        public async Task<string> ReceiveMessageAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await _pipeClient.ReadAsync(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        public async Task<string> ReceiveAllMessagesAsync()
        {
            byte[] buffer = new byte[1024];
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                int bytesRead = await _pipeClient.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0)
                {
                    break;
                }
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            return sb.ToString();
        }

        public void Close()
        {
            _pipeClient?.Close();
        }
    }
}
