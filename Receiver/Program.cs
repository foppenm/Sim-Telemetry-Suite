using System;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Start udp receiver
            var udpReceiver = new UdpReceiver(666);
            udpReceiver.Start();
            Console.WriteLine("Started listening to UDP packets.");

            Console.WriteLine("Press Enter to close this window...");
            Console.ReadLine();
        }
    }
}
