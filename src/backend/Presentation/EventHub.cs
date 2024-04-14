using Application.UseCases.GameRooms;
using Application.UseCases.GameRooms.Leave;
using FluentResults;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using Presentation.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation;

public class EventHub : ServerlessHub
{
    private readonly RequestMetadataService requestMetadataService;
    private readonly TokenService tokenService;
    private readonly ISender sender;

    private readonly JsonSerializerSettings serializerSettings = new() { TypeNameHandling = TypeNameHandling.All };

    public EventHub(RequestMetadataService requestMetadataService, ISender sender, TokenService tokenService)
    {
        this.requestMetadataService = requestMetadataService;
        this.sender = sender;
        this.tokenService = tokenService;
    }

    [FunctionName("negotiate")]
    public SignalRConnectionInfo Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req)
    {
        var claims = requestMetadataService.ReadClaims();
        return Negotiate(
            claims!.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            claims!.ToList()
        );
    }


    [FunctionName(nameof(OnServerDispatch))]
    public async Task OnServerDispatch([SignalRTrigger] InvocationContext invocationContext, string rawNotification, CancellationToken cancellationToken)
    {
        requestMetadataService.SetSignalRConnectionInfo(invocationContext, Clients, Groups, UserGroups);
        await requestMetadataService.SetRequestMetadata(cancellationToken);

        var request = JsonConvert.DeserializeObject<GameRoomHubRequestBase>(rawNotification, serializerSettings);
        var response = await sender.Send(request!, cancellationToken);
        if (response is Result<GameRoomHubResponse> typedResponse)
        {
            if (typedResponse.IsSuccess)
            {
                var responseRaw = JsonConvert.SerializeObject(typedResponse.Value, serializerSettings);
                await Clients.Group(requestMetadataService.RequestMetadata!.RoomId.ToString()!).SendAsync("OnClientDispatch", responseRaw, cancellationToken: cancellationToken);
            }
            else
            {
                var responseRaw = JsonConvert.SerializeObject(typedResponse.Value, serializerSettings);
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
