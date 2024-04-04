using Application.Behaviours;
using Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    private static readonly Type thisType = typeof(DependencyInjection);
    public static IServiceCollection AddLostCardsApp(this IServiceCollection services)
    {
        services.AddSingleton<ICryptographyService, CryptographyService>();

        services.AddMediatR(cfg => cfg
            .RegisterServicesFromAssemblyContaining(thisType)
        );

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

        services.AddValidatorsFromAssemblyContaining(thisType);

        return services;
    }
}
