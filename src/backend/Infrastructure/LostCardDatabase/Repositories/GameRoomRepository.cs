using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Infrastructure.LostCardDatabase.Repositories;

internal class GameRoomRepository : IGameRoomRepository
{
    private readonly CosmosClient cosmosClient;
    private Database? lostCardDatabase;
    private Container? lostCardContainer;

    public GameRoomRepository(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
    }

    private async Task<Container> GetContainer(CancellationToken cancellationToken = default)
    {
        lostCardDatabase ??= (await cosmosClient.CreateDatabaseIfNotExistsAsync("LostCardDb", cancellationToken: cancellationToken)).Database;
        return lostCardContainer ??= (await lostCardDatabase.CreateContainerIfNotExistsAsync("LostCardDbContext", "/PartitionKey", throughput: 400, cancellationToken: cancellationToken)).Container;
    }

    public async Task Create(GameRoom gameRoom, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        gameRoom.Id = Guid.NewGuid();
        await container.CreateItemAsync(gameRoom, new PartitionKey(gameRoom.PartitionKey), cancellationToken: cancellationToken);
    }

    public async ValueTask<GameRoom?> Find(Guid id, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        var query = new QueryDefinition("SELECT * FROM GameRooms gr WHERE gr.id = @id").WithParameter("@id", id.ToString());

        FeedResponse<GameRoom>? response = null;

        using (var resultSetIterator = container.GetItemQueryIterator<GameRoom>(query))
            while (resultSetIterator.HasMoreResults)
                response = await resultSetIterator.ReadNextAsync(cancellationToken);

        return response?.FirstOrDefault();
    }

    public async Task<IEnumerable<GameRoom>> Find(IEnumerable<GameRoomState>? semaphoreStatesFilter = default, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        var query = new QueryDefinition("SELECT * FROM GameRooms gr WHERE gr.State IN @statesFilter")
            .WithParameter("@statesFilter", (semaphoreStatesFilter ?? Array.Empty<GameRoomState>()).Select(s => (int)s).ToArray());

        var gameRooms = new HashSet<IEnumerable<GameRoom>>();

        using (var resultSetIterator = container.GetItemQueryIterator<GameRoom>(query))
            while (resultSetIterator.HasMoreResults)
                gameRooms.Add(await resultSetIterator.ReadNextAsync(cancellationToken));

        return gameRooms.SelectMany(gmrs => gmrs);
    }

    public async Task Remove(GameRoom gameRoom, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        _ = await container.DeleteItemAsync<GameRoom>(gameRoom.Id!.Value.ToString(), new PartitionKey(gameRoom.PartitionKey), cancellationToken: cancellationToken);
    }

    public async Task Update(GameRoom gameRoom, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(cancellationToken);
        _ = await container.UpsertItemAsync(gameRoom, new PartitionKey(gameRoom.PartitionKey), cancellationToken: cancellationToken);
    }
}
