namespace Crane.Infrastructure.EntityFramework;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

internal static class ServiceExtensions
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Crane") ?? "Data Source=crane.db";

        services.AddDbContext<CraneDbContext>(cfg => cfg.UseSqlite(connectionString));
        services.AddHostedService<EntityFrameworkInitializerHostedService>();
    }

    private sealed class EntityFrameworkInitializerHostedService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CraneDbContext>();

            await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
