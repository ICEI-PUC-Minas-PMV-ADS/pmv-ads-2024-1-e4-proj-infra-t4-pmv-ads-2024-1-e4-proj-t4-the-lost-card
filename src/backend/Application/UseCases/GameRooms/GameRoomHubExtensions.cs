using Domain.Extensions.Serialization;
using FluentResults;
using Mediator;

namespace Application.UseCases.GameRooms;

public record GameRoomHubRequestBase : IJsonDerivedTypeBase { }
public record GameRoomHubRequest : GameRoomHubRequestBase, IRequest<Result> { }
public record GameRoomHubRequest<TResponse> : GameRoomHubRequestBase, IRequest<Result<GameRoomHubResponse>>;
public record GameRoomHubResponse : IJsonDerivedTypeBase { }
public interface IGameRoomRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, Result<GameRoomHubResponse>>
    where TRequest : GameRoomHubRequest<TResponse>
    where TResponse : GameRoomHubResponse
{ }

public interface IGameRoomRequestHandler<TRequest> : IRequestHandler<TRequest, Result>
    where TRequest : GameRoomHubRequest
{ }