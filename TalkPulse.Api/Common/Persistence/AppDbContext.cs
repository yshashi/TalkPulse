using Microsoft.EntityFrameworkCore;
using TalkPulse.Api.Common.Domains;

namespace TalkPulse.Api.Common.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<PollOption> PollOptions => Set<PollOption>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<Speaker> Speakers => Set<Speaker>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Speaker>(s =>
        {
            s.HasKey(s => s.Id).HasName("PK_Speakers");
            s.HasIndex(s => s.Email).IsUnique();
            s.Property(s => s.Name).IsRequired().HasMaxLength(100);
            s.Property(s => s.Email).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Session>(s =>
        {
            s.HasKey(s => s.Id).HasName("PK_Sessions");
            s.Property(s => s.Title).HasMaxLength(200);
            s.Property(s => s.Description).HasMaxLength(1000);
            s.HasIndex(s => s.JoinCode).IsUnique();
            s.Property(s => s.Status).HasConversion<string>();

            s.HasOne(s => s.Speaker)
                .WithMany(sp => sp.Sessions)
                .HasForeignKey(s => s.SpeakerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Poll>(p =>
        {
            p.HasKey(p => p.Id).HasName("PK_Polls");
            p.Property(p => p.Question).IsRequired().HasMaxLength(500);

            // PostgreSQL optimistic concurrency via the built-in xmin system column.
            // No migration needed — xmin is maintained automatically by PostgreSQL.
            p.Property(p => p.Version).IsConcurrencyToken();

            p.HasOne(p => p.Session)
                .WithMany(s => s.Polls)
                .HasForeignKey(p => p.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PollOption>(o =>
        {
            o.HasKey(o => o.Id).HasName("PK_PollOptions");
            o.Property(o => o.Text).IsRequired().HasMaxLength(200);

            // Optimistic concurrency via PostgreSQL xmin system column.
            o.Property(o => o.Version).IsConcurrencyToken();

            o.HasOne(o => o.Poll)
                .WithMany(p => p.Options)
                .HasForeignKey(o => o.PollId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Vote>(v =>
        {
            v.HasKey(v => v.Id).HasName("PK_Votes");

            // Prevent the same audience member from voting on the same poll twice.
            v.HasIndex(v => new { v.PollId, v.AudienceFingerprint })
                .IsUnique()
                .HasFilter("\"AudienceFingerprint\" IS NOT NULL");

            v.HasOne(v => v.PollOption)
                .WithMany()
                .HasForeignKey(v => v.PollOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            v.HasOne<Poll>()
                .WithMany()
                .HasForeignKey(v => v.PollId)
                .OnDelete(DeleteBehavior.Cascade);

            v.HasOne(v => v.Session)
                .WithMany()
                .HasForeignKey(v => v.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Feedback>(f =>
        {
            f.HasKey(f => f.Id).HasName("PK_Feedbacks");
            f.Property(f => f.Text).IsRequired().HasMaxLength(1000);
            f.Property(f => f.Rating).IsRequired();

            // Prevent the same audience member from submitting feedback for the same session twice.
            f.HasIndex(f => new { f.SessionId, f.AudienceFingerprint })
                .IsUnique()
                .HasFilter("\"AudienceFingerprint\" IS NOT NULL");

            f.Property(f => f.Type).HasConversion<string>();

            f.HasOne(f => f.Session)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.SessionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}