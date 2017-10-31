using Newtonsoft.Json;
using Receiver.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Receiver
{
    public class UdpReceiver : IDisposable
    {
        private readonly UdpClient _udpClient;

        public UdpReceiver(int receiveUdpPort)
        {
            _udpClient = new UdpClient(receiveUdpPort);
        }

        public async Task ListenForData()
        {
            Console.WriteLine("Started listening to UDP packets.");

            var loop = true;
            while (loop)
            {
                try
                {
                    // Blocks until a message returns on this socket from a remote host.
                    var udpResult = await _udpClient.ReceiveAsync();
                    var json = Encoding.UTF8.GetString(udpResult.Buffer).Trim('\0');
                    var track = JsonConvert.DeserializeObject<Track>(json);

                    // Add the message to the collection
                    if (Globals.Messages.Count > 9)
                    {
                        Globals.Messages.RemoveAt(0);
                    }
                    Globals.Messages.Add(track);
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
