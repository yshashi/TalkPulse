using Microsoft.EntityFrameworkCore;

namespace TalkPulse.Api.Data;

/// <summary>
/// Main application DbContext. Connection string is injected by Aspire via
/// the "talkpulsedb" connection name registered in AppHost.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Add your DbSet<TEntity> properties here, e.g.:
    // public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Fluent API configuration goes here
    }
}
