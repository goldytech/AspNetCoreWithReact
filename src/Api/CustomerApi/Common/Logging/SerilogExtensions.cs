using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace CustomerApi.Common.Logging;

public static class SerilogExtensions
{
    public static void AddLoggingInfrastructure(this IServiceCollection services, WebApplicationBuilder webAppBuilder)
    {
        ArgumentNullException.ThrowIfNull(webAppBuilder, nameof(webAppBuilder));
      
        webAppBuilder.Host.UseSerilog(((context, configuration) =>
        {
            configuration.Enrich.FromLogContext()
                .Enrich.WithProperty("ProductName", context.HostingEnvironment.ApplicationName)
                .Enrich.WithProperty("EnvironmentName", context.HostingEnvironment.EnvironmentName)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .WriteTo.Console(new JsonFormatter());
        }));
}

}