using Application.UseCases.PlayerSignIn;
using Application.UseCases.PlayerSignUp;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Presentation.Serialization;
using System.Threading.Tasks;

namespace Presentation;

public class Endpoints
{
    private readonly ISender sender;

    public Endpoints(ISender sender)
    {
        this.sender = sender;
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
}