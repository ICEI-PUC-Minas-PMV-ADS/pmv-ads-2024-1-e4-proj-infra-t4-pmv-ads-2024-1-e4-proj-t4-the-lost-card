using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR.Management;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System;
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

        var token = GetToken();

        if (token is null)
            return Task.FromResult(default(IRequestMetadata.Metadata?));

        var userClaims = tokenService.ValidateToken(token);

        var requesterIdClaim = userClaims?.FindFirst(ClaimTypes.NameIdentifier);
        Guid? requesterId = requesterIdClaim?.Value is null ? null : Guid.Parse(requesterIdClaim.Value);

        var roomGuidClaim = userClaims?.FindFirst(ClaimTypes.GroupSid);

        Guid? roomGuid = default;

        if (Guid.TryParse(roomGuidClaim?.Value, out var parsedGuid))
            roomGuid = parsedGuid;

        RequestMetadata = new IRequestMetadata.Metadata(requesterId, signalRContext?.ConnectionId, roomGuid, DateTime.Now);

        return Task.FromResult((IRequestMetadata.Metadata?)RequestMetadata);
    }

    public void SetRoomGuid(Guid roomGuid)
    {
        throw new NotImplementedException();
    }

    private string? GetToken()
    {
        if (signalRContext is not null && signalRContext.Query.TryGetValue("access_token", out var queryToken))
            return queryToken;

        if (httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault() is { } headerToken)
            return headerToken[7..];

        return null;
    }
}
