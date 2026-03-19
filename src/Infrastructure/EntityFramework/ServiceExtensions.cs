namespace Crane.Infrastructure.EntityFramework;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

internal static class ServiceExtensions
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Crane") ?? "User Id=Crane;Password=Crane;Data Source=localhost:1521/FREEPDB1";

        services.AddDbContext<CraneDbContext>(cfg => cfg
            .UseOracle(connectionString, o => o
                .MaxBatchSize(100)
                .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        services.AddHostedService<EntityFrameworkInitializerHostedService>();
    }

    private sealed class EntityFrameworkInitializerHostedService(IServiceProvider serviceProvider) : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CraneDbContext>();

            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
