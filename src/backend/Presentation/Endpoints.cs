using Application.UseCases.GameRooms.Query;
using Application.UseCases.PlayerSignIn;
using Application.UseCases.PlayerSignUp;
using Application.UseCases.QueryGameObjects;
using Application.UseCases.SeePlayerInfoRequest;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Presentation.Serialization;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

namespace Presentation;

public class Endpoints
{
    private readonly ISender sender;
    private static readonly JsonSerializerSettings serializerSettings = new() { TypeNameHandling = TypeNameHandling.All };

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

    [FunctionName("QueryGameObjects")]
    public async Task<IActionResult> QueryGameObjects(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "gameobjects/{queryKey}")] HttpRequest req,
        string queryKey,
        CancellationToken cancellationToken
    )
    {
        var responseResult = await sender.Send(new QueryGameObjectsRequest(queryKey), cancellationToken);
        var responseRaw = JsonConvert.SerializeObject(responseResult, serializerSettings);
        return new ContentResult
        {
            Content = responseRaw,
            ContentType = "application/json",
        };
    }

    [FunctionName("SignUp")]
    public async Task<IActionResult> SignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "players")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<PlayerSignUpRequest>(cancellationToken: cancellationToken);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("SignIn")]
    public async Task<IActionResult> SignIn(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "players/sessions")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<PlayerSignInRequest>(cancellationToken: cancellationToken);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("QueryGameRooms")]
    public async Task<IActionResult> QueryGameRooms(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "gamerooms")] HttpRequest req,
        CancellationToken cancellationToken
    )
    {
        var responseResult = await sender.Send(QueryGameRoomsRequest.Value, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }

    [FunctionName("SeePlayerInfo")]
    public async Task<IActionResult> SeePlayerInfo(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "players/{id}")] HttpRequest req,
        Guid id,
        CancellationToken cancellationToken)
    {
        var request = new SeePlayerInfoRequest(id);

        var responseResult = await sender.Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }
}