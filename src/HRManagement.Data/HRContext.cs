using Microsoft.EntityFrameworkCore;

namespace HRManagement.Data
{
    public class HRContext : DbContext
    {
        public HRContext(DbContextOptions<HRContext> options) : base(options)
        {
        }

        // DbSets will be added as we create entities
        // These will be added in subsequent phases

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations will be added here as we create entity configurations
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Add audit tracking before save (optional)
            // Implementation will be added in the next phase

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
