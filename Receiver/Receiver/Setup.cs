using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Receiver.Data;
using Receiver.Mappings;
using System.IO;

namespace Receiver
{
    public static class Setup
    {
        /// <summary>
        /// Automapper: Initialize maps
        /// </summary>
        public static void ConfigureMappers()
        {
            Mapper.Initialize(cfg =>
            {
                // Add profiles for simulations
                cfg.AddProfile<Rfactor2Profile>();
            });
        }

        public static IConfiguration ConfigureApplicationSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            return builder.Build();
        }

        public static DbContextOptions<T> ConfigureDbContextOptions<T>(IConfiguration config)
            where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseSqlite(config.GetConnectionString("DefaultConnection"))
                .Options;
        }

        public static IContainer ConfigureDependencyInjection(IConfiguration config)
        {
            var dbContextOptions = ConfigureDbContextOptions<TelemetryContext>(config);

            var autofacBuilder = new ContainerBuilder();
            autofacBuilder.Register(b => new TelemetryContext(dbContextOptions)).SingleInstance();
            autofacBuilder.RegisterType<ReceiverLogic>();
            autofacBuilder.RegisterType<TrackMapper>();
            autofacBuilder.RegisterType<HubSender>();

            return autofacBuilder.Build();
        }
    }
}
