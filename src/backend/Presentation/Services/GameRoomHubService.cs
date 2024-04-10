using Application.Services;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services;

public class GameRoomHubService : IGameRoomHubService
{
    private readonly IHubContext<GameRoomHub, GameRoomHub.IGameRoomHubClient> hubContext;

    public GameRoomHubService(IHubContext<GameRoomHub, GameRoomHub.IGameRoomHubClient> hubContext)
    {
        this.hubContext = hubContext;
    }

    public Task JoinGroup(string connectionId, string groupId, CancellationToken cancellationToken = default)
    {
        return hubContext.Groups.AddToGroupAsync(connectionId, groupId, cancellationToken);
    }

    public Task LeaveGroup(string connectionId, string groupId, CancellationToken cancellationToken = default)
    {
        return hubContext.Groups.RemoveFromGroupAsync(connectionId, groupId, cancellationToken);
    }
}
