using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLostCardsApp(this IServiceCollection services)
    {
        services.AddSingleton<ICryptographyService, CryptographyService>();

        services.AddMediator(opt =>
        {
            opt.Namespace = "Application.Mediator";
            opt.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return services;
    }
}
