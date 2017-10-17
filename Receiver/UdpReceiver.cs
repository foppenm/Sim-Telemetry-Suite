using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class UdpReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;

        public UdpReceiver(int udpPort)
        {
            _udpClient = new UdpClient(udpPort);
        }

        public void Start()
        {
            // Set up UDP Listener
            Task.Run(ListenForData);
        }

        private async Task ListenForData()
        {
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

                    // TODO: send the json string to the clients
                    //await _telemetryHubContext.Clients.All.InvokeAsync("status", returnData);
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
