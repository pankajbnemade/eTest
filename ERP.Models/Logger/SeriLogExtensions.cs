using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace ERP.Models.Logger
{
    public static class SeriLogExtensions
    {
        public static void ConfigureSeriLog(string projectName)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Information()
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
                .WriteTo.Logger(config => config
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.File(
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        path: $"Logs/{projectName}.txt",
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {MachineName} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                        rollingInterval: RollingInterval.Day,
                        retainedFileCountLimit: null))
                .CreateLogger();
        }

        public static void AddSeriLog(this ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new SeriLogProvider());
        }

        public static IApplicationBuilder UseSeriLogMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SeriLogMiddleware>();
        }

        public static IApplicationBuilder UseContextAccessor(this IApplicationBuilder builder)
        {
            ContextAccessor.Configure(
                builder.ApplicationServices.GetRequiredService<IHttpContextAccessor>()
            );

            return builder;
        }
    }
}
