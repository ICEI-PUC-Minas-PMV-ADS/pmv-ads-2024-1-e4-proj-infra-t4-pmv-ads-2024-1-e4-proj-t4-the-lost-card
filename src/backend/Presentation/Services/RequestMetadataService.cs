using Application.Services;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services;
public class RequestMetadataService : IRequestMetadataService, IGameRoomHubService
{
    private static readonly JsonSerializerSettings serializerSettings = new() { TypeNameHandling = TypeNameHandling.Objects };

    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly TokenService tokenService;
    private InvocationContext? signalRContext = default;
    private IGroupManager? signalRGroups = default;
    private IHubClients? signalRClients = default;
    public Guid? RoomGuid { get; private set; } = default;
    public HashSet<INotification> DelayedNotifications { get; private init; } = new HashSet<INotification>();
    public IRequestMetadata.Metadata? RequestMetadata { get; private set; }
    public bool IsHubRequest { get; private set; } = false;

    public RequestMetadataService(IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.tokenService = tokenService;
    }

    public void SetSignalRConnectionInfo(InvocationContext callerContext, IGroupManager groupManager, IHubClients hubClients)
    {
        signalRContext = callerContext;
        signalRGroups = groupManager;
        signalRClients = hubClients;

        IsHubRequest = true;
    }

    public Task JoinGroup(string connectionId, string groupId, CancellationToken cancellationToken = default)
    {
        return signalRGroups!.AddToGroupAsync(connectionId, groupId, cancellationToken);
    }

    public Task LeaveGroup(string connectionId, string groupId, CancellationToken cancellationToken = default)
    {
        return signalRGroups!.RemoveFromGroupAsync(connectionId, groupId, cancellationToken);
    }

    public Task<IRequestMetadata.Metadata?> SetRequestMetadata(CancellationToken cancellationToken = default)
    {
        if (RequestMetadata is not null)
            return Task.FromResult((IRequestMetadata.Metadata?)RequestMetadata);

        var claims = ReadClaims();

        if (claims is null || !claims.Any())
            return Task.FromResult(default(IRequestMetadata.Metadata?));

        var requesterIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        Guid? requesterId = default;
        if (requesterIdClaim?.Value is not null && Guid.TryParse(requesterIdClaim.Value, out var parsedRequesterId))
            requesterId = parsedRequesterId;

        RequestMetadata = new IRequestMetadata.Metadata(requesterId, signalRContext?.ConnectionId, DateTime.Now);

        return Task.FromResult((IRequestMetadata.Metadata?)RequestMetadata);
    }

    public IEnumerable<Claim>? ReadClaims()
    {
        if (signalRContext is not null)
            return signalRContext.Claims.Select(kvp => new Claim(kvp.Key, kvp.Value));

        if (httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault() is { } headerToken)
        {
            var claimsPrincipal = tokenService.ValidateToken(headerToken[7..]);
            return claimsPrincipal?.Claims;
        }

        return null;
    }

    public void SetRoomGuid(Guid roomGuid)
    {
        RoomGuid = roomGuid;
    }

    public Task Dispatch<TDispatchEvent>(TDispatchEvent dispatchEvent, CancellationToken cancellationToken = default)
    {
        var responseRaw = JsonConvert.SerializeObject(dispatchEvent, serializerSettings);
        return signalRClients!.Group(RoomGuid!.Value.ToString()!).SendAsync("OnClientDispatch", responseRaw, cancellationToken: cancellationToken);
    }

    public Task Dispatch<TDispatchEvent>(string connectionId, TDispatchEvent dispatchEvent, CancellationToken cancellationToken = default)
    {
        var responseRaw = JsonConvert.SerializeObject(dispatchEvent, serializerSettings);
        return signalRClients!.Client(connectionId).SendAsync("OnClientDispatch", responseRaw, cancellationToken: cancellationToken);
    }

    public void AddDelayed(INotification notification)
    {
        DelayedNotifications.Add(notification);
    }
}
