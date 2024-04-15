using Application.UseCases.GameRooms.Query;
using Application.UseCases.PlayerSignIn;
using Application.UseCases.PlayerSignUp;
using Application.UseCases.SeePlayerInfoRequest;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Presentation.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Presentation;

public class Endpoints
{
    private readonly ISender sender;

    public Endpoints(ISender sender)
    {
        this.sender = sender;
    }

    [FunctionName("HubDebugHelper")]
    public static IActionResult HubDebugHelper([HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req, ExecutionContext context)
    {
        var path = Path.Combine(context.FunctionAppDirectory, "content", "HubDebugHelper.html");
        return new ContentResult
        {
            Content = File.ReadAllText(path),
            ContentType = "text/html",
        };
    }

    [FunctionName("SignUp")]
    public async Task<IActionResult> SignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "players")] HttpRequest req,
        System.Threading.CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<PlayerSignUpRequest>(cancellationToken: cancellationToken);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("SignIn")]
    public async Task<IActionResult> SignIn(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "players/sessions")] HttpRequest req,
        System.Threading.CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<PlayerSignInRequest>(cancellationToken: cancellationToken);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("QueryGameRooms")]
    public async Task<IActionResult> QueryGameRooms(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "gamerooms")] HttpRequest req,
        System.Threading.CancellationToken cancellationToken
    )
    {
        var responseResult = await sender.Send(QueryGameRoomsRequest.Value, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("SeePlayerInfo")]
    public async Task<IActionResult> SeePlayerInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "players/{id}")] HttpRequest req,
        Guid id,
        System.Threading.CancellationToken cancellationToken)
    {
        var request = new SeePlayerInfoRequest(id);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }
}