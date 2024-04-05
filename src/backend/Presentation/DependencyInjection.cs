using Application;
using Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Presentation.DependencyInjection))]

namespace Presentation;

public class DependencyInjection : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddLostCardsApp();
        builder.Services.AddInfrastructure();
    }
}
