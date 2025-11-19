using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HRManagement.Data
{
    /// <summary>
    /// Design-time factory for creating HRContext instances for migrations
    /// </summary>
    public class HRContextFactory : IDesignTimeDbContextFactory<HRContext>
    {
        public HRContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRContext>();

            // Default to SQLite with a local development database
            var connectionString = "Data Source=hrmanagement.db";

            // Allow connection string to be overridden via command line or environment variable
            if (args.Length > 0)
            {
                connectionString = args[0];
            }
            else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HR_CONNECTION_STRING")))
            {
                connectionString = Environment.GetEnvironmentVariable("HR_CONNECTION_STRING");
            }

            optionsBuilder.UseSqlite(connectionString);

            return new HRContext(optionsBuilder.Options);
        }
    }
}
