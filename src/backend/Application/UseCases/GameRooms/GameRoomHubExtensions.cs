using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms;

public abstract record GameRoomHubRequestBase { }
public abstract record GameRoomHubRequest : GameRoomHubRequestBase, IRequest<Result> { }
public abstract record GameRoomHubRequest<TResponse> : GameRoomHubRequestBase, IRequest<Result<GameRoomHubRequestResponse>>;
public abstract record GameRoomHubRequestResponse { }
public interface IGameRoomRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<GameRoomHubRequestResponse>>
    where TRequest : GameRoomHubRequest<TResponse>
    where TResponse : GameRoomHubRequestResponse
{ }

public interface IGameRoomRequestHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : GameRoomHubRequest
{ }