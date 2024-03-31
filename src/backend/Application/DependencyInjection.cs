using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    private static readonly Type _thisType = typeof(DependencyInjection);
    public static IServiceCollection AddLostCardsApp(this IServiceCollection services)
    {
        services.AddSingleton<ICryptographyService, CryptographyService>();

        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssemblyContaining(_thisType)
        );

        return services;
    }
}
