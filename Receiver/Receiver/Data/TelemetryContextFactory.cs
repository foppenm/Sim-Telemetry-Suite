using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Data
{
    public class TelemetryContextFactory : IDesignTimeDbContextFactory<TelemetryContext>
    {
        public TelemetryContext CreateDbContext(string[] args)
        {
            var config = Setup.ConfigureApplicationSettings();
            var options = Setup.ConfigureDbContextOptions<TelemetryContext>(config);
            return new TelemetryContext(options);
        }
    }
}
