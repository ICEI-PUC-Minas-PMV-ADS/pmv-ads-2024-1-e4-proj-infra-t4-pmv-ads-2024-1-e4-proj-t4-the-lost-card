using Application;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Presentation.DependencyInjection))]

namespace Presentation;

public class DependencyInjection : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddLostCardsApp();
    }
}
