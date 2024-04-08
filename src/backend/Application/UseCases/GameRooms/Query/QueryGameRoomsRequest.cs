using Application.Services;
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
    private readonly IGameRoomService gameRoomService;

    public QueryGameRoomsRequestHandler(IGameRoomService gameRoomService)
    {
        this.gameRoomService = gameRoomService;
    }

    public async ValueTask<Result<IEnumerable<QueryGameRoomsResponse>>> Handle(QueryGameRoomsRequest request, CancellationToken cancellationToken)
    {
        var openRooms = await gameRoomService.GetOpenRooms(cancellationToken);
        return openRooms.Select(o => new QueryGameRoomsResponse(o.Key, o.Value.RoomName, o.Value.CurrentPlayers)).ToResult();
    }
}
