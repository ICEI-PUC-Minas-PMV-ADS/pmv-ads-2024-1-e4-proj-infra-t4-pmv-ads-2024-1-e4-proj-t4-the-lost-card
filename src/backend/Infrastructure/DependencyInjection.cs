using Application.Contracts.LostCardDatabase;
using Infrastructure.LostCardDatabase;
using Infrastructure.LostCardDatabase.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<LostCardDbContext>((sp, cfg) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            if (configuration.GetSection("useSqlite").Exists())
            {
                var connectionStringBuilder = new SqliteConnectionStringBuilder
                {
                    DataSource = "lostcards.db",
                    Cache = SqliteCacheMode.Shared
                };
                var sqliteConnection = new SqliteConnection(connectionStringBuilder.ToString());
                sqliteConnection.Open();
                cfg.UseSqlite(sqliteConnection);
            }
            else
            {
                cfg.UseCosmos(configuration.GetConnectionString("LostCardsDbConnectionString")!, "LostCardDb");
            }
        });

        services.AddScoped(sp => sp.GetRequiredService<LostCardDbContext>() as ILostCardDbUnitOfWork);
        services.AddScoped<IPlayerRepository, PlayerRepository>();
        services.AddScoped<IGameRoomRepository, GameRoomRepository>();

        return services;
    }

}
