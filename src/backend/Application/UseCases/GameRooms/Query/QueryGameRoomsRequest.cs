using Application.Contracts.LostCardDatabase;
using Domain.Entities;
using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms.Query;

public class QueryGameRoomsRequest : IRequest<Result<IEnumerable<QueryGameRoomsResponse>>>
{
    private QueryGameRoomsRequest() { }
    public static QueryGameRoomsRequest Value => new();
}

public record QueryGameRoomsResponse(Guid RoomGuid, string RoomName, int CurrentPlayers);

public class QueryGameRoomsRequestHandler : IRequestHandler<QueryGameRoomsRequest, Result<IEnumerable<QueryGameRoomsResponse>>>
{
    private readonly IGameRoomRepository gameRoomRepository;

    public QueryGameRoomsRequestHandler(IGameRoomRepository gameRoomRepository)
    {
        this.gameRoomRepository = gameRoomRepository;
    }

    public async ValueTask<Result<IEnumerable<QueryGameRoomsResponse>>> Handle(QueryGameRoomsRequest request, CancellationToken cancellationToken)
    {
        var lobbyFilter = new GameRoomState[] { GameRoomState.Lobby };
        var openRooms = await gameRoomRepository.Find(lobbyFilter, cancellationToken);
        return openRooms.Select(o => new QueryGameRoomsResponse(o.Id!.Value, o.Name, o.Players.Count)).ToResult();
    }
}
