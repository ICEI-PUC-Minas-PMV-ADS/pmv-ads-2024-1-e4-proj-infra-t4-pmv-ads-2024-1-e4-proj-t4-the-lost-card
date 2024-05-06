using Domain.Entities;

namespace Application.Services;

public interface IRequestMetadata
{
    public record Metadata(
        Guid? RequesterId,
        string? HubConnectionId,
        Guid? RoomId,
        DateTime RecievedAt
    );

    Player? Requester { get; set; }
    GameRoom? CurrentRoom { get; set; }
    Metadata? RequestMetadata { get; set; }
    bool RequiresAuthorization { get; }
}

public interface IRequestMetadataService
{
    IRequestMetadata.Metadata? RequestMetadata { get; }
    void SetRoomGuid(Guid roomGuid);
    Task<IRequestMetadata.Metadata?> SetRequestMetadata(CancellationToken cancellationToken = default);
}
