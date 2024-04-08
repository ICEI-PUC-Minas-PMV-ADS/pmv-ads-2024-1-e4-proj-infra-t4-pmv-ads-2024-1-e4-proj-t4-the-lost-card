using Infrastructure.LostCardDatabase;
using Mediator;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Behaviours.RequestPreprocessor;

public class RequestPreprocessorPipelineBehaviour<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse> where TMessage : notnull, IMessage
{
    private readonly LostCardDbContext lostCardDbContext;

    public RequestPreprocessorPipelineBehaviour(LostCardDbContext lostCardDbContext)
    {
        this.lostCardDbContext = lostCardDbContext;
    }

    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        await lostCardDbContext.Database.EnsureCreatedAsync(cancellationToken);
        return await next(message, cancellationToken);
    }
}
