using Application;
using Application.Behaviours.RequestPreprocessor;
using Infrastructure;
using Mediator;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Presentation.DependencyInjection))]

namespace Presentation;

public class DependencyInjection : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddLostCardsApp();
        builder.Services.AddInfrastructure();

        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreprocessorPipelineBehaviour<,>));
    }
}
