namespace Application.Services;

public interface IRequestMetadata
{
    public record Info(
        int? RequesterId,
        string? HubConnectionId,
        Guid? RoomId,
        DateTime RecievedAt
    );

    Info RequestInfo { get; set; }
}

public interface IRequestInfoService
{
    IRequestMetadata.Info? RequestInfo { get; }
    void SetRoomGuid(Guid roomGuid);
    Task<IRequestMetadata.Info> SetRequestInfo(CancellationToken cancellationToken = default);
}
