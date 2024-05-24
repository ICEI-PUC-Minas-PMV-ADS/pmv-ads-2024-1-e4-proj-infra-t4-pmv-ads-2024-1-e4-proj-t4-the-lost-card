using Application.UseCases.GameRooms;
using Application.UseCases.GameRooms.Leave;
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
            var error = responseResult.Errors.FirstOrDefault(e => e is GameRoomHubRequestErrorBase) as GameRoomHubRequestErrorBase;
            if(error != null)
                await requestMetadataService.Dispatch(error, cancellationToken);
        }

        // TODO: Adicionar tratamento de erro

        foreach (var delayedNotification in requestMetadataService.DelayedNotifications)
            await mediator.Publish(delayedNotification, cancellationToken);
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
