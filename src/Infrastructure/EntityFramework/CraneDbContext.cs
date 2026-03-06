namespace Crane.Infrastructure.EntityFramework;

using global::MassTransit;
using Microsoft.EntityFrameworkCore;

internal sealed class CraneDbContext : DbContext
{
    public CraneDbContext(DbContextOptions<CraneDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}