using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
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
    private IGroupManager? signalRGroups = default;
    public Guid? RoomGuid { get; private set; } = default;

    public IRequestMetadata.Metadata? RequestMetadata { get; private set; }

    public RequestMetadataService(IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.tokenService = tokenService;
    }

    public void SetSignalRConnectionInfo(InvocationContext callerContext, IGroupManager groupManager)
    {
        signalRContext = callerContext;
        signalRGroups = groupManager;
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
}
