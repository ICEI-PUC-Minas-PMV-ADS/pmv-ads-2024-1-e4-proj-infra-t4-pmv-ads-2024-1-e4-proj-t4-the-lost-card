using Application.Behaviours;
using Application.Services;
using Application.UseCases.GameRooms.ServerTick;
using FluentValidation;
using Mediator;
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

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TurnEndingGameRoomActionBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestAuthBehaviour<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        return services;
    }
}
