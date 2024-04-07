using Application.UseCases.GameRooms;
using Application.UseCases.GameRooms.Leave;
using FluentResults;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Presentation.Services;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Presentation;

[Authorize]
public partial class GameRoomHub : Hub<GameRoomHub.IGameRoomHubClient>
{
    public interface IGameRoomHubClient
    {
        Task Recieve(string rawNotification);
    }

    private readonly ISender sender;
    private readonly RequestMetadataService requestInfoService;
    private readonly IOptions<JsonOptions> jsonOptions;

    public GameRoomHub(ISender sender, RequestMetadataService requestInfoService, IOptions<JsonOptions> jsonOptions)
    {
        this.sender = sender;
        this.requestInfoService = requestInfoService;
        this.jsonOptions = jsonOptions;
    }

    public override async Task OnConnectedAsync()
    {
        requestInfoService.SetSignalRConnectionInfo(Context);
        await requestInfoService.SetRequestMetadata(Context.ConnectionAborted);
        await base.OnConnectedAsync();
    }

    public async Task Send(string rawNotification)
    {
        requestInfoService.SetSignalRConnectionInfo(Context);
        await requestInfoService.SetRequestMetadata(Context.ConnectionAborted);

        var request = JsonSerializer.Deserialize<GameRoomHubRequestBase>(rawNotification, options: jsonOptions.Value.SerializerOptions);
        var response = await sender.Send(request!, Context.ConnectionAborted);
        if (response is Result<GameRoomHubResponse> typedResponse)
        {
            if (typedResponse.IsSuccess)
            {
                var responseRaw = JsonSerializer.Serialize(typedResponse.Value, options: jsonOptions.Value.SerializerOptions);
                await Clients.Group(requestInfoService.RequestMetadata!.RoomId.ToString()!).Recieve(responseRaw);
            }
            else
            {
                var responseRaw = JsonSerializer.Serialize(typedResponse.Errors, options: jsonOptions.Value.SerializerOptions);
                await Clients.Group(requestInfoService.RequestMetadata!.RoomId.ToString()!).Recieve(responseRaw);
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        requestInfoService.SetSignalRConnectionInfo(Context);
        await requestInfoService.SetRequestMetadata(Context.ConnectionAborted);
        if (requestInfoService.RequestMetadata?.HubConnectionId is not null)
            await sender.Send(new LeaveGameRoomHubRequest());

        await base.OnDisconnectedAsync(exception);
    }
}
