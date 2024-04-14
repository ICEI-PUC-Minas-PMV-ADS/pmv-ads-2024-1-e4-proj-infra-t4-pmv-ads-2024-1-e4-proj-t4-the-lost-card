using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services;

public class RequestMetadataService : IRequestMetadataService, IGameRoomHubService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly TokenService tokenService;
    private InvocationContext? signalRContext = default;
    private IHubClients? signalRClients = default;
    private IGroupManager? signalRGroups = default;
    private IUserGroupManager? signalRGUserGroups = default;

    public IRequestMetadata.Metadata? RequestMetadata { get; private set; }

    public RequestMetadataService(IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.tokenService = tokenService;
    }

    public void SetSignalRConnectionInfo(InvocationContext callerContext, IHubClients hubClients, IGroupManager groupManager, IUserGroupManager userGroupManager)
    {
        signalRContext = callerContext;
        signalRClients = hubClients;
        signalRGroups = groupManager;
        signalRGUserGroups = userGroupManager;
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

        var roomGuidClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid);
        Guid? roomGuid = default;
        if (roomGuidClaim?.Value is not null && Guid.TryParse(roomGuidClaim.Value, out var parsedGuid))
            roomGuid = parsedGuid;

        RequestMetadata = new IRequestMetadata.Metadata(requesterId, signalRContext?.ConnectionId, roomGuid, DateTime.Now);

        return Task.FromResult((IRequestMetadata.Metadata?)RequestMetadata);
    }

    public void SetRoomGuid(Guid roomGuid)
    {
        if (RequestMetadata is null)
            RequestMetadata = new IRequestMetadata.Metadata(null, null, roomGuid, DateTime.Now);
        else
            RequestMetadata = new IRequestMetadata.Metadata(RequestMetadata.RequesterId, RequestMetadata.HubConnectionId, roomGuid, DateTime.Now);
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
}
