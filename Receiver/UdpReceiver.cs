using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace Receiver
{
    public class UdpReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly HubConnection _hubConnection;

        public UdpReceiver(int receiveUdpPort, string sendHubConnectionUrl)
        {
            _udpClient = new UdpClient(receiveUdpPort);
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(sendHubConnectionUrl)
                .WithConsoleLogger()
                .WithJsonProtocol()
                .WithTransport(Microsoft.AspNetCore.Sockets.TransportType.WebSockets)
                .Build();
        }

        public async Task Start()
        {
            // Start the hub connection
            await _hubConnection.StartAsync();

            // Set up UDP Listener
            await ListenForData();
        }

        private async Task ListenForData()
        {
            Console.WriteLine("Started listening to UDP packets.");

            var loop = true;
            while (loop)
            {
                try
                {
                    Console.WriteLine("Waiting for Bridge data...");

                    // Blocks until a message returns on this socket from a remote host.
                    var udpResult = await _udpClient.ReceiveAsync();
                    var receiveBytes = udpResult.Buffer;

                    Console.WriteLine("Received data:");

                    string returnData = Encoding.ASCII.GetString(receiveBytes).Trim('\0');

                    Console.WriteLine(
                        $"Message received from" +
                        $" {udpResult.RemoteEndPoint.Address.ToString()}:{udpResult.RemoteEndPoint.Port.ToString()} :\n" +
                        returnData.ToString());

                    // Send the json string to the clients
                    await _hubConnection.InvokeAsync("status", returnData);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    loop = false;
                }
            }
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }
    }
}
