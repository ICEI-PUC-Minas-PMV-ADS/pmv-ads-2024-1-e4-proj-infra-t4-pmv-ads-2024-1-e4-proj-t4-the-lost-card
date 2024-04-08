using Application.UseCases.GameRooms;
using Application.UseCases.GameRooms.Leave;
using FluentResults;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Options;
using Presentation.Services;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation;

public class EventHub : ServerlessHub
{
    private readonly RequestMetadataService requestMetadataService;
    private readonly ISender sender;
    private readonly IOptions<JsonOptions> jsonOptions;

    public EventHub(RequestMetadataService requestMetadataService, ISender sender, IOptions<JsonOptions> jsonOptions)
    {
        this.requestMetadataService = requestMetadataService;
        this.sender = sender;
        this.jsonOptions = jsonOptions;
    }

    [FunctionName("negotiate")]
    public SignalRConnectionInfo Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req)
    {
        var claims = GetClaims(req.Headers["Authorization"]);
        return Negotiate(
            claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            claims
        );
    }


    [FunctionName(nameof(OnServerDispatch))]
    public async Task OnServerDispatch([SignalRTrigger] InvocationContext invocationContext, string rawNotification, CancellationToken cancellationToken)
    {
        requestMetadataService.SetSignalRConnectionInfo(invocationContext, Clients, Groups, UserGroups);
        await requestMetadataService.SetRequestMetadata(cancellationToken);

        var request = JsonSerializer.Deserialize<GameRoomHubRequestBase>(rawNotification, options: jsonOptions.Value.SerializerOptions);
        var response = await sender.Send(request!, cancellationToken);
        if (response is Result<GameRoomHubResponse> typedResponse)
        {
            if (typedResponse.IsSuccess)
            {
                var responseRaw = JsonSerializer.Serialize(typedResponse.Value, options: jsonOptions.Value.SerializerOptions);
                await Clients.Group(requestMetadataService.RequestMetadata!.RoomId.ToString()!).SendAsync("OnClientDispatch", responseRaw, cancellationToken: cancellationToken);
            }
            else
            {
                var responseRaw = JsonSerializer.Serialize(typedResponse.Value, options: jsonOptions.Value.SerializerOptions);
                await Clients.Group(requestMetadataService.RequestMetadata!.RoomId.ToString()!).SendAsync("OnClientDispatch", responseRaw, cancellationToken: cancellationToken);
            }
        }
    }

    [FunctionName(nameof(OnDisconnected))]
    public async Task OnDisconnected([SignalRTrigger] InvocationContext invocationContext, CancellationToken cancellationToken)
    {
        requestMetadataService.SetSignalRConnectionInfo(invocationContext, Clients, Groups, UserGroups);
        await requestMetadataService.SetRequestMetadata(cancellationToken);
        if (requestMetadataService.RequestMetadata?.HubConnectionId is not null)
            await sender.Send(new LeaveGameRoomHubRequest(), cancellationToken);
    }
}
