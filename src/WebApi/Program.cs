namespace Crane.WebApi;

using Crane.Application;
using Crane.Infrastructure;
using MassTransit.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = AppContext.BaseDirectory,
        });

        var serviceSection = builder.Configuration.GetSection("OpenTelemetry:Service");
        var serviceName = serviceSection["Name"] ?? builder.Environment.ApplicationName;
        var serviceNamespace = serviceSection["Namespace"] ?? "default";
        var serviceVersion = serviceSection["Version"] ?? "1.0.0";

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
            .AddAttributes(
            [
                new("service.namespace", serviceNamespace),
                new("service.instance.id", Environment.MachineName),
            ]);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Logging.AddOpenTelemetry(options => options.SetResourceBuilder(resourceBuilder).AddOtlpExporter());

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing => tracing
                .SetResourceBuilder(resourceBuilder)
                .SetSampler(new AlwaysOnSampler())
                .AddAspNetCoreInstrumentation(opt => opt.RecordException = true)
                .AddHttpClientInstrumentation(opt => opt.RecordException = true)
                .AddSource(DiagnosticHeaders.DefaultListenerName)
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .SetResourceBuilder(resourceBuilder)
                .AddProcessInstrumentation()
                .AddRuntimeInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter())
                ;

        builder.Services.AddHealthChecks();

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        app.UseHealthChecks("/healthz");

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.MapControllers();

        app.UseAuthorization();

        app.MapFallbackToFile("index.html");

        await app.RunAsync();
    }
}
