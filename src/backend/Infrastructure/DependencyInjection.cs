using Application.Contracts.LostCardDatabase;
using Infrastructure.LostCardDatabase;
using Infrastructure.LostCardDatabase.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Infrastructure;

public static class DependencyInjection
{

    private static readonly JsonSerializerSettings serializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Objects
    };

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("LostCardsDbConnectionString");

            return new CosmosClient(connectionString, new CosmosClientOptions
            {
                Serializer = new NewtonsoftJsonCosmosSerializer(serializerSettings)
            });
        });

        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IGameRoomRepository, GameRoomRepository>();

        return services;
    }
}
