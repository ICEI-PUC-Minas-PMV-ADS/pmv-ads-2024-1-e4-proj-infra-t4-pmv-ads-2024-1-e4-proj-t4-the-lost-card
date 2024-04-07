using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Services;

internal class RequestMetadataService : IRequestMetadataService
{
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly TokenService tokenService;
    private InvocationContext? _signalRContext = default;
    public IRequestMetadata.Metadata? RequestMetadata { get; private set; }

    public RequestMetadataService(IHttpContextAccessor httpContextAccessor, TokenService tokenService)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.tokenService = tokenService;
    }

    public void SetSignalRConnectionInfo(InvocationContext? callerContext)
    {
        _signalRContext = callerContext;
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
        int? requesterId = requesterIdClaim?.Value is null ? null : int.Parse(requesterIdClaim.Value);

        var roomGuidClaim = userClaims?.FindFirst(ClaimTypes.GroupSid);

        Guid? roomGuid = default;

        if (Guid.TryParse(roomGuidClaim?.Value, out var parsedGuid))
            roomGuid = parsedGuid;

        RequestMetadata = new IRequestMetadata.Metadata(requesterId, _signalRContext?.ConnectionId, roomGuid, DateTime.Now);

        return Task.FromResult((IRequestMetadata.Metadata?)RequestMetadata);
    }

    public void SetRoomGuid(Guid roomGuid)
    {
        throw new NotImplementedException();
    }

    private string? GetToken()
    {
        if (_signalRContext is not null && _signalRContext.Query.TryGetValue("access_token", out var queryToken))
            return queryToken;

        if (httpContextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault() is { } headerToken)
            return headerToken[7..];

        return null;
    }
}
