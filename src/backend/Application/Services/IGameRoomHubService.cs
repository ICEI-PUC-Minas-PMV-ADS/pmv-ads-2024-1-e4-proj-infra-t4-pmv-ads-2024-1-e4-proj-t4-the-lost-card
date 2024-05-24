using Mediator;

namespace Application.Services;

public interface IGameRoomHubService
{
    void AddDelayed(INotification notification);
    Task Dispatch<TDispatchEvent>(string connectionId, TDispatchEvent dispatchEvent, CancellationToken cancellationToken = default);
    Task Dispatch<TDispatchEvent>(TDispatchEvent dispatchEvent, CancellationToken cancellationToken = default);

    Task JoinGroup(
        string connectionId,
        string groupId,
        CancellationToken cancellationToken = default
    );

    Task LeaveGroup(
        string connectionId,
        string groupId,
        CancellationToken cancellationToken = default
    );
}
