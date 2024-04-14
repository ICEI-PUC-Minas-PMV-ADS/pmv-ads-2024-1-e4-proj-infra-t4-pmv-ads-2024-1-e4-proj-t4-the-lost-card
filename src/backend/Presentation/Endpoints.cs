using Application.UseCases.GetPlayerAchievements;
using Application.UseCases.PlayerSignIn;
using Application.UseCases.PlayerSignUp;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Presentation.Serialization;
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
    
    [FunctionName("PlayerAchievments")]
    public static async Task<IActionResult> PlayerAchievments(
        [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "players/achievements")] HttpRequest req,
        System.Threading.CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<GetPlayerAchievementsRequest>(cancellationToken: cancellationToken);

        var responseResult = await req.HttpContext.RequestServices.GetRequiredService<ISender>().Send(request!, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }
}