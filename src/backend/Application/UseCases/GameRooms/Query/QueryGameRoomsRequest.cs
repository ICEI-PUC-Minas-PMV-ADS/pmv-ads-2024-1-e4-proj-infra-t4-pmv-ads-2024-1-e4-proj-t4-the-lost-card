using Application.Contracts.LostCardDatabase;
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
    private readonly ILostCardDbUnitOfWork dbUnitOfWork;

    public QueryGameRoomsRequestHandler(ILostCardDbUnitOfWork dbUnitOfWork)
    {
        this.dbUnitOfWork = dbUnitOfWork;
    }

    public async ValueTask<Result<IEnumerable<QueryGameRoomsResponse>>> Handle(QueryGameRoomsRequest request, CancellationToken cancellationToken)
    {
        var openRooms = await dbUnitOfWork.GameRoomRepository.Find(false, cancellationToken);
        return openRooms.Select(o => new QueryGameRoomsResponse(o.Id!.Value, o.Name, o.Players.Count)).ToResult();
    }
}
