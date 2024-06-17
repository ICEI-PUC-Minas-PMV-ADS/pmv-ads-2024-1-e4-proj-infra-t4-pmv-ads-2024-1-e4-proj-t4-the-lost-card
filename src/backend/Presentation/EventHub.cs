using Application.UseCases.GameRooms;
using Application.UseCases.GameRooms.LobbyActions;
using Mediator;
using Microsoft.AspNetCore.Http;
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
    private readonly IMediator mediator;

    private static readonly JsonSerializerSettings serializerSettings = new() { TypeNameHandling = TypeNameHandling.All };

    public EventHub(RequestMetadataService requestMetadataService, IMediator mediator, TokenService tokenService)
    {
        this.requestMetadataService = requestMetadataService;
        this.mediator = mediator;
        this.tokenService = tokenService;
    }


#pragma warning disable IDE0060 // Remove unused parameter
    [FunctionName("negotiate")]
    public SignalRConnectionInfo Negotiate([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req)
    {
#pragma warning restore IDE0060 // Remove unused parameter

        var claims = requestMetadataService.ReadClaims();
        return Negotiate(
            claims!.First(c => c.Type == ClaimTypes.NameIdentifier).Value,
            claims!.ToList()
        );
    }


    [FunctionName(nameof(OnServerDispatch))]
    public async Task OnServerDispatch([SignalRTrigger] InvocationContext invocationContext, string rawNotification, CancellationToken cancellationToken)
    {
        requestMetadataService.SetSignalRConnectionInfo(invocationContext, Groups, Clients);
        await requestMetadataService.SetRequestMetadata(cancellationToken);

        var request = JsonConvert.DeserializeObject<GameRoomHubRequestBase>(rawNotification, serializerSettings);
        var responseResult = await mediator.Send(request!, cancellationToken);

        if (responseResult.IsSuccess)
            await requestMetadataService.Dispatch(responseResult.Value, cancellationToken);
        else
        {
            // TODO: Adicionar tratamento de erro
            if (responseResult.Errors.FirstOrDefault(e => e is GameRoomHubRequestErrorBase) is GameRoomHubRequestErrorBase error)
                await requestMetadataService.Dispatch(error, cancellationToken);
        }

        while (requestMetadataService.DelayedNotifications.Count > 0)
        {
            var delayedNotification = requestMetadataService.DelayedNotifications.First();
            await mediator.Publish(delayedNotification, cancellationToken);
            requestMetadataService.DelayedNotifications.Remove(delayedNotification);
        }
    }

    [FunctionName(nameof(OnDisconnected))]
    public async Task OnDisconnected([SignalRTrigger] InvocationContext invocationContext, CancellationToken cancellationToken)
    {
        requestMetadataService.SetSignalRConnectionInfo(invocationContext, Groups, Clients);
        await requestMetadataService.SetRequestMetadata(cancellationToken);
        if (requestMetadataService.RequestMetadata?.HubConnectionId is not null)
            await mediator.Send(new LeaveGameRoomHubRequest(), cancellationToken);
    }
}
