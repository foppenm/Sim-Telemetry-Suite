using Newtonsoft.Json;
using Receiver.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    /// <summary>
    /// This component receives UDP messages and writes them to a global list of messages.
    /// It will manage this list by keeping a certain amount of recent messages available.
    /// Older messages will be removed from the global messages container.
    /// </summary>
    public class UdpReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;
        private readonly HubSender _hubSender;

        public UdpReceiver(int receiveUdpPort)
        {
            _udpClient = new UdpClient(receiveUdpPort);
            _hubSender = new HubSender("http://localhost:8081/", "telemetry");
        }

        public async Task ListenForData()
        {
            await _hubSender.Start();

            Console.WriteLine("Started listening to UDP packets.");
            TrackMapGenerator trackMapGen = new TrackMapGenerator();

            while (true)
            {
                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    var udpResult = await _udpClient.ReceiveAsync();
                    var json = Encoding.UTF8.GetString(udpResult.Buffer).Trim('\0');
                    var track = JsonConvert.DeserializeObject<Track>(json);

                    // Process the received message
                    trackMapGen.ProcessTrackMessage(_hubSender, track);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Dispose()
        {
            _udpClient.Dispose();
        }
    }
}
