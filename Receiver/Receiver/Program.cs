using Autofac;
using AutoMapper;
using Receiver.Mappings;
using System;
using System.Threading.Tasks;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Initialize Dependency Injection
            var builder = new ContainerBuilder();
            builder.RegisterType<HubSender>();
            builder.RegisterType<TrackMapGenerator>();

            // Initialize maps
            Mapper.Initialize(cfg =>
            {
                // Add profiles for simulations
                cfg.AddProfile<Rfactor2Profile>();
            });

            // Start udp receiver
            var udpReceiver = new UdpReceiver(666);
            Task.Run(udpReceiver.ListenForData);

            Console.WriteLine("Press Enter to close this window...");
            Console.ReadLine();
        }
    }
}
