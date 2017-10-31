using AutoMapper;
using Newtonsoft.Json.Linq;
using Receiver.Mappings;
using Receiver.Models;
using System;
using System.Threading.Tasks;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize maps
            Mapper.Initialize(cfg =>
            {
                // Add profiles for simulations
                cfg.AddProfile<Rfactor2Profile>();
            });

            // Start udp receiver
            var udpReceiver = new UdpReceiver(666);
            Task.Run(udpReceiver.ListenForData);

            // Start the hub connection
            var hubSender = new HubSender("http://localhost:8081/", "telemetry");
            Task.Run(hubSender.Start);

            Console.WriteLine("Press Enter to close this window...");
            Console.ReadLine();
        }
    }
}
