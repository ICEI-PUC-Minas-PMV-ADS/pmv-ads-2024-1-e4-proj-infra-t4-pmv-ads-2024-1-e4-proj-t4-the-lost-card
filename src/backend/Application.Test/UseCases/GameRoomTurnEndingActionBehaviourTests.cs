using Application.Contracts.LostCardDatabase;
using Application.Services;
using Application.UseCases.GameRooms.GameActions.ChooseClass;
using Application.UseCases.GameRooms;
using Application.UseCases.PlayerSignIn;
using Domain.Entities;
using Mediator;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FluentResults;

namespace Application.Test.UseCases;

public record TurnEndingGameRoomActionRequest() : GameRoomHubRequest<TurnEndingGameRoomActionRequestResponse>, ITurnEndingGameRoomActionRequest;

public record TurnEndingGameRoomActionRequestResponse() : GameRoomHubRequestResponse;


public class GameRoomTurnEndingActionBehaviourTests
{
    private readonly ISender sender = Substitute.For<ISender>();


    private readonly TurnEndingGameRoomActionBehaviour<TurnEndingGameRoomActionRequest, Result<GameRoomHubRequestResponse>> sut;

    public GameRoomTurnEndingActionBehaviourTests()
    {
        sut = new TurnEndingGameRoomActionBehaviour<TurnEndingGameRoomActionRequest, Result<GameRoomHubRequestResponse>>(sender);
    }

    [Fact]
    public async Task WhenAllActionsAreFinishedAndActionIsSuccess_ServerTickGameRoomIsCalled()
    {
        var request = new TurnEndingGameRoomActionRequest()
        {
            CurrentRoom = new GameRoom
            {
                GameInfo = new GameRoom.RoomGameInfo
                {
                    PlayersInfo = new HashSet<GameRoom.RoomGameInfo.PlayerGameInfo>
                    {
                        new() { ActionsFinished = true }
                    }
                }
            }
        };
        var serverTickGameRoomRequest = new PlayerTurnEndedNotification(request.CurrentRoom);

        var wasDelegateCalled = false;

        ValueTask<Result<GameRoomHubRequestResponse>> nextDelegate(TurnEndingGameRoomActionRequest message, CancellationToken cancellationToken)
        {
            wasDelegateCalled = true;
            return ValueTask.FromResult(new GameRoomHubRequestResponse().ToResult());
        }


        var hanldeResult = await sut.Handle(request, CancellationToken.None, nextDelegate);

        _ = await sender.Received().Send(serverTickGameRoomRequest, Arg.Any<CancellationToken>());

        Assert.True(wasDelegateCalled);
    }
}
