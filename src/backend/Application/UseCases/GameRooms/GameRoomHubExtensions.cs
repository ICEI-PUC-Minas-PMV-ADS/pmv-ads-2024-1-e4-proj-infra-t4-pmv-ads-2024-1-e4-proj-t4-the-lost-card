using Application.Services;
using Domain.Entities;
using FluentResults;
using Mediator;
using Newtonsoft.Json;

namespace Application.UseCases.GameRooms;

public abstract record GameRoomHubRequestBase : IRequest<Result<GameRoomHubRequestResponse>>, IRequestMetadata
{
    [JsonIgnore]
    public virtual bool RequiresAuthorization => true;

    [JsonIgnore]
    public virtual IRequestMetadata.Metadata? RequestMetadata { get; set; }
    [JsonIgnore]
    public virtual Player? Requester { get; set; }
    [JsonIgnore]
    public virtual GameRoom? CurrentRoom { get; set; }
    [JsonIgnore]
    public GameRoom.RoomGameInfo.PlayerGameInfo? RequesterPlayerInfo => CurrentRoom?.GameInfo?.PlayersInfo.FirstOrDefault(pi => pi.PlayerId == Requester?.Id);
}

public abstract record GameRoomHubRequest<TResponse> : GameRoomHubRequestBase
    where TResponse : GameRoomHubRequestResponse
{ }
public abstract record GameRoomHubRequest : GameRoomHubRequest<GameRoomHubRequestResponse> { }

public record GameRoomHubRequestResponse 
{
#pragma warning disable CA1822 // Mark members as static
    public bool Ack => true;
#pragma warning restore CA1822 // Mark members as static
}
public interface IGameRoomRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<GameRoomHubRequestResponse>>
    where TRequest : GameRoomHubRequest<TResponse>
    where TResponse : GameRoomHubRequestResponse
{ }

public interface IGameRoomRequestHandler<TRequest> : IRequestHandler<TRequest, Result<GameRoomHubRequestResponse>>
    where TRequest : GameRoomHubRequest<GameRoomHubRequestResponse>
{ }