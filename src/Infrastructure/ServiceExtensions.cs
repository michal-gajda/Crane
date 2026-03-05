namespace Crane.Infrastructure;

using System.Reflection;
using Crane.Infrastructure.EntityFramework;
using Crane.Infrastructure.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

        services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

        services.AddEntityFramework(configuration);
        services.AddMassTransit(configuration);

        services.AddSingleton(TimeProvider.System);
    }
}
