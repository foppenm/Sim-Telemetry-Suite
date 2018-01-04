using Microsoft.EntityFrameworkCore;
using Receiver.Data;
using Receiver.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Receiver.Tests
{
    public class Arrange
    {
        public static IGenericRepository<TEntity> GetGenericRepository<TEntity>() where TEntity : class, IEntity
        {
            var entityName = typeof(TEntity).Name;
            var options = new DbContextOptionsBuilder<TelemetryContext>()
                .UseSqlite($"Data Source={entityName}.unittest.db")
                .Options;

            TelemetryContext context = new TelemetryContext(options);
            context.Database.EnsureDeleted();
            context.Database.Migrate();

            return new GenericRepository<TEntity>(context);
        }
    }
}
