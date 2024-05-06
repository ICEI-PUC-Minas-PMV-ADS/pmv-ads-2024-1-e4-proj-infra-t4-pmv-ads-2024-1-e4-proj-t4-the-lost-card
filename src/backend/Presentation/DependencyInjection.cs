using Application;
using Application.Behaviours.RequestPreprocessor;
using Application.Services;
using Infrastructure;
using Mediator;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Presentation.Services;
using System.Linq;
using System;
using System.Text;

[assembly: FunctionsStartup(typeof(Presentation.DependencyInjection))]

namespace Presentation;

public class DependencyInjection : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        // Replace ILogger<T> with the one that works fine in all scenarios 
        var loggerServiceDescriptor = builder.Services.FirstOrDefault(s => s.ServiceType == typeof(ILogger<>));
        if (loggerServiceDescriptor != null)
            builder.Services.Remove(loggerServiceDescriptor);

        builder.Services.Add(new ServiceDescriptor(typeof(ILogger<>), typeof(FunctionsLogger<>), ServiceLifetime.Transient));

        builder.Services.AddLostCardsApp();
        builder.Services.AddInfrastructure();

        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreprocessorPipelineBehaviour<,>));

        builder.Services
            .AddOptions<TokenService.Options>()
            .Configure<IConfiguration>((tso, config) => tso.TokenKey = Encoding.UTF8.GetBytes(config.GetValue<string>("JWT_SECRET_KEY")!));

        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<TokenService>() as ITokenService);
        builder.Services.AddScoped<RequestMetadataService>();
        builder.Services.AddScoped(sp => sp.GetRequiredService<RequestMetadataService>() as IRequestMetadataService);
        builder.Services.AddScoped(sp => sp.GetRequiredService<RequestMetadataService>() as IGameRoomHubService);
        builder.Services.AddHttpContextAccessor();
    }

    class FunctionsLogger<T> : ILogger<T>
    {
        readonly ILogger logger;
        public FunctionsLogger(ILoggerFactory factory)
            // See https://github.com/Azure/azure-functions-host/issues/4689#issuecomment-533195224
            => logger = factory.CreateLogger(LogCategories.CreateFunctionUserCategory(typeof(T).FullName));
        public IDisposable BeginScope<TState>(TState state) => logger.BeginScope(state);
        public bool IsEnabled(LogLevel logLevel) => logger.IsEnabled(logLevel);
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => logger.Log(logLevel, eventId, state, exception, formatter);
    }
}
