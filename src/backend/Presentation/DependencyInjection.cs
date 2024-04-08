using Application;
using Application.Behaviours.RequestPreprocessor;
using Application.Services;
using Infrastructure;
using Mediator;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Services;
using System.Text;

[assembly: FunctionsStartup(typeof(Presentation.DependencyInjection))]

namespace Presentation;

public class DependencyInjection : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
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
}
