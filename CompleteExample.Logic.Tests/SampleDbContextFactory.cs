using CompleteExample.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;

namespace CompleteExample.Logic.Tests
{
    public class SampleDbContextFactory : IDisposable
    {
        private bool disposedValue = false; // To detect redundant calls

        public CompleteExampleDBContext CreateContext()
        {
            try
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                var option = new DbContextOptionsBuilder<CompleteExampleDBContext>().UseSqlite(connection).Options;

                var context = new CompleteExampleDBContext(option);

                if (context != null)
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }

                return context;
            }
            catch (Exception ex)
            {

                throw ex;
            }            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
