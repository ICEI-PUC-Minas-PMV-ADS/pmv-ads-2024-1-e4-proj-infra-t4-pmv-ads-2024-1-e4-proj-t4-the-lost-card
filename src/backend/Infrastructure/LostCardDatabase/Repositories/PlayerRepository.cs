using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.LostCardDatabase.Repositories;

public class PlayerRepository : IPlayerRepository
{
    private readonly CosmosClient cosmosClient;
    private Database? lostCardDatabase;
    private Container? lostCardContainer;

    public PlayerRepository(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
    }

    private async Task<Container> GetContainer(CancellationToken cancellationToken = default)
    {
        lostCardDatabase ??= (await cosmosClient.CreateDatabaseIfNotExistsAsync("LostCardDb", cancellationToken: cancellationToken)).Database;
        return lostCardContainer ??= (await lostCardDatabase.CreateContainerIfNotExistsAsync("LostCardDbContext", "/PartitionKey", throughput: 400, cancellationToken: cancellationToken)).Container;
    }

    public async Task Create(Player player, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        player.Id = Guid.NewGuid();
        await container.CreateItemAsync(player, new PartitionKey(player.PartitionKey), cancellationToken: cancellationToken);
    }

    public async ValueTask<Player?> Find(Guid id, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        return await container.ReadItemAsync<Player>(id.ToString(), new PartitionKey(id.ToString()), cancellationToken: cancellationToken);
    }

    public async Task<Player?> Find(string email, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);

        var query = new QueryDefinition("SELECT * FROM Players p WHERE p.Email = @email").WithParameter("@email", email);

        FeedResponse<Player>? response = null;

        using (var resultSetIterator = container.GetItemQueryIterator<Player>(query))
            while (resultSetIterator.HasMoreResults)
                response = await resultSetIterator.ReadNextAsync(cancellationToken);

        return response?.FirstOrDefault();
    }

    public async Task Update(Player player, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        _ = await container.UpsertItemAsync(player, new PartitionKey(player.PartitionKey), cancellationToken: cancellationToken);
    }
}
