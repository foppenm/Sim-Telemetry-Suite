using System;
using System.Threading.Tasks;

namespace Receiver
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Start udp receiver
            var udpReceiver = new UdpReceiver(666, "http://localhost:58814/telemetry");
            await udpReceiver.Start();

            Console.WriteLine("Press Enter to close this window...");
            Console.ReadLine();
        }
    }
}
