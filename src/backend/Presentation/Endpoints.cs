using Application.UseCases.PlayerSignUp;
using Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Serialization;
using System.Threading.Tasks;

namespace Presentation;

public static class Endpoints
{
    [FunctionName("players")]
    public static async Task<IActionResult> SignUp(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequest req,
        System.Threading.CancellationToken cancellationToken)
    {
        var request = await req.ReadFromJsonAsync<PlayerSignUpRequest>(cancellationToken: cancellationToken);
        
        var responseResult = await req.HttpContext.RequestServices.GetService<ISender>().Send(request, cancellationToken);

        return HttpSerialization.Serialize(responseResult);
    }
}