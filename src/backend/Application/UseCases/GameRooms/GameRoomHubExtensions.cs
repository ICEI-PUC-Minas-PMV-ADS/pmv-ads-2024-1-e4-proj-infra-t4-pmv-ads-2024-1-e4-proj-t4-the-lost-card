using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms;

public abstract record GameRoomHubRequestBase { }
public abstract record GameRoomHubRequest : GameRoomHubRequestBase, IRequest<Result> { }
public abstract record GameRoomHubRequest<TResponse> : GameRoomHubRequestBase, IRequest<Result<GameRoomHubResponse>>;
public abstract record GameRoomHubResponse { }
public interface IGameRoomRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<GameRoomHubResponse>>
    where TRequest : GameRoomHubRequest<TResponse>
    where TResponse : GameRoomHubResponse
{ }

public interface IGameRoomRequestHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : GameRoomHubRequest
{ }