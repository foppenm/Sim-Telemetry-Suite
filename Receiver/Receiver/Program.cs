using Autofac;
using Microsoft.Extensions.Configuration;
using System;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Setup.ConfigureMappers();
            var config = Setup.ConfigureApplicationSettings();
            var container = Setup.ConfigureDependencyInjection(config);

            using (var scope = container.BeginLifetimeScope())
            {
                var logic = scope.Resolve<ReceiverLogic>();
                logic.Start();
            }

            Console.WriteLine("#######################################################");
            Console.WriteLine("Sim Telemetry Suite - Receiver started.");
            Console.WriteLine("Press any key to shutdown...");
            Console.WriteLine("#######################################################");
            Console.ReadLine();
        }
    }
}
